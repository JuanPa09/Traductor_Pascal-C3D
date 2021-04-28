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

        public Declaracion(utils.Type type, LinkedList<string> idList, Expresion value, int line, int column):base(line,column)
        {
            this.type = type;
            this.idList = idList;
            this.value = value;
            this.line = line;
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            Generador generador = Generador.getInstance();

            if(this.value == null)
            {
                switch(this.type.type)
                {
                    case Types.NUMBER:
                        this.value = new Primitivo(this.type.type, 0, line, column);
                        break;
                    case Types.DOUBLE:
                        this.value = new Primitivo(this.type.type, 0.0, line, column);
                        break;
                    case Types.BOOLEAN:
                        this.value = new Primitivo(this.type.type, true, line, column);
                        break;
                    case Types.STRING:
                        this.value = new Primitivo(this.type.type, "", line, column);
                        break;
                }
            }

            Retorno value = this.value!=null?this.value.compile(entorno):new Retorno("",false,this.type);
            if(!this.sameType(this.type,value.type))
            {
                throw new ErroPascal(this.line, this.column, "Tipos De Datos Diferentes " + this.type + ", " + value.type, "Semántico");
            }
            this.validateType(entorno);
            foreach(string id in idList)
            {
                Simbolo newVar = entorno.addVar(id, value.type.type == Types.NULLL ? this.type : value.type, false, false);
                if (newVar == null)
                    throw new ErroPascal(this.line, this.column, "La variable " + id + " ya existe en este ambito", "Semántico");

                if (newVar.isGlobal)
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
                    throw new ErroPascal(this.line, this.column, "No existe el struct " + this.type.typeId, "Semántico");
                }
                this.type._struct = _struct;
            }
        }
    }
}
