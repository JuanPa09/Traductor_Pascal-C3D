using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.reportes;

namespace Traductor_Pascal_C3D.traductor.abstractas
{
    abstract class Instruccion
    {
        int line;
        int column;
        public Instruccion(int line, int column)
        {
            this.line = line;
            this.column = column;
        }

        public abstract object compile(Entorno entorno, Reporte reporte);

    }
}
