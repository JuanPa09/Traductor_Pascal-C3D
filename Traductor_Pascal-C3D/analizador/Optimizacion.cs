using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.reportes;
using Irony.Parsing;
using System.Windows.Forms;

namespace Traductor_Pascal_C3D.analizador
{
    class Optimizacion
    {
        ParseTreeNode raiz;
        RichTextBox consola;
        Reporte reporte;

        public Optimizacion(ParseTreeNode raiz, RichTextBox consola, Reporte reporte)
        {
            this.raiz = raiz;
            this.consola = consola;
            this.reporte = reporte;
        }

        public void iniciar()
        {
            
        }



    }
}
