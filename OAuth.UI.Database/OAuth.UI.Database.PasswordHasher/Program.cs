using System;
using Microsoft.AspNetCore.Identity;
using OAuth.UI.Database.Repository;

namespace OAuth.UI.Database.PasswordHasher
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("Missing password");

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            Console.WriteLine(passwordHasher.HashPassword(null, args[0]));
        }
    }
}
