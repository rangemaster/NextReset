using Network;
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
using System.Windows.Threading;

namespace Main.Screen.Tutorial
{
    /// <summary>
    /// Interaction logic for TutorialPage.xaml
    /// </summary>
    public partial class TutorialPage : PageFunction<String>
    {
        private int _GamePlayInteger = 0;
        private DispatcherTimer _Timer = null;
        public TutorialPage()
        {
            InitializeComponent();
            Init();
            AppSettings.PageSettings(this);
        }
        private void Init()
        {
            #region Field
            AddLegendaFieldLine(AppSettings.Color._You, "This is 'You'. By moving and using skills you have to reach the 'Finish'.");
            AddLegendaFieldLine(AppSettings.Color._Finish, "This is the 'Finish'. You have to reach the finish to compleet the level.");
            AddLegendaFieldLine(AppSettings.Color._Path, "This is the 'Path'. You can walk over the path to reach the finish");
            AddLegendaFieldLine(AppSettings.Color._Wall, "This is a 'Wall'. You cant go through it.");
            AddLegendaFieldLine(AppSettings.Color._Rock, "This is a 'Rock'. You cant walk through it, but with a bomb you can blow it up.");
            AddLegendaFieldLine(AppSettings.Color._Enemy1, "This is 'Enemy1'. You cant walk through it, but whit attack you might defied it."); // TODO END: Check 'enemy 1'
            #endregion

            #region Command
            AddLegendaCommandLine(AppSettings.Directons._Right, "To move in the right direction");
            AddLegendaCommandLine(AppSettings.Directons._Left, "To move in the left direction");
            AddLegendaCommandLine(AppSettings.Directons._Up, "To move in the top direction");
            AddLegendaCommandLine(AppSettings.Directons._Down, "To move in the bottem direction");
            AddLegendaCommandLine(AppSettings.Normal._Attack, "To attack an 'Enemy'"); // TODO END: More description
            AddLegendaCommandLine(AppSettings.Normal._Bomb, "To destroy a 'Rock'."); // TODO END: More description
            AddLegendaCommandLine("Some Button", "Un useable command", false);
            #endregion

            #region Game Use
            AddGameUseLine("Start", "To 'start' the game");
            AddGameUseLine(AppSettings.Command.Pause, "To pause the game, press again to resume.");
            AddGameUseLine("Stop", "To 'stop' the game");
            AddGameUseLine("Undo", "To undo your last command");
            AddGameUseLine("Clear", "To clear the list and undo all the commandlines"); // TODO: Check difference with Reset
            AddGameUseLine("Reset", "To reset the level and undo all the commandlines");
            AddGameUseLine("Slow", "To change the speed of the movement");
            ((_Legenda_StackPanel_GameUse.Children[_Legenda_StackPanel_GameUse.Children.Count - 1] as StackPanel).Children[0] as Button).Click += ShowMediumFast_bn_Click;
            #endregion

            #region Game Play
            InitGamePlay();
            #endregion

            Button button = new Button();
            button.Content = "Back";
            button.Width = 200;
            button.Height = 50;
            button.FontSize = 24;
            button.Click += DoReturn;
            _Legenda_Main.Children.Add(button);
        }
        private void ShowMediumFast_bn_Click(object sender, RoutedEventArgs e)
        {
            Button button = ((_Legenda_StackPanel_GameUse.Children[6] as StackPanel).Children[0] as Button);
            if (button.Content.Equals("Slow")) { button.Content = "Medium"; }
            else if (button.Content.Equals("Medium")) { button.Content = "Fast"; }
            else if (button.Content.Equals("Fast")) { button.Content = "Slow"; }
        }

        #region Add Legenda Field
        private void AddLegendaFieldLine(Brush color, string description)
        {
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Height = 50;
            sp.Width = (int)(_Legenda_StackPanel.Width / 2);
            sp.Margin = new Thickness(1, 1, 1, 1);
            sp.Background = new SolidColorBrush(Colors.LightCyan); // TODO: Merge method to 1 (Create Stackpanel)
            StackPanel rect = new StackPanel();
            rect.Width = 50;
            rect.Height = 50;
            rect.Background = new SolidColorBrush(Colors.Black);
            Rectangle rectangle = new Rectangle();
            rectangle.Width = rect.Width - 4;
            rectangle.Height = rect.Height - 4;
            rectangle.Margin = new Thickness(2, 2, 2, 2);
            rectangle.Fill = color;
            rect.Children.Add(rectangle);
            TextBlock tb = new TextBlock();
            tb.Text = description;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.FontSize = 16;
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            tb.TextAlignment = TextAlignment.Center;
            tb.Width = sp.Width - rect.Width;
            sp.Children.Add(rect);
            sp.Children.Add(tb);
            _Legenda_StackPanel_Field.Children.Add(sp);
        }
        #endregion

