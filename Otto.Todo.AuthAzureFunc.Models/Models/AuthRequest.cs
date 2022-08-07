using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otto.Todo.AuthAzureFunc.Models.Models
{
    public class AuthRequest
    {
        public long UserId { get; set; }
        public string AppId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

    }
}
