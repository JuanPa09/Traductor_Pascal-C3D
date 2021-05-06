using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.optimizador.generador;
using Traductor_Pascal_C3D.optimizador.abstracta;

namespace Traductor_Pascal_C3D.optimizador.instrucciones
{
    class Metodo : Optimizar
    {
        string id;
        LinkedList<Optimizar> instrucciones;

        public Metodo(string id, LinkedList<Optimizar> instrucciones)
        {
            this.id = id;
            this.instrucciones = instrucciones;
        }

        public override string optimizar()
        {
            Generador generador = Generador.getInstance();
            generador.addFunc(id);
            if(instrucciones!= null)
            {
                foreach(Optimizar instruccion in instrucciones)
                {
                    instruccion.optimizar();
                }
            }
            generador.addEndFunc();
            return null;
        }
    }
}
