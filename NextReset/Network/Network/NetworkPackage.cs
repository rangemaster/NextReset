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
        public int ExecuteCode { get; set; }
        public int Value { get; set; }
        public string Message { get; set; }
        public List<string> Data { get; set; }
        public NetworkPackage()
        {
            ExecuteCode = -1;
            Value = -2;
            Message = null;
            Data = null;
        }
        public NetworkPackage(SerializationInfo info, StreamingContext context)
            : base()
        {
            ExecuteCode = info.GetInt32("Code");
            Value = (int)info.GetInt16("Value");
            Message = (string)info.GetString("Message");
            Data = (List<string>)info.GetValue("Payload", typeof(List<string>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Code", ExecuteCode);
            info.AddValue("Value", Value);
            info.AddValue("Message", Message);
            info.AddValue("Payload", Data);
        }
    }
}
