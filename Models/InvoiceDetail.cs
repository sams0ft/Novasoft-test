using System.ComponentModel.DataAnnotations;

namespace novasoft_technical_test.Models
{
    public class InvoiceDetail
    {
        public int InvoiceDetailId { get; set; }

        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        [Required]
        public int ProductCode { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }
    }
}
