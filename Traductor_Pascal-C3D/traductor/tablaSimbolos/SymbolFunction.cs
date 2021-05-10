using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.instruccion.funciones;

namespace Traductor_Pascal_C3D.traductor.tablaSimbolos
{
    class SymbolFunction
    {
        public utils.Type type;
        string id;
        public string uniqueId;
        int size;
        public LinkedList<Param> _params;

        public SymbolFunction(FunctionSt func, string uniqueId, LinkedList<Param> _params = null)
        {
            this.type = func.type;
            this.id = func.id;
            this.size = func._params.Count;
            this.uniqueId = uniqueId;
            this._params = func._params;
        }

    }
}
