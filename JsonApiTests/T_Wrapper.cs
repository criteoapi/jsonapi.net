using System;
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
    public class T_Wrapper
    {
        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void Errors_NoErrors_Exception()
        {
            var wrapper = WrapperBuilder.WithServer("").Build();

            Assert.Throws<ArgumentException>(() => wrapper.Errors(new ApiError[0]));
        }

        [Test]
        public void Errors_OneError_IsWrapped()
        {
            var wrapper = WrapperBuilder.WithServer("").Build();
            var apiError = new ApiError("internal", "400", "detail");
            apiError.Source["pointer"] = "it/is/here";
            apiError.Source["header"] = "and/here";
            var envelope = wrapper.Errors(new[] {apiError});

            var e0 = envelope.Errors.ElementAt(0);

            Assert.That(e0.Code, Is.EqualTo("internal"));
            Assert.That(e0.Status, Is.EqualTo("400"));
            Assert.That(e0.Detail, Is.EqualTo("detail"));
            Assert.That(e0.Source, Contains.Key("pointer"));
            Assert.That(e0.Source["header"], Is.EqualTo("and/here"));
        }
    }
}
