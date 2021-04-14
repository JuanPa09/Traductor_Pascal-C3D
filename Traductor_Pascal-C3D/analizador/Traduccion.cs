using System;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;
using System.Windows.Forms;
using Traductor_Pascal_C3D.traductor.abstractas;
using System.Diagnostics;
using Traductor_Pascal_C3D.traductor.generador;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.reportes;
using Traductor_Pascal_C3D.traductor.instruccion.funciones;

namespace Traductor_Pascal_C3D.analizador
{
    class Traduccion
    {
        ParseTreeNode nodoRaiz;
        RichTextBox consola;
        Reporte reporte;

        public Traduccion(ParseTreeNode nodoRaiz, RichTextBox consola, Reporte reporte)
        {
            this.nodoRaiz = nodoRaiz;
            this.consola = consola;
            this.reporte = reporte;
        }

        public void iniciar()
        {
            LinkedList<Instruccion> listaInstrucciones = instrucciones(nodoRaiz);
            traducir(listaInstrucciones);
        }

        public void traducir(LinkedList<Instruccion> listaInstrucciones)
        {

            /*
             Compilar Aqui
             */
            Entorno global = new Entorno(null);
            foreach(Instruccion instruccion in listaInstrucciones)
            {
                instruccion.compile(global,reporte);
            }

            Generador generador = Generador.getInstance();
            generador.addHeader();
            consola.Text = generador.getCode();
        }

        public LinkedList<Instruccion> instrucciones(ParseTreeNode actual)
        {
            LinkedList<Instruccion> listaInstrucciones = new LinkedList<Instruccion>();
            foreach (ParseTreeNode nodo in actual.ChildNodes)
            {
                Debug.WriteLine("Nodo -> " + nodo.Term.ToString());

                if (nodo.ChildNodes.Count == 2)
                {
                    instruccionesMultiples(ref listaInstrucciones, nodo);
                }
                else
                {
                    listaInstrucciones.AddLast(instruccion(nodo));
                }
            }
            return listaInstrucciones;
        }

        public void instruccionesMultiples(ref LinkedList<Instruccion> listaInstrucciones, ParseTreeNode actual)
        {
            //LLEGA A INSTRUCCIONES QUE PUEDEN TENER MAS INSTRUCCIONES
            foreach (ParseTreeNode nodo in actual.ChildNodes)
            {
                //Aca se van a hacer los 2 ciclos
                Debug.WriteLine("Nodo -> " + nodo.Term.ToString());
                if (nodo.ChildNodes.Count == 2 && nodo.ChildNodes[1].Term.ToString() != "Pt_Comas")
                {
                    instruccionesMultiples(ref listaInstrucciones, nodo);
                    continue;
                }

                if (nodo.ChildNodes.Count != 0)
                {
                    Instruccion instr = instruccion(nodo);
                    if (instr != null)
                    {
                        listaInstrucciones.AddLast(instr);
                    }
                    else
                    {
                        instruccionesMultiples(ref listaInstrucciones, nodo);
                    }
                }
            }

        }

        public Instruccion instruccion(ParseTreeNode actual)
        {
            Debug.WriteLine("Evaluando: " + actual.ChildNodes[0].Term.ToString());
            string nombreNodo = actual.ChildNodes[0].Term.ToString();
            if (actual.ChildNodes.Count == 0) { nombreNodo = actual.ChildNodes[0].Token.Text; }

            switch (nombreNodo)
            {
                case "program":
                    //Obtener Instrucciones Head e Instrucciones Body
                    return new Estructura(instrucciones(actual.ChildNodes[3]), instrucciones(actual.ChildNodes[5]));
                case "Writes":
                    if (actual.ChildNodes[0].ChildNodes[0].Token.Text == "writeln")
                        return null;
                    return null;
                case "Variables":
                case "Types":
                case "Asignacion":
                case "If_Statement":
                case "For_Statement":
                case "While_Statement":
                case "Repeat_Statement":
                case "Case_Statement":
                case "Funcion":
                case "Procedimiento":
                case "Llamada":
                case "break":
                case "continue":
                case "exit":
                case "graficar_ts":
                    return null;


            }

            return null;
        }

    }
}
