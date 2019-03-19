using NUnit.Framework;
using JsonApi.Wrapper;

namespace JsonApiTests
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class T_WrapperBuilder
    {
        const string HttpsExampleCom = "https://example.com";

        [Test]
        public void WrapperBuilder_MinimalBuilder_ServerPathSet()
        {
            var wrapper = WrapperBuilder.WithServer(HttpsExampleCom).Build();

            Assert.That(wrapper.ServerPath, Is.EqualTo(HttpsExampleCom));
        }

        [Test]
        public void WithDefaultConfig_AllPolicyAsserts_NoException()
        {
            Wrapper wrapper = WrapperBuilder.WithServer(HttpsExampleCom).WithDefaultConfig(p =>
            {
                p.CamelCasedNames();
                p.HideDefaults();
                p.HyphenCasedTypes();
                p.PluralTypes();
                p.WithCanonicalLinks();
                p.WithSelfLinks();
            }).Build();

            Assert.That(wrapper.ServerPath, Is.EqualTo(HttpsExampleCom));
        }
    }
}