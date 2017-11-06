using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

using Fornax.Net.Index.Storage;
using Fornax.Net.Document;
using Fornax.Net.Index.IO;
using Fornax.Net.Search;
using Fornax.Net.Util.Security.Cryptography;
using System.Linq;

namespace Corvus._1._0
{
    public partial class PerTab : UserControl
    {
        internal IEnumerable<DocResult> Results = new List<DocResult>();

        public PerTab()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        private void PerTab_Load(object sender, EventArgs e)
        {
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
            panResult.Controls.Add(new PerResult("alakazam", "alakazam"));
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

        }

        private void PerTab_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            double start = Environment.TickCount;
            Task.Factory.StartNew(() =>
            {
                FreeTextQuery ftq = new FreeTextQuery(searchBox.Text.Trim(), Deus.ConfigWin.config);
                IndexSearcher searcher = new IndexSearcher(ftq, Deus.ConfigWin.index, Deus.ConfigWin.repo);

                Results = searcher.GetResults(SortBy.Relevance);
            }).Wait();

            double end = Environment.TickCount;
            var time = (end - start) / 1000;

            lbltime.Text = time.ToString();
            panResult.Controls.Clear();
            foreach (var item in Results)
            {
                panResult.Controls.Add(new PerResult($"{item.Document.FullName}", $"[{item.Document.Extension}]\n {GetSnipes(Deus.ConfigWin.repo, item.Document.FullName)}"));
            }
        }

        private Snippet GetSnipes(Repository repo, string fullname)
        {
            var snf = repo.Snippets;
            foreach (var item in snf)
            {
                if (item.Key == Adler32.Compute(fullname)) return item.Value;
            }
            return new Snippet(2, "Document content not found..");
        }
    }
}
