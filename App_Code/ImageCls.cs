using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing.Imaging;

using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
namespace Resim
{
    /// <summary>
    /// Summary description for ImageCls
    /// </summary>
    public class ResimIslem
    {
        public ResimIslem()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private Bitmap sourceImage;
        public Bitmap SourceImage
        {
            get
            {
                //BuildImage();
                return sourceImage;
            }
            set { sourceImage = value; }
        }
        private Bitmap yedek;
        public Bitmap Yedek
        {
            get
            {
                //BuildImage();
                return yedek;
            }
            set { yedek = value; }
        }

        private Bitmap resultImage;

        /// <summary>
        /// Result image.
        /// </summary>
        public Bitmap ResultImage
        {
            get
            {
                resultImage = sourceImage;
                //BuildImage();
                return resultImage;
            }
            set { resultImage = value; }
        }

        public void ResimOku(FileUpload fu)
        {

            if (fu.HasFile)
            {
                System.Drawing.Image orjinalFoto = null;
                HttpPostedFile jpeg_image_upload = fu.PostedFile;
                orjinalFoto = System.Drawing.Image.FromStream(jpeg_image_upload.InputStream);
                sourceImage = new Bitmap(orjinalFoto);
                Yedek = new Bitmap(orjinalFoto); ;
            }
        }

