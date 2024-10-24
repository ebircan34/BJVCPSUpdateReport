using System;
using System.Collections;
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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BJVOtoparkMuhasebe
{
    public partial class ihGenelHasilatFrm : Form
    {

        SqlConnection baglanti, SDbaglanti;
        string connetionString,SDconnectionString;
        int SimdikiWidth = 0;
        int SimdikiHeight = 0;
        public int ekran_x;
        public int ekran_y;
        public bool txtKontol=false;
        public decimal genelToplam =0;
        public decimal MParkCksToplam, MParkCks2Toplam, NFAT, KFAT, EFAT, AboneHariciGelirToplam;
        public ihGenelHasilatFrm()
        {
            InitializeComponent();
        }

        private void ekrancozbul()
        {

            ekran_x = Screen.GetBounds(new Point(0, 0)).Width;
            ekran_y = Screen.GetBounds(new Point(0, 0)).Height;
        }

        private void DBConnect()
        {

            StreamReader oku = new StreamReader(@"data\Connection_DB.dat");
            connetionString = oku.ReadLine();
            baglanti = new SqlConnection(connetionString);
            baglanti.Open();
            //MessageBox.Show("Connection Open  !");
            baglanti.Close();
        }


        private void SD_Connect()
        {
            StreamReader oku = new StreamReader(@"data\SD_DB.dat");
            SDconnectionString = oku.ReadLine();
            SDbaglanti = new SqlConnection(SDconnectionString);
            SDbaglanti.Open();
            //MessageBox.Show("SKIDATA Connection Open  !");
            SDbaglanti.Close();
        }

        private void ihGenelHasilat_Load(object sender, EventArgs e)
        {
            txtMrkzOAdet.Focus();
            DBConnect();    
            ekrancozbul();
            ClearAllText(this);
            SD_Connect();
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            btnYeni.Enabled = true;
            btnKaydet.Enabled = false;
            btniptal.Enabled = true ;
            tableLayoutPanel2.Enabled = false;
            txtId.Text = "0";
            comboBoxP.Items.Clear();
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("Select * from BjvPersonel", baglanti);
            SqlDataReader dr_validasyon = cmd.ExecuteReader();

            while (dr_validasyon.Read())
            {
                comboBoxP.Items.Add(dr_validasyon["PerAdSoyad"]);

            }
            baglanti.Close();
            dr_validasyon.Close();
            comboBoxP.SelectedIndex = 0;
        }

        //private void txtMerkezKasaAdet_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter) {txtMrkzOGelir.Focus();}  
        //}
        

        private void txtNakitFaturaGelir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) 
            {

                
                txtKrediKartiFaturaAdet.Focus();}
              }

        private void txtIhKrediKartiFaturaAdet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {txtKrediKartiFaturaGelir.Focus();}
        }

        private void txtIhKrediKartiFaturaGelir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) 
            {

                txtKrediKartiFaturaGelir.Text = string.Format("{0:c}", txtKrediKartiFaturaGelir.Text);



                txtEFTFaturaAdet.Focus(); } 
        }

        private void txtIhEFTFaturaAdet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { txtEFTFaturaGelir.Focus(); }
        }

       

        private void txtKsbKrediKartiAdet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { txtKsbKrediKartiGelir.Focus(); }
        }

        private void txtKsbKrediKartiGelir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {txtAboneKrediKartiAdet.Focus();}
        }

        private void txtAboneKrediKartiAdet_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter) {
                int Adet = 0;
                //int KKAboneAdet = 0;
            //int KKCongAdet = 0;
            string query = "Select count(*) From Gelirler Where BaslangicTarihi='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Otopark='IC HAT 1 OTOPARK' and  OdemeYontemi='KREDI KARTI' and NOT Status='ÖZEL SATIŞ'";
            baglanti.Open();
            SqlCommand cmd = new SqlCommand(query, baglanti);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr[0].ToString() == string.Empty) { Adet = 0; }
                else
                {
                        Adet = Convert.ToInt32(dr[0]);
                }
            }
            baglanti.Close();
            
            dr.Close();
            txtAboneKrediKartiAdet.Text = Convert.ToString(Adet);
            txtAboneKrediKartiGelir.Focus();
            }

        }

        private void txtAboneKrediKartiGelir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {

                decimal KKAbonePara = 0;
                decimal KKCongPara = 0;
                string query = "Select SUM(GenelToplam) From Gelirler Where BaslangicTarihi='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Otopark='IC HAT 1 OTOPARK' and Status='CONGRESS' and OdemeYontemi='KREDI KARTI'";
                baglanti.Open();
                SqlCommand cmd = new SqlCommand(query, baglanti);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr[0].ToString() == string.Empty) { KKCongPara = 0; }
                    else
                    {
                        KKCongPara = Convert.ToDecimal(dr[0]);
                    }
                }
                baglanti.Close();

                dr.Close();

                query = "Select count(*) From Gelirler Where BaslangicTarihi='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Otopark='IC HAT 1 OTOPARK' and Status='ABONE' and OdemeYontemi='KREDI KARTI'";
                baglanti.Open();
                cmd = new SqlCommand(query, baglanti);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr[0].ToString() == string.Empty) { KKAbonePara = 0; }
                    else
                    {
                        KKAbonePara = Convert.ToDecimal(dr[0]);

                    }
                }
                baglanti.Close();
                dr.Close();

                txtAboneKrediKartiGelir.Text = Convert.ToString(KKAbonePara + KKCongPara);
                txtAboneKrediKartiGelir.Text = string.Format("{0:C}", txtAboneKrediKartiGelir.Text);
                btnBulGetir.Focus();
                btnBulGetir.BackColor = Color.Gold;
            }


        }

        private void txtGenelToplamAdet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {txtGenelToplam.Focus();}
        }

        private void txtGenelToplam_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { txtAboneHariciGelirAd.Focus();}
        }

        private void txtAboneHariciGelirAd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {txtAboneHariciGelir.Focus(); }    
        }

        

        

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnBulGetir_Click(object sender, EventArgs e)
        {
            //genelToplam = 0;
            //MParkCksToplam = 0; MParkCks2Toplam=0; NFAT = 0; KFAT = 0; EFAT = 0; AboneHariciGelirToplam=0;
            //decimal congressAbone = 0;
            //int ToplamAdet = 0;
            //int AbonelikHariciGelirAdet = 0;
            
            //try
            //{
            //    MParkCksToplam = decimal.Parse(txtMrkzOGelir.Text);
            //    MParkCks2Toplam = decimal.Parse(txtIcCks2OGelir.Text);
            //    NFAT = decimal.Parse(txtNakitFaturaGelir.Text);
            //    KFAT = decimal.Parse(txtKrediKartiFaturaGelir.Text);
            //    EFAT = decimal.Parse(txtEFTFaturaGelir.Text);
            //    genelToplam = MParkCksToplam + MParkCks2Toplam + NFAT + EFAT + KFAT;
            //    txtGenelToplam.Text=genelToplam.ToString("C"); 
            //    AboneHariciGelirToplam = genelToplam-(decimal.Parse(txtAboneOCongGelir.Text)+decimal.Parse(txtIcWksAboneGelir.Text)+decimal.Parse(txtRentCarMbCongGelir.Text)+decimal.Parse(txtRentCarMbGelir.Text)+NFAT+KFAT+EFAT);
            //    ToplamAdet = (int.Parse(txtMrkzOAdet.Text)+int.Parse(txtIcCks2OAdet.Text)+int.Parse(txtNakitFaturaAdet.Text)+int.Parse(txtKrediKartiFaturaAdet.Text)+int.Parse(txtEFTFaturaAdet.Text));
            //    txtGenelToplamAdet.Text=ToplamAdet.ToString();
            //    AbonelikHariciGelirAdet = (ToplamAdet-(int.Parse(txtAboneOCongAdet.Text)+int.Parse(txtIcWksAboneAdet.Text)+int.Parse(txtRentCarMbAdet.Text)+int.Parse(txtRentCarMbCongAdet.Text)+int.Parse(txtNakitFaturaAdet.Text)+int.Parse(txtKrediKartiFaturaAdet.Text)+int.Parse(txtEFTFaturaAdet.Text)));
            //    txtAboneHariciGelirAd.Text= AbonelikHariciGelirAdet.ToString();   
            //    txtAboneHariciGelir.Text= AboneHariciGelirToplam.ToString("c"); 
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Matematiksel İşlem Hatası! Çözümü için Boş Alan Kontrolü Yapınız!");
            //}



        }

        

       

        private void txtAboneOCongGelir_Leave(object sender, EventArgs e)
        {
            //double para;

            //if (txtAboneOCongGelir.Text == string.Empty) { }
            //else
            //{
            //    para = double.Parse(txtAboneOCongGelir.Text);
            //    //    //para = Math.Round(para, 2);
            //    //txtMerkezKasaGelir.Text= String.Format("{0:N}”,10000);
            //    //txtMerkezKasaGelir.Text = String.Format("{0:0.0,0}", para);
            //    txtAboneOCongGelir.Text = para.ToString("N");


            //}
        }

           
       

        private void txtKsbKrediKartiGelir_Leave(object sender, EventArgs e)
        {
            double para;

            if (txtKsbKrediKartiGelir.Text == string.Empty) { }
            else
            {
                para = double.Parse(txtKsbKrediKartiGelir.Text);
                //    //para = Math.Round(para, 2);
                //txtMerkezKasaGelir.Text= String.Format("{0:N}”,10000);
                //txtMerkezKasaGelir.Text = String.Format("{0:0.0,0}", para);
                txtKsbKrediKartiGelir.Text = para.ToString("N");


            }

        }

        

        private void txtGenelToplam_Leave(object sender, EventArgs e)
        {
            double para;

            if (txtGenelToplam.Text == string.Empty) { }
            else
            {
                para = double.Parse(txtGenelToplam.Text);
                //    //para = Math.Round(para, 2);
                //txtMerkezKasaGelir.Text= String.Format("{0:N}”,10000);
                //txtMerkezKasaGelir.Text = String.Format("{0:0.0,0}", para);
                txtGenelToplam.Text = para.ToString("N");


            }
        }

        private void txtAboneHariciGelir_Leave(object sender, EventArgs e)
        {
            double para;

            if (txtAboneHariciGelir.Text == string.Empty) { }
            else
            {
                para = double.Parse(txtAboneHariciGelir.Text);
                //    //para = Math.Round(para, 2);
                //txtMerkezKasaGelir.Text= String.Format("{0:N}”,10000);
                //txtMerkezKasaGelir.Text = String.Format("{0:0.0,0}", para);
                txtAboneHariciGelir.Text = para.ToString("N");


            }
        }

        private void btnBulGetir_Click_1(object sender, EventArgs e)
        {
            //genelToplam = 0;
            //MParkCksToplam = 0; MParkCks2Toplam = 0; NFAT = 0; KFAT = 0; EFAT = 0; AboneHariciGelirToplam = 0;
            //decimal congressAbone = 0;
            //int ToplamAdet = 0;
            //int AbonelikHariciGelirAdet = 0;

            //try
            //{
                
            //    MParkCksToplam = decimal.Parse(txtMrkzOGelir.Text);
            //    MParkCks2Toplam = decimal.Parse(txtIcCks2OGelir.Text);
            //    NFAT = decimal.Parse(txtNakitFaturaGelir.Text);
            //    KFAT = decimal.Parse(txtKrediKartiFaturaGelir.Text);
            //    EFAT = decimal.Parse(txtEFTFaturaGelir.Text);
            //    genelToplam = MParkCksToplam + MParkCks2Toplam + NFAT + EFAT + KFAT;
            //    txtGenelToplam.Text = genelToplam.ToString("N");
            //    AboneHariciGelirToplam = genelToplam - (decimal.Parse(txtAboneOCongGelir.Text) + decimal.Parse(txtIcWksAboneGelir.Text) + decimal.Parse(txtRentCarMbCongGelir.Text) + decimal.Parse(txtRentCarMbGelir.Text) + NFAT + KFAT + EFAT);
            //    ToplamAdet = (int.Parse(txtMrkzOAdet.Text) + int.Parse(txtIcCks2OAdet.Text) + int.Parse(txtNakitFaturaAdet.Text) + int.Parse(txtKrediKartiFaturaAdet.Text) + int.Parse(txtEFTFaturaAdet.Text));
            //    txtGenelToplamAdet.Text = ToplamAdet.ToString();
            //    AbonelikHariciGelirAdet = (ToplamAdet - (int.Parse(txtAboneOCongAdet.Text) + int.Parse(txtIcWksAboneAdet.Text) + int.Parse(txtRentCarMbAdet.Text) + int.Parse(txtRentCarMbCongAdet.Text) + int.Parse(txtNakitFaturaAdet.Text) + int.Parse(txtKrediKartiFaturaAdet.Text) + int.Parse(txtEFTFaturaAdet.Text)));
            //    txtAboneHariciGelirAd.Text = AbonelikHariciGelirAdet.ToString();
            //    txtAboneHariciGelir.Text = AboneHariciGelirToplam.ToString("N");
                


            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Matematiksel İşlem Hatası! Çözümü için Boş Alan Kontrolü Yapınız!");
            //}
        }

        private void TumListe()
        {
            string sql = "";
            try
            {
                var fileStream = new FileStream(@"data\sql2.dat", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    sql = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                sql = "Select Id,Tarih,Personel,Otopark,IcMrkzOAdet,IcMrkzOGelir,IcAboneOAboneAdet,IcAboneOAboneGelir,IcAboneOCongAdet,IcAboneOCongGelir,IcCks2OAdet,IcCks2OGelir,RentCarMbCongAdet,RentCarMbCongGelir,RentCarMbAboneAdet,RentCarMbAboneGelir,IhNakitFaturaAdet,IhNakitFatuıraGelir,IhKrediKartiFaturaAdet,IhKrediKartiFaturaGelir,IhEFTFaturaAdet,IhEFTFaturaGelir,KsbKrediKartiAdet,KsbKrediKartiGelir,AboneKrediKartiAdet,AboneKrediKartiGelir,GenelToplamAdet,GenelToplam,AboneHariciGelirAd,AboneHariciGelir FROM iHGenelHasilat";
                baglanti.Open();
                SqlCommand cmd = new SqlCommand(sql, baglanti);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                baglanti.Close();
                //baglanti.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

        }

        private void btnListele_Click(object sender, EventArgs e)
        {
            
            string sql = "";
            try
            {
                var fileStream = new FileStream(@"data\sql2.dat", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    sql = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);    
            }
            try
            {
                sql = "Select Id,Tarih,Personel,Otopark,IcMrkzOAdet,IcMrkzOGelir,IcAboneOAboneAdet,IcAboneOAboneGelir,IcAboneOCongAdet,IcAboneOCongGelir,IcCks2OAdet,IcCks2OGelir,RentCarMbCongAdet,RentCarMbCongGelir,RentCarMbAboneAdet,RentCarMbAboneGelir,IhNakitFaturaAdet,IhNakitFatuıraGelir,IhKrediKartiFaturaAdet,IhKrediKartiFaturaGelir,IhEFTFaturaAdet,IhEFTFaturaGelir,KsbKrediKartiAdet,KsbKrediKartiGelir,AboneKrediKartiAdet,AboneKrediKartiGelir,GenelToplamAdet,GenelToplam,AboneHariciGelirAd,AboneHariciGelir FROM iHGenelHasilat Where Tarih>='"+dateTimePicker2.Value.ToString("yyyy-MM-dd")+"'";
                baglanti.Open();
                SqlCommand cmd = new SqlCommand(sql, baglanti);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                baglanti.Close();
               // baglanti.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

        }

        private void btnTumListe_Click(object sender, EventArgs e)
        {
            TumListe();
        }

        private void SartliListe()
            {
            string sql = "";
            try
            {
                var fileStream = new FileStream(@"data\sql2.dat", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    sql = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                sql = "Select Id,Tarih,Personel,Otopark,IcMrkzOAdet,IcMrkzOGelir,IcAboneOAboneAdet,IcAboneOAboneGelir,IcAboneOCongAdet,IcAboneOCongGelir,IcCks2OAdet,IcCks2OGelir,RentCarMbCongAdet,RentCarMbCongGelir,RentCarMbAboneAdet,RentCarMbAboneGelir,IhNakitFaturaAdet,IhNakitFatuıraGelir,IhKrediKartiFaturaAdet,IhKrediKartiFaturaGelir,IhEFTFaturaAdet,IhEFTFaturaGelir,KsbKrediKartiAdet,KsbKrediKartiGelir,AboneKrediKartiAdet,AboneKrediKartiGelir,GenelToplamAdet,GenelToplam,AboneHariciGelirAd,AboneHariciGelir FROM iHGenelHasilat where Tarih>='"+dateTimePicker2.Value.ToString("yyyy-MM-dd")+"'";
                baglanti.Open();
                SqlCommand cmd = new SqlCommand(sql, baglanti);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                baglanti.Close();
                //baglanti.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (txtId.Text == "0")
            {
                MessageBox.Show("Kayıt Silinemedi!", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {


                if (dataGridView1.Rows.Count > 0)
                {
                    string sql = "DELETE FROM iHGenelHasilat Where Id=@Id";

                    if (MessageBox.Show("Silme işlemini onaylıyor musunuz?", "Onay Verin", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        baglanti.Open();
                        SqlCommand cmd = new SqlCommand(sql, baglanti);
                        cmd.Parameters.AddWithValue("@Id", txtId.Text);
                        cmd.ExecuteNonQuery();
                        baglanti.Close();
                        //baglanti.Dispose() ;
                        MessageBox.Show("Kayıt Silindi!", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SartliListe();
                        ClearAllText(this);
                        txtId.Text = "0";

                    }
                    else
                    {
                        MessageBox.Show("Silme işlemi tarafınızca iptal edilmiştir.", "Kayıt Silme İptal", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }


                }
                else
                {
                    MessageBox.Show("Silinecek Kayıt Bulunamadı", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtId.Text = "0";
                }
            }

        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           txtId.Text= dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }

        private void txtAboneHariciGelir_KeyDown(object sender, KeyEventArgs e)
        {
           // if (e.KeyCode == Keys.Enter) { btnBulGetir.Focus()}
        }

        private void txtMrkzOAdet_KeyDown(object sender, KeyEventArgs e)
        {
           // SD_Connect();
            if (e.KeyCode == Keys.Enter)
            {
                int Adet = 0;
                DateTime dateTime;
                dateTime= dateTimePicker1.Value.AddDays(1);
                string query = "Select Count(*) from  RevenueParkingTransSales where Time>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Time<='" + dateTime.ToString("yyyy-MM-dd") + "' and DeviceDesig='IC MERKEZ ODEME'";
                SDbaglanti.Open();
                SqlCommand cmd= new SqlCommand(query,SDbaglanti);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                 if (dr[0] == string.Empty) { txtMrkzOAdet.Text = "0";}
                 else
                    {
                        Adet = Convert.ToInt32(dr[0]);
                    }
                }
                txtMrkzOAdet.Text=Adet.ToString();

                SDbaglanti.Close();

                txtMrkzOGelir.Focus();
            }
        }

        private void txtMrkzOGelir_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void txtMrkzOGelir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                decimal Gelir = 0;
                DateTime dateTime;
                dateTime = dateTimePicker1.Value.AddDays(1);
                string query = "Select sum(Revenue) from  RevenueParkingTransSales where Time>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Time<='" + dateTime.ToString("yyyy-MM-dd") + "' and DeviceDesig='IC MERKEZ ODEME'";
                SDbaglanti.Open();
                SqlCommand cmd = new SqlCommand(query, SDbaglanti);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr[0] == string.Empty) { txtMrkzOGelir.Text = "0"; }
                    else
                    {
                        Gelir = Convert.ToInt32(dr[0]);
                    }
                }
                txtMrkzOGelir.Text = Gelir.ToString("C");

                SDbaglanti.Close();

                txtIcWksAboneAdet.Focus();  
            }
        }

        private void txtIcWksAboneAdet_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    int Adet = 0;
                    DateTime dateTime;
                    dateTime = dateTimePicker1.Value.AddDays(1);
                    string query = "Select Count(*) from  RevenueParkingTransSales where Time>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Time<='" + dateTime.ToString("yyyy-MM-dd") + "' and DeviceDesig='IC ABONE WORKSTATION'";
                    SDbaglanti.Open();
                    SqlCommand cmd = new SqlCommand(query, SDbaglanti);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr[0] == string.Empty) { txtIcWksAboneAdet.Text = "0"; }
                        else
                        {
                            Adet = Convert.ToInt32(dr[0]);
                        }
                    }
                    txtIcWksAboneAdet.Text = Adet.ToString();

                    SDbaglanti.Close();
                    txtIcWksAboneGelir.Focus();

                }
            }
        }

        private void txtIcWksAboneGelir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode== Keys.Enter)
            {
                decimal Gelir = 0;
                DateTime dateTime;
                dateTime = dateTimePicker1.Value.AddDays(1);
                string query = "Select sum(Revenue) from  RevenueParkingTransSales where Time>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Time<='" + dateTime.ToString("yyyy-MM-dd") + "' and DeviceDesig='IC ABONE WORKSTATION'";
                SDbaglanti.Open();
                SqlCommand cmd = new SqlCommand(query, SDbaglanti);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr[0] == string.Empty) { txtIcWksAboneGelir.Text = "0"; }
                    else
                    {
                        Gelir = Convert.ToDecimal(dr[0]);
                    }
                }
                txtIcWksAboneGelir.Text = string.Format("{0:c}",Gelir); 

                SDbaglanti.Close();
                txtIcCks2OAdet.Focus();
            }
            
        }

        private void txtIcCks2OAdet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {


                int Adet = 0;
                DateTime dateTime;
                dateTime = dateTimePicker1.Value.AddDays(1);
                string query = "Select Count(*) from  RevenueParkingTransSales where Time>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Time<='" + dateTime.ToString("yyyy-MM-dd") + "' and DeviceDesig='IC CKS 2 ODEME'";
                SDbaglanti.Open();
                SqlCommand cmd = new SqlCommand(query, SDbaglanti);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr[0] == string.Empty) { txtIcCks2OAdet.Text = "0"; }
                    else
                    {
                        Adet = Convert.ToInt32(dr[0]);
                    }
                }
                txtIcCks2OAdet.Text = Adet.ToString();

                SDbaglanti.Close();
                txtIcCks2OGelir.Focus();
            }
        }

        private void txtIcCks2OGelir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                if (txtIcCks2OAdet.Text == "0")
                {
                 txtIcCks2OGelir.Text = string.Format("{0:c}",0);
                }
                else
                {
                    decimal Gelir = 0;
                    DateTime dateTime;
                    dateTime = dateTimePicker1.Value.AddDays(1);
                    string query = "Select sum(Revenue) from  RevenueParkingTransSales where Time>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Time<='" + dateTime.ToString("yyyy-MM-dd") + "' and DeviceDesig='IC CKS 2 ODEME'";
                    SDbaglanti.Open();
                    SqlCommand cmd = new SqlCommand(query, SDbaglanti);
                    SqlDataReader dr = cmd.ExecuteReader();
                    
                    txtIcCks2OGelir.Text = string.Format("{0:c}", Gelir);

                    SDbaglanti.Close();
                   
                }
                txtRentCarMbAdet.Focus();
            }
        }

        private void txtRentCarMbAdet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                
                txtRentCarMbGelir.Focus();
            }
        }

        private void txtRentCarMbGelir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtNakitFaturaAdet.Focus();
            }
        }

        private void txtNakitFaturaAdet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                
                txtNakitFaturaGelir.Focus();
            }
        }

        private void txtEFTFaturaGelir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtEFTFaturaGelir.Text = string.Format("{0:c}", txtEFTFaturaGelir.Text);
                txtAPM1Adet.Focus();
            }
        }

        private void txtAPM1Adet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int Adet = 0;
                DateTime dateTime;
                dateTime = dateTimePicker1.Value.AddDays(1);
                string query = "Select Count(*) from  RevenueParkingTransSales where Time>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Time<='" + dateTime.ToString("yyyy-MM-dd") + "' and DeviceDesig='IC HAT APM 01'";
                SDbaglanti.Open();
                SqlCommand cmd = new SqlCommand(query, SDbaglanti);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr[0] == string.Empty) { txtAPM1Adet.Text = "0"; }
                    else
                    {
                        Adet = Convert.ToInt32(dr[0]);
                    }
                }
                txtAPM1Adet.Text = Adet.ToString();

                SDbaglanti.Close();

                txtAPM1Gelir.Focus();
            }
        }

        private void txtAPM1Gelir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtAPM1Adet.Text == "0")
                {
                    txtAPM1Gelir.Text = "0";
                }
                else
                {
                    decimal Gelir = 0;
                    DateTime dateTime;
                    dateTime = dateTimePicker1.Value.AddDays(1);
                    string query = "Select sum(Revenue) from  RevenueParkingTransSales where Time>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Time<='" + dateTime.ToString("yyyy-MM-dd") + "' and DeviceDesig='IC HAT APM 01'";
                    SDbaglanti.Open();
                    SqlCommand cmd = new SqlCommand(query, SDbaglanti);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                        {
                        Gelir = Convert.ToDecimal(dr[0]);
                        }
                    
                    txtAPM1Gelir.Text = Gelir.ToString("N");
                    SDbaglanti.Close();
                }
                
                txtAPM2Adet.Focus();
            }

        }

        private void txtAPM2Adet_KeyDown(object sender, KeyEventArgs e)
        {
           if (e.KeyCode == Keys.Enter)
            {
                int Adet = 0;
                DateTime dateTime;
                dateTime = dateTimePicker1.Value.AddDays(1);
                string query = "Select Count(*) from  RevenueParkingTransSales where Time>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Time<='" + dateTime.ToString("yyyy-MM-dd") + "' and DeviceDesig='IC HAT APM 02'";
                SDbaglanti.Open();
                SqlCommand cmd = new SqlCommand(query, SDbaglanti);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr[0] == string.Empty) { txtAPM2Adet.Text = "0"; }
                    else
                    {
                        Adet = Convert.ToInt32(dr[0]);
                    }
                }
                txtAPM2Adet.Text = Adet.ToString();

                SDbaglanti.Close();
                txtAPM2Gelir.Focus();
            }
        }

        private void txtAPM2Gelir_KeyDown(object sender, KeyEventArgs e)
        {
           if (e.KeyCode == Keys.Enter)
            {
                if (txtAPM2Adet.Text == "0")
                {
                    txtAPM2Gelir.Text = "0";
                }
                else
                {
                    decimal Gelir = 0;
                    DateTime dateTime;
                    dateTime = dateTimePicker1.Value.AddDays(1);
                    string query = "Select sum(Revenue) from  RevenueParkingTransSales where Time>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Time<='" + dateTime.ToString("yyyy-MM-dd") + "' and DeviceDesig='IC HAT APM 02'";
                    SDbaglanti.Open();
                    SqlCommand cmd = new SqlCommand(query, SDbaglanti);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Gelir = Convert.ToDecimal(dr[0]);
                    }

                    txtAPM2Gelir.Text = Gelir.ToString("N");
                    SDbaglanti.Close();
                }
                txtAPMRentCarAdet.Focus();
            }
        }

        private void txtAPMRentCarAdet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int Adet = 0;
                DateTime dateTime;
                dateTime = dateTimePicker1.Value.AddDays(1);
                string query = "Select Count(*) from  RevenueParkingTransSales where Time>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Time<='" + dateTime.ToString("yyyy-MM-dd") + "' and DeviceDesig='RENT A CAR APM'";
                SDbaglanti.Open();
                SqlCommand cmd = new SqlCommand(query, SDbaglanti);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr[0] == string.Empty) { txtAPMRentCarAdet.Text = "0"; }
                    else
                    {
                        Adet = Convert.ToInt32(dr[0]);
                    }
                }
                txtAPMRentCarAdet.Text = Adet.ToString();

                SDbaglanti.Close();

                txtAPMRentCarGelir.Focus();
            }
        }

        private void txtAPMRentCarGelir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtAPMRentCarAdet.Text == "0")
                {
                    txtAPMRentCarGelir.Text = "0";
                }
                else
                {
                    decimal Gelir = 0;
                    DateTime dateTime;
                    dateTime = dateTimePicker1.Value.AddDays(1);
                    string query = "Select sum(Revenue) from  RevenueParkingTransSales where Time>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Time<='" + dateTime.ToString("yyyy-MM-dd") + "' and DeviceDesig='RENT A CAR APM'";
                    SDbaglanti.Open();
                    SqlCommand cmd = new SqlCommand(query, SDbaglanti);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Gelir = Convert.ToDecimal(dr[0]);
                    }

                    txtAPMRentCarGelir.Text = Gelir.ToString("N");
                    SDbaglanti.Close();
                }
                txtIcWksCongAdet.Focus();
            }
        }

        private void txtIcWksCongAdet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int Adet = 0;
                string query = "Select Count(*) From Gelirler Where BaslangicTarihi='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Status='CONGRESS' and Otopark='IC HAT 1 OTOPARK'" ;
                baglanti.Open();
                SqlCommand cmd = new SqlCommand(query,baglanti);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr[0].ToString() == string.Empty)
                    {
                        txtIcWksCongAdet.Text = "0";
                    }
                    else
                    {
                        //txtIcWksCongAdet.Text= string.Format("{0:c}",dr[0].ToString());
                        txtIcWksCongAdet.Text=dr[0].ToString();

                    }
                }

                baglanti.Close();
                dr.Close();
                
                
                txtIcWksCongGelir.Focus();
            }
        }

        private void txtNakitFaturaGelir_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) 
            {

                txtNakitFaturaGelir.Text = string.Format("{0:C}", txtNakitFaturaGelir.Text);
                txtKrediKartiFaturaAdet.Focus();
            }
        }

        private void txtIcWksCongGelir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtIcWksCongAdet.Text == string.Empty)
                {
                    txtIcWksCongGelir.Text = "0";
                }
                else
                {

                    decimal para = 0;
                    string query = "Select Sum(GenelToplam) From Gelirler Where BaslangicTarihi='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Status='CONGRESS' and Otopark='IC HAT 1 OTOPARK'";
                    baglanti.Open();
                    SqlCommand cmd = new SqlCommand(query, baglanti);
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr.IsDBNull(0)) { txtIcWksCongGelir.Text = "0"; }
                        else
                        {
                            para = Convert.ToDecimal(dr[0].ToString());
                            txtIcWksCongGelir.Text = string.Format("{0:c}", Convert.ToString(para));
                        }

                    }



                    baglanti.Close();
                }

                txtKsbKrediKartiAdet.Focus();
            }

        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            tableLayoutPanel2.Enabled = true;
            btnKaydet.Enabled = true;
            btnYeni.Enabled = false;
        }

        

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            //ControlAllText(this);
            //if (txtKontol == true)
            //{
            //    MessageBox.Show("Sarı Alanlar Boş Geçilemez!");
            //}
            //if (txtKontol == false)
            //{
            //    //try
            //    //{
            //        string text;
            //        var fileStream = new FileStream(@"data\sql1.txt", FileMode.Open, FileAccess.Read);
            //        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            //        {
            //            text = streamReader.ReadToEnd();
            //        }

            //    string sql = "INSERT INTO iHGenelHasilat (Tarih,Personel,Otopark,IcMrkzOAdet,IcMrkzOGelir,IcAboneOAboneAdet,IcAboneOAboneGelir,IcAboneOCongAdet,IcAboneOCongGelir,IcCks2OAdet,IcCks2OGelir,RentCarMbCongAdet,RentCarMbCongGelir,RentCarMbAboneAdet,RentCarMbAboneGelir,IhNakitFaturaAdet,IhNakitFatuıraGelir,IhKrediKartiFaturaAdet,IhKrediKartiFaturaGelir,IhEFTFaturaAdet,IhEFTFaturaGelir,KsbKrediKartiAdet,KsbKrediKartiGelir,AboneKrediKartiAdet,AboneKrediKartiGelir,GenelToplamAdet,GenelToplam,AboneHariciGelirAd,AboneHariciGelir) VALUES (@Tarih,@Personel,@Otopark,@IcMrkzOAdet,@IcMrkzOGelir,@IcAboneOAboneAdet,@IcAboneOAboneGelir,@IcAboneOCongAdet,@IcAboneOCongGelir,@IcCks2OAdet,@IcCks2OGelir,@RentCarMbCongAdet,@RentCarMbCongGelir,@RentCarMbAboneAdet,@RentCarMbAboneGelir,@IhNakitFaturaAdet,@IhNakitFatuıraGelir,@IhKrediKartiFaturaAdet,@IhKrediKartiFaturaGelir,@IhEFTFaturaAdet,@IhEFTFaturaGelir,@KsbKrediKartiAdet,@KsbKrediKartiGelir,@AboneKrediKartiAdet,@AboneKrediKartiGelir,@GenelToplamAdet,@GenelToplam,@AboneHariciGelirAd,@AboneHariciGelir)";
            //      //string sql = "INSERT INTO iHGenelHasilat (Tarih,Personel,Otopark,IcMrkzOAdet,IcMrkzOGelir) VALUES (@Tarih,@Personel,@Otopark,@IcMrkzOAdet,@IcMrkzOGelir)";

            //    //MessageBox.Show(text);
            //        btnKaydet.Enabled = false;
            //        btnYeni.Enabled = true;
            //        ControlAllTextColor(this);
                    
            //        baglanti.Open();
            //        SqlCommand komut = new SqlCommand(sql, baglanti);
            //        komut.Parameters.AddWithValue("@Tarih",dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            //        komut.Parameters.AddWithValue("@Personel", comboBoxP.Text);
            //        komut.Parameters.AddWithValue("@Otopark",label31.Text);
            //        komut.Parameters.AddWithValue("@IcMrkzOAdet", txtMrkzOAdet.Text);
            //        komut.Parameters.AddWithValue("@IcMrkzOGelir",Convert.ToDecimal(txtMrkzOGelir.Text));
            //        komut.Parameters.AddWithValue("@IcAboneOAboneAdet", txtIcWksAboneAdet.Text);
            //        komut.Parameters.AddWithValue("@IcAboneOAboneGelir", Convert.ToDecimal(txtIcWksAboneGelir.Text));
            //        komut.Parameters.AddWithValue("@IcAboneOCongAdet", txtAboneOCongAdet.Text);
            //        komut.Parameters.AddWithValue("@IcAboneOCongGelir", Convert.ToDecimal(txtAboneOCongGelir.Text));
            //        komut.Parameters.AddWithValue("@IcCks2OAdet", txtIcCks2OAdet.Text);
            //        komut.Parameters.AddWithValue("@IcCks2OGelir", Convert.ToDecimal(txtIcCks2OGelir.Text));
            //        komut.Parameters.AddWithValue("@RentCarMbCongAdet", txtRentCarMbCongAdet.Text);
            //        komut.Parameters.AddWithValue("@RentCarMbCongGelir", Convert.ToDecimal(txtRentCarMbCongGelir.Text));
            //        komut.Parameters.AddWithValue("@RentCarMbAboneAdet", txtRentCarMbAdet.Text);
            //        komut.Parameters.AddWithValue("@RentCarMbAboneGelir", Convert.ToDecimal(txtRentCarMbGelir.Text));
            //        komut.Parameters.AddWithValue("@IhNakitFaturaAdet", txtNakitFaturaAdet.Text);
            //        komut.Parameters.AddWithValue("@IhNakitFatuıraGelir", Convert.ToDecimal(txtNakitFaturaGelir.Text));
            //        komut.Parameters.AddWithValue("@IhKrediKartiFaturaAdet", txtKrediKartiFaturaAdet.Text);
            //        komut.Parameters.AddWithValue("@IhKrediKartiFaturaGelir",Convert.ToDecimal(txtKrediKartiFaturaGelir.Text));
            //        komut.Parameters.AddWithValue("@IhEFTFaturaAdet", txtEFTFaturaAdet.Text);
            //        komut.Parameters.AddWithValue("@IhEFTFaturaGelir", Convert.ToDecimal(txtEFTFaturaGelir.Text));
            //        komut.Parameters.AddWithValue("@KsbKrediKartiAdet", txtKsbKrediKartiAdet.Text);
            //        komut.Parameters.AddWithValue("@KsbKrediKartiGelir", Convert.ToDecimal(txtKsbKrediKartiGelir.Text));
            //        komut.Parameters.AddWithValue("@AboneKrediKartiAdet", txtAboneKrediKartiAdet.Text);
            //        komut.Parameters.AddWithValue("@AboneKrediKartiGelir", Convert.ToDecimal(txtAboneKrediKartiGelir.Text));
            //        komut.Parameters.AddWithValue("@GenelToplamAdet", txtGenelToplamAdet.Text);
            //        komut.Parameters.AddWithValue("@GenelToplam", Convert.ToDecimal(txtGenelToplam.Text));
            //        komut.Parameters.AddWithValue("@AboneHariciGelirAd", txtAboneHariciGelirAd.Text);
            //        komut.Parameters.AddWithValue("@AboneHariciGelir", Convert.ToDecimal(txtAboneHariciGelir.Text));
            //        komut.ExecuteNonQuery();
            //        baglanti.Close();
            //        MessageBox.Show("Genel Hasılat Verisi Kaydı Tamamlandı.");
            //        ClearAllText(this);
            //        txtId.Text = "0";
                    
            //        sql = "Select Id,Tarih,Personel,Otopark,IcMrkzOAdet,IcMrkzOGelir,IcAboneOAboneAdet,IcAboneOAboneGelir,IcAboneOCongAdet,IcAboneOCongGelir,IcCks2OAdet,IcCks2OGelir,RentCarMbCongAdet,RentCarMbCongGelir,RentCarMbAboneAdet,RentCarMbAboneGelir,IhNakitFaturaAdet,IhNakitFatuıraGelir,IhKrediKartiFaturaAdet,IhKrediKartiFaturaGelir,IhEFTFaturaAdet,IhEFTFaturaGelir,KsbKrediKartiAdet,KsbKrediKartiGelir,AboneKrediKartiAdet,AboneKrediKartiGelir,GenelToplamAdet,GenelToplam,AboneHariciGelirAd,AboneHariciGelir FROM iHGenelHasilat where Tarih>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'";
            //        baglanti.Open();
            //        SqlCommand cmd = new SqlCommand(sql, baglanti);
            //        SqlDataAdapter da = new SqlDataAdapter();
            //        da.SelectCommand = cmd;
            //        DataTable dt = new DataTable();
            //        da.Fill(dt);
            //        dataGridView1.DataSource = dt;
            //        baglanti.Close();     
            //        baglanti.Dispose();        
                    
            //        //catch (Exception ex)
            //        //{
            //        //    MessageBox.Show(ex.Message);
            //        //    if (baglanti.State != ConnectionState.Open)
            //        //    {
            //        //        baglanti.Close();
            //        //    }
            //        //}
               

            //}


             

        }

        void ClearAllText(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c is System.Windows.Forms.TextBox)
                    ((System.Windows.Forms.TextBox)c).Clear();
                else
                    ClearAllText(c);
            }
            txtRentCarMbAdet.Text = "0";
            txtRentCarMbGelir.Text = "0";
            

        }

        private void btniptal_Click(object sender, EventArgs e)
        {
            
            ClearAllText(this);
            ControlAllTextColor(this);

            txtId.Text = "0";
            btnYeni.Enabled= true;
            btnKaydet.Enabled= false;   
            tableLayoutPanel2.Enabled= false;
        }

        void ControlAllTextColor(Control con)
        {
            foreach (Control txtbox in con.Controls)
            {
                if (txtbox is System.Windows.Forms.TextBox)
                {


                    txtbox.BackColor = Color.White;
                    txtKontol = false;
                    //MessageBox.Show("Buradayım Aslan");

                }

                else
                    ControlAllTextColor(txtbox);
            }
        }

        void ControlAllText(Control con)
        {
            foreach (Control z in con.Controls)
            {
                if (z is System.Windows.Forms.TextBox)
                {
                    if (((System.Windows.Forms.TextBox)z).Text == string.Empty)
                    {
                       z.BackColor = Color.Yellow;
                       txtKontol=true;
                    }
                }

                else
                    ControlAllText(z);
            }
           
        }




    }
}
