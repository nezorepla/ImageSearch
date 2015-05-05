using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;
using System.IO;

public partial class _Default : System.Web.UI.Page
{
    SearchEngine se = new SearchEngine();
    Resim.ResimIslem rs = new Resim.ResimIslem();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            sirala();
        //Response.Redirect("Default3.aspx");

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string hash = DateTime.Now.Ticks.ToString().Substring(5);
        //    
        //yukle(FileUpload fu, int Ksize, string imagename)


        //se.ResimBasla(FileUpload1);
        string session = se.ResimAra(FileUpload1);

        string sira = " ";
        string query = " exec Rsm_Sp_EqRslt '" + session + "'";
        DataTable dt = new DataTable();

        dt = PCL.MsSQL_DBOperations.GetData(query, "rs");

        foreach (DataRow dr in dt.Rows)
        {
            sira += "<img width=\"20%\" height=\"20%\"  src=\"images/eq_list/" + dr["NAME"].ToString() + "\"><br>";

        }
        lblOrt.Text = sira;

         //  se.ResimOkuYol(path);



        /*   lblOrt.Text = se.RenkOrtalama(0, se.SourceWith, 0, se.SourceHeight).ToString() + "<br>R:" + se.OR + "<br>G:" + se.OG
               + "<br>B:" + se.OB
               + "<br>A:" + se.OA
               + "<br>px:" + se.Px
               + "<br>K:" + se.OK;
           */

        //rs.ResimOku(FileUpload1);
        //rs.Kucult(500);
        //rs.fEqRenk();


        //string yol = "img/" + DateTime.Now.Ticks.ToString().Substring(5) + "/";
        //string yol2 = "images/" + yol;
        //string path = "~/images/" + yol;

        //bool folderExists = Directory.Exists(Server.MapPath(path));
        //if (!folderExists)
        //    Directory.CreateDirectory(Server.MapPath(path));


        //rs.ResimKaydet(yol + "/fEqRenk");


    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string[] array1 = Directory.GetFiles(@"C:\Users\Alper\Desktop\Resimler", "*.jpg");
        string names, sonuc;


        string path;
        //lblOrt.Text = se.RenkOrtalama(0, se.SourceWith, 0, se.SourceHeight).ToString() + "<br>R:" + se.OR + "<br>G:" + se.OG
        //    + "<br>B:" + se.OB
        //    + "<br>A:" + se.OA
        //    + "<br>px:" + se.Px
        //    + "<br>K:" + se.OK;

        string query;
        sonuc = "ok";
        DataTable dt = new DataTable();
        foreach (string YOL in array1)
        {
            names = Path.GetFileName(YOL);

            dt = PCL.MsSQL_DBOperations.GetData("select count(DISTINCT NAME) from   Rsm_Tbl_Eq where NAME='" + names + "'", "rs");

            if (dt.Rows[0][0].ToString() == "0")
            {


                se.ResimOkuYol(YOL, names);
                
                System.Threading.Thread.Sleep(1000);
                //ResimOkuYol içinde kayıt yapılıyor
                //try
                //{
                //    query = "insert into Rsm_Tbl_Eq(S,O,A,R,G,B,K,PX,NAME) VALUES(1,"
                //        + se.RenkOrtalama(0, se.SourceWith, 0, se.SourceHeight).ToString()
                //        + "," + se.OA.ToString()
                //        + "," + se.OR.ToString()
                //        + "," + se.OG.ToString()
                //        + "," + se.OB.ToString()
                //        + "," + se.OK.ToString()
                //        + "," + se.Px.ToString()
                //        + ",'" + names + "')";

                //    PCL.MsSQL_DBOperations.ExecuteSQLStr(query, "rs");
                
                //}
                //catch (Exception t)
                //{
                //    sonuc = "error " + t.ToString();
                //}
            }
        }
        lblOrt.Text = sonuc;
    }
    public void sirala()
    {
        string sira = " ";
        string query = " exec Rsm_Sp_Eq";
        DataTable dt = new DataTable();
        dt = PCL.MsSQL_DBOperations.GetData(query, "rs");

        foreach (DataRow dr in dt.Rows)
        {
            sira += "<img width=\"20%\" height=\"20%\"  src=\"images/eq_list/" + dr["NAME"].ToString() + "\"><br>";

        }
        lblOrt.Text = sira;

    }
}
