using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using novasoft_technical_test.Data;
using novasoft_technical_test.DTOs;
using novasoft_technical_test.Models;
using System.Collections.Generic;

namespace novasoft_technical_test.Pages.Invoices
{

    public class InvoicesCreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public InvoicesCreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CustomerModel Customer { get; set; } = new CustomerModel();

        [BindProperty]
        public List<InvoiceDetailModel> InvoiceDetails { get; set; }

        public string? Message { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            bool allDistinct = InvoiceDetails.Select(x => x.ProductCode).Distinct().Count() == InvoiceDetails.Count;

            if (!allDistinct) {
                ModelState.AddModelError("", "Some products have the same code. Product Codes must be unique");
                return Page();
            }

            if (InvoiceDetails.Count() <= 0)
            {
                ModelState.AddModelError("", "You need to add at least one product");
                return Page();
            }

            var existingCustomer = _context.Customers.FirstOrDefault(c => c.DocumentNumber == Customer.DocumentNumber);
            var customerEntity = existingCustomer != null 
                ? existingCustomer 
                : new Customer
            {
                DocumentNumber = Customer.DocumentNumber,
                FirstName = Customer.FirstName,
                LastName = Customer.LastName,
                Address = Customer.Address,
                Phone = Customer.Phone
            };

            var invoiceEntity = new Invoice
            {
                Date = DateTime.Now,
                Customer = customerEntity,
                Details = new List<InvoiceDetail>()
            };

            var amounts = CalculateAmounts(InvoiceDetails.Select(i => {
                return new QuantityAmount
                {
                    Quantity = i.Quantity,
                    UnitPrice = (float)i.UnitPrice,
                };
            }).ToList());

            invoiceEntity.GrossAmount = amounts.GrossAmount;
            invoiceEntity.DiscountAmount = amounts.DiscountAmount;
            invoiceEntity.VatAmount = amounts.VatAmount;
            invoiceEntity.TotalAmount = amounts.TotalAmount;

            invoiceEntity.Details = InvoiceDetails.Select(i => new InvoiceDetail { 
                ProductCode = i.ProductCode,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();

            _context.Invoices.Add(invoiceEntity);
            _context.SaveChanges();

            Message = $"✅ Invoice saved successfully with ID {invoiceEntity.InvoiceId}";

            ModelState.Clear();
            Customer = new CustomerModel();
            InvoiceDetails.Clear();

            return Page();
        }

        public JsonResult OnPostCalculateInvoiceAmounts([FromBody] List<QuantityAmount> inputPrices) {
            var amounts = CalculateAmounts(inputPrices);
            return new JsonResult(new { 
                grossAmount = amounts.GrossAmount,
                discountAmount = amounts.DiscountAmount,
                vatAmount = amounts.VatAmount, 
                totalAmount = amounts.TotalAmount 
            });
        }
        private InvoiceAmounts CalculateAmounts(List<QuantityAmount> inputPrices) {
            var grossAmount = inputPrices.Sum((i) => i.Quantity * i.UnitPrice);
            var discountAmount = grossAmount >= 500000 ? grossAmount * 0.05 : 0;
            var vatAmount = (grossAmount - discountAmount) * 0.19;
            var totalAmount = grossAmount - discountAmount + vatAmount;

            return new InvoiceAmounts { 
                GrossAmount = (decimal)grossAmount,
                DiscountAmount = (decimal)discountAmount,
                VatAmount = (decimal)vatAmount,
                TotalAmount = (decimal)totalAmount,
            };
        }
    }

}
