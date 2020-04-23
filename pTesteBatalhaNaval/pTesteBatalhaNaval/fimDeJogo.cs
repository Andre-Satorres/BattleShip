using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Media;

namespace pTesteBatalhaNaval
{
    public partial class fimDeJogo : Form
    {
        private frmNaval nav;
        private Label lbImg;
        private bool venceu;
        private String pais;
        private int qlDeveSer=0;
        private bool temp = true;
        private int soma;
        private int nivel;
        private bool ehOCerto;
        private SoundPlayer player = new SoundPlayer();
        private bool estaTocando = false;
        public fimDeJogo(bool venceu, String pais, int nivel, int s, bool ehOCerto, frmNaval n, bool estaTocando)
        {
            InitializeComponent();
            this.venceu = venceu;
            this.nav = n;
            this.nivel = nivel;
            this.pais = pais;
            this.ehOCerto = ehOCerto;
            this.estaTocando = estaTocando;

            if(!this.estaTocando)
            {
                this.player = new SoundPlayer();
                pbxAudio.BackgroundImage = Image.FromFile("../imagens/loudspeakerMute.png");
            }
            this.soma = s;

            this.lbSit.Text = "VITÓRIA !!!";
            this.Text = "Vitória";
            this.lbSit.BackColor = Color.Red;
            this.btnProx.Text = "Prosseguir";

            if (nivel == 1)
            {
                if (pais.ToLower().Equals("urss"))
                {
                    this.BackgroundImage = Image.FromFile("../../../../imagens/urss/1.jpg");
                    this.pais = "urss";
                    this.qlDeveSer = 1;
                    this.pbxNacao1.BackgroundImage = Image.FromFile("../../../../imagens/urss/nivel.png");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/nivel.png");
                }

                else
                if (pais.ToLower().Equals("alemanha"))
                {
                    this.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/1.jpg");
                    this.pais = "nazi";
                    this.qlDeveSer = 2;
                    this.pbxNacao1.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/nivel.png");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/inglaterra/nivel.png");
                }

                else
                if (pais.ToLower().Equals("inglaterra"))
                {
                    this.BackgroundImage = Image.FromFile("../../../../imagens/inglaterra/1.jpg");
                    this.pais = "ingl";
                    this.qlDeveSer = 3;
                    this.pbxNacao1.BackgroundImage = Image.FromFile("../../../../imagens/inglaterra/nivel.png");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/nivel.png");
                }

                else
                if (pais.ToLower().Equals("japao"))
                {
                    this.BackgroundImage = Image.FromFile("../../../../imagens/japao/1.jpg");
                    this.pais = "japao";
                    this.qlDeveSer = 4;
                    this.pbxNacao1.BackgroundImage = Image.FromFile("../../../../imagens/japao/nivel.png");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/eua/nivel.png");
                }
                else
                if (pais.ToLower().Equals("eua"))
                {
                    this.BackgroundImage = Image.FromFile("../../../../imagens/eua/1.gif");
                    this.pais = "eua";
                    this.qlDeveSer = 5;
                    this.pbxNacao1.BackgroundImage = Image.FromFile("../../../../imagens/eua/nivel.png");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/japao/nivel.png");
                }
            }
            else
            {
                if (pais.ToLower().Equals("urss"))
                {
                    this.BackgroundImage = Image.FromFile("../../../../imagens/urss/2.jpg");
                    this.pais = "urss";
                    this.qlDeveSer = 1;
                    this.pbxNacao1.BackgroundImage = Image.FromFile("../../../../imagens/urss/nivel.png");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/nivel.png");
                }
                else
                if (pais.ToLower().Equals("alemanha"))
                {
                    this.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/2.jpg");
                    this.pais = "nazi";
                    this.qlDeveSer = 2;
                    this.pbxNacao1.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/nivel.png");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/urss/nivel.png");
                }
                else
                if (pais.ToLower().Equals("inglaterra"))
                {
                    this.BackgroundImage = Image.FromFile("../../../../imagens/inglaterra/2.jpg");
                    this.pais = "ingl";
                    this.qlDeveSer = 3;
                    this.pbxNacao1.BackgroundImage = Image.FromFile("../../../../imagens/inglaterra/nivel.png");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/nivel.png");
                }
                else
                if (pais.ToLower().Equals("japao"))
                {
                    this.BackgroundImage = Image.FromFile("../../../../imagens/japao/2.jpg");
                    this.pais = "japao";
                    this.qlDeveSer = 4;
                    this.pbxNacao1.BackgroundImage = Image.FromFile("../../../../imagens/japao/nivel.png");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/eua/nivel.png");
                }
                else
                if (pais.ToLower().Equals("eua"))
                {
                    this.BackgroundImage = Image.FromFile("../../../../imagens/eua/2.jpg");
                    this.pais = "eua";
                    this.qlDeveSer = 5;
                    this.pbxNacao1.BackgroundImage = Image.FromFile("../../../../imagens/eua/nivel.png");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/nivel.png");
                }
            }

