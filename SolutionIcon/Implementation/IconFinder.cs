using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AshMind.IO.Abstractions;
using EnvDTE;
using JetBrains.Annotations;

namespace SolutionIcon.Implementation {
    public class IconFinder {
        private readonly IFileSystem _fileSystem;

        private static readonly HashSet<string> ImageFileExtensions = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) {
            ".gif",
            ".jpg",
            ".jpeg",
            ".png",
            ".ico"
        };

        private static readonly Regex NameRegex = new Regex(@"^(favicon|logo|icon)$|^apple-touch-icon", RegexOptions.IgnoreCase);

        public IconFinder(IFileSystem fileSystem) {
            _fileSystem = fileSystem;
        }

        [CanBeNull]
        public IFile FindIcon([NotNull] Solution solution) {
            // ReSharper disable once PossibleNullReferenceException
            var solutionDirectory = _fileSystem.GetFile(solution.FileName).Directory;

            // .editoricon, not part of any standard. But it should be!
            // ReSharper disable once PossibleNullReferenceException
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