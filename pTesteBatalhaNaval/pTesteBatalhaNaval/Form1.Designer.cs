namespace pTesteBatalhaNaval
{
    partial class frmNaval
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNaval));
            this.lblNaval = new System.Windows.Forms.Label();
            this.pbxCanhao = new System.Windows.Forms.PictureBox();
            this.btnNovo = new System.Windows.Forms.Button();
            this.btnSair = new System.Windows.Forms.Button();
            this.pbxAudio = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCanhao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxAudio)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNaval
            // 
            this.lblNaval.AutoSize = true;
            this.lblNaval.BackColor = System.Drawing.Color.Transparent;
            this.lblNaval.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblNaval.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblNaval.Font = new System.Drawing.Font("Comic Sans MS", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNaval.ForeColor = System.Drawing.Color.Black;
            this.lblNaval.Location = new System.Drawing.Point(128, 9);
            this.lblNaval.Name = "lblNaval";
            this.lblNaval.Size = new System.Drawing.Size(472, 90);
            this.lblNaval.TabIndex = 0;
            this.lblNaval.Text = "Batalha Naval";
            this.lblNaval.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbxCanhao
            // 
            this.pbxCanhao.BackColor = System.Drawing.Color.Transparent;
            this.pbxCanhao.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbxCanhao.BackgroundImage")));
            this.pbxCanhao.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxCanhao.Location = new System.Drawing.Point(218, 136);
            this.pbxCanhao.Name = "pbxCanhao";
            this.pbxCanhao.Size = new System.Drawing.Size(68, 38);
            this.pbxCanhao.TabIndex = 3;
            this.pbxCanhao.TabStop = false;
            // 
            // btnNovo
            // 
            this.btnNovo.BackColor = System.Drawing.Color.Transparent;
            this.btnNovo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNovo.BackgroundImage")));
            this.btnNovo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNovo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNovo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnNovo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnNovo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNovo.Font = new System.Drawing.Font("Comic Sans MS", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNovo.ForeColor = System.Drawing.Color.Black;
            this.btnNovo.Location = new System.Drawing.Point(302, 124);
            this.btnNovo.Name = "btnNovo";
            this.btnNovo.Size = new System.Drawing.Size(150, 62);
            this.btnNovo.TabIndex = 7;
            this.btnNovo.Text = "Novo Jogo";
            this.btnNovo.UseVisualStyleBackColor = false;
            this.btnNovo.Click += new System.EventHandler(this.btnNovo_Click);
            // 
            // btnSair
            // 
            this.btnSair.BackColor = System.Drawing.Color.Transparent;
            this.btnSair.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSair.BackgroundImage")));
            this.btnSair.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSair.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSair.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnSair.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSair.Font = new System.Drawing.Font("Comic Sans MS", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSair.ForeColor = System.Drawing.Color.Black;
            this.btnSair.Location = new System.Drawing.Point(302, 210);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(150, 62);
            this.btnSair.TabIndex = 8;
            this.btnSair.Text = "Sair";
            this.btnSair.UseVisualStyleBackColor = false;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // pbxAudio
            // 
            this.pbxAudio.BackColor = System.Drawing.Color.Transparent;
            this.pbxAudio.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbxAudio.BackgroundImage")));
            this.pbxAudio.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxAudio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbxAudio.Location = new System.Drawing.Point(682, 9);
            this.pbxAudio.Name = "pbxAudio";
            this.pbxAudio.Size = new System.Drawing.Size(30, 30);
            this.pbxAudio.TabIndex = 9;
            this.pbxAudio.TabStop = false;
            this.pbxAudio.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // frmNaval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(724, 362);
            this.Controls.Add(this.pbxAudio);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.btnNovo);
            this.Controls.Add(this.pbxCanhao);
            this.Controls.Add(this.lblNaval);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmNaval";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Batalha Naval";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmNaval_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pbxCanhao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxAudio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNaval;
        private System.Windows.Forms.PictureBox pbxCanhao;
        private System.Windows.Forms.Button btnNovo;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.PictureBox pbxAudio;
    }
}

