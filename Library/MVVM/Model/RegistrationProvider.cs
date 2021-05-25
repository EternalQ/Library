using Library.Core;
using System;
using System.Threading.Tasks;

namespace Library.MVVM.Model
{
    class RegistrationProvider : BaseProvider
    {
        public override void SendRequest(string Login, string Password, Action<bool> callback)
        {
            Error = string.Empty;

            var task = Task.Run(() =>
            {
                try
                {
                    User user = FindUser(Login);

                    if (user != null)
                    {
                        Error = "This Login is registered";
                        return;
                    }
                    using (var context = new DatabaseContext())
                    {
                        User newUser = new User(Login, ComputeHash(Password));
                        context.Users.Add(newUser);
                        context.SaveChanges();
                    }
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
            task.Wait();
        }
    }
}
