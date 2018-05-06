using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Contacts = new HashSet<Contact>();
        }

        [JsonIgnore]
        public Guid CustomerId { get; set; }

        [NotMapped]
        public string CustomerSid {
            get
            {
                return CustomerId.ToString();
            }
            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    CustomerId = Guid.Parse(value);
                }
            }
        }

        public string Name { get; set; }

        public ICollection<Contact> Contacts { get; set; }
    }
}
