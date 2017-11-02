//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using Fornax.Net.Util.IO.Readers;

//namespace Corvus._1._0
//{
//    public partial class XmlExtractor : Form
//    {
//        IReader reader;
        
//        public XmlExtractor() {
//            InitializeComponent();
//        }

//        private void button1_Click(object sender, EventArgs e) {
//            double start = Environment.TickCount;
//            reader = new FornaxReader(textBox1.Text);
//            var t = ExtractText();
//            richTextBox1.Text = t;
//            double end = Environment.TickCount;
//            label1.Text = " Completed in " + ((end - start) / 1000).ToString() +"s";
//        }


//        private async Task<string> ExtractTextAsync() {
//            string text = await reader.XmlReadAsync();
//            return text;
//        }

//        private string ExtractText() {
//            return reader.XmlRead();
//        }
//    }
//}
