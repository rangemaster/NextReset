using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.ThrowableException
{
    public class ReachedFinnishException : Exception
    {
        public override string Message
        {
            get
            {
                return "You have reached the finnish";
            }
        }
    }
}
