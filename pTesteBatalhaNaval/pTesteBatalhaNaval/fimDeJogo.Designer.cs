namespace pTesteBatalhaNaval
{
    partial class fimDeJogo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fimDeJogo));
            this.lbSit = new System.Windows.Forms.Label();
            this.btnProx = new System.Windows.Forms.Button();
            this.pbxNacao2 = new System.Windows.Forms.PictureBox();
            this.pbxNacao1 = new System.Windows.Forms.PictureBox();
            this.tmrAndar = new System.Windows.Forms.Timer(this.components);
            this.btnTryAgain = new System.Windows.Forms.Button();
            this.btnGoBack = new System.Windows.Forms.Button();
            this.pbxAudio = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbxNacao2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxNacao1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxAudio)).BeginInit();
            this.SuspendLayout();
            // 
            // lbSit
            // 
            this.lbSit.BackColor = System.Drawing.Color.Red;
            this.lbSit.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbSit.Font = new System.Drawing.Font("Comic Sans MS", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSit.Location = new System.Drawing.Point(0, 0);
            this.lbSit.Name = "lbSit";
            this.lbSit.Size = new System.Drawing.Size(684, 63);
            this.lbSit.TabIndex = 0;
            this.lbSit.Text = "VITÓRIA !!!";
            this.lbSit.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnProx
            // 
            this.btnProx.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnProx.BackgroundImage = global::pTesteBatalhaNaval.Properties.Resources._try;
            this.btnProx.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnProx.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProx.Location = new System.Drawing.Point(225, 342);
            this.btnProx.Name = "btnProx";
            this.btnProx.Size = new System.Drawing.Size(250, 45);
            this.btnProx.TabIndex = 0;
            this.btnProx.Text = "Tentar Novamente";
            this.btnProx.UseVisualStyleBackColor = false;
            this.btnProx.Visible = false;
            this.btnProx.Click += new System.EventHandler(this.btnProx_Click);
            // 
            // pbxNacao2
            // 
            this.pbxNacao2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxNacao2.Location = new System.Drawing.Point(409, 66);
            this.pbxNacao2.Name = "pbxNacao2";
            this.pbxNacao2.Size = new System.Drawing.Size(250, 270);
            this.pbxNacao2.TabIndex = 4;
            this.pbxNacao2.TabStop = false;
            this.pbxNacao2.Visible = false;
            // 
            // pbxNacao1
            // 
            this.pbxNacao1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxNacao1.Location = new System.Drawing.Point(41, 66);
            this.pbxNacao1.Name = "pbxNacao1";
            this.pbxNacao1.Size = new System.Drawing.Size(250, 270);
            this.pbxNacao1.TabIndex = 3;
            this.pbxNacao1.TabStop = false;
            this.pbxNacao1.Visible = false;
            // 
            // tmrAndar
            // 
            this.tmrAndar.Interval = 1000;
            this.tmrAndar.Tick += new System.EventHandler(this.tmrAndar_Tick);
            // 
            // btnTryAgain
            // 
            this.btnTryAgain.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnTryAgain.BackgroundImage = global::pTesteBatalhaNaval.Properties.Resources._try;
            this.btnTryAgain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTryAgain.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTryAgain.Location = new System.Drawing.Point(361, 342);
            this.btnTryAgain.Name = "btnTryAgain";
            this.btnTryAgain.Size = new System.Drawing.Size(114, 45);
            this.btnTryAgain.TabIndex = 5;
            this.btnTryAgain.Text = "Revanche";
            this.btnTryAgain.UseVisualStyleBackColor = false;
            this.btnTryAgain.Visible = false;
            this.btnTryAgain.Click += new System.EventHandler(this.btnTryAgain_Click);
            // 
            // btnGoBack
            // 
            this.btnGoBack.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnGoBack.BackgroundImage = global::pTesteBatalhaNaval.Properties.Resources._try;
            this.btnGoBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGoBack.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGoBack.Location = new System.Drawing.Point(225, 342);
            this.btnGoBack.Name = "btnGoBack";
            this.btnGoBack.Size = new System.Drawing.Size(114, 45);
            this.btnGoBack.TabIndex = 6;
            this.btnGoBack.Text = "Voltar";
            this.btnGoBack.UseVisualStyleBackColor = false;
            this.btnGoBack.Visible = false;
            this.btnGoBack.Click += new System.EventHandler(this.btnGoBack_Click);
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
            this.pbxAudio.TabIndex = 24;
            this.pbxAudio.TabStop = false;
            this.pbxAudio.Click += new System.EventHandler(this.pbxAudio_Click);
            // 
            // fimDeJogo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(684, 389);
            this.Controls.Add(this.pbxAudio);
            this.Controls.Add(this.btnGoBack);
            this.Controls.Add(this.btnTryAgain);
            this.Controls.Add(this.pbxNacao2);
            this.Controls.Add(this.pbxNacao1);
            this.Controls.Add(this.btnProx);
            this.Controls.Add(this.lbSit);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "fimDeJogo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vitória";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fimDeJogo_FormClosed);
            this.Shown += new System.EventHandler(this.fimDeJogo_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbxNacao2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxNacao1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxAudio)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbSit;
        private System.Windows.Forms.Button btnProx;
        private System.Windows.Forms.PictureBox pbxNacao2;
        private System.Windows.Forms.PictureBox pbxNacao1;
        private System.Windows.Forms.Timer tmrAndar;
        private System.Windows.Forms.Button btnTryAgain;
        private System.Windows.Forms.Button btnGoBack;
        private System.Windows.Forms.PictureBox pbxAudio;
    }
}