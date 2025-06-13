using customers_manager.Models.Interfaces;

namespace customers_manager.Models
{
    public class Customer: ICustomer
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required int Age { get; set; }
        public required int Id { get; set; }


    }
}
