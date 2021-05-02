using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.expresion;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.generador;
using Traductor_Pascal_C3D.traductor.utils;

namespace Traductor_Pascal_C3D.traductor.expresion.access
{
    class AccessId : Expresion
    {
        private string id;
        private Expresion anterior = null;

        public AccessId(string id, Expresion anterior, int line, int column):base(line,column)
        {
            this.id = id;
            this.anterior = anterior;
        }

        public override Retorno compile(Entorno entorno)
        {
            Generador generador = Generador.getInstance();
            if(this.anterior == null)
            {
                Simbolo simbolo = entorno.getVar(this.id);
                if (simbolo == null)
                    throw new ErroPascal(this.line, this.column, "La variable " + this.id + " no existe","Semántico");

                string temp = generador.newTemporal();
                if(simbolo.isGlobal)
                {
                    generador.addGetStack(temp, simbolo.position.ToString());
                    if (simbolo.type.type != Types.BOOLEAN)
                        return new Retorno(temp, true, simbolo.type, simbolo);

                    Retorno retorno = new Retorno("", false, simbolo.type, simbolo);
                    this.trueLabel = this.trueLabel == "" ? generador.newLabel() : this.trueLabel;
                    this.falseLabel = this.falseLabel == "" ? generador.newLabel() : this.falseLabel;
                    generador.addIf(temp, "1", "==", this.trueLabel);
                    generador.addGoto(this.falseLabel);
                    retorno.trueLabel = this.trueLabel;
                    retorno.falseLabel = this.falseLabel;
                    return retorno;
                }
                else
                {
                    string tempAux = generador.newTemporal(); generador.freeTemp(tempAux);
                    generador.addExpression(tempAux, "p", simbolo.position.ToString(), "+");
                    generador.addGetStack(temp, tempAux);
                    if (simbolo.type.type != Types.BOOLEAN) return new Retorno(temp, true, simbolo.type, simbolo);

                    Retorno retorno = new Retorno("", false, simbolo.type);
                    this.trueLabel = this.trueLabel == "" ? generador.newLabel() : this.trueLabel;
                    this.falseLabel = this.falseLabel == "" ? generador.newLabel() : this.falseLabel;
                    generador.addIf(temp, "1", "==", this.trueLabel);
                    generador.addGoto(this.falseLabel);
                    retorno.trueLabel = this.trueLabel;
                    retorno.falseLabel = this.falseLabel;
                    return retorno;
                }

            }
            else
            {
                Retorno anterior = this.anterior.compile(entorno);
                SymbolStruct symStruct = entorno.getStruct(anterior.type.typeId);
                if (anterior.type.type != Types.STRUCT || symStruct == null)
                    throw new ErroPascal(this.line, this.column, "Acceso no valido para el tipo " + anterior.type.type, "Semántico");
                object[] attribute = symStruct.getAttribute(this.id);
                if (attribute[1] == null)
                    throw new ErroPascal(this.line, this.column, "El struct " + symStruct.identifier + " no tiene el atributo " + id, "Semántico");

                string tempAux = generador.newTemporal();
                generador.freeTemp(tempAux);
                string temp = generador.newTemporal();

                generador.addExpression(tempAux, anterior.getValue(), attribute[0].ToString(), "+");
                generador.addGetHeap(temp, tempAux);
                return new Retorno(temp, true, ((Param)attribute[1]).type);


            }
        }
    }
}
