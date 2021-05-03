using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.reportes;

namespace Traductor_Pascal_C3D.traductor.abstractas
{
    abstract class Instruccion
    {
        public int line;
        public int column;
        public Instruccion(int line, int column)
        {
            this.line = line;
            this.column = column;
        }

        public abstract object compile(Entorno entorno, Reporte reporte);

        public bool sameType(utils.Type type1, utils.Type type2)
        {
            if(type1.type == type2.type)
            {
                if(type1.type == utils.Types.STRUCT)
                {
                    return type1.typeId.ToLower() == type2.typeId.ToLower();
                }
                return true;
            }
            else if(type1.type == utils.Types.STRUCT || type2.type == utils.Types.STRUCT)
            {
                if (type1.type == utils.Types.NULLL || type2.type == utils.Types.NULLL)
                {
                    return true;
                }
                else if (type1.type == utils.Types.ARRAY)
                {
                    return type1.dimension > 0;
                }
                else if (type2.type == utils.Types.ARRAY)
                {
                    return type2.dimension > 0;
                }
            }
            return false;
        }

    }
}
