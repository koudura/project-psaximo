using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Fornax.Net;
using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Index;
using Fornax.Net.Index.Common;
using Fornax.Net.Index.IO;
using Fornax.Net.Index.Storage;
using Fornax.Net.Util.System;
using Fornax.Net.Util.Text;

namespace Corvus._1._0
{
    public partial class ConfigWin : Form
    {
        public string _Path { get; internal set; }
        System.Timers.Timer t = new System.Timers.Timer(12000)
        {
            AutoReset = true
        };
        bool isCreatable;
        IList<string> selectedFormats;

        FolderBrowserDialog fbdd;
        internal Configuration config;
        internal Repository repo;
        internal InvertedFile index;

        public ConfigWin()
        {
            InitializeComponent();
            fbdd = fbDialog;
            isCreatable = true;
        }

        bool Ascreate = true;

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            fbDialog.ShowDialog();

            txtFolder.Text = fbDialog.SelectedPath;
            _Path = (Directory.Exists(txtFolder.Text))? txtFolder.Text : fbDialog.SelectedPath;
        }

        private void ConfigWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing || e.CloseReason == CloseReason.TaskManagerClosing)
            {
                Application.Exit();
            }
          
        }

        private void txtFolder_TextChanged(object sender, EventArgs e)
        {
            _Path = (Directory.Exists(txtFolder.Text)) ? txtFolder.Text : fbDialog.SelectedPath;
            if (Directory.Exists(txtFolder.Text))
            {
                btnCreate.Enabled = isCreatable;
            }
            else
            {
                btnCreate.Enabled = false;
            }
        }

        private void btnIndex_Click(object sender, EventArgs e)
        {
            if (Ascreate)
            {
                indexWorker.RunWorkerAsync();
            }
            else
            {
                OpenConfigWorker.RunWorkerAsync();
            }
          
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            indexWorker.ReportProgress(5);
            DirectoryInfo dir = new DirectoryInfo(_Path);
            indexWorker.ReportProgress(10);
            repo = Repository.Create(dir, RepositoryType.Local, config);
            Task.WaitAll(Task.Run(() => Deus.Corrector = GramFactory.Default_BiGram));
            indexWorker.ReportProgress(30);
            index = new InvertedFile();
            indexWorker.ReportProgress(15);
            Task.WaitAll(IndexFactory.AddAsync(repo, index));
            indexWorker.ReportProgress(35);
            Task.WaitAll(IndexFactory.SaveAsync(config, index, repo));
            indexWorker.ReportProgress(5);

        }

        private void InitCrawler()
        {
            t.Elapsed += T_Elapsed;
            t.Start();
        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            t.Stop();
            if (!updateWorker.IsBusy)
            {
                updateWorker.RunWorkerAsync();
            }
        }

        private IList<string> FilterFiles(List<string> files, ICollection<string> values)
        {
            var nt = new List<string>();
            foreach (var item in files)
            {
                if (!values.Contains(item)) nt.Add(item);
            }
            return nt;
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

        private (IEnumerable<FileFormat> AsFormats, IEnumerable<string> AsExts) GetSelectedFormats()
        {
            var ff = new List<FileFormat>(); var ss = new List<string>();
            foreach (CheckBox cnt in extensionsBox.Controls)
            {
                if (cnt.Checked)
                {
                    var f = (FileFormat)Enum.Parse(typeof(FileFormat), cnt.Text);
                    ff.Add(f); ss.Add(f.GetString());
                }
            }
            return (ff, ss);
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

            selectedFormats = GetSelectedFormats().AsExts.ToList();
            InitCrawler();
            indexUpdateNote.ShowBalloonTip(2000);
            Hide();
            progressBar1.Value = 0;
        }

        private void updateWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            var files = Directory.EnumerateFiles(_Path, "*.*", SearchOption.AllDirectories).Where(s => selectedFormats.Any(ext => ext == Path.GetExtension(s)));
            var newfiles = FilterFiles(files.ToList(), repo.Corpus.Values);

            if (newfiles.Count > 0)
            {
                var newind = new InvertedFile();
                var newconfig = (Configuration)config.Clone();
                var nrepo = Repository.Create(newfiles.ToArray(), RepositoryType.Local, newconfig);

                Task.WaitAll(IndexFactory.AddAsync(nrepo, newind));
                IndexFactory.Update(ref index, newind);
                IndexFactory.Update(ref repo, nrepo);

                Task.WaitAll(IndexFactory.SaveAsync(config, index, repo));
            }
        }

        private void updateWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            t.Start();
        }

        private void ConfigWin_Load(object sender, EventArgs e)
        {
           
        }

        private void ConfigWin_Shown(object sender, EventArgs e)
        {
            configSelector.Items.AddRange(ConfigFactory.LoadExistingConfigurations().ToList());
            configSelector.SelectedItem = (configSelector.Items.Count > 0 )?  configSelector.Items.ToArray()[0] : "";
            Swap(!rbtnCreateChoice.Checked);
        }

        private void rbtnLoadChoice_CheckedChanged(object sender, EventArgs e)
        {
            Swap(rbtnLoadChoice.Checked);
        }

        private void Swap(bool v)
        {
            if (v)
            {
                configSelector.Enabled = true;
                BtnloadConfig.Enabled = true;

                btnBrowse.Enabled = false;
                TxtconfigId.Enabled = false;
                btnCreate.Enabled = isCreatable = false;
            }
            else
            {
                configSelector.Enabled = false;
                BtnloadConfig.Enabled = false;

                btnBrowse.Enabled = true;
                TxtconfigId.Enabled = true;
                btnCreate.Enabled = isCreatable = Directory.Exists(txtFolder.Text);
            }
        }

        private void rbtnCreateChoice_CheckedChanged(object sender, EventArgs e)
        {
            Swap(!rbtnCreateChoice.Checked);
        }

        private void OpenConfigWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            OpenConfigWorker.ReportProgress(5);
            var data = IndexFactory.LoadAsync(config).Result;
            OpenConfigWorker.ReportProgress(10);
            repo = data.Repository;
            OpenConfigWorker.ReportProgress(10);
            index = data.Index;
            OpenConfigWorker.ReportProgress(20);
            Task.WaitAll(Task.Run(() => Deus.Corrector = GramFactory.Default_BiGram));
            OpenConfigWorker.ReportProgress(25);
            selectedFormats = ToListFormat(config.Formats);
            OpenConfigWorker.ReportProgress(25);
            OpenConfigWorker.ReportProgress(5);
        }

        private IList<string> ToListFormat(FileFormat[] formats)
        {
            var gets = new List<string>();
            foreach (var f in formats)
            {
                gets.Add(f.GetString());
            }
            return gets;
        }

        private void BtnloadConfig_Click(object sender, EventArgs e)
        {
            var id = configSelector.SelectedItem.ToString();
            Task.WaitAll(Task.Run(() => config = ConfigFactory.OpenConfiguration(id)));
            Ascreate = false;
            btnIndex.Enabled = true;
        }

        private void OpenConfigWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value += e.ProgressPercentage;
        }

        private void OpenConfigWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            indexUpdateNote.ShowBalloonTip(2000);
            Hide();
            progressBar1.Value = 0;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            var lang = FornaxLanguage.English;
            var tokenizer = new PerFieldTokenizer();


            Task.WaitAll(Task.Run(() => config = ConfigFactory.GetConfiguration(TxtconfigId.Text.Clean(false), FetchAttribute.Weak, CachingMode.Default, lang, tokenizer, GetSelectedFormats().AsFormats.ToArray())));
            Ascreate = true;
            btnIndex.Enabled = true;
        }
    }
}
