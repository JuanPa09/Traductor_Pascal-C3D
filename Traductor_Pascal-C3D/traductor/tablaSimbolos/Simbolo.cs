using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.utils;

namespace Traductor_Pascal_C3D.traductor.tablaSimbolos
{
    class Simbolo
    {
        public utils.Type type;
        string identifier;
        public int position;
        public bool isConst;
        public bool isGlobal;
        public bool isHeap;

        public Simbolo(utils.Type type, string identifier, int position, bool isConst, bool isGlobal, bool isHeap)
        {
            this.type = type;
            this.identifier = identifier;
            this.position = position;
            this.isConst = isConst;
            this.isGlobal = isGlobal;
            this.isHeap = isHeap;
        }

    }
}
