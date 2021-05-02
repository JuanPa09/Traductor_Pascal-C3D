using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.generador;
using Traductor_Pascal_C3D.traductor.reportes;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Type = Traductor_Pascal_C3D.traductor.utils.Type;

namespace Traductor_Pascal_C3D.traductor.variables
{
    class DeclaracionArray : Instruccion
    {
        Type type;
        string id;
        LinkedList<Dictionary<int, int>> dimensiones; //max;val min;val 
        bool isType;

        public DeclaracionArray(Type type, string id, LinkedList<Dictionary<int, int>> dimensiones, bool isType,int line, int column):base(line,column)
        {
            this.type = type;
            this.id = id;
            this.dimensiones = dimensiones;
            this.isType = isType;
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            if (isType)
            {

            }
            throw new NotImplementedException();
        }
    }
}
