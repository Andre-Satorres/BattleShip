namespace pTesteBatalhaNaval
{
    partial class frmNivel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNivel));
            this.lbTitulo = new System.Windows.Forms.Label();
            this.pbxNacao1 = new System.Windows.Forms.PictureBox();
            this.pbxNacao2 = new System.Windows.Forms.PictureBox();
            this.lbTexto = new System.Windows.Forms.Label();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.pbVS = new System.Windows.Forms.PictureBox();
            this.pbxAudio = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbxNacao1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxNacao2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxAudio)).BeginInit();
            this.SuspendLayout();
            // 
            // lbTitulo
            // 
            this.lbTitulo.BackColor = System.Drawing.Color.Red;
            this.lbTitulo.Font = new System.Drawing.Font("Comic Sans MS", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitulo.ForeColor = System.Drawing.Color.Black;
            this.lbTitulo.Location = new System.Drawing.Point(0, 0);
            this.lbTitulo.Name = "lbTitulo";
            this.lbTitulo.Size = new System.Drawing.Size(684, 65);
            this.lbTitulo.TabIndex = 0;
            this.lbTitulo.Text = "-";
            this.lbTitulo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pbxNacao1
            // 
            this.pbxNacao1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxNacao1.Location = new System.Drawing.Point(-80, 80);
            this.pbxNacao1.Name = "pbxNacao1";
            this.pbxNacao1.Size = new System.Drawing.Size(250, 270);
            this.pbxNacao1.TabIndex = 1;
            this.pbxNacao1.TabStop = false;
            this.pbxNacao1.Visible = false;
            // 
            // pbxNacao2
            // 
            this.pbxNacao2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxNacao2.Location = new System.Drawing.Point(520, 80);
            this.pbxNacao2.Name = "pbxNacao2";
            this.pbxNacao2.Size = new System.Drawing.Size(250, 270);
            this.pbxNacao2.TabIndex = 2;
            this.pbxNacao2.TabStop = false;
            this.pbxNacao2.Visible = false;
            // 
            // lbTexto
            // 
            this.lbTexto.BackColor = System.Drawing.Color.LightCyan;
            this.lbTexto.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTexto.ForeColor = System.Drawing.Color.Black;
            this.lbTexto.Location = new System.Drawing.Point(73, 67);
            this.lbTexto.Name = "lbTexto";
            this.lbTexto.Size = new System.Drawing.Size(518, 261);
            this.lbTexto.TabIndex = 3;
            this.lbTexto.Text = "label1";
            // 
            // btnIniciar
            // 
            this.btnIniciar.BackColor = System.Drawing.Color.Transparent;
            this.btnIniciar.BackgroundImage = global::pTesteBatalhaNaval.Properties.Resources._try;
            this.btnIniciar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIniciar.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIniciar.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnIniciar.Location = new System.Drawing.Point(280, 331);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(140, 50);
            this.btnIniciar.TabIndex = 4;
            this.btnIniciar.Text = "Lutar!!";
            this.btnIniciar.UseVisualStyleBackColor = false;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // pbVS
            // 
            this.pbVS.BackColor = System.Drawing.Color.Transparent;
            this.pbVS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbVS.Location = new System.Drawing.Point(284, 127);
            this.pbVS.Name = "pbVS";
            this.pbVS.Size = new System.Drawing.Size(110, 149);
            this.pbVS.TabIndex = 5;
            this.pbVS.TabStop = false;
            this.pbVS.Visible = false;
            // 
            // pbxAudio
            // 
            this.pbxAudio.BackColor = System.Drawing.Color.Red;
            this.pbxAudio.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbxAudio.BackgroundImage")));
            this.pbxAudio.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxAudio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbxAudio.Location = new System.Drawing.Point(642, 12);
            this.pbxAudio.Name = "pbxAudio";
            this.pbxAudio.Size = new System.Drawing.Size(30, 30);
            this.pbxAudio.TabIndex = 25;
            this.pbxAudio.TabStop = false;
            this.pbxAudio.Click += new System.EventHandler(this.pbxAudio_Click);
            // 
            // frmNivel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::pTesteBatalhaNaval.Properties.Resources.shipdestoyed;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(684, 391);
            this.Controls.Add(this.pbxAudio);
            this.Controls.Add(this.pbVS);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.lbTexto);
            this.Controls.Add(this.pbxNacao2);
            this.Controls.Add(this.pbxNacao1);
            this.Controls.Add(this.lbTitulo);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmNivel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nivel";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmNivel_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pbxNacao1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxNacao2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxAudio)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbTitulo;
        private System.Windows.Forms.PictureBox pbxNacao1;
        private System.Windows.Forms.PictureBox pbxNacao2;
        private System.Windows.Forms.Label lbTexto;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.PictureBox pbVS;
        private System.Windows.Forms.PictureBox pbxAudio;
    }
}