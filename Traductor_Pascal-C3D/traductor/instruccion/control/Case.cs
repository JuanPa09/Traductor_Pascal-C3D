using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.reportes;
using Traductor_Pascal_C3D.traductor.generador;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.expresion.relacional;

namespace Traductor_Pascal_C3D.traductor.instruccion.control
{
    class Case : Instruccion
    {
        Dictionary<Expresion, LinkedList<Instruccion>> casos;
        Expresion valor;
        LinkedList<Instruccion> caseDefalut;

        public Case(Dictionary<Expresion, LinkedList<Instruccion>> casos, Expresion valor, LinkedList<Instruccion> caseDefalut, int line, int column):base(line,column)
        {
            this.casos = casos;
            this.valor = valor;
            this.caseDefalut = caseDefalut;
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            Generador generador = Generador.getInstance();
            Retorno condicion = valor.compile(entorno);
            string salida = generador.newLabel();
            foreach(KeyValuePair<Expresion,LinkedList<Instruccion>> caso in casos)
            {
                /*string trueLabel = generador.newLabel();
                string falseLabel = generador.newLabel();

                Retorno _case = caso.Key.compile(entorno);
                generador.addIf(_case.getValue(), condicion.getValue(), "==",trueLabel);
                generador.addGoto(falseLabel);
                generador.addLabel(trueLabel);*/

                Expresion igualdad = new Igualdad(caso.Key, this.valor, this.line, this.column);
                Retorno retorno = igualdad.compile(entorno);
                generador.addLabel(retorno.trueLabel);
                //Instrucciones;
                foreach(Instruccion instruccion in caso.Value)
                {
                    try
                    {
                        instruccion.compile(entorno, reporte);
                    }catch(Exception ex) { ex.ToString(); }
                }
                generador.addGoto(salida);
                generador.addLabel(retorno.falseLabel);
                //generador.addLabel(falseLabel);
            }
            //Instrucciones Default
            foreach(Instruccion instruccion in caseDefalut)
            {
                try
                {
                    instruccion.compile(entorno, reporte);
                }catch(Exception ex) { ex.ToString(); }
            }
            generador.addLabel(salida);


            return null;
        }
    }
}
