using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using COMPI_PY1.Analizador;

namespace COMPI_PY1
{
    public partial class Form1 : Form
    {

        List<TabPage> plist = new List<TabPage>();
        int pestaña = 0;
        OpenFileDialog abrir = null;

        public Form1()
        {
            InitializeComponent();
            button1_Click(new object(), new EventArgs());
            seleccion.DropDownStyle = ComboBoxStyle.DropDownList;
            seleccion.Items.Clear();
            //seleccion.Items.Add("Puto");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RichTextBox t = new RichTextBox();
            TabPage n = new TabPage("Pestaña "+pestaña);
            t.SetBounds(0,0,entrada.Width, entrada.Height);
            n.Controls.Add(t);
            plist.Add(n);
            entrada.TabPages.Add(n);
            pestaña++;
            entrada.SelectedTab = n;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TabPage n = entrada.SelectedTab;
            if (n != null) {
                plist.Remove(n);
                entrada.TabPages.Remove(n);
            }
            

            //obtener texto y modificarlo
            //RichTextBox t = (RichTextBox) n.Controls[0];
            //t.AppendText("malditos");
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrir = new OpenFileDialog();
            abrir.Filter = "Documento de texto |* .er";
            abrir.Title = "Abrir";
            var resultado = abrir.ShowDialog();
            if (resultado == DialogResult.OK)
            {
                StreamReader leer = new StreamReader(abrir.FileName);
                TabPage n = entrada.SelectedTab;
                if (n != null) {
                    RichTextBox t = (RichTextBox)n.Controls[0];
                    t.Text = leer.ReadToEnd();
                }
                
                leer.Close();
            }
            else
            {
                abrir = null;
            }

        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage n = entrada.SelectedTab;
            if (abrir == null)
            {
                SaveFileDialog guardar = new SaveFileDialog();
                guardar.Filter = "Documento de texto |* .er";
                guardar.Title = "Guardar";
                guardar.FileName = "Titulo";
                var resultado = guardar.ShowDialog();
                if (resultado == DialogResult.OK)
                {
                    StreamWriter escribir = new StreamWriter(guardar.FileName);
                    
                    if (n != null)
                    {
                        RichTextBox t = (RichTextBox)n.Controls[0];
                        
                        foreach (object line in t.Lines)
                        {
                            escribir.WriteLine(line);
                        }
                        abrir = new OpenFileDialog();
                        abrir.Filter = "Documento de texto |* .er";
                        abrir.Title = "Abrir";
                        abrir.FileName = guardar.FileName;
                        escribir.Close();
                    }
                    
                }
            }
            else
            {
                StreamWriter escribir = new StreamWriter(abrir.FileName);
                RichTextBox t = (RichTextBox)n.Controls[0];
                foreach (object line in t.Lines)
                {
                    escribir.WriteLine(line);
                }
                escribir.Close();
            }
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog guardar = new SaveFileDialog();
            guardar.Filter = "Documento de texto |* .er";
            guardar.Title = "Guardar";
            guardar.FileName = "Titulo";
            var resultado = guardar.ShowDialog();
            if (resultado == DialogResult.OK)
            {
                StreamWriter escribir = new StreamWriter(guardar.FileName);
                TabPage n = entrada.SelectedTab;
                if (n != null)
                {
                    RichTextBox t = (RichTextBox)n.Controls[0];

                    foreach (object line in t.Lines)
                    {
                        escribir.WriteLine(line);
                    }
                    abrir = new OpenFileDialog();
                    abrir.Filter = "Documento de texto |* .er";
                    abrir.Title = "Abrir";
                    abrir.FileName = guardar.FileName;
                    escribir.Close();
                }
                
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void seleccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string t = "Reportes\\" + seleccion.GetItemText(seleccion.SelectedItem) + ".jpg";
            //salida.AppendText(t);
            try
            {
                img.Image = Image.FromFile(t);
                img.SizeMode = PictureBoxSizeMode.AutoSize;
                panel.AutoScroll = true;
            }
            catch (FileNotFoundException)
            {
                img.Image = null;
            }
            
        }

        private void analizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            salida.Text = "";
            seleccion.Items.Clear();
            //seleccion.Items.Add("Puto");
            //try
            //{
                TabPage n = entrada.SelectedTab;
                RichTextBox t = (RichTextBox)n.Controls[0];
                if (t.Text != "")
                {
                    Lexico temp = new Lexico(t.Text, salida, seleccion);
                    temp.Analizar();
                    

                }
            //}
            //catch (NullReferenceException)
            //{
            //    MessageBox.Show("No hay pestaña, cree una...","Error");
            //}
            
            

        }
    }
}
