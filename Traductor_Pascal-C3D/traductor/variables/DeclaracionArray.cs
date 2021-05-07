using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.generador;
using Traductor_Pascal_C3D.traductor.reportes;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.utils;
using Type = Traductor_Pascal_C3D.traductor.utils.Type;

namespace Traductor_Pascal_C3D.traductor.variables
{
    class DeclaracionArray : Expresion
    {
        public Type type;
        string id;
        LinkedList<Dictionary<int, int>> dimensiones; //max;val min;val
        SymbolArray symbolArray;

        public DeclaracionArray(SymbolArray symbolArray,int line, int column):base(line,column)
        {
            this.type = symbolArray.type;
            this.id = symbolArray.id;
            this.dimensiones = symbolArray.dimensiones;

        }

        public override Retorno compile(Entorno entorno)
        {

            Generador generador = Generador.getInstance();
            generador.addComment("Inicia Declaracion De Array");
            this.validateType(entorno);
            this.type.dimension = dimensiones.Count;
            Simbolo newVar = entorno.addVar(this.id, this.type, false, false,this.line,this.column);
            if (newVar == null)
                throw new ErroPascal(this.line, this.column, "No se pudo declarar el array " + this.id, "Semantico");

            generador.addSetStack(newVar.position.ToString(), "h");

            foreach(Dictionary<int,int> dims in dimensiones)
            {
                foreach (KeyValuePair<int, int> limites in dims)
                {
                    generador.addSetHeap("h", limites.Key.ToString()); //Se almacena el minimo
                    generador.nextHeap();
                    generador.addSetHeap("h", limites.Value.ToString()); //Se almacena el maximo
                    generador.nextHeap();
                }
            }
            generador.addSetHeap("h", "-1");
            generador.nextHeap();

            int index = 0;

            /*Agregando los campos del array*/
            foreach(Dictionary<int,int> dims in dimensiones)
            {
                foreach (KeyValuePair<int, int> limites in dims)
                {
                    int tam = limites.Value - limites.Key;
                    for (int i = 0; i <= tam; i++)
                    {
                        switch (type.type)
                        {
                            case Types.DOUBLE:
                            case Types.NUMBER:
                            case Types.BOOLEAN:
                                generador.addSetHeap("h", "0");
                                generador.nextHeap();
                                break;
                        }
                        index++;
                    }
                }
            }
            generador.addComment("Termina Declaracion De Array");
            return new Retorno(newVar.position.ToString(),true,new Type(Types.ARRAY,this.type.typeId,null,this.type.dimension));
        }

        

        private void validateType(Entorno entorno)
        {
            if(this.type.type == Types.STRUCT)
            {
                SymbolStruct symbolStruct = entorno.getStruct(this.type.typeId);
                if(symbolStruct == null)
                {
                    throw new ErroPascal(this.line, this.column, "No existe el struct " + this.type.typeId, "Sematico");
                }
            }

        }

    }
}
