using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Dtos
{
    public static class Extensions
    {
        public static IEnumerable<CustomerDto> ToDataModels(this IEnumerable<Customer> customers)
        {
            return customers.Select(item =>
             {
                 var dtoItem = new CustomerDto();
                 dtoItem.FromDataModel(item);
                 return dtoItem;
             });
        }
    }
}
