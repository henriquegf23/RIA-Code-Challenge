
using customer_manager_simulator.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;


public static class Globals
{
    public static int id { get; set; } = 1;
    public static readonly object lockObject = new object();
}


class Program
{

    static async Task Main(string[] args)
    {
        Console.WriteLine("\nStarting processing...");
        var semaphore = new SemaphoreSlim(400);
        var tasks = new List<Task>();

        Random random = new Random();

        int number_of_requests = random.Next(1, 30);

        for (int i = 0; i < number_of_requests; i++)
        {
            int currentRequestId = i;

            await semaphore.WaitAsync();

            tasks.Add(Task.Run(async () =>
            {
                try
                {
                    var payload = BuildPayload(currentRequestId);
                    var response = SendRequest(payload);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    semaphore.Release();
                }
            }
            ));

        }

        await Task.WhenAll(tasks);

        Console.WriteLine("\nProcessing complete. Press any key to exit...");
        Console.ReadKey();
    }

    public static List<Customer> BuildPayload(int thread)
    {

        Random random = new Random();

        int number_of_elements = random.Next(2, 11);

        List<string> firstName = ["Leia", "Sadie", "Jose", "Sara", "Frank", "Dewey", "Tomas", "Joel", "Lukas", "Carlos"];

        List<string> lastName = ["Liberty", "Ray", "Harrison", "Ronan", "Drew", "Anderson", "Powell", "Larsen", "Chan", "Lane"];

        List<Customer> lc = new List<Customer>();
        for(int i = 0; i< number_of_elements; i++)
        {

            int firstNameIdx = random.Next(firstName.Count);
            int lastNameIdx = random.Next(lastName.Count);

            Customer customer = new Customer();
            customer.FirstName = firstName[firstNameIdx];
            customer.LastName = lastName[lastNameIdx];
            customer.Age = random.Next(10, 90);
            
            lock (Globals.lockObject)
            {
                customer.Id = Globals.id;
                Globals.id++;
            }

            lc.Add(customer);

        }

        return lc;
    }

    public static async Task<HttpResponseMessage> SendRequest(List<Customer> payload)
    {
        Random random = new Random();

        int gp = random.Next(1,11);

        var httpClient = new HttpClient();

        string apiUrl;
        apiUrl = "https://localhost:44315/Customer";
        if (gp % 2 == 0)
        {
            return await httpClient.GetAsync(apiUrl);
        }
        else
        {
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await httpClient.PostAsync(apiUrl, content);
        }

            
    }
}