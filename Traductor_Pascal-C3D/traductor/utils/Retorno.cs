using System;
using System.Collections.Generic;
using System.Text;

namespace Traductor_Pascal_C3D.traductor.utils
{
    class Retorno
    {
        public string value;
        public bool isTemp;
        public utils.Type type;
        public string trueLabel;
        public string falseLabel;
        public tablaSimbolos.Simbolo simbolo = null;

        public Retorno(string value, bool isTemp, utils.Type type, tablaSimbolos.Simbolo simbolo = null)
        {
            this.value = value;
            this.isTemp = isTemp;
            this.type = type;
            this.simbolo = simbolo;
            this.trueLabel = this.falseLabel = "";
        }

        public string getValue()
        {
            Traductor_Pascal_C3D.traductor.generador.Generador.getInstance().freeTemp(this.value);
            return this.value;
        }

        public void asignarValor(string value)
        {
            this.value = value;
        }

    }
}
