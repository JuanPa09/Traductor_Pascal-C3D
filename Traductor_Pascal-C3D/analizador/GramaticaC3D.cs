using System;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;

namespace Traductor_Pascal_C3D.analizador
{
    class GramaticaC3D  : Grammar
    {
        public GramaticaC3D() : base(caseSensitive:false)
        {
            #region ER
            var Identificador = new IdentifierTerminal("ID");
            //var Identificador = new RegexBasedTerminal("^(?![T|L][0-9]+).*[_a-zA-Z][_a-zA-Z0-9]*");
            var Entero = new NumberLiteral("INT");
            var Decimal = new RegexBasedTerminal("DOUBLE", "[0-9]+[.][0-9]+");
            var Asteriscos = new RegexBasedTerminal("COMENTARIO", "/[*]+[\\s]+[_a-zA-Z][_a-zA-Z0-9]*[\\s]+[*]/");
            var Tmp = new RegexBasedTerminal("[T][0-9]+");
            var Lbl = new RegexBasedTerminal("[L][0-9]+");
            var comentarios = new CommentTerminal("Comentario", "/*", "*/");
            #endregion

            #region Terminales
            //var Tmp = ToTerm("T"+Entero.ToString());
            //var Lbl = ToTerm("L");
            var _goto = ToTerm("goto");
            var CorIzq = ToTerm("[");
            var CorDer = ToTerm("]");
            var LlavIzq = ToTerm("{");
            var LlavDer = ToTerm("}");
            var ParIzq = ToTerm("(");
            var ParDer = ToTerm(")");
            var Coma = ToTerm(",");
            var PtComa = ToTerm(";");
            var dsPts = ToTerm(":");
            var Pt = ToTerm(".");
            var Print = ToTerm("printf");
            var charPrint = ToTerm("\"%c\"");
            var decPrint = ToTerm("\"%d\"");
            var doubPrint = ToTerm("\"%f\"");
            var punteroStack = ToTerm("p");
            var punteroHeap = ToTerm("h");
            var _if = ToTerm("if");
            var _void = ToTerm("void");
            var num = ToTerm("#");
            var por = ToTerm("*");
            var div = ToTerm("/");
            var mod = ToTerm("%");
            var mas = ToTerm("+");
            var menos = ToTerm("-");
            var minus = ToTerm("-");
            var menIg = ToTerm("<=");
            var men = ToTerm("<");
            var mayIg = ToTerm(">=");
            var may = ToTerm(">");
            var dif = ToTerm("!=");
            var ig = ToTerm("==");
            var igual = ToTerm("=");
            var entero = ToTerm("int");
            var stack = ToTerm("Stack");
            var heap = ToTerm("Heap");
            var _float = ToTerm("float");
            var _return = ToTerm("return");
            NonGrammarTerminals.Add(comentarios);
            #endregion

            #region NoTerminales
            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal ELEMENTO = new NonTerminal("ELEMENTO");
            NonTerminal ELEMENTOS = new NonTerminal("ELEMENTOS");
            NonTerminal INSTRUCCION = new NonTerminal("INSTRUCCION");
            NonTerminal INSTRUCCIONES = new NonTerminal("INSTRUCCIONES");
            NonTerminal SALTOS = new NonTerminal("SALTOS");
            NonTerminal ASIGNACION = new NonTerminal("ASIGNACION");
            NonTerminal LLAMADA = new NonTerminal("LLAMADA");
            NonTerminal PRINT = new NonTerminal("PRINT");
            NonTerminal OPERACION = new NonTerminal("OPERACION");
            NonTerminal ETIQUETA = new NonTerminal("ETIQUETA");
            NonTerminal CONDICIONAL = new NonTerminal("CONDICIONAL");
            NonTerminal INCONDICIONAL = new NonTerminal("INCONDICIONAL");
            NonTerminal PARAMETROPRINT = new NonTerminal("PARAMETROPRINT");
            NonTerminal STRUCTURA = new NonTerminal("STRUCTURA");
            NonTerminal TIPOSALTO = new NonTerminal("TIPOSALTO");
            NonTerminal VALOR = new NonTerminal("VALOR");
            NonTerminal VARIABLE = new NonTerminal("VARIABLE");
            NonTerminal METODO = new NonTerminal("METODO");
            NonTerminal ESTRUCTURA = new NonTerminal("ESTRUCTURA");
            NonTerminal VALORESTRUCTURA = new NonTerminal("VALORESTRUCTURA");
            NonTerminal ENCABEZADO = new NonTerminal("ENCABEZADO");
            NonTerminal TEMPORALES = new NonTerminal("TEMPORALES");
            #endregion

            #region Gramatica
            INICIO.Rule
                                = ENCABEZADO + ELEMENTOS
                                ;

            ELEMENTOS.Rule
                                = /*ELEMENTOS + ELEMENTO*/MakePlusRule(ELEMENTOS, ELEMENTO)
                                //| ELEMENTO
                                ;

            ELEMENTO.Rule
                                = INSTRUCCION
                                | METODO
                                ;

            INSTRUCCIONES.Rule
                                = /*INSTRUCCIONES + INSTRUCCION*/MakePlusRule(INSTRUCCIONES, INSTRUCCION)
                                //| INSTRUCCION
                                ;

            INSTRUCCION.Rule
                                = LLAMADA
                                | ASIGNACION
                                | SALTOS
                                | PRINT
                                | OPERACION
                                | ETIQUETA
                                ;

            SALTOS.Rule
                                = CONDICIONAL
                                | INCONDICIONAL
                                ;

            CONDICIONAL.Rule
                                = _if + ParIzq + VALOR + TIPOSALTO + VALOR + ParDer + _goto + Lbl + PtComa + INCONDICIONAL
                                | _if + ParIzq + VALOR + TIPOSALTO + VALOR + ParDer + _goto + Lbl + PtComa
                                ;

            INCONDICIONAL.Rule
                                = _goto + Lbl + PtComa
                                ;

            ETIQUETA.Rule
                                = Lbl + dsPts
                                ;

            ASIGNACION.Rule
                                = ESTRUCTURA + CorIzq + ParIzq + entero + ParDer + VALOR + CorDer + igual + VALOR + PtComa
                                | VARIABLE + igual + VALOR + PtComa
                                | VARIABLE + igual + ESTRUCTURA + CorIzq + ParIzq + entero + ParDer + VALOR + CorDer + PtComa
                                ;

            METODO.Rule
                                = _void + Identificador + ParIzq + ParDer + LlavIzq + INSTRUCCIONES + _return + PtComa + LlavDer
                                | _void + Identificador + ParIzq + ParDer + LlavIzq + _return + PtComa + LlavDer
                                ;

            LLAMADA.Rule
                                = Identificador + ParIzq + ParDer + PtComa
                                ;
            
            PRINT.Rule
                                = Print + ParIzq + PARAMETROPRINT + Coma + ParIzq + entero + ParDer + VALOR + ParDer + PtComa
                                | Print + ParIzq + doubPrint + Coma + VALOR + ParIzq + PtComa
                                ;

            PARAMETROPRINT.Rule
                                = charPrint
                                | decPrint
                                
                                ;

            ESTRUCTURA.Rule
                                = stack
                                | heap
                                ;

            TIPOSALTO.Rule
                                = menIg
                                | men
                                | mayIg
                                | may
                                | ig
                                | dif
                                ;

            VALOR.Rule
                                = Entero
                                | Decimal
                                | VARIABLE
                                | minus + Entero
                                | minus + Decimal
                                | minus + VARIABLE
                                //| ESTRUCTURA + CorIzq + ParIzq + entero + ParDer + VALOR + CorDer
                                ;

            VARIABLE.Rule
                                = Tmp
                                | punteroHeap
                                | punteroStack
                                ;

            OPERACION.Rule
                                = VARIABLE + igual + VALOR + mas + VALOR + PtComa
                                | VARIABLE + igual + VALOR + menos + VALOR + PtComa
                                | VARIABLE + igual + VALOR + div + VALOR + PtComa
                                | VARIABLE + igual + VALOR + por + VALOR + PtComa
                                | VARIABLE + igual + VALOR + mod + VALOR + PtComa
                                ;

            ENCABEZADO.Rule
                                = num + Identificador + men + Identificador + Pt + Identificador + may + _float + ESTRUCTURA + CorIzq + Entero + CorDer + PtComa + _float + ESTRUCTURA + CorIzq + Entero + CorDer + PtComa + _float + VARIABLE + PtComa + _float + VARIABLE + PtComa + _float + TEMPORALES + PtComa
                                | num + Identificador + men + Identificador + Pt + Identificador + may + _float + ESTRUCTURA + CorIzq + Entero + CorDer + PtComa + _float + ESTRUCTURA + CorIzq + Entero + CorDer + PtComa + _float + VARIABLE + PtComa + _float + VARIABLE + PtComa
                                ;

            TEMPORALES.Rule
                                = TEMPORALES + Coma + Tmp
                                | Tmp
                                ;
            /*VALORESTRUCTURA.Rule
                                = ESTRUCTURA + CorIzq + ParIzq + entero + ParDer + VALOR + CorDer
                                ;*/

            #endregion

            #region Preferencias
            this.Root = INICIO;
            this.RegisterOperators(0,minus,mas,menos);


            #endregion


        }
    }
}
