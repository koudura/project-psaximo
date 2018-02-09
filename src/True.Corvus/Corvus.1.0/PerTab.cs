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
using Fornax.Net.Analysis.Tools;
using Fornax.Net.Index.Common;
using System.Threading;

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

            if (chkCorrect.Checked)
            {
                var t = searchBox.Text;
                if (t.Contains(" "))
                {
                    var tt = t.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    t = (tt.Length > 1) ? tt[tt.Length - 1] : t;
                }
                searchBox.AutoCompleteCustomSource = GenerationSuggetion(t, 0.6f);

            }
        }
        public static AutoCompleteStringCollection GenerationSuggetion(string text, float threshold)
        {
            var count = new AutoCompleteStringCollection();
            var ngram = new Ngram(text, 2, NgramModel.Character, true);

            var common = GramFactory.IntersectOf(GramFactory.SubGramIndex(ngram, Deus.Corrector));
            var close = EditFactory.RetrieveCommon(text, common, threshold);

            foreach (var found in close)
            {
                count.Add(found.Key);
            }
            return count;
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
                var q = searchBox.Text.Split(); //test line
                var query = (searchBox.Text.Trim().Contains(" ")) ? searchBox.Text.Trim() : q[q.Length - 1];
                
                FreeTextQuery ftq = new FreeTextQuery((chkCorrect.Checked) ? Check(query) : query, Deus.ConfigWin.config);
                IndexSearcher searcher = new IndexSearcher(ftq, Deus.ConfigWin.index, Deus.ConfigWin.repo);

                Results = searcher.GetResults(SortBy.Size);
            }).Wait();

            double end = Environment.TickCount;
            var time = (end - start) / 1000;

            lbltime.Text = time.ToString();
            panResult.Controls.Clear();
            foreach (var item in Results)
            {
                if (chkSnippets.Checked)
                {
                    panResult.Controls.Add(new PerResult($"{item.Document.FullName}", $"[{item.Document.Extension}]\n {GetSnipes(Deus.ConfigWin.repo, item.Document.FullName)}"));
                }
                else
                {
                    panResult.Controls.Add(new PerResult($"{item.Document.FullName}", $"[{item.Document.Extension}]\n {item.Document.FullName}"));
                }

            }
        }

        private string Check(string text)
        {
            var ngram = new Ngram(text, 2, NgramModel.Character, true);

            var sub = GramFactory.SubGramIndex(ngram, Deus.Corrector);
            var common = GramFactory.IntersectOf(sub);
            var close = EditFactory.RetrieveCommon(text, common, 0.6f);

            return (close == null || close.Count == 0) ? text : close.First().Key;
        }


        private Snippet GetSnipes(Repository repo, string fullname)
        {
            var snf = repo.Snippets;
            if (snf != null)
            {
                foreach (var item in snf)
                {
                    if (item.Key == Adler32.Compute(fullname)) return item.Value;
                }
            }
            return new Snippet(4, "Document content Not found..");
        }

        private void searchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }

        private void panResult_Paint(object sender, PaintEventArgs e)
        {
             
        }
    }
}
