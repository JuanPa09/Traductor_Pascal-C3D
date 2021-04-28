using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.generador;

namespace Traductor_Pascal_C3D.traductor.expresion.relacional
{
    class Mayor : Expresion
    {

        private Expresion left;
        private Expresion right;
        private bool isGrtEqual;

        public Mayor(bool isGrtEqual, Expresion left, Expresion right,int line, int column):base(line,column)
        {
            this.left = left;
            this.right = right;
            this.isGrtEqual = isGrtEqual;
        }

        public override Retorno compile(Entorno entorno)
        {
            Retorno _left = this.left.compile(entorno);
            Retorno _right = this.right.compile(entorno);

            Types leftType = _left.type.type;
            Types righType = _right.type.type;

            switch(leftType)
            {
                case Types.NUMBER:
                case Types.DOUBLE:
                    switch (righType)
                    {
                        case Types.NUMBER:
                        case Types.DOUBLE:

                            Generador generador = Generador.getInstance();
                            this.trueLabel = this.trueLabel == "" ? generador.newLabel() : this.trueLabel;
                            this.falseLabel = this.falseLabel == "" ? generador.newLabel() : this.falseLabel;
                            if (this.isGrtEqual)
                            {
                                generador.addIf(_left.getValue(), _right.getValue(), ">=", this.trueLabel);
                            }
                            else
                            {
                                generador.addIf(_left.getValue(), _right.getValue(), ">", this.trueLabel);
                            }
                            generador.addGoto(this.falseLabel);
                            Retorno retorno = new Retorno("", false, new utils.Type(Types.BOOLEAN, null));
                            retorno.trueLabel = this.trueLabel;
                            retorno.falseLabel = this.falseLabel;
                            return retorno;
                        default:
                            return null;//Excepcion
                    }
                default:
                    return null;

            }

            throw new NotImplementedException();
        }

        
    }
}
