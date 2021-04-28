using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.reportes;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.generador;

namespace Traductor_Pascal_C3D.traductor.instruccion.funciones
{
    class Estructura : Instruccion
    {

        LinkedList<Instruccion> instruccionesHead;
        LinkedList<Instruccion> instruccionesBody;

        public Estructura(LinkedList<Instruccion> instruccionesHead, LinkedList<Instruccion> instruccionesBody) : base(0,0) 
        {
            this.instruccionesHead = instruccionesHead;
            this.instruccionesBody = instruccionesBody;
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            LinkedList<Instruccion> variables = new LinkedList<Instruccion>();
            Generador generador = Generador.getInstance();

            generador.addNativaPrint();
            generador.addNativaCompareString();
            
            foreach (Instruccion instruccion in instruccionesHead)
            {
                if (instruccion != null)
                {
                    try
                    {
                        
                        if(instruccion.GetType().Name == "DeclararVariables")
                        {
                            variables.AddLast(instruccion);
                        }
                        else
                        {
                            instruccion.compile(entorno, reporte);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }
            }


            generador.addStartFunc("main", "void");

            foreach(Instruccion instruccion in variables)
            {
                try
                {
                    instruccion.compile(entorno,reporte);
                }catch(Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            foreach (Instruccion instruccion in instruccionesBody)
            {
                if (instruccion != null)
                {
                    try
                    {
                        instruccion.compile(entorno, reporte);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }
            }
            generador.addReturn("");
            generador.addEndFunc();

            return null;
        }
    }
}
