using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.generador;

namespace Traductor_Pascal_C3D.traductor.expresion.relacional
{
    class Not : Expresion
    {
        private Expresion value;

        public Not(Expresion value, int line, int column):base(line,column)
        {
            this.value = value;
        }

        public override Retorno compile(Entorno entorno)
        {
            Generador generador = Generador.getInstance();
            this.trueLabel = this.trueLabel == "" ? generador.newLabel() : this.trueLabel;
            this.falseLabel = this.falseLabel == "" ? generador.newLabel() : this.falseLabel;

            this.value.trueLabel = this.falseLabel;
            this.value.falseLabel = this.trueLabel;

            Retorno value = this.value.compile(entorno);
            if (value.type.type == Types.BOOLEAN)
            {
                Retorno retorno = new Retorno("", false, value.type);
                retorno.trueLabel = this.trueLabel;
                retorno.falseLabel = this.falseLabel;
                return retorno;
            }
            throw new ErroPascal(this.line, this.column, "No se puede evaluar el not. Tipo -> " + value.type.type,"Semantico");
        }
    }
}
