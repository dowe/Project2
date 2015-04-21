using Newtonsoft.Json;
using System;

namespace CommunicationCommon
{
    public abstract class Command
    {

        private Guid id = Guid.Empty;
        private string commandTypeString = null;

        public Command(Guid id)
        {
            this.id = id;
            commandTypeString = GetType().ToString();
        }

        public Command() : this (Guid.NewGuid())
        {
        }

        public string CommandTypeString
        {
            get
            {
                return commandTypeString;
            }
        }

        [JsonProperty]
        public Guid Id
        {
            get
            {
                return id;
            }
            protected set // This is necessary for the JSON Deserialization.
            {
                id = value;
            }
        }

    }
}
