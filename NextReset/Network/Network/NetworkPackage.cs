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
        public List<Tuple<List<string>, string>> Data { get; set; }
        public NetworkPackage(int execute_code)
        {
            ExecuteCode = execute_code;
            Value = -2;
            Message = "";
            Data = new List<Tuple<List<string>, string>>();
        }
        public NetworkPackage(SerializationInfo info, StreamingContext context)
            : base()
        {
            ExecuteCode = info.GetInt32("Code");
            Value = (int)info.GetInt16("Value");
            Message = (string)info.GetString("Message");
            Data = (List<Tuple<List<string>, string>>)info.GetValue("Payload", typeof(List<Tuple<List<string>, string>>));
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
