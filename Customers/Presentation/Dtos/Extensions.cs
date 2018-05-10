using Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Presentation.Dtos
{
    public static class Extensions
    {
        public static IEnumerable<CustomerDto> ToDataModels(this IEnumerable<Customer> customers)
        {
            return customers.Select(item =>
             {
                 return CustomerDto.FromDataModel(item);
             });
        }
    }
}
