using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.optimizador.abstracta;
using Traductor_Pascal_C3D.optimizador.generador;
using Traductor_Pascal_C3D.optimizador.simbolos;
using Traductor_Pascal_C3D.optimizador.reporte;

namespace Traductor_Pascal_C3D.optimizador.instrucciones
{
    class Operacion : Optimizar
    {
        string Target;
        Evaluar izq;
        Evaluar der;
        string operador;
        string line;
        public Operacion(string target, Evaluar izq, Evaluar der, string operador,string line)
        {
            Target = target;
            this.izq = izq;
            this.der = der;
            this.operador = operador;
            this.line = line;
        }

        public override string optimizar()
        {
            Generador generador = Generador.getInstance();
            Reporte reporte = Reporte.getInstance();
            Simbolo izquierda = izq.evaluar();
            Simbolo derecha = der.evaluar();

            string valIzquierda = izquierda.valor;
            string valDerecha = derecha.valor;
            switch (operador)
            {
                case "+":
                    if (izquierda.isConst && izquierda.valor == "0")
                        izquierda.valor = "";
                    if (derecha.isConst && derecha.valor == "0")
                        derecha.valor = "";
                    if (derecha.valor == "" || izquierda.valor == "")
                        operador = "";
                    if (operador == "" && (Target == izquierda.valor || Target == derecha.valor))
                    {
                        reporte.newOptimizacion("Bloques", "6", Target + "=" + valIzquierda + "+" + valDerecha, "", this.line);
                    }
                    else
                    {
                        if(operador == "")
                        {
                            reporte.newOptimizacion("Bloques", "10", Target + "=" + valIzquierda + "+" + valDerecha, Target + "=" + izquierda.valor + derecha.valor, this.line);
                        }
                        generador.addExpression(Target, izquierda.valor, derecha.valor, operador);
                    }
                    break;
                case "-":
                    if (izquierda.isConst && izquierda.valor == "0")
                        izquierda.valor = "";
                    if (derecha.isConst && derecha.valor == "0")
                        derecha.valor = "";
                    if (derecha.valor == "" || izquierda.valor == "")
                        operador = "";
                    if (operador == "" && (Target == izquierda.valor || Target == derecha.valor))
                    {
                        reporte.newOptimizacion("Bloques", "7", Target + "=" + valIzquierda + "-" + valDerecha, "", this.line);
                    }
                    else
                    {
                        if (operador == "")
                        {
                            reporte.newOptimizacion("Bloques", "11", Target + "=" + valIzquierda + "-" + valDerecha, Target + "=" + izquierda.valor + derecha.valor, this.line);
                        }
                        generador.addExpression(Target, izquierda.valor, derecha.valor, operador);
                    }


                    break;
                case "*":
                    if (izquierda.isConst && izquierda.valor == "1")
                        izquierda.valor = "";
                    if (derecha.isConst && derecha.valor == "1")
                        derecha.valor = "";
                    if (derecha.valor == "" || izquierda.valor == "")
                        operador = "";
                    if (operador == "" && (Target == izquierda.valor || Target == derecha.valor))
                    {
                        reporte.newOptimizacion("Bloques", "8", Target + "=" + valIzquierda + "*" + valDerecha, "", this.line);
                    }
                    else
                    {
                        if (operador == "")
                        {
                            reporte.newOptimizacion("Bloques", "12", Target + "=" + valIzquierda + "*" + valDerecha, Target + "=" + izquierda.valor + derecha.valor, this.line);
                        }

                        if (operador != "")
                        {
                            if(izquierda.valor == "2")
                            {
                                izquierda.valor = "";
                                operador = "";
                                derecha.valor = derecha.valor + "+" + derecha.valor;
                                reporte.newOptimizacion("Bloques", "14", Target + "=" + valIzquierda + "*" + valDerecha, Target + "=" + derecha.valor, this.line);
                            }else if(derecha.valor == "2")
                            {
                                derecha.valor = "";
                                operador = "";
                                izquierda.valor = izquierda.valor + "+" + izquierda.valor;
                                reporte.newOptimizacion("Bloques","14", Target + "=" + valIzquierda + "*" + valDerecha,Target + "=" + izquierda.valor,this.line);
                            }

                            if (izquierda.valor == "0")
                            {
                                derecha.valor = "";
                                operador = "";
                                reporte.newOptimizacion("Bloques", "15", Target + "=" + valIzquierda + "*" + valDerecha, Target + "=" + izquierda.valor, this.line);
                            }
                            else if (derecha.valor == "0")
                            {
                                izquierda.valor = "";
                                operador = "";
                                reporte.newOptimizacion("Bloques", "15", Target + "=" + valIzquierda + "*" + valDerecha, Target + "=" + derecha.valor, this.line);
                            }

                        }
                        generador.addExpression(Target, izquierda.valor, derecha.valor, operador);

                        

                    }
                    break;
                case "/":
                    if (derecha.isConst && derecha.valor == "1")
                        derecha.valor = "";
                    if (derecha.valor == "")
                        operador = "";
                    if (operador == "" && Target == izquierda.valor)
                    {
                        reporte.newOptimizacion("Bloques", "9", Target + "=" + valIzquierda + "/" + valDerecha, "", this.line);
                    }
                    else
                    {
                        if (operador == "")
                        {
                            reporte.newOptimizacion("Bloques", "13", Target + "=" + valIzquierda + "/" + valDerecha, Target + "=" + izquierda.valor + derecha.valor, this.line);
                        }

                        if(izquierda.valor == "0")
                        {
                            operador = "";
                            derecha.valor = "";
                            reporte.newOptimizacion("Bloques", "16", Target + "=" + valIzquierda + "/" + valDerecha, Target + "= 0",this.line);
                        }

                        generador.addExpression(Target, izquierda.valor, derecha.valor, operador);
                    }
                    break;
            }

            return null;
        }
    }
}
