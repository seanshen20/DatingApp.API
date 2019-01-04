using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        private readonly DataContext _context;

        public Seed(DataContext context)
        {
            _context = context;
        }

        public void SeedUsers()
        {
            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            foreach (var user in users)
            {
                (user.PasswordHash, user.PasswordSalt) = CreatePasswordHash("password");
                user.Name = user.Name.ToLower();
                //_context.Users.Add(user);
            }

            _context.Users.AddRange(users);
            _context.SaveChanges();
        }

        private (byte[], byte[]) CreatePasswordHash(string password)
        {
            byte[] passwordHash, passwordSalt;
            // hmac implements Idisposible 
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

            return (passwordHash, passwordSalt);

        }
    }
}
