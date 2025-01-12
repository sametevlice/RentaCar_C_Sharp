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
    public partial class Baslangic : Form
    {
        public Baslangic()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel2.Width += 6;
            if (panel2.Width >= 587)
            {
                timer1.Stop();

                Frm_ilkgiris ilkgrs = new Frm_ilkgiris();
                ilkgrs.Show();
                this.Hide();
            }

        }


        private void İlkGiris_Load(object sender, EventArgs e)
        {

        }
    }
}
