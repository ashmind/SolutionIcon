using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;

namespace SolutionIcon.Implementation {
    public static class DteExtensions {
        public static string GetName(this Solution solution) {
            return Path.GetFileNameWithoutExtension(solution.FullName);
        }
    }
}
