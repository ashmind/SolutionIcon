using System;
using System.Linq;
using AshMind.IO.Abstractions.Mocks;
using EnvDTE;
using Moq;
using SolutionIcon.Implementation;
using Xunit;
using Xunit.Extensions;

namespace SolutionIcon.Tests.Unit {
    public class IconSelectorTests {
        [Theory]
        [InlineData("Should not match unknown names.", new[] { "xsomething.png" }, null)]
        [InlineData("Should not match unknown extensions.", new[] { "favicon.meh" }, null)]
        [InlineData("Should prefer editoricon.", new[] {  "favicon.ico", ".editoricon.png" }, ".editoricon.png")]
        [InlineData("Should match favicon.", new[] { "favicon.ico" }, "favicon.ico")]
        [InlineData("Should match logo.", new[] { "logo.png" }, "logo.png")]
        public void FindIcon_ReturnsExpectedIconBasedOnName(string description, string[] itemNames, string expected) {
            var solution = new Mock<Solution>();
            solution.SetupGet(x => x.FileName).Returns(Guid.NewGuid().ToString());

            var solutionDirectory = new DirectoryMock("", itemNames.Select(name => new FileMock(name)).ToArray());

            var fileSystem = new FileSystemMock(new FileMock(".sln") {
                FullName = solution.Object.FileName,
                Directory = solutionDirectory
            });

            var best = new IconDiscovery(fileSystem).FindIcon(solution.Object);
            Assert.Equal(expected, best != null ? best.FullName : null);
        }

        //private static ProjectItem MockFileItem(string name) {
        //    var mock = new Mock<ProjectItem>();
        //    mock.Setup(x => x.FileCount).Returns(1);
        //    mock.Setup(x => x.FileNames[1]).Returns(name);
        //    return mock.Object;
        //}
    }
}
