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
    public partial class AracListele : DevExpress.XtraEditors.XtraForm
    {
        public AracListele()
        {
            InitializeComponent();
        }

        private string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";

        public string GonderenForm { get; internal set; }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPlaka.Text))
            {
                MessageBox.Show("Henüz seçim yapmadınız. Lütfen bir Araç seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Fonksiyonu sonlandır
            }

            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();

            string komutCumlesi = "Update Araclar set marka=@Marka,Seri=@Seri,Model=@Model,Renk=@Renk,Kilometre=@Kilometre,Yakit=@Yakit,Kira_Ücreti=@Kira_Ücreti,Durumu=@Durumu,resim=@resim," +
                "Motor_Gucu=@Motor_Gucu,Vites=@Vites,Cekis=@Cekis,Kapı=@Kapı,Kasa_Tipi=@Kasa_Tipi where Plaka=@plaka";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            komut.Parameters.AddWithValue("@Plaka", txtPlaka.Text);
            komut.Parameters.AddWithValue("@Marka", cbxMarka.Text);
            komut.Parameters.AddWithValue("@Seri", cbxSeri.Text);
            komut.Parameters.AddWithValue("@Model", txtModel.Text);
            komut.Parameters.AddWithValue("@Renk", txtRenk.Text);
            komut.Parameters.AddWithValue("@Kilometre", txtKm.Text);
            komut.Parameters.AddWithValue("@Yakit", cbxYakit.Text);
            komut.Parameters.AddWithValue("@Kira_Ücreti", txtUcret.Text);
            komut.Parameters.AddWithValue("@Durumu", cbxDurum.Text);
            komut.Parameters.AddWithValue("@resim", pictureBox2.ImageLocation);
            komut.Parameters.AddWithValue("@Motor_Gucu", cbxMotorGucu.Text);
            komut.Parameters.AddWithValue("@Vites", cbxVites.Text);
            komut.Parameters.AddWithValue("@Cekis", cbxCekis.Text);
            komut.Parameters.AddWithValue("@Kapı", cbxKapi.Text);
            komut.Parameters.AddWithValue("@Kasa_Tipi", cbxKasaTipi.Text);

            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Araç Bilgileri Güncellendi!");
            Arac_Listele();
        }

        public void Arac_Listele()
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();

            String komutCumlesi = "Select * From Araclar";
            //String komutCumlesi = "Select Plaka,Marka,Seri,Model,Renk,Kilometre,Yakit,Kira_Ücreti,Durumu from Araclar";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }
        private void AracListele_Load(object sender, EventArgs e)
        {
            Arac_Listele();
            BosAraclar_Listele();
            DoluAraclar_Listele();
            TamirAraclar_Listele();
            TemizlikAraclar_Listele();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtPlaka.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            cbxMarka.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            cbxSeri.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            txtModel.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            txtRenk.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            txtKm.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            cbxYakit.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            txtUcret.Text= dataGridView1.CurrentRow.Cells[8].Value.ToString();
            cbxDurum.Text = dataGridView1.CurrentRow.Cells[9].Value.ToString();
            pictureBox2.ImageLocation = dataGridView1.CurrentRow.Cells["resim"].Value.ToString();
            cbxMotorGucu.Text = dataGridView1.CurrentRow.Cells["Motor_Gucu"].Value.ToString();
            cbxVites.Text = dataGridView1.CurrentRow.Cells["Vites"].Value.ToString();
            cbxCekis.Text = dataGridView1.CurrentRow.Cells["Cekis"].Value.ToString();
            cbxKapi.Text = dataGridView1.CurrentRow.Cells["Kapı"].Value.ToString();
            cbxKasaTipi.Text = dataGridView1.CurrentRow.Cells["Kasa_Tipi"].Value.ToString();

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPlaka.Text))
            {
                MessageBox.Show("Henüz seçim yapmadınız. Lütfen bir Araç seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Fonksiyonu sonlandır
            }

            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();

            string komutCumlesi = "Delete from Araclar where Plaka='" + dataGridView1.CurrentRow.Cells["Plaka"].Value.ToString() + "'";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);

            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kayıtlı Araç Bilgileri Silindi!");
            Arac_Listele();
            txtPlaka.Text = string.Empty;
            cbxMarka.Text = string.Empty;
            cbxSeri.Text = string.Empty;
            txtModel.Text = string.Empty;
            txtRenk.Text = string.Empty;
            txtKm.Text = string.Empty;
            cbxYakit.Text = string.Empty;
            txtUcret.Text = string.Empty;
            cbxDurum.Text = string.Empty;
            pictureBox2.Image = null;
            cbxCekis.Text = string.Empty;
            cbxMotorGucu.Text = string.Empty;
            cbxKasaTipi.Text = string.Empty;
            cbxVites.Text = string.Empty;
            cbxKapi.Text = string.Empty;

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbxMarka_SelectedIndexChanged(object sender, EventArgs e)
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

        private void txtPlakaAra_EditValueChanged(object sender, EventArgs e)
        {


            using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
            {
                try
                {
                    baglanti.Open();

                    // DataGridView'i doldur
                    string komutCumlesi = "SELECT * FROM Araclar WHERE Plaka LIKE @Plaka";
                    SqlDataAdapter da = new SqlDataAdapter(komutCumlesi, baglanti);
                    da.SelectCommand.Parameters.AddWithValue("@Plaka", txtPlakaAra.Text + "%");
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                    // TextBox ve diğer kontrolleri doldur
                    SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
                    komut.Parameters.AddWithValue("@Plaka", txtPlakaAra.Text + "%");
                    SqlDataReader read = komut.ExecuteReader();

                    if (read.Read())
                    {
                        txtPlaka.Text = read["Plaka"].ToString();
                        cbxMarka.Text = read["marka"].ToString();
                        cbxSeri.Text = read["Seri"].ToString();
                        txtModel.Text = read["Model"].ToString();
                        txtRenk.Text = read["Renk"].ToString();
                        txtKm.Text = read["Kilometre"].ToString();
                        cbxYakit.Text = read["Yakit"].ToString();
                        txtUcret.Text = read["Kira_Ücreti"].ToString();
                        cbxDurum.Text = read["Durumu"].ToString();
                        cbxVites.Text = read["Vites"].ToString();
                        cbxMotorGucu.Text = read["Motor_Gucu"].ToString();
                        cbxCekis.Text = read["Cekis"].ToString();
                        cbxKasaTipi.Text = read["Kasa_Tipi"].ToString();
                        cbxKapi.Text = read["Kapı"].ToString();

                        // Resim ekleme
                        try
                        {
                            if (read["resim"] != DBNull.Value && !string.IsNullOrEmpty(read["resim"].ToString()))
                            {
                                pictureBox2.Image = Image.FromFile(read["resim"].ToString());
                            }
                            else
                            {
                                pictureBox2.Image = null; // Eğer resim yoksa PictureBox'ı temizle
                            }
                        }
                        catch (Exception ex)
                        {
                            pictureBox2.Image = null; // Resim yüklenemezse sadece resim kısmını temizle
                            MessageBox.Show("Aracın Resmi Sistemde yok.: " + ex.Message, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }

        // Kontrolleri temizlemek için yardımcı metot
        

    
        private void BosAraclar_Listele()
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();

            string komutCumlesi = "Select * From Araclar Where Durumu = 'Boş'";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridBosAraclar.DataSource = dt;
            baglanti.Close();
        }
        private void DoluAraclar_Listele()
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();

            string komutCumlesi = "Select * From Araclar Where Durumu = 'Dolu'";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridDoluAraclar.DataSource = dt;
            baglanti.Close();
        }
        private void TamirAraclar_Listele()
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();

            string komutCumlesi = "Select * From Araclar Where Durumu = 'Tamir'";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            baglanti.Close();
        }

        private void TemizlikAraclar_Listele()
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();

            string komutCumlesi = "Select * From Araclar Where Durumu = 'Temizlik'";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView3.DataSource = dt;
            baglanti.Close();
        }


        private void btnResim_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            pictureBox2.ImageLocation = openFileDialog1.FileName;
        }

        private void dataGridBosAraclar_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtPlaka.Text = dataGridBosAraclar.CurrentRow.Cells[1].Value.ToString();
            cbxMarka.Text = dataGridBosAraclar.CurrentRow.Cells[2].Value.ToString();
            cbxSeri.Text = dataGridBosAraclar.CurrentRow.Cells[3].Value.ToString();
            txtModel.Text = dataGridBosAraclar.CurrentRow.Cells[4].Value.ToString();
            txtRenk.Text = dataGridBosAraclar.CurrentRow.Cells[5].Value.ToString();
            txtKm.Text = dataGridBosAraclar.CurrentRow.Cells[6].Value.ToString();
            cbxYakit.Text = dataGridBosAraclar.CurrentRow.Cells[7].Value.ToString();
            txtUcret.Text = dataGridBosAraclar.CurrentRow.Cells[8].Value.ToString();
            cbxDurum.Text = dataGridBosAraclar.CurrentRow.Cells[9].Value.ToString();
            pictureBox2.ImageLocation = dataGridBosAraclar.CurrentRow.Cells["resim"].Value.ToString();
            cbxMotorGucu.Text = dataGridBosAraclar.CurrentRow.Cells["Motor_Gucu"].Value.ToString();
            cbxVites.Text = dataGridBosAraclar.CurrentRow.Cells["Vites"].Value.ToString();
            cbxCekis.Text = dataGridBosAraclar.CurrentRow.Cells["Cekis"].Value.ToString();
            cbxKapi.Text = dataGridBosAraclar.CurrentRow.Cells["Kapı"].Value.ToString();
            cbxKasaTipi.Text = dataGridBosAraclar.CurrentRow.Cells["Kasa_Tipi"].Value.ToString();
        }

        private void dataGridDoluAraclar_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtPlaka.Text = dataGridDoluAraclar.CurrentRow.Cells[1].Value.ToString();
            cbxMarka.Text = dataGridDoluAraclar.CurrentRow.Cells[2].Value.ToString();
            cbxSeri.Text = dataGridDoluAraclar.CurrentRow.Cells[3].Value.ToString();
            txtModel.Text = dataGridDoluAraclar.CurrentRow.Cells[4].Value.ToString();
            txtRenk.Text = dataGridDoluAraclar.CurrentRow.Cells[5].Value.ToString();
            txtKm.Text = dataGridDoluAraclar.CurrentRow.Cells[6].Value.ToString();
            cbxYakit.Text = dataGridDoluAraclar.CurrentRow.Cells[7].Value.ToString();
            txtUcret.Text = dataGridDoluAraclar.CurrentRow.Cells[8].Value.ToString();
            cbxDurum.Text = dataGridDoluAraclar.CurrentRow.Cells[9].Value.ToString();
            pictureBox2.ImageLocation = dataGridDoluAraclar.CurrentRow.Cells["resim"].Value.ToString();
            cbxMotorGucu.Text = dataGridDoluAraclar.CurrentRow.Cells["Motor_Gucu"].Value.ToString();
            cbxVites.Text = dataGridDoluAraclar.CurrentRow.Cells["Vites"].Value.ToString();
            cbxCekis.Text = dataGridDoluAraclar.CurrentRow.Cells["Cekis"].Value.ToString();
            cbxKapi.Text = dataGridDoluAraclar.CurrentRow.Cells["Kapı"].Value.ToString();
            cbxKasaTipi.Text = dataGridDoluAraclar.CurrentRow.Cells["Kasa_Tipi"].Value.ToString();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
            
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            Arac_Listele();
            BosAraclar_Listele();
            DoluAraclar_Listele();
            TamirAraclar_Listele();
            TemizlikAraclar_Listele();
        }
        //YENİLE
        private void btnMusteriEkle_Click(object sender, EventArgs e)
        {
            Arac_Listele();
            BosAraclar_Listele();
            DoluAraclar_Listele();
            TamirAraclar_Listele();
            TemizlikAraclar_Listele();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtPlaka.Text = dataGridView3.CurrentRow.Cells[1].Value.ToString();
            cbxMarka.Text = dataGridView3.CurrentRow.Cells[2].Value.ToString();
            cbxSeri.Text = dataGridView3.CurrentRow.Cells[3].Value.ToString();
            txtModel.Text = dataGridView3.CurrentRow.Cells[4].Value.ToString();
            txtRenk.Text = dataGridView3.CurrentRow.Cells[5].Value.ToString();
            txtKm.Text = dataGridView3.CurrentRow.Cells[6].Value.ToString();
            cbxYakit.Text = dataGridView3.CurrentRow.Cells[7].Value.ToString();
            txtUcret.Text = dataGridView3.CurrentRow.Cells[8].Value.ToString();
            cbxDurum.Text = dataGridView3.CurrentRow.Cells[9].Value.ToString();
            pictureBox2.ImageLocation = dataGridView3.CurrentRow.Cells["resim"].Value.ToString();
            cbxMotorGucu.Text = dataGridView3.CurrentRow.Cells["Motor_Gucu"].Value.ToString();
            cbxVites.Text = dataGridView3.CurrentRow.Cells["Vites"].Value.ToString();
            cbxCekis.Text = dataGridView3.CurrentRow.Cells["Cekis"].Value.ToString();
            cbxKapi.Text = dataGridView3.CurrentRow.Cells["Kapı"].Value.ToString();
            cbxKasaTipi.Text = dataGridView3.CurrentRow.Cells["Kasa_Tipi"].Value.ToString();
        }
        //tamir
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtPlaka.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            cbxMarka.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
            cbxSeri.Text = dataGridView2.CurrentRow.Cells[3].Value.ToString();
            txtModel.Text = dataGridView2.CurrentRow.Cells[4].Value.ToString();
            txtRenk.Text = dataGridView2.CurrentRow.Cells[5].Value.ToString();
            txtKm.Text = dataGridView2.CurrentRow.Cells[6].Value.ToString();
            cbxYakit.Text = dataGridView2.CurrentRow.Cells[7].Value.ToString();
            txtUcret.Text = dataGridView2.CurrentRow.Cells[8].Value.ToString();
            cbxDurum.Text = dataGridView2.CurrentRow.Cells[9].Value.ToString();
            pictureBox2.ImageLocation = dataGridView2.CurrentRow.Cells["resim"].Value.ToString();
            cbxMotorGucu.Text = dataGridView2.CurrentRow.Cells["Motor_Gucu"].Value.ToString();
            cbxVites.Text = dataGridView2.CurrentRow.Cells["Vites"].Value.ToString();
            cbxCekis.Text = dataGridView2.CurrentRow.Cells["Cekis"].Value.ToString();
            cbxKapi.Text = dataGridView2.CurrentRow.Cells["Kapı"].Value.ToString();
            cbxKasaTipi.Text = dataGridView2.CurrentRow.Cells["Kasa_Tipi"].Value.ToString();
        }
    }
}
