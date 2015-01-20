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
            int[] row1 = landscape_row_help(98, 00, 00, 00, 00, 00);
            int[] row2 = landscape_row_help(01, 01, 01, 01, 01, 00);
            int[] row3 = landscape_row_help(00, 00, 00, 00, 01, 00);
            int[] row4 = landscape_row_help(00, 01, 99, 01, 01, 00);
            int[] row5 = landscape_row_help(00, 01, 01, 01, 01, 00);
            int[] row6 = landscape_row_help(00, 00, 00, 00, 00, 00);
            return landscape_help(row1, row2, row3, row4, row5, row6);
        }
        internal override int[] SetAvailableMethods()
        { return new AvailableMethodBuilder().Below5().Build(); }
        internal override string SetNameOfClass()
        { return "Level 3"; }
    }
}
