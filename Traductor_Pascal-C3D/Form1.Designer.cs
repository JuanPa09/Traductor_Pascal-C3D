
namespace Traductor_Pascal_C3D
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.LinearNumberEntrada = new System.Windows.Forms.RichTextBox();
            this.Entrada = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LinearNumberSalida = new System.Windows.Forms.RichTextBox();
            this.Salida = new System.Windows.Forms.RichTextBox();
            this.Debugger = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Traducir";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(93, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Optimizar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(174, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(102, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Reporte Errores";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(519, 11);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(42, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "AST";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // LinearNumberEntrada
            // 
            this.LinearNumberEntrada.BackColor = System.Drawing.SystemColors.Control;
            this.LinearNumberEntrada.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LinearNumberEntrada.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.LinearNumberEntrada.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.LinearNumberEntrada.Location = new System.Drawing.Point(12, 56);
            this.LinearNumberEntrada.Name = "LinearNumberEntrada";
            this.LinearNumberEntrada.ReadOnly = true;
            this.LinearNumberEntrada.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.LinearNumberEntrada.Size = new System.Drawing.Size(47, 521);
            this.LinearNumberEntrada.TabIndex = 4;
            this.LinearNumberEntrada.Text = "";
            // 
            // Entrada
            // 
            this.Entrada.AcceptsTab = true;
            this.Entrada.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Entrada.Location = new System.Drawing.Point(59, 56);
            this.Entrada.Name = "Entrada";
            this.Entrada.Size = new System.Drawing.Size(434, 521);
            this.Entrada.TabIndex = 5;
            this.Entrada.Text = "";
            this.Entrada.WordWrap = false;
            this.Entrada.SelectionChanged += new System.EventHandler(this.Entrada_SelectionChanged);
            this.Entrada.VScroll += new System.EventHandler(this.Entrada_VScroll);
            this.Entrada.TextChanged += new System.EventHandler(this.Entrada_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Entrada:";
            // 
            // LinearNumberSalida
            // 
            this.LinearNumberSalida.BackColor = System.Drawing.SystemColors.Control;
            this.LinearNumberSalida.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LinearNumberSalida.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.LinearNumberSalida.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.LinearNumberSalida.Location = new System.Drawing.Point(514, 56);
            this.LinearNumberSalida.Name = "LinearNumberSalida";
            this.LinearNumberSalida.ReadOnly = true;
            this.LinearNumberSalida.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.LinearNumberSalida.Size = new System.Drawing.Size(47, 521);
            this.LinearNumberSalida.TabIndex = 7;
            this.LinearNumberSalida.Text = "";
            // 
            // Salida
            // 
            this.Salida.AcceptsTab = true;
            this.Salida.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Salida.Location = new System.Drawing.Point(561, 56);
            this.Salida.Name = "Salida";
            this.Salida.Size = new System.Drawing.Size(434, 521);
            this.Salida.TabIndex = 8;
            this.Salida.Text = "";
            this.Salida.WordWrap = false;
            this.Salida.SelectionChanged += new System.EventHandler(this.Salida_SelectionChanged);
            this.Salida.VScroll += new System.EventHandler(this.Salida_VScroll);
            this.Salida.TextChanged += new System.EventHandler(this.Salida_TextChanged);
            // 
            // Debugger
            // 
            this.Debugger.BackColor = System.Drawing.Color.Bisque;
            this.Debugger.Location = new System.Drawing.Point(12, 597);
            this.Debugger.Name = "Debugger";
            this.Debugger.Size = new System.Drawing.Size(983, 126);
            this.Debugger.TabIndex = 9;
            this.Debugger.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(561, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "Salida:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 580);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "Consola:";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(875, 27);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(120, 23);
            this.button5.TabIndex = 12;
            this.button5.Text = "Seleccionar Todo";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(282, 12);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(130, 23);
            this.button6.TabIndex = 13;
            this.button6.Text = "Reporte Optimización";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(419, 11);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(94, 23);
            this.button7.TabIndex = 14;
            this.button7.Text = "Tabla Símbolos";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1007, 735);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Debugger);
            this.Controls.Add(this.Salida);
            this.Controls.Add(this.LinearNumberSalida);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Entrada);
            this.Controls.Add(this.LinearNumberEntrada);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.RichTextBox LinearNumberEntrada;
        private System.Windows.Forms.RichTextBox Entrada;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox LinearNumberSalida;
        private System.Windows.Forms.RichTextBox Salida;
        private System.Windows.Forms.RichTextBox Debugger;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
    }
}

