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

/// <summary>
/// Summary description for SearchEngine
/// </summary>
public class SearchEngine
{
    Resim.ResimIslem rs = new Resim.ResimIslem();
    private int sourceWith;
    public int SourceWith
    {
        get
        {
            //BuildImage();
            return sourceWith;
        }
        set { sourceWith = value; }
    }
    private int sourceHeight;
    public int SourceHeight
    {
        get
        {
            //BuildImage();
            return sourceHeight;
        }
        set { sourceHeight = value; }
    }
    private int oR;
    public int OR { get { return oR; } set { oR = value; } }
    private int oG;
    public int OG { get { return oG; } set { oG = value; } }
    private int oB;
    public int OB { get { return oB; } set { oB = value; } }

    private int oA;
    public int OA { get { return oA; } set { oA = value; } }

    private int oK;
    public int OK { get { return oK; } set { oK = value; } }
    private int px;
    public int Px { get { return px; } set { px = value; } }

    public SearchEngine()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public void ResimOkuYol(string YOL, string names)
    {

        if (rs.ResimOkuYol(YOL) == "1")
        {

            rs.Kucult(500);


            string session = DateTime.Now.Ticks.ToString().Substring(5);
            sourceWith = rs.SourceImage.Width;
            sourceHeight = rs.SourceImage.Height;

            decimal ortaNoktaW = Math.Round((decimal)(sourceWith / 2), 0);
            decimal ortaNoktaH = Math.Round((decimal)(sourceHeight / 2), 0);

            decimal birimH = 0;
            decimal birimW = 0;
            if (sourceWith > sourceHeight)
            {
                birimW = Math.Round((decimal)(sourceWith / 24), 0);
                birimH = Math.Round((decimal)(sourceHeight / 12), 0);
                int w0;
                int h0;
                int h1;
                int w1;
                w0 = (int)birimW * S_11;
                w1 = (int)birimW * S_13;
                h0 = (int)birimH * S_1;
                h1 = (int)birimH * S_3;
                RenkOrtalamaK(1, w0, w1, h0, h1, names, session);

                w0 = (int)birimW * 4;
                w1 = (int)birimW * S_6;
                h0 = (int)birimH * 5;
                h1 = (int)birimH * S_7;
                RenkOrtalamaK(2, w0, w1, h0, h1, names, session);

                w0 = (int)birimW * S_11;
                w1 = (int)birimW * S_13;
                h0 = (int)birimH * 5;
                h1 = (int)birimH * S_7;
                RenkOrtalamaK(3, w0, w1, h0, h1, names, session);

                w0 = (int)birimW * S_18;
                w1 = (int)birimW * S_20;
                h0 = (int)birimH * 5;
                h1 = (int)birimH * S_7;
                RenkOrtalamaK(4, w0, w1, h0, h1, names, session);

                w0 = (int)birimW * S_11;
                w1 = (int)birimW * S_13;
                h0 = (int)birimH * 9;
                h1 = (int)birimH * S_11;
                RenkOrtalamaK(5, w0, w1, h0, h1, names, session);

            }
            if (sourceWith < sourceHeight)
            {
                birimW = Math.Round((decimal)(sourceWith / 12), 0);
                birimH = Math.Round((decimal)(sourceHeight / 24), 0);
                int w0;
                int h0;
                int h1;
                int w1;
                w0 = (int)birimW * 5;
                w1 = (int)birimW * S_7;
                h0 = (int)birimH * 4;
                h1 = (int)birimH * S_6;
                RenkOrtalamaK(1, w0, w1, h0, h1, names, session);

                w0 = (int)birimW * S_1;
                w1 = (int)birimW * S_3;
                h0 = (int)birimH * S_11;
                h1 = (int)birimH * S_13;
                RenkOrtalamaK(2, w0, w1, h0, h1, names, session);


                w0 = (int)birimW * 5;
                w1 = (int)birimW * S_7;
                h0 = (int)birimH * S_11;
                h1 = (int)birimH * S_13;
                RenkOrtalamaK(3, w0, w1, h0, h1, names, session);



                w0 = (int)birimW * 9;
                w1 = (int)birimW * S_11;
                h0 = (int)birimH * S_11;
                h1 = (int)birimH * S_13;
                RenkOrtalamaK(4, w0, w1, h0, h1, names, session);



                w0 = (int)birimW * 5;
                w1 = (int)birimW * S_7;
                h0 = (int)birimH * S_18;
                h1 = (int)birimH * S_20;
                RenkOrtalamaK(5, w0, w1, h0, h1, names, session);

            }
        }
    }


