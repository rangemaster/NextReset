using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Network
{
    public class AppSettings
    {
        public static int _MinimumAmountOfAvailableMethods = 4;
        public static int _MaximumAmountOfAvailableMethods = 6;
        public class Color
        {
            public static Brush _Path = new SolidColorBrush(Colors.LightGoldenrodYellow);
            public static Brush _Rock = new SolidColorBrush(Colors.Brown);
            public static Brush _Wall = new SolidColorBrush(Colors.Gray);
            public static Brush _Enemy1 = new SolidColorBrush(Colors.Pink);
            public static Brush _You = new SolidColorBrush(Colors.GreenYellow);
            public static Brush _Finish = new SolidColorBrush(Colors.Gold);
            public static Brush _Unknown = new SolidColorBrush(Colors.Orange);
        }
        public class ButtonLevelColor
        {
            public static Brush _Compleet = new SolidColorBrush(Colors.YellowGreen);
            public static Brush _NotCompleet = new SolidColorBrush(Colors.LightCyan);
        }
        public sealed class Field
        {
            public const int _Path = 0;
            public const int _Wall = 1;
            public const int _Rock = 2;
            public const int _Enemy1 = 50;
            public const int _You = 98;
            public const int _Finnish = 99;
        }
        public sealed class Directons
        {
            public const string _Right = "Right";
            public const string _Left = "Left";
            public const string _Up = "Up";
            public const string _Down = "Down";
            public static string[] _All = new string[] { _Right, _Left, _Up, _Down };
        }
        public sealed class Normal
        {
            public const string _Attack = "Attack";
            public const string _Bomb = "Bomb";
        }
        public sealed class Login
        {
            public const string _Username_tx = "Username:";
            public const string _Password_tx = "Password:";
            public static Brush _Username_label_brush = new SolidColorBrush(Colors.Green);
            public static Brush _Username_tb_brush = new SolidColorBrush(Colors.Black);
            public static Brush _Password_label_brush = new SolidColorBrush(Colors.Green);
            public static Brush _Password_tb_brush = new SolidColorBrush(Colors.Black);
            public static Brush _Feedback_tb_brush = new SolidColorBrush(Colors.Magenta);
            public const string _CreateData = "Create Data";
            public const string _LoginData = "Login Data";
            public const string _LoginWrong = "Wrong Username or password";
            public const string _LoginFailed = "Login Failed";
        }
        public sealed class Messages
        {
            public sealed class Errors
            {
                public const string SetLevelName_ArgumentNull = "The name of the level is unknown";
                public const string SetLandscape_ArgumentNull = "The tiles of the landscape are NULL";
                public const string SetLandscape_Format = "The array of tiles for the landscape is lower then 9. This is to low for a usefull landscape";
                public const string SetAvailableMethods_ArgumentNull = "The array of available methods is NULL";
                public const string SetAvailableMethods_Format = "The array of available methods is to small";
                public const string SetLevelsToPlay = "LevelsToPlay are allready defind";
                public const string SetLevelsCompleted = "LevelsCompleted are allready defind";
            }
            public sealed class UserFeedback
            {
                public const string Loading = "Loading game...";
                public const string UnableToLoad = "Could not load level!";
                public const string Saving = "Saving game...";
                public const string None = "";
                public const string ReachedFinish = "You have reached the finish";
                public const string OutOfArea = "Out Of The Game Error";
                public const string AdminChanges = "Good luck with these changes";
            }
        }
        public sealed class MessageBox
        {
            public sealed class StartPage_Admin
            {
                public const string _Line = "You want to continue";
                public const string _Title = "Admin";
            }
            public sealed class UpdateCheck
            {
                public const string _Line = "You want to get the update?";
                public const string _Title = "Up - To - Date";
                public const string _No_Update = "No Update Available";
            }
            public sealed class FirstTime
            {
                public const string _Line = "Is this your first time?";
                public const string _Title = "Question";
            }
            public sealed class Confirmation
            {
                public const string _User = "User";
                public const string _Pass = "Pass";
                public const string _Confirm = "Correct?";
                public static string _WrongPackage(int code)
                { return "You have received the wrong networkpackage. Contact the app developer. [" + code + "]"; }
                public const string _WrongTitle = "Wrong network package";
            }
        }
        public sealed class Seperate
        {
            public const string LvlName = "Level name:";
            public const string MethodNames = "Methods:";
            public const string PathStart = "Path [";
            public const string PathEnd = "]";
            public const char PathSplit = ',';
            public const char MethodSplit = ',';
            public const string Exclude = "##";
            public const string End = "END";
        }
        public sealed class SaveOrLoad
        {
            public const string _State_Filename = "State.reset";
            public const string _Level_Source_Location = "GameData";
            public const string _Level_Source_Filename = "GameData.reset";
            public const string _Version_Filename = "Version.reset";
            public const string _Version_Default = "Version: 1.0.0";
        }
        public sealed class Timer
        {
            public const int _Slow = 500;
            public const int _Medium = 250;
            public const int _Fast = 100;
        }
        public sealed class Command
        {
            public const string Pause = "Pause";
            public const string Resume = "Resume";
            public const string UncompleetAll = "Uncompleet All";
            public const string CompleetAll = "Compleet All";
            public const string Uncompleet = "Uncompleet";
            public const string Compleet = "Compleet";
            public const string Unknown = "--- Unknown Command ---";
        }
        public sealed class Return
        {
            public const string Succes = "Succes";
            public const string NoSucces = "No Succes";
        }
        public sealed class ServerSettings
        {
            public sealed class Buttons
            {
                public static Brush _Active = new SolidColorBrush(Colors.LightGreen);
                public static Brush _InActive = new SolidColorBrush(Colors.Salmon);
            }
        }
        public static void PageSettings(Page page)
        {
            page.ShowsNavigationUI = false;
            page.Background = new SolidColorBrush(Colors.LightGray);
        }
    }
}