            player.SoundLocation = "../audios/victory.wav";

            if (!venceu)
            {
                this.lbSit.Text = "DERROTADO !!";
                this.Text = "Derrota";
                this.lbSit.BackColor = Color.Red;
                this.player.SoundLocation = "../audios/defeated.wav";

                if (nivel == 1)
                    this.BackgroundImage = Image.FromFile("../../../../imagens/defeated1.png");
                else
                if (nivel == 2)
                    this.BackgroundImage = Image.FromFile("../../../../imagens/defeated2.jpg");
            
            }

        }

        private void btnProx_Click(object sender, EventArgs e)
        {          
            if(venceu)
            {
                frmJogo jooj = null;
                if (ehOCerto)
                {

                    switch (soma)
                    {
                        case 1: jooj = new frmJogo(1, 1, 0, 0, 0, nav, estaTocando); break;
                        case 2: jooj = new frmJogo(1, 1, 1, 0, 0, nav, estaTocando); break;
                        case 3: jooj = new frmJogo(1, 1, 1, 1, 0, nav, estaTocando); break;
                        case 4: jooj = new frmJogo(1, 1, 1, 1, 1, nav, estaTocando); break;
                        case 5: jooj = new frmJogo(2, 1, 1, 1, 1, nav, estaTocando); break;
                        case 6: jooj = new frmJogo(2, 2, 1, 1, 1, nav, estaTocando); break;
                        case 7: jooj = new frmJogo(2, 2, 2, 1, 1, nav, estaTocando); break;
                        case 8: jooj = new frmJogo(2, 2, 2, 2, 1, nav, estaTocando); break;
                        case 9: jooj = new frmJogo(2, 2, 2, 2, 2, nav, estaTocando); break;
                        case 10: jooj = new frmJogo(2, 2, 2, 2, 2, nav, estaTocando, true); break;
                    }
                }
                else
                {
                    switch (soma)
                    {
                        case 1: jooj = new frmJogo(1, 0, 0, 0, 0, nav, estaTocando); break;
                        case 2: jooj = new frmJogo(1, 1, 0, 0, 0, nav, estaTocando); break;
                        case 3: jooj = new frmJogo(1, 1, 1, 0, 0, nav, estaTocando); break;
                        case 4: jooj = new frmJogo(1, 1, 1, 1, 0, nav, estaTocando); break;
                        case 5: jooj = new frmJogo(1, 1, 1, 1, 1, nav, estaTocando); break;
                        case 6: jooj = new frmJogo(2, 1, 1, 1, 1, nav, estaTocando); break;
                        case 7: jooj = new frmJogo(2, 2, 1, 1, 1, nav, estaTocando); break;
                        case 8: jooj = new frmJogo(2, 2, 2, 1, 1, nav, estaTocando); break;
                        case 9: jooj = new frmJogo(2, 2, 2, 2, 1, nav, estaTocando); break;
                        case 10: jooj = new frmJogo(2, 2, 2, 2, 2, nav, estaTocando); break;
                    }
                }

                jooj.Show();
                jooj.ClickBot();
                this.temp = false;
                Close();
            }
            else
            {
                frmNivel nivel = new frmNivel(this.nav, this.pais, this.nivel, this.soma, ehOCerto, this.estaTocando);
                nivel.Show();
                this.temp = false;
                Close();
            }
        }

        private void fimDeJogo_Shown(object sender, EventArgs e)
        {
            this.pbxNacao1.Visible = true;
            this.pbxNacao2.Visible = true;

            this.lbImg = new Label();
            lbImg.AutoSize = false;

            if (venceu)
            {
                lbImg.Location = pbxNacao2.Location;
                lbImg.Size = pbxNacao2.Size;
            }
            else
            {
                lbImg.Location = pbxNacao1.Location;
                lbImg.Size = pbxNacao1.Size;
            }           
            lbImg.Visible = true;
            lbImg.BackColor = Color.Transparent;
            lbImg.Image = Image.FromFile("../../../../imagens/explosion gif.gif");
            lbImg.Show();
            this.Controls.Add(lbImg);
            lbImg.BringToFront();
            pbxNacao1.SendToBack();
            pbxNacao2.SendToBack();
            this.player.SoundLocation = "../audios/explo.wav";
            player.Play();
            this.Refresh();            

            if (venceu)
                player.SoundLocation = "../audios/victory.wav";
            else
                player.SoundLocation = "../audios/defeated.wav";
            
            tmrAndar.Enabled = true;
            tmrAndar.Start();           
        }

        private void tmrAndar_Tick(object sender, EventArgs e)
        {
            tmrAndar.Enabled = false;
            tmrAndar.Stop();
            this.lbImg.Image = null;
            this.Controls.Remove(lbImg);
            this.Refresh();
            player.Play();
            for (int i = 0; i < 184; i++)
            {
                if (venceu)
                {
                    pbxNacao1.Location = new Point(pbxNacao1.Location.X + 1, pbxNacao1.Location.Y);
                    pbxNacao2.Location = new Point(pbxNacao2.Location.X + 1, pbxNacao2.Location.Y);
                }
                else
                {
                    pbxNacao1.Location = new Point(pbxNacao1.Location.X - 1, pbxNacao1.Location.Y);
                    pbxNacao2.Location = new Point(pbxNacao2.Location.X - 1, pbxNacao2.Location.Y);
                }

                pbxNacao1.Refresh();
                pbxNacao2.Refresh();
                this.Refresh();
                Thread.Sleep(2);
            }
        
            if (venceu)
            {
                this.pbxNacao2.Visible = false;
                this.btnProx.Visible = true;
            }
            else
            {
                this.pbxNacao1.Visible = false;
                this.btnTryAgain.Visible = true;
                this.btnGoBack.Visible = true;
            }
        }

        private void fimDeJogo_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.temp)
                this.nav.Close();
        }

        private void btnGoBack_Click(object sender, EventArgs e)
        {
            frmJogo jooj = null;
            switch (soma)
            {
                case 1: jooj = new frmJogo(1, 0, 0, 0, 0, nav, estaTocando); break;
                case 2: jooj = new frmJogo(1, 1, 0, 0, 0, nav, estaTocando); break;
                case 3: jooj = new frmJogo(1, 1, 1, 0, 0, nav, estaTocando); break;
                case 4: jooj = new frmJogo(1, 1, 1, 1, 0, nav, estaTocando); break;
                case 5: jooj = new frmJogo(1, 1, 1, 1, 1, nav, estaTocando); break;
                case 6: jooj = new frmJogo(2, 1, 1, 1, 1, nav, estaTocando); break;
                case 7: jooj = new frmJogo(2, 2, 1, 1, 1, nav, estaTocando); break;
                case 8: jooj = new frmJogo(2, 2, 2, 1, 1, nav, estaTocando); break;
                case 9: jooj = new frmJogo(2, 2, 2, 2, 1, nav, estaTocando); break;
                case 10: jooj = new frmJogo(2, 2, 2, 2, 2, nav, estaTocando); break;
            }

            jooj.Show();
            jooj.ClickBot();
            this.temp = false;
            Close();
        }

        private void btnTryAgain_Click(object sender, EventArgs e)
        {
            frmNivel nivel = new frmNivel(this.nav, this.pais, this.nivel, soma, ehOCerto, estaTocando);
            nivel.Show();
            this.temp = false;
            Close();
        }

        private void pbxAudio_Click(object sender, EventArgs e)
        {
            if (estaTocando)
            {
                pbxAudio.BackgroundImage = Image.FromFile("../imagens/loudspeakerMute.png");
                player.Stop();
                estaTocando = false;
                player.Dispose();
            }
            else
            {
                player = new SoundPlayer("../audios/main.wav");
                estaTocando = true;
                pbxAudio.BackgroundImage = Image.FromFile("../imagens/loudspeaker.png");
                player.PlayLooping();
            }
        }
    }
}