        #region Add Legenda Command
        private void AddLegendaCommandLine(string command, string description)
        { AddLegendaCommandLine(command, description, true); }
        private void AddLegendaCommandLine(string command, string description, bool enabled)
        {
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Height = 50;
            sp.Width = (int)(_Legenda_StackPanel.Width / 2);
            sp.Margin = new Thickness(1, 1, 1, 1);
            sp.Background = new SolidColorBrush(Colors.LightCyan); // TODO: Merge method to 1 (Create Stackpanel)
            Button button = new Button();
            button.Content = command + " 1x";
            button.IsEnabled = enabled;
            button.MinHeight = 30;
            button.MinWidth = 100;
            button.Height = button.MinHeight;
            button.Width = button.MinWidth;
            TextBlock tb = new TextBlock();
            tb.Width = sp.Width - button.Width;
            tb.Text = description;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.TextAlignment = TextAlignment.Center;
            tb.FontSize = 16;
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            sp.Children.Add(button);
            sp.Children.Add(tb);
            _Legenda_StackPanel_Command.Children.Add(sp);
        }
        #endregion

        #region Add Game Use
        private void AddGameUseLine(string button_name, string description)
        {
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Height = 50;
            sp.Width = _Legenda_StackPanel.Width;
            sp.Margin = new Thickness(1, 1, 1, 1);
            sp.Background = new SolidColorBrush(Colors.LightCyan);
            Button button = new Button();
            button.Content = button_name;
            button.MinHeight = 30;
            button.MinWidth = 100;
            button.Height = button.MinHeight;
            button.Width = button.MinWidth;
            TextBlock tb = new TextBlock();
            tb.Text = description;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.TextAlignment = TextAlignment.Center;
            tb.FontSize = 16;
            tb.Width = sp.Width - button.Width;
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            sp.Children.Add(button);
            sp.Children.Add(tb);
            _Legenda_StackPanel_GameUse.Children.Add(sp);
        }
        #endregion

        #region GamePlay
        private void InitGamePlay()
        {
            _Legenda_StackPanel_GamePlay1.Orientation = Orientation.Horizontal;
            _Legenda_StackPanel_GamePlay1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            _Legenda_StackPanel_GamePlay1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            InitTimer();
            Reset();
        }
        #region Init Timer
        private void InitTimer()
        {
            _Timer = null;
            _Timer = new DispatcherTimer();
            _Timer.Interval = new TimeSpan(0, 0, 2);
            _Timer.Tick += GamePlayTimer_Tick;
            _Timer.Start();
        }
        #endregion

        #region Timer Tick
        private void GamePlayTimer_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine("Tick: " + _GamePlayInteger);
            switch (_GamePlayInteger)
            {
                case 0: Step01(); Step02(); break;
                case 1: Step1(); break;
                case 2: Step2(); break;
                case 3: Step3(); break;
                case 4: Step4(); break;
                case 5: Step5(); break;
                case 6: Step6(); break;
                case 7: Step7(); break;
                case 8: Step8(); break;
                case 9: Step9(); break;
                case 10: Step10(); break;
                case 11: Step11(); break;
                case 12: Step12(); break;
                default: Reset(); return;
            }
            _GamePlayInteger++;
        }
        #endregion

        #region Reset
        private void Reset()
        {
            _GamePlayInteger = 0;
            _Legenda_StackPanel_GamePlay1.Children.Clear();
            _Legenda_StackPanel_GamePlay2.Children.Clear();
        }
        #endregion

