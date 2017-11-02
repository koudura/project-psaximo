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
        ConfigWin cfg;
        PerHome home;

        public CorvusHome() {
            InitializeComponent();
            home = new PerHome(this);
            tabHolders.SelectedTab.Controls.Add(home);
        }

        internal TabPage currentTab => tabHolders.SelectedTab;
        private void CorvusHome_Shown(object sender, EventArgs e) {
            cfg = new ConfigWin();
            btnConfig.PerformClick();
        }

        private void newTabBtn_Click(object sender, EventArgs e) {
            var new_page = new TabPage("New Tab");
            var new_home = new PerHome(this);
            new_page.Controls.Add(new_home);
            tabHolders.Controls.Add(new_page);
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            cfg.ShowDialog();
        }

        private void CorvusHome_Load(object sender, EventArgs e)
        {
            
        }

        private void closeTabBtn_Click(object sender, EventArgs e)
        {
            if(tabHolders.TabPages.Count > 1)
            {
                tabHolders.TabPages.Remove(currentTab);
            }
        }
    }
}
