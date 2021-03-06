using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.optimizador.reporte;
using System.Linq;

namespace Traductor_Pascal_C3D.optimizador.generador
{
    class Generador
    {
        private static Generador generador;
        Reporte reporte;
        LinkedList<string> codigo;
        LinkedList<string> codigoBorrado;
        LinkedList<Declaracion> asignaciones;
        string isFunc = "";
        bool buscando = false;
        string etiqueta = "";
        string linea = "";

        

        private Generador()
        {
            this.codigo = new LinkedList<string>();
            this.codigoBorrado = new LinkedList<string>();
            this.reporte = Reporte.getInstance();
            this.asignaciones = new LinkedList<Declaracion>();
        }

        public static Generador getInstance()
        {
            if (generador == null)
            {
                generador = new Generador();
            }
            return generador;
        }

        public void clearData()
        {
            codigo.Clear();
            codigoBorrado.Clear();
            asignaciones.Clear();
            buscando = false;
            etiqueta = "";
            linea = "";
        }

        public void addCall(string nombre)
        {
            this.cancelarBusqueda(); //////////////
            this.asignaciones.Clear(); /////////////
            this.codigo.AddLast(this.isFunc + nombre + "();");
        }

        public void addFunc(string nombre)
        {
            if (!buscando)
            {
                codigo.AddLast(this.isFunc + "void " + nombre + "() {");
            }
            else
            {
                codigoBorrado.AddLast(this.isFunc + "void " + nombre + "() {");
            }
            this.isFunc += " ";
        }

        public void addEndFunc()
        {
            this.cancelarBusqueda(); ////////////
            this.asignaciones.Clear(); ///////////////
            this.etiqueta = "";
            this.buscando = false;
            if (!buscando)
            {
                codigo.AddLast("return;");
                this.isFunc = this.isFunc.Remove(this.isFunc.Length - 1);
                codigo.AddLast("}");
            }
            else
            {
                this.codigoBorrado.AddLast("return;");
                this.isFunc = this.isFunc.Remove(this.isFunc.Length - 1);
                this.codigoBorrado.AddLast("}");
            }
        }

        public void addIf(string left,string right,string op,string trueLbl)
        {
            if (!buscando)
            {
                codigo.AddLast(this.isFunc + "if(" + left + " " + op + " " + right + ") goto " + trueLbl + ";");
            }
            else
            {
                codigoBorrado.AddLast(this.isFunc + "if(" + left + " " + op + " " + right + ") goto " + trueLbl + ";");
            }
        }

        public void addExpression(string target, string left, string right = "", string op = "",string line = "")
        {
            string valor = left == "" ? right : left;
            if(op == "")
            {
                foreach(Declaracion dato in asignaciones)
                {
                    if(dato.target == target && dato.value == valor)
                    {
                        reporte.newOptimizacion("Bloques", "5", target + "=" + valor, "", line);
                        return;
                    }
                }

                if (!buscando)
                {
                    codigo.AddLast(this.isFunc + target + " = " + left + op + right + ";");
                }
                else
                {
                    codigoBorrado.AddLast(this.isFunc + target + " = " + left + op + right + ";");
                }
                asignaciones.AddLast(new Declaracion(valor,target));
            }
            else
            {
                foreach (Declaracion dato in asignaciones)
                {
                    if (dato.value == target)
                    {
                        deleteTargetValue(dato.value);
                        break;
                    }
                }

                if (!buscando)
                {
                    codigo.AddLast(this.isFunc + target + " = " + left + op + right + ";");
                }
                else
                {
                    codigoBorrado.AddLast(this.isFunc + target + " = " + left + op + right + ";");
                }
            }

            
        }

        public void addCode(string code)
        {
            if (!buscando)
            {
                this.codigo.AddLast(this.isFunc + code);
            }
            else
            {
                this.codigoBorrado.AddLast(this.isFunc + code);
            }
        }

        public void addGoto(string lbl,string linea)
        {
            this.asignaciones.Clear();
            this.cancelarBusqueda();
            this.codigo.AddLast(this.isFunc + "goto " + lbl + ";");
            this.buscandoEtiqueta(lbl, linea);
            this.etiqueta = lbl;
            this.buscando = true;
        }
        
        public void addLabel(string lbl)
        {
            asignaciones.Clear();
            if(lbl == this.etiqueta)
            {
                borrarCodigo();
            }
            else
            {
                cancelarBusqueda();
            }
            codigo.AddLast(lbl + ":");
        }

        public void buscandoEtiqueta(string lbl, string linea)
        {
            this.etiqueta = lbl;
            this.linea = linea;
            buscando = true;
        }
        public void cancelarBusqueda()
        {
            buscando = false;
            foreach(string codigo in codigoBorrado)
            {
                this.codigo.AddLast(codigo);
            }
            codigoBorrado.Clear();
        }
        public void borrarCodigo()
        {
            this.buscando = false;
            string deletedCode = string.Join('\n', this.codigoBorrado);
            reporte.newOptimizacion("Bloques", "1", deletedCode, "", linea);
            this.codigoBorrado.Clear();
            
        }

        public string getCode()
        {
            return string.Join("\n", this.codigo);
        }

        public void deleteTargetValue(string value)
        {
            LinkedList<Declaracion> dec = new LinkedList<Declaracion>();
            foreach(Declaracion declaracion in asignaciones)
            {
                if(declaracion.value == value)
                {
                    dec.AddLast(declaracion);
                }
            }
            foreach(Declaracion ds in dec)
            {
                asignaciones.Remove(ds);
            }
            dec.Clear();
        }

        class Declaracion
        {
            public string target;
            public string value;

            public Declaracion(string target,string value)
            {
                this.target = target;
                this.value = value;
            }
        }

    }
}
