using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.optimizador.reporte;

namespace Traductor_Pascal_C3D.optimizador.generador
{
    class Generador
    {
        private static Generador generador;
        Reporte reporte;
        LinkedList<string> codigo;
        LinkedList<string> codigoBorrado;
        string isFunc = "";
        bool buscando = false;
        string etiqueta = "";
        string linea = "";

        

        private Generador()
        {
            this.codigo = new LinkedList<string>();
            this.codigoBorrado = new LinkedList<string>();
            this.reporte = Reporte.getInstance();
        }

        public static Generador getInstance()
        {
            if (generador == null)
            {
                generador = new Generador();
            }
            return generador;
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

        public void addExpression(string target, string left, string right = "", string op = "")
        {
            if (!buscando)
            {
                codigo.AddLast(this.isFunc + target + op + right + ";");
            }
            else
            {
                codigoBorrado.AddLast(this.isFunc + target + op + right + ";");
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
            this.cancelarBusqueda();
            this.codigo.AddLast(this.isFunc + "goto" + lbl + ";");
            this.buscandoEtiqueta(lbl, linea);
            this.etiqueta = lbl;
            this.buscando = true;
        }
        
        public void addLabel(string lbl)
        {
            codigo.AddLast(lbl + ":");
            if(lbl == this.etiqueta)
            {
                borrarCodigo();
            }
            else
            {
                cancelarBusqueda();
            }
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

    }
}
