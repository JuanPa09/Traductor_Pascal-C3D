using System;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;
using System.Windows.Forms;
using Traductor_Pascal_C3D.optimizador.abstracta;
using Traductor_Pascal_C3D.optimizador.instrucciones;
using Traductor_Pascal_C3D.optimizador.reporte;
using Traductor_Pascal_C3D.optimizador.generador;
using System.Diagnostics;

namespace Traductor_Pascal_C3D.analizador
{
    class Optimizacion
    {
        ParseTreeNode raiz;
        RichTextBox consola;

        public Optimizacion(ParseTreeNode raiz, RichTextBox consola)
        {
            this.raiz = raiz;
            this.consola = consola;
        }

        public void iniciar()
        {
            Reporte reporte = Reporte.getInstance();
            Generador generador = Generador.getInstance();
            generador.clearData();
            reporte.clearData();
            LinkedList<Optimizar> instrucciones = new LinkedList<Optimizar>();
            this.instrucciones(raiz, ref instrucciones);
            this.ejecutarInstrucciones(instrucciones);
            consola.Text = generador.getCode();
        }

        public void ejecutarInstrucciones(LinkedList<Optimizar> instrucciones)
        {
            foreach(Optimizar instruccion in instrucciones)
            {
                instruccion.optimizar();
            }
        }

        public void instrucciones(ParseTreeNode nodoRaiz, ref LinkedList<Optimizar> instrs)
        {

            foreach(ParseTreeNode nodo in nodoRaiz.ChildNodes)
            {

                

                if (nodo.Term.ToString() != "INSTRUCCION")
                {
                    if (nodo.ChildNodes.Count > 0 && nodo.ChildNodes[0].Term.ToString() == "METODO")
                    {
                        instrs.AddLast(instruccion(nodo.ChildNodes[0]));
                    }else if (nodo.Term.ToString() == "ENCABEZADO")
                    {
                        armarEncabezado(nodo);
                    }
                    else
                    {
                        instrucciones(nodo, ref instrs);
                    }
                }
                else
                {
                    instrs.AddLast(instruccion(nodo.ChildNodes[0]));
                }
                
            }
        }
 
        public Optimizar instruccion(ParseTreeNode actual)
        {
            Debug.WriteLine("Instruccion.. " + actual.Term.ToString());
            switch (actual.Term.ToString())
            {
                case "LLAMADA":
                    return evaluarLlamada(actual);
                case "ASIGNACION":
                    return evaluarAsignacion(actual);
                case "SALTOS":
                    return evaluarSaltos(actual.ChildNodes[0]);
                case "PRINT":
                    return evaluarPrint(actual);
                case "OPERACION":
                    return evaluarOperacion(actual);
                case "ETIQUETA":
                    return evaluarEtiqueta(actual);
                case "METODO":
                    return evaluarMetodo(actual);
            }
            return null;
        }

        public Optimizar evaluarMetodo(ParseTreeNode actual)
        {
            switch (actual.ChildNodes.Count)
            {
                case 8:
                    return new Metodo(actual.ChildNodes[1].Token.Text,null);
                default:
                    LinkedList<Optimizar> instrucciones = new LinkedList<Optimizar>();
                    this.instrucciones(actual.ChildNodes[5], ref instrucciones);
                    return new Metodo(actual.ChildNodes[1].Token.Text,instrucciones);
            }
        }

        public Optimizar evaluarLlamada(ParseTreeNode actual)
        {
            return new Llamada(actual.ChildNodes[0].Token.Text);
        }

        public Optimizar evaluarAsignacion(ParseTreeNode actual)
        {
            string linea = actual.ChildNodes[1].Token.Location.Line.ToString();
            switch (actual.ChildNodes.Count)
            {
                case 10:
                    switch (actual.ChildNodes[0].Term.ToString())
                    {
                        case "VARIABLE":
                            return new Asignacion(actual.ChildNodes[0].ChildNodes[0].Token.Text,new Valor(actual.ChildNodes[2].ChildNodes[0].Token.Text+"[(int)"+getValor(actual.ChildNodes[7])+"]",false),true,linea);
                        default:
                            return new Asignacion(actual.ChildNodes[0].ChildNodes[0].Token.Text+"[(int)"+getValor(actual.ChildNodes[5])+"]",VALOR(actual.ChildNodes[8]),true,linea);
                    }
                    
                default:
                    return new Asignacion(actual.ChildNodes[0].ChildNodes[0].Token.Text,VALOR(actual.ChildNodes[2]),false,linea);
            }
        }

