using System;
using System.Configuration;
using System.Drawing;

namespace QiBuBlog.Util
{
    public class Captcha
    {

        private const float FontSize = 19;

        private static readonly Font CodeFont = new Font("Arial", FontSize);

        private static readonly Color[] FontColors = new Color[] { Color.Black, Color.Orange, Color.Red, Color.RoyalBlue, Color.Chartreuse, Color.YellowGreen };

        private static readonly Color[] NoiseColors = new Color[] { Color.LightGray, Color.LightYellow, Color.Lavender };

        private static readonly string CaptchaChars = ConfigurationManager.AppSettings["CaptchaChars"];

        public Captcha(byte length)
        {
            this.Generate(length);
        }

        public string Value { get; private set; }

        private string Generate(byte length)
        {
            this.Value = string.Empty;

            var rand = new Random();
            for (byte i = 0; i < length; i++)
            {
                this.Value += CaptchaChars[rand.Next(CaptchaChars.Length)];
            }

            return this.Value;
        }

        public Bitmap CreateImage(Color bgColor, short noiseAmount, int solid, double range, double phase)
        {
            Bitmap desImg = null;
            var codeLen = this.Value.Length;

            if (codeLen <= 0) return null;
            desImg = new Bitmap((int)(FontSize * codeLen * 1.3), (int)(FontSize * 1.8));
            var tempImg = new Bitmap(desImg.Width, desImg.Height);
            Graphics gDes = Graphics.FromImage(desImg), gTemp = Graphics.FromImage(tempImg);

            var rand = new Random();
            int xLimit = desImg.Width - 2, yLimit = desImg.Height - 2;

            gDes.Clear(bgColor);
            gTemp.Clear(bgColor);

            const float y = 0.1F * FontSize;
            for (var i = 0; i < codeLen; i++)
            {
                Brush brush = new SolidBrush(FontColors[rand.Next(FontColors.Length)]);
                gTemp.DrawString(this.Value.Substring(i, 1), CodeFont, brush, (i + 0.36F) * FontSize, y);
            }

            for (var i = 0; i < noiseAmount; i++)
            {
                var pen = new Pen(NoiseColors[rand.Next(NoiseColors.Length)]);
                gDes.DrawRectangle(pen, rand.Next(2, xLimit), rand.Next(2, yLimit), 1, 1);
            }

            var baseAxisLen = (double)desImg.Height;
            for (int i = 0, bgArgb = bgColor.ToArgb(); i < desImg.Width; i++)
            {
                int j;
                for (j = 0; j < desImg.Height; j++)
                {
                    var color = tempImg.GetPixel(i, j);

                    if (color.A == 0 || color.ToArgb() == bgArgb || rand.Next(solid) <= 0) continue;
                    var newX = i + (int)(Math.Sin((Math.PI * 2 * (double)j) / baseAxisLen + phase) * range);
                    var newY = j;

                    if (newX >= 0 && newX < desImg.Width
                        && newY >= 0 && newY < desImg.Height)
                    {
                        desImg.SetPixel(newX, newY, color);
                    }
                }
            }

            gDes.Dispose();
            gTemp.Dispose();
            return desImg;
        }
    }
}
