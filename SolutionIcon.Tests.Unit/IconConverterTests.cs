using System;
using System.Drawing;
using System.IO;
using System.Linq;
using SolutionIcon.Tests.Unit.Helpers;
using Xunit;
using Xunit.Extensions;
using IconConverter = SolutionIcon.Implementation.IconConverter;

namespace SolutionIcon.Tests.Unit {
    public class IconConverterTests {
        [Theory]
        [InlineData("wikimedia-logo.png")]
        [InlineData("jabbr-apple-touch-icon.png")]
        [InlineData("hat.ico")]
        public void ConvertIcon_ProducesExpectedImage_FromStaticFile(string inputFileName) {
            var actualFileName = inputFileName + ".actual.ico";
            using (var image = (Bitmap) Image.FromFile(ResolveTestPath(inputFileName))) {
                ConvertToIconAndSave(image, actualFileName);
            }

            AssertImages.Equal(
                ResolveTestPath(inputFileName + ".expected.ico"),
                ResolveTestPath(actualFileName)
            );
        }

        [Fact]
        public void ConvertIcon_ProducesExpectedImage_FromGeneratedIcon() {
            var actualFileName = "_generated.actual.ico";
            using (var image = new Bitmap(32, 32)) {
                using (var graphics = Graphics.FromImage(image)) {
                    graphics.FillRectangle(Brushes.Gold, 0, 0, 32, 32);
                }

                ConvertToIconAndSave(image, actualFileName);
            }

            AssertImages.Equal(
                ResolveTestPath("_generated.expected.ico"),
                ResolveTestPath(actualFileName)
            );
        }

        private static void ConvertToIconAndSave(Bitmap image, string fileName) {
            using (var icon = new IconConverter().ConvertToIcon(image, new Size(32, 32)))
            using (var output = File.OpenWrite(ResolveTestPath(fileName))) {
                icon.Save(output);
            }
        }

        private static string ResolveTestPath(string fileName) {
            return TestPathResolver.Resolve(Path.Combine("TestFiles", "IconConverter", fileName));
        }
    }
}
