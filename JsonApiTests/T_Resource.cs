using System.Collections.Generic;
using NUnit.Framework;
using JsonApi.Wrapper;
using JsonApi.Envelope;

namespace JsonApiTests
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class T_Resource
    {
        // building a wrapper manually.
        private Person _person1;
        private Person _person2;

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
        }

        [Test]
        public void Resource_NewResource_IdAndTypeNull()
        {
            Person p = _person2;
            Resource<Person> rp = new Resource<Person>(p);

            Assert.IsNull(rp.Type);
            Assert.IsNull(rp.Id);
        }

        [Test]
        public void Resource_NewResourceEnvelope_IdAndTypeNull()
        {
            Person p = _person2;
            ResourceEnvelope<Person> rep = new ResourceEnvelope<Person>(p);
            Resource<Person> rp = rep.Data;

            Assert.IsNull(rp.Type);
            Assert.IsNull(rp.Id);
        }

        [DatapointSource]
        public IEnumerable<Person> People
        {
            get
            {
                Setup();
                yield return _person2;
                yield return _person1;
            }
        }

        [Theory]
        public void Wrapper_WrapPerson_IdAndTypeSet(Person p)
        {
            Assume.That(p, Is.Not.Null);
            IWrapper wrapper = WrapperBuilder
                .WithServer("http://api.example.com")
                .WithDefaultConfig(null)
                .WithTypeConfig<Person>(x => x.WithId("Id"))
                .Build();

            var rep = wrapper.Wrap(p);
            Resource<Person> rp = rep.Data;

            var expectedType = typeof(Person).Name.ToPlural().ToHyphenCase(); // people
            Assert.That(rp.Type, Is.EqualTo(expectedType));
            var expectedId = p.Id.ToString();
            Assert.That(rp.Id, Is.EqualTo(expectedId));
        }
    }
}