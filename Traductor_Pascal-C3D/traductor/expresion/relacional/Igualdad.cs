using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.generador;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.expresion;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.utils;

namespace Traductor_Pascal_C3D.traductor.expresion.relacional
{
    class Igualdad : Expresion
    {
        private Expresion left;
        private Expresion right;

        public Igualdad(Expresion left, Expresion right, int line, int column):base(line,column)
        {
            this.left = left;
            this.right = right;
        }

        public override Retorno compile(Entorno entorno)
        {
            Retorno _left = this.left.compile(entorno);
            Retorno _right = null;
            Generador generador = Generador.getInstance();
            switch (_left.type.type)
            {
                case Types.NUMBER:
                case Types.DOUBLE:
                    _right = this.right.compile(entorno);
                    switch (_right.type.type)
                    {
                        case Types.NUMBER:
                        case Types.DOUBLE:
                            this.trueLabel = this.trueLabel == "" ? generador.newLabel() : this.trueLabel;
                            this.falseLabel = this.falseLabel == "" ? generador.newLabel() : this.falseLabel;
                            generador.addIf(_left.getValue(), _right.getValue(), "==", this.trueLabel);
                            generador.addGoto(this.falseLabel);
                            Retorno retorno = new Retorno("", false, new utils.Type(Types.BOOLEAN, null));
                            retorno.trueLabel = this.trueLabel;
                            retorno.falseLabel = this.falseLabel;
                            return retorno;
                    }
                    break;
                case Types.BOOLEAN:
                    string trueLabel = generador.newLabel();
                    string falseLabel = generador.newLabel();

                    generador.addLabel(left.trueLabel);
                    this.right.trueLabel = trueLabel;
                    this.right.falseLabel = falseLabel;
                    _right = this.right.compile(entorno);

                    generador.addLabel(left.falseLabel);
                    this.right.trueLabel = falseLabel;
                    this.right.falseLabel = trueLabel;
                    _right = this.right.compile(entorno);
                    if (_right.type.type == Types.BOOLEAN)
                    {
                        Retorno retorno = new Retorno("", false, _left.type);
                        retorno.trueLabel = trueLabel;
                        retorno.falseLabel = falseLabel;
                        return retorno;
                    }
                    break;
                case Types.STRING:
                    _right = this.right.compile(entorno);
                    switch(_right.type.type)
                    {
                        case Types.STRING:
                            string temp = generador.newTemporal();
                            string tempAux = generador.newTemporal(); generador.freeTemp(tempAux);
                            generador.addExpression(tempAux, "p", (entorno.size + 1).ToString(), "+"); ; // + 1 porque en la posicion 1 se guarda el return;
                            generador.addSetStack(tempAux, _left.getValue());
                            generador.addExpression(tempAux, tempAux, "1", "+");
                            generador.addSetStack(tempAux, _right.getValue());
                            generador.addExpression("T0", _left.getValue(),"","");
                            generador.addExpression("T1",_right.getValue(),"","");
                            //generador.addNextEnv(entorno.size);
                            generador.addCall("native_compare_str_str");
                            //generador.addGetStack(temp, "p");
                            //generador.addAntEnv(entorno.size);

                            this.trueLabel = this.trueLabel == "" ? generador.newLabel() : this.trueLabel;
                            this.falseLabel = this.falseLabel == "" ? generador.newLabel() : this.falseLabel;
                            generador.addIf("T2", "1", "==", this.trueLabel);
                            generador.addGoto(this.falseLabel);
                            Retorno retorno = new Retorno("", false, new utils.Type(Types.BOOLEAN,null));
                            retorno.trueLabel = this.trueLabel;
                            retorno.falseLabel = this.falseLabel;
                            return retorno;
                    }
                    break;
            }
            throw new NotImplementedException();
        }
    }
}
