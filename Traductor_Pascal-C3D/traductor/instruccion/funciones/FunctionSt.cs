using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.generador;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.reportes;
using Type = Traductor_Pascal_C3D.traductor.utils.Type;
using System.Diagnostics;

namespace Traductor_Pascal_C3D.traductor.instruccion.funciones
{
    class FunctionSt : Instruccion
    {
        public string id;
        public LinkedList<Param> _params;
        public Type type;
        private LinkedList<Instruccion> body;
        private LinkedList<Instruccion> head;
        private bool preCompile;

        public FunctionSt(string id, LinkedList<Param> @params, Type type, LinkedList<Instruccion> head, LinkedList<Instruccion> body, bool preCompile, int line, int column):base(line,column)
        {
            this.id = id;
            _params = @params;
            this.type = type;
            this.body = body;
            this.head = head;
            this.preCompile = preCompile;
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            /*if(this.preCompile)
            {
                this.preCompile = false;
                this.validateParams(entorno);
                this.validateType(entorno);
                string uniqueId = this.uniqueId(entorno);
                if (!entorno.addFunc(this, uniqueId))
                    throw new ErroPascal(this.line, this.column, "Ya existe una funcion con el id " + this.id,"Semantico");
                return null;
            }

            //SymbolFunction symbolFunction = entorno.getFunc(this.id);

            throw new NotImplementedException();*/

            this.validateParams(entorno);
            this.validateType(entorno);
            string uniqueId = this.uniqueId(entorno);
            if (!entorno.addFunc(this, uniqueId,this.line,this.column))
                throw new ErroPascal(this.line, this.column, "Ya existe una funcion con el id " + this.id, "Semantico");

            SymbolFunction symbolFunction = entorno.getFunc(this.id);
            if (symbolFunction != null)
            {
                Generador generator = Generador.getInstance();
                Entorno nuevoEntorno = new Entorno(entorno);
                nuevoEntorno.prop = this.id;
                string returnLbl = generator.newLabel();
                LinkedList<string> tempStorage = generator.getTempStorage();
                nuevoEntorno.setEnvironmentFunc(this.id, symbolFunction, returnLbl);

                foreach(Param param in _params)
                {
                    nuevoEntorno.addVar(param.id, param.type, false, false,this.line,this.column);
                }
                generator.clearTempStorage();
                generator.addStartFunc(symbolFunction.uniqueId, "void");
                //Instrucciones Head
                foreach (Instruccion instruccion in this.head)
                {
                    try
                    {
                        if (instruccion.GetType().Name == "DeclararVariables")
                        {
                            instruccion.compile(nuevoEntorno, reporte); //Se compila una vez para guardar las variables, esto sirve en Estructura.cs para que las variables aparezcan dentro del metodo main
                        }
                        instruccion.compile(nuevoEntorno, reporte);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }
                //Instrucciones Body
                foreach (Instruccion instruccion in this.body)
                {
                    try
                    {
                        instruccion.compile(nuevoEntorno,reporte);
                    }catch(Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }
                generator.addLabel(returnLbl);
                
                generator.addReturn("");
                generator.addEndFunc();
                generator.setTempStorage(tempStorage);

            }
            else
            {
                throw new ErroPascal(this.line, this.column, "No se pudo obtener la funcion", "Semantico");
            }
            return null;

        }

        private void validateParams(Entorno entorno)
        {
            LinkedList<string> set = new LinkedList<string>();
            foreach(Param param in _params)
            {
                if (set.Contains(param.id.ToLower()))
                    throw new ErroPascal(this.line, this.column, "Ya existe un parametro con el id " + param.id, "Semantico");
                if(param.type.type == Types.STRUCT)
                {
                    //SymbolStruct struct = entorno
                    SymbolStruct _struct = entorno.searchStruct(param.type.typeId);
                    if (_struct == null)
                    {
                        SymbolArray symbolArray = entorno.getArray(param.type.typeId);
                        if (symbolArray == null)
                        {
                            throw new ErroPascal(this.line, this.column, "No existe el type " + param.type.typeId, "Semantico");
                        }   
                    }
                        
                    param.type._struct = _struct;
                }
                set.AddLast(param.id.ToLower());
            }
        }

        private void validateType(Entorno entorno)
        {
            if(this.type.type == Types.STRUCT)
            {
                SymbolStruct _struct = entorno.searchStruct(this.type.typeId);
                if (_struct == null)
                    throw new ErroPascal(this.line, this.line, "No existe el struct " + this.type.typeId, "Semantico");
                this.type._struct = _struct;
            }
        }

        public string uniqueId(Entorno entorno)
        {
            string id = entorno.prop + "_" + this.id;
            if(this._params.Count == 0)
            {
                return id + "_empty";
            }
            foreach(Param param in this._params)
            {
                id += "_" + param.getUnicType();
            }
            return id;
        }

    }
}
