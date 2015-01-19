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
            public static Brush _Path = new SolidColorBrush(Colors.Blue);
            public static Brush _Rock = new SolidColorBrush(Colors.Brown);
            public static Brush _Wall = new SolidColorBrush(Colors.Gray);
            public static Brush _You = new SolidColorBrush(Colors.Red);
            public static Brush _Finnish = new SolidColorBrush(Colors.YellowGreen);
        }
    }
}
