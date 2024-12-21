﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; //1-MySql bağlantısı için
using MySql.Data; //1-MySql bağlantısı için
using MySql.Data.MySqlClient; //1-MySql bağlantısı için
using System.Data;//1-veritablosu için

namespace Kitap_Satış_Sistemi
{
    public partial class sipariş : Form
    {
        public sipariş()
        {
            InitializeComponent();
            this.MaximizeBox = false;  // Pencereyi büyütme butonunu devre dışı bırak
            this.MinimizeBox = false;  // Pencereyi küçültme butonunu devre dışı bırak
        }

        //2-VERİTABANI NESNELERİ
        MySqlCommand komut; //sql komutunu çalıştırmak için
        MySqlDataAdapter veritutucu; //tablo ile veri bağı kurar
        DataTable veritablosu = new DataTable(); //seçme sorgusu(select) ile gelen verilerin tutulacağı yer
        MySqlDataReader veriokuyucu;//bir tablodan gelen değerleri satır satır okumak için kullanılır(select)
        MySqlConnection bağlantı = new MySqlConnection("Server=localhost;" +
        "Database=veri;" +//veritabanı adı veri
        "Uid=root;" +//kullanıcı
        "Pwd='';" +//şifre
        "AllowUserVariables=True;" +
        "UseCompression=True");

        string tabloadı = "";//seçilen tabloadını tutar
     

        public void doldur(string sql)
        {
            
            //3-DİNAMİK DATAGRIDVIEW DOLDURMA
            veritablosu = new DataTable();

            bağlantı.Open();//1-bağlantı aç
            veritutucu = new MySqlDataAdapter(sql, bağlantı);//2-sql komutu çalıştır,tablodan gelen bilgiler veritutucu'da
            veritutucu.Fill(veritablosu);//3-veritutucudaki bilgiler veritablosu'na aktarıldı
            dataGridView1.DataSource = veritablosu;//4-tablodan gelen bilgiler dataGridView1'de gösteriliyor
            bağlantı.Close();//5-bağlantı kapat

            label10.Text = "Sisteme kayıtlı " + dataGridView1.RowCount + " adet "+tabloadı+" kaydı vardır.";
        }

        public void göster(int satırno)
        {
            //4-DATAGRIDVIEW TIKLANAN SATIRI GÖSTERME
            if (satırno > dataGridView1.RowCount - 2 || satırno < 0)//satır numaralıarı sınırlar içinde mi?
                return;

            switch (tabloadı)
            {
                case "müşteri":
                    textBox2.Text = dataGridView1.Rows[satırno].Cells[0].ToString();//müşteriid
                    break;
                case "sepet":
                    textBox3.Text = dataGridView1.Rows[satırno].Cells[0].ToString();//sepetid
                    break;

                default://sipariş seçildi
                    textBox1.Text = dataGridView1.Rows[satırno].Cells[0].Value.ToString();//siparişid
                    textBox2.Text = dataGridView1.Rows[satırno].Cells[1].Value.ToString();//müşteriid
                    textBox3.Text = dataGridView1.Rows[satırno].Cells[2].Value.ToString();//sepetid
                    dateTimePicker1.Text = dataGridView1.Rows[satırno].Cells[3].Value.ToString();//tarih
                    dateTimePicker2.Text = dataGridView1.Rows[satırno].Cells[4].Value.ToString();//saat
                    textBox4.Text = dataGridView1.Rows[satırno].Cells[5].Value.ToString();//tutar
                    comboBox1.Text = dataGridView1.Rows[satırno].Cells[6].Value.ToString();//kargadı
                    textBox5.Text = dataGridView1.Rows[satırno].Cells[7].Value.ToString();//kargono
                    textBox6.Text = dataGridView1.Rows[satırno].Cells[8].Value.ToString();//durum
                    break;
            }
  
        }

