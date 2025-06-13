using customers_manager.Models;
using customers_manager.Repository;
using customers_manager.Services;
using customers_manager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace customers_manager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private List<Customer> _customersList = [];
        private ICustomerServices _customerServices;

        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger, ICustomerServices cs)
        {
            _logger = logger;
            _customerServices = cs;
            _customersList = _customerServices.LoadCustomers();

        }

        [HttpGet(Name = "GetCustomers")]
        public ActionResult<List<Customer>> Get()
        {
            return  Ok(_customersList);
        }

        [HttpPost(Name = "PostCustomers")]
        public async Task<IActionResult> PostCustomers([FromBody] List<Customer> customers)
        {
            if (customers == null)
            {
                return BadRequest("No customers provided");
            }

            List<int> idsValidation = new List<int>();

            // Validate each customer
            foreach (var customer in customers)
            {
                if (string.IsNullOrWhiteSpace(customer.FirstName))
                    return BadRequest("First name is required");

                if (string.IsNullOrWhiteSpace(customer.LastName))
                    return BadRequest("Last name is required");

                if (customer.Age < 18)
                    return BadRequest("Age must be greater than 18");

                if(_customersList.Any(cl => cl.Id == customer.Id))
                {
                    idsValidation.Add(customer.Id);
                }
            }

            if (idsValidation.Count > 0)
            {
                string idList = string.Join(", ", idsValidation);
                return BadRequest("The following Id are already in use: " + idList + ". Please verify and send the list again.");
            }

            _customerServices.AddCustomers(customers);

            return Ok();

        }
    }
}
