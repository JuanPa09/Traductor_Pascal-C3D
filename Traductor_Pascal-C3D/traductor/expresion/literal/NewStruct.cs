using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.generador;

namespace Traductor_Pascal_C3D.traductor.expresion.literal
{
    class NewStruct : Expresion
    {
        private string id;

        public NewStruct(string id, int line, int column) : base(line, column)
        {
            this.id = id;
        }

        public override Retorno compile(Entorno entorno)
        {
            SymbolStruct symStruct = entorno.getStruct(this.id);
            Generador generador = Generador.getInstance();
            if (symStruct == null)
                throw new ErroPascal(this.line, this.column, "No existe el type " + this.id, "Semantico");
            string temp = generador.newTemporal();
            generador.addExpression(temp, "h");
            foreach(Param _param in symStruct.attributes)
            {
                switch (_param.type.type)
                {
                    case Types.NUMBER:
                    case Types.DOUBLE:
                    case Types.BOOLEAN:
                        generador.addSetHeap("h", "0");
                        break;
                    case Types.STRING:
                    case Types.STRUCT:
                    case Types.ARRAY:
                        generador.addSetHeap("h", "-1");
                        break;
                }
                generador.nextHeap();
            }
            return new Retorno(temp, true, new utils.Type(Types.STRUCT, symStruct.identifier));
        }
    }
}
