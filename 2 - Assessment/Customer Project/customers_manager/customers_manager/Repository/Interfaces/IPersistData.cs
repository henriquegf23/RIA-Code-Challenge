using customers_manager.Models;

namespace customers_manager.Repository.Interfaces
{
    public interface IPersistData 
    {
        public List<Customer> LoadData();
        public void SaveData(List<Customer> data);
    }
}
