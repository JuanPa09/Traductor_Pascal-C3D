using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.generador;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.reportes;

namespace Traductor_Pascal_C3D.traductor.instruccion.transferencia
{
    class Return : Instruccion
    {
        private Expresion value;

        public Return(int line, int column, Expresion value = null) : base(line, column)
        {
            this.value = value;
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            Retorno value = this.value != null ? this.value.compile(entorno) : new Retorno(null, false, new utils.Type(Types.NULLL, ""));
            SymbolFunction symFunc = entorno.actualFunc;
            Generador generador = Generador.getInstance();

            if (symFunc == null)
                throw new ErroPascal(this.line, this.column, "No se puede realizar el retorno", "Semantico");

            if (!this.sameType(symFunc.type, value.type))
                throw new ErroPascal(this.line, this.column, "Se esperaba " + symFunc.type.type + "y se obtuvo " + value.type.type, "Semantico");

            if (symFunc.type.type == Types.BOOLEAN)
            {
                string templabel = generador.newLabel();
                generador.addLabel(value.trueLabel);
                generador.addSetStack("p", "1");
                generador.addGoto(templabel);
                generador.addLabel(value.falseLabel);
                generador.addSetStack("p", "0");
                generador.addLabel(templabel);
            }
            else if (symFunc.type.type != Types.NULLL)
                generador.addSetStack("p", value.getValue());

            generador.addGoto(entorno._return);

            return null;
        }
    }
}
