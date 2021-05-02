using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.reportes;

namespace Traductor_Pascal_C3D.traductor.instruccion.funciones
{
    class StructSt : Instruccion
    {
        private string id;
        private LinkedList<Param> attributes;

        public StructSt(string id, LinkedList<Param> attributes,int line, int column):base(line, column)
        {
            this.id = id;
            this.attributes = attributes;
        }


        public override object compile(Entorno entorno, Reporte reporte)
        {
            if (!entorno.addStruct(this.id, this.attributes.Count, this.attributes))
                throw new ErroPascal(this.line, this.column, "Ya existe un type con el id " + this.id,"Semantico");
            this.validateParams(entorno);
            return null;
        }
        private void validateParams(Entorno entorno)
        {
            LinkedList<string> set = new LinkedList<string>();
            foreach(Param _param in attributes)
            {
                if (set.Contains(_param.id.ToLower()))
                    throw new ErroPascal(this.line, this.column, "Ya existe un parametro con el id " + _param.id,"Semantico");
                if(_param.type.type == Types.STRUCT)
                {
                    SymbolStruct _struct = entorno.getStruct(_param.type.typeId);
                    if (_struct == null)
                        throw new ErroPascal(this.line, this.column, "No existe el type " + _param.type.typeId,"Semantico");
                }
                set.AddLast(_param.id.ToLower());
            }
        }
    }
}
