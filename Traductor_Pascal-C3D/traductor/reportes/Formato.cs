using System;
using System.Collections.Generic;
using System.Text;

namespace Traductor_Pascal_C3D.traductor.reportes
{
    class Formato
    {
        public int fila;
        public int columna;
        public string tipo;
        public string mensaje;

        public Formato(int fila, int columna, string tipo, string mensaje)
        {
            this.fila = fila;
            this.columna = columna;
            this.tipo = tipo;
            this.mensaje = mensaje;
        }
    }
}
