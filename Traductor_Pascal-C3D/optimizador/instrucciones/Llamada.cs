using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.optimizador.abstracta;
using Traductor_Pascal_C3D.optimizador.generador;

namespace Traductor_Pascal_C3D.optimizador.instrucciones
{
    class Llamada : Optimizar
    {
        string nombre;

        public Llamada(string nombre)
        {
            this.nombre = nombre;
        }

        public override string optimizar()
        {
            Generador generador = Generador.getInstance();
            generador.addCall(nombre);
            return null;

        }
    }
}
