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

namespace AracKiralama
{
    public partial class MusteriSayfa : DevExpress.XtraEditors.XtraForm
    {
        public string TcNo { get; set; } // Login'den alınan TC'yi burada saklayacağız.

        public MusteriSayfa()
        {
            InitializeComponent();
        }

       

        private void btnBilgilerim_Click_1(object sender, EventArgs e)
        {
            MusteriBilgiler musteriBilgiler = new MusteriBilgiler();
            musteriBilgiler.TcNo = this.TcNo; // TC Kimlik Numarasını aktar
            musteriBilgiler.Show();
        }

        private void btnSozlesme_Click(object sender, EventArgs e)
        {
            müsteriSayfaSözlesme sözlesme = new müsteriSayfaSözlesme();
            sözlesme.TcNo = this.TcNo;
            sözlesme.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Frm_ilkgiris ilkgiris = new Frm_ilkgiris();
            ilkgiris.Show();
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Çıkmak istediğinize emin misiniz?", "Çıkış Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Eğer kullanıcı "Evet" derse, uygulamayı kapat
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnRapor1_Click(object sender, EventArgs e)
        {
            SirketDegerlendirme degerlendirme = new SirketDegerlendirme();
            degerlendirme.Show();
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            // Kullanıcıdan onay al
            DialogResult result = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?", "Çıkış Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Eğer kullanıcı 'Yes' seçerse uygulamadan çık
            if (result == DialogResult.Yes)
            {
                Frm_ilkgiris frm_İlkgiris = new Frm_ilkgiris();
                frm_İlkgiris.Show();
                this.Hide();
            }
        }

        private void btnBilgilerim_Click(object sender, EventArgs e)
        {
            MusteriBilgiler musteriBilgiler = new MusteriBilgiler();
            musteriBilgiler.TcNo = this.TcNo; // TC Kimlik Numarasını aktar
            musteriBilgiler.Show();
        }

        private void btnSozlesme_Click_1(object sender, EventArgs e)
        {
            müsteriSayfaSözlesme sözlesme = new müsteriSayfaSözlesme();
            sözlesme.TcNo = this.TcNo;
            sözlesme.Show();

        }
    }

}