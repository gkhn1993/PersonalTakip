using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonalTakip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection con;
        SqlCommand cmd;
        string connectionString = "Server=DESKTOP-SJ82E1K\\SQLEXPRESS;Database=PersonalDb;Trusted_Connection=True;";
        int personelId = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            birimLoad();
            personelLoad();
        }
        void birimLoad()
        {
            try
            {
                con = new SqlConnection(connectionString);
                cmd = new SqlCommand("select * from birimler", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    comboBoxBirim.Items.Add(row["birim"].ToString());

                }
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }
        void personelLoad()
        {
            try
            {
                con = new SqlConnection(connectionString);
                cmd = new SqlCommand("select * from personeller", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dg.DataSource = dt;
                dg.Columns["id"].Visible = false;
               
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        void personelEkle()
        {
            try
            {
                con = new SqlConnection(connectionString);
                cmd = new SqlCommand("insert into personeller(sicil, ad, soyad, cinsiyet, dogumTarihi, birim, cepTel, adres) " +
                    "values (@sicil, @ad, @soyad, @cinsiyet, @dogumTarihi, @birim, @cepTel, @adres)"
                    , con);
                con.Open();
                cmd.Parameters.Add("@sicil", SqlDbType.Int).Value = txtSicil.Text;
                cmd.Parameters.Add("@ad", SqlDbType.VarChar).Value = txtAd.Text;
                cmd.Parameters.Add("@soyad", SqlDbType.VarChar).Value = txtSoyad.Text;

                string cinsiyet = "";
                if(radioButtonErkek.Checked)
                {
                    cinsiyet = radioButtonErkek.Text;
                }
                else if(radioButtonKadin.Checked)
                {
                    cinsiyet = radioButtonKadin.Text;
                }
                cmd.Parameters.Add("@cinsiyet", SqlDbType.VarChar).Value = cinsiyet;
                cmd.Parameters.Add("@dogumTarihi", SqlDbType.Date).Value = maskedTextBoxDogumTarihi.Text;
                cmd.Parameters.Add("@birim", SqlDbType.VarChar).Value = comboBoxBirim.Text;
                cmd.Parameters.Add("@cepTel", SqlDbType.VarChar).Value = maskedTextBoxCepTel.Text;
                cmd.Parameters.Add("@adres", SqlDbType.VarChar).Value = txtAdres.Text;
                cmd.ExecuteNonQuery();
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            personelLoad();
                
        }
        void personelGuncelle()
        {
            try
            {
                con = new SqlConnection(connectionString);
                cmd = new SqlCommand("update personeller set sicil = @sicil, ad = @ad, soyad = @soyad, cinsiyet = @cinsiyet, dogumTarihi = @dogumTarihi, birim = @birim, cepTel = @cepTel, adres = @adres where id = @id", con);
                con.Open();
                cmd.Parameters.Add("@sicil", SqlDbType.Int).Value = txtSicil.Text;
                cmd.Parameters.Add("@ad", SqlDbType.VarChar).Value = txtAd.Text;
                cmd.Parameters.Add("@soyad", SqlDbType.VarChar).Value = txtSoyad.Text;

                string cinsiyet = "";
                if (radioButtonErkek.Checked)
                {
                    cinsiyet = radioButtonErkek.Text;
                }
                else if (radioButtonKadin.Checked)
                {
                    cinsiyet = radioButtonKadin.Text;
                }
                cmd.Parameters.Add("@cinsiyet", SqlDbType.VarChar).Value = cinsiyet;
                cmd.Parameters.Add("@dogumTarihi", SqlDbType.Date).Value = maskedTextBoxDogumTarihi.Text;
                cmd.Parameters.Add("@birim", SqlDbType.VarChar).Value = comboBoxBirim.Text;
                cmd.Parameters.Add("@cepTel", SqlDbType.VarChar).Value = maskedTextBoxCepTel.Text;
                cmd.Parameters.Add("@adres", SqlDbType.VarChar).Value = txtAdres.Text;
                cmd.Parameters.Add("id", SqlDbType.Int).Value = personelId;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            personelLoad();

        }
        void personelSil()
        {
            try
            {
                con = new SqlConnection(connectionString);
                cmd = new SqlCommand("delete personeller where id = @id", con);
                con.Open();
                
                cmd.Parameters.Add("id", SqlDbType.Int).Value = personelId;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            personelLoad();

        }
        void temizle()
        {
            personelId = 0;

            foreach (var item in tableLayoutPanel1.Controls)
            {
                if(item is TextBox)
                {
                    ((TextBox)item).Text = "";
                }
                if (item is MaskedTextBox)
                {
                    ((MaskedTextBox)item).Text = "";
                }
                if (item is ComboBox)
                {
                    ((ComboBox)item).Text = "";
                }

            }

            foreach (var item in tableLayoutPanel2.Controls)
            {
                if(item is RadioButton)
                {
                    ((RadioButton)item).Checked = false;
                }
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if(personelId == 0)
            {
                personelEkle();
            }
            else
            {
                MessageBox.Show("Secili personel var");
            }
            
        }

        private void dg_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                personelId = Convert.ToInt32(dg.Rows[e.RowIndex].Cells["id"].Value);
            }
            try
            {
                con = new SqlConnection(connectionString);
                cmd = new SqlCommand("select * from personeller where id = @id", con);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = personelId;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                radioButtonErkek.Checked = false;
                radioButtonKadin.Checked = false;

                foreach (DataRow row in dt.Rows)
                {
                    txtSicil.Text = row["sicil"].ToString();
                    txtAd.Text = row["ad"].ToString();
                    txtSoyad.Text = row["soyad"].ToString();
                    string cinsiyet = row["cinsiyet"].ToString();
                    if(cinsiyet == radioButtonErkek.Text)
                    {
                        radioButtonErkek.Checked = true;
                    }
                    else if(cinsiyet == radioButtonKadin.Text)
                    {
                        radioButtonKadin.Checked = true;
                    }
                    maskedTextBoxDogumTarihi.Text = string.Format("{0:dd/MM/yyyy}", row["dogumTarihi"]);
                    comboBoxBirim.Text = row["birim"].ToString();
                    maskedTextBoxCepTel.Text = row["cepTel"].ToString();
                    txtAdres.Text = row["adres"].ToString();
                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if(personelId > 0)
            {
                personelGuncelle();
            }
            else
            {
                MessageBox.Show("Personel Seciniz");
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (personelId > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Seçili personeli silmek istediğinize emin misiniz? ", "Personel Sil", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    personelSil();
                    temizle();
                }
                else
                {
                    MessageBox.Show("İslem iptal edildi!");
                }
            }
            else
            {
                MessageBox.Show("Personel Seciniz");
            }
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }
    }
}
