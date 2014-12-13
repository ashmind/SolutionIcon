using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EnvDTE;
using JetBrains.Annotations;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace SolutionIcon.Implementation {
    public class SolutionIconManager {
        private static readonly Size IconSize = new Size(32, 32);

        private readonly DTE _dte;
        private readonly IconFinder _iconFinder;
        private readonly IconConverter _iconConverter;
        private readonly IconGenerator _iconGenerator;
        private readonly IDiagnosticLogger _logger;
        
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable (GC fix)
        private readonly SolutionEvents _solutionEvents;

        public SolutionIconManager(DTE dte, IconFinder iconFinder, IconConverter iconConverter, IconGenerator iconGenerator, IDiagnosticLogger logger) {
            _dte = dte;
            _iconFinder = iconFinder;
            _iconConverter = iconConverter;
            _iconGenerator = iconGenerator;
            _logger = logger;

            if (!TaskbarManager.IsPlatformSupported) {
                _logger.WriteLine("Overlay icons are not supported on this platform, exiting.");
                return;
            }

            _solutionEvents = _dte.Events.SolutionEvents;
            _solutionEvents.Opened += SolutionEvents_Opened;
            _solutionEvents.AfterClosing += SolutionEvents_AfterClosing;
        }

        private void SolutionEvents_Opened() {
            var solution = _dte.Solution;
            _logger.WriteLine("Solution '{0}' opened.", solution.GetName());

            try {
                using (var icon = GetIcon(solution)) {
                    TaskbarManager.Instance.SetOverlayIcon(icon, "");
                }
            }
            catch (Exception ex) {
                _logger.WriteLine(ex.ToString());
            }
        }

        private void SolutionEvents_AfterClosing() {
            _logger.WriteLine("Solution closed.");
            TaskbarManager.Instance.SetOverlayIcon(null, "");
        }
        
        [CanBeNull]
        private Icon GetIcon([NotNull] Solution solution) {
            var solutionName = solution.GetName();
            var existing = FindAndConvertExistingIcon(solution);
            if (existing != null)
                return existing;

            _logger.WriteLine("Solution '{0}': icon not available, generating.", solutionName);
            using (var image = _iconGenerator.GenerateIcon(solutionName, solution.FileName, IconSize)) {
                return ConvertToIconFailSafe(image, "generated image");
            }
        }
        
        [CanBeNull]
        private Icon FindAndConvertExistingIcon([NotNull] Solution solution) {
            var iconFile = _iconFinder.FindIcon(solution);
            if (iconFile == null)
                return null;

            _logger.WriteLine("Solution '{0}': found icon at '{1}'.", solution.GetName(), iconFile.FullName);
            using (var stream = iconFile.OpenRead())
            using (var image = (Bitmap) Image.FromStream(stream)) {
                return ConvertToIconFailSafe(image, iconFile.FullName);
            }
        }

        [CanBeNull]
        private Icon ConvertToIconFailSafe([NotNull] Bitmap image, [NotNull] string imageDescriptionForLog) {
            try {
                return _iconConverter.ConvertToIcon(image, IconSize);
            }
            catch (Exception ex) {
                _logger.WriteLine("Failed to convert {0} to icon: {1}", imageDescriptionForLog, ex);
                return null;
            }
        }
    }
}
