using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace novasoft_technical_test.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public decimal GrossAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public ICollection<InvoiceDetail> Details { get; set; }
    }
}
