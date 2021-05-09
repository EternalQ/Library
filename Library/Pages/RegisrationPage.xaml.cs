﻿using Library.Windows;
using Library.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Library.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegisrationPage.xaml
    /// </summary>
    public partial class RegisrationPage : Page
    {
        public RegisrationPage()
        {
            InitializeComponent();
            DataContext = new RegistrationViewModel();
            //Createbtn.IsEnabled = false;
        }

        //AuthorizationWindow mainWindow { get => Application.Current.MainWindow as AuthorizationWindow; }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
            
        //}
    }
}
