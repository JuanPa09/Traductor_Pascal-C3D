using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Irony.Parsing;
using System.Diagnostics;

namespace Traductor_Pascal_C3D.analizador
{
    class Analizador
    {
        RichTextBox debuggerConsole;
        RichTextBox Salida;

        public Analizador(RichTextBox debugger, RichTextBox Salida)
        {
            this.debuggerConsole = debugger;
            this.Salida = Salida;
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

        }


    }
}
