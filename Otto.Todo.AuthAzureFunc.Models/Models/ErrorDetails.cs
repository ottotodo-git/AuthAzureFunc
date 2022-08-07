using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Otto.Todo.AuthAzureFunc.Models.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }

        public String ErrorMessage { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
