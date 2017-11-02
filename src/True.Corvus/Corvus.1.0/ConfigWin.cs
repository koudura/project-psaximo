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

namespace Corvus._1._0
{
    public partial class ConfigWin : Form
    {
        string[] select;
        internal string Path => fbDialog.SelectedPath;
        FolderBrowserDialog fbdd;

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
                btnIndex.Enabled = true;
            }
            else
            {
                btnIndex.Enabled = false;
            }
        }

        private void btnIndex_Click(object sender, EventArgs e)
        {

            ICollection selected = lsbxExt.SelectedItems;
            if (selected.Count == 0)
            {
                select = new string[lsbxExt.Items.Count];
                int selectIndex = 0;
                foreach (var item in lsbxExt.Items)
                {
                    select[selectIndex] = "." + ((string)item).ToLower();
                    selectIndex++;
                }
            }
            else
            {

                select = new string[selected.Count];
                int selectIndex = 0;
                foreach (var item in selected)
                {
                    select[selectIndex] = "." + ((string)item).ToLower();
                    selectIndex++;
                }
            }


            //DocDirectory dir = new DocDirectory(new DirectoryInfo(txtFolder.Text));

            //double start = Environment.TickCount;
            //IndexWriter ind = new IndexWriter(new DirectoryInfo(@"C:\Users\user\Documents\c# programs\HSE\HSE"));
            //ind.indexDirectory(dir);
            //double end = Environment.TickCount;

            //double time = (end - start) / 1000;
            //lblTime.Text = time.ToString();
            //Task.Delay(3000);
            //this.Hide();
            Hide();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var language = FornaxLanguage.English;
            //var temp_supportedFormat = new FileFormat[] { FileFormat.Txt, FileFormat.Docx, FileFormat.Doc };
            DirectoryInfo dir = new DirectoryInfo(Path);

            Configuration config = ConfigFactory.Default;
            var repo = Repository.Create(dir, RepositoryType.Local, config);



        }



    }
}
