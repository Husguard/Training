using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApplication7
{
   
    public partial class Form1 : Form
    {
        Cezar obj = new Cezar();
        public Form1()
        {   
            InitializeComponent();    
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            textBox2.Text = obj.Codeс(textBox1.Text, (int)numericUpDown1.Value);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            textBox3.Text = obj.Codeс(textBox2.Text, -(int)numericUpDown1.Value);
        }
    }
    class Alphabet
    {
        string alphabet;

        public Alphabet(string m)
        {
            alphabet = m;
        }

        public string Change(string m, int key)
        {
            if (m == " ") return " ";
            int pos = alphabet.IndexOf(m);
            if (pos == -1) return ""; 
            pos = (pos + key) % alphabet.Length; 
            if (pos < 0) pos += alphabet.Length; // при дешифровке
            return alphabet.Substring(pos, 1);
        }
    }

    class Cezar : List<Alphabet> 
    {
        string cache;
        public Cezar()
        {
            
            try { StreamReader sr = new StreamReader("asciifile.txt");
                while ((cache = sr.ReadLine()) != null)
                {
                    this.Add(new Alphabet(cache));
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("File ASCII not found");
                Environment.Exit(0);
            }            
        }
        public string Codeс(string m, int key)
        {
            string result = "", tmp = "";
            for (int i = 0; i < m.Length; i++)
            {
                foreach (Alphabet v in this)
                {
                    tmp = v.Change(m.Substring(i, 1), key);
                    if (tmp != "") 
                    {
                        result += tmp;
                        break;
                    }
                }
                if (tmp == "") result += m.Substring(i, 1);
            }
            return result;
        }
    }
}
