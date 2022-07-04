using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSTeam
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DBConnection.startConnection();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReturnedInvoice.Insert(51, 532, 101, 11, 12, 10);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void toggleButton1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Shift x = Shift.Select(15);
            textBox1.Text = x.Start_time.Month.ToString();
            Hidden.Text = x.End_time.Month.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
