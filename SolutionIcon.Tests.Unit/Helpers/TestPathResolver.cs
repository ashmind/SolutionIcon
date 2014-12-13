using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AshMind.Extensions;

namespace SolutionIcon.Tests.Unit.Helpers {
    public static class TestPathResolver {
        private static string BasePath = Assembly.GetExecutingAssembly().GetAssemblyFileFromCodeBase().DirectoryName;

        public static string Resolve(string subPath) {
            return Path.Combine(BasePath, subPath);
        }
    }
}
