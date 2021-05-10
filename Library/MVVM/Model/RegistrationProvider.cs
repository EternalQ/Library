using Library.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                        //foreach(DbEntityValidationResult errors in context.GetValidationErrors())
                        //{
                        //    foreach(DbValidationError err in errors.ValidationErrors)
                        //    {
                        //        MessageBox.Show(err.PropertyName + ": " + err.ErrorMessage);
                        //    }
                        //}
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Error = $"Database connection error";
                    //MessageBox.Show(ex.Message);
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
