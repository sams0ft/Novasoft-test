using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace novasoft_technical_test.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required, MaxLength(15)]
        public string DocumentNumber { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(150)]
        public string Address { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        public ICollection<Invoice> Invoices { get; set; }
    }
}
