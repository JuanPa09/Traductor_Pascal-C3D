using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.generador;
using Traductor_Pascal_C3D.traductor.utils;

namespace Traductor_Pascal_C3D.traductor.expresion.assignment
{
    class AssignmentId : Expresion
    {
        private string id;
        private Expresion anterior;

        public AssignmentId(string id, Expresion anterior, int line, int column):base(line,column)
        {
            this.id = id;
            this.anterior = anterior;
        }



        public override Retorno compile(Entorno entorno)
        {
            Generador generador = Generador.getInstance();
            if(this.anterior == null)
            {
                Simbolo symbol = entorno.getVar(this.id);
                if (symbol == null)
                    throw new ErroPascal(this.line, this.column, "La variable " + this.id + " no existe","Semantico");
                if (symbol.isGlobal)
                {
                    /*if(symbol.type.type == Types.ARRAY)
                    {
                        SymbolArray symbolArray = entorno.getArray(symbol.type.typeId);
                        return new Retorno(symbol.position.ToString(),false,new utils.Type(symbolArray.type.type,symbolArray.type.typeId),symbol);
                    }*/
                    return new Retorno(symbol.position.ToString(), false, symbol.type, symbol);
                }
                else
                {
                    string temp = generador.newTemporal();
                    generador.addExpression(temp, "p", symbol.position.ToString(), "+");
                    return new Retorno(temp, true, symbol.type, symbol);
                }
            }
            else
            {
                Retorno anterior = this.anterior.compile(entorno);
                if (anterior.type.type != Types.STRUCT)
                    throw new ErroPascal(this.line, this.column, "Acceso no valido para el tipo " + anterior.type.type, "Semantico");
                SymbolStruct symStruct = entorno.getStruct(anterior.type.typeId);
                object[] attribute = symStruct != null ? symStruct.getAttribute(this.id) : anterior.type._struct != null ? anterior.type._struct.getAttribute(this.id) : null;
                if (attribute == null || attribute[1] == null)
                    throw new ErroPascal(this.line, this.column, "El type " + symStruct.identifier + "no tiene el atributo " + this.id,"Semantico");

                string tempAux = generador.newTemporal();
                generador.freeTemp(tempAux);
                string temp = generador.newTemporal();
                if(anterior.simbolo != null && !anterior.simbolo.isHeap)
                {
                    generador.addGetStack(tempAux, anterior.getValue());
                }
                else
                {
                    generador.addGetHeap(tempAux, anterior.getValue());
                }
                generador.addExpression(temp, tempAux, attribute[0].ToString(), "+");
                if (((Param)attribute[1]).type.type == Types.STRUCT)
                {
                    SymbolArray symbolArray = entorno.getArray(((Param)attribute[1]).type.typeId);
                    if (symbolArray != null)
                        return new Retorno(temp, true, symbolArray.type);
                }
                return new Retorno(temp, true, ((Param)attribute[1]).type, new Simbolo(((Param)attribute[1]).type, this.id, ((int)attribute[0]).ToString(), false, false, true));
            }
        }
    }
}
