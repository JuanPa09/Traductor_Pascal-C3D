using System;
using System.Collections.Generic;
using System.Text;

namespace Traductor_Pascal_C3D.traductor.tablaSimbolos
{
    class SymbolArray 
    {
        public utils.Type type;
        public string id;
        public LinkedList<Dictionary<int, int>> dimensiones; //max;val min;val 

        public SymbolArray(utils.Type type, string id, LinkedList<Dictionary<int,int>> dimensiones)
        {
            this.dimensiones = dimensiones;
            this.id = id;
            this.type = type;
        }



    }
}
