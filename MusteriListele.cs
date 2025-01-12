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
using System.IO;

namespace AracKiralama
{
    public partial class MusteriListele : DevExpress.XtraEditors.XtraForm
    {
        public MusteriListele()
        {
            InitializeComponent();
        }

        

        private string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";
        private void MusteriListele_Load(object sender, EventArgs e)
        {
            Musteri_Listele();
        }
        public void Musteri_Listele()
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();

            
            String komutCumlesi = "Select Tc_No,Ad_Soyad,Telefon_Numarasi,Mail,Ehliyet_No,Ehliyet_Tarihi,Adres,resim,Dogum_Tarihi,Meslek,Ehliyet_Türü from Musteriler";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTcNo.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtAdSoyad.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtTelNo.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtMail.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            txtEhliyetNo.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            txtEhliyetTarihi.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            txtAdres.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            pictureBox3.ImageLocation = dataGridView1.CurrentRow.Cells["resim"].Value.ToString();
            txtDogumTarihi.Text = dataGridView1.CurrentRow.Cells["Dogum_Tarihi"].Value.ToString();
            txtMeslek.Text = dataGridView1.CurrentRow.Cells["Meslek"].Value.ToString();
            txtEhliyetTürü.Text = dataGridView1.CurrentRow.Cells["Ehliyet_Türü"].Value.ToString();

        }


        public void Musteri_Guncelle()
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();

            string komutCumlesi = "Update Musteriler set Ad_Soyad=@Ad_Soyad,Telefon_Numarasi=@Telefon_Numarasi,Mail=@Mail,Ehliyet_No=@Ehliyet_No,Ehliyet_Tarihi=@Ehliyet_Tarihi,Adres=@Adres," +
                "resim=@resim,Dogum_Tarihi=@Dogum_Tarihi,Meslek=@Meslek,Ehliyet_Türü=@Ehliyet_Türü where Tc_No=@Tc_No";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            komut.Parameters.AddWithValue("@Tc_No", txtTcNo.Text);
            komut.Parameters.AddWithValue("@Ad_Soyad", txtAdSoyad.Text);
            komut.Parameters.AddWithValue("@Telefon_Numarasi", txtTelNo.Text);
            komut.Parameters.AddWithValue("@Mail", txtMail.Text);
            komut.Parameters.AddWithValue("@Ehliyet_No", txtEhliyetNo.Text);
            komut.Parameters.AddWithValue("@Ehliyet_Tarihi", txtEhliyetTarihi.Text);
            komut.Parameters.AddWithValue("@Adres", txtAdres.Text);
            komut.Parameters.AddWithValue("@resim", pictureBox3.ImageLocation);
            komut.Parameters.AddWithValue("@Dogum_Tarihi", txtDogumTarihi.Text);
            komut.Parameters.AddWithValue("@Meslek", txtMeslek.Text);
            komut.Parameters.AddWithValue("@Ehliyet_Türü", txtEhliyetTürü.Text);

            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Müşteri Bilgileri Güncellendi!");
            Musteri_Listele();
        }

        public void Musteri_Sil()
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();

            string komutCumlesi = "Delete from Musteriler where Tc_No='" + dataGridView1.CurrentRow.Cells["Tc_No"].Value.ToString() + "'";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);

            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Müşteri Bilgileri Silindi!");
            txtTcAra.Text = string.Empty;
            txtTcNo.Text = string.Empty;
            txtAdSoyad.Text = string.Empty;
            txtTelNo.Text = string.Empty;
            txtMail.Text = string.Empty;
            txtEhliyetNo.Text = string.Empty;
            txtEhliyetTarihi.Text = string.Empty;
            txtAdres.Text = string.Empty;
            pictureBox3.Image = null;
            txtDogumTarihi.Text = string.Empty;
            txtMeslek.Text = string.Empty;
            txtEhliyetTürü.Text = string.Empty;


            Musteri_Listele();

        }


        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTcNo.Text))
            {
                MessageBox.Show("Henüz seçim yapmadınız. Lütfen bir müşteri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Fonksiyonu sonlandır
            }
            Musteri_Guncelle();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTcNo.Text))
            {
                MessageBox.Show("Henüz seçim yapmadınız. Lütfen bir müşteri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Fonksiyonu sonlandır
            }
            Musteri_Sil();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtEhliyetNo_EditValueChanged(object sender, EventArgs e)
        {

        }
        public string GonderenForm { get; set; }
        private void btnback_Click(object sender, EventArgs e)
        {
            if (GonderenForm == "Anasayfa")
            {
                Anasayfa anasayfa = new Anasayfa();
                anasayfa.Show();
                this.Hide();
            }
            
            else if (GonderenForm == "PersonelAnasayfa")
            {
                PersonelAnasayfa personelAnasayfa = new PersonelAnasayfa();
                personelAnasayfa.Show();
                this.Hide();
            }
        }

        private void txtTcAra_EditValueChanged(object sender, EventArgs e)
        {

            using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
            {
                try
                {
                    baglanti.Open();

                    // DataGridView'i güncelle
                    string komutCumlesi = "SELECT * FROM Musteriler WHERE Tc_No LIKE @TcNo";
                    SqlDataAdapter da = new SqlDataAdapter(komutCumlesi, baglanti);
                    da.SelectCommand.Parameters.AddWithValue("@TcNo", txtTcAra.Text + "%");
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                    // TextBox ve diğer kontrolleri doldur
                    SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
                    komut.Parameters.AddWithValue("@TcNo", txtTcAra.Text + "%");
                    SqlDataReader read = komut.ExecuteReader();

                    if (read.Read())
                    {
                        txtTcNo.Text = read["Tc_No"].ToString();
                        txtAdSoyad.Text = read["Ad_Soyad"].ToString();
                        txtTelNo.Text = read["Telefon_Numarasi"].ToString();
                        txtMail.Text = read["Mail"].ToString();
                        txtEhliyetNo.Text = read["Ehliyet_No"].ToString();
                        txtEhliyetTarihi.Text = read["Ehliyet_Tarihi"].ToString();
                        txtAdres.Text = read["Adres"].ToString();
                        txtDogumTarihi.Text = read["Dogum_Tarihi"].ToString();
                        txtMeslek.Text = read["Meslek"].ToString();
                        txtEhliyetTürü.Text = read["Ehliyet_Türü"].ToString();

                        // Resim doldurma
                        try
                        {
                            if (read["resim"] != DBNull.Value && !string.IsNullOrEmpty(read["resim"].ToString()))
                            {
                                pictureBox3.Image = Image.FromFile(read["resim"].ToString());
                            }
                            else
                            {
                                pictureBox3.Image = null; // Eğer resim yoksa PictureBox'ı temizle
                            }
                        }
                        catch (Exception ex)
                        {
                            pictureBox3.Image = null; // Resim yüklenemezse sadece resim kısmını temizle
                            MessageBox.Show("Müşterinin Resmi Sistemde yok.: " + ex.Message, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                                    }
                catch (SqlException ex)
                {
                    MessageBox.Show("Veritabanı hatası: " + ex.Message);
                }
            }



        }

        private void btnResim_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            pictureBox3.ImageLocation = openFileDialog1.FileName;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

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