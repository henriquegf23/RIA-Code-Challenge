using customers_manager.Models;

namespace customers_manager.Services.Interfaces
{
    public interface ICustomerServices
    {
        public void AddCustomers(List<Customer> customers);
        public List<Customer> LoadCustomers();
    }
}
