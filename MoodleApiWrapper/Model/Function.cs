using Newtonsoft.Json;

using System;

namespace MoodleApiWrapper
{
    public class Function : ICloneable
    {
        public string name { get; set; }
        public string version { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        [JsonConstructor]
        internal Function(string name, string version)
        {
            this.name = name;
            this.version = version;
        }
    }
}