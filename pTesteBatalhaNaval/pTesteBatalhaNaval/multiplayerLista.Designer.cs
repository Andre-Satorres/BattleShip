namespace pTesteBatalhaNaval
{
    partial class frmLista
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLista));
            this.btnHost = new System.Windows.Forms.Button();
            this.btnLista = new System.Windows.Forms.Button();
            this.lbTitulo = new System.Windows.Forms.Label();
            this.lbxLista = new System.Windows.Forms.ListBox();
            this.lbLista = new System.Windows.Forms.Label();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.btnRecarregar = new System.Windows.Forms.Button();
            this.lbObs = new System.Windows.Forms.Label();
            this.btnAtras = new System.Windows.Forms.Button();
            this.cpgCarregar = new CircularProgressBar.CircularProgressBar();
            this.lblErro = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnHost
            // 
            this.btnHost.BackColor = System.Drawing.Color.Black;
            this.btnHost.BackgroundImage = global::pTesteBatalhaNaval.Properties.Resources._try;
            this.btnHost.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnHost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHost.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHost.Location = new System.Drawing.Point(270, 140);
            this.btnHost.Name = "btnHost";
            this.btnHost.Size = new System.Drawing.Size(240, 70);
            this.btnHost.TabIndex = 0;
            this.btnHost.Text = "Hospedar Servidor";
            this.btnHost.UseVisualStyleBackColor = false;
            this.btnHost.Click += new System.EventHandler(this.btnHost_Click);
            // 
            // btnLista
            // 
            this.btnLista.BackColor = System.Drawing.Color.Black;
            this.btnLista.BackgroundImage = global::pTesteBatalhaNaval.Properties.Resources._try;
            this.btnLista.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLista.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLista.Font = new System.Drawing.Font("Comic Sans MS", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLista.Location = new System.Drawing.Point(270, 240);
            this.btnLista.Name = "btnLista";
            this.btnLista.Size = new System.Drawing.Size(240, 70);
            this.btnLista.TabIndex = 1;
            this.btnLista.Text = "Lista de Servidores";
            this.btnLista.UseVisualStyleBackColor = false;
            this.btnLista.Click += new System.EventHandler(this.btnLista_Click);
            // 
            // lbTitulo
            // 
            this.lbTitulo.AutoSize = true;
            this.lbTitulo.BackColor = System.Drawing.Color.Transparent;
            this.lbTitulo.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitulo.ForeColor = System.Drawing.SystemColors.Control;
            this.lbTitulo.Location = new System.Drawing.Point(91, 60);
            this.lbTitulo.Name = "lbTitulo";
            this.lbTitulo.Size = new System.Drawing.Size(682, 38);
            this.lbTitulo.TabIndex = 2;
            this.lbTitulo.Text = "Escolha entre: hospedar ou entrar em um servidor";
            // 
            // lbxLista
            // 
            this.lbxLista.BackColor = System.Drawing.SystemColors.WindowText;
            this.lbxLista.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbxLista.ForeColor = System.Drawing.SystemColors.Window;
            this.lbxLista.FormattingEnabled = true;
            this.lbxLista.ItemHeight = 25;
            this.lbxLista.Items.AddRange(new object[] {
            "-> Nome Servidor                  IP            Tempo     Jogadores      Disputa"});
            this.lbxLista.Location = new System.Drawing.Point(12, 63);
            this.lbxLista.Name = "lbxLista";
            this.lbxLista.Size = new System.Drawing.Size(760, 354);
            this.lbxLista.TabIndex = 3;
            this.lbxLista.Visible = false;
            this.lbxLista.DoubleClick += new System.EventHandler(this.lbxLista_DoubleClick);
            // 
            // lbLista
            // 
            this.lbLista.AutoSize = true;
            this.lbLista.BackColor = System.Drawing.Color.Transparent;
            this.lbLista.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLista.ForeColor = System.Drawing.SystemColors.Control;
            this.lbLista.Location = new System.Drawing.Point(118, 10);
            this.lbLista.Name = "lbLista";
            this.lbLista.Size = new System.Drawing.Size(590, 30);
            this.lbLista.TabIndex = 4;
            this.lbLista.Text = "Lista de Servidores Disponíveis para o Modo Multiplayer";
            this.lbLista.Visible = false;
            // 
            // btnVoltar
            // 
            this.btnVoltar.BackColor = System.Drawing.Color.Black;
            this.btnVoltar.BackgroundImage = global::pTesteBatalhaNaval.Properties.Resources._try;
            this.btnVoltar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnVoltar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVoltar.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoltar.Location = new System.Drawing.Point(178, 425);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(142, 49);
            this.btnVoltar.TabIndex = 5;
            this.btnVoltar.Text = "Voltar";
            this.btnVoltar.UseVisualStyleBackColor = false;
            this.btnVoltar.Visible = false;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // btnRecarregar
            // 
            this.btnRecarregar.BackColor = System.Drawing.Color.Black;
            this.btnRecarregar.BackgroundImage = global::pTesteBatalhaNaval.Properties.Resources._try;
            this.btnRecarregar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRecarregar.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnRecarregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecarregar.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRecarregar.Location = new System.Drawing.Point(465, 423);
            this.btnRecarregar.Name = "btnRecarregar";
            this.btnRecarregar.Size = new System.Drawing.Size(154, 51);
            this.btnRecarregar.TabIndex = 6;
            this.btnRecarregar.Text = "Recarregar";
            this.btnRecarregar.UseVisualStyleBackColor = false;
            this.btnRecarregar.Visible = false;
            this.btnRecarregar.Click += new System.EventHandler(this.btnRecarregar_Click);
            // 
            // lbObs
            // 
            this.lbObs.AutoSize = true;
            this.lbObs.BackColor = System.Drawing.Color.Transparent;
            this.lbObs.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbObs.ForeColor = System.Drawing.SystemColors.Control;
            this.lbObs.Location = new System.Drawing.Point(11, 41);
            this.lbObs.Name = "lbObs";
            this.lbObs.Size = new System.Drawing.Size(358, 21);
            this.lbObs.TabIndex = 7;
            this.lbObs.Text = "Obs: Clique duas vezes para entrar no servidor!";
            this.lbObs.Visible = false;
            // 
            // btnAtras
            // 
            this.btnAtras.BackColor = System.Drawing.Color.Black;
            this.btnAtras.BackgroundImage = global::pTesteBatalhaNaval.Properties.Resources._try;
            this.btnAtras.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAtras.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAtras.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAtras.Location = new System.Drawing.Point(270, 340);
            this.btnAtras.Name = "btnAtras";
            this.btnAtras.Size = new System.Drawing.Size(240, 70);
            this.btnAtras.TabIndex = 8;
            this.btnAtras.Text = "Voltar";
            this.btnAtras.UseVisualStyleBackColor = false;
            this.btnAtras.Click += new System.EventHandler(this.btnAtras_Click);
            // 
            // cpgCarregar
            // 
            this.cpgCarregar.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.Liner;
            this.cpgCarregar.AnimationSpeed = 500;
            this.cpgCarregar.BackColor = System.Drawing.Color.Transparent;
            this.cpgCarregar.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold);
            this.cpgCarregar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cpgCarregar.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cpgCarregar.InnerMargin = 2;
            this.cpgCarregar.InnerWidth = -1;
            this.cpgCarregar.Location = new System.Drawing.Point(226, 117);
            this.cpgCarregar.MarqueeAnimationSpeed = 2000;
            this.cpgCarregar.Name = "cpgCarregar";
            this.cpgCarregar.OuterColor = System.Drawing.Color.Silver;
            this.cpgCarregar.OuterMargin = -25;
            this.cpgCarregar.OuterWidth = 26;
            this.cpgCarregar.ProgressColor = System.Drawing.Color.RoyalBlue;
            this.cpgCarregar.ProgressWidth = 10;
            this.cpgCarregar.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 36F);
            this.cpgCarregar.Size = new System.Drawing.Size(300, 300);
            this.cpgCarregar.StartAngle = 270;
            this.cpgCarregar.Step = 1;
            this.cpgCarregar.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.cpgCarregar.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.cpgCarregar.SubscriptText = "";
            this.cpgCarregar.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.cpgCarregar.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.cpgCarregar.SuperscriptText = "";
            this.cpgCarregar.TabIndex = 9;
            this.cpgCarregar.Text = "0%";
            this.cpgCarregar.TextMargin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.cpgCarregar.Value = 68;
            this.cpgCarregar.Visible = false;
            // 
            // lblErro
            // 
            this.lblErro.AutoSize = true;
            this.lblErro.BackColor = System.Drawing.Color.Transparent;
            this.lblErro.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErro.ForeColor = System.Drawing.Color.Red;
            this.lblErro.Location = new System.Drawing.Point(392, 41);
            this.lblErro.Name = "lblErro";
            this.lblErro.Size = new System.Drawing.Size(144, 21);
            this.lblErro.TabIndex = 10;
            this.lblErro.Text = "Mensagem de Erro";
            this.lblErro.Visible = false;
            // 
            // frmLista
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::pTesteBatalhaNaval.Properties.Resources.formLista;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(784, 486);
            this.Controls.Add(this.lbxLista);
            this.Controls.Add(this.cpgCarregar);
            this.Controls.Add(this.lblErro);
            this.Controls.Add(this.btnVoltar);
            this.Controls.Add(this.btnRecarregar);
            this.Controls.Add(this.lbObs);
            this.Controls.Add(this.lbLista);
            this.Controls.Add(this.lbTitulo);
            this.Controls.Add(this.btnHost);
            this.Controls.Add(this.btnLista);
            this.Controls.Add(this.btnAtras);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLista";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Batalha Naval: Modo Multiplayer Opções";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLista_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnHost;
        private System.Windows.Forms.Button btnLista;
        private System.Windows.Forms.Label lbTitulo;
        private System.Windows.Forms.ListBox lbxLista;
        private System.Windows.Forms.Label lbLista;
        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.Button btnRecarregar;
        private System.Windows.Forms.Label lbObs;
        private System.Windows.Forms.Button btnAtras;
        private CircularProgressBar.CircularProgressBar cpgCarregar;
        private System.Windows.Forms.Label lblErro;
    }
}