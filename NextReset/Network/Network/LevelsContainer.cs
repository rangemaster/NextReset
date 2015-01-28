using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Network
{
    public class LevelsContainer : ISerializable
    {
        public List<Tuple<List<string>, string>> Levels { get; set; }
        public LevelsContainer()
            : base()
        {
            Levels = null;
        }
        public LevelsContainer(SerializationInfo info, StreamingContext context)
            : base()
        {
            info.GetValue("Levels", typeof(List<Tuple<List<string>, string>>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Levels", Levels);
        }
    }
}
