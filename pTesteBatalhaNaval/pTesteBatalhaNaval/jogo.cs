using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace pTesteBatalhaNaval
{
    public partial class frmJogo : Form
    {
        private int nivelUrss;
        private int nivelNazi;
        private int nivelIngl;
        private int nivelJapao;
        private int nivelEua;
        private frmNaval naval;
        private bool podeDiminuirAumentar = true;
        private SoundPlayer hinoDoPais = new SoundPlayer();
        private SoundPlayer player;
        private bool estaTocando;
        private bool temp = true;
        private int nivelAtualDaTela = 1;
        private string locAntiga = "";
        private bool podeTrocarOSom = true;

        public frmJogo(frmNaval naval, int soma)
        {
            InitializeComponent();

            this.SetaNiveis(soma);
            this.verificar();
            this.naval = naval;

            if (SomaDosNiveis() > 5)
            {
                nivelAtualDaTela = 2;

                bool eua         = false;
                bool japao       = false;
                bool ingl        = false;
                bool nazi        = false;
                bool urss        = false;
                pbxRight.Visible = false;
                pbxLeft.Visible  = true;
                if (nivelEua == 2)
                    eua = true;

                if (nivelJapao == 2)
                    japao = true;

                if (nivelIngl == 2)
                    ingl = true;

                if (nivelNazi == 2)
                    nazi = true;

                if (nivelUrss == 2)
                    urss = true;

                lbNivel.Text = "Nível 2";
                this.verificarNaSeta(eua, japao, ingl, nazi, urss);
            }
        }
        private void SetaNiveis(int soma)
        {
            switch (soma)
            {
                case 1:
                    {
                        this.nivelEua = 0;
                        this.nivelIngl = 0;
                        this.nivelJapao = 0;
                        this.nivelNazi = 0;
                        this.nivelUrss = 1;
                    }; break;
                case 2:
                    {
                        this.nivelEua = 0;
                        this.nivelIngl = 0;
                        this.nivelJapao = 0;
                        this.nivelNazi = 1;
                        this.nivelUrss = 1;
                    }; break;
                case 3:
                    {
                        this.nivelEua = 0;
                        this.nivelIngl = 1;
                        this.nivelJapao = 0;
                        this.nivelNazi = 1;
                        this.nivelUrss = 1;
                    }; break;
                case 4: 
                    {
                    this.nivelEua = 0;
                    this.nivelIngl = 1;
                    this.nivelJapao = 1;
                    this.nivelNazi = 1;
                    this.nivelUrss = 1;
                    }; break;
                case 5:
                    {
                        this.nivelEua = 1;
                        this.nivelIngl = 1;
                        this.nivelJapao = 1;
                        this.nivelNazi = 1;
                        this.nivelUrss = 1;
                    }; break;
                case 6:
                    {
                        this.nivelEua = 1;
                        this.nivelIngl = 1;
                        this.nivelJapao = 1;
                        this.nivelNazi = 1;
                        this.nivelUrss = 2;
                    }; break;
                case 7:
                    {
                        this.nivelEua = 1;
                        this.nivelIngl = 1;
                        this.nivelJapao = 1;
                        this.nivelNazi = 2;
                        this.nivelUrss = 2;
                    }; break;
                case 8:
                    {
                        this.nivelEua = 1;
                        this.nivelIngl = 2;
                        this.nivelJapao = 1;
                        this.nivelNazi = 2;
                        this.nivelUrss = 2;
                    }; break;
                case 9:
                    {
                        this.nivelEua = 1;
                        this.nivelIngl = 2;
                        this.nivelJapao = 2;
                        this.nivelNazi = 2;
                        this.nivelUrss = 2;
                    }; break;
                case 10:
                    {
                        this.nivelEua = 2;
                        this.nivelIngl = 2;
                        this.nivelJapao = 2;
                        this.nivelNazi = 2;
                        this.nivelUrss = 2;
                    }; break;
            }
        }
        public frmJogo(frmNaval naval, SoundPlayer p, bool estaTocando, int soma)
        {
            InitializeComponent();

            this.estaTocando = estaTocando;

            if (!estaTocando)
                pbxAudio.BackgroundImage = Image.FromFile("../imagens/loudspeakerMute.png");

            this.SetaNiveis(soma);
            this.player = p;
            this.verificar();
            this.naval = naval;

            if (SomaDosNiveis() > 5)
            {
                nivelAtualDaTela = 2;

                pbxRight.Visible = false;
                pbxLeft.Visible = true;

                bool eua = false;
                bool japao = false;
                bool ingl = false;
                bool nazi = false;
                bool urss = false;

                if (nivelEua == 2)
                    eua = true;

                if (nivelJapao == 2)
                    japao = true;

                if (nivelIngl == 2)
                    ingl = true;

                if (nivelNazi == 2)
                    nazi = true;

                if (nivelUrss == 2)
                    urss = true;

                lbNivel.Text = "Nível 2";
                this.verificarNaSeta(eua, japao, ingl, nazi, urss);
            }

            pbxRight.Visible = false;
        }
        private void verificar()
        {
            if (nivelEua < nivelAtualDaTela || nivelEua == 0)
            {
                this.pbxEUA.Enabled = false;
                this.pbxEUA.Image = Image.FromFile("../../../../imagens/eua/amercian flagc.png");
            }
            else
            {
                this.pbxEUA.Enabled = true;
                this.pbxEUA.Image = Image.FromFile("../../../../imagens/eua/amercian flag.png");
            }


            if (nivelJapao < nivelAtualDaTela || nivelJapao == 0)
            {
                this.pbxJapao.Enabled = false;
                this.pbxJapao.Image = Image.FromFile("../../../../imagens/japao/japan_flagc.png");
            }
            else
            {
                this.pbxJapao.Enabled = true;
                this.pbxJapao.Image = Image.FromFile("../../../../imagens/japao/japan_flag.png");
            }

            if (nivelIngl < nivelAtualDaTela || nivelIngl == 0)
            {
                this.pbxIngl.Enabled = false;
                this.pbxIngl.Image = Image.FromFile("../../../../imagens/inglaterra/inglaflagfc.png");
            }
            else
            {
                this.pbxIngl.Enabled = true;
                this.pbxIngl.Image = Image.FromFile("../../../../imagens/inglaterra/inglaflagf.png");

            }

            if (nivelNazi < nivelAtualDaTela || nivelNazi == 0)
            {
                this.pbxNazi.Enabled = false;
                this.pbxNazi.Image = Image.FromFile("../../../../imagens/alemanha/naziflagc.png");
            }
            else
            {
                this.pbxNazi.Enabled = true;
                this.pbxNazi.Image = Image.FromFile("../../../../imagens/alemanha/naziflag.png");

            }

            if (nivelUrss < nivelAtualDaTela)
            {
                this.pbxURSS.Enabled = false;
                this.pbxURSS.Image = Image.FromFile("../../../../imagens/urss/bandeiraurssc.png");
            }
            else
            {
                this.pbxURSS.Enabled = true;
                this.pbxURSS.Image = Image.FromFile("../../../../imagens/urss/bandeiraurss.png");

            }

            if (SomaDosNiveis()>5)
            {
                pbxLeft.Visible = true;
                pbxRight.Visible = false;
            }
            else
            {
                pbxLeft.Visible = false;
                pbxRight.Visible = true;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up)
            {
                return true;
            }
            else
            if (keyData == Keys.Down)
            {
                return true;
            }

            if (keyData == Keys.Enter)
            {
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void frmJogo_Load(object sender, EventArgs e)
        {          
            this.VerImagens(false);
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            if (pbxCanhao.Top == 342)
            {
                this.naval.Show();
                this.temp = false;
                Close();
            }
            else
            if (pbxCanhao.Top == 187)
                btnLan.Focus();
            else
            if (pbxCanhao.Top == 267)
                btnCreditos.Focus();
            else
            if (pbxCanhao.Top == 104)
                btnBot.Focus();
        }
        private void LimparAlgo(Control algo)
        {
            this.Controls.Remove(algo);
        }
        public void ClickBot() 
        {
            this.btnBot.PerformClick();
        }
        public frmJogo(int n1, int n2, int n3, int n4, int n5, frmNaval n, bool estToc)
        {
            this.naval = n;
            this.nivelUrss =n1;
            this.nivelNazi =n2;
            this.nivelIngl = n3;
            this.nivelJapao = n4;
            this.nivelEua =n5;    
            
            InitializeComponent();

            verificar();

            player = new SoundPlayer("../audios/main.wav");
            estaTocando = estToc;
            if (!estaTocando)
                pbxAudio.BackgroundImage = Image.FromFile("../imagens/loudspeakerMute.png");
            else
                player.PlayLooping();

            if (SomaDosNiveis()>5)
            {
                nivelAtualDaTela = 2;

                pbxRight.Visible = false;
                pbxLeft.Visible = true;

                bool eua = false;
                bool japao = false;
                bool ingl = false;
                bool nazi = false;
                bool urss = false;

                if (nivelEua == 2)
                    eua = true;

                if (nivelJapao == 2)
                    japao = true;

                if (nivelIngl == 2)
                    ingl = true;

                if (nivelNazi == 2)
                    nazi = true;

                if (nivelUrss == 2)
                    urss = true;

                lbNivel.Text = "Nível 2";
                this.verificarNaSeta(eua, japao, ingl, nazi, urss);    
            }
                
            this.player = new SoundPlayer("../audios/main.wav");
        }

        public frmJogo(int n1, int n2, int n3, int n4, int n5, frmNaval n, bool estToc, bool mostrar)
        {
            this.naval = n;
            this.nivelUrss = n1;
            this.nivelNazi = n2;
            this.nivelIngl = n3;
            this.nivelJapao = n4;
            this.nivelEua = n5;

            InitializeComponent();

            verificar();

            player = new SoundPlayer("../audios/main.wav");
            estaTocando = estToc;
            if (!estaTocando)
                pbxAudio.BackgroundImage = Image.FromFile("../imagens/loudspeakerMute.png");
            else
                player.PlayLooping();

            if (SomaDosNiveis() > 5)
            {
                nivelAtualDaTela = 2;

                pbxRight.Visible = false;
                pbxLeft.Visible = true;

                bool eua = false;
                bool japao = false;
                bool ingl = false;
                bool nazi = false;
                bool urss = false;

                if (nivelEua == 2)
                    eua = true;

                if (nivelJapao == 2)
                    japao = true;

                if (nivelIngl == 2)
                    ingl = true;

                if (nivelNazi == 2)
                    nazi = true;

                if (nivelUrss == 2)
                    urss = true;

                lbNivel.Text = "Nível 2";
                this.verificarNaSeta(eua, japao, ingl, nazi, urss);
            }

            this.player = new SoundPlayer("../audios/main.wav");

            frmCreditos c = new frmCreditos();
            c.Show();
            c.Rodar();
        }
        private void btnBot_Click(object sender, EventArgs e)
        {
            if (pbxCanhao.Top == 104)
            {
                this.nivelAtualDaTela = 1;
                this.lbNivel.Text = "Nível 1";
                this.LimparAlgo(pbxCanhao);
                this.LimparAlgo(btnBack);
                this.LimparAlgo(btnLan);
                this.LimparAlgo(lbEsc);
                this.LimparAlgo(btnCreditos);
                this.LimparAlgo(btnBot);

                this.Controls.Add(btnVoltar);
                this.Controls.Add(btnAceitar);
                this.Controls.Add(lbEscolha);
                this.Controls.Add(lbNivel);

                this.verSegundo(true);
                this.VerImagens(true);
                this.verificar();
                this.pbxRight.Visible = true;
                this.pbxLeft.Visible = false;
                this.BackgroundImage = Properties.Resources.mapamundi;
                this.pbxAudio.BringToFront();
            }
            else
            if (pbxCanhao.Top == 187)
                btnLan.Focus();
            else
            if (pbxCanhao.Top == 267)
                btnCreditos.Focus();
            else
            if (pbxCanhao.Top == 342)
                btnBack.Focus();
        }
        private void verSegundo(bool ver)
        {
            this.btnAceitar.Visible = ver;
            this.btnVoltar.Visible  = ver;
            this.lbEscolha.Visible  = ver;
            this.lbNivel.Visible    = ver;
            this.pbxRight.Visible   = ver;
            this.pbxLeft.Visible    = ver;
        }
        private void VerImagens(Boolean querVer)
        {
            pbxURSS.Visible  = querVer;
            pbxNazi.Visible  = querVer;
            pbxIngl.Visible  = querVer;
            pbxJapao.Visible = querVer;
            pbxEUA.Visible   = querVer;
        }
        private int verNivelPb()
        {
            if (this.pbEscolhido().Equals(pbxURSS))
                return nivelUrss;

            if (this.pbEscolhido().Equals(pbxNazi))
                return nivelNazi;

            if (this.pbEscolhido().Equals(pbxIngl))
                return nivelIngl;

            if (this.pbEscolhido().Equals(pbxJapao))
                return nivelJapao;

            if (this.pbEscolhido().Equals(pbxEUA))
                return nivelEua;

            return 0;
        }
        private int SomaDosNiveis()
        {
            return this.nivelEua + nivelIngl + nivelJapao + nivelNazi + nivelUrss;          
        }
        private void Iniciar()
        {
            String text = pbEscolhido().Name.Substring(3);
            int niv = this.verNivelPb();

            if (niv == 2 && nivelAtualDaTela == 1)
                niv = 1;

            int soma = this.SomaDosNiveis();

            bool ehOCerto = false;

            if (SomaAteOPbxEscolhido() == SomaDosNiveis())
                ehOCerto = true;

            temp = false;

            if(niv>1)
            {
                frmNivel fNiv = new frmNivel(this.naval, text, niv, soma, ehOCerto, estaTocando);
                fNiv.Show();
                Close();
            }
            else
            {
                frmInfo i = new frmInfo(this.naval, text, niv, this.player, estaTocando, soma, ehOCerto);
                i.Show();
                Close();
            }          
        }
        private void Alterar(PictureBox pb, int novoY, int novoTam)
        {
            if (podeDiminuirAumentar)
            {
                lblErro.Visible = false;
                pb.Size = new Size(pb.Size.Width, novoTam);
                pb.Location = new Point(pb.Location.X, novoY);
                pb.Refresh();
                this.Refresh();
            }

        }
        private bool AlgumAlterado()
        {
            if (pbxURSS.Location.Y == 65)
                return true;

            if (pbxNazi.Location.Y == 65)
                return true;

            if (pbxIngl.Location.Y == 65)
                return true;

            if (pbxJapao.Location.Y == 65)
                return true;

            if (pbxEUA.Location.Y == 65)
                return true;

            return false;
        }
        private PictureBox pbEscolhido()
        {
            if (pbxURSS.Location.Y == 65)
                return pbxURSS;

            if (pbxNazi.Location.Y == 65)
                return pbxNazi;

            if (pbxIngl.Location.Y == 65)
                return pbxIngl;

            if (pbxJapao.Location.Y == 65)
                return pbxJapao;

            if (pbxEUA.Location.Y == 65)
                return pbxEUA;

            return null;
        }
        private void pbxURSS_Click(object sender, EventArgs e)
        {
            if (podeDiminuirAumentar)
            {
                this.podeDiminuirAumentar = false;
                this.pbxLeft.Enabled = false;
                this.pbxRight.Enabled = false;
            }
            else
            if (AlgumAlterado() == false || sender.Equals(this.pbEscolhido()))
            {
                podeDiminuirAumentar = true;
                PictureBox pb = (PictureBox)sender;
                this.Alterar(pb, 65, 250);
                this.podeTrocarOSom = false;
            }

        }
        private void pbxNazi_Click(object sender, EventArgs e)
        {
            if (podeDiminuirAumentar)
            {
                this.podeDiminuirAumentar = false;
                this.pbxLeft.Enabled = false;
                this.pbxRight.Enabled = false;
            }
            else
            if (AlgumAlterado() == false || sender.Equals(this.pbEscolhido()))
            {
                podeDiminuirAumentar = true;
                PictureBox pb = (PictureBox)sender;
                this.Alterar(pb, 65, 250);
                this.podeTrocarOSom = false;
            }
        }
        private void pbxIngl_Click(object sender, EventArgs e)
        {
            if (podeDiminuirAumentar)
            {
                this.podeDiminuirAumentar = false;
                this.pbxLeft.Enabled = false;
                this.pbxRight.Enabled = false;
            }
            else
            if (AlgumAlterado() == false || sender.Equals(this.pbEscolhido()))
            {
                podeDiminuirAumentar = true;
                PictureBox pb = (PictureBox)sender;
                this.Alterar(pb, 65, 250);
                this.podeTrocarOSom = false;
            }
        }
        private void pbxJapao_Click(object sender, EventArgs e)
        {
            if (podeDiminuirAumentar)
            {
                this.podeDiminuirAumentar = false;
                this.pbxLeft.Enabled = false;
                this.pbxRight.Enabled = false;
            }
            else
            if (AlgumAlterado() == false || sender.Equals(this.pbEscolhido()))
            {
                podeDiminuirAumentar = true;
                PictureBox pb = (PictureBox)sender;
                this.Alterar(pb, 65, 250);
                this.podeTrocarOSom = false;
            }
        }
        private void pbxEUA_Click(object sender, EventArgs e)
        {
            if (podeDiminuirAumentar)
            {
                this.podeDiminuirAumentar = false;
                this.pbxLeft.Enabled = false;
                this.pbxRight.Enabled = false;
            }
            else
             if (AlgumAlterado() == false || sender.Equals(this.pbEscolhido()))
            {
                podeDiminuirAumentar = true;
                PictureBox pb = (PictureBox)sender;
                this.Alterar(pb, 65, 250);
                this.podeTrocarOSom = false;
            }
        }
        private void btnLan_Click(object sender, EventArgs e)
        {
            if (pbxCanhao.Top == 187)
            {
                frmLista lista = new frmLista("Usuario             ", this.naval);
                lista.Show();
                this.temp = false;
                Close();
            }
            else
            if (pbxCanhao.Top == 104)
                btnBot.Focus();
            else
            if (pbxCanhao.Top == 267)
                btnCreditos.Focus();
            else
            if (pbxCanhao.Top == 342)
                btnBack.Focus();
        }
        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.verSegundo(false);
            this.LimparAlgo(btnAceitar);
            this.LimparAlgo(btnVoltar);
            this.LimparAlgo(lbEscolha);
            this.LimparAlgo(lbNivel);

            this.VerImagens(false);

            this.BackgroundImage = Image.FromFile("../../../../imagens/zegostou.jpg");
            this.Controls.Add(btnBack);
            this.Controls.Add(btnLan);
            this.Controls.Add(btnBot);
            this.Controls.Add(pbxCanhao);
            this.Controls.Add(lbEsc);
            this.Controls.Add(btnCreditos);

            if (pbEscolhido() != null)
            {
                podeDiminuirAumentar = true;
                this.Alterar(pbEscolhido(), 90, 250);
            }
            if (player != null)
            {
                this.player.Stop();
                this.player.SoundLocation = "../audios/main.wav";
                player.PlayLooping();
            }

            this.Focus();
        }
        private void btnAceitar_Click(object sender, EventArgs e)
        {
            if (this.pbEscolhido() == null)
                lblErro.Visible=true;
            else
                this.Iniciar();
        }
        private void pbxURSS_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;

            this.Alterar(pb, 65, 250);

            if (estaTocando && podeTrocarOSom)
            {
                if (sender.Equals(pbxURSS))
                    this.hinoDoPais.SoundLocation = "../audios/hinos/sovietico.wav";
                else
                if (sender.Equals(pbxNazi))
                    this.hinoDoPais.SoundLocation = "../audios/hinos/nazista.wav";
                else
                if (sender.Equals(pbxIngl))
                    this.hinoDoPais.SoundLocation = "../audios/hinos/inglaterra.wav";
                else
                if (sender.Equals(pbxJapao))
                    this.hinoDoPais.SoundLocation = "../audios/hinos/japao.wav";
                else
                if (sender.Equals(pbxEUA))
                    this.hinoDoPais.SoundLocation = "../audios/hinos/eua.wav";

                if((locAntiga=="" || locAntiga!=this.hinoDoPais.SoundLocation))
                    hinoDoPais.Play();

                this.locAntiga = this.hinoDoPais.SoundLocation;
            }
        }
        private void pbxURSS_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;

            this.Alterar(pb, 90, 250);

            this.player = new SoundPlayer();
            if (AlgumAlterado())
                podeTrocarOSom = false;
            else
            {
                podeTrocarOSom = true;
                pbxRight.Enabled = true;
                pbxLeft.Enabled = true;
            }
            if (estaTocando && !sender.Equals(this.pbEscolhido()) && podeTrocarOSom)
            {
                this.player.SoundLocation = "../audios/main.wav";
                locAntiga = "";
                
                player.PlayLooping();
            }
  
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
        private void pbxLeft_Click(object sender, EventArgs e)
        {
            if(nivelAtualDaTela == 2)
            {
                nivelAtualDaTela = 1;

                pbxLeft.Visible = false;
                pbxRight.Visible = true;
                PictureBox pb = pbEscolhido();

                if (pb != null)
                {
                    pb.Location = new Point(pb.Location.X, 90);
                    podeDiminuirAumentar = true;
                }
                bool eua   = false;
                bool japao = false;
                bool ingl  = false;
                bool nazi  = false;
                bool urss  = false;

                if (nivelEua >= 1)
                    eua = true;

                if (nivelJapao >= 1)
                    japao = true;

                if (nivelIngl >= 1)
                    ingl = true;

                if (nivelNazi >= 1)
                    nazi = true;

                if (nivelUrss >= 1)
                    urss = true;
                
                this.verificarNaSeta(eua, japao, ingl, nazi, urss);

                lbNivel.Text = "Nível 1";
            }
        }
        private void verificarNaSeta(bool umEsta, bool doisEsta, bool tresEsta, bool quatroEsta, bool cincoesta)
        {
            if (!umEsta)
            {
                this.pbxEUA.Enabled = false;
                this.pbxEUA.Image = Image.FromFile("../../../../imagens/eua/amercian flagc.png");
            }
            else
            {
                this.pbxEUA.Enabled = true;
                this.pbxEUA.Image = Image.FromFile("../../../../imagens/eua/amercian flag.png");
            }


            if (!doisEsta)
            {
                this.pbxJapao.Enabled = false;
                this.pbxJapao.Image = Image.FromFile("../../../../imagens/japao/japan_flagc.png");
            }
            else
            {
                this.pbxJapao.Enabled = true;
                this.pbxJapao.Image = Image.FromFile("../../../../imagens/japao/japan_flag.png");
            }

            if (!tresEsta)
            {
                this.pbxIngl.Enabled = false;
                this.pbxIngl.Image = Image.FromFile("../../../../imagens/inglaterra/inglaflagfc.png");
            }
            else
            {
                this.pbxIngl.Enabled = true;
                this.pbxIngl.Image = Image.FromFile("../../../../imagens/inglaterra/inglaflagf.png");

            }

            if (!quatroEsta)
            {
                this.pbxNazi.Enabled = false;
                this.pbxNazi.Image = Image.FromFile("../../../../imagens/alemanha/naziflagc.png");
            }
            else
            {
                this.pbxNazi.Enabled = true;
                this.pbxNazi.Image = Image.FromFile("../../../../imagens/alemanha/naziflag.png");

            }

            if (!cincoesta)
            {
                this.pbxURSS.Enabled = false;
                this.pbxURSS.Image = Image.FromFile("../../../../imagens/urss/bandeiraurssc.png");
            }
            else
            {
                this.pbxURSS.Enabled = true;
                this.pbxURSS.Image = Image.FromFile("../../../../imagens/urss/bandeiraurss.png");

            }
        }
        private void pbxRight_Click(object sender, EventArgs e)
        {
            if(nivelAtualDaTela==1)
            {
                nivelAtualDaTela = 2;
                pbxRight.Visible = false;
                pbxLeft.Visible = true;
                PictureBox pb = pbEscolhido();
                if (pb != null)
                {
                    pb.Location = new Point(pb.Location.X, 90);
                    podeDiminuirAumentar = true;
                }


                bool eua = false;
                bool japao = false;
                bool ingl = false;
                bool nazi = false;
                bool urss = false;

                if (nivelEua == 2)
                    eua = true;

                if (nivelJapao == 2)
                    japao = true;

                if (nivelIngl == 2)
                    ingl = true;

                if (nivelNazi == 2)
                    nazi = true;

                if (nivelUrss == 2)
                    urss = true;

                lbNivel.Text = "Nível 2";
                this.verificarNaSeta(eua, japao, ingl, nazi, urss);
            }
        }
        private void lbNivel_Click(object sender, EventArgs e)
        {
            this.nivelEua = 1;
            this.nivelJapao = 2;
            this.nivelIngl = 2;
            this.nivelNazi = 2;
            this.nivelUrss = 2;
            this.verificar();

            pbxRight.Visible = true;
            pbxLeft.Visible = false;
        }
        private int SomaAteOPbxEscolhido()
        {
            int soma = 1;

            if (nivelAtualDaTela == 2)
                soma +=5;

            PictureBox pb = pbEscolhido();

            if (pb == pbxURSS)
                return soma;

            soma++;

            if (pb == pbxNazi)
                return soma;

            soma++;

            if (pb == pbxIngl)
                return soma;

            soma++;

            if (pb == pbxJapao)
                return soma;

            soma++;

            return soma;

        }
        private void frmJogo_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.temp)
                this.naval.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pbxCanhao.Top == 267)
            {
                frmCreditos c = new frmCreditos();
                c.Show();
                c.Rodar();
            }
            else
            if (pbxCanhao.Top == 187)
                btnLan.Focus();
            else
            if (pbxCanhao.Top == 104)
                btnBot.Focus();
            else
            if (pbxCanhao.Top == 342)
                btnBack.Focus();
        }

        private void frmJogo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (pbxCanhao.Top == 104)
                {
                    pbxCanhao.Top = 342;

                    btnBack.Focus();
                }
                else
                if (pbxCanhao.Top == 187)
                {
                    pbxCanhao.Top = 104;

                    btnBot.Focus();
                }
                else
                if (pbxCanhao.Top == 267)
                {
                    pbxCanhao.Top = 187;

                    btnLan.Focus();

                }
                else
                if (pbxCanhao.Top == 342)
                {
                    pbxCanhao.Top = 267;

                    btnCreditos.Focus();

                }
            }
            else
            if (e.KeyCode == Keys.Down)
            {
                if (pbxCanhao.Top == 104)
                {
                    pbxCanhao.Top = 187;

                    btnLan.Focus();
                }
                else
                if (pbxCanhao.Top == 187)
                {
                    pbxCanhao.Top = 267;

                    btnCreditos.Focus();
                }
                else
                if (pbxCanhao.Top == 267)
                {
                    pbxCanhao.Top = 342;

                    btnBack.Focus();
                    

                }
                else
                if (pbxCanhao.Top == 342)
                {
                    pbxCanhao.Top = 104;

                    btnBot.Focus();

                }
            }

            if (e.KeyCode == Keys.Enter)
            {
                if (btnBot.Focused)
                    btnBot.PerformClick();
                else
                if (btnLan.Focused)
                    btnLan.PerformClick();
                else
                if (btnBack.Focused)
                    btnBack.PerformClick();
                else
                if (btnCreditos.Focused)
                    btnCreditos.PerformClick();
            }

            pbxCanhao.Refresh();
            this.Refresh();
        }
    }
}