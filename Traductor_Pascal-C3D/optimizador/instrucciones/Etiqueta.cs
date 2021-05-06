using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.optimizador.abstracta;
using Traductor_Pascal_C3D.optimizador.generador;

namespace Traductor_Pascal_C3D.optimizador.instrucciones
{
    class Etiqueta : Optimizar
    {
        private string lbl;

        public Etiqueta(string lbl)
        {
            this.lbl = lbl;
        }

        public override string optimizar()
        {
            Generador generador = Generador.getInstance();
            generador.addLabel(lbl);
            return null;
        }
    }
}
