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
        string operador;
        string trueLbl;
        string falseLbl;
        string fila;

        public Condicional(Evaluar left, Evaluar right, string operador, string trueLbl, string falseLbl, string fila)
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
            string codigoNormal = "if (" + left.valor + operador + right.valor + ") goto " + trueLbl + ";\ngoto " + falseLbl + ";";

            if (left.isConst && right.isConst)
            {
                double num_left = double.Parse(left.valor.ToString());
                double num_right = double.Parse(right.valor.ToString());
                string codigoOptimizado = "";
                bool operacion = false;
                switch (operador)
                {
                    case "==":
                        operacion = double.Parse(num_left.ToString()) == double.Parse(num_right.ToString());
                        break;
                    case "!=":
                        operacion = double.Parse(num_left.ToString()) != double.Parse(num_right.ToString());
                        break;
                    case ">=":
                        operacion = double.Parse(num_left.ToString()) >= double.Parse(num_right.ToString());
                        break;
                    case ">":
                        operacion = double.Parse(num_left.ToString()) > double.Parse(num_right.ToString());
                        break;
                    case "<=":
                        operacion = double.Parse(num_left.ToString()) <= double.Parse(num_right.ToString());
                        break;
                    case "<":
                        operacion = double.Parse(num_left.ToString()) < double.Parse(num_right.ToString());
                        break;
                }

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
                    switch (operador)
                    {
                        case "==":
                            operador = "!=";
                            break;
                        case ">=":
                            operador = "<";
                            break;
                        case ">":
                            operador = "<=";
                            break;
                        case "<=":
                            operador = ">";
                            break;
                        case "<":
                            operador = ">=";
                            break;
                        case "!=":
                            operador = "==";
                            break;
                    }
                    string codigoOptimizado = "if (" + left.valor + this.operador + right.valor + ") goto " + falseLbl + ";";
                    generador.addIf(left.valor, right.valor, operador, falseLbl);
                    reporte.newOptimizacion("Bloques", "2", codigoNormal, codigoOptimizado, fila);
                }
                else
                {
                    generador.addIf(left.valor, right.valor, operador, trueLbl);
                }
            }

            return null;
        }
    }
}
