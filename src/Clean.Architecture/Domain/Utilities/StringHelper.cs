using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clean.Architecture.Domain.Utilities
{
    public static class StringHelper
    {
        public static string Hash(this string inputString)
            => BCrypt.Net.BCrypt.HashPassword(inputString);

        public static bool Verify(string Pass, string oldPass)
            => BCrypt.Net.BCrypt.Verify(Pass, oldPass);
    }
}