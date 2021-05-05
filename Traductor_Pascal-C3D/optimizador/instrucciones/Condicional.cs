using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.optimizador.abstracta;
using Traductor_Pascal_C3D.optimizador.generador;
using Traductor_Pascal_C3D.optimizador.reporte;
using Traductor_Pascal_C3D.optimizador.simbolos;

namespace Traductor_Pascal_C3D.optimizador.instrucciones
{
    class Condicional : Optimizar
    {
        Evaluar left;
        Evaluar right;
        Evaluar operador;
        string trueLbl;
        Optimizar falseLbl;
        string fila;

        public Condicional(Evaluar left, Evaluar right, Evaluar operador, string trueLbl, Optimizar falseLbl, string fila)
        {
            this.left = left;
            this.right = right;
            this.operador = operador;
            this.trueLbl = trueLbl;
            this.falseLbl = falseLbl;
            this.fila = fila;
        }

        public override string optimizar()
        {
            Generador generador = Generador.getInstance();
            Reporte reporte = Reporte.getInstance();
            Simbolo left = this.left.evaluar();
            Simbolo right = this.right.evaluar();
            Simbolo operador = this.operador.evaluar();
            string falseLbl =  this.falseLbl!=null?this.falseLbl.optimizar():null;
            string codigoNormal = "if (" + left.valor + operador.valor + right.valor + ") goto " + trueLbl + ";\ngoto " + falseLbl + ";";

            if (left.isConst && right.isConst)
            {
                double num_left = double.Parse(left.valor);
                double num_right = double.Parse(right.valor);
                string codigoOptimizado = "";
                bool operacion = bool.Parse(num_left.ToString() + operador.valor + num_right.ToString());
                string regla = "";
                if (operacion)
                {
                    codigoOptimizado = "goto " + trueLbl + ";";
                    regla = "3";
                }
                else
                {
                    regla = "4";
                    if (falseLbl != null)
                    {
                        codigoOptimizado = "goto " + falseLbl + ";";
                    }
                }
                if (codigoOptimizado != "")
                    generador.addCode(codigoOptimizado);
                reporte.newOptimizacion("Bloques", regla, codigoNormal, codigoOptimizado, this.fila);
            }
            else
            {
                if (falseLbl != null)
                {
                    switch (operador.valor)
                    {
                        case "==":
                            operador.valor = "!=";
                            break;
                        case ">=":
                            operador.valor = "<";
                            break;
                        case ">":
                            operador.valor = "<=";
                            break;
                        case "<=":
                            operador.valor = ">";
                            break;
                        case "<":
                            operador.valor = ">=";
                            break;
                        case "!=":
                            operador.valor = "==";
                            break;
                    }
                    string codigoOptimizado = "if (" + this.left + this.operador + this.right + ") goto " + falseLbl + ";";
                    generador.addIf(left.valor, right.valor, operador.valor, falseLbl);
                    reporte.newOptimizacion("Bloques", "2", codigoNormal, codigoOptimizado, fila);
                }
                else
                {
                    generador.addIf(left.valor, right.valor, operador.valor, trueLbl);
                }
            }

            return null;
        }
    }
}
