﻿using Main.Screens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            AddButton("Start", StartPage);
            AddButton("Exit", Exit);
        }

        private void AddButton(string text, Action<object, RoutedEventArgs> SelectionPage)
        {
            Button button = new Button();
            button.Content = text;
            button.FontSize = 32;
            button.FontFamily = new System.Windows.Media.FontFamily("Comic Sans MS");
            button.Background = new SolidColorBrush(Colors.Black);
            button.Foreground = new SolidColorBrush(Colors.Brown);
            button.Click += new RoutedEventHandler(SelectionPage);
            _Button_Stackpanel.Children.Add(button);
        }

        #region Buttons
        private void StartPage(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Navigate to start page");
            this._NavigationFrame.Navigate(new Uri("StartWindow.xaml", UriKind.RelativeOrAbsolute));
        }
        private void SettingsPage(object sender, RoutedEventArgs e)
        { }
        private void HelpPage(object sender, RoutedEventArgs e)
        { }
        private void AboutPage(object sender, RoutedEventArgs e)
        { }
        private void Exit(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            SaveAppData();
            Application.Current.Shutdown(0);
        }
        #endregion

        #region Functions
        private void SaveSettings() { }
        private void SaveAppData() { }
        #endregion
    }
}
