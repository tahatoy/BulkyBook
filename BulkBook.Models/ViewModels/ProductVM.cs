using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkBook.Models.ViewModels
{
    public class ProductVM
    {
    
        public Product Product { get;set; }
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public IEnumerable<SelectListItem> CoverTypeList { get; set; }
    }
}