        public Optimizar evaluarSaltos(ParseTreeNode actual)
        {
            switch (actual.Term.ToString())
            {
                case "CONDICIONAL":
                    //actual = actual.ChildNodes[0];
                    if (actual.ChildNodes.Count == 9)
                        return new Condicional(VALOR(actual.ChildNodes[2]),VALOR(actual.ChildNodes[4]),actual.ChildNodes[3].ChildNodes[0].Token.Text,actual.ChildNodes[7].Token.Text,null,actual.ChildNodes[7].Token.Location.Line.ToString());
                    return new Condicional(VALOR(actual.ChildNodes[2]), VALOR(actual.ChildNodes[4]), actual.ChildNodes[3].ChildNodes[0].Token.Text, actual.ChildNodes[7].Token.Text, actual.ChildNodes[9].ChildNodes[1].Token.Text, actual.ChildNodes[7].Token.Location.Line.ToString());
                case "INCONDICIONAL":
                    //actual = actual.ChildNodes[0];
                    return new NoCondicional(actual.ChildNodes[1].Token.Text,actual.ChildNodes[1].Token.Location.Line.ToString(),false);
            }
            return null;
        }

        public Optimizar evaluarPrint(ParseTreeNode actual)
        {
            switch (actual.ChildNodes.Count)
            {
                case 7:
                    return new Print(actual.ChildNodes[2].Token.Text, VALOR(actual.ChildNodes[4]),true);
                default:
                    return new Print(actual.ChildNodes[2].ChildNodes[0].Token.Text, VALOR(actual.ChildNodes[7]),false);
            }
        }

        public Optimizar evaluarOperacion(ParseTreeNode actual)
        {
            ParseTreeNode siguiente = actual.ChildNodes[0];
            return new Operacion(siguiente.ChildNodes[0].Token.Text,VALOR(actual.ChildNodes[2]),VALOR(actual.ChildNodes[4]),actual.ChildNodes[3].Token.Text,actual.ChildNodes[3].Token.Location.Line.ToString());
        }

        public Optimizar evaluarEtiqueta(ParseTreeNode actual)
        {
            return new Etiqueta(actual.ChildNodes[0].Token.Text);
        }

        public Evaluar VALOR(ParseTreeNode actual)
        {
            switch (actual.ChildNodes.Count)
            {
                case 1:
                    switch (actual.ChildNodes[0].Term.ToString())
                    {
                        case "INT":
                            return new Valor(actual.ChildNodes[0].Token.Text,true);
                        case "DOUBLE":
                            return new Valor(actual.ChildNodes[0].Token.Text, true);
                        default: //VARIABLE
                            return new Valor(actual.ChildNodes[0].ChildNodes[0].Token.Text, false);
                    }
                case 2:
                    switch (actual.ChildNodes[1].Term.ToString())
                    {
                        case "INT":
                            return new Valor("-"+actual.ChildNodes[1].Token.Text, true);
                        case "DOUBLE":
                            return new Valor("-"+actual.ChildNodes[1].Token.Text, true);
                        default: //VARIABLE
                            return new Valor("-"+actual.ChildNodes[1].ChildNodes[0].Token.Text, false);
                    }
            }
            return null;
        }

        public string getValor(ParseTreeNode actual)
        {
            switch (actual.ChildNodes.Count)
            {
                case 1:
                    switch (actual.ChildNodes[0].Term.ToString())
                    {
                        case "INT":
                            return actual.ChildNodes[0].Token.Text;
                        case "DOUBLE":
                            return actual.ChildNodes[0].Token.Text;
                        default: //VARIABLE
                            return actual.ChildNodes[0].ChildNodes[0].Token.Text;
                    }
                case 2:
                    switch (actual.ChildNodes[1].Term.ToString())
                    {
                        case "INT":
                            return "-" + actual.ChildNodes[0].Token.Text;
                        case "DOUBLE":
                            return "-" + actual.ChildNodes[0].Token.Text;
                        default: //VARIABLE
                            return "-" + actual.ChildNodes[0].ChildNodes[0].Token.Text;
                    }
            }
            return null;
        }
        
        public void armarEncabezado(ParseTreeNode actual)
        {
            Generador generador = Generador.getInstance();
            string codigo = "";
            foreach (ParseTreeNode elemento in actual.ChildNodes)
            {
                switch (elemento.Term.ToString())
                {
                    case "VARIABLE":
                        codigo += elemento.ChildNodes[0].Token.Text;
                        break;
                    case "TEMPORALES":
                        codigo += getTemporales(elemento);
                        break;
                    case "ESTRUCTURA":
                        codigo += elemento.ChildNodes[0].Token.Text;
                        break;
                    case "#":
                    case "ID":
                    case "<":
                        codigo += elemento.Token.Text + "";
                        break;
                    case ">":
                        generador.addCode(codigo + ">");
                        codigo = "";
                        break;
                    case ";":
                        generador.addCode(codigo + ";");
                        codigo = "";
                        break;
                    case ".":
                        codigo += elemento.Token.Text + "";
                        break;
                    default:
                        codigo += elemento.Token.Text + " ";
                        break;
                }
            }
        }

        public string getTemporales(ParseTreeNode actual)
        {
            switch (actual.ChildNodes.Count)
            {
                case 3:
                    return getTemporales(actual.ChildNodes[0]) + "," + actual.ChildNodes[2].Token.Text;
                default:
                    return actual.ChildNodes[0].Token.Text;
            }
        }

    }
}
