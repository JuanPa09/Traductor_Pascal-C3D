﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using Traductor_Pascal_C3D.analizador;

namespace Traductor_Pascal_C3D
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void AddLineNumbers(RichTextBox editor, RichTextBox numbers)
        {
            // create & set Point pt to (0,0)    
            Point pt = new Point(0, 0);
            // get First Index & First Line from richTextBox1    
            int First_Index = editor.GetCharIndexFromPosition(pt);
            int First_Line = editor.GetLineFromCharIndex(First_Index);
            // set X & Y coordinates of Point pt to ClientRectangle Width & Height respectively    
            pt.X = ClientRectangle.Width;
            pt.Y = ClientRectangle.Height;
            // get Last Index & Last Line from richTextBox1    
            int Last_Index = editor.GetCharIndexFromPosition(pt);
            int Last_Line = editor.GetLineFromCharIndex(Last_Index);
            // set Center alignment to LineNumberTextBox    
            numbers.SelectionAlignment = HorizontalAlignment.Center;
            // set LineNumberTextBox text to null & width to getWidth() function value    
            numbers.Text = "";
            // now add each line number to LineNumberTextBox upto last line    
            for (int i = First_Line; i <= Last_Line + 2; i++)
            {
                numbers.Text += i + 1 + "\n";
            }
        }

        private void Salida_SelectionChanged(object sender, EventArgs e)
        {
            Point pt = Salida.GetPositionFromCharIndex(Salida.SelectionStart);
            if (pt.X == 1)
            {
                AddLineNumbers(Salida, LinearNumberSalida);
            }
        }

        private void Entrada_SelectionChanged(object sender, EventArgs e)
        {
            Point pt = Entrada.GetPositionFromCharIndex(Entrada.SelectionStart);
            if (pt.X == 1)
            {
                AddLineNumbers(Entrada, LinearNumberEntrada);
            }
        }

        private void Entrada_VScroll(object sender, EventArgs e)
        {
            LinearNumberEntrada.Text = "";
            AddLineNumbers(Entrada, LinearNumberEntrada);
            LinearNumberEntrada.Invalidate();
        }

        private void Salida_VScroll(object sender, EventArgs e)
        {
            LinearNumberSalida.Text = "";
            AddLineNumbers(Salida, LinearNumberSalida);
            LinearNumberSalida.Invalidate();
        }

        private void Salida_TextChanged(object sender, EventArgs e)
        {
            if (Salida.Text == "")
            {
                AddLineNumbers(Salida, LinearNumberSalida);
            }
        }

        private void Entrada_TextChanged(object sender, EventArgs e)
        {
            if (Entrada.Text == "")
            {
                AddLineNumbers(Entrada, LinearNumberEntrada);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LinearNumberEntrada.Font = Entrada.Font;
            LinearNumberSalida.Font = Salida.Font;
            AddLineNumbers(Entrada, LinearNumberEntrada);
            AddLineNumbers(Salida, LinearNumberSalida);
            Entrada.Text = "program compiladores2;\nbegin\nend.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Debugger.Text = "";
            Analizador analizador = new Analizador(Debugger, Salida);
            Debug.WriteLine("Iniciando Traducción");
            Debugger.AppendText("Iniciando Traducción\n");
            analizador.traducir(Entrada.Text);
            Debugger.AppendText("Finalizando Traducción");
            Debug.WriteLine("Finalizando Traducción");
            
        }
    }
}