    //public void RenkOrtalamaK(int sira, int w0, int w1, int h0, int h1, string names, string session)
    //{
    //    Color c;
    //    px = ((w1 - w0) * (h1 - h0));
    //    int R, G, B, A, K;
    //    int e = K = A = R = G = B = 0;
    //    for (int i = w0; i < w1; i++)
    //    {
    //        for (int j = h0; j < h1; j++)
    //        {
    //            c = rs.SourceImage.GetPixel(i, j);


    //            if (c.R == c.B || c.R == c.G || c.B == c.G)
    //                K++;

    //            int gray = (c.R + c.G + c.B) / 3;
    //            e += gray;
    //            R += c.R;
    //            G += c.G;
    //            B += c.B;
    //            A += c.A;


    //        }
    //    }
    //    string query = "insert into Rsm_Tbl_Eq(S,O,A,R,G,B,K,PX,NAME,SESSION) VALUES(" + sira.ToString() + ","
    //   + (e / px).ToString()
    //   + "," + (A / px).ToString()
    //   + "," + (R / px).ToString()
    //   + "," + (G / px).ToString()
    //   + "," + (B / px).ToString()
    //   + "," + K.ToString()
    //   + "," + Px.ToString()
    //   + ",'" + names + "','" + session + "')";
    int S_11 = 11;
    int S_13 = 12;
    int S_18 = 18;
    int S_20 = 19;
    int S_1 = 1;
    int S_3 = 2;
    int S_7 = 6;
    int S_6 = 5;


    //    PCL.MsSQL_DBOperations.ExecuteSQLStr(query, "rs");

    //}

