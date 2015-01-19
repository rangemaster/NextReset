using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Levels
{
    public class Level3 : ILevel
    {
        internal override int[][] SetLandscape()
        {
            int[] row1 = landscape_row_help(0, 0, 0, 0, 0, 0);
            int[] row2 = landscape_row_help(0, 0, 0, 0, 0, 0);
            int[] row3 = landscape_row_help(0, 0, 0, 0, 0, 0);
            int[] row4 = landscape_row_help(0, 0, 0, 0, 0, 0);
            int[] row5 = landscape_row_help(0, 0, 0, 0, 0, 0);
            int[] row6 = landscape_row_help(0, 0, 0, 0, 0, 0);
            return landscape_help(row1, row2, row3, row4, row5, row6);
        }
        internal override bool[] SetAvailableMethods()
        { return method_help_below10(); }
        internal override string SetNameOfClass()
        { return "Level 3"; }
    }
}
