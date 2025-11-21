using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly CineContext _context;

        public AddressRepository(CineContext context)
        {
            _context = context;
        }

        public async Task Create(Address address)
        {
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Address address)
        {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
        }

        public Task Delete(Address address)
        {
            _context.Addresses.Remove(address);
            return _context.SaveChangesAsync();
        }

        public async Task<List<Address>> GetAll()
        {
            var data = await _context.Addresses.ToListAsync();
            return data;
        }
        //questpdf
        public async Task<Address?> GetById(int id)
        {
            var address = await _context.Addresses.Where(a => a.Id == id).FirstOrDefaultAsync();
            return address;
        }

        public async Task<List<Address>> GetByCityName(string name)
        {
            var addresses = await _context.Addresses.Where(c => c.City!.ToLower().Contains(name.ToLower())).ToListAsync();

            return addresses;
        }

        

    }
}
