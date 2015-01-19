using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.ThrowableExceptions
{
    public class LevelNotFoundException : Exception
    {
        public override string ToString()
        {
            return "Asked level was not found. Check Level Data.";
        }
    }
}
