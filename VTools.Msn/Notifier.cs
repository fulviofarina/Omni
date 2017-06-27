using System;
using System.Drawing;

///FULVIO
namespace VTools
{
    public partial class Notifier
    {
        public static Bitmap MakeBitMap(string text, System.Drawing.Font font, Color col)
        {
            System.Drawing.Brush brush = new System.Drawing.SolidBrush(col);

            // Create a bitmap and draw text on it
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(64, 64);
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
            graphics.DrawString(text, font, brush, 0, 0);

            // Convert the bitmap with text to an Icon
            return bitmap;
        }

        public static Icon MakeIcon(string text, System.Drawing.Font font, Color col)
        {
            System.Drawing.Brush brush = new System.Drawing.SolidBrush(col);

            // Create a bitmap and draw text on it
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(16, 16);
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
            graphics.DrawString(text, font, brush, 0, 0);

            // Convert the bitmap with text to an Icon
            IntPtr hIcon = bitmap.GetHicon();

            System.Drawing.Icon icon = System.Drawing.Icon.FromHandle(hIcon);
            return icon;
        }

        public Notifier()
        {
        }
    }
}