    public void ResimBasla(FileUpload fu)
    {
        String names = "";
        if (fu.HasFile)
        {
            names = fu.FileName;
            //System.Drawing.Image orjinalFoto = null;
            //HttpPostedFile jpeg_image_upload = fu.PostedFile;
            //orjinalFoto = System.Drawing.Image.FromStream(jpeg_image_upload.InputStream);
            //rs.SourceImage = new Bitmap(orjinalFoto);
            //rs.Yedek = new Bitmap(orjinalFoto); ;
            rs.ResimOku(fu);
        }
        rs.Kucult(50);



        string session = DateTime.Now.Ticks.ToString().Substring(5);
        sourceWith = rs.SourceImage.Width;
        sourceHeight = rs.SourceImage.Height;

        decimal ortaNoktaW = Math.Round((decimal)(sourceWith / 2), 0);
        decimal ortaNoktaH = Math.Round((decimal)(sourceHeight / 2), 0);

        decimal birimH = 0;
        decimal birimW = 0;
        if (sourceWith > sourceHeight)
        {
            birimW = Math.Round((decimal)(sourceWith / 24), 0);
            birimH = Math.Round((decimal)(sourceHeight / 12), 0);
            int w0;
            int h0;
            int h1;
            int w1;
            w0 = (int)birimW * S_11;
            w1 = (int)birimW * S_13;
            h0 = (int)birimH * S_1;
            h1 = (int)birimH * S_3;
            RenkOrtalamaK(1, w0, w1, h0, h1, names, session);

            w0 = (int)birimW * 4;
            w1 = (int)birimW * S_6;
            h0 = (int)birimH * 5;
            h1 = (int)birimH * S_7;
            RenkOrtalamaK(2, w0, w1, h0, h1, names, session);

            w0 = (int)birimW * S_11;
            w1 = (int)birimW * S_13;
            h0 = (int)birimH * 5;
            h1 = (int)birimH * S_7;
            RenkOrtalamaK(3, w0, w1, h0, h1, names, session);

            w0 = (int)birimW * S_18;
            w1 = (int)birimW * S_20;
            h0 = (int)birimH * 5;
            h1 = (int)birimH * S_7;
            RenkOrtalamaK(4, w0, w1, h0, h1, names, session);

            w0 = (int)birimW * S_11;
            w1 = (int)birimW * S_13;
            h0 = (int)birimH * 9;
            h1 = (int)birimH * S_11;
            RenkOrtalamaK(5, w0, w1, h0, h1, names, session);

        }
        if (sourceWith < sourceHeight)
        {
            birimW = Math.Round((decimal)(sourceWith / 12), 0);
            birimH = Math.Round((decimal)(sourceHeight / 24), 0);
            int w0;
            int h0;
            int h1;
            int w1;
            w0 = (int)birimW * 5;
            w1 = (int)birimW * S_7;
            h0 = (int)birimH * 4;
            h1 = (int)birimH * S_6;
            RenkOrtalamaK(1, w0, w1, h0, h1, names, session);

            w0 = (int)birimW * S_1;
            w1 = (int)birimW * S_3;
            h0 = (int)birimH * S_11;
            h1 = (int)birimH * S_13;
            RenkOrtalamaK(2, w0, w1, h0, h1, names, session);


            w0 = (int)birimW * 5;
            w1 = (int)birimW * S_7;
            h0 = (int)birimH * S_11;
            h1 = (int)birimH * S_13;
            RenkOrtalamaK(3, w0, w1, h0, h1, names, session);



            w0 = (int)birimW * 9;
            w1 = (int)birimW * S_11;
            h0 = (int)birimH * S_11;
            h1 = (int)birimH * S_13;
            RenkOrtalamaK(4, w0, w1, h0, h1, names, session);



            w0 = (int)birimW * 5;
            w1 = (int)birimW * S_7;
            h0 = (int)birimH * S_18;
            h1 = (int)birimH * S_20;
            RenkOrtalamaK(5, w0, w1, h0, h1, names, session);

        }

    }
    public string ResimAra(FileUpload fu)
    {
        String names = "";
        if (fu.HasFile)
        {
            names = fu.FileName;
            //System.Drawing.Image orjinalFoto = null;
            //HttpPostedFile jpeg_image_upload = fu.PostedFile;
            //orjinalFoto = System.Drawing.Image.FromStream(jpeg_image_upload.InputStream);
            //rs.SourceImage = new Bitmap(orjinalFoto);
            //rs.Yedek = new Bitmap(orjinalFoto); ;
            rs.ResimOku(fu);
        }
        rs.Kucult(50);


        string session = DateTime.Now.Ticks.ToString().Substring(5);
        sourceWith = rs.SourceImage.Width;
        sourceHeight = rs.SourceImage.Height;

        decimal ortaNoktaW = Math.Round((decimal)(sourceWith / 2), 0);
        decimal ortaNoktaH = Math.Round((decimal)(sourceHeight / 2), 0);

        decimal birimH = 0;
        decimal birimW = 0;
        if (sourceWith > sourceHeight)
        {
            birimW = Math.Round((decimal)(sourceWith / 24), 0);
            birimH = Math.Round((decimal)(sourceHeight / 12), 0);
            int w0;
            int h0;
            int h1;
            int w1;
            w0 = (int)birimW * S_11;
            w1 = (int)birimW * S_13;
            h0 = (int)birimH * S_1;
            h1 = (int)birimH * S_3;
            RenkOrtalamaS(1, w0, w1, h0, h1, names, session);

            w0 = (int)birimW * 4;
            w1 = (int)birimW * S_6;
            h0 = (int)birimH * 5;
            h1 = (int)birimH * S_7;
            RenkOrtalamaS(2, w0, w1, h0, h1, names, session);

            w0 = (int)birimW * S_11;
            w1 = (int)birimW * S_13;
            h0 = (int)birimH * 5;
            h1 = (int)birimH * S_7;
            RenkOrtalamaS(3, w0, w1, h0, h1, names, session);

            w0 = (int)birimW * S_18;
            w1 = (int)birimW * S_20;
            h0 = (int)birimH * 5;
            h1 = (int)birimH * S_7;
            RenkOrtalamaS(4, w0, w1, h0, h1, names, session);

            w0 = (int)birimW * S_11;
            w1 = (int)birimW * S_13;
            h0 = (int)birimH * 9;
            h1 = (int)birimH * S_11;
            RenkOrtalamaS(5, w0, w1, h0, h1, names, session);

        }
        if (sourceWith < sourceHeight)
        {
            birimW = Math.Round((decimal)(sourceWith / 12), 0);
            birimH = Math.Round((decimal)(sourceHeight / 24), 0);
            int w0;
            int h0;
            int h1;
            int w1;
            w0 = (int)birimW * 5;
            w1 = (int)birimW * S_7;
            h0 = (int)birimH * 4;
            h1 = (int)birimH * S_6;
            RenkOrtalamaS(1, w0, w1, h0, h1, names, session);

            w0 = (int)birimW * S_1;
            w1 = (int)birimW * S_3;
            h0 = (int)birimH * S_11;
            h1 = (int)birimH * S_13;
            RenkOrtalamaS(2, w0, w1, h0, h1, names, session);


            w0 = (int)birimW * 5;
            w1 = (int)birimW * S_7;
            h0 = (int)birimH * S_11;
            h1 = (int)birimH * S_13;
            RenkOrtalamaS(3, w0, w1, h0, h1, names, session);



            w0 = (int)birimW * 9;
            w1 = (int)birimW * S_11;
            h0 = (int)birimH * S_11;
            h1 = (int)birimH * S_13;
            RenkOrtalamaS(4, w0, w1, h0, h1, names, session);



            w0 = (int)birimW * 5;
            w1 = (int)birimW * S_7;
            h0 = (int)birimH * S_18;
            h1 = (int)birimH * S_20;
            RenkOrtalamaS(5, w0, w1, h0, h1, names, session);

        }
        return session;
    }


