using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.optimizador.abstracta;
using Traductor_Pascal_C3D.optimizador.generador;
using Traductor_Pascal_C3D.optimizador.simbolos;

namespace Traductor_Pascal_C3D.optimizador.instrucciones
{
    class Valor : Evaluar
    {
        private string valor;
        private bool isConst;

        public Valor(string valor, bool isConst)
        {
            this.valor = valor;
            this.isConst = isConst;
        }

        public override Simbolo evaluar()
        {
            return new Simbolo(valor, isConst);
        }
    }
}
