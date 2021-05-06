using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Traductor_Pascal_C3D.optimizador.reporte
{
    class Reporte
    {
        private static Reporte reporte;
        LinkedList<Contenido> optimizaciones;
        RichTextBox consola;

        private Reporte()
        {
            this.optimizaciones = new LinkedList<Contenido>();
        }

        public static Reporte getInstance()
        {
            if(reporte == null)
            {
                reporte = new Reporte();
            }
            return reporte;
        }

        public void newOptimizacion(string tipo, string regla, string codigoEliminado, string codigoAgregado, string fila)
        {
            optimizaciones.AddLast(new Contenido(tipo, regla, codigoEliminado, codigoAgregado, fila));
        }

        public void realizarReporte()
        {
            string path = "C:\\compiladores2\\reporteOptimizaciones.html";
            string reporte = "<html><title>Errores Lexicos</title><body><center><h1>Reporte De Errores</h1></center><br><br><center>";
            reporte += "<table style=\"width: 100%\">";
            reporte += "<tr>";
            reporte += "<th style=\"border: 1px solid black; background-color:#DFC93F \">Tipo</th>";
            reporte += "<th style=\"border: 1px solid black; background-color:#DFC93F \">Regla</th>";
            reporte += "<th style=\"border: 1px solid black; background-color:#DFC93F \">Código Eliminado</th>";
            reporte += "<th style=\"border: 1px solid black; background-color:#DFC93F \">Código Agregado</th>";
            reporte += "<th style=\"border: 1px solid black; background-color:#DFC93F \">Fila</th>";
            reporte += "</tr>";
            foreach (Contenido optimizacion in optimizaciones)
            {
                reporte += "<tr>";
                reporte += "<th style=\"border: 1px solid black; \">" + optimizacion.tipo + "</th>";
                reporte += "<th style=\"border: 1px solid black; \">" + optimizacion.regla + "</th>";
                reporte += "<th style=\"border: 1px solid black; \">" + optimizacion.codigoEliminado + "</th>";
                reporte += "<th style=\"border: 1px solid black; \">" + optimizacion.codigoAgregado + "</th>";
                reporte += "<th style=\"border: 1px solid black; \">" + optimizacion.fila + "</th>";
                reporte += "</tr>";
            }
            reporte += "</table></center></body></html>";
            try
            {
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(reporte);
                    fs.Write(info, 0, info.Length);
                }
                this.consola.AppendText("Reporte De Optimización Generado Con Éxito!");
            }catch (Exception ex)
            {
                this.consola.AppendText(ex.ToString());
            }
        }

        public void clearData()
        {
            this.optimizaciones.Clear();
        }

        public void setConsola(RichTextBox consola)
        {
            this.consola = consola;
        }

        class Contenido
        {
            public string tipo, regla, codigoEliminado, codigoAgregado, fila;

            public Contenido(string tipo, string regla, string codigoEliminado, string codigoAgregado, string fila)
            {
                this.tipo = tipo;
                this.regla = regla;
                this.codigoEliminado = codigoEliminado;
                this.codigoAgregado = codigoAgregado;
                this.fila = fila;
            }
        }

    }
}
