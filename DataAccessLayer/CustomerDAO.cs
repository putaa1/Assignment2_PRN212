using BusinessObjects;
using DataAccessLayer.DTO;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class CustomerDAO
{
    public static Customer? GetCustomerById(int id)
    {
        using var db = new FuminiHotelManagementContext();
        return db.Customers.FirstOrDefault(c => c.CustomerId.Equals(id));
    }

    public static Customer? GetCustomerByEmail(string email)
    {
        using var db = new FuminiHotelManagementContext();
        return db.Customers.FirstOrDefault(c => c.EmailAddress.Equals(email));
    }

    public static List<CustomerDTO> GetCustomers(Func<Customer, bool> predicate)
    {
        using var db = new FuminiHotelManagementContext();
        return db.Customers
            .Where(predicate)
            .Select(c => new CustomerDTO
            {
                CustomerId = c.CustomerId,
                CustomerFullName = c.CustomerFullName,
                Telephone = c.Telephone,
                EmailAddress = c.EmailAddress,
                CustomerBirthday = c.CustomerBirthday,
                CustomerStatus = c.CustomerStatus,
                Password = c.Password,
            })
            .ToList();
    }

    public static int CountCustomers()
    {
        using var db = new FuminiHotelManagementContext();
        return db.Customers
            .Where(c => c.CustomerStatus == 1)
            .Count();
    }

    public static bool UpdateCustomer(Customer customer)
    {
        using var db = new FuminiHotelManagementContext();
        db.Customers.Update(customer);
        var success = db.SaveChanges();
        return success == 1;
    }

    public static void UpdateCustomer(CustomerDTO customer)
    {
        using var db = new FuminiHotelManagementContext();
        var existingCustomer = db.Customers.Find(customer.CustomerId);
        if (existingCustomer != null)
        {
            existingCustomer.CustomerFullName = customer.CustomerFullName;
            existingCustomer.Telephone = customer.Telephone;
            existingCustomer.EmailAddress = customer.EmailAddress;
            existingCustomer.CustomerBirthday = customer.CustomerBirthday;
            existingCustomer.CustomerStatus = customer.CustomerStatus;
            existingCustomer.Password = customer.Password;

            db.Customers.Update(existingCustomer);
            db.SaveChanges();
        }
    }

    public static void DeleteCustomer(int customerId)
    {
        using var db = new FuminiHotelManagementContext();
        var customer = db.Customers.Find(customerId);
        if (customer != null)
        {
            db.Customers.Remove(customer);
            db.SaveChanges();
        }
    }

    public static void AddCustomer(CustomerDTO customer)
    {
        using var db = new FuminiHotelManagementContext();
        var newCustomer = new Customer
        {
            CustomerFullName = customer.CustomerFullName,
            Telephone = customer.Telephone,
            EmailAddress = customer.EmailAddress,
            CustomerBirthday = customer.CustomerBirthday,
            CustomerStatus = customer.CustomerStatus,
            Password = customer.Password
        };

        db.Customers.Add(newCustomer);
        db.SaveChanges();
    }
}
