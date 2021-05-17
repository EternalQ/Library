using Library.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Library.MVVM.Model
{
    #region LoginSettings
    /// <summary>
    /// Storage for login settings 
    /// </summary>
    [Serializable]
    public class LoginSettings
    {
        public LoginSettings() { }
        public LoginSettings(string login, string password, bool isRememberChecked, bool isAutoLoginChecked)
        {
            Login = login;
            Password = password;
            IsRememberChecked = isRememberChecked;
            IsAutoLoginChecked = isAutoLoginChecked;
        }

        public static LoginSettings Default()
        {
            LoginSettings settings = new LoginSettings("", "", false, false);
            return settings;
        }

        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsRememberChecked { get; set; }
        public bool IsAutoLoginChecked { get; set; }
    }
    #endregion

    /// <summary>
    /// Saving some appdata
    /// </summary>
    static class LocalDataSaver
    {
        static private string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static private string dataPath = Path.Combine(appDataPath, "Libra");
        static private string loginSettingsPath = Path.Combine(dataPath, "LoginSettings.xml");

        /// <summary>
        /// Saving login settings in AppData
        /// </summary>
        /// <param name="login">This authorisize login value</param>
        /// <param name="password">This authorisize password value</param>
        /// <param name="isRememberChecked">This authorisize checkbox1 value</param>
        /// <param name="isAutoLoginChecked">This authorisize checkbox2 value</param>
        public static void SaveLoginSettings(string login, string password, bool isRememberChecked, bool isAutoLoginChecked)
        {
            LoginSettings settings = new LoginSettings(login, password, isRememberChecked, isAutoLoginChecked);

            var doc = new XDocument();
            using (var writer = doc.CreateWriter())
            {
                var serializer = new XmlSerializer(typeof(LoginSettings));
                serializer.Serialize(writer, settings);
            }

            doc.Save(loginSettingsPath);
        }

        /// <summary>
        /// Saving login settings in AppData
        /// </summary>
        public static void SaveLoginSettings(LoginSettings settings)
        {
            var doc = new XDocument();
            using (var writer = doc.CreateWriter())
            {
                var serializer = new XmlSerializer(typeof(LoginSettings));
                serializer.Serialize(writer, settings);
            }

            doc.Save(loginSettingsPath);
        }

        /// <summary>
        /// Getting saved or default settings
        /// </summary>
        /// <param name="login">Last authorisize login</param>
        /// <param name="password">Last authorisize password</param>
        /// <param name="isRememberChecked">Last authorisize checkbox1</param>
        /// <param name="isAutologinChecked">Last authorisize checkbox2</param>
        public static void GetLoginSettings(out string login, out string password, out bool isRememberChecked, out bool isAutologinChecked)
        {
            if (!File.Exists(loginSettingsPath))
            {
                login = "";
                password = "";
                isRememberChecked = false;
                isAutologinChecked = false;
            }

            LoginSettings settings;

            var serializer = new XmlSerializer(typeof(LoginSettings));
            using (var fs = new FileStream(loginSettingsPath, FileMode.OpenOrCreate))
            {
                settings = (LoginSettings)serializer.Deserialize(fs);
            }

            login = settings.Login;
            password = settings.Password;
            isRememberChecked = settings.IsRememberChecked;
            isAutologinChecked = settings.IsAutoLoginChecked;
        }

        /// <summary>
        /// Getting saved or default settings
        /// </summary>
        /// <returns>Settings</returns>
        public static LoginSettings GetLoginSettings()
        {
            if (!File.Exists(loginSettingsPath))
            {
                return LoginSettings.Default();
            }

            LoginSettings settings = new LoginSettings();

            var serializer = new XmlSerializer(typeof(LoginSettings));
            using (var fs = new FileStream(loginSettingsPath, FileMode.OpenOrCreate))
            {
                settings = (LoginSettings)serializer.Deserialize(fs);
            }

            return settings;
        }

        public static void SetAutologin(bool value = false)
        {
            LoginSettings settings = GetLoginSettings();
            settings.IsAutoLoginChecked = value;
            SaveLoginSettings(settings);
        }

        static LocalDataSaver()
        {
            Directory.CreateDirectory(dataPath);
            if (!File.Exists(loginSettingsPath))
                SaveLoginSettings(LoginSettings.Default());
        }
    }
}
