using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.generador;
using Traductor_Pascal_C3D.traductor.reportes;
using System.Diagnostics;
using Traductor_Pascal_C3D.traductor.expresion.assignment;
using Traductor_Pascal_C3D.traductor.variables;
using Traductor_Pascal_C3D.traductor.expresion.literal;

namespace Traductor_Pascal_C3D.traductor.instruccion.control
{
    class For : Instruccion
    {
        string tipo;
        private Expresion valInicio;
        private Expresion valFinal;
        private LinkedList<Instruccion> instrucciones;
        private string id;

        public For(string tipo, Expresion valInicio, Expresion valFinal, LinkedList<Instruccion> instrucciones, string id, int linea, int columna) : base(linea,columna)
        {
            this.tipo = tipo;
            this.valInicio = valInicio;
            this.valFinal = valFinal;
            this.instrucciones = instrucciones;
            this.id = id;
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            try
            {
                Generador generador = Generador.getInstance();
                Retorno valorInicial = this.valInicio.compile(entorno);
                //Asignacion asignacion = new Asignacion(new AssignmentId(this.id, null, this.line, this.column),valInicio, this.line, this.column);
                //asignacion.compile(entorno,reporte);
                Retorno valorFinal = this.valFinal.compile(entorno);
                if(valorInicial.type.type != Types.NUMBER || valorFinal.type.type != Types.NUMBER)
                    throw new ErroPascal(0, 0, "No se puede evaluar la sentencia for porque \"" + valorInicial.value + "\" y/o \"" + valorFinal.value + "\" no coinciden con tipo numero", "Semantico");
                string temp = generador.newTemporal();
                string recursiva = generador.newLabel();
                string salida = generador.newLabel();
                string continueLbl = generador.newLabel();
                entorno.newBreak(salida);
                entorno.newContinue(continueLbl);
                Simbolo sym = entorno.getVar(id);//entorno.addVar(id, valorInicial.type, false, false);

                generador.addComment("Empieza For");
                generador.addSetStack(sym.position.ToString(), valorInicial.value);
                generador.addGetStack(temp, sym.position.ToString());
                generador.addLabel(recursiva);
                if(tipo.ToLower() == "to")
                {
                    generador.addIf(temp, valorFinal.value, ">", salida);
                }
                else
                {
                    generador.addIf(temp, valorFinal.value, "<", salida);
                }
                foreach(Instruccion instruccion in instrucciones)
                {
                    instruccion.compile(entorno, reporte);
                }
                generador.addLabel(continueLbl);
                generador.addGetStack(temp, sym.position.ToString());
                if (tipo.ToLower() == "to")
                {
                    generador.addExpression(temp, temp, "1", "+");
                }
                else
                {
                    generador.addExpression(temp, temp, "1", "-");
                }
                Asignacion asignacion = new Asignacion(new AssignmentId(id, null, this.line, this.column), new Primitivo(Types.NUMBER, temp, this.line, this.column), this.line, this.column);
                asignacion.compile(entorno, reporte);
                generador.addSetStack(sym.position.ToString(), temp);
                generador.addGoto(recursiva);
                generador.addLabel(salida);
                generador.addComment("Termina For");

            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }


            return null;

            /*
             
             
             dasdasd
            as
            da
            s
            d
            as
            d
            as
            da
             */
        }
    }
}
