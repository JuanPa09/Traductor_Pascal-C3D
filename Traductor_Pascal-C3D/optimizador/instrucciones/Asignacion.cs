using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.optimizador.abstracta;
using Traductor_Pascal_C3D.optimizador.generador;
using Traductor_Pascal_C3D.optimizador.simbolos;

namespace Traductor_Pascal_C3D.optimizador.instrucciones
{
    class Asignacion : Optimizar
    {
        public string target;
        public Evaluar value;
        string fila;
        bool containsEstructura;

        public Asignacion(string target, Evaluar value, bool containsEstructura, string fila)
        {
            this.target = target;
            this.value = value;
            this.containsEstructura = containsEstructura;
            this.fila = fila;
        }

        public override string optimizar()
        {
            Generador generador = Generador.getInstance();
            Simbolo valor = value.evaluar();
            generador.addExpression(target, valor.valor,"","",this.fila);
            return null;
        }
    }
}
