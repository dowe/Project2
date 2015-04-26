using Newtonsoft.Json;
using System;

namespace Common.Communication
{
    public abstract class Command
    {

        [JsonProperty]
        public Guid Id { get; protected set; }
        [JsonProperty]
        protected string CommandTypeString { get; private set; }

        public Command(Guid id)
        {
            Id = id;
            CommandTypeString = GetType().ToString();
        }

        public Command() : this (Guid.NewGuid())
        {
        }

    }
}
