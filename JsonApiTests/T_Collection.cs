using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JsonApi.Envelope;
using JsonApi.Wrapper;
using NUnit.Framework;

/* Serialization
 * - links
 *   - to self
 *   - canonical
 * - meta
 *   - creation date
 * - type tests
 *   - hyphenation (e.g. sellerCampaigns, SellerCampaign)
 *   - non-hyphen, non-plural cases
 * - id tests
 *   - compound id values (sellerId + . + campaignId)
 *   - id regexp matching (e.g ends with)
 *   - nullable id value
 * - attributes
 *   - nested structures
 *   - attribute name mapping?
 *   - suppression of null values or default values
 *
 * Deserialization
 * - type used correctly
 * - id properly used
 * - attributes converted to type correctly by JSON.NET
 * - error return codes
 *   - good error messages (status, source, detail)
 *   - correct response codes? 
 *   
 */

namespace JsonApiTests
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class T_Collection
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
        public void WrapAll_EmptyCollection_CountZero()
        {
            Person[] people = new Person[] {};

            IWrapper wrapper = WrapperBuilder.WithServer("http://api.example.com").Build();
            var collection = wrapper.WrapAll<Person>(people);
            collection.Meta["count"] = 0;

            Assert.That(collection.Data.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WrapAll_SmallCollection_TypeAndIdMatches()
        {
            Person[] people = { _person1, _person2 };

            IWrapper wrapper = WrapperBuilder.WithServer("http://api.example.com").Build();
            var collection = wrapper.WrapAll<Person>(people);

            var e0 = collection.Data.ElementAt(0);
            var e1 = collection.Data.ElementAt(1);
            Assert.That(e0.Type, Is.EqualTo("people"));
            Assert.That(e0.Id, Is.EqualTo("1"));
            Assert.That(e1.Type, Is.EqualTo("people"));
            Assert.That(e1.Id, Is.EqualTo("2"));
        }

        [Test]
        public void WrapAll_SmallCollection_CountMatches()
        {
            Person[] people = { _person1, _person2 };

            IWrapper wrapper = WrapperBuilder.WithServer("http://api.example.com").Build();
            var collection = wrapper.WrapAll<Person>(people);
            collection.Meta["count"] = people.Length;

            Assert.That(collection.Data.Count(), Is.EqualTo(2));
        }

        [Test]
        public void WrapAll_SmallCollection_WrappingIsDeferred()
        {
            IEnumerable<Person> People()
            {
                yield return _person1;
                yield return _person2;
                throw new Exception();
            }

            IWrapper wrapper = WrapperBuilder.WithServer("http://api.example.com").Build();
            var collection = wrapper.WrapAll<Person>(People());
            collection.Meta["count"] = 3;

            using (var e = collection.Data.GetEnumerator())
            {
                if (!e.MoveNext()) Assert.Fail("Collection too small");
                if (!e.MoveNext()) Assert.Fail("Collection too small");
                Assert.Throws<Exception>(() => e.MoveNext());
            }
        }
    }
}
