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
    public partial class frmInfo : Form
    {
        private String texto;
        private bool clicou = false;
        private int nivel;
        private frmNaval nav;
        private SoundPlayer player;
        private bool estaTocando;
        private int soma;
        private bool temp = true;
        private bool ehOCerto;

        public frmInfo(frmNaval nav, String text, int niv, SoundPlayer p, bool estaTocando, int soma, bool ehOCerto)
        {
            if (text == null)
                throw new Exception("Texto invalido!!");

            InitializeComponent();
            this.texto = text;
            this.nav = nav;
            this.nivel = niv;
            this.player = p;
            this.ehOCerto = ehOCerto;
            this.estaTocando = estaTocando;

            if (!estaTocando)
                pbxAudio.BackgroundImage = Image.FromFile("../imagens/loudspeakerMute.png");
            this.soma = soma;
        }

        public void Iniciar()
        {
            if (player != null)
            {
                this.player.Stop();
                this.player.Dispose();
            }
            temp = false;
            frmNivel n = new frmNivel(this.nav, texto, nivel, this.soma, ehOCerto, estaTocando);
            n.Show();
            Close();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            if (!clicou)
            {
                frmJogo antes = new frmJogo(nav, soma);
                antes.Show();
                antes.ClickBot();

                this.Close();
            }
            else
            {
                clicou = false;
                if (texto.Equals("URSS"))
                {
                    lbExplicacao.Text =
                        "A Marinha Soviética foi formada em 1917 com base no que  " + Environment.NewLine +
                        "restava da Marinha Imperial Russa. Num primeiro momento," + Environment.NewLine +
                        "os danos sofridos pelos conflitos no país durante o início " + Environment.NewLine +
                        "do século XX, quase levaram ao desaparecimento completo da " + Environment.NewLine +
                        "Marinha, e a uma incompleta inoperância." + Environment.NewLine + Environment.NewLine +

                        "Com a industrialização da União Soviética, na década de 1930," + Environment.NewLine +
                        "a marinha foi pela primeira vez alvo de investimentos maciços, " + Environment.NewLine +
                        "que iriam fazer dela, no futuro, uma das marinhas mais " + Environment.NewLine +
                        "poderosas do mundo.";
                }
                else
                    if (texto.Equals("Nazi"))
                {
                    this.lbExplicacao.Text =
                    "Destruída após a 1ª Guerra Mundial e impedida de se " + Environment.NewLine +
                    "restabelecer por causa do Tratado de versalhes, a Marinha   " + Environment.NewLine +
                    "Alemã ficou enfraquecida até meados da década de 1930." + Environment.NewLine +
                    "Quando Hitler chegou ao poder e denunciou o Tratado, " + Environment.NewLine +
                    "restabeleceu a Força Aérea Alemã e permitiu a construção " + Environment.NewLine +
                    "de novos navios, melhores e mais potentes." + Environment.NewLine + Environment.NewLine +

                    "Com a grande produção de aço após a conquista da Álsacia - " + Environment.NewLine +
                    "Lorena, região francesa rica em ferro, a marinha de Hitler" + Environment.NewLine +
                    "passou a ser quase tão poderosa quanto a Marinha Inglesa." + Environment.NewLine;

                }
                else
                {
                    if (texto.Equals("Ingl"))
                    {
                        this.lbExplicacao.Text =
                        "Marinha Real Britânica é o ramo naval das Forças Armadas do " + Environment.NewLine +
                        "Reino Unido. Desde o final do século XVII até meados do " + Environment.NewLine +
                        "século XX, era a marinha mais poderosa do mundo," + Environment.NewLine +
                        "desempenhando um papel fundamental no estabelecimento do " + Environment.NewLine +
                        "Império Britânico como a potência mundial dominante." + Environment.NewLine +
                        Environment.NewLine +

                        "O fato da Grã - Bretanha ser uma ilha estimulou-a a investir" + Environment.NewLine +
                        "pesado em sua Marinha. Por isso, a Marinha Britânica sempre " + Environment.NewLine +
                        "foi uma das melhores do mundo.";

                    }
                    else
                    {
                        if (texto.Equals("Japao"))
                        {
                            this.lbExplicacao.Text =
                            "A Marinha Imperial Japonesa(MIJ) foi a marinha do Japão de " + Environment.NewLine +
                            "1869 até 1947, quando foi dissolvida após o Japão ter " + Environment.NewLine +
                            "renunciado constitucionalmente ao uso da força como meio para" + Environment.NewLine +
                            "resolver conflitos jurídicos internacionais. " + Environment.NewLine + Environment.NewLine +

                            "Por volta de 1920, era a terceira maior marinha do mundo," + Environment.NewLine +
                            "atrás somente da Royal Navy(Marinha Inglesa) e da United " + Environment.NewLine +
                            "States Navy(Marinha dos EUA).Recebia o apoio do Serviço " + Environment.NewLine +
                            "Aéreo da Marinha Imperial Japonesa para ataques aéreos " + Environment.NewLine +
                            "conduzidos a partir de sua frota. ";


                        }
                        else
                        {
                            if (texto.Equals("EUA"))
                            {
                                this.lbExplicacao.Text =
                                "A Marinha dos Estados Unidos não crescia muito antes da      " + Environment.NewLine +
                                "Segunda Guerra Mundial. Só depois de 1937 que a produção " + Environment.NewLine +
                                "de navios retomou-se em grande escala." + Environment.NewLine + Environment.NewLine +

                                "Por ter duas frentes de batalha nos mares, os EUA investiram" + Environment.NewLine +
                                "pesado em sua marinha, fazendo-a crescer tremendamente." + Environment.NewLine +
                                "A Marinha dos EUA lutou em grandes  batalhas contra a " + Environment.NewLine +
                                "Marinha Imperial do Japão.";


                            }
                        }
                    }

                }
            }
        }

        ///private void tratarFase()
       // {
        //    this.Controls.Clear();

        //    this.BackgroundImage
            
      //  }

        private void btnProx_Click(object sender, EventArgs e)
        {          
            if (clicou)
            {
                this.Iniciar();
            }
            else
            {
                clicou = true;

                if (texto.Equals("URSS"))
                {
                    lbExplicacao.Text =
                        "Às vésperas da Segunda Guerra Mundial, a frota de submarinos" + Environment.NewLine +
                        "da marinha soviética era a maior do mundo. Em termos " + Environment.NewLine +
                        "numéricos, ela tinha mais que o dobro de submarinos da " + Environment.NewLine +
                        "marinha americana e quase o quádruplo da alemã " + Environment.NewLine +
                        "(Kriegsmarine). Porém, como tinha poucas saídas para o mar, " + Environment.NewLine +
                        "as funções desta marinha eram restritas." + Environment.NewLine + Environment.NewLine +
                        "A marinha soviética afundou mais submarinos alemães que os\n" +
                        "aliados ocidentais e perdeu apenas três.";
                    
                }
                else
                {
                    if (texto.Equals("Nazi"))
                    {
                        lbExplicacao.Text =
                        "Foi fundamental para a conquista do norte da África e dos     " + Environment.NewLine +
                        "Países Bálticos e para protegê - los de ataques dos Aliados." + Environment.NewLine +
                        Environment.NewLine +

                        "Porém, com a entrada dos Estados Unidos na guerra, mais " + Environment.NewLine +
                        "fortalecidos que o resto da Europa(justamete por não terem " + Environment.NewLine +
                        "sido atacados), apoiados pela forte e resistente Marinha " + Environment.NewLine +
                        "Inglesa, os Aliados retomam a Normandia, no Dia D, o norte" + Environment.NewLine +
                        "da África e o sul da Itália, iniciando - se assim o fim dos" + Environment.NewLine+
                        "Nazistas.";

                    }
                    else
                    {
                        if(texto.Equals("Ingl"))
                        {
                            lbExplicacao.Text =
                                "Foi de extrema importância durante a 2ª Guerra Mundial." + Environment.NewLine +
                                "Hitler havia conquistado toda a Europa, porém a Inglaterra" + Environment.NewLine +
                                "resistiu por longos anos travando intensas batalhas navais" + Environment.NewLine +
                                "e aéreas contra os nazistas." + Environment.NewLine + Environment.NewLine +

                                "Hitler sabia que se a Inglaterra se rendesse, a guerra  " + Environment.NewLine +
                                "estaria praticamente vencida. Mas, a Marinha Inglesa impediu " + Environment.NewLine +
                                "que os nazistas chegassem à Inglatera, e ao receber apoio  " + Environment.NewLine +
                                "dos EUA a partir de 1942, o rumo da Guerra começou a se" + Environment.NewLine +
                                "reverter a favor dos Aliados."; 
                        }
                        else
                        {
                            if(texto.Equals("Japao"))
                            {
                                lbExplicacao.Text =
                                    "Foi a principal adversária dos Aliados na Guerra do Pacífico." + Environment.NewLine +
                                    "Responsável por dominar grande parte das ilhas asiáticas e " + Environment.NewLine +
                                    "auxiliar nos ataques japoneses a diversas regiões da Ásia." + Environment.NewLine +
                                    Environment.NewLine +
                                    "A história dos sucessos da marinha imperial, algumas vezes " + Environment.NewLine +
                                    "contra inimigos muito mais poderosos, como na Guerra " + Environment.NewLine +
                                    "Sino-Japonesa e na Guerra Russo-Japonesa, terminou em " + Environment.NewLine +
                                    "quase completa aniquilação durante o fim da Segunda Guerra" + Environment.NewLine +
                                    "em grande parte graças à Marinha dos Estados Unidos" + Environment.NewLine +
                                    "da América(USN).Em 1947, a MIJ foi dissolvida oficialmente. ";
                            }
                            else
                            {
                                if(texto.Equals("EUA"))
                                {
                                    lbExplicacao.Text =
                                      
                                    "A Marinha imperial Japonesa tinha uma superioridade no" + Environment.NewLine +
                                    "Pacífico e afundou o principal navio de batalha da marinha" + Environment.NewLine +
                                    "americana em Pearl Harbor num ataque surpresa. Após este " + Environment.NewLine +
                                    "ocorrido os EUA entraram na Segunda Guerra Mundial ao lado" + Environment.NewLine +
                                    "dos Aliados."
                                    + Environment.NewLine + Environment.NewLine +
                                    "Após a rendição alemã, numa demostração de poder perante" + Environment.NewLine +
                                    "a URSS, os EUA realizaram um ataque nuclear a civis no " + Environment.NewLine +
                                    "Japão nas cidades de Hiroshima e Nagasaki. Iniciava-se," + Environment.NewLine +
                                    "assim, a Guerra Fria.";
                                }
                            }
                        }
                    }
                }              
                
            }
            
        }

        private void frmInfo_Shown(object sender, EventArgs e)
        {
            if (texto.Equals("URSS"))
            {
                this.BackgroundImage = Image.FromFile("../../../../imagens/urss/urss2.jpg");
                this.pbxLogo.Image   = Image.FromFile("../../../../imagens/urss/soviet navy.png");
                this.lbTitulo.Text   = "Marinha Soviética";
                this.lbTitulo.Location = new Point(200, 7);             
                this.lbExplicacao.Text =
                        "A Marinha Soviética foi formada em 1917 com base no que  " + Environment.NewLine +
                        "restava da Marinha Imperial Russa. Num primeiro momento," + Environment.NewLine +
                        "os danos sofridos pelos conflitos no país durante o início " + Environment.NewLine +
                        "do século XX, quase levaram ao desaparecimento completo da " + Environment.NewLine +
                        "Marinha, e a uma incompleta inoperância." + Environment.NewLine + Environment.NewLine +

                        "Com a industrialização da União Soviética, da década de 1930," + Environment.NewLine +
                        "a marinha foi pela primeira vez alvo de investimentos maciços, " + Environment.NewLine +
                        "que iram fazer dela no futuro uma das marinhas mais " + Environment.NewLine +
                        "poderosas do mundo.";

            }
            else
            {
                if (texto.Equals("Nazi"))
                {
                    this.BackgroundImage = Image.FromFile("../../../../imagens/alemanha/encouracado.jpg");
                    this.pbxLogo.Image = Image.FromFile("../../../../imagens/alemanha/kriegsmarine.png");
                    this.lbTitulo.Text = "Marinha Nazista";
                    this.lbTitulo.Location = new Point(200, 7);
                    this.lbExplicacao.Text =
                    "Destruída após a 1ª Guerra Mundial e impedida de " + Environment.NewLine +
                    "restabelecer por causa do Tratado de versalhes, a Marinha   " + Environment.NewLine +
                    "Alemã ficou enfraquecida até meados da década de 1930."       + Environment.NewLine +
                    "Quando Hitler chegou ao poder e denunciou o Tratado, "        + Environment.NewLine +
                    "restabeleceu a Força Aérea Alemã e permitiu a construção "    + Environment.NewLine+
                    "de novos navios, melhores e mais potentes." + Environment.NewLine + Environment.NewLine +

                    "Com a grande produção de aço após a conquista da Álsacia - "  + Environment.NewLine +
                    "Lorena, região francesa rica em ferro, a marinha de Hitler"   + Environment.NewLine +
                    "passou a ser quase tão poderosa quanto a Marinha Inglesa."    + Environment.NewLine;

                }
                else
                {
                    if (texto.Equals("Ingl"))
                    {
                        this.BackgroundImage = Image.FromFile("../../../../imagens/inglaterra/inglaterra.png");
                        this.pbxLogo.Image = Image.FromFile("../../../../imagens/inglaterra/england_logo.png");
                        this.lbTitulo.Text = "Marinha Inglesa";
                        this.lbTitulo.Location = new Point(200, 7);
                        this.lbExplicacao.Text =
                            "Marinha Real Britânica é o ramo naval das Forças Armadas do " + Environment.NewLine +
                            "Reino Unido. Desde o final do século XVII até meados do " + Environment.NewLine +
                            "século XX, era a marinha mais poderosa do mundo," + Environment.NewLine +
                            "desempenhando um papel fundamental no estabelecimento do " + Environment.NewLine +
                            "Império Britânico como a potência mundial dominante." + Environment.NewLine +
                            Environment.NewLine +

                            "O fato da Grã - Bretanha ser uma ilha estimulou-a a investir" + Environment.NewLine +
                            "pesado em sua Marinha. Por isso, a Marinha Britânica sempre " + Environment.NewLine +
                            "foi uma das melhores do mundo.";

                    }
                    else
                    {
                        if (texto.Equals("Japao"))
                        {
                            this.BackgroundImage = Image.FromFile("../../../../imagens/japao/japan_ship.jpg");
                            this.pbxLogo.Image = Image.FromFile("../../../../imagens/japao/japao_logo.png");
                            this.lbTitulo.Text = "Marinha Japonesa";
                            this.lbTitulo.Location = new Point(200, 7);
                            this.lbExplicacao.Text =
                            "A Marinha Imperial Japonesa(MIJ) foi a marinha do Japão de " + Environment.NewLine +
                            "1869 até 1947, quando foi dissolvida após o Japão ter " + Environment.NewLine +
                            "renunciado constitucionalmente ao uso da força como meio para" + Environment.NewLine +
                            "resolver conflitos jurídicos internacionais. " + Environment.NewLine + Environment.NewLine+

                            "Por volta de 1920, era a terceira maior marinha do mundo,"+ Environment.NewLine +
                            "atrás somente da Royal Navy(Marinha Inglesa) e da United "+ Environment.NewLine +
                            "States Navy(Marinha dos EUA).Recebia o apoio do Serviço " + Environment.NewLine +
                            "Aéreo da Marinha Imperial Japonesa para ataques aéreos " + Environment.NewLine +
                            "conduzidos a partir de sua frota. ";

                            
                        }
                        else
                        {
                            if (texto.Equals("EUA"))
                            {
                                this.BackgroundImage = Image.FromFile("../../../../imagens/eua/eua.jpg");
                                this.pbxLogo.Image = Image.FromFile("../../../../imagens/eua/us_logo.png");
                                this.lbTitulo.Text = "Marinha Americana";
                                this.lbTitulo.Location = new Point(200, 7);
                                this.lbExplicacao.Text =
                                "A Marinha dos Estados Unidos não crescia muito antes da      " + Environment.NewLine +
                                "Segunda Guerra Mundial. Só depois de 1937 que a produção " + Environment.NewLine +
                                "de navios retomou-se em grande escala." + Environment.NewLine + Environment.NewLine +

                                "Por ter duas frentes de batalha nos mares, os EUA investiram" + Environment.NewLine +
                                "pesado em sua marinha, fazendo-a crescer tremendamente." + Environment.NewLine +
                                "A Marinha dos EUA lutou em grandes  batalhas contra a " + Environment.NewLine +
                                "Marinha Imperial do Japão.";

                            }
                        }
                    }
                }
            }
               
        }

        private void fechar(bool navTambem)
        {
            if (navTambem)
                this.fecharNaval();
            else
                this.Close();
        }

        private void fecharNaval()
        {
            this.nav.Close();
        }

        private void frmInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.temp)
                this.nav.Close();            
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
