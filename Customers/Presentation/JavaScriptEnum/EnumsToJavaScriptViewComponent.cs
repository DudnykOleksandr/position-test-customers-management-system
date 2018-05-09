using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.JavaScriptEnum
{
    public class EnumsToJavaScriptViewComponent : ViewComponent
    {
        public Task<HtmlString> InvokeAsync()
        {
            var query = from a in GetReferencingAssemblies()
                        from t in a.GetTypes()
                        from r in t.GetTypeInfo().GetCustomAttributes<JavaScriptEnumAttribute>()
                        where t.GetTypeInfo().BaseType == typeof(Enum)
                        select t;

            var buffer = new StringBuilder(10000);

            foreach (var jsEnum in query)
            {
                buffer.Append("var ");
                buffer.Append(jsEnum.Name);
                buffer.Append(" = ");
                buffer.Append(EnumToString(jsEnum));
                buffer.Append("; \r\n");
            }

            return Task.FromResult(new HtmlString(buffer.ToString()));
        }

        private static string EnumToString(Type enumType)
        {
            var values = Enum.GetValues(enumType).Cast<int>();
            var enumDictionary = values.ToDictionary(value => Enum.GetName(enumType, value));

            return JsonConvert.SerializeObject(enumDictionary);
        }

        private static IEnumerable<Assembly> GetReferencingAssemblies()
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;

            foreach (var library in dependencies)
            {
                try
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
                catch (FileNotFoundException)
                { }
            }
            return assemblies;
        }
    }
}
