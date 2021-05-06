using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.optimizador.abstracta;
using Traductor_Pascal_C3D.optimizador.generador;
using Traductor_Pascal_C3D.optimizador.simbolos;

namespace Traductor_Pascal_C3D.optimizador.instrucciones
{
    class Print : Optimizar
    {
        string tipo;
        Evaluar valor;
        bool isDoub;

        public Print(string tipo, Evaluar valor, bool isDoub)
        {
            this.tipo = tipo;
            this.valor = valor;
            this.isDoub = isDoub;
        }

        public override string optimizar()
        {
            Generador generador = Generador.getInstance();
            Simbolo valor = this.valor.evaluar();
            if (isDoub)
            {
                generador.addCode("printf(" + tipo + "," + valor.valor + ");");
            }
            else
            {
                generador.addCode("printf(" + tipo + ",(int)" + valor.valor + ");");
            }
            return null;
        }
    }
}
