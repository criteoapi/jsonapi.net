using System;
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
    }
}
