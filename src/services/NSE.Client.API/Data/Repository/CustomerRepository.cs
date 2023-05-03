using Microsoft.EntityFrameworkCore;
using NSE.Client.API.Models;
using NSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Client.API.Data.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomersContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public CustomerRepository(CustomersContext context)
        {
            _context = context;
        }

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _context.Customers.AsNoTracking().ToListAsync();
        }

        public async Task<Customer> GetByCPF(string cpf)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.Cpf.Number == cpf);
        }

        public void AddAddress(Address endereco)
        {
            _context.Addressess.Add(endereco);
        }

        public async Task<Address> GetAddressById(Guid id)
        {
            return await _context.Addressess.FirstOrDefaultAsync(e => e.CustomerId == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
