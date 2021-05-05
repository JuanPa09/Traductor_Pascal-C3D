using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.optimizador.abstracta;
using Traductor_Pascal_C3D.optimizador.generador;

namespace Traductor_Pascal_C3D.optimizador.instrucciones
{
    class NoCondicional : Optimizar
    {
        string lbl;
        string fila;
        bool isIf;
        public NoCondicional(string lbl,string fila,bool isIf)
        {
            this.lbl = lbl;
            this.fila = fila;
            this.isIf = isIf;
        }

        public override string optimizar()
        {
            Generador generador = Generador.getInstance();
            if (!isIf)
            {
                generador.addGoto(lbl, fila);
            }
            else
            {
                generador.addCode("goto " + lbl);
            }
            return lbl;
        }
    }
}
