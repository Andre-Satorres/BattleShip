namespace pTesteBatalhaNaval
{
    partial class frmInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInfo));
            this.lbTitulo = new System.Windows.Forms.Label();
            this.pbxLogo = new System.Windows.Forms.PictureBox();
            this.btnProx = new System.Windows.Forms.Button();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.lbExplicacao = new System.Windows.Forms.Label();
            this.pbxAudio = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxAudio)).BeginInit();
            this.SuspendLayout();
            // 
            // lbTitulo
            // 
            this.lbTitulo.AutoSize = true;
            this.lbTitulo.BackColor = System.Drawing.Color.Transparent;
            this.lbTitulo.Font = new System.Drawing.Font("Comic Sans MS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitulo.ForeColor = System.Drawing.Color.Black;
            this.lbTitulo.Location = new System.Drawing.Point(306, 7);
            this.lbTitulo.Name = "lbTitulo";
            this.lbTitulo.Size = new System.Drawing.Size(87, 45);
            this.lbTitulo.TabIndex = 0;
            this.lbTitulo.Text = "Info";
            this.lbTitulo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pbxLogo
            // 
            this.pbxLogo.BackColor = System.Drawing.Color.Transparent;
            this.pbxLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbxLogo.Image")));
            this.pbxLogo.Location = new System.Drawing.Point(12, 7);
            this.pbxLogo.Name = "pbxLogo";
            this.pbxLogo.Size = new System.Drawing.Size(84, 91);
            this.pbxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxLogo.TabIndex = 3;
            this.pbxLogo.TabStop = false;
            // 
            // btnProx
            // 
            this.btnProx.BackgroundImage = global::pTesteBatalhaNaval.Properties.Resources._try;
            this.btnProx.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProx.Location = new System.Drawing.Point(465, 313);
            this.btnProx.Name = "btnProx";
            this.btnProx.Size = new System.Drawing.Size(150, 35);
            this.btnProx.TabIndex = 5;
            this.btnProx.Text = "Próximo";
            this.btnProx.UseVisualStyleBackColor = true;
            this.btnProx.Click += new System.EventHandler(this.btnProx_Click);
            // 
            // btnVoltar
            // 
            this.btnVoltar.BackgroundImage = global::pTesteBatalhaNaval.Properties.Resources._try;
            this.btnVoltar.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoltar.Location = new System.Drawing.Point(112, 313);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(150, 35);
            this.btnVoltar.TabIndex = 6;
            this.btnVoltar.Text = "Voltar";
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // lbExplicacao
            // 
            this.lbExplicacao.AutoSize = true;
            this.lbExplicacao.BackColor = System.Drawing.Color.LightCyan;
            this.lbExplicacao.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbExplicacao.ForeColor = System.Drawing.Color.Black;
            this.lbExplicacao.Location = new System.Drawing.Point(112, 68);
            this.lbExplicacao.Name = "lbExplicacao";
            this.lbExplicacao.Size = new System.Drawing.Size(20, 23);
            this.lbExplicacao.TabIndex = 7;
            this.lbExplicacao.Text = "-";
            // 
            // pbxAudio
            // 
            this.pbxAudio.BackColor = System.Drawing.Color.Transparent;
            this.pbxAudio.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbxAudio.BackgroundImage")));
            this.pbxAudio.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxAudio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbxAudio.Location = new System.Drawing.Point(642, 12);
            this.pbxAudio.Name = "pbxAudio";
            this.pbxAudio.Size = new System.Drawing.Size(30, 30);
            this.pbxAudio.TabIndex = 24;
            this.pbxAudio.TabStop = false;
            this.pbxAudio.Click += new System.EventHandler(this.pbxAudio_Click);
            // 
            // frmInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(684, 361);
            this.Controls.Add(this.pbxAudio);
            this.Controls.Add(this.lbExplicacao);
            this.Controls.Add(this.btnVoltar);
            this.Controls.Add(this.btnProx);
            this.Controls.Add(this.pbxLogo);
            this.Controls.Add(this.lbTitulo);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Info";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmInfo_FormClosed);
            this.Shown += new System.EventHandler(this.frmInfo_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbxLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxAudio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbTitulo;
        private System.Windows.Forms.PictureBox pbxLogo;
        private System.Windows.Forms.Button btnProx;
        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.Label lbExplicacao;
        private System.Windows.Forms.PictureBox pbxAudio;
    }
}