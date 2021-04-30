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
using Traductor_Pascal_C3D.traductor.expresion.aritmetica;
using Traductor_Pascal_C3D.traductor.expresion.literal;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.expresion.relacional;
using Traductor_Pascal_C3D.traductor.instruccion.control;
using Traductor_Pascal_C3D.traductor.variables;
using Type = Traductor_Pascal_C3D.traductor.utils.Type;
using Traductor_Pascal_C3D.traductor.expresion.access;
using Traductor_Pascal_C3D.traductor.expresion.assignment;
using Traductor_Pascal_C3D.traductor.instruccion.transferencia;

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
                        return new Print(evaluarExpresionCadena(actual.ChildNodes[0].ChildNodes[2], actual.ChildNodes[0].ChildNodes[3]), true,actual.ChildNodes[0].ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].ChildNodes[0].Token.Location.Column);
                    return new Print(evaluarExpresionCadena(actual.ChildNodes[0].ChildNodes[2], actual.ChildNodes[0].ChildNodes[3]), false, actual.ChildNodes[0].ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].ChildNodes[0].Token.Location.Column);
                case "Variables":
                    LinkedList<Instruccion> listaDeclaraciones = new LinkedList<Instruccion>();
                    evaluarVarConst(actual.ChildNodes[0],ref listaDeclaraciones);
                    return new DeclararVariables(listaDeclaraciones,0,0);
                case "Types":
                case "Asignacion":
                    return nuevaAsignacion(actual.ChildNodes[0],0,0);
                case "If_Statement":
                    return evaluarIf(actual.ChildNodes[0]);
                case "For_Statement":
                    actual = actual.ChildNodes[0];
                    return new For(actual.ChildNodes[5].Token.Text, evaluarExpresionNumerica(actual.ChildNodes[4]), evaluarExpresionNumerica(actual.ChildNodes[6]), instrucciones(actual.ChildNodes[9]), actual.ChildNodes[1].Token.Text, actual.ChildNodes[3].Token.Location.Line, actual.ChildNodes[3].Token.Location.Column);
                case "While_Statement":
                    return evaluarWhile(actual.ChildNodes[0],0,0);
                case "Repeat_Statement":
                    actual = actual.ChildNodes[0];
                    return new Repeat(evaluarExpresionLogica(actual.ChildNodes[3]), instrucciones(actual.ChildNodes[1]), 0, 0);
                case "Case_Statement":
                    return evaluarCase(actual.ChildNodes[0]);
                case "Funcion":
                    return evaluarFuncion(actual.ChildNodes[0]);
                case "Procedimiento":
                    return evaluarProcedimiento(actual.ChildNodes[0]);
                case "Llamada":
                    return evaluarNuevaLlamada(actual.ChildNodes[0]);
                case "break":
                    return new Break(actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
                case "continue":
                    return new Continue(actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
                case "exit":
                    if (actual.ChildNodes.Count == 5)
                        return new Return(actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column, expresionCadena(actual.ChildNodes[2]));
                    return new Return(actual.Token.Location.Line, actual.Token.Location.Column);
                case "graficar_ts":
                    return null;


            }

            return null;
        }

        /***************************     Funciones    *******************************/

        public FunctionSt evaluarFuncion(ParseTreeNode actual)
        {
            Dictionary<string, Instruccion> paramsValor = new Dictionary<string, Instruccion>();
            Dictionary<string, Instruccion> paramsRef = new Dictionary<string, Instruccion>();
            Dictionary<int, string> orden = new Dictionary<int, string>();
            LinkedList<Type> paramsTipos = new LinkedList<Type>();

            LinkedList<Param> @params = new LinkedList<Param>();

            int fila = actual.ChildNodes[1].Token.Location.Line;
            int columna = actual.ChildNodes[1].Token.Location.Column;
            switch (actual.ChildNodes.Count)
            {
                case 13:
                    parametrosFuncion(actual.ChildNodes[3], ref paramsValor, ref paramsRef, ref paramsTipos, ref orden, 1,ref @params);
                    return crearFuncion(actual.ChildNodes[1].Token.Text, getTipo(actual.ChildNodes[6]), paramsValor, paramsRef, instrucciones(actual.ChildNodes[8]), instrucciones(actual.ChildNodes[10]), paramsTipos, orden,@params, fila, columna);//NuevaFuncion(actual.ChildNodes[1].Token.Text, crearFuncion(actual.ChildNodes[1].Token.Text, getTipo(actual.ChildNodes[6]), paramsValor, paramsRef, instrucciones(actual.ChildNodes[8]), instrucciones(actual.ChildNodes[10]), paramsTipos, orden, fila, columna), fila, columna);
                default:
                    return crearFuncion(actual.ChildNodes[1].Token.Text, getTipo(actual.ChildNodes[3]), paramsValor, paramsRef, instrucciones(actual.ChildNodes[5]), instrucciones(actual.ChildNodes[7]), paramsTipos, orden, @params, fila, columna);
            }
        }

        public FunctionSt crearFuncion(string nombre, Type tipo, Dictionary<string, Instruccion> paramsValor, Dictionary<string, Instruccion> paramsRef, LinkedList<Instruccion> head, LinkedList<Instruccion> body, LinkedList<Type> paramsTipos, Dictionary<int, string> orden, LinkedList<Param> @params, int fila, int columna)
        {

            /*foreach (Instruccion instruccion in body)
            {
                head.AddLast(instruccion);
            }*/
            return new FunctionSt(nombre, @params, tipo, head, body, true, 0, 0);
            //return new FunctionSt(nombre, tipo, paramsValor, paramsRef, head, paramsTipos, orden, fila, columna);
        }

        public LinkedList<Param> parametrosFuncion(ParseTreeNode actual, ref Dictionary<string, Instruccion> paramsValor, ref Dictionary<string, Instruccion> paramsRef, ref LinkedList<Type> paramsTipos, ref Dictionary<int, string> orden, int pos,ref LinkedList<Param> @params)
        {

            switch (actual.ChildNodes.Count)
            {
                case 4:
                    LinkedList<string> ids = new LinkedList<string>();
                    listaVariables(actual.ChildNodes[0], ids);
                    Type tipoVar = getTipo(actual.ChildNodes[2]);
                    foreach (string variable in ids)
                    {
                        /*paramsValor.Add(variable, new NuevaDeclaracion(null, variable, tipoVar, true, actual.ChildNodes[1].Token.Location.Line, actual.ChildNodes[1].Token.Location.Column));
                        orden.Add(pos, variable);
                        paramsTipos.AddLast(tipoVar);
                        pos++;*/
                        @params.AddLast(new Param(variable, tipoVar));
                    }
                    if (actual.ChildNodes[3].ChildNodes.Count == 0)
                        return null;
                    parametrosFuncion(actual.ChildNodes[3].ChildNodes[1], ref paramsValor, ref paramsRef, ref paramsTipos, ref orden, pos,ref @params);
                    return null;
                case 5:
                    /*paramsRef.Add(actual.ChildNodes[1].Token.Text, new NuevaDeclaracion(null, actual.ChildNodes[1].Token.Text, getTipo(actual.ChildNodes[3]), true, actual.ChildNodes[1].Token.Location.Line, actual.ChildNodes[1].Token.Location.Column));
                    paramsTipos.AddLast(getTipo(actual.ChildNodes[3]));
                    orden.Add(pos, actual.ChildNodes[1].Token.Text);*/
                    @params.AddLast(new Param(actual.ChildNodes[1].Token.Text, getTipo(actual.ChildNodes[3]),true));
                    if (actual.ChildNodes[4].ChildNodes.Count == 0)
                        return null;
                    parametrosFuncion(actual.ChildNodes[4].ChildNodes[1], ref paramsValor, ref paramsRef, ref paramsTipos, ref orden, pos + 1,ref @params);
                    return null;

                default:
                    return null;
            }
        }

        public Instruccion evaluarNuevaLlamada(ParseTreeNode actual)
        {
            int line = actual.ChildNodes[0].Token.Location.Line;
            int column = actual.ChildNodes[0].Token.Location.Column;

            LinkedList<Expresion> valores = new LinkedList<Expresion>();
            entradaFuncion(actual.ChildNodes[2], ref valores);
            return new Llamada(new AssignmentFunc(actual.ChildNodes[0].Token.Text,null,valores,line,column),line,column);

        }


        public void entradaFuncion(ParseTreeNode actual, ref LinkedList<Expresion> valores)
        {

            switch (actual.ChildNodes.Count)
            {
                case 3:
                    valores.AddLast(expresionCadena(actual.ChildNodes[0]));
                    entradaFuncion(actual.ChildNodes[2], ref valores);
                    return;
                case 1:
                    if (actual.ChildNodes[0].Term.ToString() != "Expresion_Cadena")
                        return;
                    valores.AddLast(expresionCadena(actual.ChildNodes[0]));
                    return;
            }

            return;
        }

        public void entradFuncionp(ParseTreeNode actual, ref LinkedList<Expresion> valores)
        {
            switch (actual.ChildNodes.Count)
            {
                case 1: return;

                default:
                    entradaFuncion(actual.ChildNodes[1], ref valores);
                    break;
            }
        }

        public FunctionSt evaluarProcedimiento(ParseTreeNode actual)
        {
            Dictionary<string, Instruccion> paramsValor = new Dictionary<string, Instruccion>();
            Dictionary<string, Instruccion> paramsRef = new Dictionary<string, Instruccion>();
            Dictionary<int, string> orden = new Dictionary<int, string>();
            LinkedList<Type> paramsTipos = new LinkedList<Type>();
            LinkedList<Param> @params = new LinkedList<Param>();

            switch (actual.ChildNodes.Count)
            {
                case 11:
                    parametrosFuncion(actual.ChildNodes[3], ref paramsValor, ref paramsRef, ref paramsTipos, ref orden, 1,ref @params);
                    return crearProcedimiento(actual.ChildNodes[1].Token.Text, paramsValor, paramsRef, instrucciones(actual.ChildNodes[6]), instrucciones(actual.ChildNodes[8]), paramsTipos, orden, @params);//NuevoProcedimiento(actual.ChildNodes[1].Token.Text, crearProcedimiento(actual.ChildNodes[1].Token.Text, paramsValor, paramsRef, instrucciones(actual.ChildNodes[6]), instrucciones(actual.ChildNodes[8]), paramsTipos, orden), actual.ChildNodes[1].Token.Location.Line, actual.ChildNodes[1].Token.Location.Column);
                default:
                    return crearProcedimiento(actual.ChildNodes[0].Token.Text,paramsValor,paramsRef, instrucciones(actual.ChildNodes[3]), instrucciones(actual.ChildNodes[5]),paramsTipos,orden,@params); //NuevoProcedimiento(actual.ChildNodes[1].Token.Text, crearProcedimiento(actual.ChildNodes[1].Token.Text, paramsValor, paramsRef, instrucciones(actual.ChildNodes[3]), instrucciones(actual.ChildNodes[5]), paramsTipos, orden), actual.ChildNodes[1].Token.Location.Line, actual.ChildNodes[1].Token.Location.Column);
            }
        }

        public FunctionSt crearProcedimiento(string nombre, Dictionary<string, Instruccion> paramsValor, Dictionary<string, Instruccion> paramsRef, LinkedList<Instruccion> head, LinkedList<Instruccion> body, LinkedList<Type> paramsTipos, Dictionary<int, string> orden,LinkedList<Param> @params)
        {
            return new FunctionSt(nombre, @params, new Type(Types.NULLL, ""), head, body, true, 0, 0);
        }


        /* *************************     Variables    ******************************/

        public void evaluarVarConst(ParseTreeNode actual, ref LinkedList<Instruccion> listaDeclaraciones)
        {
            if (actual.ChildNodes[0].Term.ToString() == "var")
            {
                evaluarVariable(actual.ChildNodes[1], ref listaDeclaraciones, true); //Es variable
            }
            else
            {
                evaluarVariable(actual.ChildNodes[1], ref listaDeclaraciones, false); //Es constante
            }

        }

        public LinkedList<Instruccion> evaluarVariable(ParseTreeNode actual, ref LinkedList<Instruccion> listaDeclaraciones, bool isVariable)
        {
            Debug.WriteLine("nodo -> " + actual.Term.ToString());
            //Estoy en var Nueva_Asignacion_variable
            if (actual.ChildNodes.Count == 0)
                return null;


            //Ir a Asignacion Variable
            listaDeclaraciones.AddLast(declaracionVariable(actual.ChildNodes[0], ref listaDeclaraciones, isVariable));

            if (actual.ChildNodes[1].ChildNodes.Count != 0)
                evaluarVariable(actual.ChildNodes[1].ChildNodes[0], ref listaDeclaraciones, isVariable);
            return null;
        }

        public Instruccion declaracionVariable(ParseTreeNode actual, ref LinkedList<Instruccion> listaDeclaraciones, bool isVariable)
        {
            // Estoy en ID : Tipo ......
            int cantidad = actual.ChildNodes.Count;
            int linea = actual.ChildNodes[0].Token.Location.Line;
            int columna = actual.ChildNodes[0].Token.Location.Column;
            LinkedList<string> ids = new LinkedList<string>();

            switch (cantidad)
            {
                case 4:
                    ids.AddLast(actual.ChildNodes[0].Token.Text);
                    listaDeclaraciones.AddLast(new Declaracion(getTipo(actual.ChildNodes[2]), ids, null,linea,columna));
                    break;
                case 6:
                    if (actual.ChildNodes[1].Token.Text != ",")
                    {
                        ids.AddLast(actual.ChildNodes[0].Token.Text);
                        listaDeclaraciones.AddLast(new Declaracion(getTipo(actual.ChildNodes[2]), ids, expresionCadena(actual.ChildNodes[4]),linea,columna));
                    }
                    else
                    {
                        //Tiene ,
                        listaDeclaraciones = variosIds(actual.ChildNodes[2], actual.ChildNodes[0].Token.Text, getTipo(actual.ChildNodes[4]), ref listaDeclaraciones, isVariable, actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
                    }
                    break;
                case 9:
                    /*LinkedList<Dictionary<string, int>> diccionarios = new LinkedList<Dictionary<string, int>>();
                    getDimensiones(actual.ChildNodes[4], ref diccionarios);
                    listaDeclaraciones.AddLast(new NuevoArreglo(actual.ChildNodes[0].Token.Text, diccionarios, getTipo(actual.ChildNodes[7]), actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column));
                    */break;
            }
            return null;

        }

        public Instruccion nuevaAsignacion(ParseTreeNode actual,int line, int column)
        {
            return new Asignacion(expresionCadena(actual.ChildNodes[0]), expresionCadena(actual.ChildNodes[3]),line,column);
            /*switch (actual.ChildNodes.Count)
            {
                case 7:
                    return new Asignacion(expresionCadena(actual.ChildNodes[0]), actual.ChildNodes[2].Token.Text, expresionCadena(actual.ChildNodes[5]));
                default:
                    switch (actual.ChildNodes[0].Term.ToString())
                    {
                        case "ID":
                            return new NuevaAsignacion(actual.ChildNodes[0].Token.Text, expresionCadena(actual.ChildNodes[3]));
                        case "Valor_Arreglo":
                            Expresion expresion = expresionCadena(actual.ChildNodes[3]);
                            actual = actual.ChildNodes[0];
                            LinkedList<Expresion> indices = new LinkedList<Expresion>();
                            return new AsignacionArreglo(actual.ChildNodes[0].Token.Text, getIndicesArray(actual.ChildNodes[2], indices), expresion);
                        default:
                            return null;
                    }
            }*/
        }

        public LinkedList<Instruccion> variosIds(ParseTreeNode actual, string id, Type tipo, ref LinkedList<Instruccion> listaDeclaraciones, bool isVariable, int linea, int columna)
        {
            // listaDeclaraciones.AddLast(NuevaDeclaracion(expresionCadena(actual.ChildNodes)))
            LinkedList<string> variables = listaVariables(actual, new LinkedList<string>());

            /*listaDeclaraciones.AddLast(new NuevaDeclaracion(null, id, tipo, isVariable, linea, columna));
            lista
            listaDeclaraciones.AddLast(new Declaracion(tipo,))

            foreach (string identificador in variables)
            {
                listaDeclaraciones.AddLast(new NuevaDeclaracion(null, identificador, tipo, isVariable, linea, columna));
            }*/
            variables.AddLast(id);
            listaDeclaraciones.AddLast(new Declaracion(tipo, variables, null, linea, columna));

            return listaDeclaraciones;
        }

        public LinkedList<string> listaVariables(ParseTreeNode actual, LinkedList<string> lista)
        {
            Debug.WriteLine(actual.Term.ToString());
            switch (actual.ChildNodes.Count)
            {
                case 2:
                    lista.AddLast(actual.ChildNodes[0].Token.Text);
                    if (actual.ChildNodes[1].ChildNodes.Count == 0)
                        return lista;
                    return listaVariables(actual.ChildNodes[1], lista);
                default: // 3
                    lista.AddLast(actual.ChildNodes[1].Token.Text);
                    if (actual.ChildNodes[2].ChildNodes.Count == 0)
                        return lista;
                    return listaVariables(actual.ChildNodes[2], lista);
            }
        }


        /***************************     Estructuras de control   ********************/

        public Instruccion evaluarIf(ParseTreeNode actual)
        {
            LinkedList<Instruccion> instruccionSimple = new LinkedList<Instruccion>();
            int cantidad = actual.ChildNodes.Count;
            int line = actual.ChildNodes[0].Token.Location.Line;
            int column = actual.ChildNodes[0].Token.Location.Column;
            switch (cantidad)
            {
                case 10:
                    return new If(evaluarExpresionLogica(actual.ChildNodes[2]), instrucciones(actual.ChildNodes[6]), evaluarElse(actual.ChildNodes[8]), line, column);
                case 8:
                    return new If(evaluarExpresionLogica(actual.ChildNodes[1]), instrucciones(actual.ChildNodes[4]), evaluarElse(actual.ChildNodes[6]), line, column);
                case 7:
                    instruccionSimple.AddLast(instruccion(actual.ChildNodes[5]));
                    return new If(evaluarExpresionLogica(actual.ChildNodes[2]), instruccionSimple, evaluarElse(actual.ChildNodes[6]), line, column);
                default:
                    instruccionSimple.AddLast(instruccion(actual.ChildNodes[3]));
                    return new If(evaluarExpresionLogica(actual.ChildNodes[1]), instruccionSimple, evaluarElse(actual.ChildNodes[4]), line, column);
            }
        }

        public LinkedList<Instruccion> evaluarElse(ParseTreeNode actual)
        {
            Debug.WriteLine("No Terminal Else " + actual.Term.ToString());
            if (actual.ChildNodes.Count == 0)
                return null;
            int cantidad = actual.ChildNodes.Count;
            switch (cantidad)
            {
                case 4:
                    return instrucciones(actual.ChildNodes[2]);
                default:
                    // 1 Instruccion
                    LinkedList<Instruccion> instruccion = new LinkedList<Instruccion>();
                    instruccion.AddLast(this.instruccion(actual.ChildNodes[1]));
                    return instruccion;
            }
        }

        public Instruccion evaluarWhile(ParseTreeNode actual,int linea,int columna)
        {
            switch (actual.ChildNodes.Count)
            {
                case 7:
                    return new While(evaluarExpresionLogica(actual.ChildNodes[1]), instrucciones(actual.ChildNodes[4]),linea,columna);
                default:
                    //9
                    return new While(evaluarExpresionLogica(actual.ChildNodes[2]), instrucciones(actual.ChildNodes[6]),linea,columna);
            }
        }

        public Instruccion evaluarCase(ParseTreeNode actual)
        {
            Dictionary<Expresion, LinkedList<Instruccion>> casos = new Dictionary<Expresion, LinkedList<Instruccion>>();
            switch (actual.ChildNodes.Count)
            {
                case 8:
                    return cases(expresionCadena(actual.ChildNodes[2]), casos, actual.ChildNodes[5]);
                default:
                    //Son 6
                    return cases(expresionCadena(actual.ChildNodes[1]), casos, actual.ChildNodes[3]);
            }
        }

        public Case cases(Expresion valor, Dictionary<Expresion, LinkedList<Instruccion>> casos, ParseTreeNode actual)
        {

            switch (actual.ChildNodes.Count)
            {
                case 4:
                    LinkedList<Instruccion> instr = new LinkedList<Instruccion>();
                    instr.AddLast(instruccion(actual.ChildNodes[2]));
                    casos.Add(expresionCadena(actual.ChildNodes[0]), instr);
                    return casePrima(valor, casos, actual.ChildNodes[3]);
                default:
                    casos.Add(expresionCadena(actual.ChildNodes[0]), instrucciones(actual.ChildNodes[3]));
                    return casePrima(valor, casos, actual.ChildNodes[6]);
            }
        }

        public Case casePrima(Expresion valor, Dictionary<Expresion, LinkedList<Instruccion>> casos, ParseTreeNode actual)
        {

            if (actual.ChildNodes.Count == 0)
                return new Case(casos,valor, new LinkedList<Instruccion>(),0,0);

            switch (actual.ChildNodes[0].Term.ToString())
            {
                case "Cases_Statement":
                    return cases(valor, casos, actual.ChildNodes[0]);
                case "Case_Else_Statement":
                    return new Case(casos,valor, CaseElse(actual.ChildNodes[0]),0,0);
            }
            return null;
        }

        public LinkedList<Instruccion> CaseElse(ParseTreeNode actual)
        {
            switch (actual.ChildNodes.Count)
            {
                case 5:
                    return instrucciones(actual.ChildNodes[2]);
                default: // 2
                    LinkedList<Instruccion> instr = new LinkedList<Instruccion>();
                    instr.AddLast(instruccion(actual.ChildNodes[1]));
                    return instr;
            }
        }




        /***************************     Expresiones   **********************/
        public Expresion evaluarExpresionCadena(ParseTreeNode expresionCadena, ParseTreeNode masTexto)
        {
            Expresion ExpresionCadena = null;
            ExpresionCadena = this.expresionCadena(expresionCadena);
            Expresion MasTexto = null;
            MasTexto = this.masTexto(masTexto);

            if (MasTexto != null)
            {
                return new Aritmetica(ExpresionCadena,MasTexto,'+',expresionCadena.Token.Location.Line,expresionCadena.Token.Location.Column); //retornar algo
            }
            else
            {
                return ExpresionCadena;
            }
        }

        public Expresion evaluarExpresionNumerica(ParseTreeNode actual)
        {
            int line = 0;
            int column = 0;
            if (actual.ChildNodes.Count == 3)
            {
                string operador = actual.ChildNodes[1].Token.Text;
                line = actual.ChildNodes[1].Token.Location.Line;
                column = actual.ChildNodes[1].Token.Location.Column;
                switch (operador.ToLower())
                {
                    case "+":
                        return new Aritmetica(evaluarExpresionNumerica(actual.ChildNodes[0]), evaluarExpresionNumerica(actual.ChildNodes[2]), '+',line,column);
                    case "-":
                        return new Aritmetica(evaluarExpresionNumerica(actual.ChildNodes[0]), evaluarExpresionNumerica(actual.ChildNodes[2]), '-',line,column);
                    case "*":
                        return new Aritmetica(evaluarExpresionNumerica(actual.ChildNodes[0]), evaluarExpresionNumerica(actual.ChildNodes[2]), '*',line,column);
                    case "/":
                        return new Aritmetica(evaluarExpresionNumerica(actual.ChildNodes[0]), evaluarExpresionNumerica(actual.ChildNodes[2]), '/',line,column);
                    case "div":
                        return new Aritmetica(evaluarExpresionNumerica(actual.ChildNodes[0]), evaluarExpresionNumerica(actual.ChildNodes[2]), 'd',line,column);
                    case ".":
                        /*Falta Codigo*/
                        return null;
                    default:
                        return new Aritmetica(evaluarExpresionNumerica(actual.ChildNodes[0]), evaluarExpresionNumerica(actual.ChildNodes[2]), '%', line,column);
                }
            }
            else
            {
                Debug.WriteLine(actual.ChildNodes[0].Term.ToString());
                //Verificar el tipo y no solo poner la n porque pueden venir strings o bools
                switch (actual.ChildNodes[0].Term.ToString())
                {
                    case "DOUBLE":
                        //return new Literal('D', actual.ChildNodes[0].Token.Text);
                        line = actual.ChildNodes[0].Token.Location.Line;
                        column = actual.ChildNodes[0].Token.Location.Column;
                        return new Primitivo(Types.DOUBLE, actual.ChildNodes[0].Token.Text,line,column);

                    case "ID":
                        //return new ObtenerVariable(actual.ChildNodes[0].Token.Text);
                        return new AccessId(actual.ChildNodes[0].Token.Text, null, line, column);
                    case "Llamada":
                        //return new ObtenerLlamada(evaluarNuevaLlamada(actual.ChildNodes[0]));
                        actual = actual.ChildNodes[0];
                        LinkedList<Expresion> @params = new LinkedList<Expresion>();
                        entradaFuncion(actual.ChildNodes[2], ref @params);
                        return new AssignmentFunc(actual.ChildNodes[0].Token.Text, null, @params, 0, 0);
                    case "Valor_Arreglo":
                        //actual = actual.ChildNodes[0];
                        //return new ObtenerArreglo(actual.ChildNodes[0].Token.Text, getIndicesArray(actual.ChildNodes[2], new LinkedList<Expresion>())); ;
                        return null;
                    case "true":
                        line = actual.ChildNodes[0].Token.Location.Line;
                        column = actual.ChildNodes[0].Token.Location.Column;
                        return new Primitivo(Types.BOOLEAN, "true", line, column);
                    case "false":
                        line = actual.ChildNodes[0].Token.Location.Line;
                        column = actual.ChildNodes[0].Token.Location.Column;
                        return new Primitivo(Types.BOOLEAN, "false", line, column);
                    default:
                        // Es INT
                        return new Primitivo(Types.NUMBER, actual.ChildNodes[0].Token.Text, line, column);
                }

            }
        }

        public Expresion expresionCadena(ParseTreeNode expresionCadena)
        {

            if(expresionCadena.ChildNodes.Count == 1)
            {
                if(expresionCadena.ChildNodes[0].Term.ToString() != "Expresion_Numerica")
                {
                    return new Primitivo(Literales(expresionCadena),expresionCadena.ChildNodes[0].Token.Text,expresionCadena.ChildNodes[0].Token.Location.Line,expresionCadena.ChildNodes[0].Token.Location.Column);
                }
                else
                {
                    return evaluarExpresionNumerica(expresionCadena.ChildNodes[0]);
                }
            }
            else
            {
                Expresion ExpresionCadena1 = null;
                Expresion ExpresionCadena2 = null;

                ExpresionCadena1 = this.expresionCadena(expresionCadena.ChildNodes[0]);
                ExpresionCadena2 = this.expresionCadena(expresionCadena.ChildNodes[2]);

                if (ExpresionCadena1 != null && ExpresionCadena2 != null)
                {
                    return new Aritmetica(ExpresionCadena1, ExpresionCadena2, getSignoAritmetica(expresionCadena.ChildNodes[1]), expresionCadena.Token.Location.Line, expresionCadena.Token.Location.Column);
                }
                else
                {
                    return ExpresionCadena1;
                }

            }
        }

        public Expresion masTexto(ParseTreeNode actual)
        {
            if (actual.ChildNodes.Count == 0)
            {
                return null;
            }
            else
            {
                //Tiene 3 hijos (, Expresion_Cadena Mas_Texto)
                Expresion expresionCadena = this.expresionCadena(actual.ChildNodes[1]);
                Expresion masTexto = this.masTexto(actual.ChildNodes[2]);

                if (expresionCadena != null && masTexto != null)
                {
                    return new Aritmetica(expresionCadena, masTexto, '+',actual.ChildNodes[0].Token.Location.Line,actual.ChildNodes[0].Token.Location.Column);
                }
                else
                {
                    return expresionCadena;
                }

            }
        }

        public Expresion evaluarExpresionLogica(ParseTreeNode actual)
        {
            int cantidad = actual.ChildNodes.Count;
            switch (cantidad)
            {
                case 3:
                    // Llevan Or o And
                    //return new RelacionalMultiple(evaluarExpresionLogica(actual.ChildNodes[0]), evaluarExpresionLogica(actual.ChildNodes[2]), actual.ChildNodes[1].Token.Text);
                    int line = actual.ChildNodes[1].Token.Location.Line;
                    int column = actual.ChildNodes[1].Token.Location.Column;
                    if (actual.ChildNodes[1].Token.Text.ToLower() == "and")
                        return new And(evaluarExpresionLogica(actual.ChildNodes[0]), evaluarExpresionLogica(actual.ChildNodes[2]), line, column);
                    return new Or(evaluarExpresionLogica(actual.ChildNodes[0]), evaluarExpresionLogica(actual.ChildNodes[2]), line, column);
                case 2:
                    // Tiene operador Not
                    //return new Not(evaluarExpresionLogica(actual.ChildNodes[1]));
                    return new Not(evaluarExpresionLogica(actual.ChildNodes[1]), actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
                case 1:
                    // Tiene Expresion Relacional
                    return evaluarExpresionRelacional(actual.ChildNodes[0]);
            }
            return null;
        }

        public Expresion evaluarExpresionRelacional(ParseTreeNode actual)
        {
            if (actual.ChildNodes.Count == 3)
            {
                string operador = actual.ChildNodes[1].Token.Text;
                int line = actual.ChildNodes[1].Token.Location.Line;
                int column = actual.ChildNodes[1].Token.Location.Column;
                if (actual.ChildNodes[0].Term.ToString() == "Expresion_Numerica")
                    switch(operador)
                    {
                        case "=":
                            return new Igualdad(evaluarExpresionNumerica(actual.ChildNodes[0]), evaluarExpresionNumerica(actual.ChildNodes[2]),line,column);
                        case ">":
                            return new Mayor(false, evaluarExpresionNumerica(actual.ChildNodes[0]), evaluarExpresionNumerica(actual.ChildNodes[2]), line, column);
                        case ">=":
                            return new Mayor(true, evaluarExpresionNumerica(actual.ChildNodes[0]), evaluarExpresionNumerica(actual.ChildNodes[2]), line, column);
                        case "<":
                            return new Menor(false, evaluarExpresionNumerica(actual.ChildNodes[0]), evaluarExpresionNumerica(actual.ChildNodes[2]), line, column);
                        case "<=":
                            return new Menor(true, evaluarExpresionNumerica(actual.ChildNodes[0]), evaluarExpresionNumerica(actual.ChildNodes[2]), line, column);
                        case "<>":
                            return new NoIgual(evaluarExpresionNumerica(actual.ChildNodes[0]), evaluarExpresionNumerica(actual.ChildNodes[2]), line, column);
                        default:
                            return null;
                    }
                switch (operador)
                {
                    case "=":
                        return new Igualdad(expresionCadena(actual.ChildNodes[0]), expresionCadena(actual.ChildNodes[2]), line, column);
                    case ">":
                        return new Mayor(false, expresionCadena(actual.ChildNodes[0]), expresionCadena(actual.ChildNodes[2]), line, column);
                    case ">=":
                        return new Mayor(true, expresionCadena(actual.ChildNodes[0]), expresionCadena(actual.ChildNodes[2]), line, column);
                    case "<":
                        return new Menor(false, expresionCadena(actual.ChildNodes[0]), expresionCadena(actual.ChildNodes[2]), line, column);
                    case "<=":
                        return new Menor(true, expresionCadena(actual.ChildNodes[0]), expresionCadena(actual.ChildNodes[2]), line, column);
                    case "<>":
                        return new NoIgual(expresionCadena(actual.ChildNodes[0]), expresionCadena(actual.ChildNodes[2]), line, column);
                        
                    default:
                        return null;
                }

            }
            else
            {
                //Buscar Identificador
                if (actual.ChildNodes.Count != 0)
                    if (actual.ChildNodes[0].Token != null)
                    {
                        //return new Relacional(new ObtenerVariable(actual.ChildNodes[0].Token.Text), null, "unica");
                        return null;
                    }
                    else
                    {
                        //Es una llamada
                        //return new Relacional(evaluarNuevaLlamada(actual.ChildNodes[0]), null, "unica");
                        return null;
                    }
                if (actual.Token.Text.ToLower() == "true")
                {
                    //return new Relacional(new Literal('T', true), null, "unica");
                    return null;
                }
                else
                {
                    //return new Relacional(new Literal('F', false), null, "unica");
                    return null;
                }
            }
        }

        public char getSignoAritmetica(ParseTreeNode actual)
        {
            switch (actual.Token.Text)
            {
                case "+":
                    return '+';
                case "-":
                    return '-';
                case "*":
                    return '*';
                case "/":
                    return '/';
                default:
                    return '%';
            }
        }
        
        public Type getTipo(ParseTreeNode actual)
        {
            Debug.WriteLine(actual.ChildNodes[0].Term.ToString());
            switch (actual.ChildNodes[0].Term.ToString())
            {
                case "integer":
                    return new traductor.utils.Type(Types.NUMBER, null);
                case "string":
                    return new Type(Types.STRING, null);
                case "boolean":
                    return new Type(Types.BOOLEAN, null);
                case "real":
                    return new Type(Types.DOUBLE, null);
                case "array":
                    return new Type(Types.ARRAY, null);
                case "ID":
                    return new Type(Types.TYPE, actual.ChildNodes[0].Token.Text);
                default:
                    return new Type(Types.NULLL, null);
            }
        }
        
        public Types Literales(ParseTreeNode actual)
        {
            switch (actual.ChildNodes[0].Term.ToString())
            {


                case "INT":
                    return Types.NUMBER;

                case "CADENA":
                    return Types.STRING;
                case "Array":
                    return Types.ARRAY;
                case "true":
                    return Types.BOOLEAN;
                case "false":
                    return Types.BOOLEAN;
            }
            return Types.NULLL;
        }



    }
}
