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
    public partial class PersonelListele : DevExpress.XtraEditors.XtraForm
    {
        public PersonelListele()
        {
            InitializeComponent();
        }

        
        private string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";

        public string GonderenForm { get; internal set; }

        private void PersonelListele_Load(object sender, EventArgs e)
        {
            Personel_Listele();
        }

        // Personel Listeleme
        public void Personel_Listele()
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();

            // Veritabanından personel bilgilerini çekme
            string komutCumlesi = "Select TcKimlikNo, Ad, Soyad, Telefon, Email, Departman, Pozisyon, Adres,resim,Maas,Baslama_Tarihi,Calisma_Durumu,Dogum_Tarihi,Cinsiyet from Personeller";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }

        // DataGridView üzerinden seçilen veriyi form alanlarına aktar
       
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Alanları doldurun
                txtTc.Text = row.Cells[0]?.Value?.ToString() ?? string.Empty;
                txtAd.Text = row.Cells[1]?.Value?.ToString() ?? string.Empty;
                txtSoyad.Text = row.Cells[2]?.Value?.ToString() ?? string.Empty;
                txtTelNo.Text = row.Cells[3]?.Value?.ToString() ?? string.Empty;
                txtMail.Text = row.Cells[4]?.Value?.ToString() ?? string.Empty;
                cbxDepartman.Text = row.Cells[5]?.Value?.ToString() ?? string.Empty;
                txtPozisyon.Text = row.Cells[6]?.Value?.ToString() ?? string.Empty;
                txtAdres.Text = row.Cells[7]?.Value?.ToString() ?? string.Empty;
                pictureBox3.ImageLocation = row.Cells["resim"]?.Value?.ToString();
                txtDogumTarihi.Text = row.Cells["Dogum_Tarihi"]?.Value?.ToString();
                txtBaslamaTarihi.Text = row.Cells["Baslama_Tarihi"]?.Value?.ToString();
                cbxCalismaDurumu.Text = row.Cells["Calisma_Durumu"]?.Value?.ToString();
                cbxCinsiyet.Text = row.Cells["Cinsiyet"]?.Value?.ToString();
                txtMaas.Text = row.Cells["Maas"]?.Value?.ToString();
            }
            else
            {
                MessageBox.Show("Geçersiz satır seçimi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        // Personel Güncelleme
        public void Personel_Guncelle()
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();

            string komutCumlesi = "Update Personeller set Ad=@Ad, Soyad=@Soyad, Telefon=@Telefon, Adres=@Adres, Departman=@Departman, Pozisyon=@Pozisyon, Email=@Email," +
                "resim=@resim,Maas=@Maas,Dogum_Tarihi=@Dogum_Tarihi,Baslama_Tarihi=@Baslama_Tarihi,Calisma_Durumu=@Calisma_Durumu,Cinsiyet=@Cinsiyet where TcKimlikNo=@TcKimlikNo";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            komut.Parameters.AddWithValue("@TcKimlikNo", txtTc.Text);
            komut.Parameters.AddWithValue("@Ad", txtAd.Text);
            komut.Parameters.AddWithValue("@Soyad", txtSoyad.Text);
            komut.Parameters.AddWithValue("@Telefon", txtTelNo.Text);
            komut.Parameters.AddWithValue("@Email", txtMail.Text);
            komut.Parameters.AddWithValue("@Departman", cbxDepartman.Text);
            komut.Parameters.AddWithValue("@Pozisyon", txtPozisyon.Text);
            komut.Parameters.AddWithValue("@Maas", txtMaas.Text);
            komut.Parameters.AddWithValue("@Adres", txtAdres.Text);
            komut.Parameters.AddWithValue("@resim", pictureBox3.ImageLocation);
            komut.Parameters.AddWithValue("@Dogum_Tarihi", txtDogumTarihi.Text);
            komut.Parameters.AddWithValue("@Baslama_Tarihi", txtBaslamaTarihi.Text);
            komut.Parameters.AddWithValue("@Calisma_Durumu", cbxCalismaDurumu.Text);
            komut.Parameters.AddWithValue("@Cinsiyet", cbxCinsiyet.Text);

            komut.ExecuteNonQuery();
            baglanti.Close();

            MessageBox.Show("Personel bilgileri güncellendi.");
            Personel_Listele();
        }

        // Personel Silme
        public void Personel_Sil()
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();

            string komutCumlesi = "Delete from Personeller where TcKimlikNo='" + dataGridView1.CurrentRow.Cells["TcKimlikNo"].Value.ToString() + "'";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();

            MessageBox.Show("Personel silindi.");
            // Alanları temizle
            txtTc.Text = string.Empty;
            txtAd.Text = string.Empty;
            txtSoyad.Text = string.Empty;
            txtTelNo.Text = string.Empty;
            txtMail.Text = string.Empty;
            txtAdres.Text = string.Empty;
            cbxDepartman.Text = string.Empty;
            txtPozisyon.Text = string.Empty;
            pictureBox3.Image = null;
            txtMaas.Text= string.Empty;
            cbxCalismaDurumu.Text = string.Empty;
            cbxCinsiyet.Text = string.Empty;
            txtBaslamaTarihi.Text = string.Empty;
            txtDogumTarihi.Text = string.Empty;
            Personel_Listele();
        }

        
       
       

        private void btnPersonelEkleCikis_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btnGuncelle_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTc.Text))
            {
                MessageBox.Show("Henüz seçim yapmadınız. Lütfen bir personel seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Fonksiyonu sonlandır
            }
            Personel_Guncelle();
        }

        private void btnSil_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTc.Text))
            {
                MessageBox.Show("Henüz seçim yapmadınız. Lütfen bir personel seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Fonksiyonu sonlandır
            }
            Personel_Sil();
        }

        private void btnback_Click(object sender, EventArgs e)
        {
            if (GonderenForm == "Anasayfa")
            {
                Anasayfa anasayfa = new Anasayfa();
                anasayfa.Show();
                this.Hide();
            }
            // Eğer form Personel Anasayfa'dan açıldıysa, Personel Anasayfa'ya yönlendir
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

                    // DataGridView'i doldur
                    string komutCumlesi = "SELECT * FROM Personeller WHERE TCKimlikNo LIKE @TCKimlikNo";
                    SqlDataAdapter da = new SqlDataAdapter(komutCumlesi, baglanti);
                    da.SelectCommand.Parameters.AddWithValue("@TCKimlikNo", txtTcAra.Text + "%");
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                    // TextBox ve diğer kontrolleri doldur
                    SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
                    komut.Parameters.AddWithValue("@TCKimlikNo", txtTcAra.Text + "%");
                    SqlDataReader read = komut.ExecuteReader();

                    if (read.Read())
                    {
                        txtTc.Text = read["TCKimlikNo"].ToString();
                        txtAd.Text = read["Ad"].ToString();
                        txtSoyad.Text = read["Soyad"].ToString();
                        txtTelNo.Text = read["Telefon"].ToString();
                        txtMail.Text = read["Email"].ToString();
                        cbxDepartman.Text = read["Departman"].ToString();
                        txtPozisyon.Text = read["Pozisyon"].ToString();
                        txtMaas.Text = read["Maas"].ToString();
                        txtAdres.Text = read["Adres"].ToString();
                        cbxCalismaDurumu.Text = read["Calisma_Durumu"].ToString();
                        cbxCinsiyet.Text = read["Cinsiyet"].ToString();
                        txtDogumTarihi.Text = read["Dogum_Tarihi"].ToString();
                        txtBaslamaTarihi.Text = read["Baslama_Tarihi"].ToString();

                        // Resim ekleme
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
                    MessageBox.Show("Personelin Resmi Sistemde yok.: " + ex.Message, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }

        private void btnResim_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            pictureBox3.ImageLocation = openFileDialog1.FileName;
        }

        private void labelControl2_Click(object sender, EventArgs e)
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