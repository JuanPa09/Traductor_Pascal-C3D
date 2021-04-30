using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.reportes;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.generador;

namespace Traductor_Pascal_C3D.traductor.instruccion.transferencia
{
    class Break : Instruccion
    {
        
        public Break(int line, int column):base(line,column)
        {
        }
        
        public override object compile(Entorno entorno, Reporte reporte)
        {
            string lblRetorno = entorno.getBreak();
            if (lblRetorno == null)
                throw new ErroPascal(this.line, this.column, "No se pudo encontrar un break", "Semantico");
            Generador.getInstance().addGoto(lblRetorno);
            return null;
        }
    }
}
