using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.expresion;
using Traductor_Pascal_C3D.traductor.reportes;
using Traductor_Pascal_C3D.traductor.utils;

namespace Traductor_Pascal_C3D.traductor.variables
{
    class Llamada : Instruccion
    {
        private Expresion call;

        public Llamada(Expresion call, int line, int column):base(line,column)
        {
            this.call = call;
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            Retorno value = this.call.compile(entorno);
            value.getValue();
            return value;
        }
    }
}
