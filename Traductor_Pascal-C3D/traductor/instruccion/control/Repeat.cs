using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.generador;
using Traductor_Pascal_C3D.traductor.reportes;

namespace Traductor_Pascal_C3D.traductor.instruccion.control
{
    class Repeat : Instruccion
    {
        private Expresion condition;
        private LinkedList<Instruccion> instrucciones;

        public Repeat(Expresion condition, LinkedList<Instruccion> instrucciones, int line, int column):base(line,column)
        {
            this.condition = condition;
            this.instrucciones = instrucciones;
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            Generador generador = Generador.getInstance();
            generador.addComment("Inicia Repeat");
            entorno._continue = this.condition.falseLabel = generador.newLabel();
            entorno._break = this.condition.trueLabel = generador.newLabel();
            generador.addLabel(this.condition.falseLabel);
            foreach(Instruccion instruccion in instrucciones)
            {
                instruccion.compile(entorno, reporte);
            }
            Retorno condition = this.condition.compile(entorno);
            if(condition.type.type == Types.BOOLEAN)
            {
                generador.addLabel(condition.trueLabel);
                generador.addComment("Finaliza Repeat");
                return null;
            }
            throw new ErroPascal(this.line, this.column, "La condicion no es booleana", "Semantico");
        }
    }
}
