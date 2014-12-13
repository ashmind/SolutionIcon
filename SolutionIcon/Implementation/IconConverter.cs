using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.IconLib;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace SolutionIcon.Implementation {
    public class IconConverter {
        [NotNull]
        public Icon ConvertToIcon([NotNull] Bitmap image, Size iconSize) {
            if (image.Width == iconSize.Width && image.Height == iconSize.Height)
                return ConvertToIcon(image);

            using (var resized = new Bitmap(iconSize.Width, iconSize.Height, PixelFormat.Format32bppArgb)) {
                using (var g = Graphics.FromImage(resized)) {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, 0, 0, iconSize.Width, iconSize.Height);
                }
                return ConvertToIcon(resized);
            }
        }

        private Icon ConvertToIcon(Bitmap image) {
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
