using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;//SQL KÜTÜPHANESİ EKLENDİ
using Microsoft.VisualBasic;  //INPUTBOX İÇİN

namespace FGGRUP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection bagla = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=FGGRUP;Integrated Security=True");
        private void button1_Click(object sender, EventArgs e)
        {
            Listele goster = new Listele();
            goster.ShowDialog();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            bagla.Open();
            if (textBox2.Text.ToString() == "")
            {
                MessageBox.Show("Kayıt İşlemi Yapılamadı Boş!", "Bilgilendirme Penceresi");
                bagla.Close();
            }
            else
            {
                SqlCommand komut = new SqlCommand("insert into Ürünler(ÜrünKod,ÜrünAd,Nerede,VerildiğiTarih,AlındığıTarih,Açıklama) values ('" + textBox1.Text.ToString() + "','" + textBox2.Text.ToString() + "','" + textBox3.Text.ToString() + "','" + dateTimePicker1.Value.ToShortDateString() + "','" + dateTimePicker2.Value.ToShortDateString() + "','" + textBox4.Text.ToString() + "')", bagla);
                komut.ExecuteNonQuery();
                MessageBox.Show("Kayıt İşlemi Tamamlandı!", "Bilgilendirme Penceresi");
                bagla.Close();
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            bagla.Open();
            string text = Interaction.InputBox("Ürünün Nerede Olduğunu Giriniz.", "Silme İşlemi", "", 350, 350);
            if (text != string.Empty)
            {
                string komut = "Delete from Ürünler Where Nerede='" + text + "'";
                SqlCommand kom = new SqlCommand(komut, bagla);
                kom.ExecuteNonQuery();
                
                MessageBox.Show("Silme İşlemi Tamamlandı!", "Bilgilendirme Penceresi");
            }
            bagla.Close();
            
        }   
        private void button4_Click(object sender, EventArgs e)
        {
            bagla.Open();
            string nerede = Interaction.InputBox("Ürünün Nerede Olduğunu Giriniz", "Güncelleme İşlemi", "", 350, 350);
            if (nerede != string.Empty)
            {
                string acıklama = Interaction.InputBox("Açıklama Ekleyiniz", "Güncelleme İşlemi", "", 350, 350);
                string komut = "update Ürünler set Açıklama=@acıklama where Nerede=@nerede";
                SqlCommand kom = new SqlCommand(komut, bagla);
                kom.Parameters.AddWithValue("@acıklama", acıklama);
                kom.Parameters.AddWithValue("@nerede", nerede);
                kom.ExecuteNonQuery();
                MessageBox.Show("Güncelleme İşlemi Tamamlandı!", "Bilgilendirme Penceresi");
            }
            bagla.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
           
            bagla.Open();
            string kod = Interaction.InputBox("Ürün Kodu Giriniz", "Listeleme İşlemi", "", 350, 350);
            if (kod != "")
            {   
                int adet = listView1.Items.Count;
                if (adet != 0) { listView1.Items.Clear(); }
                string komut = "Select *from Ürünler Where ÜrünKod=@kod Order by convert(datetime, VerildiğiTarih, 103) DESC";
                SqlCommand kom = new SqlCommand(komut, bagla); 
                kom.Parameters.AddWithValue("@kod", kod);
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
        private void button8_Click(object sender, EventArgs e)
        {

            using (StreamWriter sw = new StreamWriter("C:\\UltrasonCihazı.txt"))
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
                    MessageBox.Show("Kaydetme İşlemi Tamamlandı(UltrasonCihazı.txt)!!", "Bilgilendirme Penceresi");
                }
            }
        }
        
        
        

        
        
        
        
        
        
        
        
        
        
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0) 
                MessageBox.Show(listView1.SelectedItems[0].SubItems[5].Text);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bagla.Open();
            string nerede = Interaction.InputBox("Ürünün nerede olduğunu giriniz", "Güncelleme İşlemi", "", 350, 350);
            if (nerede != string.Empty)
            {
                string tarih = Interaction.InputBox("Ürünün yeni alınacağı tarihi giriniz", "Güncelleme İşlemi", "", 350, 350);
                string komut = "update Ürünler set AlındığıTarih=@tarih where Nerede=@nerede";
                SqlCommand kom = new SqlCommand(komut, bagla);
                kom.Parameters.AddWithValue("@tarih", tarih);
                kom.Parameters.AddWithValue("@nerede", nerede);
                kom.ExecuteNonQuery();
                MessageBox.Show("Güncelleme İşlemi Tamamlandı!", "Bilgilendirme Penceresi");
            }
            bagla.Close();
        }
      

    } 
}
