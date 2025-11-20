using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class ProductOrderRepository : IProductOrderRepository
    {
        private readonly CineContext _context;

        public ProductOrderRepository(CineContext movieContext)
        {
            _context = movieContext;
        }

        // --- MÉTODOS DE MANIPULAÇÃO DE DADOS (CUD) ---

        public async Task Create(ProductOrder ProductOrder)
        {
            await _context.ProductOrders.AddAsync(ProductOrder);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int? originalProductId, int? originalOrderId, ProductOrder ProductOrderNewData)
        {
            var ProductOrderOld = await _context.ProductOrders
                .FindAsync(originalProductId, originalOrderId);

            if (ProductOrderOld != null)
            {
                // Remove o registro antigo
                _context.ProductOrders.Remove(ProductOrderOld);
                await _context.SaveChangesAsync();

                // Adiciona o novo registro com os dados atualizados
                await _context.ProductOrders.AddAsync(ProductOrderNewData);
                await _context.SaveChangesAsync();
            }
        }
        public async Task Delete(ProductOrder ProductOrder)
        {
            _context.ProductOrders.Remove(ProductOrder);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByOrder(int orderId)
        {
            var items = await _context.ProductOrders
                .Where(po => po.OrderId == orderId)
                .ToListAsync();

            if (items.Any())
            {
                _context.ProductOrders.RemoveRange(items);
                await _context.SaveChangesAsync();
            }
        }

        // --- MÉTODOS DE CONSULTA (READ) ---

        public async Task<ProductOrder?> Get(int productId, int orderId)
        {
            return await _context.ProductOrders
                .Include(x => x.Product)
                .Include(x => x.Order)
                .FirstOrDefaultAsync(w => w.ProductId == productId && w.ProductId == orderId);
        }

        public async Task<List<ProductOrder>> GetAll()
        {
            return await _context.ProductOrders
                .Include(x => x.Product)
                .Include(x => x.Order)
                .ToListAsync();
        }

        public async Task<List<ProductOrder>> GetByOrderId(int orderId)
        {
            return await _context.ProductOrders
                .Include(x => x.Product)
                .Include(x => x.Order)
                .Where(w => w.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<List<ProductOrder>> GetByProductId(int productId)
        {
            return await _context.ProductOrders
                .Include(x => x.Product)
                .Include(x => x.Order)
                .Where(w => w.ProductId == productId)
                .ToListAsync();
        }
    }
}
