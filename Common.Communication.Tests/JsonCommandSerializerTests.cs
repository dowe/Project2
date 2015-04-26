using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Communication.Tests
{
    [TestClass]
    public class JsonCommandSerializerTests
    {

        private JsonCommandSerializer CreateTestee()
        {
            return new JsonCommandSerializer();
        }

        [TestMethod]
        public void Test_Deserialize_AfterSerializing_ReturnsCopyOfOriginalCommand()
        {
            JsonCommandSerializer testee = CreateTestee();
            DummyCommand command = new DummyCommand(Guid.NewGuid());
            List<Type> deserializableTypes = new List<Type>() { typeof(DummyCommand) };
            command.Value = "Look at me I am a value.";

            string serializedCommand = testee.SerializeCommand(command);
            DummyCommand deserializedCommand = (DummyCommand) testee.DeserializeCommand(serializedCommand, deserializableTypes);

            Assert.AreEqual(command, deserializedCommand);
        }

        private class DummyCommand : Command
        {

            public string Value { get; set; }

            public DummyCommand(Guid id) : base(id)
            {
            }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                DummyCommand other = (DummyCommand) obj;
                return other.Id.Equals(Id) && other.Value.Equals(Value);
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode() + Value.GetHashCode();
            }
        }
    }
}
