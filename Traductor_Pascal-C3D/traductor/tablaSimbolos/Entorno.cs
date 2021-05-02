﻿using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.instruccion.funciones;
using Traductor_Pascal_C3D.traductor.utils;

namespace Traductor_Pascal_C3D.traductor.tablaSimbolos
{
    class Entorno
    {
        public Dictionary<string, SymbolFunction> functions;
        public Dictionary<string, SymbolStruct> structs;
        public Dictionary<string, Simbolo> vars;
        public Entorno anterior = null;
        public int size;
        /*public string _break = null;
        public string _continue = null;*/
        public string _return = null;
        public string prop = null;

        public LinkedList<string> _break;
        public LinkedList<string> _continue;

        public SymbolFunction actualFunc = null;

        public Entorno(Entorno anterior)
        {
            this.anterior = anterior;
            this.functions = new Dictionary<string, SymbolFunction>();
            this.structs = new Dictionary<string, SymbolStruct>();
            this.vars = new Dictionary<string, Simbolo>();
            this.size = (anterior != null?anterior.size:0);
            this._break = (anterior != null ? anterior._break : new LinkedList<string>());
            this._return = (anterior != null ? anterior._return : null);
            this._continue = (anterior != null ? anterior._continue : new LinkedList<string>());
            this.prop = "main";
            this.actualFunc = (anterior != null ? anterior.actualFunc : null);
        }

        public void setEnvironmentFunc(string prop, SymbolFunction actualFunc, string ret)
        {
            this.size = 1; //1 porque la posicion 0 es para el return
            this.prop = prop;
            this._return = ret;
            this.actualFunc = actualFunc;
        }

        public Simbolo addVar(string id, utils.Type type, bool isConst, bool isRef)
        {
            id = id.ToLower();
            if(vars.Count == 0 || !this.vars.ContainsKey(id))
            {
                Simbolo newVar = new Simbolo(type, id, this.size++, isConst, this.anterior == null, isRef);
                this.vars.Add(id, newVar);
                return newVar;
            }
            return null;
            
        }

        public bool addFunc(FunctionSt func, string uniqueId)
        {
            if (this.functions.ContainsKey(func.id.ToLower()))
                return false;
            this.functions.Add(func.id.ToLower(), new SymbolFunction(func, uniqueId));
            return true;
        }

        public SymbolFunction getFunc(string id)
        {
            if(this.functions.ContainsKey(id.ToLower()))
            {
                return this.functions[id.ToLower()];
            }
            return null;
        }

        public Simbolo getVar(string id)
        {
            Entorno actual = this;
            id = id.ToLower();
            while(actual!=null)
            {
                if(actual.vars.ContainsKey(id))
                {
                    return actual.vars[id]; 
                }
                actual = actual.anterior;
            }
            return null;
        }

        public SymbolStruct structExist(string id)
        {
            if(this.structs.ContainsKey(id.ToLower()))
                return this.structs[id.ToLower()];
            return null;
        }

        public SymbolStruct searchStruct(string id)
        {
            Entorno actual = this;
            id = id.ToLower();
            while(actual != null)
            {
                if (actual.structs.ContainsKey(id))
                    return actual.structs[id];
                actual = actual.anterior;
            }
            return null;
        }

        public SymbolFunction searchFunc(string id)
        {
            Entorno entorno = this;
            id = id.ToLower();
            while (entorno != null)
            {
                if (entorno.functions.ContainsKey(id))
                {
                    return entorno.functions[id];
                }
                entorno = entorno.anterior;
            }
            return null;
        }

        public void newBreak(string _breakLbl)
        {
            this._break.AddFirst(_breakLbl);
        }

        public string getBreak()
        {
            Entorno actual = this;
            while(actual!=null)
            {
                if(actual._break.Count != 0)
                {
                    string retornar = actual._break.First.Value;
                    actual._break.RemoveFirst();
                    return retornar;
                }
                actual = actual.anterior;
            }
            return null;
        }

        public void newContinue(string _continueLbl)
        {
            this._continue.AddFirst(_continueLbl);
        }

        public string getContinue()
        {
            Entorno actual = this;
            while (actual != null)
            {
                if (actual._continue.Count != 0)
                {
                    string retornar = actual._continue.First.Value;
                    actual._continue.RemoveFirst();
                    return retornar;
                }
                actual = actual.anterior;
            }
            return null;
        }


        public bool addStruct(string id, int size,LinkedList<Param> @params)
        {
            if (this.structs.ContainsKey(id.ToLower()))
            {
                return false;
            }
            else
            {
                this.structs.Add(id.ToLower(), new SymbolStruct(id.ToLower(), size, @params));
                return true;
            }
        }

        public SymbolStruct getStruct(string id)
        {
            Entorno global = getGlobal();
            if (global.structs.ContainsKey(id.ToLower()))
            {
                return global.structs[id];
            }
            return null;
        }

        public Entorno getGlobal()
        {
            Entorno actual = this;
            while (actual.anterior != null)
            {
                actual = actual.anterior;
            }
            return actual;
        }

    }
}
