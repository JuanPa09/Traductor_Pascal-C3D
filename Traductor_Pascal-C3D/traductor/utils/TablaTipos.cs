using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;

namespace Traductor_Pascal_C3D.traductor.utils
{
    class TablaTipos
    {
        public static Types[,] types = new Types[7, 7]
        {
            { Types.NUMBER,Types.STRING,Types.NUMBER,Types.NULLL,Types.NULLL,Types.NULLL,Types.DOUBLE},
            { Types.STRING,Types.STRING,Types.STRING,Types.STRING,Types.STRING,Types.STRING,Types.STRING},
            { Types.NUMBER,Types.STRING,Types.BOOLEAN,Types.NULLL,Types.NULLL,Types.NULLL,Types.DOUBLE},
            { Types.NULLL,Types.STRING,Types.NULLL,Types.NULLL,Types.NULLL,Types.NULLL,Types.NULLL},
            { Types.NULLL,Types.STRING,Types.NULLL,Types.NULLL,Types.ARRAY,Types.NULLL,Types.NULLL},
            { Types.NULLL,Types.STRING,Types.NULLL,Types.NULLL,Types.NULLL,Types.OBJECT,Types.NULLL},
            { Types.DOUBLE,Types.STRING,Types.DOUBLE,Types.NULLL,Types.NULLL,Types.NULLL,Types.DOUBLE}
        };

        public static Types getTipo(Type izquierda, Type derecha)
        {
            return types[(int)izquierda.type, (int)derecha.type];
        }
    }
}
