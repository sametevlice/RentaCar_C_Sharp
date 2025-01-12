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

namespace AracKiralama
{
    public partial class YapilanYorumlar : Form
    {
        public YapilanYorumlar()
        {
            InitializeComponent();
        }
        private string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
               
                txtAd.Text = row.Cells["Ad"].Value.ToString();
                txtSoyad.Text = row.Cells["Soyad"].Value.ToString();
                txtMail.Text = row.Cells["Mail"].Value.ToString();
                txtPuan.Text = row.Cells["Derecelendirme"].Value.ToString();
                txtYorum.Text = row.Cells["Yorum"].Value.ToString();
            }
        }

        private void YapilanYorumlar_Load(object sender, EventArgs e)
        {
            YorumlariGetir();

            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            try
            {
                baglanti.Open();

                // Toplam araç sayısı
                SqlCommand toplamYorumSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Yorumlar", baglanti);
                int toplamYorumSayisi = Convert.ToInt32(toplamYorumSayisiCmd.ExecuteScalar() ?? 0);
                txtYorumSayisi.Text = toplamYorumSayisi.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri çekilirken bir hata oluştu: " + ex.Message);
            }

            txtYorum.ReadOnly = true;
            txtYorumSayisi.ReadOnly = true;
            txtAd.ReadOnly = true;
            txtSoyad.ReadOnly = true;
            txtMail.ReadOnly = true;
            txtPuan.ReadOnly = true;
        }

        private void YorumlariGetir()
        {
            using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
            {
                try
                {
                    baglanti.Open();
                    string sorgu = "SELECT Ad, Soyad, Mail, Derecelendirme, Yorum FROM Yorumlar";
                    SqlDataAdapter da = new SqlDataAdapter(sorgu, baglanti);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Veritabanı hatası: " + ex.Message);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