        public string ResimOkuYol(string YOL)
        {
            string s = "1";
            try
            {

                System.Drawing.Image orjinalFoto = null;
                //HttpPostedFile jpeg_image_upload = fu.PostedFile;
                orjinalFoto = System.Drawing.Image.FromFile(YOL);
                sourceImage = new Bitmap(orjinalFoto);
                Yedek = new Bitmap(orjinalFoto); ;
            }
            catch (Exception t)
            {
                s = "0";
            }
            return s;
        }
        public void ResimKaydet()
        {
            SourceImage.Save(HttpContext.Current.Server.MapPath("~/images/" + DateTime.Now.Ticks.ToString().Substring(5) + ".jpg"));
            SourceImage = new Bitmap(Yedek);
        }
        public void ResimKaydet(string name)
        {
            SourceImage.Save(HttpContext.Current.Server.MapPath("~/images/" + name + ".jpg"));
            SourceImage = new Bitmap(Yedek);
        }
        public void Kucult(int boyut)
        {
            //ResultImage
            System.Drawing.Bitmap islenmisFotograf = null;
            System.Drawing.Graphics grafik = null;
            int hedefGenislik = boyut;
            int hedefYukseklik = boyut;
            int new_width, new_height;
            new_height = (int)Math.Round(((float)ResultImage.Height * (float)boyut) / (float)ResultImage.Width);
            new_width = hedefGenislik;
            hedefYukseklik = new_height;
            new_width = new_width > hedefGenislik ? hedefGenislik : new_width;
            new_height = new_height > hedefYukseklik ? hedefYukseklik : new_height;
            islenmisFotograf = new System.Drawing.Bitmap(hedefGenislik, hedefYukseklik);
            grafik = System.Drawing.Graphics.FromImage(islenmisFotograf);
            grafik.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), new System.Drawing.Rectangle(0, 0, hedefGenislik, hedefYukseklik));
            int paste_x = (hedefGenislik - new_width) / 2;
            int paste_y = (hedefYukseklik - new_height) / 2;
            grafik.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            grafik.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            grafik.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            System.Drawing.Imaging.ImageCodecInfo codec = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders()[1];
            System.Drawing.Imaging.EncoderParameters eParams = new System.Drawing.Imaging.EncoderParameters(1);
            eParams.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 95L);
            grafik.DrawImage(ResultImage, paste_x, paste_y, new_width, new_height);
            //Buraya kadar ki işlemler resmin boyutunu enaz kayıpla küçültme işlemlerini yaptı.Kodları inceleyiniz.
            //Satır satır açıklamak bazen anlamsız olabiliyor.
            /*
            System.Drawing.Font yazi = new System.Drawing.Font("Century Schoolbook", 8, System.Drawing.FontStyle.Italic);
                        System.Drawing.Brush br = new System.Drawing.SolidBrush(System.Drawing.Color.WhiteSmoke);
                        System.Drawing.Point nokta1 = new System.Drawing.Point(hedefGenislik / 3, hedefYukseklik - 30);
                        grafik.DrawString("alper", yazi, br, nokta1);
            */
            //Bu kısımdaki kodlarla resmin üzerine Beyaz bir şekilde Kutlaybto.biz yazdırdık.
            //islenmisFotograf.Save(HttpContext.Current.Server.MapPath("~/images/" + imagename + ".jpg"), codec, eParams);
            sourceImage = islenmisFotograf;
            //Son olarak resmimizi resimler klasörüne kaydettik.
            Yedek = islenmisFotograf;
        }
        /// <summary>
        /// Return max value of color channel.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <returns>Max channel value.</returns>
        public int MaxChannel(Color color)
        {
            return Math.Max(Math.Max(color.R, color.G), color.B);
        }
        /// <summary>
        /// Return min value of color channel.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <returns>min channel value.</returns>
        public int MinChannel(Color color)
        {
            return Math.Min(Math.Min(color.R, color.G), color.B);
        }
        private int period = 20;
        public int Period
        {
            get { return period; }
            set { period = value; }
        }
        private double fade = 1;
        public double Fade
        {
            get { return fade; }
            set
            {
                if (fade > 1)
                {
                    fade = 1;
                }
                if (fade < 0)
                {
                    fade = 0;
                }
                fade = value;
            }
        }
        /// <summary>
        /// Get color superposition.
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public Color SuperpositionColor(Color c1, Color c2, double k)
        {
            return renk((int)(c1.R * k + c2.R * (1 - k)),
                                    (int)(c1.G * k + c2.G * (1 - k)),
                                    (int)(c1.B * k + c2.B * (1 - k)));
        }
        public Color MaksimumRenk(Color c)
        {
            Color R = Color.FromArgb(0, 0, 0);
            if (c.R > Math.Max(c.G, c.B))
                R = Color.FromArgb(255, 0, 0);
            if (c.G > Math.Max(c.R, c.B))
                R = Color.FromArgb(0, 255, 0);
            if (c.B > Math.Max(c.R, c.G))
                R = Color.FromArgb(0, 0, 255);
            return R;

        }
        public Color MinimumRenk(Color c)
        {
            Color R = Color.FromArgb(0, 0, 0);
            if (c.R < Math.Min(c.G, c.B))
                R = Color.FromArgb(255, 0, 0);
            if (c.G < Math.Min(c.R, c.B))
                R = Color.FromArgb(0, 255, 0);
            if (c.B < Math.Min(c.R, c.G))
                R = Color.FromArgb(0, 0, 255);
            return R;

        }
        public Color EsitRenk(Color c)
        {
            Color R = Color.FromArgb(0, 0, 0);


            if (c.R == c.B || c.R == c.G || c.B == c.G)
                R = Color.FromArgb(255, 255, 255);
            return R;

        }


        /// <summary>
        /// Get grayscale color.
        /// Gray = Green * 0.59 + Blue * 0.30 + Red * 0.11
        /// </summary>
        /// <param name="color">RGB Color.</param>
        /// <returns></returns>
        public int GetGray(Color color)
        {
            return (int)(color.R * 0.11 + color.G * 0.59 + color.B * 0.3);
        }
        public Color GetGrayColor(Color color)
        {
            return renk(GetGray(color), GetGray(color), GetGray(color));
        }
        public Color renk(int R, int G, int B)
        {
            if (R > 255)
                R = 255;
            if (G > 255)
                G = 255;
            if (B > 255)
                B = 255;

            if (R < 0)
                R = 0;
            if (G < 0)
                G = 0;
            if (B < 0)
                B = 0;

            return Color.FromArgb(R, G, B);
        }
        public Color OrtaRenk()
        {
            return GetMeanValue(resultImage);
        }

        /// <summary>
        /// Get color channel.
        /// </summary>
        /// <param name="color">RGB color.</param>
        /// <param name="channel">Color channel.</param>
        /// <returns></returns>
        public static int GetColorChannel(Color color, RGB channel)
        {
            switch (channel)
            {
                case RGB.R: return color.R;
                case RGB.G: return color.G;
                default: return color.B;
            }
        }
        /// <summary>
        /// Return mean value for image.
        /// </summary>
        /// <param name="bmp">Source image.</param>
        /// <param name="channel">Color channel.</param>
        /// <returns>Mean value.</returns>
        public double GetMeanValue(Bitmap bmp, RGB channel)
        {
            double e = 0;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    e += GetColorChannel(bmp.GetPixel(i, j), channel);
                }
            }

            return e / (bmp.Width * bmp.Height);
        }
        public int RenkOrtalama()
        {
            Color c;
            int e = 0;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    c = sourceImage.GetPixel(i, j);
                    int gray = (c.R + c.G + c.B) / 3;

                    e += gray;//GetColorChannel(sourceImage.GetPixel(i, j), channel);
                }
            }

            return e / (sourceImage.Width * sourceImage.Height);
        }
        /// <summary>
        /// Return mean value for image.
        /// </summary>
        /// <param name="bmp">Source image.</param>
        /// <returns>Mean value of color.</returns>
        public Color GetMeanValue(Bitmap bmp)
        {
            return Color.FromArgb((int)(GetMeanValue(bmp, RGB.R)),
                                (int)(GetMeanValue(bmp, RGB.G)),
                                (int)(GetMeanValue(bmp, RGB.B)));
        }
        /// <summary>
        /// Get dispersion of image.
        /// </summary>
        /// <param name="bmp">Source image.</param>
        /// <param name="channel">Color channel.</param>
        /// <returns>Dispersion of image.</returns>
        /// 

        public void fxdene()
        {
            Color c;
            Color z = Color.Black;
            int o = 0;



            int gn = sourceImage.Width / 2;
            int yk = sourceImage.Height / 2;
            int a = 0;
            int b = 0;
            //int maxRadius = (int)Math.Max(sourceImage.Width, sourceImage.Height) / 2;
            int maxRadius = (sourceImage.Width + sourceImage.Height) / 3;
            for (int x = 0; x < sourceImage.Width; x++)
            {

                for (int y = 0; y < sourceImage.Height; y++)
                {

                    c = sourceImage.GetPixel(x, y);

                    int xx = (x - gn);
                    int yy = (y - yk);
                    #region mod
                    if (x % 21 == 1) a = -10;
                    if (x % 21 == 2) a = -9;
                    if (x % 21 == 3) a = -8;
                    if (x % 21 == 4) a = -7;
                    if (x % 21 == 5) a = -6;
                    if (x % 21 == 6) a = -5;
                    if (x % 21 == 7) a = -4;
                    if (x % 21 == 8) a = -3;
                    if (x % 21 == 9) a = -2;
                    if (x % 21 == 10) a = -1;
                    if (x % 21 == 11) a = 0;
                    if (x % 21 == 12) a = 1;
                    if (x % 21 == 13) a = 2;
                    if (x % 21 == 14) a = 3;
                    if (x % 21 == 15) a = 4;
                    if (x % 21 == 16) a = 5;
                    if (x % 21 == 17) a = 6;
                    if (x % 21 == 18) a = 7;
                    if (x % 21 == 19) a = 8;
                    if (x % 21 == 20) a = 9;
                    if (x % 21 == 0) a = 10;
                    if (y % 21 == 1) b = -10;
                    if (y % 21 == 2) b = -9;
                    if (y % 21 == 3) b = -8;
                    if (y % 21 == 4) b = -7;
                    if (y % 21 == 5) b = -6;
                    if (y % 21 == 6) b = -5;
                    if (y % 21 == 7) b = -4;
                    if (y % 21 == 8) b = -3;
                    if (y % 21 == 9) b = -2;
                    if (y % 21 == 10) b = -1;
                    if (y % 21 == 11) b = 0;
                    if (y % 21 == 12) b = 1;
                    if (y % 21 == 13) b = 2;
                    if (y % 21 == 14) b = 3;
                    if (y % 21 == 15) b = 4;
                    if (y % 21 == 16) b = 5;
                    if (y % 21 == 17) b = 6;
                    if (y % 21 == 18) b = 7;
                    if (y % 21 == 19) b = 8;
                    if (y % 21 == 20) b = 9;
                    if (y % 21 == 0) b = 10;
                    #endregion

                    int contrast = 100;

                    // if (Math.Pow(xx, 2) == Math.Pow(yy, 2)) //koordinat düzleminde tam orta nokta 0 oldu

                    // if (Math.Pow(xx, 3) > ((12 * yy) - 1)) //
                    //if (Math.Pow(xx, 2) < 50 && Math.Pow(yy, 2) < 50) //
                    //    if (xx*yy<0) // 
                    //if (Math.Pow(xx, 2) + Math.Pow(yy, 2) > Math.Pow(maxRadius, 2)) //koordinat düzleminde tam orta nokta 0 oldu
                    if (Math.Pow(a, 2) + Math.Pow(b, 2) > Math.Pow(9.5, 2)) //koordinat düzleminde tam orta nokta 0 oldu
                    {




                        sourceImage.SetPixel(x, y,
                   Color.FromArgb(0, 0, 0));
                    }

                    else
                    {
                        sourceImage.SetPixel(x, y, Color.FromArgb(c.R, c.G, c.B));


                    }


                }
            }
        }


        public void fSabitAralikMx()
        {







            Color c;
            Color z = Color.Black;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {

                    c = sourceImage.GetPixel(i, j);

                    byte r, g, b;

                    r = c.R;
                    g = c.G;
                    b = c.B;

                    if (r > 0 && r < 51)
                        r = 50;
                    if (r > 50 && r < 101)
                        r = 100;
                    if (r > 100 && r < 151)
                        r = 150;
                    if (r > 150 && r < 201)
                        r = 200;
                    if (r > 200)
                        r = 255;


                    if (g > 0 && g < 51)
                        g = 50;
                    if (g > 50 && g < 101)
                        g = 100;
                    if (g > 100 && g < 151)
                        g = 150;
                    if (g > 150 && g < 201)
                        g = 200;
                    if (g > 200)
                        g = 255;


                    if (b > 0 && b < 51)
                        b = 50;
                    if (b > 50 && b < 101)
                        b = 100;
                    if (b > 100 && b < 151)
                        b = 150;
                    if (b > 150 && b < 201)
                        b = 200;
                    if (b > 200)
                        b = 255;



                    sourceImage.SetPixel(i, j, Color.FromArgb(r, g, b));


                }
            }
        }
        public void fSabitAralikMn()
        {
            Color c;
            Color z = Color.Black;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {

                    c = sourceImage.GetPixel(i, j);

                    byte r, g, b;

                    r = c.R;
                    g = c.G;
                    b = c.B;

                    //if (r > 0 && r < 51)
                    //    r = 50;
                    //if (r > 50 && r < 101)
                    //    r = 100;
                    //if (r > 100 && r < 151)
                    //    r = 150;
                    //if (r > 150 && r < 201)
                    //    r = 200;
                    //if (r > 200)
                    //    r = 255;


                    //if (g > 0 && g < 51)
                    //    g = 50;
                    //if (g > 50 && g < 101)
                    //    g = 100;
                    //if (g > 100 && g < 151)
                    //    g = 150;
                    //if (g > 150 && g < 201)
                    //    g = 200;
                    //if (g > 200)
                    //    g = 255;


                    //if (b > 0 && b < 51)
                    //    b = 50;
                    //if (b > 50 && b < 101)
                    //    b = 100;
                    //if (b > 100 && b < 151)
                    //    b = 150;
                    //if (b > 150 && b < 201)
                    //    b = 200;
                    //if (b > 200)
                    //    b = 255;
                    if (r > 0 && r < 51)
                        r = 0;
                    if (r > 50 && r < 101)
                        r = 50;
                    if (r > 100 && r < 151)
                        r = 100;
                    if (r > 150 && r < 201)
                        r = 150;
                    if (r > 200)
                        r = 200;


                    if (g > 0 && g < 51)
                        g = 0;
                    if (g > 50 && g < 101)
                        g = 50;
                    if (g > 100 && g < 151)
                        g = 100;
                    if (g > 150 && g < 201)
                        g = 150;
                    if (g > 200)
                        g = 200;


                    if (b > 0 && b < 51)
                        b = 0;
                    if (b > 50 && b < 101)
                        b = 50;
                    if (b > 100 && b < 151)
                        b = 100;
                    if (b > 150 && b < 201)
                        b = 150;
                    if (b > 200)
                        b = 200;



                    sourceImage.SetPixel(i, j, Color.FromArgb(r, g, b));


                }
            }
        }
        public void fSabitAralikGMx()
        {
            Color c;
            Color z = Color.Black;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {

                    c = sourceImage.GetPixel(i, j);

                    byte r, g, b;

                    r = c.R;
                    g = c.G;
                    b = c.B;

                    if (r > 0 && r < 101)
                        r = 100;

                    if (r > 100 && r < 201)
                        r = 200;

                    if (r > 200)
                        r = 255;


                    if (g > 0 && g < 101)
                        b = 100;

                    if (g > 100 && g < 201)
                        g = 200;

                    if (g > 200)
                        g = 255;


                    if (b > 0 && b < 101)
                        b = 100;

                    if (b > 100 && b < 201)
                        b = 200;

                    if (b > 200)
                        b = 255;



                    sourceImage.SetPixel(i, j, Color.FromArgb(r, g, b));


                }
            }
        }
        public void fSabitAralikGMn()
        {
            Color c;
            Color z = Color.Black;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {

                    c = sourceImage.GetPixel(i, j);

                    byte r, g, b;

                    r = c.R;
                    g = c.G;
                    b = c.B;

                    if (r > 0 && r < 101)
                        r = 0;

                    if (r > 100 && r < 201)
                        r = 100;

                    if (r > 200)
                        r = 200;


                    if (g > 0 && g < 101)
                        b = 0;

                    if (g > 100 && g < 201)
                        g = 100;

                    if (g > 200)
                        g = 200;


                    if (b > 0 && b < 101)
                        b = 0;

                    if (b > 100 && b < 201)
                        b = 100;

                    if (b > 200)
                        b = 200;



                    sourceImage.SetPixel(i, j, Color.FromArgb(r, Math.Min(g, b), Math.Min(g, b)));


                }
            }
        }
        public void alper2()
        {
            Color c;

            float _curvature = 0.9999f;

            // get source image size
            int width = sourceImage.Width;
            int height = sourceImage.Height;
            int halfWidth = width / 2;
            int halfHeight = height / 2;

            //Obtain the Maxium Size Of the Fish Eye
            int maxRadius = (int)Math.Min(width, height) / 2;
            //The S parameter mentioned in Jason Walton Algo.
            double s = maxRadius / Math.Log(_curvature * maxRadius + 1);

            #region RGB
            //Start from the middle because point (0,0) in cartesian co-ordinates should not be in the center
            for (int y = -1 * halfHeight; y < height - halfHeight; y++)
            {
                for (int x = -1 * halfWidth; x < width - halfWidth; x++)
                {
                    //Get the Current Pixels Polar Co-ordinates
                    double radius = Math.Sqrt(x * x + y * y);
                    double angle = Math.Atan2(y, x);

                    //Check to see if the polar pixel is out of bounds
                    if (radius <= maxRadius && y < height && x < width && y > 0 && x > 0)
                    {
                        //Current Pixel is inside the Fish Eye

                        //newRad is the pixel that we want to shift to current pixel based on the fish eye posistion
                        double newRad = (Math.Exp(radius / s) - 1) / _curvature;
                        //Current Pixels Polar Cordinates Back To Cartesian
                        int newX = (int)(newRad * Math.Cos(angle));
                        int newY = (int)(newRad * Math.Sin(angle));

                        newX += halfWidth;
                        newY += halfHeight;

                        c = sourceImage.GetPixel(x, y);



                        sourceImage.SetPixel(x, y, Color.FromArgb(0, 0, 0));



                    }
                    //else
                    //{
                    //    *dst = src[y * stride + x];
                    //}

                }
            }
            #endregion
        }

        public void alper3()
        {
            Color c;

            float _curvature = 0.9999f;

            // get source image size
            int width = sourceImage.Width;
            int height = sourceImage.Height;
            int halfWidth = width / 2;
            int halfHeight = height / 2;

            //Obtain the Maxium Size Of the Fish Eye
            int maxRadius = (int)Math.Min(width, height) / 2 /**/;
            //The S parameter mentioned in Jason Walton Algo.
            double s = maxRadius / Math.Log(_curvature * maxRadius + 1);

            double contrast = 5;
            if (contrast < -100) contrast = -100;
            if (contrast > 100) contrast = 100;
            contrast = (100.0 + contrast) / 100.0;
            contrast *= contrast;




            #region RGB
            //Start from the middle because point (0,0) in cartesian co-ordinates should not be in the center
            for (int y = -1 * halfHeight; y < height - halfHeight/**/; y++)
            {
                for (int x = -1 * halfWidth; x < width - halfWidth/**/; x++)
                {
                    //Get the Current Pixels Polar Co-ordinates
                    double radius = Math.Sqrt(x * x + y * y);
                    double angle = Math.Atan2(y, x);


                    double newRad = (Math.Exp(radius / s) - 1) / _curvature;
                    //Current Pixels Polar Cordinates Back To Cartesian
                    int newX = (int)(newRad * Math.Cos(angle));
                    int newY = (int)(newRad * Math.Sin(angle));

                    newX += halfWidth;
                    newY += height;



                    if (newY > 0 && newX > 0 && newX < width && newY < height)
                    {

                        //c = sourceImage.GetPixel(newX, newY);
                        //                        double pR = c.R / 255.0;
                        //                        pR -= 0.5;
                        //                        pR *= contrast;
                        //                        pR += 0.5;
                        //                        pR *= 255;
                        //                        if (pR < 0) pR = 0;
                        //                        if (pR > 255) pR = 255;

                        //                        double pG = c.G / 255.0;
                        //                        pG -= 0.5;
                        //                        pG *= contrast;
                        //                        pG += 0.5;
                        //                        pG *= 255;
                        //                        if (pG < 0) pG = 0;
                        //                        if (pG > 255) pG = 255;

                        //                        double pB = c.B / 255.0;
                        //                        pB -= 0.5;
                        //                        pB *= contrast;
                        //                        pB += 0.5;
                        //                        pB *= 255;
                        //                        if (pB < 0) pB = 0;
                        //                        if (pB > 255) pB = 255;

                        //                        sourceImage.SetPixel(newX, newY,
                        //            Color.FromArgb((byte)pR, (byte)pG, (byte)pB));
                        sourceImage.SetPixel(newX, newY, Color.FromArgb(255, 0, 0));
                    }

                }
            }
            #endregion


        }

        public void alper()
        {
            Color c;

            float _curvature = 0.9999f;

            // get source image size
            int width = sourceImage.Width;
            int height = sourceImage.Height;
            int halfWidth = width / 2;
            int halfHeight = height / 2;

            //Obtain the Maxium Size Of the Fish Eye
            int maxRadius = (int)Math.Min(width, height) / 2;
            //The S parameter mentioned in Jason Walton Algo.
            double s = maxRadius / Math.Log(_curvature * maxRadius + 1);



            #region Grayscale
            for (int y = -1 * halfHeight; y < height - halfHeight; y++)
            {
                for (int x = -1 * halfWidth; x < width - halfWidth; x++)
                {
                    //Get the Current Pixels Polar Co-ordinates
                    double radius = Math.Sqrt(x * x + y * y);
                    double angle = Math.Atan2(y, x);

                    //Check to see if the polar pixel is out of bounds
                    if (radius <= maxRadius)
                    {
                        //Current Pixel is inside the Fish Eye
                        //newRad is the pixel that we want to shift to current pixel based on the fish eye posistion
                        double newRad = (Math.Exp(radius / s) - 1) / _curvature;
                        int newX = (int)(newRad * Math.Cos(angle));
                        int newY = (int)(newRad * Math.Sin(angle));

                        newX += halfWidth;
                        newY += halfHeight;

                        //p = src + newY * stride + newX;
                        //byte* dst2 = dst + (y + halfHeight) * stride + (x + halfWidth);
                        //dst2 = p;
                        c = sourceImage.GetPixel(newX, newY);



                        sourceImage.SetPixel(newX, newY, Color.FromArgb(0, 0, 0));



                    }
                    //else
                    //{
                    //    *dst = src[y * stride + x];
                    //}

                }

            }
            #endregion


        }

        //public void fBalik()
        //{
        //    float _curvature = 0.7f;
        //    int width = sourceImage.Width;
        //    int height = sourceImage.Height;
        //    int halfWidth = width / 2;
        //    int halfHeight = height / 2;

        //    //Obtain the Maxium Size Of the Fish Eye
        //    int maxRadius = (int)Math.Min(width, height) / 2;
        //    //The S parameter mentioned in Jason Walton Algo.
        //    double s = maxRadius / Math.Log(_curvature * maxRadius + 1);

        //    PixelFormat fmt = (sourceImage.PixelFormat == PixelFormat.Format8bppIndexed) ?
        //        PixelFormat.Format8bppIndexed : PixelFormat.Format24bppRgb;

        //    // lock source bitmap data
        //    BitmapData srcData = sourceImage.LockBits(
        //        new Rectangle(0, 0, width, height),
        //        ImageLockMode.ReadOnly, fmt);

        //    // create new image
        //    Bitmap dstImg =  new Bitmap(width, height, fmt);

        //    // lock destination bitmap data
        //    BitmapData dstData = dstImg.LockBits(
        //        new Rectangle(0, 0, width, height),
        //        ImageLockMode.ReadWrite, fmt);

        //    int stride = srcData.Stride;
        //    int offset = stride - ((fmt == PixelFormat.Format8bppIndexed) ? width : width * 3);


        //    // Perform The Fish Eye Conversion
        //    //
        //    unsafe
        //    {
        //        byte* src = (byte*)srcData.Scan0.ToPointer();
        //        byte* dst = (byte*)dstData.Scan0.ToPointer();
        //        byte* p;

        //        if (fmt == PixelFormat.Format8bppIndexed)
        //        {
        //            #region Grayscale
        //            for (int y = -1 * halfHeight; y < height - halfHeight; y++)
        //            {
        //                for (int x = -1 * halfWidth; x < width - halfWidth; x++)
        //                {
        //                    //Get the Current Pixels Polar Co-ordinates
        //                    double radius = Math.Sqrt(x * x + y * y);
        //                    double angle = Math.Atan2(y, x);

        //                    //Check to see if the polar pixel is out of bounds
        //                    if (radius <= maxRadius)
        //                    {
        //                        //Current Pixel is inside the Fish Eye
        //                        //newRad is the pixel that we want to shift to current pixel based on the fish eye posistion
        //                        double newRad = (Math.Exp(radius / s) - 1) / _curvature;
        //                        int newX = (int)(newRad * Math.Cos(angle));
        //                        int newY = (int)(newRad * Math.Sin(angle));

        //                        newX += halfWidth;
        //                        newY += halfHeight;

        //                        p = src + newY * stride + newX;
        //                        byte* dst2 = dst + (y + halfHeight) * stride + (x + halfWidth);
        //                        dst2 = p;

        //                    }
        //                    else
        //                    {
        //                        *dst = src[y * stride + x];
        //                    }

        //                }
        //                dst += offset;
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            #region RGB
        //            //Start from the middle because point (0,0) in cartesian co-ordinates should not be in the center
        //            for (int y = -1 * halfHeight; y < height - halfHeight; y++)
        //            {
        //                for (int x = -1 * halfWidth; x < width - halfWidth; x++)
        //                {
        //                    //Get the Current Pixels Polar Co-ordinates
        //                    double radius = Math.Sqrt(x * x + y * y);
        //                    double angle = Math.Atan2(y, x);

        //                    //Check to see if the polar pixel is out of bounds
        //                    if (radius <= maxRadius)
        //                    {
        //                        //Current Pixel is inside the Fish Eye

        //                        //newRad is the pixel that we want to shift to current pixel based on the fish eye posistion
        //                        double newRad = (Math.Exp(radius / s) - 1) / _curvature;
        //                        //Current Pixels Polar Cordinates Back To Cartesian
        //                        int newX = (int)(newRad * Math.Cos(angle));
        //                        int newY = (int)(newRad * Math.Sin(angle));

        //                        newX += halfWidth;
        //                        newY += halfHeight;

        //                        //Chage the Pointer to the Src
        //                        p = src + newY * stride + newX * 3;
        //                        //Change the Pointer to the destination
        //                        byte* dst2 = dst + (y + halfHeight) * stride + (x + halfWidth) * 3;
        //                        dst2[RGB.R] = p[RGB.R];
        //                        dst2[RGB.G] = p[RGB.G];
        //                        dst2[RGB.B] = p[RGB.B];

        //                    }
        //                    else
        //                    {
        //                        //Current Pixel is outside the Fish Eye thus should be output as is
        //                        p = src + (y + halfHeight) * stride + (x + halfWidth) * 3;

        //                        //Change the Pointer to the destination
        //                        byte* dst2 = dst + (y + halfHeight) * stride + (x + halfWidth) * 3;

        //                        dst2[RGB.R] = p[RGB.R];
        //                        dst2[RGB.G] = p[RGB.G];
        //                        dst2[RGB.B] = p[RGB.B];
        //                    }
        //                }
        //            }
        //            #endregion
        //        }
        //    }
        //    // unlock both images
        //    dstImg.UnlockBits(dstData);
        //    srcImg.UnlockBits(srcData);


        //}
        public void fKaraKalem2(int ayar)
        {
            Color c;
            Color z = Color.Black;
            int o = 0;
            int r2 = 0; int g2 = 0; int b2 = 0; int x2 = 0;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {

                    c = sourceImage.GetPixel(i, j);

                    int r, g, b, x;


                    //0=111
                    r = c.R;
                    g = c.G;
                    b = c.B;
                    x = (r + g + b) / 3;
                    //x=50

                    if ((r < r2 - ayar && r < r2 + ayar) || (g < g2 - ayar && g < g2 + ayar) || (b < b2 - ayar && b < b2 + ayar))
                    {
                        sourceImage.SetPixel(i, j, Color.FromArgb(0, 0, 0));

                    }


                    //if (/*i%2==0 &&*/ j % 2 == 0)
                    //    o = x;


                    r2 = c.R;
                    g2 = c.G;
                    b2 = c.B;
                    x2 = (r + g + b) / 3;


                }
            }
        }

        public int OBEB(int sayi1, int sayi2)
        {
            int Obeb = 0;
            int k;
            if (sayi1 > sayi2)
                k = sayi2;
            else
                k = sayi1;

            for (; k > 0; k--)
            {
                if (sayi1 % k == 0 && sayi2 % k == 0)
                {
                    Obeb = k;
                    break;
                }
            }
            return Obeb;
        }

        public int OKEK(int sayi1, int sayi2)
        {
            int Okek = 0;
            int k;
            if (sayi1 < sayi2)
                k = sayi2;
            else
                k = sayi1;

            for (; k <= sayi1 * sayi2; k++)
            {
                if (k % sayi1 == 0 && k % sayi2 == 0)
                {
                    Okek = k;
                    break;
                }
            }
            return Okek;
        }



        public void Parcala(int oran, int ek)
        {
            int width = sourceImage.Width;//940
            int height = sourceImage.Height;//518
            //int Obeb = OBEB(width, height);//2
            //int Okek = OKEK(width, height);//243460

            int en = (int)Math.Round((double)width / oran, 0);
            int boy = (int)Math.Round((double)height / oran, 0);
            //846
            //468

            for (int x = 0; x < oran + 1; x++)
            {
                for (int y = 0; y < oran + 1; y++)
                {
                    int w = en * x;
                    int h = boy * y;
                    int e = 0;
                    int z = 0;
                    if (x == oran || y == oran)
                    {
                        for (int i = w; i < width; i++)
                        {
                            for (int j = h; j < height; j++)
                            {
                                z++;
                                Color c = sourceImage.GetPixel(i, j);
                                int gray = (c.R + c.G + c.B) / 3;
                                e += gray;
                            }
                        }
                        for (int i = w; i < width; i++)
                        {
                            for (int j = h; j < height; j++)
                            {
                                Color c2 = sourceImage.GetPixel(i, j);
                                int luma = (int)(c2.R + c2.G + c2.B) / 3;
                                if (luma + ek < e / z)
                                {
                                    sourceImage.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                                }
                                else
                                {
                                    sourceImage.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = w; i < w + en; i++)
                        {
                            for (int j = h; j < h + boy; j++)
                            {
                                z++;
                                Color c = sourceImage.GetPixel(i, j);
                                int gray = (c.R + c.G + c.B) / 3;
                                e += gray;
                            }
                        }
                        for (int i = w; i < w + en; i++)
                        {
                            for (int j = h; j < h + boy; j++)
                            {
                                Color c2 = sourceImage.GetPixel(i, j);
                                int luma = (int)(c2.R + c2.G + c2.B) / 3;
                                if (luma + ek < e / z)
                                {
                                    sourceImage.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                                }
                                else
                                {
                                    sourceImage.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                                }
                            }
                        }
                    }
                }
            }
        }
        private int maxint;
        public int Maxint
        {
            get { return maxint; }
            set
            {

                maxint = Math.Max(value, maxint);

            }
        }
        private int minint;
        public int Minint
        {
            get { return minint; }
            set
            {

                minint = Math.Min(value, minint);

            }
        }
        public void ParcalaMax()
        {
            int width = sourceImage.Width;//940
            int height = sourceImage.Height;//518
            //int Obeb = OBEB(width, height);//2
            //int Okek = OKEK(width, height);//243460
            int en = (int)Math.Round((double)width / 10, 0);
            int boy = (int)Math.Round((double)height / 10, 0);
            //846
            //468
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    int w = en * x;
                    int h = boy * y;
                    int e = 0;
                    int z = 0;
                    maxint = 0;
                    if (x == 9 || y == 9)
                    {
                        for (int i = w; i < width; i++)
                        {
                            for (int j = h; j < height; j++)
                            {
                                z++;
                                Color c = sourceImage.GetPixel(i, j);
                                int gray = (c.R + c.G + c.B) / 3;
                                e += gray;
                                Maxint = gray;
                            }
                        }
                        for (int i = w; i < width; i++)
                        {
                            for (int j = h; j < height; j++)
                            {
                                Color c2 = sourceImage.GetPixel(i, j);
                                int luma = (int)(c2.R + c2.G + c2.B) / 3;
                                if (luma == Maxint)
                                {
                                    sourceImage.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                                }
                                else
                                {
                                    sourceImage.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = w; i < w + en; i++)
                        {
                            for (int j = h; j < h + boy; j++)
                            {
                                z++;
                                Color c = sourceImage.GetPixel(i, j);
                                int gray = (c.R + c.G + c.B) / 3;
                                e += gray; Maxint = gray;
                            }
                        }
                        for (int i = w; i < w + en; i++)
                        {
                            for (int j = h; j < h + boy; j++)
                            {
                                Color c2 = sourceImage.GetPixel(i, j);
                                int luma = (int)(c2.R + c2.G + c2.B) / 3;
                                if (luma == Maxint)
                                {
                                    sourceImage.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                                }
                                else
                                {
                                    sourceImage.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                                }
                            }
                        }
                    }
                }
            }
        }
        public void ParcalaMin()
        {
            int width = sourceImage.Width;//940
            int height = sourceImage.Height;//518
            //int Obeb = OBEB(width, height);//2
            //int Okek = OKEK(width, height);//243460
            int en = (int)Math.Round((double)width / 10, 0);
            int boy = (int)Math.Round((double)height / 10, 0);
            //846
            //468
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    int w = en * x;
                    int h = boy * y;
                    int e = 0;
                    int z = 0;
                    minint = 0;
                    if (x == 9 || y == 9)
                    {
                        for (int i = w; i < width; i++)
                        {
                            for (int j = h; j < height; j++)
                            {
                                z++;
                                Color c = sourceImage.GetPixel(i, j);
                                int gray = (c.R + c.G + c.B) / 3;
                                e += gray;
                                Minint = gray;
                            }
                        }
                        for (int i = w; i < width; i++)
                        {
                            for (int j = h; j < height; j++)
                            {
                                Color c2 = sourceImage.GetPixel(i, j);
                                int luma = (int)(c2.R + c2.G + c2.B) / 3;
                                if (luma == Minint)
                                {
                                    sourceImage.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                                }
                                else
                                {
                                    sourceImage.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = w; i < w + en; i++)
                        {
                            for (int j = h; j < h + boy; j++)
                            {
                                z++;
                                Color c = sourceImage.GetPixel(i, j);
                                int gray = (c.R + c.G + c.B) / 3;
                                e += gray; Minint = gray;
                            }
                        }
                        for (int i = w; i < w + en; i++)
                        {
                            for (int j = h; j < h + boy; j++)
                            {
                                Color c2 = sourceImage.GetPixel(i, j);
                                int luma = (int)(c2.R + c2.G + c2.B) / 3;
                                if (luma == Minint)
                                {
                                    sourceImage.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                                }
                                else
                                {
                                    sourceImage.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                                }
                            }
                        }
                    }
                }
            }
        }


        public void fKaraCizgi()
        {
            Color c;


            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {

                    c = sourceImage.GetPixel(i, j);

                    int r, g, b;

                    r = c.R;
                    g = c.G;
                    b = c.B;


                    if (r > 200 && g > 200 && b > 200 && r < 225 && g < 225 && b < 225)
                    {
                        sourceImage.SetPixel(i, j, Color.Black);

                    }
                    else
                    {
                        //  sourceImage.SetPixel(i, j, Color.FromArgb(r, g, b));
                        sourceImage.SetPixel(i, j, Color.White);

                    }

                }
            }
        }
        public void fKaraKalem()
        {
            Color c;
            Color z = Color.Black;
            int o = 0;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {

                    c = sourceImage.GetPixel(i, j);

                    int r, g, b, x;

                    r = c.R;
                    g = c.G;
                    b = c.B;
                    x = (r + g + b) / 3;
                    //0=111

                    //x=50

                    if (o < x - 10 && o < x + 10)
                    {
                        sourceImage.SetPixel(i, j, Color.FromArgb(0, 0, 0));

                    }
                    else
                    {
                        //  sourceImage.SetPixel(i, j, Color.FromArgb(r, g, b));
                        sourceImage.SetPixel(i, j, Color.FromArgb(255, 255, 255));

                    }

                    if (/*i%2==0 &&*/ j % 2 == 0)
                        o = x;
                }
            }
        }
        public void fMozaik(Int32 pixelateSize)
        {

            Color c;
            // make an exact copy of the bitmap provided
            //using (Graphics graphics = System.Drawing.Graphics.FromImage(sourceImage))
            //    graphics.DrawImage(sourceImage, new System.Drawing.Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
            //        new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), GraphicsUnit.Pixel);


            Rectangle rectangle = new Rectangle(0, 0, sourceImage.Width, sourceImage.Height);
            // look at every pixel in the rectangle while making sure we're within the image bounds
            for (Int32 xx = rectangle.X; xx < rectangle.X + rectangle.Width && xx < sourceImage.Width; xx += pixelateSize)
            {
                for (Int32 yy = rectangle.Y; yy < rectangle.Y + rectangle.Height && yy < sourceImage.Height; yy += pixelateSize)
                {
                    Int32 offsetX = pixelateSize / 2;
                    Int32 offsetY = pixelateSize / 2;
                    // make sure that the offset is within the boundry of the image
                    while (xx + offsetX >= sourceImage.Width) offsetX--;
                    while (yy + offsetY >= sourceImage.Height) offsetY--;

                    // get the pixel color in the center of the soon to be pixelated area
                    Color pixel = sourceImage.GetPixel(xx + offsetX, yy + offsetY);
                    // for each pixel in the pixelate size, set it to the center color
                    for (Int32 x = xx; x < xx + pixelateSize && x < sourceImage.Width; x++)
                        for (Int32 y = yy; y < yy + pixelateSize && y < sourceImage.Height; y++)
                            sourceImage.SetPixel(x, y, pixel);


                    //for (int i = 0; i < sourceImage.Width; i++)
                    //{
                    //    for (int j = 0; j < sourceImage.Height; j++)
                    //    {

                    //        c = sourceImage.GetPixel(i, j);

                    //        sourceImage.SetPixel(i, j, c);

                }
            }
        }
        public void fBlur(Int32 blurSize)
        {
            Rectangle rectangle = new Rectangle(0, 0, sourceImage.Width, sourceImage.Height);

            // look at every pixel in the blur rectangle
            for (Int32 xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
            {
                for (Int32 yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
                {
                    Int32 avgR = 0, avgG = 0, avgB = 0;
                    Int32 blurPixelCount = 0;

                    // average the color of the red, green and blue for each pixel in the
                    // blur size while making sure you don't go outside the image bounds
                    for (Int32 x = xx; (x < xx + blurSize && x < sourceImage.Width); x++)
                    {
                        for (Int32 y = yy; (y < yy + blurSize && y < sourceImage.Height); y++)
                        {
                            Color pixel = sourceImage.GetPixel(x, y);

                            avgR += pixel.R;
                            avgG += pixel.G;
                            avgB += pixel.B;

                            blurPixelCount++;
                        }
                    }
                    avgR = avgR / blurPixelCount;
                    avgG = avgG / blurPixelCount;
                    avgB = avgB / blurPixelCount;

                    // now that we know the average for the blur size, set each pixel to that color
                    for (Int32 x = xx; x < xx + blurSize && x < sourceImage.Width && x < rectangle.Width; x++)
                        for (Int32 y = yy; y < yy + blurSize && y < sourceImage.Height && y < rectangle.Height; y++)
                            sourceImage.SetPixel(x, y, Color.FromArgb(avgR, avgG, avgB));
                }
            }





        }
        public void fDithering(int contrast)
        {
            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    c = sourceImage.GetPixel(i, j);


                    int luma = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                    if (luma > new byte[] { 0, 192, 48, 240, 128, 64, 176, 112, 32, 224, 16, 208, 160, 96, 144, 80 }[(i % 4) + ((j % 4) * 4)])
                    {
                        //bmap.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                        c = sourceImage.GetPixel(i, j);
                        double pR = c.R / 255.0;
                        pR -= 0.5;
                        pR *= contrast;
                        pR += 0.5;
                        pR *= 255;
                        if (pR < 0) pR = 0;
                        if (pR > 255) pR = 255;

                        double pG = c.G / 255.0;
                        pG -= 0.5;
                        pG *= contrast;
                        pG += 0.5;
                        pG *= 255;
                        if (pG < 0) pG = 0;
                        if (pG > 255) pG = 255;

                        double pB = c.B / 255.0;
                        pB -= 0.5;
                        pB *= contrast;
                        pB += 0.5;
                        pB *= 255;
                        if (pB < 0) pB = 0;
                        if (pB > 255) pB = 255;

                        sourceImage.SetPixel(i, j,
                   Color.FromArgb((byte)pR, (byte)pG, (byte)pB));
                    }
                    //else { bmap.SetPixel(i, j, Color.FromArgb(0, 0, 0)); }

                }
            }
        }
        public void fRedGrayscale()
        {
            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    c = sourceImage.GetPixel(i, j);
                    byte gray = (byte)(.299 * c.R + .587 * c.G + .114 * c.B);
                    if (c.R + 50 > 255)
                    {
                        sourceImage.SetPixel(i, j, Color.FromArgb(255, gray, gray));
                    }
                    else
                    {
                        //ilk bmap.SetPixel(i, j, Color.FromArgb(255, gray, gray));
                        sourceImage.SetPixel(i, j, Color.FromArgb(c.R + 50, gray, gray));
                    }
                }
            }
        }
        public void fSiyahOran(int oran)
        {
            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    c = sourceImage.GetPixel(i, j);
                    int luma = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                    if (luma < oran)
                    {
                        sourceImage.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                    }
                    else
                    {
                        sourceImage.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    }
                }
            }
        }
        public void fBlueGrayscale()
        {
            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    c = sourceImage.GetPixel(i, j);
                    byte gray = (byte)(.299 * c.R + .587 * c.G + .114 * c.B);
                    if (c.B + 50 > 255)
                    {
                        sourceImage.SetPixel(i, j, Color.FromArgb(gray, gray, 255));
                    }
                    else
                    {
                        sourceImage.SetPixel(i, j, Color.FromArgb(gray, gray, c.B + 50));
                    }
                }
            }
        }
        public void fGreenGrayscale()
        {
            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    c = sourceImage.GetPixel(i, j);
                    byte gray = (byte)(.299 * c.R + .587 * c.G + .114 * c.B);
                    if (c.G + 50 > 255)
                    {
                        sourceImage.SetPixel(i, j, Color.FromArgb(gray, 255, gray));
                    }
                    else
                    {
                        sourceImage.SetPixel(i, j, Color.FromArgb(gray, c.G + 50, gray));
                    }
                }
            }
        }
        public void fDalga()
        {
            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    c = sourceImage.GetPixel(i, j);
                    //sourceImage.SetPixel(i, j, Color.FromArgb(MaxChannel(c), MaxChannel(c), MaxChannel(c)));
                    int dX = Math.Min(i, sourceImage.Width - i);
                    int dY = Math.Min(j, sourceImage.Height - j);

                    double k = fade * Math.Sin(j * period);
                    Color color = sourceImage.GetPixel(i, j);
                    sourceImage.SetPixel(i, j, SuperpositionColor(GetGrayColor(color), color, k));

                }
            }
        }
        public void fMaxRenk()
        {
            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    c = sourceImage.GetPixel(i, j);
                    sourceImage.SetPixel(i, j, Color.FromArgb(MaxChannel(c), MaxChannel(c), MaxChannel(c)));
                }
            }
        }
        public void fMxRenk()
        {
            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    c = sourceImage.GetPixel(i, j);

                    sourceImage.SetPixel(i, j, MaksimumRenk(c));

                }
            }
        }
        public void fMnRenk()
        {
            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    c = sourceImage.GetPixel(i, j);

                    sourceImage.SetPixel(i, j, MinimumRenk(c));

                }
            }
        }
        public void fEqRenk()
        {
            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    c = sourceImage.GetPixel(i, j);

                    sourceImage.SetPixel(i, j, EsitRenk(c));

                }
            }
        }
        public void fMinRenk()
        {
            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    c = sourceImage.GetPixel(i, j);
                    sourceImage.SetPixel(i, j, Color.FromArgb(MinChannel(c), MinChannel(c), MinChannel(c)));
                }
            }
        }
        public void fRenkKoru(Color R, int tol)
        {
            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    c = sourceImage.GetPixel(i, j);
                    //yaz(i.ToString(), j.ToString(), c.A.ToString(), c.R.ToString(), c.G.ToString(), c.B.ToString());
                    //xxx += c.ToString();
                    if (!tolerans(R, c.R, c.G, c.B, tol))
                    {
                        byte gray = (byte)(.299 * c.R + .587 * c.G + .114 * c.B);
                        sourceImage.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                    }
                }
            }
        }
        public void fSinCity(int tol)
        {
            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    c = sourceImage.GetPixel(i, j);
                    //yaz(i.ToString(), j.ToString(), c.A.ToString(), c.R.ToString(), c.G.ToString(), c.B.ToString());
                    //xxx += c.ToString();
                    int Cr = (c.R * 65536) + (c.G * 256) + c.B;
                    //if (!tolerans2(c.G, c.B, tol))
                    if (Cr < 7849840)
                    {
                        byte gray = (byte)(.299 * c.R + .587 * c.G + .114 * c.B);
                        sourceImage.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                    }
                }
            }
        }
        public void frters()
        {
            string xxx = "";

            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {

                    c = sourceImage.GetPixel(i, j);

                    sourceImage.SetPixel(i, j, Color.FromArgb(255 - c.R, c.G, c.B));

                }
            }


        }
        public void fSiyahBeyaz()
        {
            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    c = sourceImage.GetPixel(i, j);

                    byte gray = (byte)(.299 * c.R + .587 * c.G + .114 * c.B);
                    sourceImage.SetPixel(i, j, Color.FromArgb(gray, gray, gray));

                }
            }
        }
        public bool tolerans(Color c, byte r, byte g, byte b, int t)
        {
            Boolean rv;

            //R=200
            //r=155
            //t=50
            if (((c.R > r - t) && (c.R < r + t)) && ((c.G > g - t) && (c.G < g + t)) && ((c.B > b - t) && (c.B < b + t)))
            //if ((c.R < r + t && c.R > r - t) && (c.G < g + t && c.G > g - t) && (c.B < b + t && c.B > b - t))
            {
                rv = true;
            }
            else { rv = false; }

            return rv;

        }
        public bool tolerans2(byte a, byte b, int t)
        {
            Boolean rv;

            //R=200
            //r=155
            //t=50
            if ((a < b + t && a > b - t)/*&&(b<a+t&&b>a-t)*/)
            {
                rv = true;
            }
            else { rv = false; }

            return rv;

        }
        public void fTersRG()
        {
            string xxx = "";

            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {

                    c = sourceImage.GetPixel(i, j);

                    sourceImage.SetPixel(i, j, Color.FromArgb(c.G, c.R, c.B));

                }
            }


        }
        public void fTersRB()
        {
            string xxx = "";

            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {

                    c = sourceImage.GetPixel(i, j);

                    sourceImage.SetPixel(i, j, Color.FromArgb(c.B, c.G, c.R));

                }
            }


        }
        public void fTersGB()
        {
            string xxx = "";

            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {

                    c = sourceImage.GetPixel(i, j);

                    sourceImage.SetPixel(i, j, Color.FromArgb(c.R, c.B, c.G));

                }
            }
        }
        public void fR100()
        {
            string xxx = "";

            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {

                for (int j = 0; j < sourceImage.Height; j++)
                {

                    c = sourceImage.GetPixel(i, j);

                    sourceImage.SetPixel(i, j, Color.FromArgb(100, c.G, c.B));

                }
            }
        }
        public void fG100()
        {
            string xxx = "";

            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {

                for (int j = 0; j < sourceImage.Height; j++)
                {

                    c = sourceImage.GetPixel(i, j);

                    sourceImage.SetPixel(i, j, Color.FromArgb(c.R, 100, c.B));

                }
            }
        }
        public void fB100()
        {
            string xxx = "";

            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {

                for (int j = 0; j < sourceImage.Height; j++)
                {

                    c = sourceImage.GetPixel(i, j);

                    sourceImage.SetPixel(i, j, Color.FromArgb(c.R, c.G, 100));

                }
            }
        }


        //        void Sobel(int type)
        //        {
        //            int imageWidth = sourceImage.Width;
        //            int imageHeight = sourceImage.Height;
        //            int[,] greyPixelArray = new int[imageHeight, imageWidth];

        //            int normalizeMax = 0;
        //            int normalizeMin = 5;// maxIntVal;
        //            int[,] sobelPixelArray = new int[imageWidth, imageHeight];
        //            int[,] sobelArray1 =
        //            new int[3, 3] {
        //{ 1, 0, -1 },
        //{ 2, 0, -2 },
        //{ 1, 0, -1 }
        //};

        //            int[,] sobelArray2 =
        //            new int[3, 3] {
        //{ 1, 2, 1 },
        //{ 0, 0, 0 },
        //{-1,-2,-1 }
        //};
        //            int[,] sobelArray3 =
        //            new int[3, 3] {
        //{ 2, 1, 0 },
        //{ 1, 0,-1 },
        //{ 0,-1,-2 }
        //};

        //            int G1, G2, G3;
        //            for (int i = 1; i < imageWidth - 1; i++)
        //            {
        //                for (int j = 1; j < imageHeight - 1; j++)
        //                {
        //                    G1 = 0;
        //                    G2 = 0;
        //                    G3 = 0;
        //                    for (int k = -1; k < 2; k++)
        //                    {
        //                        for (int l = -1; l < 2; l++)
        //                        {
        //                            G1 += greyPixelArray[i + k, j + l] * sobelArray1[k + 1, l + 1];
        //                            G2 += greyPixelArray[i + k, j + l] * sobelArray2[k + 1, l + 1];
        //                            G3 += greyPixelArray[i + k, j + l] * sobelArray3[k + 1, l + 1];
        //                        }
        //                    }
        //                    if (type == 0)
        //                    {
        //                        sobelPixelArray[i, j] = Math.Abs(G1) + Math.Abs(G2) + Math.Abs(G3);
        //                    }
        //                    else if (type == 1)
        //                    {
        //                        sobelPixelArray[i, j] = Math.Abs(G2);
        //                    }
        //                    else if (type == 2)
        //                    {
        //                        sobelPixelArray[i, j] = Math.Abs(G1);
        //                    }
        //                    else if (type == 3)
        //                    {
        //                        sobelPixelArray[i, j] = Math.Abs(G3);
        //                    }

        //                    if (normalizeMax < sobelPixelArray[i, j])
        //                        normalizeMax = sobelPixelArray[i, j];
        //                    if (normalizeMin > sobelPixelArray[i, j])
        //                        normalizeMin = sobelPixelArray[i, j];
        //                }
        //            }

        //            NormalizeArray(ref sobelPixelArray, normalizeMax, normalizeMin);
        //        }

        //        void NormalizeArray(ref int[,] sourceArray, int normalizeMax, int normalizeMin)
        //        {

        //            int factor = normalizeMax - normalizeMin;
        //            for (int i = 0; i < sourceArray.GetLength(0); i++)
        //            {
        //                for (int j = 0; j < sourceArray.GetLength(1); j++)
        //                {
        //                    sourceArray[i, j] = (sourceArray[i, j] - normalizeMin) * 255 / factor;
        //                }
        //            }
        //        }



        public void Mean()
        {
            int imageWidth = sourceImage.Width;
            int imageHeight = sourceImage.Height;
            int[,] pixelArray = new int[imageWidth, imageHeight];
            int[,] greyPixelArray = new int[imageWidth, imageHeight];
            int[,] sourceArray;
            //private void BuildPixelArray(Image myImage)
            //{
            Rectangle rect1 = new Rectangle(0, 0, sourceImage.Width, sourceImage.Height);
            Bitmap temp1 = new Bitmap(sourceImage);
            BitmapData bmpData1 = temp1.LockBits(rect1, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int remain1 = bmpData1.Stride - bmpData1.Width * 3;
            unsafe
            {
                byte* ptr = (byte*)bmpData1.Scan0;
                for (int i = 0; i < bmpData1.Width; i++)
                {
                    for (int j = 0; j < bmpData1.Height; j++)
                    {
                        pixelArray[i, j] = (ptr[0]) + (ptr[1]) * 255 + (ptr[2]) * 255 * 255;
                        greyPixelArray[i, j] = (int)((double)ptr[0] * 0.11 + (double)ptr[1] * 0.59 + (double)ptr[2] * 0.3);
                        //pixelArray[i, j] = (int)ptr;// (int)((double)ptr[0] + (double)ptr[1] + (double)ptr[2]);
                        ptr += 3;
                    }
                    ptr += remain1;
                }
            }
            temp1.UnlockBits(bmpData1);







            int[,] robertPixelArray;

            robertPixelArray = new int[imageWidth, imageHeight];
            for (int i = 1; i < imageHeight - 1; i++)
            {
                for (int j = 1; j < imageWidth - 1; j++)
                {
                    robertPixelArray[j, i] = Math.Abs(pixelArray[j, i] - pixelArray[j - 1, i - 1]) + Math.Abs(pixelArray[j, i - 1] - pixelArray[j - 1, i]);
                }
            }






            sourceArray = robertPixelArray;
            Rectangle rect = new Rectangle(0, 0, sourceArray.GetLength(0), sourceArray.GetLength(1));
            Bitmap temp = new Bitmap(sourceArray.GetLength(0), sourceArray.GetLength(1));
            BitmapData bmpData = temp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int remain = bmpData.Stride - bmpData.Width * 3;
            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;
                for (int i = 0; i < bmpData.Width; i++)
                {
                    for (int j = 0; j < bmpData.Height; j++)
                    {
                        //65536
                        ptr[0] = (byte)(sourceArray[i, j] % 255);
                        ptr[1] = (byte)((sourceArray[i, j] / 255) % 255);
                        ptr[2] = (byte)((sourceArray[i, j] / 65025) % 255);
                        ptr += 3;
                    }
                    ptr += remain;
                }
            }
            temp.UnlockBits(bmpData);
            sourceImage = temp;




            //}

            //        


        }


        public void SobelEdgeDetect()
        {        //            int imageWidth = sourceImage.Width;
            //            int imageHeight = sourceImage.Height;
            Bitmap b = sourceImage;
            Bitmap bb = sourceImage;
            int width = b.Width;
            int height = b.Height;
            int[,] gx = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] gy = new int[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };

            int[,] allPixR = new int[width, height];
            int[,] allPixG = new int[width, height];
            int[,] allPixB = new int[width, height];

            int limit = 128 * 128;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    allPixR[i, j] = b.GetPixel(i, j).R;
                    allPixG[i, j] = b.GetPixel(i, j).G;
                    allPixB[i, j] = b.GetPixel(i, j).B;
                }
            }

            int new_rx = 0, new_ry = 0;
            int new_gx = 0, new_gy = 0;
            int new_bx = 0, new_by = 0;
            int rc, gc, bc;
            for (int i = 1; i < b.Width - 1; i++)
            {
                for (int j = 1; j < b.Height - 1; j++)
                {

                    new_rx = 0;
                    new_ry = 0;
                    new_gx = 0;
                    new_gy = 0;
                    new_bx = 0;
                    new_by = 0;
                    rc = 0;
                    gc = 0;
                    bc = 0;

                    for (int wi = -1; wi < 2; wi++)
                    {
                        for (int hw = -1; hw < 2; hw++)
                        {
                            rc = allPixR[i + hw, j + wi];
                            new_rx += gx[wi + 1, hw + 1] * rc;
                            new_ry += gy[wi + 1, hw + 1] * rc;

                            gc = allPixG[i + hw, j + wi];
                            new_gx += gx[wi + 1, hw + 1] * gc;
                            new_gy += gy[wi + 1, hw + 1] * gc;

                            bc = allPixB[i + hw, j + wi];
                            new_bx += gx[wi + 1, hw + 1] * bc;
                            new_by += gy[wi + 1, hw + 1] * bc;
                        }
                    }
                    if (new_rx * new_rx + new_ry * new_ry > limit || new_gx * new_gx + new_gy * new_gy > limit || new_bx * new_bx + new_by * new_by > limit)
                        bb.SetPixel(i, j, Color.Black);

                    //bb.SetPixel (i, j, Color.FromArgb(allPixR[i,j],allPixG[i,j],allPixB[i,j]));
                    else
                        bb.SetPixel(i, j, Color.Transparent);
                }
            }
            sourceImage = bb;

        }




        public void Emboss()
        {
            string xxx = "";

            Color renk1;
            Color renk2, renk3; int r, g, b;
            for (int i = 0; i < sourceImage.Width - 2; i++)
            {
                for (int j = 0; j < sourceImage.Height - 2; j++)
                {

                    // sourceImage.SetPixel(i, j, Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B));

                    renk1 = sourceImage.GetPixel(i, j);//i,j noktasının rengini öğren
                    renk2 = sourceImage.GetPixel(i + 1, j + 1);//sonraki noktanın rengini öğren
                    r = Math.Abs((int)(renk1.R) - renk2.R) + 128;
                    if (r > 255)
                        r = 255;
                    g = Math.Abs((int)(renk1.G) - renk2.G) + 128;
                    if (g > 255)
                        g = 255;
                    b = Math.Abs((int)(renk1.B) - renk2.B) + 128;
                    if (b > 255)
                        b = 255;
                    renk3 = Color.FromArgb(r, g, b);
                    sourceImage.SetPixel(i, j, renk3);




                }
            }


        }


        public void EmbossEdge_Or2(int f)
        {
            string xxx = "";

            Color renk1;
            Color renk2, renk3; int r, g, b;
            for (int i = 0; i < sourceImage.Width - 2; i++)
            {
                for (int j = 0; j < sourceImage.Height - 2; j++)
                {
                    renk1 = sourceImage.GetPixel(i, j);//i,j noktasının rengini öğren
                    renk2 = sourceImage.GetPixel(i + 1, j);//sonraki noktanın rengini öğren
                    r = Math.Abs((int)(renk1.R) - renk2.R) /*+ 128*/;
                    if (r > 255)
                        r = 255;
                    g = Math.Abs((int)(renk1.G) - renk2.G) /*+ 128*/;
                    if (g > 255)
                        g = 255;
                    b = Math.Abs((int)(renk1.B) - renk2.B) /*+ 128*/;
                    if (b > 255)
                        b = 255;
                    //renk3 = Color.FromArgb(r, g, b);
                    //  int f = 10;

                    if (r < f || g < f || b < f)
                    {
                        renk3 = Color.Black;
                    }
                    else
                    {
                        renk3 = Color.White;
                    }
                    sourceImage.SetPixel(i, j, renk3);
                }
            }
        }




        public void EmbossEdge_And2(int f)
        {
            string xxx = "";

            Color renk1;
            Color renk2, renk3; int r, g, b;
            for (int i = 0; i < sourceImage.Width - 2; i++)
            {
                for (int j = 0; j < sourceImage.Height - 2; j++)
                {
                    renk1 = sourceImage.GetPixel(i, j);//i,j noktasının rengini öğren
                    renk2 = sourceImage.GetPixel(i + 1, j);//sonraki noktanın rengini öğren
                    r = Math.Abs((int)(renk1.R) - renk2.R) /*+ 128*/;
                    if (r > 255)
                        r = 255;
                    g = Math.Abs((int)(renk1.G) - renk2.G) /*+ 128*/;
                    if (g > 255)
                        g = 255;
                    b = Math.Abs((int)(renk1.B) - renk2.B) /*+ 128*/;
                    if (b > 255)
                        b = 255;
                    //renk3 = Color.FromArgb(r, g, b);
                    //  int f = 10;

                    //if (r < f || g < f || b < f)
                    if (r < f && g < f && b < f)
                    {
                        renk3 = Color.Black;
                    }
                    else
                    {
                        renk3 = Color.White;
                    }
                    sourceImage.SetPixel(i, j, renk3);
                }
            }
        }

        public void EmbossEdge_And(int f)
        {
            string xxx = "";

            Color renk1;
            Color renk2, renk3; int r, g, b;
            for (int i = 0; i < sourceImage.Width - 2; i++)
            {
                for (int j = 0; j < sourceImage.Height - 2; j++)
                {
                    renk1 = sourceImage.GetPixel(i, j);//i,j noktasının rengini öğren
                    renk2 = sourceImage.GetPixel(i + 1, j + 1);//sonraki noktanın rengini öğren
                    r = Math.Abs((int)(renk1.R) - renk2.R) /*+ 128*/;
                    if (r > 255)
                        r = 255;
                    g = Math.Abs((int)(renk1.G) - renk2.G) /*+ 128*/;
                    if (g > 255)
                        g = 255;
                    b = Math.Abs((int)(renk1.B) - renk2.B) /*+ 128*/;
                    if (b > 255)
                        b = 255;
                    //renk3 = Color.FromArgb(r, g, b);
                    //  int f = 10;

                    //if (r < f || g < f || b < f)
                    if (r < f && g < f && b < f)
                    {
                        renk3 = Color.Black;
                    }
                    else
                    {
                        renk3 = Color.White;
                    }
                    sourceImage.SetPixel(i, j, renk3);
                }
            }
        }


        public void EmbossEdge_Or(int f)
        {
            string xxx = "";

            Color renk1;
            Color renk2, renk3; int r, g, b;
            for (int i = 0; i < sourceImage.Width - 2; i++)
            {
                for (int j = 0; j < sourceImage.Height - 2; j++)
                {
                    renk1 = sourceImage.GetPixel(i, j);//i,j noktasının rengini öğren
                    renk2 = sourceImage.GetPixel(i + 1, j + 1);//sonraki noktanın rengini öğren
                    r = Math.Abs((int)(renk1.R) - renk2.R) /*+ 128*/;
                    if (r > 255)
                        r = 255;
                    g = Math.Abs((int)(renk1.G) - renk2.G) /*+ 128*/;
                    if (g > 255)
                        g = 255;
                    b = Math.Abs((int)(renk1.B) - renk2.B) /*+ 128*/;
                    if (b > 255)
                        b = 255;
                    //renk3 = Color.FromArgb(r, g, b);
                    //  int f = 10;

                    if (r < f || g < f || b < f)
                    {
                        renk3 = Color.Black;
                    }
                    else
                    {
                        renk3 = Color.White;
                    }
                    sourceImage.SetPixel(i, j, renk3);
                }
            }
        }







        public void Oil()
        {

            SobelEdgeDetect();
            string xxx = "";
            int r, g, b;
            Color c, y;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {

                    c = sourceImage.GetPixel(i, j);

                    if (c != Color.Black)
                    {
                        y = yedek.GetPixel(i, j);
                        sourceImage.SetPixel(i, j, y);
                    }
                    else { sourceImage.SetPixel(i, j, c); }
                }
            }



        }

        public void SetGrayscale()
        {
            string xxx = "";

            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    if (i % 4 == 0 && j % 4 == 0)
                    {
                        c = sourceImage.GetPixel(i, j);
                        //byte gray1 = (byte)(((1 + j) / sourceImage.Height) * c.R + ((1 + i) / sourceImage.Width) * c.G + ((1 + i) / (1 + j)) * c.B);
                        //byte gray2 = (byte)(((1 + i) / sourceImage.Width) * c.R + ((1 + j) / sourceImage.Height) * c.G + ((1 + i) / (1 + j)) * c.B);
                        //byte gray3 = (byte)(((1 + j) / sourceImage.Height) * c.G + ((1 + i) / sourceImage.Width) * c.R + ((1 + i) / (1 + j)) * c.B);
                        //sourceImage.SetPixel(i, j, Color.FromArgb(gray1, gray2, gray2));

                        sourceImage.SetPixel(i, j, Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B));
                    }
                }
            }


        }
        public void fSetContrast(double contrast)
        {

            if (contrast < -100) contrast = -100;
            if (contrast > 100) contrast = 100;
            contrast = (100.0 + contrast) / 100.0;
            contrast *= contrast;
            Color c;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    c = sourceImage.GetPixel(i, j);
                    double pR = c.R / 255.0;
                    pR -= 0.5;
                    pR *= contrast;
                    pR += 0.5;
                    pR *= 255;
                    if (pR < 0) pR = 0;
                    if (pR > 255) pR = 255;

                    double pG = c.G / 255.0;
                    pG -= 0.5;
                    pG *= contrast;
                    pG += 0.5;
                    pG *= 255;
                    if (pG < 0) pG = 0;
                    if (pG > 255) pG = 255;

                    double pB = c.B / 255.0;
                    pB -= 0.5;
                    pB *= contrast;
                    pB += 0.5;
                    pB *= 255;
                    if (pB < 0) pB = 0;
                    if (pB > 255) pB = 255;

                    sourceImage.SetPixel(i, j,
        Color.FromArgb((byte)pR, (byte)pG, (byte)pB));
                }
            }

        }





        public void fBalik()
        {

            sourceImage = Apply(sourceImage);
        }






        public enum RGB
        {
            R,
            G,
            B
        }

        public Bitmap Apply(Bitmap srcImg)
        {
            #region üst

            float _curvature = 0.005f;
            if (_curvature <= 0.0f)
            {
                throw new ArgumentException("Curvature Must Be Set and It Must Be > 0.0f");
            }

            // get source image size
            int width = srcImg.Width;
            int height = srcImg.Height;
            int halfWidth = width / 2;
            int halfHeight = height / 2;

            //Obtain the Maxium Size Of the Fish Eye
            int maxRadius = (int)Math.Min(width, height) / 2;
            //The S parameter mentioned in Jason Walton Algo.
            double s = maxRadius / Math.Log(_curvature * maxRadius + 1);

            PixelFormat fmt = (srcImg.PixelFormat == PixelFormat.Format8bppIndexed) ?
                PixelFormat.Format8bppIndexed : PixelFormat.Format24bppRgb;

            // lock source bitmap data
            BitmapData srcData = srcImg.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, fmt);

            // create new image
            //Bitmap dstImg = (fmt == PixelFormat.Format8bppIndexed) ?
            //    Tiger.Imaging.Image.CreateGrayscaleImage(width, height) :
            //    new Bitmap(width, height, fmt);
            Bitmap dstImg = new Bitmap(width, height, fmt);
            // lock destination bitmap data
            BitmapData dstData = dstImg.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, fmt);

            int stride = srcData.Stride;
            int offset = stride - ((fmt == PixelFormat.Format8bppIndexed) ? width : width * 3);

            // Perform The Fish Eye Conversion
            //
            #endregion

            unsafe
            {
                byte* src = (byte*)srcData.Scan0.ToPointer();
                byte* dst = (byte*)dstData.Scan0.ToPointer();
                byte* p;

                if (fmt == PixelFormat.Format8bppIndexed)
                {
                    #region Grayscale
                    for (int y = -1 * halfHeight; y < height - halfHeight; y++)
                    {
                        for (int x = -1 * halfWidth; x < width - halfWidth; x++)
                        {
                            //Get the Current Pixels Polar Co-ordinates
                            double radius = Math.Sqrt(x * x + y * y);
                            double angle = Math.Atan2(y, x);

                            //Check to see if the polar pixel is out of bounds
                            if (radius <= maxRadius)
                            {
                                //Current Pixel is inside the Fish Eye
                                //newRad is the pixel that we want to shift to current pixel based on the fish eye posistion
                                double newRad = (Math.Exp(radius / s) - 1) / _curvature;
                                int newX = (int)(newRad * Math.Cos(angle));
                                int newY = (int)(newRad * Math.Sin(angle));

                                newX += halfWidth;
                                newY += halfHeight;

                                p = src + newY * stride + newX;
                                byte* dst2 = dst + (y + halfHeight) * stride + (x + halfWidth);
                                dst2 = p;

                            }
                            else
                            {
                                *dst = src[y * stride + x];
                            }

                        }
                        dst += offset;
                    }
                    #endregion
                }
                else
                {
                    #region RGB
                    //Start from the middle because point (0,0) in cartesian co-ordinates should not be in the center
                    for (int y = -1 * halfHeight; y < height - halfHeight; y++)
                    {
                        for (int x = -1 * halfWidth; x < width - halfWidth; x++)
                        {
                            //Get the Current Pixels Polar Co-ordinates
                            double radius = Math.Sqrt(x * x + y * y);
                            double angle = Math.Atan2(y, x);

                            //Check to see if the polar pixel is out of bounds
                            if (radius <= maxRadius)
                            {
                                //Current Pixel is inside the Fish Eye

                                //newRad is the pixel that we want to shift to current pixel based on the fish eye posistion
                                double newRad = (Math.Exp(radius / s) - 1) / _curvature;
                                //Current Pixels Polar Cordinates Back To Cartesian
                                int newX = (int)(newRad * Math.Cos(angle));
                                int newY = (int)(newRad * Math.Sin(angle));

                                newX += halfWidth;
                                newY += halfHeight;

                                //Chage the Pointer to the Src
                                p = src + newY * stride + newX * 3;
                                //Change the Pointer to the destination
                                byte* dst2 = dst + (y + halfHeight) * stride + (x + halfWidth) * 3;
                                dst2[(byte)RGB.R] = p[(byte)RGB.R];
                                dst2[(byte)RGB.G] = p[(byte)RGB.G];
                                dst2[(byte)RGB.B] = p[(byte)RGB.B];

                            }
                            else
                            {
                                //Current Pixel is outside the Fish Eye thus should be output as is
                                p = src + (y + halfHeight) * stride + (x + halfWidth) * 3;

                                //Change the Pointer to the destination
                                byte* dst2 = dst + (y + halfHeight) * stride + (x + halfWidth) * 3;

                                dst2[(byte)RGB.R] = p[(byte)RGB.R];
                                dst2[(byte)RGB.G] = p[(byte)RGB.G];
                                dst2[(byte)RGB.B] = p[(byte)RGB.B];
                            }
                        }
                    }
                    #endregion
                }
            }
            // unlock both images












            dstImg.UnlockBits(dstData);
            srcImg.UnlockBits(srcData);

            return dstImg;
        }




















        public void OilPaintFilter(
                                           int levels,
                                           int filterSize)
        {

            Bitmap sourceBitmap = sourceImage;
            BitmapData sourceData =
                       sourceBitmap.LockBits(new Rectangle(0, 0,
                       sourceBitmap.Width, sourceBitmap.Height),
                       ImageLockMode.ReadOnly,
                       PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride *
                                          sourceData.Height];


            byte[] resultBuffer = new byte[sourceData.Stride *
                                           sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0,
                                       pixelBuffer.Length);


            sourceBitmap.UnlockBits(sourceData);


            int[] intensityBin = new int[levels];
            int[] blueBin = new int[levels];
            int[] greenBin = new int[levels];
            int[] redBin = new int[levels];


            levels = levels - 1;


            int filterOffset = (filterSize - 1) / 2;
            int byteOffset = 0;
            int calcOffset = 0;
            int currentIntensity = 0;
            int maxIntensity = 0;
            int maxIndex = 0;


            double blue = 0;
            double green = 0;
            double red = 0;


            for (int offsetY = filterOffset; offsetY <
                sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX <
                    sourceBitmap.Width - filterOffset; offsetX++)
                {
                    blue = green = red = 0;


                    currentIntensity = maxIntensity = maxIndex = 0;


                    intensityBin = new int[levels + 1];
                    blueBin = new int[levels + 1];
                    greenBin = new int[levels + 1];
                    redBin = new int[levels + 1];


                    byteOffset = offsetY *
                    sourceData.Stride + offsetX * 4;


                    for (int filterY = -filterOffset;
                        filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset;
                            filterX <= filterOffset; filterX++)
                        {
                            calcOffset = byteOffset +
                                         (filterX * 4) +
                                         (filterY * sourceData.Stride);


                            currentIntensity = (int)Math.Round(((double)
                                       (pixelBuffer[calcOffset] +
                                       pixelBuffer[calcOffset + 1] +
                                       pixelBuffer[calcOffset + 2]) / 3.0 *
                                       (levels)) / 255.0);


                            intensityBin[currentIntensity] += 1;
                            blueBin[currentIntensity] += pixelBuffer[calcOffset];
                            greenBin[currentIntensity] += pixelBuffer[calcOffset + 1];
                            redBin[currentIntensity] += pixelBuffer[calcOffset + 2];


                            if (intensityBin[currentIntensity] > maxIntensity)
                            {
                                maxIntensity = intensityBin[currentIntensity];
                                maxIndex = currentIntensity;
                            }
                        }
                    }


                    blue = blueBin[maxIndex] / maxIntensity;
                    green = greenBin[maxIndex] / maxIntensity;
                    red = redBin[maxIndex] / maxIntensity;


                    resultBuffer[byteOffset] = ClipByte(blue);
                    resultBuffer[byteOffset + 1] = ClipByte(green);
                    resultBuffer[byteOffset + 2] = ClipByte(red);
                    resultBuffer[byteOffset + 3] = 255;

                }
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width,
                                             sourceBitmap.Height);


            BitmapData resultData =
                       resultBitmap.LockBits(new Rectangle(0, 0,
                       resultBitmap.Width, resultBitmap.Height),
                       ImageLockMode.WriteOnly,
                       PixelFormat.Format32bppArgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0,
                                       resultBuffer.Length);


            resultBitmap.UnlockBits(resultData);


            sourceImage = resultBitmap;
        }




        private byte ClipByte(double colour)
        {
            return (byte)(colour > 255 ? 255 :
                   (colour < 0 ? 0 : colour));
        }



        private static bool CheckThreshold(byte[] pixelBuffer,
                                   int offset1, int offset2,
                                   ref int gradientValue,
                                   byte threshold,
                                   int divideBy)
        {
            gradientValue +=
            Math.Abs(pixelBuffer[offset1] -
            pixelBuffer[offset2]) / divideBy;


            gradientValue +=
            Math.Abs(pixelBuffer[offset1 + 1] -
            pixelBuffer[offset2 + 1]) / divideBy;


            gradientValue +=
            Math.Abs(pixelBuffer[offset1 + 2] -
            pixelBuffer[offset2 + 2]) / divideBy;


            return (gradientValue >= threshold);
        }

        public void GradientBasedEdgeDetectionFilter(byte threshold)
        {
            Bitmap sourceBitmap = sourceImage;

            // = 0;
            BitmapData sourceData =
                       sourceBitmap.LockBits(new Rectangle(0, 0,
                       sourceBitmap.Width, sourceBitmap.Height),
                       ImageLockMode.ReadOnly,
                       PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);


            int sourceOffset = 0, gradientValue = 0;
            bool exceedsThreshold = false;


            for (int offsetY = 1; offsetY < sourceBitmap.Height - 1; offsetY++)
            {
                for (int offsetX = 1; offsetX < sourceBitmap.Width - 1; offsetX++)
                {
                    sourceOffset = offsetY * sourceData.Stride + offsetX * 4;
                    gradientValue = 0;
                    exceedsThreshold = true;


                    // Horizontal Gradient 
                    CheckThreshold(pixelBuffer,
                                   sourceOffset - 4,
                                   sourceOffset + 4,
                                   ref gradientValue, threshold, 2);
                    // Vertical Gradient 
                    exceedsThreshold =
                    CheckThreshold(pixelBuffer,
                                   sourceOffset - sourceData.Stride,
                                   sourceOffset + sourceData.Stride,
                                   ref gradientValue, threshold, 2);


                    if (exceedsThreshold == false)
                    {
                        gradientValue = 0;


                        // Horizontal Gradient 
                        exceedsThreshold =
                        CheckThreshold(pixelBuffer,
                                       sourceOffset - 4,
                                       sourceOffset + 4,
                                       ref gradientValue, threshold, 1);


                        if (exceedsThreshold == false)
                        {
                            gradientValue = 0;

                            // Vertical Gradient 
                            exceedsThreshold =
                            CheckThreshold(pixelBuffer,
                                           sourceOffset - sourceData.Stride,
                                           sourceOffset + sourceData.Stride,
                                           ref gradientValue, threshold, 1);


                            if (exceedsThreshold == false)
                            {
                                gradientValue = 0;

                                // Diagonal Gradient : NW-SE 
                                CheckThreshold(pixelBuffer,
                                               sourceOffset - 4 - sourceData.Stride,
                                               sourceOffset + 4 + sourceData.Stride,
                                               ref gradientValue, threshold, 2);
                                // Diagonal Gradient : NE-SW 
                                exceedsThreshold =
                                CheckThreshold(pixelBuffer,
                                               sourceOffset - sourceData.Stride + 4,
                                               sourceOffset - 4 + sourceData.Stride,
                                               ref gradientValue, threshold, 2);


                                if (exceedsThreshold == false)
                                {
                                    gradientValue = 0;

                                    // Diagonal Gradient : NW-SE 
                                    exceedsThreshold =
                                    CheckThreshold(pixelBuffer,
                                                   sourceOffset - 4 - sourceData.Stride,
                                                   sourceOffset + 4 + sourceData.Stride,
                                                   ref gradientValue, threshold, 1);


                                    if (exceedsThreshold == false)
                                    {
                                        gradientValue = 0;

                                        // Diagonal Gradient : NE-SW 
                                        exceedsThreshold =
                                        CheckThreshold(pixelBuffer,
                                                       sourceOffset - sourceData.Stride + 4,
                                                       sourceOffset + sourceData.Stride - 4,
                                                       ref gradientValue, threshold, 1);
                                    }
                                }
                            }
                        }
                    }


                    resultBuffer[sourceOffset] = (byte)(exceedsThreshold ? 255 : 0);
                    resultBuffer[sourceOffset + 1] = resultBuffer[sourceOffset];
                    resultBuffer[sourceOffset + 2] = resultBuffer[sourceOffset];
                    resultBuffer[sourceOffset + 3] = 255;
                }
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);


            sourceImage = resultBitmap;
        }



        public void CartoonFilter(int filterSize, int levels,

                                       byte threshold)
        {
            //      Rose: Oil Painting, Filter 9, Levels 25, Cartoon Threshold 25
            //Roses: Oil Painting, Filter 11, Levels 60, Cartoon Threshold 80
            Bitmap sourceBitmap = sourceImage;

            //int levels =25;
            //int filterSize=9;
            //byte threshold=11;


            OilPaintFilter(levels, filterSize);
            Bitmap paintFilterImage = sourceImage;
            sourceImage = yedek;


            GradientBasedEdgeDetectionFilter(threshold);
            Bitmap edgeDetectImage = sourceImage;


            BitmapData paintData =
                       paintFilterImage.LockBits(new Rectangle(0, 0,
                       paintFilterImage.Width, paintFilterImage.Height),
                       ImageLockMode.ReadOnly,
                       PixelFormat.Format32bppArgb);


            byte[] paintPixelBuffer = new byte[paintData.Stride *
                                              paintData.Height];


            Marshal.Copy(paintData.Scan0, paintPixelBuffer, 0,
                                       paintPixelBuffer.Length);


            paintFilterImage.UnlockBits(paintData);


            BitmapData edgeData =
                       edgeDetectImage.LockBits(new Rectangle(0, 0,
                       edgeDetectImage.Width, edgeDetectImage.Height),
                       ImageLockMode.ReadOnly,
                       PixelFormat.Format32bppArgb);


            byte[] edgePixelBuffer = new byte[edgeData.Stride *
                                             edgeData.Height];


            Marshal.Copy(edgeData.Scan0, edgePixelBuffer, 0,
                                      edgePixelBuffer.Length);


            edgeDetectImage.UnlockBits(edgeData);


            byte[] resultBuffer = new byte[edgeData.Stride *
                                             edgeData.Height];


            for (int k = 0; k + 4 < paintPixelBuffer.Length; k += 4)
            {
                if (edgePixelBuffer[k] == 255 ||
                    edgePixelBuffer[k + 1] == 255 ||
                    edgePixelBuffer[k + 2] == 255)
                {
                    resultBuffer[k] = 0;
                    resultBuffer[k + 1] = 0;
                    resultBuffer[k + 2] = 0;
                    resultBuffer[k + 3] = 255;
                }
                else
                {
                    resultBuffer[k] = paintPixelBuffer[k];
                    resultBuffer[k + 1] = paintPixelBuffer[k + 1];
                    resultBuffer[k + 2] = paintPixelBuffer[k + 2];
                    resultBuffer[k + 3] = 255;
                }
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width,
                                             sourceBitmap.Height);


            BitmapData resultData =
                       resultBitmap.LockBits(new Rectangle(0, 0,
                       resultBitmap.Width, resultBitmap.Height),
                       ImageLockMode.WriteOnly,
                       PixelFormat.Format32bppArgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0,
                                       resultBuffer.Length);


            resultBitmap.UnlockBits(resultData);


            sourceImage = resultBitmap;
        }

        #region aritmetik

        public void aritmetik()
        {
            Bitmap r0 = sourceImage;
            GradientBasedEdgeDetectionFilter(27);
            //SobelEdgeDetect();
            Bitmap r1 = sourceImage;
            sourceImage = r0;
            fMnRenk();
            Bitmap r2 = sourceImage;
            r0 = null;


            sourceImage = ArithmeticBlend(r2, r1, ColorCalculationType.Min);
        }

        public enum ColorCalculationType
        {
            Average,
            Add,
            SubtractLeft,
            SubtractRight,
            Difference,
            Multiply,
            Min,
            Max,
            Amplitude
        }
        public Bitmap ArithmeticBlend(Bitmap sourceBitmap, Bitmap blendBitmap,
                                  ColorCalculationType calculationType)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                    sourceBitmap.Width, sourceBitmap.Height),
                                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);


            BitmapData blendData = blendBitmap.LockBits(new Rectangle(0, 0,
                                   blendBitmap.Width, blendBitmap.Height),
                                   ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] blendBuffer = new byte[blendData.Stride * blendData.Height];
            Marshal.Copy(blendData.Scan0, blendBuffer, 0, blendBuffer.Length);
            blendBitmap.UnlockBits(blendData);


            for (int k = 0; (k + 4 < pixelBuffer.Length) &&
                            (k + 4 < blendBuffer.Length); k += 4)
            {
                pixelBuffer[k] = Calculate(pixelBuffer[k],
                                    blendBuffer[k], calculationType);


                pixelBuffer[k + 1] = Calculate(pixelBuffer[k + 1],
                                        blendBuffer[k + 1], calculationType);


                pixelBuffer[k + 2] = Calculate(pixelBuffer[k + 2],
                                        blendBuffer[k + 2], calculationType);
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }
        public static byte Calculate(byte color1, byte color2,
                          ColorCalculationType calculationType)
        {
            byte resultValue = 0;
            int intResult = 0;


            if (calculationType == ColorCalculationType.Add)
            {
                intResult = color1 + color2;
            }
            else if (calculationType == ColorCalculationType.Average)
            {
                intResult = (color1 + color2) / 2;
            }
            else if (calculationType == ColorCalculationType.SubtractLeft)
            {
                intResult = color1 - color2;
            }
            else if (calculationType == ColorCalculationType.SubtractRight)
            {
                intResult = color2 - color1;
            }
            else if (calculationType == ColorCalculationType.Difference)
            {
                intResult = Math.Abs(color1 - color2);
            }
            else if (calculationType == ColorCalculationType.Multiply)
            {
                intResult = (int)((color1 / 255.0 * color2 / 255.0) * 255.0);
            }
            else if (calculationType == ColorCalculationType.Min)
            {
                intResult = (color1 < color2 ? color1 : color2);
            }
            else if (calculationType == ColorCalculationType.Max)
            {
                intResult = (color1 > color2 ? color1 : color2);
            }
            else if (calculationType == ColorCalculationType.Amplitude)
            {
                intResult = (int)(Math.Sqrt(color1 * color1 + color2 * color2)
                                                            / Math.Sqrt(2.0));
            }


            if (intResult < 0)
            {
                resultValue = 0;
            }
            else if (intResult > 255)
            {
                resultValue = 255;
            }
            else
            {
                resultValue = (byte)intResult;
            }


            return resultValue;
        }
        #endregion



        //bitti


    }

}