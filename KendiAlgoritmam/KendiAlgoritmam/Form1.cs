using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KendiAlgoritmam
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        char[] alfabe = { 'a', 'b', 'c', 'ç', 'd', 'e', 'f', 'g', 'ğ',
            'h', 'ı', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'ö', 'p', 'q', 'r',
            's', 'ş', 't', 'u', 'ü', 'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'Ç', 'D', 'E', 'F', 'G', 'Ğ', 'H', 'I',
            'İ', 'J', 'K', 'L', 'M', 'N', 'O', 'Ö', 'P', 'Q', 'R',
            'S', 'Ş', 'T', 'U', 'Ü', 'V', 'W', 'X', 'Y', 'Z' };
        public int a;
        public int b;
        public int c;
        private string dosyaYolu = string.Empty;


        private string Sifrele(string metin, int anahtar1, int anahtar2, int anahtar3)
        {
            try
            {

                string sifrelenmisMetin = "";
                for (int i = 0; i < metin.Length; i++)
                {
                    char karakter = metin[i];

                    int index = Array.IndexOf(alfabe, karakter);
                    if (index != -1)
                    {
                        int sifrelenmisIndex = (index + anahtar1 - anahtar2 + anahtar3) % alfabe.Length;
                        anahtar3 += anahtar3;
                        if (sifrelenmisIndex < 0) sifrelenmisIndex += alfabe.Length;

                        char sifrelenmisKarakter = alfabe[sifrelenmisIndex];
                        sifrelenmisMetin += alfabe[sifrelenmisIndex];
                    }
                    else
                    {
                        sifrelenmisMetin += karakter;
                    }
                }
                return sifrelenmisMetin;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Şifrelenirken bir hata oluştu" +
                    " Hata: " + ex.Message);
                return "";
            }
        }

        private string Desifrele(string sifrelenmisMetin, int anahtar1, int anahtar2, int anahtar3)
        {
            try
            {
                string cozulmusMetin = "";
                for (int i = 0; i < sifrelenmisMetin.Length; i++)
                {
                    char karakter = sifrelenmisMetin[i];
                    int index = Array.IndexOf(alfabe, karakter);
                    if (index != -1)
                    {
                        int cozulmusIndex = (index - anahtar1 + anahtar2 - anahtar3) % alfabe.Length;
                        anahtar3 += anahtar3;
                        if (cozulmusIndex < 0) cozulmusIndex += alfabe.Length;

                        char cozulmusKarakter = alfabe[cozulmusIndex];
                        while (cozulmusIndex < 0)
                        {
                            cozulmusIndex += alfabe.Length;
                        }
                        cozulmusMetin += alfabe[cozulmusIndex];
                    }
                    else
                    {
                        cozulmusMetin += karakter;
                    }
                }
                return cozulmusMetin;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Deşifrelerken bir hata oluştu" +
                    " Hata: " + ex.Message);
                return "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string girilenKelime = textBox1.Text;
            int a = (int)numericUpDown1.Value;
            int b = (int)numericUpDown2.Value;
            int c = (int)numericUpDown3.Value;
            string sifreliKelime = Sifrele(girilenKelime, a, b, c);
            textBox2.Text = sifreliKelime;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string girilenKelime = textBox1.Text;
            int a = (int)numericUpDown1.Value;
            int b = (int)numericUpDown2.Value;
            int c = (int)numericUpDown3.Value;
            string desifreliKelime = Desifrele(girilenKelime, a, b, c);
            textBox2.Text = desifreliKelime;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Text = "Şifrele";
            button2.Text = "Şifre Çöz";
            button3.Text = "Dosya ekle";
            button4.Text = "Dosya Kaydet";
            button5.Text = "Temizle";
            numericUpDown1.ValueChanged += NumericUpDown1_ValueChanged;
            numericUpDown2.ValueChanged += NumericUpDown2_ValueChanged;
            numericUpDown3.ValueChanged += NumericUpDown3_ValueChanged;

        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            a = (int)numericUpDown1.Value;
            UpdateFileContent();
        }

        private void NumericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            b = (int)numericUpDown2.Value;
            UpdateFileContent();
        }

        private void NumericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            c = (int)numericUpDown3.Value;
            UpdateFileContent();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string dosyaYolu = openFileDialog.FileName;
                try
                {
                    string metin = File.ReadAllText(dosyaYolu);
                    textBox1.Text = metin;


                    ExtractKeysFromText(metin);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Dosya okunurken bir hata oluştu: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string dosyaYolu = saveFileDialog.FileName;
                try
                {
                    File.WriteAllText(dosyaYolu, textBox2.Text);
                    MessageBox.Show("Dosya başarıyla kaydedildi.");
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Dosya kaydedilirken bir hata oluştu: " + ex.Message);
                }
            }
        }

        private void ExtractKeysFromText(string metin)
        {
            bool numericUpDownEnabled = true;

            string[] lines = metin.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                if (line.Contains("⁕"))
                {
                    a = ExtractValue(line, '⁕');
                    numericUpDownEnabled = false;
                }
                else if (line.Contains("※"))
                {
                    b = ExtractValue(line, '※');
                    numericUpDownEnabled = false;
                }
                else if (line.Contains("⁜"))
                {
                    c = ExtractValue(line, '⁜');
                    numericUpDown3.Enabled = false;
                }
                if (numericUpDownEnabled)
                {
                    numericUpDown1.Enabled = true;
                    numericUpDown2.Enabled = true;
                    numericUpDown3.Enabled = true;
                }
                else
                {
                    numericUpDown1.Enabled = false;
                    numericUpDown2.Enabled = false;
                    numericUpDown3.Enabled = false;
                }
            }


            numericUpDown1.Value = a;
            numericUpDown2.Value = b;
            numericUpDown3.Value = c;
        }

        private int ExtractValue(string line, char keyChar)
        {
            int value = 0;
            int index = line.IndexOf(keyChar);
            if (index != -1 && index + 1 < line.Length)
            {
                string valueStr = line.Substring(index + 1).Trim();
                int.TryParse(valueStr, out value);
            }
            return value;
        }

        private void UpdateFileContent()
        {
            if (!string.IsNullOrEmpty(dosyaYolu))
            {
                try
                {
                    string[] lines = File.ReadAllLines(dosyaYolu);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains("⁕"))
                        {
                            lines[i] = UpdateLineValue(lines[i], '⁕', a);
                        }
                        else if (lines[i].Contains("※"))
                        {
                            lines[i] = UpdateLineValue(lines[i], '※', b);
                        }
                        else if (lines[i].Contains("⁜"))
                        {
                            lines[i] = UpdateLineValue(lines[i], '⁜', c);
                        }

                    }
                    File.WriteAllLines(dosyaYolu, lines);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Dosya güncellenirken bir hata oluştu: " + ex.Message);
                }
            }
        }

        private string UpdateLineValue(string line, char keyChar, int newValue)
        {
            int index = line.IndexOf(keyChar);
            if (index != -1)
            {
                return line.Substring(0, index + 1) + newValue.ToString();
            }
            return line;
        }



        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
