using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.utils;

namespace Traductor_Pascal_C3D.traductor.abstractas
{
    abstract class Expresion
    {
        string trueLabel;
        string falseLabel;
        int line;
        int column;

        public Expresion(int line, int column)
        {
            this.trueLabel = this.falseLabel = "";
            this.line = line;
            this.column = column;
        }

        public abstract Retorno compile(Entorno entorno);

    }
}
