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
    public partial class PerTab : UserControl
    {
        public PerTab() {
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        private void PerTab_Load(object sender, EventArgs e) {
            searchBox.Focus();
            searchBox.DeselectAll();
           
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        internal void AddResult(IList<Result> res) 
        {
            //foreach (var item in res)
            //{
            //    panResult.Controls.Add(new PerResult(item));
            //}
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            panResult.Controls.Clear();
            panResult.Controls.Add(new PerResult("alakazam","alakazam"));
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void searchBox_TextChanged_1(object sender, EventArgs e)
        {
            searchBox.Focus();
        }

        private void PerTab_DragOver(object sender, DragEventArgs e)
        {
            searchBox.Focus();
        }

        private void PerTab_Paint(object sender, PaintEventArgs e)
        {
            searchBox.Focus();
        }

        private void panel5_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {

        }
    }
}
