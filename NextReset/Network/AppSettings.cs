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
    }
}
