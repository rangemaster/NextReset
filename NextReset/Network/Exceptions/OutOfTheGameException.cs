using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Exceptions
{
    public class OutOfTheGameException : Exception
    {
        public override string ToString()
        {
            return "Out of the game Exception. This means your doing something out of the game area.";
        }
    }
}
