using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public partial class Contact
    {
        [JsonIgnore]
        public Guid ContactId { get; set; }

        [NotMapped]
        public string ContactSid { 
            get
            {
                return ContactId.ToString();
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ContactId = Guid.Parse(value);
                }
            }
        }

        public string Name { get; set; }
        
        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; }
    }
}
