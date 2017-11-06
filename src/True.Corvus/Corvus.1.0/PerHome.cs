using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Corvus._1._0
{
    public partial class PerHome : UserControl
    {
        PerTab pt;
        private CorvusHome corvusHome;

        //public PerHome() {

        //}

        public PerHome(CorvusHome corvusHome)
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            pt = new PerTab();
            this.corvusHome = corvusHome;
        }

        internal PerTab Page => pt;
        private void PerHome_Load(object sender, EventArgs e)
        {

        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {

                Switch(this);
                pt.searchBox.Text = searchBox.Text;

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }


        public void Switch(PerHome home)
        {
            corvusHome.currentTab.Controls.Remove(home);
            corvusHome.currentTab.Controls.Add(home.Page);
            

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
