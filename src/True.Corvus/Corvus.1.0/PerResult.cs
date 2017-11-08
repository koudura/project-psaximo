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

    public partial class PerResult : UserControl
    {
        public string Header { get; set; } // header -> Document.Link

        public string Snippet { get; set; } //todo: to be replaced with Snippet object

        public PerResult(string header, string snippet)
        {
            Anchor = AnchorStyles.Left & AnchorStyles.Right;
            Header = header;
            Snippet = snippet;
            InitializeComponent();
            linkLabel.Text = Header;
            snippetLabel.Text = Snippet;

        }

        public PerResult(Result res)
        {

            Header = res.Header;
            Snippet = res.Snippet;
            InitializeComponent();
            linkLabel.Text = Header;
            snippetLabel.Text = Snippet;

        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel.LinkVisited = true;
            System.Diagnostics.Process.Start(Header);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    public class Result
    {
        public string Header { get; set; }
        public string Snippet { get; set; }
        public Result(string header, string snippet)
        {
            Header = header;
            Snippet = snippet;
        }

    }


    
}
