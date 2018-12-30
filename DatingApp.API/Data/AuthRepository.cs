using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<User> Login(string name, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name);

            if (user == null) return null;
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) return null;
            return user;

        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }

            }

            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            (user.PasswordHash, user.PasswordSalt) = CreatePasswordHash(password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }


        public async Task<bool> UserExists(string name)
        {
            return await _context.Users.AnyAsync(u => u.Name == name);
        }

        private  (byte[], byte[]) CreatePasswordHash(string password)
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
