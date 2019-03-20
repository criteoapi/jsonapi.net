using NUnit.Framework;
using JsonApi.Wrapper;
using JsonApi.Envelope;

namespace JsonApiTests
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class T_Resource
    {
        // building a wrapper manually
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
                    PrivateGetter = "3000 Infinity Court",
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
                Type = "people",
                Id = "2",
            };
            // Create an envelope
            _envelope = new ResourceEnvelope<Person>(data);
        }

        [Test]
        public void Resource_WrapPerson_IdAndTypeNull()
        {
            Person p = _person2;
            Resource<Person> rp = new Resource<Person>(p);

            Assert.IsNull(rp.Type);
            Assert.IsNull(rp.Id);
        }

        [Test]
        public void Wrapper_WrapPerson_IdAndTypeSet()
        {
            Person p = _person2;
            ResourceEnvelope<Person> rep = new ResourceEnvelope<Person>(p);
            Resource<Person> rp = rep.Data;

            Assert.That(rp.Type, Is.EqualTo("people"));
            Assert.That(rp.Id, Is.EqualTo("2"));
        }
    }
}