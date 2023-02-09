using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests
{
    public class HelperTests
    {
        public HelperTests()
        {

        }

        [Fact]
        public void TryGetValue_Succeeds()
        {
            var testDictionary = new Dictionary<string, string>
            {
                { "abc", "123" }
            };

            var value = Services.Helpers.GetValue(testDictionary, "abc");

            Assert.Equal("123", value);
        }

        [Fact]
        public void TryGetValue_Defaults()
        {
            var testDictionary = new Dictionary<string, string>
            {
                { "abc", "123" }
            };

            var value = Services.Helpers.GetValue(testDictionary, "abcd", "def");

            Assert.Equal("def", value);
        }
    }
}
