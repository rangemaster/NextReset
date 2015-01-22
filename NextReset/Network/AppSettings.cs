using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            public static Brush _You = new SolidColorBrush(Colors.Red);
            public static Brush _Finnish = new SolidColorBrush(Colors.YellowGreen);
            public static Brush _Unknown = new SolidColorBrush(Colors.Cyan);
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
        public sealed class Messages
        {
            public sealed class Errors
            {
                public const string SetLevelName_ArgumentNull = "The name of the level is unknown";
                public const string SetLandscape_ArgumentNull = "The tiles of the landscape are NULL";
                public const string SetLandscape_Format = "The array of tiles for the landscape is lower then 9. This is to low for a usefull landscape";
                public const string SetAvailableMethods_ArgumentNull = "The array of available methods is NULL";
                public const string SetAvailableMethods_Format = "The array of available methods is to small";
            }
            public sealed class Feedback
            {
                public const string Loading = "Loading game...";
                public const string UnableToLoad = "Could not load level!";
                public const string Saving = "Saving game...";
                public const string None = "";
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
            public const string _Level_Source_Location= "GameData";
            public const string _Level_Source_Filename = "GameData.reset";
        }
    }
}
