using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongodbRepository.Entities;
using MongodbRepository.Persistance;
using MongoDB.Driver;
namespace MongodbRepository.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMongoRepository<Customer> _customerReository;

        public IEnumerable<Customer> customers { get; set; }

        [BindProperty]
        public Customer customer { get; set; }
        public IndexModel(IMongoRepository<Customer> CustomerReository)
        {
            _customerReository = CustomerReository;
        }


        public async Task OnGetAsync()
        {
            // customers = await _customerReository.GetAsync(p=>p.FirstName=="میلاد",s=>s.OrderByDescending(p=>p.Contact),1,1);

            // customers = await _customerReository.GetAsync(FilterBuilder: Builders<Customer>.Filter.Eq(p=>p.FirstName,"میلاد"),Builders<Customer>.Sort.Descending(p=>p.Contact),10,1 );

            customers = await _customerReository.GetAllAsync();
        }


        public async Task<IActionResult> OnGetDeleteCustomerAsync(string Id)
        {

            await _customerReository.DeleteOneAsync(Id);

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostCreateCustomerAsync()
        {

            await _customerReository.AddAsync(customer);

            return RedirectToPage();
        }
    }
}