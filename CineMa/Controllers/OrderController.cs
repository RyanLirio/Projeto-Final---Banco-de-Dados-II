using Cine_Ma.Models;
using Cine_Ma.Repository;
using Cine_Ma.ViewModels.Orders;
using CineMa.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Cine_Ma.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductOrderRepository _productOrderRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ICinemaRoomRepository _roomRepository;
        private readonly IChairRepository _chairRepository;

        public OrderController(
            IOrderRepository orderRepository,
            ISessionRepository sessionRepository,
            ITicketRepository ticketRepository,
            IProductRepository productRepository,
            IProductOrderRepository productOrderRepository,
            IClientRepository clientRepository,
            ICinemaRoomRepository roomRepository,
            IChairRepository chairRepository)
        {
            _orderRepository = orderRepository;
            _sessionRepository = sessionRepository;
            _ticketRepository = ticketRepository;
            _productRepository = productRepository;
            _productOrderRepository = productOrderRepository;
            _clientRepository = clientRepository;
            _roomRepository = roomRepository;
            _chairRepository = chairRepository;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAdmin()
        {
            if (!AdminHelper.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

            var orders = await _orderRepository.GetAll();
            return View("~/Views/Admin/Order/Index.cshtml", orders);
        }

        [HttpGet]
        public async Task<IActionResult> Buying(int sessionId)
        {
            int? userId = HttpContext.Session.GetInt32("UsuarioId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var session = await _sessionRepository.GetById(sessionId);
            if (session == null)
                return NotFound();

            var room = session.CinemaRoom;
            if (room == null)
                return BadRequest("Sessão sem sala vinculada.");

            var cinema = room.Cinema;
            if (cinema == null)
                return BadRequest("Sala sem cinema vinculado.");

            var chairs = await _chairRepository.GetByRoom(room.Id);
            var ticketsSession = await _ticketRepository.GetBySessionId(session.Id);

            var occupiedSeats = ticketsSession
                .Select(t => new { t.Row, t.Column, t.RoomId })
                .ToHashSet();

            var vm = new OrderCreateViewModel
            {
                ClientId = userId.Value,
                SessionId = session.Id,
                CinemaId = cinema.Id,
                RoomId = room.Id,
                MovieTitle = session.Movie!.Title!,
                MovieAge = session.Movie!.MinimumAge!,
                MovieDescription = session.Movie!.Description!,
                MovieImage = session.Movie!.ImageUrl!,
                MovieDuration = session.Movie!.Duration!,
                SessionHour = session.SessionHour,
                RoomDescription =
                    $"{room.RoomNumber} - {cinema.Name}, {cinema.Address!.City}/{cinema.Address.State}",
                BaseTicketPrice = session.TicketPrice,

                Clients = await _clientRepository.GetAll(),
                Products = (await _productRepository.GetAll())
                    .Select(p => new ProductSelectionViewModel
                    {
                        ProductId = p.Id,
                        Name = p.Name!,
                        UnitPrice = p.Price,
                        Discount = p.Discount,
                        Quantity = 0
                    }).ToList(),

                Seats = chairs.Select(c =>
                {
                    bool isOccupied = occupiedSeats.Any(o =>
                        o.RoomId == room.Id &&
                        o.Column == c.Column &&
                        o.Row == c.Row);

                    return new SeatSelectionViewModel
                    {
                        RoomId = room.Id,
                        Column = c.Column,
                        Row = c.Row!,
                        IsVip = c.IsVip,
                        IsOccupied = isOccupied,
                        Selected = false,
                        HalfPrice = false,
                        CalculatedPrice = session.TicketPrice
                    };
                }).OrderBy(s => s.Row).ThenBy(s => s.Column).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Buying(OrderCreateViewModel vm)
        {
            int? userId = HttpContext.Session.GetInt32("UsuarioId");
            if (userId == null)
                return BadRequest("Usuário não autenticado.");

            vm.ClientId = userId.Value;

            if (!ModelState.IsValid)
            {
                await RecarregarListas(vm);
                return View(vm); // usa a view Buying
            }

            var session = await _sessionRepository.GetById(vm.SessionId);
            if (session == null)
                return NotFound();

            var room = await _roomRepository.GetById(vm.RoomId);
            if (room == null)
                return NotFound();

            await RecarregarListas(vm);

            var chairs = await _chairRepository.GetByRoom(vm.RoomId);
            var ticketsSession = await _ticketRepository.GetBySessionId(vm.SessionId);

            var occupiedSeats = ticketsSession
                .Select(t => new { t.Row, t.Column, t.RoomId })
                .ToHashSet();

            var validSeats = new List<SeatSelectionViewModel>();

            foreach (var seatVm in vm.Seats)
            {
                var chair = chairs.FirstOrDefault(c =>
                    c.RoomId == vm.RoomId &&
                    c.Column == seatVm.Column &&
                    c.Row == seatVm.Row);

                if (chair == null)
                    continue;

                bool isOccupied = occupiedSeats.Any(o =>
                    o.RoomId == vm.RoomId &&
                    o.Column == seatVm.Column &&
                    o.Row == seatVm.Row);

                validSeats.Add(new SeatSelectionViewModel
                {
                    RoomId = vm.RoomId,
                    Column = seatVm.Column,
                    Row = seatVm.Row,
                    IsVip = chair.IsVip,
                    IsOccupied = isOccupied,
                    Selected = seatVm.Selected,
                    HalfPrice = seatVm.HalfPrice
                });
            }

            var selectedSeats = validSeats
                .Where(s => s.Selected && !s.IsOccupied)
                .ToList();

            if (!selectedSeats.Any())
            {
                ModelState.AddModelError("", "Selecione pelo menos uma poltrona livre.");
                vm.Seats = validSeats;
                return View(vm);
            }

            int ticketsTotal = 0;

            foreach (var seat in selectedSeats)
            {
                int price = session.TicketPrice;

                if (seat.IsVip)
                    price = (int)(price * 1.15);

                if (seat.HalfPrice)
                    price /= 2;

                seat.CalculatedPrice = price;
                ticketsTotal += price;
            }

            var allProducts = await _productRepository.GetAll();
            var productsDict = allProducts.ToDictionary(p => p.Id, p => p);

            int productsTotal = vm.Products
                .Where(p => p.Quantity > 0 && productsDict.ContainsKey(p.ProductId))
                .Sum(pr =>
                {
                    var prod = productsDict[pr.ProductId];
                    int finalUnit = Math.Max(prod.Price - ((prod.Price * prod.Discount) / 100), 0);
                    return finalUnit * pr.Quantity;
                });

            int grandTotal = ticketsTotal + productsTotal;

            var order = new Order
            {
                CinemaId = vm.CinemaId,
                DtSale = DateOnly.FromDateTime(DateTime.Today),
                TotalPrice = grandTotal,
                PaymentType = vm.PaymentType,
                ClientId = vm.ClientId
            };

            await _orderRepository.Create(order);

            foreach (var seat in selectedSeats)
            {
                var ticket = new Ticket
                {
                    SessionId = vm.SessionId,
                    Column = seat.Column,
                    Row = seat.Row,
                    RoomId = vm.RoomId,
                    OrderId = order.Id,
                    Price = seat.CalculatedPrice,
                    HalfPrice = seat.HalfPrice
                };

                await _ticketRepository.Create(ticket);
            }

            foreach (var pr in vm.Products.Where(p => p.Quantity > 0))
            {
                var prod = productsDict[pr.ProductId];
                int price = Math.Max(prod.Price - ((prod.Price * prod.Discount) / 100), 0);

                var po = new ProductOrder
                {
                    OrderId = order.Id,
                    ProductId = prod.Id,
                    QuantitySale = pr.Quantity,
                    OrderPrice = price * pr.Quantity
                };

                await _productOrderRepository.Create(po);
            }

            return RedirectToAction("Success", new { orderId = order.Id });
        }

        [HttpGet]
        public IActionResult Success(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View("~/Views/Order/Success.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> Create(int sessionId)
        {
            if (!AdminHelper.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

            var session = await _sessionRepository.GetById(sessionId);
            if (session == null)
                return NotFound();


            var room = session.CinemaRoom;
            if (room == null)
                return BadRequest("Sessão sem sala vinculada.");

            var cinema = room.Cinema;
            if (cinema == null)
                return BadRequest("Sala sem cinema vinculado.");

            var chairs = await _chairRepository.GetByRoom(room.Id);
            var ticketsSession = await _ticketRepository.GetBySessionId(session.Id);

            var occupiedSeats = ticketsSession
                .Select(t => new { t.Row, t.Column, t.RoomId })
                .ToHashSet();

            var vm = new OrderCreateViewModel
            {
                SessionId = session.Id,
                CinemaId = cinema.Id,
                RoomId = room.Id,
                MovieTitle = session.Movie!.Title!,
                MovieAge = session.Movie!.MinimumAge!,
                MovieDescription = session.Movie!.Description!,
                MovieImage = session.Movie!.ImageUrl!,
                MovieDuration = session.Movie!.Duration!,
                SessionHour = session.SessionHour,
                RoomDescription =
                    $"{room.RoomNumber} - {cinema.Name}, {cinema.Address!.City}/{cinema.Address.State}",
                BaseTicketPrice = session.TicketPrice,

                Clients = await _clientRepository.GetAll(),
                Products = (await _productRepository.GetAll())
                    .Select(p => new ProductSelectionViewModel
                    {
                        ProductId = p.Id,
                        Name = p.Name!,
                        UnitPrice = p.Price,
                        Discount = p.Discount,
                        Quantity = 0
                    }).ToList(),

                Seats = chairs.Select(c =>
                {
                    bool isOccupied = occupiedSeats.Any(o =>
                        o.RoomId == room.Id &&
                        o.Column == c.Column &&
                        o.Row == c.Row);

                    return new SeatSelectionViewModel
                    {
                        RoomId = room.Id,
                        Column = c.Column,
                        Row = c.Row!,
                        IsVip = c.IsVip,
                        IsOccupied = isOccupied,
                        Selected = false,
                        HalfPrice = false,
                        CalculatedPrice = session.TicketPrice
                    };
                }).OrderBy(s => s.Row).ThenBy(s => s.Column).ToList()
            };

            return View("~/Views/Admin/Order/Create.cshtml", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateViewModel vm)
        {
            if (!AdminHelper.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                await RecarregarListas(vm);
                return View("~/Views/Admin/Order/Create.cshtml", vm);
            }

            var session = await _sessionRepository.GetById(vm.SessionId);
            if (session == null)
                return NotFound();

            var room = await _roomRepository.GetById(vm.RoomId);
            if (room == null)
                return NotFound();

            var cinema = room.Cinema;

            vm.MovieTitle = session.Movie?.Title ?? "";
            vm.SessionHour = session.SessionHour;
            vm.RoomDescription =
                $"{room.RoomNumber} - {cinema?.Name}, {cinema?.Address?.City}/{cinema?.Address?.State}";
            vm.BaseTicketPrice = session.TicketPrice;

            await RecarregarListas(vm);

            var chairs = await _chairRepository.GetByRoom(vm.RoomId);
            var ticketsSession = await _ticketRepository.GetBySessionId(vm.SessionId);

            var occupiedSeats = ticketsSession
                .Select(t => new { t.Row, t.Column, t.RoomId })
                .ToHashSet();

            var validSeats = new List<SeatSelectionViewModel>();

            foreach (var seatVm in vm.Seats)
            {
                var chair = chairs.FirstOrDefault(c =>
                    c.RoomId == vm.RoomId &&
                    c.Column == seatVm.Column &&
                    c.Row == seatVm.Row);

                if (chair == null)
                    continue;

                bool isOccupied = occupiedSeats.Any(o =>
                    o.RoomId == vm.RoomId &&
                    o.Column == seatVm.Column &&
                    o.Row == seatVm.Row);

                validSeats.Add(new SeatSelectionViewModel
                {
                    RoomId = vm.RoomId,
                    Column = seatVm.Column,
                    Row = seatVm.Row,
                    IsVip = chair.IsVip,
                    IsOccupied = isOccupied,
                    Selected = seatVm.Selected,
                    HalfPrice = seatVm.HalfPrice,
                    CalculatedPrice = session.TicketPrice
                });
            }

            var selectedSeats = validSeats
                .Where(s => s.Selected && !s.IsOccupied)
                .ToList();

            if (!selectedSeats.Any())
            {
                ModelState.AddModelError("", "Selecione pelo menos uma poltrona livre.");
                vm.Seats = validSeats;
                return View("~/Views/Admin/Order/Create.cshtml", vm);
            }

            int ticketsTotal = 0;
            foreach (var seat in selectedSeats)
            {
                int price = session.TicketPrice;
                float finalPrice = price;

                if (seat.IsVip)
                {
                    finalPrice = (float)(price * 1.15);
                    price = (int)finalPrice;
                }

                if (seat.HalfPrice)
                    price = price / 2;

                seat.CalculatedPrice = price;
                ticketsTotal += price;
            }

            var allProducts = await _productRepository.GetAll();
            var productsDict = allProducts.ToDictionary(p => p.Id, p => p);

            int productsTotal = vm.Products
                .Where(p => p.Quantity > 0 && productsDict.ContainsKey(p.ProductId))
                .Sum(pr =>
                {
                    var prod = productsDict[pr.ProductId];
                    int unit = prod.Price - ((prod.Price * prod.Discount) / 100);
                    return Math.Max(unit, 0) * pr.Quantity;
                });

            int grandTotal = ticketsTotal + productsTotal;

            var order = new Order
            {
                CinemaId = vm.CinemaId,
                DtSale = DateOnly.FromDateTime(DateTime.Today),
                TotalPrice = grandTotal,
                PaymentType = vm.PaymentType,
                ClientId = vm.ClientId
            };

            await _orderRepository.Create(order);

            foreach (var seat in selectedSeats)
            {
                var ticket = new Ticket
                {
                    SessionId = vm.SessionId,
                    Column = seat.Column,
                    Row = seat.Row,
                    RoomId = vm.RoomId,
                    OrderId = order.Id,
                    Price = seat.CalculatedPrice,
                    HalfPrice = seat.HalfPrice
                };

                await _ticketRepository.Create(ticket);
            }

            foreach (var pr in vm.Products.Where(p => p.Quantity > 0))
            {
                var prod = productsDict[pr.ProductId];
                int price = Math.Max(prod.Price - ((prod.Price * prod.Discount) / 100), 0);

                var po = new ProductOrder
                {
                    OrderId = order.Id,
                    ProductId = prod.Id,
                    QuantitySale = pr.Quantity,
                    OrderPrice = price * pr.Quantity
                };

                await _productOrderRepository.Create(po);
            }

            return RedirectToAction("IndexAdmin");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (!AdminHelper.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

            var order = await _orderRepository.GetById(id);
            if (order == null)
                return NotFound();

            var session = await _sessionRepository.GetById(order.Tickets.First().SessionId);
            if (session == null)
                return BadRequest("Sessão não encontrada.");

            var room = session.CinemaRoom;
            var cinema = room.Cinema;

            var chairs = await _chairRepository.GetByRoom(room.Id);
            var ticketsSession = await _ticketRepository.GetBySessionId(session.Id);

            var occupiedSeats = ticketsSession
                .Where(t => t.OrderId != order.Id)
                .Select(t => new { t.Row, t.Column, t.RoomId })
                .ToHashSet();

            var vm = new OrderCreateViewModel
            {
                OrderId = order.Id,
                SessionId = session.Id,
                CinemaId = cinema.Id,
                RoomId = room.Id,
                MovieTitle = session.Movie!.Title!,
                MovieAge = session.Movie!.MinimumAge!,
                MovieDescription = session.Movie!.Description!,
                MovieImage = session.Movie!.ImageUrl!,
                MovieDuration = session.Movie!.Duration!,
                SessionHour = session.SessionHour,
                RoomDescription = $"{room.RoomNumber} - {cinema.Name}, {cinema.Address!.City}/{cinema.Address.State}",
                BaseTicketPrice = session.TicketPrice,
                PaymentType = order.PaymentType,
                ClientId = order.ClientId,

                Clients = await _clientRepository.GetAll(),

                Products = (await _productRepository.GetAll())
                    .Select(p =>
                    {
                        var existing = order.ProductOrders.FirstOrDefault(x => x.ProductId == p.Id);

                        return new ProductSelectionViewModel
                        {
                            ProductId = p.Id,
                            Name = p.Name!,
                            UnitPrice = p.Price,
                            Discount = p.Discount,
                            Quantity = existing?.QuantitySale ?? 0
                        };
                    }).ToList(),

                Seats = chairs.Select(c =>
                {
                    bool isOccupied = occupiedSeats.Any(o =>
                        o.RoomId == room.Id &&
                        o.Row == c.Row &&
                        o.Column == c.Column);

                    bool isSelected = order.Tickets.Any(t => t.Column == c.Column && t.Row == c.Row);

                    return new SeatSelectionViewModel
                    {
                        RoomId = room.Id,
                        Column = c.Column,
                        Row = c.Row!,
                        IsVip = c.IsVip,
                        IsOccupied = isOccupied,
                        Selected = isSelected,
                        HalfPrice = order.Tickets
                            .FirstOrDefault(t => t.Column == c.Column && t.Row == c.Row)
                            ?.HalfPrice ?? false,
                        CalculatedPrice = session.TicketPrice
                    };

                }).OrderBy(s => s.Row).ThenBy(s => s.Column).ToList()
            };

            return View("~/Views/Admin/Order/Update.cshtml", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(OrderCreateViewModel vm)
        {
            if (!AdminHelper.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                await RecarregarListas(vm);
                return View("~/Views/Admin/Order/Update.cshtml", vm);
            }

            var order = await _orderRepository.GetById(vm.OrderId!.Value);
            if (order == null)
                return NotFound();

            var session = await _sessionRepository.GetById(vm.SessionId);
            var room = await _roomRepository.GetById(vm.RoomId);

            if (session == null || room == null)
                return BadRequest();

            await RecarregarListas(vm);

            var chairs = await _chairRepository.GetByRoom(vm.RoomId);

            var ticketsSession = await _ticketRepository.GetBySessionId(vm.SessionId);

            var occupiedSeats = ticketsSession
                .Where(t => t.OrderId != order.Id)
                .Select(t => new { t.Row, t.Column, t.RoomId })
                .ToHashSet();

            var validSeats = new List<SeatSelectionViewModel>();

            foreach (var seatVm in vm.Seats)
            {
                var chair = chairs.FirstOrDefault(c =>
                    c.RoomId == vm.RoomId &&
                    c.Column == seatVm.Column &&
                    c.Row == seatVm.Row);

                if (chair == null)
                    continue;

                bool isOccupied = occupiedSeats.Any(o =>
                    o.RoomId == vm.RoomId &&
                    o.Column == seatVm.Column &&
                    o.Row == seatVm.Row);

                validSeats.Add(new SeatSelectionViewModel
                {
                    RoomId = vm.RoomId,
                    Column = seatVm.Column,
                    Row = seatVm.Row,
                    IsVip = chair.IsVip,
                    IsOccupied = isOccupied,
                    Selected = seatVm.Selected,
                    HalfPrice = seatVm.HalfPrice,
                });
            }

            var selectedSeats = validSeats
                .Where(s => s.Selected && !s.IsOccupied)
                .ToList();

            if (!selectedSeats.Any())
            {
                ModelState.AddModelError("", "Selecione pelo menos uma poltrona livre.");
                vm.Seats = validSeats;
                return View("~/Views/Admin/Order/Update.cshtml", vm);
            }

            int ticketsTotal = 0;

            foreach (var seat in selectedSeats)
            {
                int price = session.TicketPrice;

                if (seat.IsVip)
                    price = (int)(price * 1.15);

                if (seat.HalfPrice)
                    price /= 2;

                seat.CalculatedPrice = price;
                ticketsTotal += price;
            }

            var allProducts = await _productRepository.GetAll();
            var productsDict = allProducts.ToDictionary(p => p.Id, p => p);

            int productsTotal = vm.Products
                .Where(p => p.Quantity > 0 && productsDict.ContainsKey(p.ProductId))
                .Sum(pr =>
                {
                    var prod = productsDict[pr.ProductId];
                    return Math.Max(prod.Price - ((prod.Price * prod.Discount) / 100), 0) * pr.Quantity;
                });

            int grandTotal = ticketsTotal + productsTotal;

            order.PaymentType = vm.PaymentType;
            order.ClientId = vm.ClientId;
            order.TotalPrice = grandTotal;

            await _orderRepository.Update(order);

            await _ticketRepository.DeleteByOrder(order.Id);
            await _productOrderRepository.DeleteByOrder(order.Id);

            foreach (var seat in selectedSeats)
            {
                var ticket = new Ticket
                {
                    SessionId = vm.SessionId,
                    Column = seat.Column,
                    Row = seat.Row,
                    RoomId = vm.RoomId,
                    OrderId = order.Id,
                    Price = seat.CalculatedPrice,
                    HalfPrice = seat.HalfPrice
                };

                await _ticketRepository.Create(ticket);
            }

            foreach (var pr in vm.Products.Where(p => p.Quantity > 0))
            {
                var prod = productsDict[pr.ProductId];
                int price = Math.Max(prod.Price - ((prod.Price * prod.Discount) / 100), 0);

                var po = new ProductOrder
                {
                    OrderId = order.Id,
                    ProductId = prod.Id,
                    QuantitySale = pr.Quantity,
                    OrderPrice = price * pr.Quantity
                };

                await _productOrderRepository.Create(po);
            }

            return RedirectToAction("IndexAdmin");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!AdminHelper.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

            var order = await _orderRepository.GetById(id);
            if (order == null)
                return NotFound();

            await _ticketRepository.DeleteByOrder(id);
            await _productOrderRepository.DeleteByOrder(id);
            await _orderRepository.Delete(order);

            return RedirectToAction("IndexAdmin");
        }


        private async Task RecarregarListas(OrderCreateViewModel vm)
        {

            vm.Clients = await _clientRepository.GetAll();

            var products = await _productRepository.GetAll();
            var dictQuant = vm.Products.ToDictionary(p => p.ProductId, p => p.Quantity);

            vm.Products = products.Select(p => new ProductSelectionViewModel
            {
                ProductId = p.Id,
                Name = p.Name!,
                UnitPrice = p.Price,
                Discount = p.Discount,
                Quantity = dictQuant.ContainsKey(p.Id) ? dictQuant[p.Id] : 0
            }).ToList();
        }
    }
}
