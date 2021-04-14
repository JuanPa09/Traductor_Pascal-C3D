using System;
using System.Collections.Generic;
using System.Text;

namespace Traductor_Pascal_C3D.traductor.tablaSimbolos
{
    class Entorno
    {
        public Dictionary<string, Simbolo> functions;
        public Dictionary<string, Simbolo> structs;
        public Dictionary<string, Simbolo> vars;
        public Entorno anterior = null;
        public int size;
        public string _break = null;
        public string _continue = null;
        public string _return = null;
        public string prop = null;
        public Simbolo actualFunc = null;

        public Entorno(Entorno anterior)
        {
            this.anterior = anterior;
            this.functions = new Dictionary<string, Simbolo>();
            this.structs = new Dictionary<string, Simbolo>();
            this.vars = new Dictionary<string, Simbolo>();
            this.size = (anterior != null?anterior.size:0);
            this._break = (anterior != null ? anterior._break : null);
            this._return = (anterior != null ? anterior._return : null);
            this._continue = (anterior != null ? anterior._continue : null);
            this.prop = "main";
            this.actualFunc = (anterior != null ? anterior.actualFunc : null);
        }

        public void setEnvironmentFunc(string prop, Simbolo actualFunc, string ret)
        {
            this.size = 1; //1 porque la posicion 0 es para el return
            this.prop = prop;
            this._return = ret;
            this.actualFunc = actualFunc;

        }
    }
}
