using CustomerAPI.Data;
using CustomerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerContext _context;

        public CustomersController(CustomerContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        // PUT: api/Customers/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, [FromBody] Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            var existingCustomer = await _context.Customers
                .Include(c => c.ContactNumbers)
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            _context.Entry(existingCustomer).CurrentValues.SetValues(customer);

            // Handle ContactNumbers
            var contactNumbersToRemove = existingCustomer.ContactNumbers
                .Where(cn => !customer.ContactNumbers.Any(c => c.Id == cn.Id))
                .ToList();

            foreach (var contactNumber in contactNumbersToRemove)
            {
                existingCustomer.ContactNumbers.Remove(contactNumber);
            }

            foreach (var contactNumber in customer.ContactNumbers)
            {
                var existingContactNumber = existingCustomer.ContactNumbers
                    .FirstOrDefault(cn => cn.Id == contactNumber.Id && cn.Type == contactNumber.Type && cn.Number == contactNumber.Number);

                if (existingContactNumber != null)
                {
                    _context.Entry(existingContactNumber).CurrentValues.SetValues(contactNumber);
                }
                else
                {
                    existingCustomer.ContactNumbers.Add(contactNumber);
                }
            }

            // Handle Addresses
            var addressesToRemove = existingCustomer.Addresses
                .Where(a => !customer.Addresses.Any(
                        a2 => a2.Id == a.Id))
                .ToList();

            foreach (var address in addressesToRemove)
            {
                existingCustomer.Addresses.Remove(address);
            }

            foreach (var address in customer.Addresses)
            {
                var existingAddress = existingCustomer.Addresses
                    .FirstOrDefault(a => a.Id == address.Id && a.Barangay == address.Barangay && a.City == address.City && a.Province == address.Province);

                if (existingAddress != null)
                {
                    _context.Entry(existingAddress).CurrentValues.SetValues(address);
                }
                else
                {
                    existingCustomer.Addresses.Add(address);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        // DELETE: api/Customers/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
