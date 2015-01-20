using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.ThrowableException
{
    public class LocationUnknownException : Exception
    {
        public override string Message
        {
            get
            {
                return "The location is unknown to the game. Initialize the point of the unknown location";
            }
        }
        public override string Source
        {
            get
            {
                return base.Source;
            }
            set
            {
                base.Source = "Please initialize the unknown location";
            }
        }
    }
}
