using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.utils;

namespace Traductor_Pascal_C3D.traductor.tablaSimbolos
{
    class SymbolStruct
    {
        public string identifier;
        int size;
        public LinkedList<Param> attributes;

        public SymbolStruct(string identifier, int size, LinkedList<Param> attributes)
        {
            this.identifier = identifier;
            this.size = size;
            this.attributes = attributes;
        }

        public object[] getAttribute(string id)
        {
            /*
             @index pos0
             @value pos1
             */
            int i = 0;
            Dictionary<int, Param> dic = new Dictionary<int, Param>();
            foreach(Param param in attributes)
            {
                if(param.id == id)
                {
                    dic.Add(i, param);
                    object[] retorno = { i,param};
                    return retorno;
                }
                i++;
            }
            object[] ret = { -1, null };

            return ret;
        }
    
        
    }
}
