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
    }
}
