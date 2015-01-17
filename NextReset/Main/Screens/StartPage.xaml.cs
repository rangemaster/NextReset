using Main.Screens.Game;
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
using System.Windows.Shapes;

namespace Main.Screens
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            List<StackPanel> panels = new List<StackPanel>();
            for (int i = 0; i < 20; i++)
            {
                if (i % 5 == 0)
                {
                    panels.Add(CreateSelectionStackpanel());
                }
                Button button = new Button();
                button.Content = "Level " + (i + 1);
                button.Margin = new Thickness(10, 10, 10, 10);
                button.Click += button_Click;
                panels[i / 5].Children.Add(button);
            }
            foreach (StackPanel sp in panels)
            { _Buttons_Stackpanel.Children.Add(sp); }
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            GoToSingleGame("Test");
        }
        private StackPanel CreateSelectionStackpanel()
        {
            StackPanel sp = new StackPanel();
            sp.Width = 100;
            return sp;
        }
        private void GoToSingleGame(string text)
        {
            SingleGamePage page = new SingleGamePage();
            this.NavigationService.Navigate(page, text);
        }
    }
}
