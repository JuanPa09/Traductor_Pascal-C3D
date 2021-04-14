using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.reportes;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;

namespace Traductor_Pascal_C3D.traductor.instruccion.funciones
{
    class Encabezado : Instruccion
    {

        public Encabezado(int line, int column):base(line,column)
        {
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            return null;
        }
    }
}
