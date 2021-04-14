using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Irony.Parsing;
using System.Diagnostics;
using System.IO;
using Traductor_Pascal_C3D.traductor.reportes;

namespace Traductor_Pascal_C3D.analizador
{
    class Analizador
    {
        RichTextBox debuggerConsole;
        RichTextBox Salida;
        Reporte reporte;

        public Analizador(RichTextBox debugger, RichTextBox Salida, Reporte reporte)
        {
            this.debuggerConsole = debugger;
            this.Salida = Salida;
            this.reporte = reporte;
        }

        public void traducir(string cadena)
        {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            foreach (var item in lenguaje.Errors)
            {
                Debug.WriteLine(item);
            }
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode raiz = arbol.Root;

            if (arbol.ParserMessages.Count > 0)
            {
                int i = 1;
                foreach (var item in arbol.ParserMessages)
                {
                    //Error Lexico
                    if (item.Message.Contains("Invalid character"))
                    {
                        Debug.WriteLine(i + ") Error Sintáctico: Linea " + item.Location.Line + " Columna: " + item.Location.Column + "Mensaje: " + item.Message);
                        debuggerConsole.AppendText("Error Sintáctico: Linea " + item.Location.Line + " Columna: " + item.Location.Column + "Mensaje: " + item.Message + "\n\n");
                    }
                    //Error Sintactico
                    else
                    {
                        Debug.WriteLine("Error Sintáctico: Linea " + item.Location.Line + " Columna: " + item.Location.Column + " Mensaje: " + item.Message);
                        debuggerConsole.AppendText(i + ") Error Sintáctico: Linea " + item.Location.Line + " Columna: " + item.Location.Column + " Mensaje: " + item.Message + "\n\n");
                    }
                    i++;
                }
            }

            if (raiz == null)
            {
                Debug.WriteLine(arbol.ParserMessages[0].Message);
                debuggerConsole.AppendText(arbol.ParserMessages[0].Message + "\n");
                return;
            }

            Traduccion traduccion = new Traduccion(raiz, Salida,reporte);
            debuggerConsole.Text = "";
            traduccion.iniciar();
            //traduccion.traducir();
            generarGrafo(raiz);

        }

        public void generarGrafo(ParseTreeNode raiz)
        {

            string grafoDot = Graficador.getDot(raiz);
            string path = "C:\\compiladores2\\ast.txt";
            try
            {
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(grafoDot);
                    fs.Write(info, 0, info.Length);
                }
                debuggerConsole.AppendText("Arbol generado!\n");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }


    }
}
