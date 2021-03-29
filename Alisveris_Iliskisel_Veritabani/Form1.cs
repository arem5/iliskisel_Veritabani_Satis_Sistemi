using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
/*  AŞAĞIDAKİ ŞEKİLDE SQL SERVER DA BİR PROCEDURE OLUŞTURULMUŞTUR VE BU ÇALIŞMADA BUNU KULLANACAĞIZ.
 *  DATAGRİDVİEW DA DATASOURCE EKLENEREK SEÇİLEN VERİ TABANINDA YALNIZ PROCEDUR EKLENİR.
 * 
 * create proc proje_Alisveris 
as
select u.UrunAdi, m.AdSoyad as [Müsteri Adı Soyadı], p.PersonelAd, Fiyat
from TBL_Haraketler as h inner join TBL_Urunler as u 
on h.Urun =u.UrunID
inner join TBL_Musteriler as m on h.Musteri=m.MusteriID
inner join TBL_Personel as p on h.Personel = p.PersonelId
*/
namespace Alisveris_Iliskisel_Veritabani
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'db_AlisverisDataSet.proje_Alisveris' table. You can move, or remove it, as needed.
            this.proje_AlisverisTableAdapter.Fill(this.db_AlisverisDataSet.proje_Alisveris);
            
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * from TBL_Urunler", baglanti);
            da.Fill(dt);
            cmb_urun.DisplayMember = "UrunAdi";
            cmb_urun.ValueMember = "UrunID";
            cmb_urun.DataSource = dt;

            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter("select * from TBL_Personel", baglanti);
            da2.Fill(dt2);
            cmb_Personel.DisplayMember = "PersonelAd";
            cmb_Personel.ValueMember = "PersonelID";
            cmb_Personel.DataSource = dt2;

            DataTable dt3 = new DataTable();
            SqlDataAdapter da3 = new SqlDataAdapter("select * from [TBL_Musteriler]", baglanti);
            da3.Fill(dt3);
            cmb_Musteri.DisplayMember = "AdSoyad";
            cmb_Musteri.ValueMember = "MusteriID";
            cmb_Musteri.DataSource = dt3;

        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Db_Alisveris;Integrated Security=True");


        private void btn_Ekle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand cmnd = new SqlCommand("insert into TBL_Haraketler ([Urun] ,[Musteri] ,[Personel],[Fiyat]) " +
                "values (@P1,@P2,@P3,@P4)", baglanti);
            cmnd.Parameters.AddWithValue("@P1", cmb_urun.SelectedValue);
            cmnd.Parameters.AddWithValue("@P2", cmb_Musteri.SelectedValue);
            cmnd.Parameters.AddWithValue("@P3", cmb_Personel.SelectedValue);
            cmnd.Parameters.AddWithValue("@P4", txt_Fiyat.Text);

            cmnd.ExecuteNonQuery();
            baglanti.Close();



            SqlDataAdapter da = new SqlDataAdapter("Execute proje_Alisveris", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;  //Procedur çalıştırarak her eklemde getirirz.
            MessageBox.Show("Alışveriş başarılı bir şekilde eklendi.", "Bilgi", MessageBoxButtons.OK);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand cmnd1 = new SqlCommand("insert into TBL_Musteriler ([AdSoyad]) values (@P1)", baglanti);
            cmnd1.Parameters.AddWithValue("@P1", txt_Musteri.Text);
            cmnd1.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Müsteri başarılı bir şekilde eklendi.","Bilgi",MessageBoxButtons.OK);
            DataTable dt3 = new DataTable();
            SqlDataAdapter da3 = new SqlDataAdapter("select * from [TBL_Musteriler]", baglanti);
            da3.Fill(dt3);
            cmb_Musteri.DisplayMember = "AdSoyad";
            cmb_Musteri.ValueMember = "MusteriID";
            cmb_Musteri.DataSource = dt3;
        }
    }
}
