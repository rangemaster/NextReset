using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Levels
{
    public class Level2 : ILevel
    {
        internal override int[][] SetLandscape()
        {
            int[] row1 = landscape_row_help(01, 01, 01, 01, 01);
            int[] row2 = landscape_row_help(01, 98, 00, 01, 01);
            int[] row3 = landscape_row_help(01, 01, 00, 99, 01);
            int[] row4 = landscape_row_help(01, 01, 01, 01, 01);
            return landscape_help(row1, row2, row3, row4);
        }
        internal override int[] SetAvailableMethods()
        { return new AvailableMethodBuilder().Right(2).Down(1).Build(); }
        internal override string SetNameOfClass()
        { return "Level 2"; }
    }
}
