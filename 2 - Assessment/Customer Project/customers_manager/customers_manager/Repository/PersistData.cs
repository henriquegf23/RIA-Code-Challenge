using customers_manager.Models;
using customers_manager.Repository.Interfaces;
using System.IO;
using System.Security.AccessControl;
using System.Text.Json;


namespace customers_manager.Repository
{
    public class PersistData : IPersistData
    {
        private readonly string _file;
        public PersistData(string file) { 
            _file = file;
        }

        public List<Customer> LoadData()
        {
            
            string dataFolder = Path.Combine(_file, "App_Data");
            string filePath = Path.Combine(dataFolder, "data_store.json");

            if (!string.IsNullOrEmpty(dataFolder) && !Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
            }


            var json = File.ReadAllText(filePath);
            if (json == "")
            {
                json = "[]";
            }
            var customers = JsonSerializer.Deserialize<List<Customer>>(json);

            return customers;
        }

        public void SaveData(List<Customer> customers)
        {
            string dataFolder = Path.Combine(_file, "App_Data");
            string filePath = Path.Combine(dataFolder, "data_store.json");

            lock (filePath)
            {
                try
                {
                    var customersSaved = LoadData();
                    List<Customer> newCustomerList = new List<Customer>();

                    int maxLenght = customers.Count + customersSaved.Count;

                    for (int i = 0; i < customers.Count; i++)
                    {
                        var customer = customers[i];

                        if (customersSaved.Count > 0)
                        {
                            bool inserted = false;
                            for (int j = 0; j < customersSaved.Count; j++)
                            {
                                int compFirst = String.Compare(customer.FirstName, customersSaved[j].FirstName, StringComparison.OrdinalIgnoreCase);
                                int compLast = String.Compare(customer.LastName, customersSaved[j].LastName, StringComparison.OrdinalIgnoreCase);

                                if (compLast < 0)
                                {
                                    customersSaved.Insert(j, customer);
                                    inserted = true;
                                    break;
                                }
                                else if (compLast == 0)
                                {
                                    if (compFirst < 0)
                                    {
                                        customersSaved.Insert(j, customer);
                                        inserted = true;
                                        break;
                                    }

                                    if (compFirst == 0)
                                    {
                                        customersSaved.Insert(j, customer);
                                        inserted = true;
                                        break;
                                    }
                                }

                                if (j == customersSaved.Count - 1)
                                {
                                    customersSaved.Add(customer);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            customersSaved.Add(customer);
                        }
                    }

                    var json = JsonSerializer.Serialize(customersSaved);
                    File.WriteAllText(filePath, json);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving data: {ex.Message}");
                }

            }
        }
    }
}
