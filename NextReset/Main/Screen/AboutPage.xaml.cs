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

namespace Main.Screens
{
    /// <summary>
    /// Interaction logic for AboutPage.xaml
    /// </summary>
    public partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();
            InitTitle();
            InitAboutText();
            InitBackButton();
        }
        private void InitTitle()
        {
            TextBlock tb = new TextBlock();
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            tb.Text = "About";
            tb.FontSize = 32;
            Top_st.Children.Add(tb);
        }
        private void InitAboutText()
        {
            TextBlock tb = new TextBlock();
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            tb.Text = CreateAboutText();
            tb.FontSize = 16;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Width = Center_st.Width;
            Center_st.Children.Add(tb);
        }
        private void InitBackButton()
        {
            Button button = new Button();
            button.Content = "Back";
            button.Click += Back_bn_Click;
            button.Width = 100;
            this.Bottom_st.Children.Add(button);
        }

        #region Button Functions
        private void Back_bn_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            { this.NavigationService.GoBack(); }
        }
        #endregion
        #region Create
        private string CreateAboutText()
        {
            string text = "";
            text += "This game is made and developed by Roel Suntjens.\n";
            text += "This game makes use from a server to update the levels if there are new ones.\n";
            text += "To use this game you need te have an account.\n";
            text += "This game was made to inspire people to do/learn programming easly.\n";
            text += "\nVersions:\n";
            text += VersionControl();
            return text;
        }
        private string VersionControl()
        {
            string versions = "";
            versions += Version("v1.0", "29-01-2015");
            versions += Version("v1.1", "03-02-2015");
            return versions;
        }
        private string Version(string versionnr, string versionDate)
        { return versionnr + "\t" + versionDate + "\n"; }
        #endregion
    }
}
