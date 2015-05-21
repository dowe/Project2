using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Common.Communication
{
    public class JsonCommandSerializer : ICommandSerializer
    {

        public string SerializeCommand(Command command)
        {
            return JsonConvert.SerializeObject(command);
        }

        public Command DeserializeCommandOrNull(string jsonCommand, IEnumerable<Type> parsableCommandTypes)
        {
            string commandTypeString = ParseSerializedCommandType(jsonCommand);
            Type matchingType = null;
            foreach (Type t in parsableCommandTypes)
            {
                if (t.ToString().Equals(commandTypeString))
                {
                    matchingType = t;
                }
            }

            Command deserialized = null;
            if (matchingType != null)
            {
                deserialized = (Command)JsonConvert.DeserializeObject(jsonCommand, matchingType);
            }

            return deserialized;
        }

        private string ParseSerializedCommandType(string jsonCommand)
        {
            JObject jObject = JObject.Parse(jsonCommand);
            return jObject.GetValue("CommandTypeString").Value<string>();
        }

    }
}
