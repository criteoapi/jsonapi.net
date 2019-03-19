using NUnit.Framework;
using JsonApi.Envelope;
using Newtonsoft.Json;
// ReSharper disable InconsistentNaming

namespace JsonApiTests
{
    public class T_Deserialization
    {
        // Set up test documents

        private readonly string _minimalDoc = @"{
}";

        private readonly string _minimalObjectDoc = @"{
    'data': {
        'type': 'person',
        'id': '1'
    }
}";

        private readonly string _minimalAttributeDoc = @"{
    'data': {
        'type': 'person',
        'id': '1',
        'attributes': {
            'firstname': 'Bob'
        }
    }
}";

        private readonly string _fullDoc = @"{
    'links': {
        'self': 'person/1'
    },
    'data': {
        'type': 'person',
        'id': '1',
        'attributes': {
            'firstname': 'Bob'
        }
    },
    'meta': {
        'released': '2019-03-05'
    }
}";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConvertQuotes_CallOnStringWithSingleQuotes_OnlyDoubleQuotesRemain()
        {
            // Arrange, Act, Assert
            var tests = new[] {_minimalDoc, _minimalAttributeDoc, _minimalObjectDoc};
            foreach (string test in tests)
            {
                var s = ConvertQuotes(test);
                Assert.That(s.Length, Is.EqualTo(test.Length));
                Assert.That(s, Does.Not.Contain('\''));
            }
        }

        [Test]
        public void DeserializeObject_MinimalDocument_NoDataLinksMeta()
        {
            // Arrange, Act, Assert
            var test = _minimalDoc;
            var result = JsonConvert.DeserializeObject<ResourceEnvelope<Attributes>>(test);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Links, Is.Null);
            Assert.That(result.Meta, Is.Null);
        }

        [Test]
        public void DeserializeObject_FullDocument_NonNullDataLinksMeta()
        {
            // Arrange, Act, Assert
            var test = _fullDoc;
            ResourceEnvelope<Attributes> result = JsonConvert.DeserializeObject<ResourceEnvelope<Attributes>>(test);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Links, Is.Not.Null);
            Assert.That(result.Links["self"], Is.EqualTo("person/1"));
            Assert.That(result.Meta, Is.Not.Null);
            Assert.That(result.Meta["released"], Is.EqualTo("2019-03-05"));
        }

        [Test]
        public void DeserializeObject_MinimalObject_TypeIdExpected()
        {
            // Arrange, Act, Assert
            var test = _minimalObjectDoc;
            ResourceEnvelope<Attributes> result = JsonConvert.DeserializeObject<ResourceEnvelope<Attributes>>(test);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Id, Is.EqualTo("1"));
            Assert.That(result.Data.Type, Is.EqualTo("person"));
            
        }

        [Test]
        public void DeserializeObject_MinimalAttributeDocument_AttributesFound()
        {
            // Arrange, Act, Assert
            var test = _minimalAttributeDoc;
            ResourceEnvelope<Attributes> result = JsonConvert.DeserializeObject<ResourceEnvelope<Attributes>>(test);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Attributes, Is.Not.Null);
            Assert.That(result.Data.Attributes["firstname"], Is.EqualTo("Bob"));
        }

        [Test]
        public void DeserializeObject_JsonApiPerson_WrappedPersonObject()
        {
            
        }

        // Ensure that all tests run with both single and double quotes
        private static string ConvertQuotes(string s) => s.Replace('\'', '"');
    }

}