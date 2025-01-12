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
    public partial class AracEkle : DevExpress.XtraEditors.XtraForm
    {
        private string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";

        public string GonderenForm { get; internal set; }

        public AracEkle()
        {
            InitializeComponent();

            // Sadece sayı girişini kontrol eden event eklemeleri
            txtModel.KeyPress += SadeceSayiGirisKontrol;
            txtKm.KeyPress += SadeceSayiGirisKontrol;
            txtUcret.KeyPress += SadeceSayiGirisKontrol;
        }

        // Çıkış butonu işlemi
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
             if (string.IsNullOrWhiteSpace(txtPlaka.Text) ||
             cbxMarka.SelectedItem == null ||
             cbxSeri.SelectedItem == null ||
             string.IsNullOrWhiteSpace(txtModel.Text) ||
             string.IsNullOrWhiteSpace(txtRenk.Text) ||
             string.IsNullOrWhiteSpace(txtKm.Text) ||
             cbxYakit.SelectedItem == null ||
             string.IsNullOrWhiteSpace(txtUcret.Text) ||
             cbxDurum.SelectedItem == null ||
             cbxVites.SelectedItem == null ||
             cbxMotorGucu.SelectedItem == null ||
             cbxCekis.SelectedItem == null ||
             cbxKapi.SelectedItem == null ||
             cbxKasaTipi.SelectedItem == null ||
             pictureBox2.Image == null ||
             string.IsNullOrEmpty(pictureBox2.ImageLocation))
            {
                MessageBox.Show(
                    "Lütfen tüm bilgileri doldurunuz.",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // Tüm kontroller geçerse veritabanına kayıt işlemi
            try
            {
                SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
                baglanti.Open();

                string komutCumlesi = "Insert Into Araclar Values (@plaka,@Marka,@Seri,@Model,@Renk,@Km,@Yakit,@Ücret,@Durumu,@resim," +
                    "@Motor_Gucu,@Vites,@Cekis,@Kapi,@Kasa_Tipi)";
                SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
                komut.Parameters.AddWithValue("@plaka", txtPlaka.Text);
                komut.Parameters.AddWithValue("@Marka", cbxMarka.SelectedItem.ToString());
                komut.Parameters.AddWithValue("@Seri", cbxSeri.SelectedItem.ToString());
                komut.Parameters.AddWithValue("@Model", txtModel.Text);
                komut.Parameters.AddWithValue("@Renk", txtRenk.Text);
                komut.Parameters.AddWithValue("@Km", txtKm.Text);
                komut.Parameters.AddWithValue("@Yakit", cbxYakit.SelectedItem.ToString());
                komut.Parameters.AddWithValue("@Ücret", txtUcret.Text);
                komut.Parameters.AddWithValue("@Durumu", cbxDurum.SelectedItem.ToString());
                komut.Parameters.AddWithValue("@resim", pictureBox2.ImageLocation);
                komut.Parameters.AddWithValue("@Motor_Gucu", cbxMotorGucu.SelectedItem.ToString());
                komut.Parameters.AddWithValue("@Vites", cbxVites.SelectedItem.ToString());
                komut.Parameters.AddWithValue("@Cekis", cbxCekis.SelectedItem.ToString());
                komut.Parameters.AddWithValue("@Kapi", cbxKapi.SelectedItem.ToString());
                komut.Parameters.AddWithValue("@Kasa_Tipi", cbxKasaTipi.SelectedItem.ToString());




                komut.ExecuteNonQuery();
                baglanti.Close();

                MessageBox.Show("Kayıt Başarılı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Alanları temizle
                txtPlaka.Text = string.Empty;
                cbxMarka.SelectedIndex = -1;
                cbxSeri.SelectedIndex = -1;
                txtModel.Text = string.Empty;
                txtRenk.Text = string.Empty;
                txtKm.Text = string.Empty;
                cbxYakit.SelectedIndex = -1;
                txtUcret.Text = string.Empty;
                cbxDurum.SelectedIndex = -1;
                pictureBox2.Image = null;
                cbxKasaTipi.SelectedIndex = -1;
                cbxMotorGucu.SelectedIndex = -1;
                cbxKapi.SelectedIndex = -1;
                cbxVites.SelectedIndex = -1;
                cbxCekis.SelectedIndex = -1;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Sadece rakam girişine izin veren kontrol
        private void SadeceSayiGirisKontrol(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Sadece rakam girişine izin ver
            }
        }

        // Marka seçildiğinde seri listesini güncelleme işlemi
        private void cbxMarka_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            cbxSeri.Items.Clear();
            cbxSeri.Text = string.Empty;

            if (cbxMarka.SelectedIndex == 0) // BMW
            {
                cbxSeri.Items.AddRange(new string[] { "M3", "M4", "M5", "G30", "320i", "X1", "X3", "X5", "X7", "i4" });
            }
            else if (cbxMarka.SelectedIndex == 1) // Mercedes
            {
                cbxSeri.Items.AddRange(new string[] { "C63", "E180", "E200", "S300", "C200", "A200", "GLA", "GLC", "GLE", "AMG GT" });
            }
            else if (cbxMarka.SelectedIndex == 2) // Tofaş
            {
                cbxSeri.Items.AddRange(new string[] { "Şahin", "Doğan", "Doğan SLX", "Serçe", "Murat", "Kartal", "Fiorino", "Uno" });
            }
            else if (cbxMarka.SelectedIndex == 3) // Dacia
            {
                cbxSeri.Items.AddRange(new string[] { "Dokker", "Sandero", "Lodgy", "Duster", "Logan", "Spring", "Jogger" });
            }
            else if (cbxMarka.SelectedIndex == 4) // Audi
            {
                cbxSeri.Items.AddRange(new string[] { "A3", "A4", "A6", "Q3", "Q5", "Q7", "RS3", "RS5", "e-tron" });
            }
            else if (cbxMarka.SelectedIndex == 5) // Volkswagen
            {
                cbxSeri.Items.AddRange(new string[] { "Golf", "Passat", "Tiguan", "Polo", "Jetta", "Touareg", "Arteon", "ID.4" });
            }
            else if (cbxMarka.SelectedIndex == 6) // Toyota
            {
                cbxSeri.Items.AddRange(new string[] { "Corolla", "Camry", "RAV4", "Hilux", "Prius", "Yaris", "C-HR", "Land Cruiser" });
            }
            else if (cbxMarka.SelectedIndex == 7) // Ford
            {
                cbxSeri.Items.AddRange(new string[] { "Focus", "Fiesta", "Kuga", "Mondeo", "Mustang", "Ranger", "Transit", "EcoSport" });
            }
            else if (cbxMarka.SelectedIndex == 8) // Honda
            {
                cbxSeri.Items.AddRange(new string[] { "Civic", "Accord", "CR-V", "HR-V", "Jazz", "City", "Pilot", "NSX" });
            }
            else if (cbxMarka.SelectedIndex == 9) // Hyundai
            {
                cbxSeri.Items.AddRange(new string[] { "i10", "i20", "i30", "Tucson", "Santa Fe", "Elantra", "Kona", "Venue" });
            }
            else if (cbxMarka.SelectedIndex == 10) // Nissan
            {
                cbxSeri.Items.AddRange(new string[] { "Micra", "Qashqai", "X-Trail", "Juke", "Navara", "Leaf", "GT-R", "370Z" });
            }

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

        private void btnResim_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            pictureBox2.ImageLocation = openFileDialog1.FileName;
        }

        private void AracEkle_Load(object sender, EventArgs e)
        {

        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        



        // Form yükleme işlemleri

    }
}