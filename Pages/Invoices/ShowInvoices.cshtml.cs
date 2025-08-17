using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using novasoft_technical_test.Data;
using novasoft_technical_test.Models;

namespace novasoft_technical_test.Pages.Invoices
{
    public class ShowInvoicesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ShowInvoicesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Invoice> Invoices { get; set; }

        public void OnGet()
        {
            Invoices = _context.Invoices
                .Include(i => i.Customer)
                .ToList();
        }
    }
}
