using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.generador;
using Traductor_Pascal_C3D.traductor.reportes;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.expresion.literal;

namespace Traductor_Pascal_C3D.traductor.variables
{
    class Declaracion : Instruccion
    {
        private utils.Type type;
        private LinkedList<string> idList;
        private Expresion value;


        /*Para la generacion del codigo*/
        string label = null;
        bool procesada = false;
        bool error = false;
        LinkedList<Simbolo> _newVar;
        Retorno _value;

        Types typeAuxiliar;

        public Declaracion(utils.Type type, LinkedList<string> idList, Expresion value, int line, int column):base(line,column)
        {
            this.type = type;
            this.idList = idList;
            //this.value = value == null && type.type == Types.STRUCT ? new NewStruct(type.typeId, this.line, this.column) : value;
            this.value = value;
            this.line = line;

            _newVar = new LinkedList<Simbolo>();
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            Generador generador = Generador.getInstance();
            if(this.value==null && type.type == Types.STRUCT)
            {
                if(entorno.getArray(type.typeId) != null)
                {
                    
                    this.value = new DeclaracionArray( entorno.getArray(type.typeId),this.line,this.column);
                    
                    
                }
                else
                {
                    this.value = new NewStruct(type.typeId, this.line, this.column);
                }
            }
            if (!procesada)
            {
                this.procesada = true;
                if (this.value == null)
                {
                    switch (this.type.type)
                    {
                        case Types.NUMBER:
                            this.value = new Primitivo(this.type.type, 0, line, column);
                            break;
                        case Types.DOUBLE:
                            this.value = new Primitivo(this.type.type, 0.0, line, column);
                            break;
                        case Types.BOOLEAN:
                            this.value = new Primitivo(this.type.type, false, line, column);
                            break;
                        case Types.STRING:
                            this.value = new Primitivo(this.type.type, "", line, column);
                            break;
                    }
                }

                Retorno value = this.value != null ? this.value.compile(entorno) : new Retorno("", false, this.type);
                
                if (value.type.type == Types.BOOLEAN)    /////////////////
                    this.label = generador.removeLast();

                if (!this.sameType(this.type, value.type))
                {
                    error = true;
                    throw new ErroPascal(this.line, this.column, "Tipos De Datos Diferentes " + this.type + ", " + value.type, "Semántico");
                }
                this._value = value;
                this.validateType(entorno);
                foreach (string id in idList)
                {
                    if(value.type.type == Types.ARRAY)//
                    {
                        SymbolArray symbolArray = entorno.getArray(this.type.typeId);//
                        value.type = symbolArray.type;//
                    }//
                    Simbolo newVar = entorno.addVar(id, value.type.type == Types.NULLL ? this.type : value.type, false, false,this.line,this.column);
                    if (newVar == null)
                    {
                        error = true;
                        throw new ErroPascal(this.line, this.column, "La variable " + id + " ya existe en este ambito", "Semántico");
                    }
                    this._newVar.AddLast(newVar);
                    /*if (newVar.isGlobal)
                    {
                        if(this.type.type == Types.BOOLEAN)
                        {
                            string tempLabel = generador.newLabel();
                            generador.addLabel(value.trueLabel);
                            generador.addSetStack(newVar.position.ToString(),"1");
                            generador.addGoto(tempLabel);
                            generador.addLabel(value.falseLabel);
                            generador.addSetStack(newVar.position.ToString(),"0");
                            generador.addLabel(tempLabel);
                        }
                        else
                        {
                            generador.addSetStack(newVar.position.ToString(), value.getValue());
                        }
                    }
                    else
                    {
                        string temp = generador.newTemporal();
                        generador.freeTemp(temp);
                        if(this.type.type == Types.BOOLEAN)
                        {
                            string tmplabel = generador.newLabel();
                            generador.addLabel(value.trueLabel);
                            generador.addSetStack(temp, "1");
                            generador.addGoto(tmplabel);
                            generador.addLabel(value.falseLabel);
                            generador.addSetStack(temp, "0");
                            generador.addLabel(tmplabel);
                        }
                        else
                        {
                            generador.addSetStack(temp, value.getValue());
                        }
                    }*/

                }
            }
            else
            {
                if (!error)
                {
                    if (label != null)
                        generador.addCode(label);
                    generarCodigo();
                }
            }

            return null;
        }

        private void validateType(Entorno entorno)
        {
            if(this.type.type == Types.STRUCT)
            {
                SymbolStruct _struct = entorno.searchStruct(this.type.typeId);
                if(_struct == null)
                {
                    SymbolArray _array = entorno.getArray(this.type.typeId);
                    if(_array == null)
                        throw new ErroPascal(this.line, this.column, "No existe el type " + this.type.typeId, "Semántico");
                }
                else
                {
                    this.type._struct = _struct;
                }
                
            }
        }

        private void generarCodigo()
        {
            Generador generador = Generador.getInstance();
            foreach(Simbolo newVar in _newVar)
            {
                if (newVar.isGlobal)
                {
                    if (this.type.type == Types.BOOLEAN)
                    {
                        string tempLabel = generador.newLabel();
                        generador.addLabel(_value.trueLabel);
                        generador.addSetStack(newVar.position.ToString(), "1");
                        generador.addGoto(tempLabel);
                        generador.addLabel(value.falseLabel);
                        generador.addSetStack(newVar.position.ToString(), "0");
                        generador.addLabel(tempLabel);
                    }
                    else
                    {
                        generador.addSetStack(newVar.position.ToString(), _value.getValue());
                    }
                }
                else
                {
                    string temp = generador.newTemporal();
                    generador.freeTemp(temp);
                    generador.addExpression(temp, "p", newVar.position.ToString(), "+");
                    if (this.type.type == Types.BOOLEAN)
                    {
                        string tmplabel = generador.newLabel();
                        generador.addLabel(_value.trueLabel);
                        generador.addSetStack(temp, "1");
                        generador.addGoto(tmplabel);
                        generador.addLabel(_value.falseLabel);
                        generador.addSetStack(temp, "0");
                        generador.addLabel(tmplabel);
                    }
                    else
                    {
                        generador.addSetStack(temp, _value.getValue());
                    }
                }
            }
        }

    }
}
