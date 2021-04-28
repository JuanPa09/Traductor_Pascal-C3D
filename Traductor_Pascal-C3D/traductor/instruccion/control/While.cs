﻿using System;
using System.Collections.Generic;
using System.Text;
using Traductor_Pascal_C3D.traductor.abstractas;
using Traductor_Pascal_C3D.traductor.utils;
using Traductor_Pascal_C3D.traductor.tablaSimbolos;
using Traductor_Pascal_C3D.traductor.generador;
using Traductor_Pascal_C3D.traductor.reportes;

namespace Traductor_Pascal_C3D.traductor.instruccion.control
{
    class While : Instruccion
    {
        private Expresion condicion;
        private LinkedList<Instruccion> instrucciones;

        public While(Expresion condicion, LinkedList<Instruccion> instrucciones, int line, int column) : base(line, column)
        {
            this.instrucciones = instrucciones;
            this.condicion = condicion;
        }

        public override object compile(Entorno entorno, Reporte reporte)
        {
            Generador generator = Generador.getInstance();
            generator.addComment("Inicia Repeat");
            entorno._continue = this.condicion.trueLabel = generator.newLabel();
            entorno._break = this.condicion.falseLabel = generator.newLabel();
            generator.addLabel(this.condicion.trueLabel);
            foreach(Instruccion instruccion in instrucciones)
            {
                instruccion.compile(entorno, reporte);
            }
            Retorno condition = this.condicion.compile(entorno);
            if (condition.type.type == Types.BOOLEAN)
            {
                generator.addLabel(condition.falseLabel);
                generator.addComment("Finaliza Repeat");
                return null;
            }
            throw new ErroPascal(this.line, this.column, "La condicion no es booleana", "Semantico");
        }
    }
}
