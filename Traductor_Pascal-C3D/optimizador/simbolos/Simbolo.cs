using System;
using System.Collections.Generic;
using System.Text;

namespace Traductor_Pascal_C3D.optimizador.simbolos
{
    class Simbolo
    {
        public string valor;
        public bool isConst;

        public Simbolo(string valor,bool isConst)
        {
            this.valor = valor;
            this.isConst = isConst;
        }

    }
}
