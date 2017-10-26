using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Corvus._1._0
{
    public partial class CorvusHome : Form
    {
        public CorvusHome() {
            InitializeComponent();
        }

        private void CorvusHome_Shown(object sender, EventArgs e) {
       
        }

        private void newTabBtn_Click(object sender, EventArgs e) {
            var new_page = new TabPage("New Tab");
            var new_home = new PerHome();
            new_page.Controls.Add(new_home);
            tabHolders.Controls.Add(new_page);
        }
    }
}
