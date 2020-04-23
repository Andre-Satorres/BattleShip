using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace pTesteBatalhaNaval
{
    public partial class frmNivel : Form
    {
        private String pais;
        private String paisAdv;
        private int nivel;
        private frmNaval naval;
        private int soma;
        private bool ehOCerto;
        private bool estaTocando=false;
        private SoundPlayer p;
        private bool temp = true;

        public frmNivel(frmNaval nav, String texto, int nivel, int soma, bool ehOCerto, bool estaToc)
        {
            if (texto == null || nivel < 0)
                throw new Exception("Valores inválidos!!");

            //hide = new frmHide();
            estaTocando = estaToc;

            p = new SoundPlayer();
            
            this.pais = texto;
            this.nivel = nivel;
            this.naval = nav;
            this.soma = soma;
            this.ehOCerto = ehOCerto;
            InitializeComponent();

            if (this.pais.ToLower().Equals("urss"))
            {
                this.pbxNacao1.BackgroundImage = Image.FromFile("../../../../imagens/urss/nivel.png");

                if (this.nivel == 1)
                {
                    lbTitulo.Text = "Contra-Ataque";
                    this.BackgroundImage = Image.FromFile("../../../../imagens/urss/shipdestroyed.jpg");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/nivel.png");
                    this.lbTexto.Text = "Após termos o Tratado de Não-Agressão quebrado por parte da Alemanha, com um ataque alemão" +
                        " forte e um avanço a Stalingrado, importante cidade nacional, estamos nos preparando para um Contra-Ataque." +
                        "Nossa missão é dominar o Mar Báltico, auxiliando nossas tropas terrestres em Stalingrado. Esta será uma batalha muito importante" +
                        " nesta Guerra. Contamos com você, marinheiro.";

                    this.paisAdv = "Marinha Nazista";

                    p.SoundLocation = "../audios/a juntar/Betrayal Desolation.wav";
                }
                else
                if (this.nivel == 2)
                {
                    lbTitulo.Text = "Bem vindo ao Mar Báltico";
                    this.BackgroundImage = Image.FromFile("../../../../imagens/urss/batalhaDoPacifico.jpg");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/nivel.png");
                    this.lbTexto.Text = "Conseguimos!! Impedimos que os nazistas dominassem Stalingrado!! Agora devemos seguir em frente," +
                        " iremos combater as tropas nazistas com força máxima no lado Oriental. Estamos em vantagem agora !! Boa sorte, capitão.";

                    this.paisAdv = "Marinha Nazista";

                    p.SoundLocation = "../audios/a juntar/Betrayal Desolation.wav";
                }
            }
            else
            if (this.pais.ToLower().Equals("nazi"))
            {
                this.pbxNacao1.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/nivel.png");

                if (this.nivel == 1)
                {
                    lbTitulo.Text = "Além-Mar";
                    this.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/anotherone.jpg");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/inglaterra/nivel.png");

                    this.lbTexto.Text = "Finalmente. Nós conquistamos toda a Europa. Agora, precisamos apenas da rendição" +
                        " inglesa. Cedo ou tarde o Reino Unido se curvará. Nosso poder de fogo é muito maior que o deles." +
                        " Estamos preparando uma invasão ao Reino Unido e vamos precisar de sua ajuda na batalha pelo mar." +
                        " Contamos com você, sargento!";

                    this.paisAdv = "Marinha Real Britânica";

                    p.SoundLocation = "../audios/a juntar/Full Attack.wav";
                }
                else
                if (this.nivel == 2)
                {
                    lbTitulo.Text = "A Operação Barbarossa";
                    this.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/tallinn.jpg");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/urss/nivel.png");

                    this.lbTexto.Text = "Está na hora. Temos a Europa completamente dominada, falta apenas conquistarmos" +
                        " o Reino Unido. O Norte da África também é nosso. Sargento, estamos prontos para atacar nosso lado oriental, agora!!" +
                        " Boa batalha, capitão";

                    this.paisAdv = "Marinha Soviética";

                    p.SoundLocation = "../audios/a juntar/Full Attack.wav";
                }
            }
            else
            if (this.pais.ToLower().Equals("ingl"))
            {
                this.pbxNacao1.BackgroundImage = Image.FromFile("../../../../imagens/inglaterra/nivel.png");

                if (this.nivel == 1)
                {
                    lbTitulo.Text = "Resgate";
                    this.BackgroundImage = Image.FromFile("../../../../imagens/inglaterra/mediterranean_battles.jpg");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/nivel.png");

                    this.lbTexto.Text = "Sargento? Que bom que voltou. Precisamos de você para um favor." +
                        " Nossas tropas foram cercadas e feitas de refém em ilhas do Mediterrâneo. Estamos montando" +
                        " uma equipe de resgate para trazê-las de volta para casa. Estaremos te aguardando no ancoradouro.";

                    this.paisAdv = "Marinha Nazista";

                    p.SoundLocation = "../audios/a juntar/its Your Ship Now.wav";
                }
                else
                if (this.nivel == 2)
                {
                    lbTitulo.Text = "'Isto não é o fim.'";
                    this.BackgroundImage = Image.FromFile("../../../../imagens/inglaterra/atlantic_war.jpg");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/nivel.png");

                    this.lbTexto.Text = "Capitão!! Precisamos de você urgente!! Acabamos de receber a notícia de que navios" +
                        " alemães estão vindo em nossa direção, em grande grupo. Estamos desesperados. Se eles conseguirem nos dominar também, " +
                        " terão toda a Europa e deixaremos os Estados Unidos sozinhos. Isto não é o fim, precisamos de você e iremos resistir" +
                        " até o fim!";

                    this.paisAdv = "Marinha Nazista";

                    p.SoundLocation = "../audios/a juntar/its Your Ship Now.wav";
                }
            }
            else
            if (this.pais.ToLower().Equals("japao"))
            {
                this.pbxNacao1.BackgroundImage = Image.FromFile("../../../../imagens/japao/nivel.png");

                if (this.nivel == 1)
                {
                    lbTitulo.Text = "Império do Sol Nascente";
                    this.BackgroundImage = Image.FromFile("../../../../imagens/japao/java_sea.jpg");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/eua/nivel.png");

                    this.lbTexto.Text = "Os planos de expansão do nosso imperador são bem altos." +
                        " Iremos começar nosso trabalho dominando as ilhas próximas, aos poucos iremos conquistar China," +
                        " ilhas da Oceania e em breve, todo o Pacífico!! Seremos um grande império!! Contamos com sua ajuda para isso," +
                        " tenente.";

                    this.paisAdv = "Marinha Americana";

                    p.SoundLocation = "../audios/a juntar/Revenge.wav";
                }
                else
                if (this.nivel == 2)
                {
                    lbTitulo.Text = "Pearl Harbor";
                    this.BackgroundImage = Image.FromFile("../../../../imagens/japao/pearl_harbor.jpg");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/eua/nivel.png");

                    this.lbTexto.Text = "Nossos espiões acabaram de relatar. É agora! Os americanos estão " +
                        " defensivamente expostos. Almirante, só com você no comando desta operação poderemos" +
                        " vencer e continuar pensando em nossa grande expansão.";

                    this.paisAdv = "Marinha Americana";

                    p.SoundLocation = "../audios/a juntar/The Battle Of Stirling.wav";
                }
            }
            else
            if (this.pais.ToLower().Equals("eua"))
            {
                this.pbxNacao1.BackgroundImage = Image.FromFile("../../../../imagens/eua/nivel.png");

                if (this.nivel == 1)
                {
                    lbTitulo.Text = "Midway";
                    this.BackgroundImage = Image.FromFile("../../../../imagens/eua/midway.jpg");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/japao/nivel.png");

                    this.lbTexto.Text = "O Japão enloqueceu. Querem dominar todo o pacífico. Já tem parte" +
                        " da China, a Coréia, as ilhas próximas e estão conquistando cada vez mais. Eles são uma ameaça." +
                        " Precisamos parar com essas conquistas intermináveis. Iniciaremos uma Guerra aqui no Pacífico que provavelmente" +
                        " durará anos. Tenente, você é a pessoa certa para realizarmos um ataque nas Ilhas Salomão.";

                    this.paisAdv = "Marinha Imperial do Japão";

                    p.SoundLocation = "../audios/a juntar/The Beacon Project.wav";
                }
                else
                if (this.nivel == 2)
                {
                    lbTitulo.Text = "A Operação Overlord";
                    this.BackgroundImage = Image.FromFile("../../../../imagens/eua/overlord.jpg");
                    this.pbxNacao2.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/nivel.png");

                    this.lbTexto.Text = "Esta será a Operação mais importante de todas nesta Guerra. Juntamos " +
                        " aliados ingleses, franceses e canadenses para atacarmos a praia de Normandia. Assim, teremos uma frente " +
                        " de batalha no oeste também, dificultando ainda mais para os nazistas e também libertaremos a França." +
                        " Almirante, uma missão tão importante como essa só pode ser comandada pelas suas mãos!! Boa sorte.";

                    this.paisAdv = "Marinha Nazista";

                    p.SoundLocation = "../audios/a juntar/thug Fight.wav";
                }
            }

            this.lbTexto.BackColor = Color.White;
            this.btnIniciar.Location = new Point(btnIniciar.Location.X, lbTexto.Location.Y + lbTexto.Height + 5);

            if(estaTocando)
                p.PlayLooping();
            else
                pbxAudio.BackgroundImage = Image.FromFile("../imagens/loudspeakerMute.png");
        }
        private void btnIniciar_Click(object sender, EventArgs e)
        {
            temp = false;
            Thread t = new Thread(this.Animar);
            t.Start();
        }
        private void Animar()
        {
            this.Invoke((MethodInvoker)delegate
            {         
                this.lbTexto.Dispose();
                this.btnIniciar.Dispose();
                this.pbxNacao1.Visible = true;
                this.pbxNacao2.Visible = true;

                for (int i = 0; i < 110; i++)
                {
                    pbxNacao1.Location = new Point(pbxNacao1.Location.X + 1, pbxNacao1.Location.Y);
                    pbxNacao2.Location = new Point(pbxNacao2.Location.X - 1, pbxNacao2.Location.Y);
                    pbxNacao1.Refresh();
                    pbxNacao2.Refresh();
                    this.Refresh();
                    Thread.Sleep(5);
                }

                pbVS.Visible = true;
                pbVS.BackgroundImage = Image.FromFile("../../../../imagens/vs.png");
                pbVS.SendToBack();
                pbVS.Refresh();
                Thread.Sleep(1000);

                String p = "";

                if (this.pais.ToLower().Equals("urss"))
                    p = "Marinha Soviética";
                else
                if (this.pais.ToLower().Equals("nazi"))
                    p = "Marinha Nazista";
                else
                if (this.pais.ToLower().Equals("ingl"))
                    p = "Marinha Real Britânica";
                else
                if (this.pais.ToLower().Equals("japao"))
                    p = "Marinha Imperial do Japão";
                else
                if (this.pais.ToLower().Equals("eua"))
                    p = "Marinha Americana";


                frmSingle s = new frmSingle(this.naval, p, this.paisAdv, this.nivel, soma, ehOCerto, estaTocando, this.p);
                s.Show();

                Close();
            });
        }
        private void frmNivel_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.temp)
                this.naval.Close();
        }
        private void pbxAudio_Click(object sender, EventArgs e)
        {
            if (estaTocando)
            {
                pbxAudio.BackgroundImage = Image.FromFile("../imagens/loudspeakerMute.png");
                p.Stop();
                p.Dispose();
                estaTocando = false;
                
            }
            else
            {
                p = new SoundPlayer();
                p.SoundLocation = "../audios/a juntar/Betrayal Desolation.wav";
                p.PlayLooping();
                estaTocando = true;
                pbxAudio.BackgroundImage = Image.FromFile("../imagens/loudspeaker.png");
            }
        }
    }
}
