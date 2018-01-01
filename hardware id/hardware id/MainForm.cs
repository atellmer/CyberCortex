using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Generator
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();

            butGeneration.Click += butGeneration_Click;
        }

        void butGeneration_Click(object sender, EventArgs e)
        {
            fieldContent.Text = Searcher.GetHardware();
        }
    }
}
