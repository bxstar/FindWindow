using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Linq;

namespace FindWindow
{
    public class Helper
    {
        /// <summary>
        /// 捕获图片
        /// </summary>
        public static Image GetImgOnLine(string src)
        {
            WebClient wbc = new WebClient();
            Image img = null;
            byte[] content = wbc.DownloadData(src);
            MemoryStream stream = new MemoryStream(content);
            img = Image.FromStream(stream);
            wbc.Dispose();
            return img;
        }

        /// <summary>
        /// 根据图片和图片宽度取图片的缩略图
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="imagewidth">图片宽度</param>
        /// <returns></returns>
        public static Image GetImageFromImageWidth(Image image, int imagewidth)
        {
            if (image != null)
            {
                int oldwidth = image.Width;
                int oldheight = image.Height;
                double newwidth = oldwidth;
                double newheight = oldheight;
                newwidth = imagewidth;
                newheight = (double)(oldheight / (oldwidth / newwidth));
                Image myThumbnail = image.GetThumbnailImage((int)newwidth, (int)newheight, null, IntPtr.Zero);
                return myThumbnail;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 获取网卡地址
        /// </summary>
        public static string GetMacAddress()
        {
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = string.Empty;
            long maxSpeed = -1;

            foreach (System.Net.NetworkInformation.NetworkInterface nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
            {
                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed &&
                    !string.IsNullOrEmpty(tempMac) &&
                    tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                {
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
                }
            }

            return macAddress;
        }

        /// <summary>
        /// 获取网页编码为GBK数据
        /// </summary>
        /// <param name="url">网址</param>
        /// <returns></returns>
        public static string DownLoadGBKString(string url)
        {
            WebClient wb = new WebClient();
            wb.Headers.Add("Accept-Encoding", "gzip, deflate");
            wb.Encoding = Encoding.GetEncoding("GBK");
            return GZipString(wb.DownloadData(url), wb.Encoding, wb.ResponseHeaders["Content-Encoding"]);
        }

        public static string GZipString(byte[] byteArray, Encoding encoder, string sContentEncoding)
        {
            try
            {
                if (sContentEncoding == "gzip")
                {
                    // 处理　gzip string sContentEncoding = client.ResponseHeaders["Content-Encoding"]；if （sContentEncoding == "gzip"）
                    MemoryStream ms = new MemoryStream(byteArray);
                    MemoryStream msTemp = new MemoryStream();
                    int count = 0;
                    GZipStream gzip = new GZipStream(ms, CompressionMode.Decompress);
                    byte[] buf = new byte[1000];
                    while ((count = gzip.Read(buf, 0, buf.Length)) > 0)
                    { msTemp.Write(buf, 0, count); }
                    byteArray = msTemp.ToArray(); // end-gzip
                    return encoder.GetString(byteArray);
                }
                else
                {
                    return encoder.GetString(byteArray);
                }
            }
            catch
            {
                return encoder.GetString(byteArray);
            }
        }

        /// <summary>
        /// 获取手机号类型
        /// </summary>
        /// <param name="mobile">手机号</param>
        public static String GetMobileType(String mobile)
        {
            if (mobile.StartsWith("0") || mobile.StartsWith("+860"))
            {
                mobile = mobile.Substring(mobile.IndexOf("0") + 1, mobile.Length);
            }
            String[] chinaUnicom = new String[] { "130", "131", "132", "133", "155", "156", "185", "186" };
            String[] chinaMobile1 = new String[] { "135", "136", "137", "138", "139", "147", "150", "151", "152", "157", "158", "159", "182", "183", "184", "187", "188" };
            String[] chinaMobile2 = new String[] { "1340", "1341", "1342", "1343", "1344", "1345", "1346", "1347", "1348" };

            Boolean bolChinaUnicom = (chinaUnicom.Contains(mobile.Substring(0, 3)));
            Boolean bolChinaMobile1 = (chinaMobile1.Contains(mobile.Substring(0, 3)));
            Boolean bolChinaMobile2 = (chinaMobile2.Contains(mobile.Substring(0, 4)));

            if (bolChinaUnicom)
                return "联通";
            if (bolChinaMobile1 || bolChinaMobile2)
                return "移动";
            return "电信";
        }
    }
}
