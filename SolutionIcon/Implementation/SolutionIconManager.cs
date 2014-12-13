using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EnvDTE;
using JetBrains.Annotations;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace SolutionIcon.Implementation {
    public class SolutionIconManager {
        private readonly DTE _dte;
        private readonly IconDiscovery _iconDiscovery;
        private readonly IconConverter _iconConverter;
        private readonly IconGenerator _iconGenerator;
        private readonly IDiagnosticLogger _logger;
        
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable (GC fix)
        private readonly SolutionEvents _solutionEvents;

        public SolutionIconManager(DTE dte, IconDiscovery iconDiscovery, IconConverter iconConverter, IconGenerator iconGenerator, IDiagnosticLogger logger) {
            _dte = dte;
            _iconDiscovery = iconDiscovery;
            _iconConverter = iconConverter;
            _iconGenerator = iconGenerator;
            _logger = logger;

            if (!TaskbarManager.IsPlatformSupported) {
                _logger.WriteLine("Overlay icons are not supported on this platform, exiting.");
                return;
            }

            _solutionEvents = _dte.Events.SolutionEvents;
            _solutionEvents.Opened += SolutionEvents_Opened;
        }

        private void SolutionEvents_Opened() {
            var solution = _dte.Solution;
            _logger.WriteLine("Opened solution '{0}'.", solution.GetName());

            try {
                using (var icon = GetIcon(solution)) {
                    TaskbarManager.Instance.SetOverlayIcon(icon, "");
                }
            }
            catch (Exception ex) {
                _logger.WriteLine(ex.ToString());
            }
        }

        [NotNull]
        private Icon GetIcon([NotNull] Solution solution) {
            using (var image = GetIconImage(solution)) {
                return _iconConverter.ConvertToIcon(image);
            }
        }

        [NotNull]
        private Bitmap GetIconImage([NotNull] Solution solution) {
            var solutionName = solution.GetName();
            var iconFile = _iconDiscovery.FindIcon(solution);
            if (iconFile != null) {
                _logger.WriteLine("Solution '{0}': found icon at '{1}'.", solutionName, iconFile.FullName);
                using (var stream = iconFile.OpenRead()) {
                    return (Bitmap)Image.FromStream(stream);
                }
            }

            _logger.WriteLine("Solution '{0}': Icon not found, generating.", solutionName);
            return _iconGenerator.GenerateIcon(solutionName, solution.FileName);
        }
    }
}
