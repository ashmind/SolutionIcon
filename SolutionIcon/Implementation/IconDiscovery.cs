using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AshMind.IO.Abstractions;
using EnvDTE;

namespace SolutionIcon.Implementation {
    public class IconDiscovery {
        private readonly IFileSystem _fileSystem;

        private static readonly HashSet<string> ImageFileExtensions = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) {
            ".gif",
            ".jpg",
            ".jpeg",
            ".png",
            ".ico"
        };

        private static readonly Regex NameRegex = new Regex(@"^favicon|logo|icon$", RegexOptions.IgnoreCase);

        public IconDiscovery(IFileSystem fileSystem) {
            _fileSystem = fileSystem;
        }

        public IFile FindIcon(Solution solution/*, IEnumerable<string> paths*/) {
            var solutionDirectory = _fileSystem.GetFile(solution.FileName).Directory;

            // .editoricon, not part of any standard. But it should be!
            var editorIcon = solutionDirectory.EnumerateFiles(".editoricon.*").FirstOrDefault();
            if (editorIcon != null && ImageFileExtensions.Contains(editorIcon.Extension))
                return editorIcon;

            // I started using EnvDTE for this, but there wasn't much benefit.
            // This approach does not cover Links and other virtual project items,
            // but I don't see those as essential for solution image.
            return solutionDirectory.EnumerateFiles("*.*", SearchOption.AllDirectories)
                                    .FirstOrDefault(f => ImageFileExtensions.Contains(f.Extension) && NameRegex.IsMatch(Path.GetFileNameWithoutExtension(f.Name)));
        }
    }
}