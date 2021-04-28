using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.reportes;
using Traductor_Pascal_C3D.traductor.generador;
using Traductor_Pascal_C3D.traductor.utils;

namespace Traductor_Pascal_C3D.traductor.instruccion.control
{
    class If : Instruccion
    {
        private Expresion codicion;
        private LinkedList<Instruccion> instruccion;
        private LinkedList<Instruccion> elseI = null;
        public If(Expresion condicion, LinkedList<Instruccion> instruccion, LinkedList<Instruccion> elseI,int line, int column):base(line,column)
        {
            this.codicion = condicion;
            this.instruccion = instruccion;
            this.elseI = elseI;
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            Generador generador = Generador.getInstance();
            generador.addComment("Inicia If");
            Retorno _condicion = this.codicion != null ? this.codicion.compile(entorno) :null;

            if(_condicion.type.type == Types.BOOLEAN)
            {
                generador.addLabel(_condicion.trueLabel);
                foreach(Instruccion instr in instruccion)
                {
                    instr.compile(entorno,reporte);
                }
                if(this.elseI != null)
                {
                    generador.addComment("Inicia Else");
                    string tempLbl = generador.newLabel();
                    generador.addGoto(tempLbl);
                    generador.addLabel(_condicion.falseLabel);
                    foreach(Instruccion instr in elseI)
                    {
                        instr.compile(entorno, reporte);
                    }
                    generador.addLabel(tempLbl);
                    generador.addComment("Termina Else");
                }
                else
                {
                    generador.addLabel(_condicion.falseLabel);
                }
                generador.addComment("Termina If");
            }
            return null;
            //throw new NotImplementedException();
        }
    }
}
