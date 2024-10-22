using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BJVOtoparkMuhasebe
{
    public partial class Form1 : Form
    {
        SqlConnection baglanti, SDbaglanti;
        string connetionString;
        public Form1()
        {
            InitializeComponent();
        }

        void ClearAllText(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Clear();
                else
                    ClearAllText(c);
            }
            




        }

        private void DBConnect()
        {

            StreamReader oku = new StreamReader(@"data\Connection_DB.dat");
            connetionString = oku.ReadLine();
            baglanti = new SqlConnection(connetionString);
            baglanti.Open();
            MessageBox.Show("Connection Open  !");
            baglanti.Close();
        }

        private void SD_Connect()
        {
            StreamReader oku = new StreamReader(@"data\SD_DB.dat");
            connetionString = oku.ReadLine();
            baglanti = new SqlConnection(connetionString);
            baglanti.Open();
            MessageBox.Show("SKIDATA Connection Open  !");
            baglanti.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {

                DBConnect();
                SD_Connect();
            }
            catch (Exception ex)
            { 
                Console.WriteLine("Veri Tabanı Bağlantı Hatası!"); 
            }

        }

        private void btnIhGHasilat_Click(object sender, EventArgs e)
        {
            this.Hide();
            ihGenelHasilatFrm frmihGenelHasilat = new ihGenelHasilatFrm();
            frmihGenelHasilat.ShowDialog();
            frmihGenelHasilat = null;
            this.Show();
        }
    }
}
