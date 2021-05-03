using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.reportes;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.utils;

namespace Traductor_Pascal_C3D.traductor.expresion.access
{
    class NuevoArray : Instruccion
    {
        utils.Type type;
        string id;
        LinkedList<Dictionary<int, int>> dimensiones; //max;val min;val 

        public NuevoArray(utils.Type type, string id, LinkedList<Dictionary<int, int>> dimensiones,int line, int column):base(line,column)
        {
            this.type = type;
            this.id = id;
            this.dimensiones = dimensiones;
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            bool newArray = entorno.addArray(id, type, dimensiones);
            if (!newArray)
                throw new ErroPascal(this.line, this.column, "El arreglo tipo " + id + " ya existe", "Semantico");
            return null;
        }
    }
}
