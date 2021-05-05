using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;

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
        private LinkedList<string> tempStorage1; //Quitar linea

        private Generador()
        {
            this.temporal = 5;
            this.label = 3;
            this.code = new LinkedList<string>();
            this.tempStorage = new LinkedList<string>();
            this.tempStorage1 = new LinkedList<string>(); //Quitar linea
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
            this.temporal = 5;
            this.label = 3;
            this.code = new LinkedList<string>();
            this.tempStorage = new LinkedList<string>();
            this.tempStorage1 = new LinkedList<string>();
        }

        public void addCode(string code)
        {
            this.code.AddLast(this.isFunc + code);
        }
        
        public string getCode()
        {
            return string.Join('\n', this.code);
        }

        public string newTemporal()
        {
             string temp = "T" + this.temporal++;
            this.tempStorage.AddLast(temp);
            this.tempStorage1.AddLast(temp);
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
            this.code.AddLast(this.isFunc + "if (" + left + operador + right + ") goto " + label + ";");
        }

        public void nextHeap()
        {
            this.code.AddLast(this.isFunc + "h = h + 1;");
        }

        public void addGetHeap(string target, string index)
        {
            this.code.AddLast(this.isFunc + target + " = Heap[(int)" + index + "];");
        }

        public void addSetHeap(string index, string value)
        {
            this.code.AddLast(this.isFunc + "Heap[(int)" + index + "] = " + value + ";");
        }

        public void addGetStack(string target, string index)
        {
            this.code.AddLast(this.isFunc + target + " = Stack[(int)" + index + "];");
        }

        public void addSetStack(string index, string value)
        {
            this.code.AddLast(this.isFunc + "Stack[(int)" + index + "] = " + value + ";");
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
            this.addTab();
        }

        public void addEndFunc()
        {
            this.removeTab();
            this.code.AddLast(this.isFunc + "}\n");
        }

        public void addPrint(char format, string value)
        {
            if(format=='c' || format == 'd')
            {
                this.code.AddLast(this.isFunc + "printf(\"%" + format + "\", (int)" + value + ");");
            }
            else
            {
                this.code.AddLast(this.isFunc + "printf(\"%" + format + "\"," + value + ");");
            }
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

            this.code.AddFirst("float h;\n");
            this.code.AddFirst("float p;");
            this.code.AddFirst("float Stack[100000];");
            this.code.AddFirst("float Heap[100000];");
            this.code.AddFirst("#include <stdio.h>");
        }

        public void declararTemporales()
        {
            string temps = "";
            /************* Temporales Por Defecto *************/
            temps += "T0,T1,T2,T3,T4";
            /**************************************************/
            int index = 1;
            foreach(string temp in tempStorage1)
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
                this.code.AddFirst("float " + temps + ";\n");
            }
        }

        public void freeTemp(string temp)
        {

            //Comentar todo si hay algun error
            if (this.tempStorage.Contains(temp))
            {
                this.tempStorage.Remove(temp);
            }
        }

        public void addTab()
        {
            this.isFunc += "\t"; 
        }

        public void addTemp(string temp)
        {
            if (!this.tempStorage1.Contains(temp)) //Quitar linea
                this.tempStorage1.AddLast(temp); //Quitar linea
            if (!this.tempStorage.Contains(temp))
                this.tempStorage.AddLast(temp);
        }

        public void removeTab()
        {
            this.isFunc = this.isFunc.Remove(this.isFunc.Length - 1);
        }

        public int saveTemps(Entorno entorno)
        {
            if(this.tempStorage.Count > 0)
            {
                string temp = this.newTemporal();
                this.freeTemp(temp);
                int size = 0;

                this.addComment("Inicia Guardado De Temporales");
                this.addExpression(temp, "p", entorno.size.ToString(), "+");
                foreach(string value in tempStorage)
                {
                    size++;
                    this.addSetStack(temp, value);
                    if (size != this.tempStorage.Count)
                        this.addExpression(temp, temp, "1", "+");
                }
                this.addComment("Fin guardado de temporales");
            }
            int ptr = entorno.size;
            entorno.size = ptr + this.tempStorage.Count;
            return ptr;
        }

        public void recoverTemps(Entorno entorno,int pos)
        {
            if (this.tempStorage.Count > 0)
            {
                string temp = this.newTemporal();
                this.freeTemp(temp);
                int size = 0;

                this.addComment("Inicia recuperado de temporales");
                this.addExpression(temp, "p", pos.ToString(), "+");
                foreach(string value in tempStorage){
                    size++;
                    this.addGetStack(value, temp);
                    if (size != this.tempStorage.Count)
                        this.addExpression(temp, temp, "1", "+");
                }
                this.addComment("Finaliza recuperado de temporales");
                entorno.size = pos;
            }
        }

        public string removeLast()
        {
            string retorno  = this.code.Last.Value;
            this.code.RemoveLast();
            return retorno;
        }

        /*public void addCode(string code)
        {
            this.code.AddLast(code);
        }*/

        /*-----Funciones Nativas-----*/

        public void addNativaPrint()
        {
            addStartFunc("native_print_str", "void");
            /*
             L0:
                T0 = H //Poner El primer valor en T0
             if (T0 != -1) goto L1
             print(T0);
            T0 = T0 + 1
            goto L0:
            L1:
             */
            this.code.AddLast(this.isFunc + "L0:");
            addExpression("T1", "Heap[(int)T0]");
            this.code.AddLast(this.isFunc + "if (T1 == -1) goto L1;");
            this.addPrint('c', "T1");
            this.code.AddLast(this.isFunc + "T0 = T0 + 1;");
            this.code.AddLast(this.isFunc + "goto L0;");
            this.code.AddLast(this.isFunc + "L1:");
            addReturn("");
            addEndFunc();
        }

        public void addNativaCompareString()
        {
            /*
             T0 -> Posicion de la primera letra en el heap (string 1)
             T1 -> Posicion de la primera letra en el heap (string 2)
             T2 -> Resultado (1:correcta; 0:incorrecta)
            T2 = 0
            L0:
            if ((int)Heap[(int)T0] == (int)Heap[(int)T1]) goto L1;
            goto L2;
            L1:
                if ((int)Heap[(int)T0] == -1) goto L3
                T0 = T0 + 1;
                T1 = T1 + 1;
                goto L0;
            L3:
                T2 = 1
            L2:
             */
            addStartFunc("native_compare_str_str", "void");
            addExpression("T2", "0", "", "");
            addLabel("L0");
            addExpression("T3", "Heap[(int)T1]");
            addExpression("T4", "Heap[(int)T0]");
            addIf("T4", "T3","==","L1");
            addGoto("L2");
            addLabel("L1");
            addIf("T4","-1","==","L3");
            addExpression("T0", "T0", "1", "+");
            addExpression("T1", "T1", "1", "+");
            addGoto("L0");
            addLabel("L3");
            addExpression("T2","1","","");
            addLabel("L2");
            addReturn("");
            addEndFunc();

        }

        public void addNativa_Concat_Str_Str()
        {
            /*void native_concat_str_str()
            {
                //T1, T2, T3
                //T4

                /*T3 = h; //valor de retorno

            L0:
                T4 = Heap[(int)T1];
                if (T4 == -1) goto L1;
                Heap[(int)h] = T4;
                T1 = T1 + 1;
                h = h + 1;
                goto L0;

            L1:
                T4 = Heap[(int)T2];
                if (T4 == -1) goto L2;
                Heap[(int)h] = T4;
                T2 = T2 + 1;
                h = h + 1;
                goto L1;

            L2:
                Heap[(int)h] = -1; //Fin de cadena
                h = h + 1;
            }*/
            addStartFunc("native_concat_str_str", "void");
            addExpression("T3", "h");
            addLabel("L0");
            addGetHeap("T4","T1");
            addIf("T4", "-1", "==","L1");
            addSetHeap("h", "T4");
            addExpression("T1", "T1", "1", "+");
            nextHeap();
            addGoto("L0");
            addLabel("L1");
            addGetHeap("T4","T2");
            addIf("T4", "-1", "==", "L2");
            addSetHeap("h", "T4");
            addExpression("T2", "T2", "1", "+");
            nextHeap();
            addGoto("L1");
            addLabel("L2");
            addSetHeap("h", "-1");
            nextHeap();
            addReturn("");
            addEndFunc();
        }

        public void addNativa_Concat_Str_Num()
        {
            /*void native_concat_str_num()
            {
                //T1, T2, T3
                //T4

                /*T3 = h; //valor de retorno

            L0:
                T4 = Heap[(int)T1];
                if (T4 == -1) goto L1;
                Heap[(int)h] = T4;
                T1 = T1 + 1;
                h = h + 1;
                goto L0;

            L1:
                T4 = Heap[(int)T2]
                Heap[(int)h] = T4;
                h = h + 1;

            L2:
                Heap[(int)h] = -1; //Fin de cadena
                h = h + 1;
            }*/
            addStartFunc("native_concat_str_num", "void");
            addExpression("T3", "h");
            addLabel("L0");
            addGetHeap("T4", "T1");
            addIf("T4", "-1", "==", "L1");
            addSetHeap("h", "T4");
            addExpression("T1", "T1", "1", "+");
            nextHeap();
            addGoto("L0");
            addLabel("L1");
            addGetHeap("T4", "T2");
            addSetHeap("h", "T4");
            nextHeap();
            addLabel("L2");
            addSetHeap("h", "-1");
            nextHeap();
            addReturn("");
            addEndFunc();
        }


        string getNumCode(string num)
        {
            switch (num)
            {
                case "1":
                    return "49";
                case "2":
                    return "50";
                case "3":
                    return "51";
                case "4":
                    return "52";
                case "5":
                    return null;
                case "6":
                    return null;
                case "7":
                    return null;
                case "8":
                    return null;
                case "9":
                    return null;
                default:
                    return "48";
            }
        }

    }
}