    public void RenkOrtalamaK(int sira, int w0, int w1, int h0, int h1, string names, string session)
    {
        Color c;
        px = ((w1 - w0) * (h1 - h0));
        int R, G, B, A, K;
        int e = K = A = R = G = B = 0;
        for (int i = w0; i < w1; i++)
        {
            for (int j = h0; j < h1; j++)
            {
                c = rs.SourceImage.GetPixel(i, j);


                if (c.R == c.B || c.R == c.G || c.B == c.G)
                    K++;

                int gray = (c.R + c.G + c.B) / 3;
                e += gray;
                R += c.R;
                G += c.G;
                B += c.B;
                A += c.A;


            }
        }
        string query = "insert into Rsm_Tbl_Eq(S,O,A,R,G,B,K,PX,NAME,SESSION) VALUES(" + sira.ToString() + ","
       + (e / px).ToString()
       + "," + (A / px).ToString()
       + "," + (R / px).ToString()
       + "," + (G / px).ToString()
       + "," + (B / px).ToString()
       + "," + K.ToString()
       + "," + Px.ToString()
       + ",'" + names + "','" + session + "')";

        PCL.MsSQL_DBOperations.ExecuteSQLStr(query, "rs");
        /*oR = R / px;
        oG = G / px;
        oB = B / px;
        oA = A / px;
        oK = K;
        return e / px;
*/
    }


    public void RenkOrtalamaS(int sira, int w0, int w1, int h0, int h1, string names, string session)
    {
        Color c;
        px = ((w1 - w0) * (h1 - h0));
        int R, G, B, A, K;
        int e = K = A = R = G = B = 0;
        for (int i = w0; i < w1; i++)
        {
            for (int j = h0; j < h1; j++)
            {
                c = rs.SourceImage.GetPixel(i, j);


                if (c.R == c.B || c.R == c.G || c.B == c.G)
                    K++;

                int gray = (c.R + c.G + c.B) / 3;
                e += gray;
                R += c.R;
                G += c.G;
                B += c.B;
                A += c.A;


            }
        }
        string query = "insert into Rsm_Src_Eq(S,O,A,R,G,B,K,PX,NAME,SESSION) VALUES(" + sira.ToString() + ","
       + (e / px).ToString()
       + "," + (A / px).ToString()
       + "," + (R / px).ToString()
       + "," + (G / px).ToString()
       + "," + (B / px).ToString()
       + "," + K.ToString()
       + "," + Px.ToString()
       + ",'" + names + "','" + session + "')";

        PCL.MsSQL_DBOperations.ExecuteSQLStr(query, "rs");
        /*oR = R / px;
        oG = G / px;
        oB = B / px;
        oA = A / px;
        oK = K;
        return e / px;
*/
    }








    public int RenkOrtalama(int w0, int w1, int h0, int h1)
    {
        Color c;
        px = ((w1 - w0) * (h1 - h0));
        int R, G, B, A, K;
        int e = K = A = R = G = B = 0;
        for (int i = w0; i < w1; i++)
        {
            for (int j = h0; j < h1; j++)
            {
                c = rs.SourceImage.GetPixel(i, j);


                if (c.R == c.B || c.R == c.G || c.B == c.G)
                    K++;

                int gray = (c.R + c.G + c.B) / 3;
                e += gray;
                R += c.R;
                G += c.G;
                B += c.B;
                A += c.A;


            }
        }
        oR = R / px;
        oG = G / px;
        oB = B / px;
        oA = A / px;
        oK = K;
        return e / px;
    }














}
