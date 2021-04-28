using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.reportes;

namespace Traductor_Pascal_C3D.traductor.utils
{
    class ErroPascal : Exception
    {
        private int linea, columna;
        private string mensaje;
        private string tipo;
        Reporte reporte;

        public ErroPascal(int linea, int columna, string mensaje, string tipo)
        {
            this.linea = linea;
            this.columna = columna;
            this.mensaje = mensaje;
            this.tipo = tipo;
            reporte = Reporte.getInstance();
            reporte.nuevoError(linea, columna, tipo, mensaje);
        }

        public override string ToString()
        {
            return "Se encontro error " + this.tipo + "en la linea" + this.linea + " y columna " + this.columna + "\n Mensaje: " + this.mensaje;
        }

    }
}
