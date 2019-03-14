using Newtonsoft.Json;
using NUnit.Framework;

namespace JsonApiTests
{
    [TestFixture]
    public class T_Serialization
    {
        readonly Person _person1 = new Person
        {
            Id = 1,
            Name = "Bob",
        };

        [Test]
        public void PersonSerializesToJson()
        {
            var json = JsonConvert.SerializeObject(_person1, Formatting.None);
            Assert.That(json, Does.Contain(NameValue("Id", 1)));
            Assert.That(json, Does.Contain(NameValue("Name", "Bob")));
        }

        private static string NameValue(string name, object value)
        {
            bool useQuotes = value is string;
            if (useQuotes)
            {
                return $"\"{name}\":\"{value}\"";
            }
            else
            {
                return $"\"{name}\":{value}";
            }
        }
    }
}