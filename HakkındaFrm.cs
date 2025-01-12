using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;

namespace AracKiralama
{
    public partial class HakkındaFrm : XtraForm
    {
        private string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";

        public HakkındaFrm()
        {
            InitializeComponent();
        }

        private void HakkındaFrm_Load(object sender, EventArgs e)
        {
            // SQL sorgusu
            string query = "SELECT [SirketAdi],[SirketSahibi], [SirketAdresi], [TelNo], [Mail], [VergiNumarası], [KurulusYili], [FaaliyetAlani] FROM [AracKiralama].[dbo].[SirketBilgi]";

            // Veritabanı bağlantısını aç
            using (SqlConnection connection = new SqlConnection(baglantiCumlesi))
            {
                try
                {
                    // Bağlantıyı aç
                    connection.Open();

                    // Komut oluştur
                    SqlCommand command = new SqlCommand(query, connection);

                    // Veriyi al
                    SqlDataReader reader = command.ExecuteReader();

                    // Veriyi okuma
                    if (reader.Read())
                    {
                        // TextBox'lara veri atama
                        txtSirketAdi.Text = reader["SirketAdi"].ToString();
                        txtSirketSahibi.Text = reader["SirketSahibi"].ToString();
                        txtSirketAdresi.Text = reader["SirketAdresi"].ToString();
                        txtSirketTelNo.Text = reader["TelNo"].ToString();
                        txtSirketMail.Text = reader["Mail"].ToString();
                        txtVergiNumarası.Text = reader["VergiNumarası"].ToString();
                        txtSirketKurulusTarih.Text = reader["KurulusYili"].ToString();
                        txtFaaliyetAlani.Text = reader["FaaliyetAlani"].ToString();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }

            this.ActiveControl = pictureBox3;
        }

       

       

        
        private void btnGeri_Click(object sender, EventArgs e)
        {

            Frm_ilkgiris ilkgrs = new Frm_ilkgiris();
            ilkgrs.Show();
            this.Hide();
        }
    }
}