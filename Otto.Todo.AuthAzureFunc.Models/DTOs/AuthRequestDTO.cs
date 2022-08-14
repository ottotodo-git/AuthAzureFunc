using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otto.Todo.AuthAzureFunc.Models.DTOs
{
    public class AuthRequestDTO
    {
        public string UserId { get; set; }
        public string AppId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public string VerificationCode { get; set; }

        public string VerificationStatus { get; set; }

        public string ExternalUserId { get; set; }

        public Token Token { get; set; }

    }

    public class Token
    {
        public string TokenType { get; set; }
        public string IdToken { get; set; }

        public long Expiry { get; set; }
    }
}
