using JsonApi;
using JsonApi.Envelope;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JsonApiTests
{
    [TestFixture]
    public class T_Serialization
    {
        private Person _person1;
        private Person _person2;
        private ResourceEnvelope<Person> _envelope;

        [SetUp]
        public void Setup()
        {
            // Create a reference loop
            _person1 = new Person
            {
                Id = 1,
                RenameName = "ManagerBob",
            };
            _person2 = new Person()
            {
                Id = 2,
                RenameName = "Joe",
                ReferenceLoopManager = _person1,
                DefaultAddress = new Address()
                {
                    PrivateGetter = "3000 Wildwood Court",
                    City = "Saline",
                    State = State.MI,
                    DefaultValue = 0,
                    DefaultNullable = null,
                    DefaultClass = null
                }
            };
            // _person1.ReferenceLoopManager = _person1;
            var data = new Resource<Person>(_person2)
            {
                Type = "person",
                Id = "1"
            };
            // Create an envelope
            _envelope = new ResourceEnvelope<Person>(data);
            _envelope.Links["self"] = "person/1";
            _envelope.Meta["released"] = "2019-03-05";
        }

        [Test]
        public void PersonSerializesToJson()
        {
            var json = JsonConvert.SerializeObject(_person1, Formatting.None);
            Assert.That(json, Does.Contain(NameValue("id", 1)));
            Assert.That(json, Does.Contain(NameValue("firstname", "ManagerBob")));
        }

        [Ignore("id handling not yet implemented")]
        [Test]
        public void ToString_Attributes_IdRemoved()
        {
            // Arrange, Act, Assert
            var attributes = _envelope.Data.Attributes;
            var json = JsonConvert.SerializeObject(attributes, Formatting.None);
            Assert.That(json, Does.Not.Contain("Id").IgnoreCase);
        }

        [Test]
        public void ToString_FullEnvelope_ReplacedHiddenPrivateNotPublishedToJson()
        {
            // Arrange, Act, Assert
            var json = _envelope.ToString();
            Assert.That(json, Does.Not.Contain("replace").IgnoreCase);
            Assert.That(json, Does.Not.Contain("hidden").IgnoreCase);
            Assert.That(json, Does.Not.Contain("private").IgnoreCase);
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