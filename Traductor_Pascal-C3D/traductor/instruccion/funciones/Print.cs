using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.instruccion;
using Traductor_Pascal_C3D.traductor.reportes;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.generador;

namespace Traductor_Pascal_C3D.traductor.instruccion.funciones
{
    class Print : Instruccion
    {
        private Expresion value;
        private bool isLine;

        public Print(Expresion value, bool isLine, int line, int column):base(line,column)
        {
            this.value = value;
            this.isLine = isLine;
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            Retorno value = this.value.compile(entorno);
            Generador generador = Generador.getInstance();
            switch(value.type.type)
            {
                case Types.NUMBER:
                    generador.addPrint('d',value.getValue());
                    break;
                case Types.DOUBLE:
                    generador.addPrint('f',value.getValue());
                    break;
                case Types.BOOLEAN:
                    string templabel = generador.newLabel();
                    generador.addLabel(value.trueLabel);
                    generador.addPrintTrue();
                    generador.addGoto(templabel);
                    generador.addLabel(value.falseLabel);
                    generador.addPrintFalse();
                    generador.addLabel(templabel);
                    break;
                case Types.STRING:
                    generador.addNextEnv(entorno.size);
                    generador.addSetStack("p", value.getValue());
                    generador.addExpression("T0", value.getValue(),"","");
                    generador.addCall("native_print_str");
                    generador.addAntEnv(entorno.size);
                    break;
            }

            if(this.isLine)
            {
                generador.addPrint('c',"10");
            }

            return null;
            //throw new NotImplementedException();
        }
    }
}
