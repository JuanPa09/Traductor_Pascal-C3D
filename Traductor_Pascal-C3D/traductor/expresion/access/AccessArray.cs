using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.generador;
using Traductor_Pascal_C3D.traductor.utils;

namespace Traductor_Pascal_C3D.traductor.expresion.access
{
    class AccessArray : Expresion
    {
        LinkedList<Expresion> indices;
        Expresion anterior;

        public AccessArray(LinkedList<Expresion> indices, Expresion anterior,int line, int column):base(line,column)
        {
            this.indices = indices;
            this.anterior = anterior;
        }

        public override Retorno compile(Entorno entorno)
        {

            Generador generador = Generador.getInstance();
            Retorno anterior = this.anterior.compile(entorno);
            if (anterior.type.dimension == 0)
                throw new ErroPascal(this.line,this.column,"No se encontro arreglo","Semantico");

            if (anterior.type.dimension != indices.Count)
                throw new ErroPascal(this.line, this.column, "Los indices no coinciden para el arreglo ", "Semantico");

            LinkedList<string> _indices = new LinkedList<string>();
            string puntero = generador.newTemporal(); generador.freeTemp(puntero);
            //Asignando puntero
            generador.addGetStack(puntero, anterior.value);
            string min = generador.newTemporal(); generador.freeTemp(min);
            string max = generador.newTemporal(); generador.freeTemp(max);
            string tempAux = generador.newTemporal(); generador.freeTemp(tempAux); //Lleva los valores del heap
            string tempIndex = generador.newTemporal(); generador.freeTemp(tempIndex); //Lleva el numero del indice
            string temp = generador.newTemporal(); //Lleva el resultado de toda la expresion aritmetica
            int conteo = 0; // 0:obtener multiplicacion //1:operar resta //3:operar suma

            generador.addComment("Accediendo Al Array");
            foreach (Expresion indice in indices)
            {
                Retorno val = indice.compile(entorno);
                if (val.type.type != Types.NUMBER)
                    throw new ErroPascal(this.line,this.column,"El indice no es un entero", "Semantico");
                _indices.AddLast(val.value);

                if (conteo != 0)
                {
                    generador.addExpression(tempAux, max, min, "-");
                    generador.addExpression(tempAux, tempAux, "1", "+");
                    generador.addExpression(temp, temp, tempAux, "*");
                }
                    conteo = 1;
                    generador.addGetHeap(tempAux, puntero);
                    generador.addExpression(puntero, puntero, "1", "+");
                    generador.addExpression(min, tempAux);
                    generador.addGetHeap(tempAux, puntero);
                    generador.addExpression(puntero, puntero, "1", "+");
                    generador.addExpression(max, tempAux);
                    if(val.isTemp)
                    {
                        generador.addGetStack(tempIndex, val.value);
                    }
                    else
                    {
                        generador.addExpression(tempIndex, val.value);
                    }
                    generador.addExpression(temp, tempIndex, min, "-");

            }
            //Paso el valor del heap
            generador.addExpression(tempAux, indices.Count.ToString(), "2", "*");
            generador.addExpression(tempAux, tempAux, "1", "+");
            generador.addExpression(temp, temp, tempAux,"+");
            generador.addGetHeap(temp, temp);

            generador.addComment("Termina Acceso A Array");


            /*string puntero = generador.newTemporal(); generador.freeTemp(puntero);
            //Asignando puntero
            generador.addGetStack(puntero, anterior.value);
            string min = generador.newTemporal(); generador.freeTemp(min);
            string max = generador.newTemporal(); generador.freeTemp(max);
            string tempAux = generador.newTemporal(); generador.freeTemp(tempAux); //Lleva los valores del heap
            string tempIndex = generador.newTemporal(); generador.freeTemp(tempIndex); //Lleva el numero del indice
            string temp = generador.newTemporal(); //Lleva el resultado de toda la expresion aritmetica
            int conteo = 0; // 0:obtener multiplicacion //1:operar resta //3:operar suma

            /*foreach (string index in _indices)
            {
                if (conteo != 0)
                {
                    generador.addExpression(tempAux, max, min, "-");
                    generador.addExpression(tempAux, tempAux, "1", "+");
                    generador.addExpression(temp, temp, tempAux, "*");
                }
                else
                {
                    conteo = 1;
                    generador.addGetHeap(tempAux, puntero);
                    generador.addExpression(puntero, puntero, "1", "+");
                    generador.addExpression(min, tempAux);
                    generador.addGetHeap(tempAux, puntero);
                    generador.addExpression(puntero, puntero, "1", "+");
                    generador.addExpression(max, tempAux);
                    
                    generador.addExpression(temp, tempIndex, min, "-");
                }
            }*/


            return new Retorno(temp, true, anterior.type);
        }
    }
}
