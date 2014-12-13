using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.IconLib;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace SolutionIcon.Implementation {
    public class IconConverter {
        [NotNull]
        public Icon ConvertToIcon([NotNull] Bitmap image) {
            var converted = new MemoryStream();
            var multi = new MultiIcon();
            var icon = multi.Add("Main");
            icon.CreateFrom(image, IconOutputFormat.Vista);
            multi.SelectedIndex = 0;
            multi.Save(converted, MultiIconFormat.ICO);
            
            converted.Seek(0, SeekOrigin.Begin);
            return new Icon(converted);
        }
    }
}
