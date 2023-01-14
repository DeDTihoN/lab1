using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string Number1 = textBox1.Text;
            string Number2 = textBox2.Text;
            int n=-1, m=-1;
            if (int.TryParse(Number1, out n) && int.TryParse(Number2, out m))
            {
                Form1.selfref.BuildData(n, m);
            }
            else
            {
                MessageBox.Show("Введіть коректні розміри таблиці");
            }
        }
    }
}
