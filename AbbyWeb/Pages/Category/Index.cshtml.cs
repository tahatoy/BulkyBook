using AbbyWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IEnumerable<Model.Category> Categories { get; set; }
        public IndexModel(ApplicationDbContext db)
        {
            _db=db;
        }

        public void OnGet()
        {
            Categories = _db.Categories.ToList();
        }
    }
}
