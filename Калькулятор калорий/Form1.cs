using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Калькулятор_калорий
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Product selectedItem;

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] temp = File.ReadAllLines("../../калории.txt");
            comboBox1.Items.Clear();
            for(int i=0;i<temp.Length;i++)
            {
                string[] temp2 = temp[i].Split(';');
                Product product = new Product(temp2[0], (float)Convert.ToDouble(temp2[1]), (float)Convert.ToDouble(temp2[2]),
                    (float)Convert.ToDouble(temp2[3]), (float)Convert.ToDouble(temp2[4]));
                comboBox1.Items.Add(product);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(comboBox1.Text, numericUpDown1.Value, textBox2.Text, textBox3.Text, textBox4.Text, textBox1.Text);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedItem = (Product)comboBox1.Items[comboBox1.SelectedIndex];
            textBox2.Text = selectedItem.kall + "";
            textBox3.Text = selectedItem.belki + "";
            textBox4.Text = selectedItem.zhiry + "";
            textBox1.Text = selectedItem.uglevody + "";
        }
    }
}
