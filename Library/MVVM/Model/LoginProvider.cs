using Library.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Library.MVVM.Model
{
    class LoginProvider : BaseProvider
    {
        public User AuthorizedUser;

        public override void SendRequest(string Login, string Password, Action<bool> callback)
        {
            Error = string.Empty;

            var task = Task.Run(() =>
            {
                try
                {
                    User user = FindUser(Login);

                    if (user == null)
                    {
                        Error = "User is not found";
                        return;
                    }
                    if (ComparePasswords(user.Password, ComputeHash(Password)) == false)
                    {
                        Error = "Wrong password";
                        return;
                    }
                    AuthorizedUser = user;
                }
                catch
                {
                    Error = $"Database connection error";
                    return;
                }
            });
            task.ContinueWith((Task t) =>
            {
                bool succes = Error == string.Empty;
                callback.Invoke(succes);
            });
        }

        private bool ComparePasswords(string userPassword, string inputPassword)
        {
            return userPassword == inputPassword;
        }
    }
}
