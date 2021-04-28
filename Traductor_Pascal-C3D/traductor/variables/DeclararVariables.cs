using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.reportes;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using System.Diagnostics;


namespace Traductor_Pascal_C3D.traductor.variables
{
    class DeclararVariables : Instruccion
    {

        private LinkedList<Instruccion> listaDeclaraciones;

        public DeclararVariables(LinkedList<Instruccion> listaDeclaraciones, int linea, int columna):base(linea,columna)
        {
            this.listaDeclaraciones = listaDeclaraciones;
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            foreach(Instruccion declaracion in listaDeclaraciones)
            {
                try
                {
                    if (declaracion != null)
                        declaracion.compile(entorno, reporte);
                }catch(Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return null;
        }

      
    }
}
