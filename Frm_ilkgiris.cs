using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AracKiralama
{
    public partial class Frm_ilkgiris : Form
    {
        public Frm_ilkgiris()
        {
            InitializeComponent();
            tarih();
        }



        void tarih()
        {
            int ay = DateTime.Now.Month;
            int yil = DateTime.Now.Year;
            int gun = DateTime.Now.Day;
            lbl_tarih.Text = "Tarih: " + gun + "/" + (ay) + "/" + yil;
        }

        private void admin_bttn_Click(object sender, EventArgs e)
        {
            LoginForm lform = new LoginForm();
            lform.Show();
            this.Hide();
        }

        private void personel_bttn_Click(object sender, EventArgs e)
        {
            PersonelLogin personelloginfrm = new PersonelLogin();
            personelloginfrm.Show();
            this.Hide();
        }

        

        private void bttn_hakkinda_Click(object sender, EventArgs e)
        {
            HakkındaFrm hakkında = new HakkındaFrm();
            hakkında.Show();
            this.Hide();
        }

        private void musteri_bttn_Click(object sender, EventArgs e)
        {
            MusteriLogin login = new MusteriLogin();
            login.Show();
            this.Hide();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Randevu randevu = new Randevu();
            randevu.Show();
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
    }
}
