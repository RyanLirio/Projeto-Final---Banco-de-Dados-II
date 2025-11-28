using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Data
{
    public class DbInitializer
    {
        public static void Initialize(CineContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Products.Any())
            {
                var products = new Product[]
                {
                    new Product { Name = "Pipoca Pequena", Price = 1200, Discount = 0, Category = "teste" },
                    new Product { Name = "Pipoca Média", Price = 1600, Discount = 0, Category = "teste" },
                    new Product { Name = "Pipoca Grande", Price = 2000, Discount = 0, Category = "teste" },
                    new Product { Name = "Pipoca Doce", Price = 1800, Discount = 0, Category = "teste" },

                    new Product { Name = "Combo Clássico - Pipoca Média + Refri 500ml", Price = 3200, Discount = 0, Category = "Combo" },
                    new Product { Name = "Combo Família - Pipoca Grande + 2 Refri 700ml", Price = 4800, Discount = 5, Category = "Combo" },
                    new Product { Name = "Combo Doce - Pipoca Doce + Refri 500ml", Price = 3500, Discount = 0, Category = "Combo" },
                    new Product { Name = "Combo Premium - Pipoca Grande + 2 Refri + Nachos", Price = 6200, Discount = 8, Category = "Combo" },
                    new Product { Name = "Combo Kids - Pipoca Pequena + Suco", Price = 2200, Discount = 0, Category = "Combo" },

                    new Product { Name = "Pipoca Pequena", Price = 1200, Discount = 0, Category = "Pipoca" },
                    new Product { Name = "Pipoca Média", Price = 1600, Discount = 0, Category = "Pipoca" },
                    new Product { Name = "Pipoca Grande", Price = 2000, Discount = 0, Category = "Pipoca" },
                    new Product { Name = "Pipoca Doce", Price = 1800, Discount = 0, Category = "Pipoca" },

                    new Product { Name = "Refrigerante 500ml", Price = 1000, Discount = 0, Category = "Bebida" },
                    new Product { Name = "Refrigerante 700ml", Price = 1200, Discount = 0, Category = "Bebida" },
                    new Product { Name = "Suco Natural 300ml", Price = 1400, Discount = 0, Category = "Bebida" },
                    new Product { Name = "Água Mineral 500ml", Price = 600,  Discount = 0, Category = "Bebida" },

                    new Product { Name = "Nachos com Queijo", Price = 1899, Discount = 0, Category = "Salgado" },
                };

                foreach (Product p in products)
                {
                    context.Products.Add(p);
                }
                context.SaveChanges();
            }

            if (!context.Sexes.Any())
            {
                var sexes = new Sex[]
                {
                    new Sex { Name = "Ação" },
                    new Sex { Name = "Aventura" },
                    new Sex { Name = "Drama" },
                    new Sex { Name = "Ficção Científica" },
                    new Sex { Name = "Animação" }
                };

                foreach(Sex s in sexes)
                {
                    context.Sexes.Add(s);
                }
                context.SaveChanges();
            }

            if (!context.Languages.Any())
            {
                var languages = new Language[]
                {
                    new Language { Name = "Português" },
                    new Language { Name = "Inglês" },
                    new Language { Name = "Espanhol" },
                    new Language { Name = "Francês" },
                    new Language { Name = "Japonês" }
                };

                foreach (Language l in languages)
                {
                    context.Languages.Add(l);
                }
                context.SaveChanges();
            }

            if (!context.Addresses.Any())
            {
                var addresses = new Address[]
                {
                    new Address { ZipCode = "89560-000", City = "Videira",        Road = "Rua Nicolau Cavon",        State = "SC", Number = 145, Descripton = "Endereço central" },
                    new Address { ZipCode = "01001-000", City = "São Paulo",     Road = "Avenida Paulista",         State = "SP", Number = 2000, Descripton = "Próximo ao MASP" },
                    new Address { ZipCode = "20040-020", City = "Rio de Janeiro", Road = "Rua da Assembleia",        State = "RJ", Number = 65,  Descripton = "Centro financeiro" },
                    new Address { ZipCode = "30190-924", City = "Belo Horizonte", Road = "Avenida Amazonas",         State = "MG", Number = 190, Descripton = "Região central" },
                    new Address { ZipCode = "70040-000", City = "Brasília",       Road = "Esplanada dos Ministérios", State = "DF", Number = 1, Descripton = "Prédio público" }
                };

                context.Addresses.AddRange(addresses);
                context.SaveChanges();
            }

            if (!context.Cinemas.Any())
            {
                var cinemas = new Cinema[]
                {
                    new Cinema { Name = "CineMaster",   Cnpj = "12.345.678/0001-95", Phone = "(49) 99111-2222", Email = "contato@cinemaster.com", AddressId = 1 },
                    new Cinema { Name = "CineVision",   Cnpj = "23.456.789/0001-45", Phone = "(11) 99888-7766", Email = "suporte@cinevision.com", AddressId = 2 },
                    new Cinema { Name = "MoviePoint",   Cnpj = "34.567.890/0001-12", Phone = "(21) 99777-6655", Email = "atendimento@moviepoint.com", AddressId = 3 },
                    new Cinema { Name = "MaxCinema",    Cnpj = "45.678.901/0001-33", Phone = "(31) 98666-5544", Email = "contato@maxcinema.com", AddressId = 4 },
                    new Cinema { Name = "GalaxyFilms",  Cnpj = "56.789.012/0001-22", Phone = "(61) 98555-4433", Email = "galaxy@films.com", AddressId = 5 }
                };

                foreach (Cinema c in cinemas)
                {
                    context.Cinemas.Add(c);
                }
                context.SaveChanges();
            }

            if (!context.CinemaRooms.Any())
            {
                var rooms = new CinemaRoom[]
                {
                    new CinemaRoom { CinemaId = 1, RoomNumber = "Sala 1" },
                    new CinemaRoom { CinemaId = 1, RoomNumber = "Sala 2" },

                    new CinemaRoom { CinemaId = 2, RoomNumber = "Sala 1" },
                    new CinemaRoom { CinemaId = 2, RoomNumber = "Sala 2" },

                    new CinemaRoom { CinemaId = 3, RoomNumber = "Sala 1" },
                    new CinemaRoom { CinemaId = 3, RoomNumber = "Sala 2" },

                    new CinemaRoom { CinemaId = 4, RoomNumber = "Sala 1" },
                    new CinemaRoom { CinemaId = 4, RoomNumber = "Sala 2" },

                    new CinemaRoom { CinemaId = 5, RoomNumber = "Sala 1" },
                    new CinemaRoom { CinemaId = 5, RoomNumber = "Sala 2" }
                };

                foreach (CinemaRoom r in rooms)
                {
                    context.CinemaRooms.Add(r);
                }
                context.SaveChanges();
            }

            if (!context.Chairs.Any())
            {
                var random = new Random();
                var rooms = context.CinemaRooms.ToList();
                var chairs = new List<Chair>();

                foreach (var room in rooms)
                {
                    int totalColumns = random.Next(7, 12);
                    int totalRows = random.Next(7, 12);

                    int vipRowIndex = random.Next(0, totalRows);

                    for (int r = 0; r < totalRows; r++)
                    {
                        string rowLetter = ((char)('A' + r)).ToString();
                        bool isVipRow = r == vipRowIndex;

                        for (int c = 1; c <= totalColumns; c++)
                        {
                            chairs.Add(new Chair
                            {
                                RoomId = room.Id,
                                Column = c,
                                Row = rowLetter,
                                IsVip = isVipRow
                            });
                        }
                    }
                }

                foreach (Chair c in chairs)
                {
                    context.Chairs.Add(c);
                }
                context.SaveChanges();
            }

            if (!context.Movies.Any())
            {
                var random = new Random();
                var today = DateOnly.FromDateTime(DateTime.Now);

                var movies = new Movie[]
                {
                    new Movie {
                        Title = "Avatar 3 - The Seed Bearer",
                        Duration = 175,
                        MinimumAge = 12,
                        Rating = 9,
                        Description = "Continuação da épica jornada dos Na'vi e da família Sully.",
                        Synopsis = "Jake Sully e Neytiri enfrentam novas ameaças em Pandora enquanto criam seus filhos.",
                        Studio = "20th Century Studios",
                        DtRelease = today.AddDays(random.Next(1, 31)),
                        LanguageId = 1,
                        ImageUrl = "avatar-3-the-seed-bearer.jpg",
                        UrlBanner = "avatar_3_the_seed_bearer-banner.jpg"
                    },

                    new Movie {
                        Title = "Batman Renascimento",
                        Duration = 145,
                        MinimumAge = 14,
                        Rating = 8,
                        Description = "Uma nova era sombria para o Cavaleiro das Trevas.",
                        Synopsis = "Bruce Wayne retorna após anos afastado para enfrentar uma ameaça que ressurge em Gotham.",
                        Studio = "Warner Bros.",
                        DtRelease = today.AddDays(random.Next(1, 31)),
                        LanguageId = 1,
                        ImageUrl = "batman-renascimento.jpg",
                        UrlBanner = "batman_renascimento-banner.jpg"
                    },

                    new Movie {
                        Title = "Jurassic World - Extinção",
                        Duration = 160,
                        MinimumAge = 12,
                        Rating = 7,
                        Description = "O capítulo final da saga dos dinossauros.",
                        Synopsis = "Humanos e dinossauros lutam por sobrevivência em um planeta fora de controle.",
                        Studio = "Universal Pictures",
                        DtRelease = today.AddDays(random.Next(1, 31)),
                        LanguageId = 1,
                        ImageUrl = "jurassic-world-extincao.jpg",
                        UrlBanner = "jurassic_world_extincao-banner.jpg"
                    },

                    new Movie {
                        Title = "Kung Fu Panda 4",
                        Duration = 110,
                        MinimumAge = 0,
                        Rating = 8,
                        Description = "Po retorna para mais aventuras e desafios espirituais.",
                        Synopsis = "Po precisa encontrar seu sucessor enquanto enfrenta uma vilã poderosa do submundo.",
                        Studio = "DreamWorks Animation",
                        DtRelease = today.AddDays(random.Next(1, 31)),
                        LanguageId = 1,
                        ImageUrl = "Kung-Fu-Panda-4.jpg",
                        UrlBanner = "kung_fu_panda_4_banner.jpg"
                    },

                    new Movie {
                        Title = "Matrix",
                        Duration = 136,
                        MinimumAge = 14,
                        Rating = 10,
                        Description = "O filme que redefiniu ficção científica.",
                        Synopsis = "Neo descobre que a realidade é uma simulação e precisa decidir seu destino.",
                        Studio = "Warner Bros.",
                        DtRelease = new DateOnly(1999, 3, 31),
                        LanguageId = 1,
                        ImageUrl = "matrix.jpg",
                        UrlBanner = "matrix-banner.jpg"
                    },

                    new Movie {
                        Title = "Moana 2",
                        Duration = 105,
                        MinimumAge = 0,
                        Rating = 8,
                        Description = "Moana retorna ao mar em busca de respostas.",
                        Synopsis = "Ela embarca em uma jornada para descobrir segredos ancestrais de sua família.",
                        Studio = "Walt Disney Animation Studios",
                        DtRelease = new DateOnly(2024, 11, 27),
                        LanguageId = 1,
                        ImageUrl = "moana-2.jpg",
                        UrlBanner = "moana_2_banner.jpg"
                    },

                    new Movie {
                        Title = "Mufasa",
                        Duration = 128,
                        MinimumAge = 10,
                        Rating = 8,
                        Description = "A origem do rei das Terras do Reino.",
                        Synopsis = "A história nunca contada de Mufasa, antes de se tornar um símbolo.",
                        Studio = "Walt Disney Pictures",
                        DtRelease = new DateOnly(2024, 12, 20),
                        LanguageId = 1,
                        ImageUrl = "mufasa.jpg",
                        UrlBanner = "mufasa-banner.jpg"
                    },

                    new Movie {
                        Title = "Sonic 3 - The Movie",
                        Duration = 122,
                        MinimumAge = 6,
                        Rating = 7,
                        Description = "Sonic enfrenta seu maior inimigo até agora.",
                        Synopsis = "A chegada de Shadow coloca o destino do mundo em risco.",
                        Studio = "Paramount Pictures",
                        DtRelease = new DateOnly(2024, 12, 12),
                        LanguageId = 1,
                        ImageUrl = "sonic-3-the-movie.jpg",
                        UrlBanner = "sonic_3_the_movie-banner.jpg"
                    },

                    new Movie {
                        Title = "Venom 3",
                        Duration = 130,
                        MinimumAge = 14,
                        Rating = 7,
                        Description = "Eddie Brock enfrenta um novo simbionte mortal.",
                        Synopsis = "O vínculo entre Eddie e Venom é testado diante de um inimigo mais poderoso.",
                        Studio = "Sony Pictures",
                        DtRelease = new DateOnly(2024, 10, 25),
                        LanguageId = 1,
                        ImageUrl = "venom-3.jpg",
                        UrlBanner = "venom_3-banner.jpg"
                    }
                };

                foreach (Movie m in movies)
                {
                    context.Movies.Add(m);
                }
                context.SaveChanges();
            }

            if (!context.SexMovies.Any())
            {
                var sexMovies = new SexMovie[]
                {
                    new SexMovie { SexId = 1, MovieId = 1 },
                    new SexMovie { SexId = 4, MovieId = 1 },

                    new SexMovie { SexId = 1, MovieId = 2 },
                    new SexMovie { SexId = 3, MovieId = 2 },

                    new SexMovie { SexId = 2, MovieId = 3 },
                    new SexMovie { SexId = 4, MovieId = 3 },

                    new SexMovie { SexId = 5, MovieId = 4 },
                    new SexMovie { SexId = 2, MovieId = 4 },

                    new SexMovie { SexId = 4, MovieId = 5 },
                    new SexMovie { SexId = 1, MovieId = 5 },

                    new SexMovie { SexId = 5, MovieId = 6 },
                    new SexMovie { SexId = 2, MovieId = 6 },

                    new SexMovie { SexId = 3, MovieId = 7 },
                    new SexMovie { SexId = 2, MovieId = 7 },

                    new SexMovie { SexId = 1, MovieId = 8 },
                    new SexMovie { SexId = 2, MovieId = 8 },

                    new SexMovie { SexId = 1, MovieId = 9 },
                    new SexMovie { SexId = 4, MovieId = 9 }
                };

                foreach (SexMovie sm in sexMovies)
                {
                    context.SexMovies.Add(sm);
                }
                context.SaveChanges();
            }

            if (!context.Sessions.Any())
            {
                var random = new Random();
                var today = DateOnly.FromDateTime(DateTime.Now);

                var movies = context.Movies.ToList();
                var cinemas = context.Cinemas.ToList();

                var sessions = new List<Session>();
                var hours = new[] { 15, 20 };

                foreach (var cinema in cinemas)
                {
                    var cinemaRooms = context.CinemaRooms
                        .Where(r => r.CinemaId == cinema.Id)
                        .OrderBy(r => r.Id)
                        .ToList();

                    if (cinemaRooms.Count < 2)
                        continue;

                    var sala1 = cinemaRooms[0];
                    var sala2 = cinemaRooms[1];
                    var validMovies = movies
                         .Where(m =>
                         {
                             for (int offset = 0; offset < 3; offset++)
                             {
                                 var d = today.AddDays(offset);

                                 bool ok =
                                     m.DtRelease <= today.AddDays(7) &&
                                     d > m.DtRelease;

                                 if (ok)
                                     return true;
                             }
                             return false;
                         })
                         .OrderBy(m => random.Next())
                         .ToList();

                    if (validMovies.Count < 2)
                        continue;

                    var movieA = validMovies[0];
                    var movieB = validMovies[1];


                    for (int dayOffset = 0; dayOffset < 3; dayOffset++)
                    {
                        var sessionDate = today.AddDays(dayOffset);

                        bool podeFilmeA =
                            movieA.DtRelease <= today.AddDays(7) &&
                            sessionDate > movieA.DtRelease;

                        if (podeFilmeA)
                        {
                            for (int i = 0; i < hours.Length; i++)
                            {
                                var sessionDateTime = new DateTime(
                                    sessionDate.Year, sessionDate.Month, sessionDate.Day,
                                    hours[i], 0, 0);

                                sessions.Add(new Session
                                {
                                    MovieId = movieA.Id,
                                    SessionHour = sessionDateTime,
                                    TicketPrice = (i == 0 ? 2500 : 3000),
                                    LanguageId = 1,
                                    CaptionId = 1,
                                    RoomId = sala1.Id,
                                    Is3D = (i == 1)
                                });
                            }
                        }

                        bool podeFilmeB =
                            movieB.DtRelease <= today.AddDays(7) &&
                            sessionDate > movieB.DtRelease;

                        if (podeFilmeB)
                        {
                            for (int i = 0; i < hours.Length; i++)
                            {
                                var sessionDateTime = new DateTime(
                                    sessionDate.Year, sessionDate.Month, sessionDate.Day,
                                    hours[i], 0, 0);

                                sessions.Add(new Session
                                {
                                    MovieId = movieB.Id,
                                    SessionHour = sessionDateTime,
                                    TicketPrice = (i == 0 ? 2500 : 3000),
                                    LanguageId = 1,
                                    CaptionId = 1,
                                    RoomId = sala2.Id,
                                    Is3D = (i == 1)
                                });
                            }
                        }
                    }
                }

                context.Sessions.AddRange(sessions);
                context.SaveChanges();
            }

            if (!context.Clients.Any())
            {
                var senhaAdmin = BCrypt.Net.BCrypt.HashPassword("123");
                var senhaUser = BCrypt.Net.BCrypt.HashPassword("123");

                var clients = new Client[]
                {
                    new Client
                    {
                        Name = "Administrador Geral",
                        Cpf = "12345678901",
                        Email = "admin@admin.com",
                        Phone = "(00) 90000-0000",
                        Role = "Admin",
                        Birthday = new DateOnly(1990, 1, 1),
                        RegistrationDate = DateTime.Now,
                        AddressId = 1,
                        SenhaHash = senhaAdmin
                    },
                    new Client
                    {
                        Name = "Usuário Comum",
                        Cpf = "98765432100",
                        Email = "user@user.com",
                        Phone = "(00) 98888-7777",
                        Role = "User",
                        Birthday = new DateOnly(1998, 5, 5),
                        RegistrationDate = DateTime.Now,
                        AddressId = 2,
                        SenhaHash = senhaUser
                    }
                };

                foreach (Client c in clients)
                {
                    context.Clients.Add(c);
                }
                context.SaveChanges();
            }
        }
    }
}
