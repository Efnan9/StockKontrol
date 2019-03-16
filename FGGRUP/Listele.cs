using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Microsoft.VisualBasic;
using System.Drawing.Printing;

namespace FGGRUP
{
    public partial class Listele : Form
    {
        SqlConnection bagla = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=FGGRUP;Integrated Security=True");
        public Listele()
        {

            InitializeComponent();
            bagla.Open();
            SqlCommand komut = new SqlCommand("Select * from Ürünler Order by convert(datetime, VerildiğiTarih, 103) DESC", bagla);
            SqlDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                ListViewItem list = new ListViewItem();
                list.Text = oku["ÜrünKod"].ToString();
                list.SubItems.Add(oku["ÜrünAd"].ToString());
                list.SubItems.Add(oku["Nerede"].ToString());
                list.SubItems.Add(oku["VerildiğiTarih"].ToString());
                list.SubItems.Add(oku["AlındığıTarih"].ToString());
                list.SubItems.Add(oku["Açıklama"].ToString());

                listView1.Items.Add(list);
            }
            bagla.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("C:\\UltrasonListe.txt"))
            {
                if (listView1.Items.Count > 0) // listview boş değil ise 
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (ColumnHeader baslik in listView1.Columns) // columns
                    {
                        sb.Append(string.Format("{0,6}   \t", baslik.Text));
                    }
                    sw.WriteLine(sb.ToString());
                    foreach (ListViewItem lvi in listView1.Items)
                    {
                        sb = new StringBuilder();
                        foreach (ListViewItem.ListViewSubItem listViewSubItem in lvi.SubItems)
                        {
                            sb.Append(string.Format("{0,6}   \t", listViewSubItem.Text));
                        }
                        sw.WriteLine(sb.ToString());
                    }
                    sw.WriteLine();
                    MessageBox.Show("Kaydetme İşlemi Tamamlandı(UltrasonListe.txt)!!", "Bilgilendirme Penceresi");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            bagla.Open();
            if (textBox1.Text.ToString() != "")
            {
                string komut = "Select *from Ürünler Where ÜrünKod='" + textBox1.Text.ToString() + "'";
                SqlCommand kom = new SqlCommand(komut, bagla);
                SqlDataReader oku = kom.ExecuteReader();
                while (oku.Read())
                {
                    ListViewItem list1 = new ListViewItem();
                    list1.Text = oku["ÜrünKod"].ToString();
                    list1.SubItems.Add(oku["ÜrünAd"].ToString());
                    list1.SubItems.Add(oku["Nerede"].ToString());
                    list1.SubItems.Add(oku["VerildiğiTarih"].ToString());
                    list1.SubItems.Add(oku["AlındığıTarih"].ToString());
                    list1.SubItems.Add(oku["Açıklama"].ToString());
                    
                    listView1.Items.Add(list1);
           
                }

            }
            bagla.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                MessageBox.Show(listView1.SelectedItems[0].SubItems[5].Text);
            
        }
 

  
        

   
    }
}
