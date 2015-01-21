using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Network
{
    [Serializable()]
    public class NetworkPackage : ISerializable
    {
        public string message { get; set; }
        public NetworkPackage()
        {
            message = null;
        }
        public NetworkPackage(SerializationInfo info, StreamingContext context)
            : base()
        {
            message = (string)info.GetString("message");
            Debug.WriteLine("Loading message: " + message);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Debug.WriteLine("Saving message: " + message);
            info.AddValue("message", message);
        }
    }
}
