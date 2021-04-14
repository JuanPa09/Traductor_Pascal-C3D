using System;
using System.Collections.Generic;
using System.Text;

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

        public Type(Types type, string typeId)
        {
            this.type = type;
            this.typeId = typeId;
        }
    }
}
