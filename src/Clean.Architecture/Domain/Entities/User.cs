using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clean.Architecture.Domain.Enums;

namespace Clean.Architecture.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.Student;
    }
}