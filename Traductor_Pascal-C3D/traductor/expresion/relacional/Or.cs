using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.generador;

namespace Traductor_Pascal_C3D.traductor.expresion.relacional
{
    class Or : Expresion
    {
        private Expresion left;
        private Expresion right;

        public Or(Expresion left, Expresion right, int line,int column):base(line,column)
        {
            this.left = left;
            this.right = right;
        }

        public override Retorno compile(Entorno entorno)
        {
            Generador generador = Generador.getInstance();
            this.trueLabel = this.trueLabel == "" ? generador.newLabel() : this.trueLabel;
            this.falseLabel = this.falseLabel == "" ? generador.newLabel() : this.falseLabel;

            this.left.trueLabel = this.right.trueLabel = this.trueLabel;
            this.left.falseLabel = generador.newLabel();
            this.right.falseLabel = this.falseLabel;

            Retorno left = this.left.compile(entorno);
            generador.addLabel(this.left.falseLabel);
            Retorno right = this.right.compile(entorno);

            if (left.type.type == Types.BOOLEAN && right.type.type == Types.BOOLEAN)
            {
                Retorno retorno = new Retorno("", false, left.type);
                retorno.trueLabel = this.trueLabel;
                retorno.falseLabel = this.falseLabel;
                return retorno;
            }

            throw new ErroPascal(this.line, this.column, "Las expresiones no devolvieron tipo booleano para el and " + left.type.type + "|" + right.type.type, "Semantico");
        }
    }
}