        #region Steps
        private void Step01()
        {
            Button button = CreateButton();
            button.Content = "Right 3x";
            _Legenda_StackPanel_GamePlay1.Children.Add(button);
        }
        private void Step02()
        {
            StackPanel sp1 = new StackPanel();
            sp1.Orientation = Orientation.Horizontal;
            sp1.Children.Add(WallRect());
            sp1.Children.Add(WallRect());
            sp1.Children.Add(WallRect());
            sp1.Children.Add(WallRect());
            sp1.Children.Add(WallRect());
            sp1.Children.Add(WallRect());
            StackPanel sp2 = new StackPanel();
            sp2.Orientation = Orientation.Horizontal;
            sp2.Children.Add(WallRect());
            sp2.Children.Add(YouRect());
            sp2.Children.Add(PathRect());
            sp2.Children.Add(PathRect());
            sp2.Children.Add(FinishRect());
            sp2.Children.Add(WallRect());
            StackPanel sp3 = new StackPanel();
            sp3.Orientation = Orientation.Horizontal;
            sp3.Children.Add(WallRect());
            sp3.Children.Add(WallRect());
            sp3.Children.Add(WallRect());
            sp3.Children.Add(WallRect());
            sp3.Children.Add(WallRect());
            sp3.Children.Add(WallRect());
            _Legenda_StackPanel_GamePlay2.Children.Add(sp1);
            _Legenda_StackPanel_GamePlay2.Children.Add(sp2);
            _Legenda_StackPanel_GamePlay2.Children.Add(sp3);
        }
        private void Step1()
        {
            TextBlock tb = CreateTextblock();
            tb.Text = "<-- Press the button";
            _Legenda_StackPanel_GamePlay1.Children.Add(tb);
        }
        private void Step2()
        {
            (_Legenda_StackPanel_GamePlay1.Children[0] as Button).Content = "Right 2x";
            (_Legenda_StackPanel_GamePlay1.Children[1] as TextBlock).Text = "";
        }
        private void Step3()
        {
            (_Legenda_StackPanel_GamePlay1.Children[1] as TextBlock).Text = "Move was added -->";
            TextBlock tb = new TextBlock();
            tb.Text = "[0001] Right";
            StackPanel sp = new StackPanel();
            sp.Children.Add(tb);
            _Legenda_StackPanel_GamePlay1.Children.Add(sp);
        }
        private void Step4()
        {
            (_Legenda_StackPanel_GamePlay1.Children[0] as Button).Content = "Right 1x";
            (_Legenda_StackPanel_GamePlay1.Children[1] as TextBlock).Text = "";
            TextBlock tb = new TextBlock();
            tb.Text = "[0002] Right";
            (_Legenda_StackPanel_GamePlay1.Children[2] as StackPanel).Children.Add(tb);
        }
        private void Step5()
        {
            (_Legenda_StackPanel_GamePlay1.Children[0] as Button).Content = "Right 0x";
            (_Legenda_StackPanel_GamePlay1.Children[0] as Button).IsEnabled = false;
            TextBlock tb = new TextBlock();
            tb.Text = "[0003] Right";
            (_Legenda_StackPanel_GamePlay1.Children[2] as StackPanel).Children.Add(tb);
        }
        private void Step6()
        {
            (_Legenda_StackPanel_GamePlay1.Children[0] as Button).Content = "Start";
            (_Legenda_StackPanel_GamePlay1.Children[0] as Button).IsEnabled = true;
            (_Legenda_StackPanel_GamePlay1.Children[1] as TextBlock).Text = "Press 'Start'";
        }
        private void Step7()
        {
            (_Legenda_StackPanel_GamePlay1.Children[0] as Button).IsEnabled = false;
        }
        private void Step8()
        {
            TextBlock tb = ((_Legenda_StackPanel_GamePlay1.Children[2] as StackPanel).Children[0] as TextBlock);
            tb.Foreground = new SolidColorBrush(Colors.Red);
            SetRectangle(1, 1, AppSettings.Color._Path);
            SetRectangle(2, 1, AppSettings.Color._You);
        }
        private void Step9()
        {
            TextBlock tb = ((_Legenda_StackPanel_GamePlay1.Children[2] as StackPanel).Children[1] as TextBlock);
            tb.Foreground = new SolidColorBrush(Colors.Red);
            SetRectangle(2, 1, AppSettings.Color._Path);
            SetRectangle(3, 1, AppSettings.Color._You);
        }
        private void Step10()
        {
            TextBlock tb = ((_Legenda_StackPanel_GamePlay1.Children[2] as StackPanel).Children[2] as TextBlock);
            tb.Foreground = new SolidColorBrush(Colors.Red);
            SetRectangle(3, 1, AppSettings.Color._Path);
            SetRectangle(4, 1, AppSettings.Color._You);
        }
        private void Step11()
        {
            _Legenda_StackPanel_GamePlay1.Children.Clear();
            TextBlock tb = CreateTextblock();
            tb.Text = "Reseave message: " + AppSettings.Messages.UserFeedback.ReachedFinish;
            _Legenda_StackPanel_GamePlay1.Children.Add(tb);
        }
        private void Step12()
        {
            (_Legenda_StackPanel_GamePlay1.Children[0] as TextBlock).Text = "You will return to the selection page";
        }
        #endregion

        #region Create
        private TextBlock CreateTextblock()
        {
            TextBlock tb = new TextBlock();
            tb.Width = 300;
            tb.Height = 50;
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            tb.TextAlignment = TextAlignment.Center;
            return tb;
        }
        private Button CreateButton()
        {
            Button button = new Button();
            button.Width = 100;
            button.Height = 30;
            button.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            button.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            return button;
        }
        private Rectangle WallRect() { return CreateRectangle(AppSettings.Color._Wall); }
        private Rectangle PathRect() { return CreateRectangle(AppSettings.Color._Path); }
        private Rectangle YouRect() { return CreateRectangle(AppSettings.Color._You); }
        private Rectangle FinishRect() { return CreateRectangle(AppSettings.Color._Finish); }
        private Rectangle CreateRectangle(Brush brush)
        {
            Rectangle rect = new Rectangle();
            rect.Width = 25;
            rect.Height = 25;
            rect.Margin = new Thickness(1, 1, 1, 1);
            rect.Fill = brush;
            return rect;
        }
        #endregion

        #region Set
        private void SetRectangle(int x, int y, Brush brush)
        {
            Rectangle rect = ((_Legenda_StackPanel_GamePlay2.Children[y] as StackPanel).Children[x] as Rectangle);
            rect.Fill = brush;
        }
        #endregion
        #endregion

        private void DoReturn(object sender, RoutedEventArgs e)
        {
            if (_Timer != null)
                _Timer.Stop();
            _Timer = null;
            OnReturn(new ReturnEventArgs<string>(AppSettings.Return.Succes));
        }
    }
}