        private void sipariş_Load(object sender, EventArgs e)
        {
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabloadı = "sipariş";
            doldur("select * from sipariş");//sipariş tablosundaki tüm kayıtlar dataGridView1'de gösteriliyor
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tabloadı = "müşteri";
            doldur("select * from müşteri");//müşteri tablosundaki tüm kayıtlar dataGridView1'de gösteriliyor

        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabloadı = "sepet";
            doldur("select * from sepet");//sepet tablosundaki tüm kayıtlar dataGridView1'de gösteriliyor
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > dataGridView1.RowCount - 2 || e.RowIndex < 0)//satır numaraları sınırlar içinde mi?
                return;
            göster(e.RowIndex);//tıklanan satır göster
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > dataGridView1.RowCount - 2 || e.RowIndex < 0)//satır numaraları sınırlar içinde mi?
                return;
            göster(e.RowIndex);//tıklanan satır göster
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > dataGridView1.RowCount - 2 || e.RowIndex < 0)//satır numaraları sınırlar içinde mi?
                return;
            göster(e.RowIndex);//tıklanan satır göster
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //VERİ GÖSTERME İŞLEMİ
            string sql = "select * from sipariş where siparişid=" + textBox1.Text;
            bağlantı.Open();//1-bağlantı aç
            komut=new MySqlCommand(sql,bağlantı);//2-komut tanımlama
            veriokuyucu=komut.ExecuteReader();//3-komut çalıştırma/tablodan gelen bilgiler veriokuyucu'da
            if(veriokuyucu.Read())//böyle bir kayıt var mı?
            {
                textBox1.Text=veriokuyucu.GetValue(0).ToString();//siparişid
                textBox2.Text = veriokuyucu.GetValue(1).ToString();//müşteriid
                textBox3.Text = veriokuyucu.GetValue(2).ToString();//sepetid
                dateTimePicker1.Text=veriokuyucu.GetValue(3).ToString();//tarih
                dateTimePicker2.Text = veriokuyucu.GetValue(4).ToString();//saat
                textBox4.Text = veriokuyucu.GetValue(5).ToString();//tutar
                comboBox1.Text=veriokuyucu.GetValue(6).ToString();//kargoadı
                textBox5.Text=veriokuyucu.GetValue(7).ToString();//kargono
                textBox6.Text = veriokuyucu.GetValue(8).ToString();//durum
            }
            bağlantı.Close();//4-bağlantı kapat

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //VERİ EKLEME İŞLEMİ
            string sql = "insert into sipariş(müşteriid,sepetid,tarih,saat,tutar,kargoadı,kargono,durum)values(";
            sql += textBox2.Text + ",";//müşteriid
            sql +="'" +textBox3.Text + "',";//sepetid
            sql += "'" + dateTimePicker1.Text + "',";//tarih
            sql += "'" + dateTimePicker2.Text + "',";//tarih
            sql += textBox4.Text + ",";//tutar
            sql += "'" + comboBox1.Text + "',";//kargoadı
            sql += "'" + textBox5.Text + "',";//kargono
            sql += "'" + textBox6.Text + "')";//durum

            bağlantı.Open();//1-bağlantı aç
            komut=new MySqlCommand(sql,bağlantı);//2-komut tanımlama
            komut.ExecuteNonQuery();//3-komut çalıştırma

            //LOG TABLOSUNA İŞLEM KAYDETME
            sql = "insert into log(kullanıcıid,kullanıcıtürü,işlemtürü,tarih,saat,açıklama)values(";
            sql += bilgi.kullanıcıid + ",";//kullanıcıid
            sql += "'" + bilgi.kullanıcıtürü + "',";//kullanıcıtürü
            sql += "'Kayıt Ekleme',";//işlemtürü
            sql += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";//tarih
            sql += "'" + DateTime.Now.ToString("HH:mm:ss") + "',";//saat
            sql += "'" + bilgi.kullanıcıid +
            " IDli " + bilgi.kullanıcıtürü + " " +
            textBox2.Text + "IDli " +
            textBox3.Text + " adlı sipariş  sisteme ekledi.')";//açıklama
            komut = new MySqlCommand(sql, bağlantı);//2-komut tanımlama
            komut.ExecuteNonQuery();//3-komut çalıştırma




            bağlantı.Close();//4-bağlantı kapat

            tabloadı = "sipariş";
            doldur("select * from sipariş");//sipariş tablosundaki tüm kayıtlar dataGridView1'de gösteriliyor

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //VERİ GÜNCELLEME İŞLEMİ
            string sql = "update sipariş set ";
            sql += "müşteriid=" +textBox2.Text + ",";//müşteriid
            sql += "sepetid='" + textBox3.Text + "',";//sepetid
            sql += "tarih='" + dateTimePicker1.Text + "',";//tarih
            sql += "saat='" + dateTimePicker2.Text + "',";//saat
            sql += "tutar="+textBox4.Text + ",";//tutar
            sql += "kargoadı='" + comboBox1.Text + "',";//kargoadı
            sql += "kargono='" + textBox5.Text + "',";//kargono
            sql += "durum='" + textBox6.Text + "' ";//durum
            sql += " where siparişid=" + textBox1.Text;//güncelleme kriteri siparişid

            bağlantı.Open();//1-bağlantı aç
            komut = new MySqlCommand(sql, bağlantı);//2-komut tanımlama
            komut.ExecuteNonQuery();//3-komut çalıştırma

            //LOG TABLOSUNA İŞLEM KAYDETME
            sql = "insert into log(kullanıcıid,kullanıcıtürü,işlemtürü,tarih,saat,açıklama)values(";
            sql += bilgi.kullanıcıid + ",";//kullanıcıid
            sql += "'" + bilgi.kullanıcıtürü + "',";//kullanıcıtürü
            sql += "'Kayıt Güncelleme',";//işlemtürü
            sql += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";//tarih
            sql += "'" + DateTime.Now.ToString("HH:mm:ss") + "',";//saat
            sql += "'" + bilgi.kullanıcıid +
            " IDli " + bilgi.kullanıcıtürü + " IDsi " +
            textBox1.Text + " olan Sipariş bilgileri güncelledi.')";//açıklama
            komut = new MySqlCommand(sql, bağlantı);//2-komut tanımlama
            komut.ExecuteNonQuery();//3-komut çalıştırma



            bağlantı.Close();//4-bağlantı kapat

            tabloadı = "sipariş";
            doldur("select * from sipariş");//sipariş tablosundaki tüm kayıtlar dataGridView1'de gösteriliyor
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //VERİ SİLME İŞLEMİ
            string sql = "delete * from sipariş where siparişid=" + textBox1.Text;//silme kriteri siparişid

            bağlantı.Open();//1-bağlantı aç
            komut = new MySqlCommand(sql, bağlantı);//2-komut tanımlama
            komut .ExecuteNonQuery();//3-komut çalıştırma


            bağlantı.Close();//4-bağlantı kapat

            //LOG TABLOSUNA İŞLEM KAYDETME
            sql = "insert into log(kullanıcıid,kullanıcıtürü,işlemtürü,tarih,saat,açıklama)values(";
            sql += bilgi.kullanıcıid + ",";//kullanıcıid
            sql += "'" + bilgi.kullanıcıtürü + "',";//kullanıcıtürü
            sql += "'Kayıt Silme',";//işlemtürü
            sql += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";//tarih
            sql += "'" + DateTime.Now.ToString("HH:mm:ss") + "',";//saat
            sql += "'" + bilgi.kullanıcıid +
            " IDli " + bilgi.kullanıcıtürü + " IDsi " +
            textBox1.Text + " olan spariş ipatl etti.')";//açıklama
            komut = new MySqlCommand(sql, bağlantı);//2-komut tanımlama
            komut.ExecuteNonQuery();//3-komut çalıştırma
            tabloadı = "sipariş";
            doldur("select * from sipariş");//sipariş tablosundaki tüm kayıtlar dataGridView1'de gösteriliyor

        }

        private void button8_Click(object sender, EventArgs e)
        {

            anamenü frmanenü = new anamenü();
            frmanenü.Show();
            this.Hide();
        }
    }
}
