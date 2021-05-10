using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.reportes;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.generador;
using Traductor_Pascal_C3D.traductor.variables;
using Traductor_Pascal_C3D.traductor.expresion.assignment ;
using Traductor_Pascal_C3D.traductor.expresion.literal;

using System.Linq;

namespace Traductor_Pascal_C3D.traductor.expresion.assignment
{
    class AssignmentFunc : Expresion
    {
        private string id;
        private Expresion anterior = null;
        private LinkedList<Expresion> @params;
        

        public AssignmentFunc(string id, Expresion anterior, LinkedList<Expresion> @params, int line, int column):base(line,column)
        {
            this.id = id;
            this.anterior = anterior;
            this.@params = @params;
        }

        public override Retorno compile(Entorno entorno)
        {
            if (this.anterior == null)
            {
                SymbolFunction symFunc = entorno.searchFunc(this.id);
                if (symFunc == null)
                    throw new ErroPascal(this.line, this.column, "No se encontro la funcion " + this.id, "Semantico");
                LinkedList<Retorno> paramsValues = new LinkedList<Retorno>();
                Generador generador = Generador.getInstance();
                int size = generador.saveTemps(entorno);
                foreach (Expresion param in @params)
                {
                    paramsValues.AddLast(param.compile(entorno));
                }
                //TODO comprobar parametros correctos
                string temp = generador.newTemporal();
                generador.freeTemp(temp);
                //Paso de parametros en cambio simulado
                if (paramsValues.Count != 0)
                {
                    generador.addExpression(temp, "p", (entorno.size + 1).ToString(), "+"); //+1 porque la posicion 0 es para el retorno;
                    int index = 0;
                    foreach (Retorno value in paramsValues)
                    {
                        generador.addSetStack(temp, value.getValue());
                        if (index != paramsValues.Count - 1)
                            generador.addExpression(temp, temp, "1", "+");
                        index++;
                    }
                }
                generador.addNextEnv(entorno.size);
                generador.addCall(symFunc.uniqueId);
                int i = 0;
                generador.addComment("Pasando variables por referencia");
                Dictionary<string, string> refValues = new Dictionary<string, string>();
                foreach(Param _param in symFunc._params)
                {
                    string refVal = generador.newTemporal(); generador.freeTemp(refVal);
                    string refPos = generador.newTemporal();
                    if (_param.isRef)
                    {
                        refPos = (entorno.size + i + 1).ToString();
                        generador.addGetStack(refVal,refPos);
                        string posTarget = paramsValues.ElementAt(i).simbolo.position;
                        refValues.Add(posTarget.ToString(), refVal);
                    }
                    i += 1;
                }
                generador.addComment("Terminan variables por referencia");
                generador.addGetStack(temp, "p");
                generador.addAntEnv(entorno.size);
                generador.recoverTemps(entorno, size);
                generador.addComment("Asignando Variables Por Referencia");
                foreach(KeyValuePair<string,string> refValue in refValues)
                {
                    string posTemp = refValue.Key;
                    string valueTemp = refValue.Value;
                    generador.addSetStack(posTemp,valueTemp);
                }
                generador.addComment("Terminan Variables Por Referencia");
                generador.addComment("Este es el temp");
                generador.addTemp(temp);

                
                
                
                


                if (symFunc.type.type != Types.BOOLEAN)
                    return new Retorno(temp, true, symFunc.type);

                Retorno retorno = new Retorno("", false, symFunc.type);
                this.trueLabel = this.trueLabel == "" ? generador.newLabel() : this.trueLabel;
                this.falseLabel = this.falseLabel == "" ? generador.newLabel() : this.falseLabel;
                generador.addIf(temp, "1", "==", this.trueLabel);
                generador.addGoto(this.falseLabel);
                retorno.trueLabel = this.trueLabel;
                retorno.falseLabel = this.falseLabel;
                return retorno;
            }

            /*if(anterior == null)
            {
                SymbolFunction symFunc = entorno.searchFunc(this.id);
                if (symFunc == null)
                    throw new ErroPascal(this.line, this.column, "No se encontro la funcion " + this.id, "Semantico");
                LinkedList<Retorno> paramsValues = new LinkedList<Retorno>();
                Generador generador = Generador.getInstance();
                int size = generador.saveTemps(entorno);
                foreach(Expresion param in @params)
                {
                    paramsValues.AddLast(param.compile(entorno));
                }
                //Cambio Simulado
                string temp = generador.newTemporal(); generador.freeTemp(temp);
                if (paramsValues.Count != 0)
                {
                    generador.addExpression(temp, "p", (entorno.size + 1).ToString(), "+");
                    int index = 0;
                    foreach(Retorno value in paramsValues)
                    {
                        if (isReferencia(symFunc._params, index))
                        {

                        }
                        else
                        {
                            generador.addSetStack(temp, value.getValue());
                        }
                        if (index != paramsValues.Count - 1)
                            generador.addExpression(temp, temp, "1", "+");
                    }
                }
                generador.addNextEnv(entorno.size);
                generador.addCall(symFunc.uniqueId);
                generador.addGetStack(temp, "p");
                generador.addAntEnv(entorno.size);
                generador.recoverTemps(entorno,size);
                generador.addTemp(temp);

                if (symFunc.type.type != Types.BOOLEAN) return new Retorno(temp, true, symFunc.type);

                Retorno retorno = new Retorno("", false, symFunc.type);
                this.trueLabel = this.trueLabel == "" ? generador.newLabel() : this.trueLabel;
                this.falseLabel = this.falseLabel == "" ? generador.newLabel() : this.falseLabel;
                generador.addIf(temp, "1", "==", this.trueLabel);
                generador.addGoto(this.falseLabel);
                retorno.trueLabel = this.trueLabel;
                retorno.falseLabel = this.falseLabel;
                return retorno;

            }*/

            throw new ErroPascal(this.line, this.column, "Funcion no implementada", "Semantico");
        }


        public bool isReferencia(LinkedList<Param> parametros,int index)
        {
            int i = 0;
            foreach(Param parametro in parametros)
            {
                if (i == index)
                {
                    if (parametro.isRef)
                        return true;
                    return false;
                }
                i++;
            }
            return false;
        }

    }
}
