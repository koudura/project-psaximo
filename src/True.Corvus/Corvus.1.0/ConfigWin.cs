using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fornax.Net;
using Fornax.Net.Util.System;
using Fornax.Net.Index.Storage;
using Fornax.Net.Util.IO.Readers;
using Fornax.Net.Index;
using System.Drawing;
using Fornax.Net.Util.Text;
using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Index.IO;
using Fornax.Net.Util.Threading;
using System.Threading;
using System.Diagnostics;

namespace Corvus._1._0
{
    public partial class ConfigWin : Form
    {
        internal string _Path => fbDialog.SelectedPath;

        bool configcreated;

        FolderBrowserDialog fbdd;
        internal Configuration config;
        internal Repository repo;
        internal InvertedFile index;

        public ConfigWin()
        {
            InitializeComponent();
            fbdd = fbDialog;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            fbDialog.ShowDialog();
            txtFolder.Text = fbDialog.SelectedPath;

        }

        private void ConfigWin_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void txtFolder_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(txtFolder.Text))
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }
        }

        private void btnIndex_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            backgroundWorker1.ReportProgress(5);
            DirectoryInfo dir = new DirectoryInfo(_Path);
            backgroundWorker1.ReportProgress(10);
            repo = Repository.Create(dir, RepositoryType.Local, config);
            backgroundWorker1.ReportProgress(20);
            index = new InvertedFile();
            backgroundWorker1.ReportProgress(25);
            Task.WaitAll(IndexFactory.AddAsync(repo, index));
            backgroundWorker1.ReportProgress(30);
            Task.WaitAll(IndexFactory.SaveAsync(config, index, repo));
            backgroundWorker1.ReportProgress(10);

        }

        private void InitCrawler()
        {

            System.Timers.Timer t = new System.Timers.Timer(12000)
            {
                AutoReset = true
            };
            t.Elapsed += T_Elapsed;
            t.Start();
        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var files = Directory.EnumerateFiles(_Path).ToList();
            var newfiles = files.Except(repo.Corpus.Values).ToArray();
            var newind = new InvertedFile();
            var nrepo = Repository.Create(newfiles, RepositoryType.Local, config);
            Task.WaitAll(IndexFactory.AddAsync(nrepo, newind));
            IndexFactory.Update(ref index, newind);
            Task.WaitAll(IndexFactory.SaveAsync(config, index, repo));
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private static bool mouseDown;
        private static Point lastLoc;

        private void ConfigWin_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLoc = e.Location;
        }

        private void ConfigWin_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void ConfigWin_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Location = new Point((Location.X - lastLoc.X) + e.X, (Location.Y - lastLoc.Y) + e.Y);
                Update();
            }
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var lang = FornaxLanguage.English;
            var tokenizer = new PerFieldTokenizer();


            await Task.Run(() => config = ConfigFactory.GetConfiguration(textBox1.Text.Clean(false), FetchAttribute.Weak, CachingMode.Default, lang, tokenizer, GetSelectedFormats().ToArray()));

            configcreated = true;
            btnIndex.Enabled = true;
        }

        private IEnumerable<FileFormat> GetSelectedFormats()
        {
            foreach (CheckBox cnt in extensionsBox.Controls)
            {
                if (cnt.Checked)
                {
                    yield return (FileFormat)Enum.Parse(typeof(FileFormat), cnt.Text);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value += e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            InitCrawler();

            Hide();
            progressBar1.Value = 0;

            indexUpdateNote.ShowBalloonTip(2000);

            

        }
    }
}
