using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;

namespace Traductor_Pascal_C3D.traductor.utils
{

    public enum Types
    {
        NUMBER = 0,
        STRING = 1,
        BOOLEAN = 2,
        NULLL = 3,
        ARRAY = 4,
        OBJECT = 5,
        DOUBLE = 6,
        TYPE = 7,
        STRUCT = 8
        
    }

    class Type
    {
        public Types type;
        public string typeId;
        public int dimension;
        public SymbolStruct _struct = null;


        public Type(Types type, string typeId,SymbolStruct _struct = null,int dimension = 0)
        {
            this.type = type;
            this.typeId = typeId;
            this.dimension = dimension;
            this._struct = _struct;
        }
    }
}
