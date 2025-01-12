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
    public partial class RandevuListesi : Form
    {
        public RandevuListesi()
        {
            InitializeComponent();
        }

        private void RandevuListesi_Load(object sender, EventArgs e)
        {
            RandevulariGetir(); // Form yüklendiğinde randevuları getir
        }

        private void RandevulariGetir()
        {
            using (SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-9FINMFA\\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True"))
            {
                try
                {
                    baglanti.Open();
                    string sorgu = "SELECT * FROM Randevular";
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

           
        

        private void Temizle()
        {
            txtTcNo.Text= string.Empty;
            txtAd.Text = string.Empty;
            txtSoyad.Text = string.Empty;
            txtTelNo.Text = string.Empty;
            dtpTarih.Value = DateTime.Now;
            txtSaat.Text = string.Empty;
        }

       

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTcNo.Text))
            {
                MessageBox.Show("Henüz seçim yapmadınız. Lütfen bir müşteri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Fonksiyonu sonlandır
            }
            using (SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-9FINMFA\\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True"))
            {
                try
                {
                    baglanti.Open();
                    string sorgu = "UPDATE Randevular SET Ad=@Ad, Soyad=@Soyad, TelefonNo=@TelefonNo, Tarih=@Tarih, Saat=@Saat WHERE TcNo=@TcNo";
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);
                    komut.Parameters.AddWithValue("@TcNo", txtTcNo.Text);
                    komut.Parameters.AddWithValue("@Ad", txtAd.Text);
                    komut.Parameters.AddWithValue("@Soyad", txtSoyad.Text);
                    komut.Parameters.AddWithValue("@TelefonNo", txtTelNo.Text);
                    komut.Parameters.AddWithValue("@Tarih", dtpTarih.Value);
                    komut.Parameters.AddWithValue("@Saat", txtSaat.Text);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Randevu güncellendi.");
                    RandevulariGetir(); // Güncellemeden sonra listeyi yenile
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Veritabanı hatası: " + ex.Message);
                }
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) // Satır seçili değilse
            {
                MessageBox.Show("Lütfen silmek için bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Fonksiyonu sonlandır
            }
            if (MessageBox.Show("Bu randevuyu silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-9FINMFA\\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True"))
                {
                    try
                    {
                        baglanti.Open();
                        string sorgu = "DELETE FROM Randevular WHERE TcNo=@TcNo";
                        SqlCommand komut = new SqlCommand(sorgu, baglanti);
                        komut.Parameters.AddWithValue("@TcNo", txtTcNo.Text);
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Randevu silindi.");
                        RandevulariGetir(); // Silme işleminden sonra listeyi yenile
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Veritabanı hatası: " + ex.Message);
                    }
                }
            }
        }

        private void txtTcAra_EditValueChanged(object sender, EventArgs e)
        {
            using (SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-9FINMFA\\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True"))
            {
                try
                {
                    baglanti.Open();
                    string sorgu = "SELECT * FROM Randevular WHERE TcNo LIKE @TcNo";
                    SqlDataAdapter da = new SqlDataAdapter(sorgu, baglanti);
                    da.SelectCommand.Parameters.AddWithValue("@TcNo", txtTcAra.Text + "%");
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                    // Eğer tek bir sonuç varsa TextBox'lara doldur
                    if (dt.Rows.Count == 1)
                    {
                        DataRow row = dt.Rows[0];
                        txtTcNo.Text = row["TcNo"].ToString();
                        txtAd.Text = row["Ad"].ToString();
                        txtSoyad.Text = row["Soyad"].ToString();
                        txtTelNo.Text = row["TelefonNo"].ToString();
                        dtpTarih.Value = Convert.ToDateTime(row["Tarih"]);
                        txtSaat.Text = row["Saat"].ToString();
                    }
                    else
                    {
                        // Çoklu sonuç veya hiç sonuç yoksa alanları temizle
                        Temizle();
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Veritabanı hatası: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtTcNo.Text = row.Cells["TcNo"].Value.ToString();
                txtAd.Text = row.Cells["Ad"].Value.ToString();
                txtSoyad.Text = row.Cells["Soyad"].Value.ToString();
                txtTelNo.Text = row.Cells["TelefonNo"].Value.ToString();
                dtpTarih.Value = Convert.ToDateTime(row.Cells["Tarih"].Value);
                txtSaat.Text = row.Cells["Saat"].Value.ToString();
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