using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Entities.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? PasswordHash { get; set; }

        public string? EmailAddress { get; set; }

        public string? Rol { get; set; }

        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public int Age { get; set; }





    }
}
