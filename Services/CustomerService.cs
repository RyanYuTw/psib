using Microsoft.EntityFrameworkCore;
using PSIB.Data;
using PSIB.Models;

namespace PSIB.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _db;

    public CustomerService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Customer>> GetAllAsync(string? keyword = null)
    {
        var query = _db.Customers.Where(c => c.IsActive);
        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(c => c.Name.Contains(keyword) || (c.Phone != null && c.Phone.Contains(keyword)));
        return await query.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(string id) =>
        await _db.Customers.FirstOrDefaultAsync(c => c.Id == id);

    public async Task AddAsync(Customer customer)
    {
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _db.Customers.Update(customer);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var customer = await _db.Customers.FindAsync(id);
        if (customer != null)
        {
            customer.IsActive = false;
            await _db.SaveChangesAsync();
        }
    }

    public async Task<string> GenerateNewIdAsync()
    {
        var max = await _db.Customers.MaxAsync(c => (string?)c.Id);
        if (max == null) return "C0001";
        if (int.TryParse(max[1..], out int num))
            return $"C{num + 1:D4}";
        return $"C{_db.Customers.Count() + 1:D4}";
    }
}
