using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit.Sdk;

namespace SolutionIcon.Tests.Unit.Helpers {
    public static class AssertImages {
        public static void Equal(string expectedPath, string actualPath) {
            var expectedBytes = File.ReadAllBytes(expectedPath);
            var actualBytes = File.ReadAllBytes(actualPath);
            if (!expectedBytes.SequenceEqual(actualBytes))
                throw new AssertException("Image '" + expectedPath + "' was not equal to '" + actualPath + "'.");
        }
    }
}
