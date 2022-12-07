using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongodbRepository.Entities;
using MongodbRepository.Persistance;

namespace MongodbRepository.Pages
{
    public class EditCustomerModel : PageModel
    {
        private readonly IMongoRepository<Customer> _customerReository;

        [BindProperty]
        public Customer customer { get; set; }
        public EditCustomerModel(IMongoRepository<Customer> CustomerReository)
        {
            _customerReository = CustomerReository;
        }
        public async Task OnGetAsync(string Id)
        {
            customer = await _customerReository.GetByIdAsync(Id);
        }

        public async Task<IActionResult> OnPostAsync(string Id)
        {
            customer.Id = Id;
            var res=   await _customerReository.UpdateOneAsync(customer);

            return RedirectToPage("Index");
        }
    }
}
