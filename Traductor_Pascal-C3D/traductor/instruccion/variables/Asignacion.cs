using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.expresion;
using Traductor_Pascal_C3D.traductor.generador;
using Traductor_Pascal_C3D.traductor.reportes;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.utils;

namespace Traductor_Pascal_C3D.traductor.instruccion.variables
{
    class Asignacion : Instruccion
    {

        private Expresion target;
        private Expresion value;

        public Asignacion(Expresion target, Expresion value, int line, int column):base(line,column)
        {
            this.target = target;
            this.value = value;
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            Retorno target = this.target.compile(entorno);
            Retorno value = this.value.compile(entorno);

            Generador generator = Generador.getInstance();
            Simbolo symbol = target.simbolo;

            if (!this.sameType(target.type, value.type))
                throw new ErroPascal(this.line, this.column, "Tipos de datos diferentes", "Semántico");

            if(symbol.isGlobal)
            {
                if (target.type.type == Types.BOOLEAN)
                {
                    string templabel = generator.newLabel();
                    generator.addLabel(value.trueLabel);
                    generator.addSetStack(symbol.position.ToString(), "1");
                    generator.addGoto(templabel);
                    generator.addLabel(value.falseLabel);
                    generator.addSetStack(symbol.position.ToString(), "0");
                    generator.addLabel(templabel);
                }
                else
                {
                    generator.addSetStack(symbol.position.ToString(), value.getValue());
                }
            }
            else if (symbol.isHeap)
            {
                if (target.type.type == Types.BOOLEAN)
                {
                    string templabel = generator.newLabel();
                    generator.addLabel(value.trueLabel);
                    generator.addSetHeap(symbol.position.ToString(), "1");
                    generator.addGoto(templabel);
                    generator.addLabel(value.falseLabel);
                    generator.addSetHeap(symbol.position.ToString(), "0");
                    generator.addLabel(templabel);
                }
                else
                {
                    generator.addSetHeap(target.getValue(), value.getValue());
                }
            }
            else
            {
                if (target.type.type == Types.BOOLEAN)
                {
                    string templabel = generator.newLabel();
                    generator.addLabel(value.trueLabel);
                    generator.addSetStack(target.getValue(), "1");
                    generator.addGoto(templabel);
                    generator.addLabel(value.falseLabel);
                    generator.addSetStack(target.getValue(), "0");
                    generator.addLabel(templabel);
                }
                else
                {
                    generator.addSetStack(target.getValue(), value.getValue());
                }
            }

            return null;;
        }
    }
}
