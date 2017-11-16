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
using System.Collections;
using System.Xml;
using System.Xml.Linq;

namespace Калькулятор_калорий
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Product selectedItem;
        Dictionary<DateTime, List<Product>> tableData;
        String pathToXMLdataTable = "../../tableData.xml";


        private void Form1_Load(object sender, EventArgs e)
        {
            string[] temp = File.ReadAllLines("../../калории.txt");
            comboBox1.Items.Clear();
            for (int i = 0; i < temp.Length; i++)
            {
                string[] temp2 = temp[i].Split(';');
                Product product = new Product(temp2[0], (float)Convert.ToDouble(temp2[1]), (float)Convert.ToDouble(temp2[2]),
                    (float)Convert.ToDouble(temp2[3]), (float)Convert.ToDouble(temp2[4]));
                comboBox1.Items.Add(product);
            }
            tableData = new Dictionary<DateTime, List<Product>>();
            readTableDataFromFile();
            updateDataGridview();
        }

        void readTableDataFromFile()
        {
            if (File.Exists(pathToXMLdataTable))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(pathToXMLdataTable);
                XmlElement xRoot = xDoc.DocumentElement;
                tableData.Clear();
                foreach (XmlElement day in xRoot)
                {
                    DateTime dateTime = new DateTime();
                    List<Product> list = new List<Product>();
                    foreach (XmlElement child in day)
                    {
                        if (child.Name == "date")
                        {
                            dateTime = Convert.ToDateTime(child.InnerText);
                        }
                        if (child.Name == "products")
                        {
                            foreach (XmlElement product in child)
                            {
                                float kall = 0;
                                float belki = 0;
                                float zhiry = 0;
                                float uglevody = 0;
                                int ves = 0;
                                string name = "";
                                foreach (XmlElement pole in product)
                                {
                                    if (pole.Name == "name")
                                    {
                                        name = pole.InnerText;
                                    }
                                    if (pole.Name == "kall")
                                    {
                                        kall = (float)Convert.ToDouble(pole.InnerText);
                                    }
                                    if (pole.Name == "belki")
                                    {
                                        belki = (float)Convert.ToDouble(pole.InnerText);
                                    }
                                    if (pole.Name == "zhiry")
                                    {
                                        zhiry = (float)Convert.ToDouble(pole.InnerText);
                                    }
                                    if (pole.Name == "uglevody")
                                    {
                                        uglevody = (float)Convert.ToDouble(pole.InnerText);
                                    }
                                    if (pole.Name == "ves")
                                    {
                                        ves = Convert.ToInt32(pole.InnerText);
                                    }
                                }
                                Product Xproduct = new Product(name, kall, belki, zhiry, uglevody);
                                Xproduct.ves = ves;
                                list.Add(Xproduct);
                            }
                        }
                    }
                    tableData.Add(dateTime, list);
                }
            }
        }
        void writeTableDataToFile()
        {
            XDocument xDoc = new XDocument();
            XElement root = new XElement("days");
            foreach (DateTime i in tableData.Keys)
            {
                List<Product> list = tableData[i];
                XElement day = new XElement("day");
                XElement date = new XElement("date");
                date.Value = i + "";
                XElement products = new XElement("products");
                foreach (Product p in list)
                {
                    XElement product = new XElement("product");
                    XElement name = new XElement("name");
                    name.Value = p.name;
                    XElement kall = new XElement("kall");
                    kall.Value = p.kall + "";
                    XElement belki = new XElement("belki");
                    belki.Value = p.belki + "";
                    XElement zhiry = new XElement("zhiry");
                    zhiry.Value = p.zhiry + "";
                    XElement uglevody = new XElement("uglevody");
                    uglevody.Value = p.uglevody + "";
                    XElement ves = new XElement("ves");
                    ves.Value = p.ves + "";
                    product.Add(name);
                    product.Add(kall);
                    product.Add(belki);
                    product.Add(zhiry);
                    product.Add(uglevody);
                    product.Add(ves);
                    products.Add(product);
                }
                day.Add(date);
                day.Add(products);
                root.Add(day);
            }

            xDoc.Add(root);

            xDoc.Save(pathToXMLdataTable);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addRow();
        }

        void addRow()
        {
            Product p = new Product(selectedItem.name, selectedItem.kall, selectedItem.belki, selectedItem.zhiry, selectedItem.uglevody);
            p.ves = Convert.ToInt32(numericUpDown1.Value);
            DateTime dateTime = monthCalendar1.SelectionRange.End.Date;
            if(!tableData.ContainsKey(dateTime))
            {
                tableData.Add(dateTime, new List<Product>());
            }
            tableData[dateTime].Add(p);
            writeTableDataToFile();
            updateDataGridview();
        }

        void updateDataGridview()
        {
            DateTime dateTime = monthCalendar1.SelectionRange.End.Date;
            dataGridView1.Rows.Clear();
            if (tableData.ContainsKey(dateTime))
            {
                List<Product> list = tableData[dateTime];
                foreach (Product p in list)
                {
                    dataGridView1.Rows.Add(p.name, p.ves, p.getKall(), p.getBelki(), p.getZhiry(), p.getUglevody());
                }
            }
        }

        void updateProduct()
        {
            selectedItem = (Product)comboBox1.Items[comboBox1.SelectedIndex];
            selectedItem.ves = Convert.ToInt32(numericUpDown1.Value);
            textBox2.Text = selectedItem.getKall() + "";
            textBox3.Text = selectedItem.getBelki() + "";
            textBox4.Text = selectedItem.getZhiry() + "";
            textBox1.Text = selectedItem.getUglevody() + "";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateProduct();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            updateProduct();
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            updateDataGridview();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                dataGridView1.Rows.RemoveAt(cell.RowIndex);
            }
        }
    }
}
