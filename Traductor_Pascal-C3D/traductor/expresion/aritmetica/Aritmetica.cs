using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.generador;


namespace Traductor_Pascal_C3D.traductor.expresion.aritmetica
{
    class Aritmetica : Expresion
    {
        Expresion left;
        Expresion right;
        char operador;

        public Aritmetica(Expresion left, Expresion right, char operador,int line, int column):base(line,column)
        {
            this.left = left;
            this.right = right;
            this.operador = operador;
        }

        public override Retorno compile(Entorno entorno)
        {
            Retorno _left = this.left.compile(entorno);
            Retorno _right = this.right.compile(entorno);
            Generador generador = Generador.getInstance();
            string temp = generador.newTemporal();

            switch(_left.type.type)
            {
                case Types.NUMBER:
                    switch (_right.type.type){
                        case Types.NUMBER:
                        case Types.DOUBLE:
                            generador.addExpression(temp, _left.getValue(), _right.getValue(), this.operador.ToString());
                            return new Retorno(temp, true, _right.type.type == Types.DOUBLE ? _right.type : _left.type);
                        case Types.STRING:
                            string tempAux = generador.newTemporal();
                            generador.freeTemp(tempAux);
                            generador.addExpression(tempAux, "p", (entorno.size + 1).ToString(), "+");
                            generador.addSetStack(tempAux, _left.getValue());
                            generador.addExpression(tempAux, tempAux, "1", "+");
                            generador.addSetStack(tempAux, _left.getValue());
                            generador.addNextEnv(entorno.size);
                            generador.addCall("native_concat_dbl_str");
                            generador.addGetStack(temp, "p");
                            generador.addAntEnv(entorno.size);
                            return new Retorno(temp, true, new utils.Type(Types.STRING,null));
                        default:
                            break;
                    }
                    break;
                case Types.DOUBLE:
                    switch (_right.type.type)
                    {
                        case Types.NUMBER:
                        case Types.DOUBLE:
                            generador.addExpression(temp, _left.getValue(), _right.getValue(), this.operador.ToString());
                            return new Retorno(temp, true, new utils.Type(Types.DOUBLE,null));
                        case Types.STRING:
                            string tempAux = generador.newTemporal();
                            generador.freeTemp(tempAux);
                            generador.addExpression(tempAux, "p", (entorno.size + 1).ToString(), "+");
                            generador.addSetStack(tempAux, _left.getValue());
                            generador.addExpression(tempAux,tempAux,"1","+");
                            generador.addSetStack(tempAux, _left.getValue());
                            generador.addNextEnv(entorno.size);
                            generador.addCall("native_concat_dbl_str");
                            generador.addGetStack(temp, "p");
                            generador.addAntEnv(entorno.size);
                            return new Retorno(temp, true, new utils.Type(Types.STRING, null));
                        default:
                            break;

                    }
                    break;
                case Types.BOOLEAN:
                    switch(_right.type.type)
                    {
                        case Types.STRING:
                            string tempAux = generador.newTemporal();
                            generador.freeTemp(tempAux);
                            string lblTemp = generador.newLabel();
                            generador.addExpression(tempAux, "p", (entorno.size + 1).ToString(), "+");
                            generador.addLabel(_left.trueLabel);
                            generador.addSetStack(tempAux, "1");
                            generador.addGoto(lblTemp);
                            generador.addLabel(_left.falseLabel);
                            generador.addSetStack(tempAux, "0");
                            generador.addLabel(lblTemp);
                            generador.addExpression(tempAux, tempAux, "1", "+");
                            generador.addSetStack(tempAux, _right.getValue());
                            generador.addNextEnv(entorno.size);
                            generador.addCall("native_concat_bool_str");
                            generador.addGetStack(temp,"p");
                            generador.addAntEnv(entorno.size);
                            return new Retorno(temp, true, new utils.Type(Types.STRING, null));
                        default:
                            break;
                    }
                    break;
                case Types.STRING:
                    {
                        string tempAux = generador.newTemporal(); 
                        generador.freeTemp(tempAux);
                        switch (_right.type.type)
                        {
                            case Types.NUMBER:
                                /*generador.addExpression(tempAux, "p", (entorno.size + 1).ToString(), "+");
                                generador.addSetStack(tempAux, _left.getValue());
                                generador.addExpression(tempAux, tempAux, "1", "+");
                                generador.addSetStack(tempAux, _right.getValue());
                                generador.addNextEnv(entorno.size);
                                generador.addCall("native_concat_str_num");
                                generador.addGetStack(temp, "p");
                                generador.addAntEnv(entorno.size);*/
                                generador.addExpression("T1", _left.getValue());
                                if (_right.isTemp)
                                {
                                    generador.addExpression("T2", _right.getValue());
                                }
                                else
                                {
                                    generador.addExpression("T2", "h");
                                    for (int i = 0; i < _right.value.Length; i++)
                                    {
                                        generador.addSetHeap("h", ((int)_right.value[i]).ToString());
                                        generador.nextHeap();
                                    }
                                    generador.addSetHeap("h", "-1");
                                    generador.nextHeap();
                                }
                                generador.addCall("native_concat_str_str");
                                generador.addExpression(temp, "T3");
                                return new Retorno(temp, true, new utils.Type(Types.STRING,null));
                            /*
                             
                             Falta Double , Boolean y string
                             */
                            case Types.STRING:
                                generador.addExpression("T1", _left.getValue());
                                generador.addExpression("T2", _right.getValue());
                                generador.addCall("native_concat_str_str");
                                generador.addExpression(temp, "T3");
                                return new Retorno(temp, true, new utils.Type(Types.STRING, null));

                        }
                    }
                    break;
            }

            return null;
        }
    }
}
