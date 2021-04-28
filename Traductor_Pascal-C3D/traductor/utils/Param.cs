using System;
using System.Collections.Generic;
using System.Text;

namespace Traductor_Pascal_C3D.traductor.utils
{
    class Param
    {
        public string id;
        public utils.Type type;
        public bool isRef;

        public Param(string id, utils.Type type, bool isRef = false)
        {
            this.id = id.ToLower();
            this.type = type;
            this.isRef = isRef;
        }

        public string getUnicType()
        {
            if(this.type.type == Types.STRUCT)
            {
                return this.type.typeId;
            }
            return this.type.type.ToString();
        }

        public string toString()
        {
            return "{id: " + this.id + ", type: " + this.type + "}";
        }

    }
}
