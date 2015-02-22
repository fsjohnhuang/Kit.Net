using System;
using System.Drawing;

namespace lpp.ImgHelper
{
    public sealed class Img
    {
        /// <summary>
        /// 转换为16进制表示形式
        /// </summary>
        /// <param name="input">输入的值</param>
        /// <returns>转换结果</returns>
        public static string ToHex(Color input)
        {
            return String.Format("0x{0:X2}{1:X2}{2:X2}", input.R, input.G, input.B);
        }

        #region 水印

        private static float WATERMARK_XY = 10; // 默认的加文字水印的x、y轴起始坐标

        /// <summary>
        /// 加文字水印
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <param name="letter">文字</param>
        /// <returns>加水印后的图片</returns>
        public static Image AttachLetterWatermark(string path, string letter)
        {
            Image image = Image.FromFile(path);
            return AttachLetterWatermark(image, letter, WATERMARK_XY, WATERMARK_XY);
        }

        /// <summary>
        /// 加文字水印
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <param name="letter">文字</param>
        /// <param name="x">文字x轴起始坐标</param>
        /// <param name="y">文字y轴起始坐标</param>
        /// <returns>加水印后的图片</returns>
        public static Image AttachLetterWatermark(string path, string letter, float x, float y)
        {
            Image image = Image.FromFile(path);
            return AttachLetterWatermark(image, letter, x, y);
        }

        /// <summary>
        /// 加文字水印
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <param name="letter">文字</param>
        /// <returns>加水印后的图片</returns>
        public static Image AttachLetterWatermark(Image image, string letter)
        {
            return AttachLetterWatermark(image, letter, WATERMARK_XY, WATERMARK_XY);
        }

        /// <summary>
        /// 加文字水印
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <param name="letter">文字</param>
        /// <param name="x">文字x轴起始坐标</param>
        /// <param name="y">文字y轴起始坐标</param>
        /// <returns>加水印后的图片</returns>
        public static Image AttachLetterWatermark(Image image, string letter, float x, float y)
        {
            Graphics g = Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);
            Font f = new Font("Verdana", 32);
            Brush b = new SolidBrush(Color.White);
            string addText = letter.Trim();
            g.DrawString(addText, f, b, x, y);
            g.Dispose();

            return image;
        }

        /// <summary>
        /// 加图片水印
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <param name="watermark">图片水印对象</param>
        /// <returns>加水印后的图片</returns>
        public static Image AttachImgWatermark(Image image, Image watermark)
        {
            Graphics g = Graphics.FromImage(image);
            g.DrawImage(watermark, new Rectangle(image.Width - watermark.Width, image.Height - watermark.Height, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel);
            g.Dispose();

            return image;
        }

        #endregion

        #region 缩略图

        // <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <returns>缩略图</returns>
        public static Image ProduceThumbnailImage(string path, int width, int height)
        {
            Image image = Image.FromFile(path);
            return ProduceThumbnailImage(image, width, height);
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <returns>缩略图</returns>
        public static Image ProduceThumbnailImage(Image image, int width, int height)
        {
            return image.GetThumbnailImage(width, height, null, System.IntPtr.Zero);
        }

        #endregion
    }
}
