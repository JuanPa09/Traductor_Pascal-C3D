using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;

namespace Traductor_Pascal_C3D.traductor.generador
{
    class Generador
    {
        private static Generador generador;
        private float temporal;
        private float label;
        private LinkedList<string> code;
        private LinkedList<string> tempStorage;
        public string isFunc = "";

        
        private Generador()
        {
            this.temporal = this.label = 0;
            this.code = new LinkedList<string>();
            this.tempStorage = new LinkedList<string>();
        }

        public static Generador getInstance()
        {
            if (generador == null)
            {
                generador = new Generador();
            }
            return generador;
        }

        public LinkedList<string> getTempStorage()
        {
            return this.tempStorage;
        }

        public void clearTempStorage()
        {
            this.tempStorage.Clear();
        }

        public void setTempStorage(LinkedList<string> tempStorage)
        {
            this.tempStorage = tempStorage;
        }

        public void clearCode()
        {
            this.temporal = this.label = 0;
            this.code = new LinkedList<string>();
            this.tempStorage = new LinkedList<string>();
        }

        public void addCode(string code)
        {
            this.code.AddLast(code);
        }
        
        public string getCode()
        {
            return string.Join('\n', this.code);
        }

        public string newTemporal()
        {
             string temp = "T" + this.temporal++;
            this.tempStorage.AddLast(temp);
            return temp;
        }

        public string newLabel()
        {
            return "L" + this.label++;
        }

        public void addLabel(string label)
        {
            this.code.AddLast(this.isFunc + label + ":");
        }

        public void addExpression(string target, string left, string right = "",string operador = "")
        {
            this.code.AddLast(this.isFunc + target + " = " + left + operador + right + ";");
        }

        public void addGoto(string label)
        {
            this.code.AddLast(this.isFunc + "goto " + label+";");
        }

        public void addIf(string left, string right, string operador, string label)
        {
            this.code.AddLast(this.isFunc + "if (" + left + operador + right + ") goto " + label);
        }

        public void nextHeap()
        {
            this.code.AddLast(this.isFunc + "h = h + 1");
        }

        public void addGetHeap(string target, string index)
        {
            this.code.AddLast(this.isFunc + target + " = Heap[" + index + "]");
        }

        public void addSetHeap(string index, string value)
        {
            this.code.AddLast(this.isFunc + "Heap[" + index + "] = " + value + ";");
        }

        public void addGetStack(string target, string index)
        {
            this.code.AddLast(this.isFunc + target + " = Stack[" + index + "];");
        }

        public void addSetStack(string index, string value)
        {
            this.code.AddLast(this.isFunc + "Stack[" + index + "] = " + value + ";");
        }

        public void addNextEnv(int size)
        {
            this.code.AddLast(this.isFunc + "p = p + " + size + ";");
        }

        public void addAntEnv(int size)
        {
            this.code.AddLast(this.isFunc + "p = p - " + size + ";");
        }

        public void addCall(string id)
        {
            this.code.AddLast(this.isFunc + id + "();");
        }

        public void addReturn(string id)
        {
            code.AddLast(this.isFunc + "return " + id + ";");
        }

        public void addStartFunc(string id, string tipo)
        {
            this.code.AddLast(this.isFunc + tipo + " " + id + "() \n" + this.isFunc + "{");
        }

        public void addEndFunc()
        {
            this.code.AddLast(this.isFunc + "}");
        }

        public void addPrint(char format, string value)
        {
            this.code.AddLast(this.isFunc + "printf(\"%" + format + "\"," + value);
        }

        public void addPrintTrue()
        {
            this.addPrint('c', "116"); //t
            this.addPrint('c', "114"); //r
            this.addPrint('c', "117"); //u
            this.addPrint('c', "101"); //e
        }

        public void addPrintFalse()
        {
            this.addPrint('c', "102"); //f
            this.addPrint('c', "97"); //a
            this.addPrint('c', "108"); //l
            this.addPrint('c', "115"); //s
            this.addPrint('c', "101"); //e
        }

        public void addPrintNull()
        {
            this.addPrint('c', "110");
            this.addPrint('c', "117");
            this.addPrint('c', "108");
            this.addPrint('c', "108");
        }

        public void addComment(string comment)
        {
            this.code.AddLast(this.isFunc + "/*****" + comment + "*****/");
        }

        public void addHeader()
        {
            //Ingresar Primero Los Temporales
            this.declararTemporales();

            this.code.AddFirst("float h;");
            this.code.AddFirst("float p;");
            this.code.AddFirst("float Stack[100000];");
            this.code.AddFirst("float Heap[100000];");
            this.code.AddFirst("#include <stdio.h>");
        }

        public void declararTemporales()
        {
            string temps = "";
            int index = 0;
            foreach(string temp in tempStorage)
            {
                if (index == 0)
                {
                    temps = temp;
                    index = 1;
                }
                else
                {
                    temps += "," + temp;
                }
            }

            if (temps != "")
            {
                this.code.AddFirst("float " + temps + ";");
            }
        }

        public void freeTemp(string temp)
        {
            if (this.tempStorage.Contains(temp))
            {
                this.tempStorage.Remove(temp);
            }
        }



    }
}
