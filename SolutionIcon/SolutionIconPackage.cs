using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using AshMind.IO.Abstractions.Adapters;
using EnvDTE;
using SolutionIcon.Implementation;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace SolutionIcon {
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideAutoLoad(UIContextGuids.SolutionExists)]
    [Guid(Guids.PackageString)]
    public sealed class SolutionIconPackage : Package {
        private SolutionIconManager _manager;

        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public SolutionIconPackage() {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this));
        }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize() {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this));
            base.Initialize();
            
            var logger = new ExtensionLogger("SolutionIcon", title => GetOutputPane(Guids.OutputPane, title));
            _manager = new SolutionIconManager(
                (DTE)GetService(typeof(DTE)),
                new IconFinder(new FileSystem()), 
                new IconConverter(),
                new IconGenerator(new TinyIdGenerator()), 
                logger
            );
        }
    }
}