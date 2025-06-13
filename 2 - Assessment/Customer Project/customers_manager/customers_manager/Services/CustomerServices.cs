


using customers_manager.Models;
using customers_manager.Repository;
using customers_manager.Repository.Interfaces;
using customers_manager.Services.Interfaces;

namespace customers_manager.Services
{
    public class CustomerServices : ICustomerServices
    {
        IPersistData _persistData;

        public CustomerServices(IPersistData pd) 
        { 
            _persistData = pd; 
        }

        public void AddCustomers(List<Customer> customers)
        {


            _persistData.SaveData(customers);
        }

        public List<Customer> LoadCustomers()
        {

            List<Customer> customers = new List<Customer>();
            customers = _persistData.LoadData();
            return customers;
        }
    }
}
