using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.generador;

namespace Traductor_Pascal_C3D.traductor.expresion.literal
{
    class Primitivo : Expresion
    {

        private Types type;
        private object value;

        public Primitivo(Types type, object value, int line, int column):base(line,column)
        {
            this.type = type;
            this.value = value;
        }

        public override Retorno compile(Entorno entorno)
        {
            Generador generador = Generador.getInstance();
            switch (this.type)
            {
                case Types.NUMBER:
                case Types.DOUBLE:
                    return new Retorno(this.value.ToString(), false, new utils.Type(this.type, null));
                case Types.BOOLEAN:
                    Retorno retorno = new Retorno("", false, new utils.Type(this.type, null));
                    this.trueLabel = this.trueLabel == "" ? generador.newLabel() : this.trueLabel;
                    this.falseLabel = this.falseLabel == "" ? generador.newLabel() : this.falseLabel;
                    if (bool.Parse(this.value.ToString()))
                    {generador.addGoto(this.trueLabel);}
                    else {generador.addGoto(this.falseLabel);}
                    retorno.trueLabel = this.trueLabel;
                    retorno.falseLabel = this.falseLabel;
                    return retorno;
                case Types.STRING:
                    this.value = this.value.ToString().Replace('\'','\0');
                    string temp = generador.newTemporal();
                    generador.addExpression(temp, "h");
                    for (int i = 0; i < this.value.ToString().Length; i++)
                    {
                        generador.addSetHeap("h",((int)this.value.ToString()[i]).ToString());
                        generador.nextHeap();
                    }
                    generador.addSetHeap("h", "-1");
                    generador.nextHeap();
                    return new Retorno(temp, true, new utils.Type(this.type, "String"));
            }


            throw new NotImplementedException();
        }
    }
}
