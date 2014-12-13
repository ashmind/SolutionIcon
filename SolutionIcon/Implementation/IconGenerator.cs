using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SolutionIcon.Implementation {
    public class IconGenerator {
        private static readonly Color[] Colors = (new[] {
            // http://flatuicolors.com/
            "#1abc9c", "#2ecc71", "#3498db", "#9b59b6", "#34495e",
            "#16a085", "#27ae60", "#2980b9", "#8e44ad", "#2c3e50",
            "#f1c40f", "#e67e22", "#95a5a6",
            "#f39c12", "#7f8c8d"
        }).Select(ColorTranslator.FromHtml).ToArray();

        private readonly TinyIdGenerator _idGenerator;
        private const int ImageSize = 32;

        public IconGenerator(TinyIdGenerator idGenerator) {
            _idGenerator = idGenerator;
        }

        public Bitmap GenerateIcon(string name, string path) {
            var id = _idGenerator.GetTinyId(name);

            Bitmap image = null;
            try {
                image = new Bitmap(ImageSize, ImageSize);

                var color = Colors[GetStableHash(path)%Colors.Length];
                var bounds = new RectangleF(0, 0, ImageSize, ImageSize);

                using (var graphics = Graphics.FromImage(image))
                using (var brush = new SolidBrush(color))
                using (var font = GetIconFont(graphics, id, bounds.Size)) {
                    graphics.FillRectangle(brush, bounds);
                    graphics.DrawString(id, font, Brushes.White, bounds, new StringFormat {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    });
                    return image;
                }
            }
            catch (Exception) {
                if (image != null)
                    image.Dispose();

                throw;
            }
        }

        private Font GetIconFont(Graphics graphics, string text, SizeF bounds) {
            using (var initial = new Font("Segoe UI", 10)) {
                var size = CalculateMaxFitFontSize(graphics, text, initial, bounds);
                return new Font(initial.FontFamily, size);
            }
        }

        // based on 
        // http://stackoverflow.com/questions/19674743/dynamically-resizing-font-to-fit-space-while-using-graphics-drawstring/#19674954
        private float CalculateMaxFitFontSize(Graphics graphics, string text, Font font, SizeF bounds) {
            var textSize = graphics.MeasureString(text, font);
            var scaleHeight = bounds.Height / textSize.Height;
            var scaleWidth = bounds.Width / textSize.Width;
            return font.Size * Math.Min(scaleHeight, scaleWidth);
        }

        // Jenkins' one-at-a-time
        // http://stackoverflow.com/questions/548158/fixed-length-numeric-hash-code-from-variable-length-string-in-c-sharp/#549352
        private uint GetStableHash(string value) {
            uint hash = 0;
            foreach (var @byte in Encoding.UTF8.GetBytes(value)) {
                hash += @byte;
                hash += (hash << 10);
                hash ^= (hash >> 6);    
            }
            // final avalanche
            hash += (hash << 3);
            hash ^= (hash >> 11);
            hash += (hash << 15);

            return hash;
        }
    }
}