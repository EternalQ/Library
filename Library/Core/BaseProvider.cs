using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Library.MVVM.Model;

namespace Library.Core
{
    abstract class BaseProvider
    {
        public string Error { get; set; }

        public abstract void SendRequest(string Login, string Password, Action<bool> callback);

        public string ComputeHash(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashConverter = new StringBuilder(128);

            using (var hash = SHA512.Create())
            {
                var hashedBytes = hash.ComputeHash(inputBytes);

                foreach (byte b in hashedBytes)
                {
                    hashConverter.Append(b.ToString("X2"));
                }
            }
            return hashConverter.ToString();
        }

        public User FindUser(string Login)
        {
            using (var context = new DatabaseContext())
            {
                return context.Users.FirstOrDefault(u => u.Login.ToLower() == Login.ToLower());
            }
        }
    }
}
