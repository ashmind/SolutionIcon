using System;
using System.Collections.Generic;
using System.Linq;
using SolutionIcon.Implementation;
using Xunit;
using Xunit.Extensions;

namespace SolutionIcon.Tests.Unit {
    public class TinyIdGeneratorTests {
        [Theory]
        [InlineData("SolutionIcon", "SI")]
        [InlineData("Company.Product.MyLibrary", "ML")]
        [InlineData("Something", "S")]
        [InlineData("o", "o")]
        [InlineData("oO", "oO")]
        [InlineData("o_O", "oO")]
        [InlineData("lowercase", "lo")]
        public void GetTinyId_ReturnsExpectedId(string name, string expectedId) {
            Assert.Equal(expectedId, new TinyIdGenerator().GetTinyId(name));
        }
    }
}
