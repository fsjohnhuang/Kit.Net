using lpp.LogHelper;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace lpp.CommonHelper
{
    public static class AuthCodeHelper
    {
        /// <summary>
        /// 生成纯数字随机码并保存到Session
        /// </summary>
        /// <param name="context">HttpContext上下文对象</param>
        /// <param name="codeCount">随机码位数</param>
        /// <param name="sessionKey">保存随机码的session键名</param>
        public static void GenerateIntAuthCode(HttpContext context, int codeCount, string sessionKey)
        {
            string authCode = GetRandomint(codeCount);
            byte[] imgByte = CreateCheckCodeImage(authCode);
            if (imgByte.Length >= 1)
            {
                context.Session[sessionKey] = authCode;
                context.Response.ClearContent();
                context.Response.BinaryWrite(imgByte);
            }
        }

        /// <summary>
        /// 生成纯数字随机码
        /// </summary>
        /// <param name="codeCount">随机码位数</param>
        /// <returns>随机码</returns>
        public static String GetRandomint(int codeCount)
        {
            int number;
            string checkCode = String.Empty;
            int iSeed = DateTime.Now.Millisecond;
            Random random = new Random(iSeed);
            for (int i = 0; i < codeCount; i++)
            {
                number = random.Next(10);
                checkCode += number.ToString();
            }

            return checkCode;
        }

        /// <summary>
        /// 生成随机码图片
        /// </summary>
        /// <param name="checkCode">随机码</param>
        /// <returns>图片字节数组</returns>
        public static byte[] CreateCheckCodeImage(string checkCode)
        {
            byte[] imgByte = null;
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return imgByte;
            int iWordWidth = 15;
            int iImageWidth = checkCode.Length * iWordWidth;
            Bitmap image = new Bitmap(iImageWidth, 20);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器 
                Random random = new Random();
                //清空图片背景色 
                g.Clear(Color.White);

                //画图片的背景噪音点
                for (int i = 0; i < 20; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                //画图片的背景噪音线 
                for (int i = 0; i < 2; i++)
                {
                    int x1 = 0;
                    int x2 = image.Width;
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    if (i == 0)
                    {
                        g.DrawLine(new Pen(Color.Gray, 2), x1, y1, x2, y2);
                    }

                }


                for (int i = 0; i < checkCode.Length; i++)
                {

                    string Code = checkCode[i].ToString();
                    int xLeft = iWordWidth * (i);
                    random = new Random(xLeft);
                    int iSeed = DateTime.Now.Millisecond;
                    int iValue = random.Next(iSeed) % 4;
                    if (iValue == 0)
                    {
                        Font font = new Font("Arial", 13, (FontStyle.Bold | System.Drawing.FontStyle.Italic));
                        Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
                        LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Blue, Color.Red, 1.5f, true);
                        g.DrawString(Code, font, brush, xLeft, 2);
                    }
                    else if (iValue == 1)
                    {
                        Font font = new System.Drawing.Font("楷体", 13, (FontStyle.Bold));
                        Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
                        LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Blue, Color.DarkRed, 1.3f, true);
                        g.DrawString(Code, font, brush, xLeft, 2);
                    }
                    else if (iValue == 2)
                    {
                        Font font = new System.Drawing.Font("宋体", 13, (System.Drawing.FontStyle.Bold));
                        Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
                        LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Green, Color.Blue, 1.2f, true);
                        g.DrawString(Code, font, brush, xLeft, 2);
                    }
                    else if (iValue == 3)
                    {
                        Font font = new System.Drawing.Font("黑体", 13, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Bold));
                        Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
                        LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Blue, Color.Green, 1.8f, true);
                        g.DrawString(Code, font, brush, xLeft, 2);
                    }
                }
                //////画图片的前景噪音点 
                //for (int i = 0; i < 8; i++)
                //{
                //    int x = random.Next(image.Width);
                //    int y = random.Next(image.Height);
                //    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                //}
                //画图片的边框线 
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Gif);
                imgByte = ms.ToArray();
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }

            return imgByte;
        }
    }
}
