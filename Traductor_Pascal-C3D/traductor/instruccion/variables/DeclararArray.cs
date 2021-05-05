using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.reportes;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;

namespace Traductor_Pascal_C3D.traductor.instruccion.variables
{
    class DeclararArray : Instruccion
    {
        Expresion arreglo;
        bool processed = false;

        public DeclararArray(Expresion arreglo,int line , int column) : base(line,column)
        {
            this.arreglo = arreglo;
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            if (!processed)
            {
                processed = true;
                this.arreglo.compile(entorno);
            }
            return null;
        }
    }
}
