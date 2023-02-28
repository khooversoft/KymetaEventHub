using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Kymeta.Cloud.Services.Toolbox.Extensions;

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests.Extensions;

public class StringExtensionTests
{
    [Theory]
    [InlineData(null, null, true)]
    [InlineData(null, "", false)]
    [InlineData("", null, false)]
    [InlineData("", "", true)]
    [InlineData("ABC", "ABC", true)]
    [InlineData("ABC", "abc", true)]
    [InlineData("abc", "abc", true)]
    [InlineData("abcd", "abc", false)]
    public void TestEqualIgnoreCase(string? subject, string? compareTo, bool expected)
    {
        bool test = subject.EqualsIgnoreCase(compareTo);
        test.Should().Be(expected);
    }
}
