using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using SolutionIcon.Properties;

[assembly: AssemblyTitle("SolutionIcon")]
[assembly: AssemblyCompany("Andrey Shchekin")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(false)]
[assembly: NeutralResourcesLanguage("en-US")]

[assembly: AssemblyVersion(AssemblyInfo.VersionString)]
[assembly: AssemblyFileVersion(AssemblyInfo.VersionString)]
[assembly: AssemblyInformationalVersion(AssemblyInfo.VersionString)]

namespace SolutionIcon.Properties {
    internal static class AssemblyInfo {
        // Please keep in sync with vsixmanifest
        public const string VersionString = "1.1.0";
    }
}