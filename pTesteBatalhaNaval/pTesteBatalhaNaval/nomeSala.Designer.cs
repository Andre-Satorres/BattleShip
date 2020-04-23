namespace pTesteBatalhaNaval
{
    partial class frmNomeSala
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNomeSala));
            this.lblTitulo = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblObs = new System.Windows.Forms.Label();
            this.lblErro = new System.Windows.Forms.Label();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.lblTitulo.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTitulo.Location = new System.Drawing.Point(15, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(450, 33);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Digite o nome do Servidor a ser criado:";
            // 
            // txtNome
            // 
            this.txtNome.Location = new System.Drawing.Point(78, 95);
            this.txtNome.MaxLength = 20;
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(391, 23);
            this.txtNome.TabIndex = 1;
            this.txtNome.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNome_KeyDown);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblInfo.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.ForeColor = System.Drawing.SystemColors.Control;
            this.lblInfo.Location = new System.Drawing.Point(17, 95);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(55, 23);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "Nome:";
            // 
            // lblObs
            // 
            this.lblObs.AutoSize = true;
            this.lblObs.BackColor = System.Drawing.Color.Transparent;
            this.lblObs.ForeColor = System.Drawing.SystemColors.Control;
            this.lblObs.Location = new System.Drawing.Point(21, 52);
            this.lblObs.Name = "lblObs";
            this.lblObs.Size = new System.Drawing.Size(130, 15);
            this.lblObs.TabIndex = 3;
            this.lblObs.Text = "Observação: IP Local -> ";
            // 
            // lblErro
            // 
            this.lblErro.AutoSize = true;
            this.lblErro.BackColor = System.Drawing.Color.Transparent;
            this.lblErro.Font = new System.Drawing.Font("Comic Sans MS", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErro.ForeColor = System.Drawing.Color.Red;
            this.lblErro.Location = new System.Drawing.Point(78, 73);
            this.lblErro.Name = "lblErro";
            this.lblErro.Size = new System.Drawing.Size(141, 19);
            this.lblErro.TabIndex = 9;
            this.lblErro.Text = "Mensagem de Erro:";
            this.lblErro.Visible = false;
            // 
            // btnVoltar
            // 
            this.btnVoltar.BackColor = System.Drawing.Color.Black;
            this.btnVoltar.BackgroundImage = global::pTesteBatalhaNaval.Properties.Resources._try;
            this.btnVoltar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnVoltar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVoltar.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoltar.Location = new System.Drawing.Point(80, 140);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(109, 49);
            this.btnVoltar.TabIndex = 7;
            this.btnVoltar.Text = "Voltar";
            this.btnVoltar.UseVisualStyleBackColor = false;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Black;
            this.btnOK.BackgroundImage = global::pTesteBatalhaNaval.Properties.Resources._try;
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOK.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(300, 140);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(114, 51);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmNomeSala
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::pTesteBatalhaNaval.Properties.Resources.submarinoPlano;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(481, 213);
            this.Controls.Add(this.lblErro);
            this.Controls.Add(this.btnVoltar);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblObs);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.txtNome);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmNomeSala";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Modo Multiplayer: Nome do Servidor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmNomeSala_FormClosed);
            this.Load += new System.EventHandler(this.frmNomeSala_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblObs;
        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblErro;
    }
}