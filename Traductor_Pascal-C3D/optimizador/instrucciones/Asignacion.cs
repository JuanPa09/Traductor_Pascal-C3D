using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.optimizador.abstracta;
using Traductor_Pascal_C3D.optimizador.generador;

namespace Traductor_Pascal_C3D.optimizador.instrucciones
{
    class Asignacion : Optimizar
    {
        public string target;
        bool isEstructura;

        public override string optimizar()
        {
            throw new NotImplementedException();
        }
    }
}
