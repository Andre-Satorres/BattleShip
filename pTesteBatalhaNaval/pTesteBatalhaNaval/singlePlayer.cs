using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Media;

namespace pTesteBatalhaNaval
{
    public partial class frmSingle : Form
    {
        private Boolean clicou = false;
        private frmNaval nav;
        private bool vezDoBot = false;
        private int temPreferencia = -1;
        private int tamanmhoUlt = 0;
        private String ultimaJogada;
        private int qtdeNaviosMeus = 30;
        private int qtdeNaviosOponente = 30;
        private int qtdNaviosDe4Inseridos = 0;
        private int qtdNaviosDe3Inseridos = 0;
        private int qtdNaviosDe2Inseridos = 0;
        private bool inseriuODe5 = false;
        private bool estaInserindoNoMomento = false;
        private int ultimoIndexInserido = 0;
        private String indicesInseridos = "";
        private Label labelDe2;
        private Label labelDe3;
        private Label labelDe4;
        private Label labelDe5;
        private String posicaoAtual;
        private bool estaInserindo = false;
        private String pais;
        private String paisAdv;
        private int nivel;
        private PictureBox pbxDe2;
        private PictureBox pbxDe3;
        private PictureBox pbxDe4;
        private PictureBox pbxDe5;

        private int soma;
        private bool ehOCerto;

        private SoundPlayer player1;
        private SoundPlayer player2;

        private static readonly Random random = new Random();
        private static object synclock = new object();

        private String[,] matriz = new String[10, 10];
        private String[,] matrizAtaque = new String[10, 10];
        private String[] paises = new String[5];
        private String[] naviosInseridos;

        private bool estaTocando = false;
        private SoundPlayer pFundo;

        private bool temp = true;

        public frmSingle(frmNaval nava, String pais, String paisAdv, int nivel, int s, bool ehOCerto, bool estaTocando, SoundPlayer p)
        {
            InitializeComponent();

            this.soma = s;
            this.pais = pais;
            this.paisAdv = paisAdv;
            this.nivel = nivel;
            this.ehOCerto = ehOCerto;
            this.pFundo = p;

            this.estaTocando = estaTocando;

            if(!estaTocando)
                pbxAudio.BackgroundImage = Image.FromFile("../imagens/loudspeakerMute.png");

            this.nav = nava;

            this.lblJogador.Text = paisAdv;
            this.lblComp.Text = this.pais;

            if (this.pais.Equals("Marinha Nazista"))
                this.pais = "alemanha";
            else
                if (this.pais.Equals("Marinha Soviética"))
                this.pais = "urss";
            else
                if (this.pais.Equals("Marinha Real Britânica"))
                this.pais = "inglaterra";
            else
                if (this.pais.Equals("Marinha Imperial do Japão"))
                this.pais = "japao";
            else
                this.pais = "eua";

            if (this.paisAdv.Equals("Marinha Nazista"))
                this.paisAdv = "alemanha";
            else
                if (this.paisAdv.Equals("Marinha Soviética"))
                this.paisAdv = "urss";
            else
                if (this.paisAdv.Equals("Marinha Real Britânica"))
                this.paisAdv = "inglaterra";
            else
                if (this.paisAdv.Equals("Marinha Imperial do Japão"))
                this.paisAdv = "japao";
            else
                this.paisAdv = "eua";
        }

        private bool temEspaco(String[,] mat, int tam)
        {
            int qtd = 0;
            int direcao = RandomNumber(4);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    while (direcao % 2 == 0)
                        direcao = RandomNumber(4);

                    if (mat[i, j] == "0.0.0")
                        qtd++;
                    else
                        qtd = 0;

                    if (qtd == tam)
                    {
                        if (naviosInseridos == null)
                            this.naviosInseridos = new String[10];

                        if (tam == 5)
                            if (direcao == 3)
                            {
                                this.colocar5NumLugarEspecifico(mat, i, j, direcao);
                                this.indicesInseridos = 9 + indicesInseridos;
                                this.posicaoAtual = "" + i + j;
                                this.naviosInseridos[9] = "" + i + j + this.formadorDePosicoes();
                                this.ultimoIndexInserido = 9;
                            }
                            else
                            {
                                this.colocar5NumLugarEspecifico(mat, i, j - 4, direcao);
                                this.indicesInseridos = 9 + indicesInseridos;
                                this.posicaoAtual = "" + i + Convert.ToString(j - 4);
                                this.naviosInseridos[9] = "" + i + Convert.ToString(j - 4) + this.formadorDePosicoes();
                                this.ultimoIndexInserido = 9;
                            }

                        else
                        if (tam == 4)
                        {
                            if (direcao == 3)
                            {
                                this.colocar4NumLocalEspecif(mat, i, j, direcao);

                                this.posicaoAtual = "" + i + Convert.ToString(j);
                                if (this.naviosInseridos[7] == null)
                                {
                                    this.indicesInseridos = 7 + indicesInseridos;
                                    this.naviosInseridos[7] = "" + i + Convert.ToString(j) + this.formadorDePosicoes();
                                    this.ultimoIndexInserido = 7;
                                }
                                else
                                {
                                    this.indicesInseridos = 8 + indicesInseridos;
                                    this.naviosInseridos[8] = "" + i + Convert.ToString(j) + this.formadorDePosicoes();
                                    this.ultimoIndexInserido = 8;
                                }
                            }
                            else
                            {
                                this.colocar4NumLocalEspecif(mat, i, j - 3, direcao);

                                this.posicaoAtual = "" + i + Convert.ToString(j - 3);
                                if (this.naviosInseridos[7] == null)
                                {
                                    this.indicesInseridos = 7 + indicesInseridos;
                                    this.naviosInseridos[7] = "" + i + Convert.ToString(j - 3) + this.formadorDePosicoes();
                                    this.ultimoIndexInserido = 7;
                                }
                                else
                                {
                                    this.indicesInseridos = 8 + indicesInseridos;
                                    this.naviosInseridos[8] = "" + i + Convert.ToString(j - 3) + this.formadorDePosicoes();
                                    this.ultimoIndexInserido = 8;
                                }
                            }
                        }

                        return true;
                    }
                }
                qtd = 0;
            }

            qtd = 0;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    while (direcao % 2 != 0)
                        direcao = RandomNumber(4);

                    if (mat[j, i] == "0.0.0")
                        qtd++;
                    else
                        qtd = 0;

                    if (qtd == tam)
                    {
                        if (naviosInseridos == null)
                            this.naviosInseridos = new String[10];

                        if (tam == 5)
                            if (direcao == 2)
                            {
                                this.colocar5NumLugarEspecifico(mat, j, i, direcao);
                                this.posicaoAtual = "" + Convert.ToString(j) + i;
                                this.indicesInseridos = 9 + indicesInseridos;
                                this.naviosInseridos[9] = "" + Convert.ToString(j) + i + this.formadorDePosicoes();
                                this.ultimoIndexInserido = 9;

                            }
                            else
                            {
                                this.colocar5NumLugarEspecifico(mat, j - 4, i, direcao);
                                this.posicaoAtual = "" + Convert.ToString(j - 4) + i;
                                this.indicesInseridos = 9 + indicesInseridos;
                                this.naviosInseridos[9] = "" + Convert.ToString(j - 4) + i + this.formadorDePosicoes();
                                this.ultimoIndexInserido = 9;

                            }
                        else
                        if (tam == 4)
                        {
                            if (direcao == 2)
                            {
                                this.colocar4NumLocalEspecif(mat, j, i, direcao);

                                this.posicaoAtual = "" + Convert.ToString(j) + i;
                                if (this.naviosInseridos[7] == null)
                                {
                                    this.indicesInseridos = 7 + indicesInseridos;
                                    this.naviosInseridos[7] = "" + Convert.ToString(j) + i + this.formadorDePosicoes();
                                    this.ultimoIndexInserido = 7;
                                }
                                else
                                {
                                    this.indicesInseridos = 8 + indicesInseridos;
                                    this.naviosInseridos[8] = "" + Convert.ToString(j) + i + this.formadorDePosicoes();
                                    this.ultimoIndexInserido = 8;
                                }
                            }
                            else
                            {
                                this.colocar4NumLocalEspecif(mat, j - 3, i, direcao);

                                this.posicaoAtual = "" + Convert.ToString(j - 3) + i;
                                if (this.naviosInseridos[7] == null)
                                {
                                    this.indicesInseridos = 7 + indicesInseridos;
                                    this.naviosInseridos[7] = "" + Convert.ToString(j - 3) + i + this.formadorDePosicoes();
                                    this.ultimoIndexInserido = 7;
                                }
                                else
                                {
                                    this.indicesInseridos = 8 + indicesInseridos;
                                    this.naviosInseridos[8] = "" + Convert.ToString(j - 3) + i + this.formadorDePosicoes();
                                    this.ultimoIndexInserido = 8;
                                }
                            }
                        }

                        return true;
                    }

                }
                qtd = 0;
            }

            return false;
        }

        private void colocar5NumLugarEspecifico(String[,] matriz, int x, int y, int direcao)
        {

            if (direcao == 0) //esquerda
            {
                if (x < 6 && matriz[x + 1, y] == "0.0.0" && matriz[x + 2, y] == "0.0.0" && matriz[x + 3, y] == "0.0.0" && matriz[x + 4, y] == "0.0.0")
                {
                    matriz[x, y] = "5.1.0";
                    matriz[x + 1, y] = "5.2.0";
                    matriz[x + 2, y] = "5.3.0";
                    matriz[x + 3, y] = "5.4.0";
                    matriz[x + 4, y] = "5.5.0";

                    if (x > 0)
                        matriz[x - 1, y] = "1.1.1";
                    if (y > 0)
                        matriz[x, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x, y + 1] = "1.1.1";

                    if (y > 0)
                        matriz[x + 1, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x + 1, y + 1] = "1.1.1";

                    if (y > 0)
                        matriz[x + 2, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x + 2, y + 1] = "1.1.1";

                    if (y > 0)
                        matriz[x + 3, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x + 3, y + 1] = "1.1.1";

                    if (y > 0)
                        matriz[x + 4, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x + 4, y + 1] = "1.1.1";
                    if (x < 5)
                        matriz[x + 5, y] = "1.1.1";

                }
            }
            else
                if (direcao == 1) //cima
            {
                if (y < 6 && matriz[x, y + 1] == "0.0.0" && matriz[x, y + 2] == "0.0.0" && matriz[x, y + 3] == "0.0.0" && matriz[x, y + 4] == "0.0.0")
                {
                    matriz[x, y] = "5.1.1";
                    matriz[x, y + 1] = "5.2.1";
                    matriz[x, y + 2] = "5.3.1";
                    matriz[x, y + 3] = "5.4.1";
                    matriz[x, y + 4] = "5.5.1";

                    if (x > 0)
                        matriz[x - 1, y] = "1.1.1";
                    if (y > 0)
                        matriz[x, y - 1] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y] = "1.1.1";

                    if (x > 0)
                        matriz[x - 1, y + 1] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y + 1] = "1.1.1";

                    if (x > 0)
                        matriz[x - 1, y + 2] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y + 2] = "1.1.1";

                    if (x > 0)
                        matriz[x - 1, y + 3] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y + 3] = "1.1.1";

                    if (x > 0)
                        matriz[x - 1, y + 4] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y + 4] = "1.1.1";
                    if (y < 5)
                        matriz[x, y + 5] = "1.1.1";

                }
            }
            else
                if (direcao == 2) //direita
            {
                if (x > 3 && matriz[x - 1, y] == "0.0.0" && matriz[x - 2, y] == "0.0.0" && matriz[x - 3, y] == "0.0.0" && matriz[x - 4, y] == "0.0.0")
                {
                    matriz[x, y] = "5.1.2";
                    matriz[x - 1, y] = "5.2.2";
                    matriz[x - 2, y] = "5.3.2";
                    matriz[x - 3, y] = "5.4.2";
                    matriz[x - 4, y] = "5.5.2";

                    if (y > 0)
                        matriz[x, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x, y + 1] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y] = "1.1.1";

                    if (y > 0)
                        matriz[x - 1, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x - 1, y + 1] = "1.1.1";

                    if (y > 0)
                        matriz[x - 2, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x - 2, y + 1] = "1.1.1";

                    if (y > 0)
                        matriz[x - 3, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x - 3, y + 1] = "1.1.1";

                    if (y > 0)
                        matriz[x - 4, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x - 4, y + 1] = "1.1.1";
                    if (x > 4)
                        matriz[x - 5, y] = "1.1.1";
                }
            }
            else
                if (direcao == 3) //baixo
            {
                if (y > 3 && matriz[x, y - 1] == "0.0.0" && matriz[x, y - 2] == "0.0.0" && matriz[x, y - 3] == "0.0.0" && matriz[x, y - 4] == "0.0.0")
                {
                    matriz[x, y] = "5.1.3";
                    matriz[x, y - 1] = "5.2.3";
                    matriz[x, y - 2] = "5.3.3";
                    matriz[x, y - 3] = "5.4.3";
                    matriz[x, y - 4] = "5.5.3";

                    if (x > 0)
                        matriz[x - 1, y] = "1.1.1";
                    if (y < 9)
                        matriz[x, y + 1] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y] = "1.1.1";

                    if (x > 0)
                        matriz[x - 1, y - 1] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y - 1] = "1.1.1";

                    if (x > 0)
                        matriz[x - 1, y - 2] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y - 2] = "1.1.1";

                    if (x > 0)
                        matriz[x - 1, y - 3] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y - 3] = "1.1.1";

                    if (x > 0)
                        matriz[x - 1, y - 4] = "1.1.1";
                    if (y > 4)
                        matriz[x, y - 5] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y - 4] = "1.1.1";
                }
            }
        }
        private void colocar4NumLocalEspecif(String[,] matriz, int x, int y, int direcao)
        {

            if (direcao == 0) //esquerda
            {
                if (x < 7 && matriz[x + 1, y] == "0.0.0" && matriz[x + 2, y] == "0.0.0" && matriz[x + 3, y] == "0.0.0")
                {
                    matriz[x, y] = "4.1.0";
                    matriz[x + 1, y] = "4.2.0";
                    matriz[x + 2, y] = "4.3.0";
                    matriz[x + 3, y] = "4.4.0";

                    if (x > 0)
                        matriz[x - 1, y] = "1.1.1";
                    if (y > 0)
                        matriz[x, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x, y + 1] = "1.1.1";

                    if (y > 0)
                        matriz[x + 1, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x + 1, y + 1] = "1.1.1";

                    if (y > 0)
                        matriz[x + 2, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x + 2, y + 1] = "1.1.1";

                    if (y > 0)
                        matriz[x + 3, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x + 3, y + 1] = "1.1.1";
                    if (x < 6)
                        matriz[x + 4, y] = "1.1.1";
                }
            }
            else
            if (direcao == 1) //cima
            {
                if (y < 7 && matriz[x, y + 1] == "0.0.0" && matriz[x, y + 2] == "0.0.0" && matriz[x, y + 3] == "0.0.0")
                {
                    matriz[x, y] = "4.1.1";
                    matriz[x, y + 1] = "4.2.1";
                    matriz[x, y + 2] = "4.3.1";
                    matriz[x, y + 3] = "4.4.1";

                    if (x > 0)
                        matriz[x - 1, y] = "1.1.1";
                    if (y > 0)
                        matriz[x, y - 1] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y] = "1.1.1";

                    if (x > 0)
                        matriz[x - 1, y + 1] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y + 1] = "1.1.1";

                    if (x > 0)
                        matriz[x - 1, y + 2] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y + 2] = "1.1.1";

                    if (x > 0)
                        matriz[x - 1, y + 3] = "1.1.1";
                    if (y < 6)
                        matriz[x, y + 4] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y + 3] = "1.1.1";
                }
            }
            else
            if (direcao == 2) //esquerda
            {
                if (x > 2 && matriz[x - 1, y] == "0.0.0" && matriz[x - 2, y] == "0.0.0" && matriz[x - 3, y] == "0.0.0")
                {
                    matriz[x, y] = "4.1.2";
                    matriz[x - 1, y] = "4.2.2";
                    matriz[x - 2, y] = "4.3.2";
                    matriz[x - 3, y] = "4.4.2";

                    if (y > 0)
                        matriz[x, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x, y + 1] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y] = "1.1.1";

                    if (y > 0)
                        matriz[x - 1, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x - 1, y + 1] = "1.1.1";

                    if (y > 0)
                        matriz[x - 2, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x - 2, y + 1] = "1.1.1";

                    if (y > 0)
                        matriz[x - 3, y - 1] = "1.1.1";
                    if (y < 9)
                        matriz[x - 3, y + 1] = "1.1.1";
                    if (x > 3)
                        matriz[x - 4, y] = "1.1.1";

                }
            }
            else
            if (direcao == 3) //baixo
            {
                if (y > 2 && matriz[x, y - 1] == "0.0.0" && matriz[x, y - 2] == "0.0.0" && matriz[x, y - 3] == "0.0.0")
                {
                    matriz[x, y] = "4.1.3";
                    matriz[x, y - 1] = "4.2.3";
                    matriz[x, y - 2] = "4.3.3";
                    matriz[x, y - 3] = "4.4.3";

                    if (x > 0)
                        matriz[x - 1, y] = "1.1.1";
                    if (y < 9)
                        matriz[x, y + 1] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y] = "1.1.1";

                    if (x > 0)
                        matriz[x - 1, y - 1] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y - 1] = "1.1.1";

                    if (x > 0)
                        matriz[x - 1, y - 2] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y - 2] = "1.1.1";

                    if (x > 0)
                        matriz[x - 1, y - 3] = "1.1.1";
                    if (y > 3)
                        matriz[x, y - 4] = "1.1.1";
                    if (x < 9)
                        matriz[x + 1, y - 3] = "1.1.1";

                }
            }
        }

        private void InicializaNavios(String[,] matriz)
        {
            for (int i = 0; i <= 9; i++)
                for (int j = 0; j <= 9; j++)
                    matriz[i, j] = "0.0.0";
        }
        private void RecarregarDtg(DataGridView dtg, bool preencher, String pais)
        {
            String[,] mat;
            if (dtg.Equals(dtgAtaque))
                mat = matrizAtaque;
            else
                mat = matriz;

            if (!preencher)
            {
                this.InicializaNavios(mat);
                int num = 1;
                for (int j = 0; j < 10; j++)
                    for (int i = 0; i < 10; i++)
                    {
                        if (pais != "japao")
                        {
                            if (num < 10)
                                dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_00" + Convert.ToString(num) + ".jpg");
                            else
                                        if (num < 100)
                                dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_0" + Convert.ToString(num) + ".jpg");
                            else
                                dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_" + Convert.ToString(num) + ".jpg");

                        }
                        else
                        {
                            if (num < 10)
                                dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_00" + Convert.ToString(num) + ".png");
                            else
                                        if (num < 100)
                                dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_0" + Convert.ToString(num) + ".png");
                            else
                                dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_" + Convert.ToString(num) + ".png");

                        }
                        num++;
                    }
            }
            else
            {
                int num = 1;
                for (int j = 0; j < 10; j++)
                    for (int i = 0; i < 10; i++)
                    {
                        if (pais != "japao")
                        {
                            if (num < 10)
                                dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_00" + Convert.ToString(num) + ".jpg");
                            else
                                        if (num < 100)
                                dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_0" + Convert.ToString(num) + ".jpg");
                            else
                                dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_" + Convert.ToString(num) + ".jpg");

                        }
                        else
                        {
                            if (num < 10)
                                dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_00" + Convert.ToString(num) + ".png");
                            else
                                        if (num < 100)
                                dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_0" + Convert.ToString(num) + ".png");
                            else
                                dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_" + Convert.ToString(num) + ".png");

                        }

                        num++;
                    }

                for (int i = 0; i < 4; i++)
                    this.colocarDe2(mat);

                for (int i = 0; i < 3; i++)
                    this.colocarDe3(mat);

                if (!this.temEspaco(mat, 4))
                {
                    this.InicializaNavios(mat);
                    this.RecarregarDtg(dtg, true, this.pais);
                }

                if (!this.temEspaco(mat, 4))
                {
                    this.InicializaNavios(mat);
                    this.RecarregarDtg(dtg, true, this.pais);
                }


                if (!this.temEspaco(mat, 5))
                {
                    this.InicializaNavios(mat);
                    this.RecarregarDtg(dtg, true, this.pais);
                }

                int direcao = -1;

                for (int i = 0; i < 10; i++) //muda a coluna
                    for (int j = 0; j < 10; j++) //linha a linha
                    {
                        string valorImagem = mat[i, j].ToString().Substring(0, 1);
                        string qualImagem = mat[i, j].ToString().Substring(2, 1);

                        if (mat[i, j].ToString().Equals("1.1.1") || mat[i, j].ToString().Equals("0.0.0"))
                        {
                            continue;
                        }

                        if (!(mat[i, j].ToString().Equals("0.0.0")))
                            direcao = Convert.ToInt16(mat[i, j].ToString().Substring(4, 1));

                        Image img = imlImagens.Images[imlImagens.Images.Keys.IndexOf(mat[i, j].ToString().Substring(0, 3) + ".png")];

                        if (direcao == 2) //direita
                            img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        else
                        if (direcao == 1) //cima
                            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        else //JA ESTAO POR PADRAO À ESQUERDA
                        if (direcao == 3) //baixo
                            img.RotateFlip(RotateFlipType.Rotate90FlipY);

                        dtg[i + 1, j].Value = null;
                        dtg[i + 1, j].Value = img;
                    }
            }
        }
        private void colocarDe2(String[,] matriz)
        {
            int direcao;
            int x;
            int y;

            direcao = RandomNumber(4);
            x = RandomNumber(10);
            y = RandomNumber(10);

            while (true)
            {
                while (matriz[x, y] != "0.0.0") //se for navio pega novas coordenadas
                {
                    x = RandomNumber(10);
                    y = RandomNumber(10);
                }
                if (direcao == 0) //esquerda
                {
                    if (x < 9 && matriz[x + 1, y] == "0.0.0")
                    {
                        matriz[x, y] = "2.1.0";
                        matriz[x + 1, y] = "2.2.0";

                        if (x > 0)
                            matriz[x - 1, y] = "1.1.1";
                        if (y > 0)
                            matriz[x, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x, y + 1] = "1.1.1";

                        if (y > 0)
                            matriz[x + 1, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x + 1, y + 1] = "1.1.1";
                        if (x < 8)
                            matriz[x + 2, y] = "1.1.1";

                        if (naviosInseridos == null)
                            naviosInseridos = new String[10];
                        this.posicaoAtual = "" + x + Convert.ToString(y);
                        if (this.naviosInseridos[0] == null)
                        {
                            this.indicesInseridos = 0 + indicesInseridos;
                            this.naviosInseridos[0] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 0;
                        }
                        else
                        if (this.naviosInseridos[1] == null)
                        {
                            this.indicesInseridos = 1 + indicesInseridos;
                            this.naviosInseridos[1] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 1;
                        }
                        else
                        if (this.naviosInseridos[2] == null)
                        {
                            this.indicesInseridos = 2 + indicesInseridos;
                            this.naviosInseridos[2] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 2;
                        }
                        else
                        if (this.naviosInseridos[3] == null)
                        {
                            this.indicesInseridos = 3 + indicesInseridos;
                            this.naviosInseridos[3] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 3;
                        }
                        break;
                    }
                }
                else
                if (direcao == 1) //cima
                {
                    if (y < 9 && matriz[x, y + 1] == "0.0.0")
                    {
                        matriz[x, y] = "2.1.1";
                        matriz[x, y + 1] = "2.2.1";

                        if (x > 0)
                            matriz[x - 1, y] = "1.1.1";
                        if (y > 0)
                            matriz[x, y - 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y + 1] = "1.1.1";
                        if (y < 8)
                            matriz[x, y + 2] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y + 1] = "1.1.1";

                        if (naviosInseridos == null)
                            naviosInseridos = new String[10];

                        this.posicaoAtual = "" + x + Convert.ToString(y);
                        if (this.naviosInseridos[0] == null)
                        {
                            this.indicesInseridos = 0 + indicesInseridos;
                            this.naviosInseridos[0] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 0;
                        }
                        else
                        if (this.naviosInseridos[1] == null)
                        {
                            this.indicesInseridos = 1 + indicesInseridos;
                            this.naviosInseridos[1] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 1;
                        }
                        else
                        if (this.naviosInseridos[2] == null)
                        {
                            this.indicesInseridos = 2 + indicesInseridos;
                            this.naviosInseridos[2] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 2;
                        }
                        else
                        if (this.naviosInseridos[3] == null)
                        {
                            this.indicesInseridos = 3 + indicesInseridos;
                            this.naviosInseridos[3] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 3;
                        }
                        break;
                    }
                }
                else
                if (direcao == 2) //direita
                {
                    if (x > 0 && matriz[x - 1, y] == "0.0.0")
                    {
                        matriz[x, y] = "2.1.2";
                        matriz[x - 1, y] = "2.2.2";
                        if (y > 0)
                            matriz[x, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x, y + 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y] = "1.1.1";

                        if (y > 0)
                            matriz[x - 1, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x - 1, y + 1] = "1.1.1";
                        if (x > 1)
                            matriz[x - 2, y] = "1.1.1";

                        if (naviosInseridos == null)
                            naviosInseridos = new String[10];

                        this.posicaoAtual = "" + x + Convert.ToString(y);
                        if (this.naviosInseridos[0] == null)
                        {
                            this.indicesInseridos = 0 + indicesInseridos;
                            this.naviosInseridos[0] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 0;
                        }
                        else
                        if (this.naviosInseridos[1] == null)
                        {
                            this.indicesInseridos = 1 + indicesInseridos;
                            this.naviosInseridos[1] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 1;
                        }
                        else
                        if (this.naviosInseridos[2] == null)
                        {
                            this.indicesInseridos = 2 + indicesInseridos;
                            this.naviosInseridos[2] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 2;
                        }
                        else
                        if (this.naviosInseridos[3] == null)
                        {
                            this.indicesInseridos = 3 + indicesInseridos;
                            this.naviosInseridos[3] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 3;
                        }

                        break;
                    }
                }
                else
                if (direcao == 3)//baixo
                {
                    if (y > 0 && matriz[x, y - 1] == "0.0.0")
                    {
                        matriz[x, y] = "2.1.3";
                        matriz[x, y - 1] = "2.2.3";
                        if (x > 0)
                            matriz[x - 1, y] = "1.1.1";
                        if (y < 9)
                            matriz[x, y + 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y - 1] = "1.1.1";
                        if (y > 1)
                            matriz[x, y - 2] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y - 1] = "1.1.1";

                        if (naviosInseridos == null)
                            naviosInseridos = new String[10];

                        this.posicaoAtual = "" + x + Convert.ToString(y);
                        if (this.naviosInseridos[0] == null)
                        {
                            this.indicesInseridos = 0 + indicesInseridos;
                            this.naviosInseridos[0] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 0;
                        }
                        else
                        if (this.naviosInseridos[1] == null)
                        {
                            this.indicesInseridos = 1 + indicesInseridos;
                            this.naviosInseridos[1] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 1;
                        }
                        else
                        if (this.naviosInseridos[2] == null)
                        {
                            this.indicesInseridos = 2 + indicesInseridos;
                            this.naviosInseridos[2] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 2;
                        }
                        else
                        if (this.naviosInseridos[3] == null)
                        {
                            this.indicesInseridos = 3 + indicesInseridos;
                            this.naviosInseridos[3] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 3;
                        }
                        break;
                    }
                }

                direcao = RandomNumber(4);
                x = RandomNumber(10);
                y = RandomNumber(10);
            }

        }
        private void colocarDe3(String[,] matriz)
        {
            int direcao;
            int x;
            int y;

            direcao = RandomNumber(4);
            x = RandomNumber(10);
            y = RandomNumber(10);

            while (true)
            {
                while (matriz[x, y] != "0.0.0") //se for navio pega novas coordenadas
                {
                    x = RandomNumber(10);
                    y = RandomNumber(10);
                }
                if (direcao == 0) //esquerda
                {
                    if (x < 8 && matriz[x + 1, y] == "0.0.0" && matriz[x + 2, y] == "0.0.0")
                    {
                        matriz[x, y] = "3.1.0";
                        matriz[x + 1, y] = "3.2.0";
                        matriz[x + 2, y] = "3.3.0";

                        if (x > 0)
                            matriz[x - 1, y] = "1.1.1";
                        if (y > 0)
                            matriz[x, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x, y + 1] = "1.1.1";

                        if (y > 0)
                            matriz[x + 1, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x + 1, y + 1] = "1.1.1";

                        if (y > 0)
                            matriz[x + 2, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x + 2, y + 1] = "1.1.1";
                        if (x < 7)
                            matriz[x + 3, y] = "1.1.1";

                        if (naviosInseridos == null)
                            naviosInseridos = new String[10];

                        this.posicaoAtual = "" + x + Convert.ToString(y);
                        if (this.naviosInseridos[4] == null)
                        {
                            this.indicesInseridos = 4 + indicesInseridos;
                            this.naviosInseridos[4] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 4;
                        }
                        else
                        if (this.naviosInseridos[5] == null)
                        {
                            this.indicesInseridos = 5 + indicesInseridos;
                            this.naviosInseridos[5] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 5;
                        }
                        else
                        if (this.naviosInseridos[6] == null)
                        {
                            this.indicesInseridos = 6 + indicesInseridos;
                            this.naviosInseridos[6] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 6;
                        }

                        break;
                    }
                }
                else
                if (direcao == 1) //cima
                {
                    if (y < 8 && matriz[x, y + 1] == "0.0.0" && matriz[x, y + 2] == "0.0.0")
                    {
                        matriz[x, y] = "3.1.1";
                        matriz[x, y + 1] = "3.2.1";
                        matriz[x, y + 2] = "3.3.1";

                        if (x > 0)
                            matriz[x - 1, y] = "1.1.1";
                        if (y > 0)
                            matriz[x, y - 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y + 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y + 1] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y + 2] = "1.1.1";
                        if (y < 7)
                            matriz[x, y + 3] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y + 2] = "1.1.1";

                        if (naviosInseridos == null)
                            naviosInseridos = new String[10];

                        this.posicaoAtual = "" + x + Convert.ToString(y);
                        if (this.naviosInseridos[4] == null)
                        {
                            this.indicesInseridos = 4 + indicesInseridos;
                            this.naviosInseridos[4] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 4;
                        }
                        else
                        if (this.naviosInseridos[5] == null)
                        {
                            this.indicesInseridos = 5 + indicesInseridos;
                            this.naviosInseridos[5] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 5;
                        }
                        else
                        if (this.naviosInseridos[6] == null)
                        {
                            this.indicesInseridos = 6 + indicesInseridos;
                            this.naviosInseridos[6] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 6;
                        }

                        break;
                    }
                }
                else
                if (direcao == 2) //direita
                {
                    if (x > 1 && matriz[x - 1, y] == "0.0.0" && matriz[x - 2, y] == "0.0.0")
                    {
                        matriz[x, y] = "3.1.2";
                        matriz[x - 1, y] = "3.2.2";
                        matriz[x - 2, y] = "3.3.2";

                        if (y > 0)
                            matriz[x, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x, y + 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y] = "1.1.1";

                        if (y > 0)
                            matriz[x - 1, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x - 1, y + 1] = "1.1.1";

                        if (y > 0)
                            matriz[x - 2, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x - 2, y + 1] = "1.1.1";
                        if (x > 2)
                            matriz[x - 3, y] = "1.1.1";

                        if (naviosInseridos == null)
                            naviosInseridos = new String[10];

                        this.posicaoAtual = "" + x + Convert.ToString(y);
                        if (this.naviosInseridos[4] == null)
                        {
                            this.indicesInseridos = 4 + indicesInseridos;
                            this.naviosInseridos[4] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 4;
                        }
                        else
                        if (this.naviosInseridos[5] == null)
                        {
                            this.indicesInseridos = 5 + indicesInseridos;
                            this.naviosInseridos[5] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 5;
                        }
                        else
                        if (this.naviosInseridos[6] == null)
                        {
                            this.indicesInseridos = 6 + indicesInseridos;
                            this.naviosInseridos[6] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 6;
                        }

                        break;
                    }
                }
                else
                if (direcao == 3) //baixo
                {
                    if (y > 1 && matriz[x, y - 1] == "0.0.0" && matriz[x, y - 2] == "0.0.0")
                    {
                        matriz[x, y] = "3.1.3";
                        matriz[x, y - 1] = "3.2.3";
                        matriz[x, y - 2] = "3.3.3";

                        if (x > 0)
                            matriz[x - 1, y] = "1.1.1";
                        if (y < 9)
                            matriz[x, y + 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y - 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y - 1] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y - 2] = "1.1.1";
                        if (y > 2)
                            matriz[x, y - 3] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y - 2] = "1.1.1";

                        if (naviosInseridos == null)
                            naviosInseridos = new String[10];

                        this.posicaoAtual = "" + x + Convert.ToString(y);
                        if (this.naviosInseridos[4] == null)
                        {
                            this.indicesInseridos = 4 + indicesInseridos;
                            this.naviosInseridos[4] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 4;
                        }
                        else
                        if (this.naviosInseridos[5] == null)
                        {
                            this.indicesInseridos = 5 + indicesInseridos;
                            this.naviosInseridos[5] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 5;
                        }
                        else
                        if (this.naviosInseridos[6] == null)
                        {
                            this.indicesInseridos = 6 + indicesInseridos;
                            this.naviosInseridos[6] = "" + x + Convert.ToString(y) + this.formadorDePosicoes();
                            this.ultimoIndexInserido = 6;
                        }

                        break;
                    }
                }
                direcao = RandomNumber(4);
                x = RandomNumber(10);
                y = RandomNumber(10);

            }
        }
        private void colocarDe4(String[,] matriz)
        {
            int direcao;
            int x;
            int y;
            direcao = RandomNumber(4);
            x = RandomNumber(10);
            y = RandomNumber(10);

            while (true)
            {
                while (matriz[x, y] != "0.0.0") //se for navio pega novas coordenadas
                {
                    x = RandomNumber(10);
                    y = RandomNumber(10);
                }
                if (direcao == 0) //esquerda
                {
                    if (x < 7 && matriz[x + 1, y] == "0.0.0" && matriz[x + 2, y] == "0.0.0" && matriz[x + 3, y] == "0.0.0")
                    {
                        matriz[x, y] = "4.1.0";
                        matriz[x + 1, y] = "4.2.0";
                        matriz[x + 2, y] = "4.3.0";
                        matriz[x + 3, y] = "4.4.0";

                        if (x > 0)
                            matriz[x - 1, y] = "1.1.1";
                        if (y > 0)
                            matriz[x, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x, y + 1] = "1.1.1";

                        if (y > 0)
                            matriz[x + 1, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x + 1, y + 1] = "1.1.1";

                        if (y > 0)
                            matriz[x + 2, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x + 2, y + 1] = "1.1.1";

                        if (y > 0)
                            matriz[x + 3, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x + 3, y + 1] = "1.1.1";
                        if (x < 6)
                            matriz[x + 4, y] = "1.1.1";

                        break;
                    }
                }
                else
            if (direcao == 1) //cima
                {
                    if (y < 7 && matriz[x, y + 1] == "0.0.0" && matriz[x, y + 2] == "0.0.0" && matriz[x, y + 3] == "0.0.0")
                    {
                        matriz[x, y] = "4.1.1";
                        matriz[x, y + 1] = "4.2.1";
                        matriz[x, y + 2] = "4.3.1";
                        matriz[x, y + 3] = "4.4.1";

                        if (x > 0)
                            matriz[x - 1, y] = "1.1.1";
                        if (y > 0)
                            matriz[x, y - 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y + 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y + 1] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y + 2] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y + 2] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y + 3] = "1.1.1";
                        if (y < 6)
                            matriz[x, y + 4] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y + 3] = "1.1.1";

                        break;
                    }
                }
                else
            if (direcao == 2) //esquerda
                {
                    if (x > 2 && matriz[x - 1, y] == "0.0.0" && matriz[x - 2, y] == "0.0.0" && matriz[x - 3, y] == "0.0.0")
                    {
                        matriz[x, y] = "4.1.2";
                        matriz[x - 1, y] = "4.2.2";
                        matriz[x - 2, y] = "4.3.2";
                        matriz[x - 3, y] = "4.4.2";

                        if (y > 0)
                            matriz[x, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x, y + 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y] = "1.1.1";

                        if (y > 0)
                            matriz[x - 1, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x - 1, y + 1] = "1.1.1";

                        if (y > 0)
                            matriz[x - 2, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x - 2, y + 1] = "1.1.1";

                        if (y > 0)
                            matriz[x - 3, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x - 3, y + 1] = "1.1.1";
                        if (x > 3)
                            matriz[x - 4, y] = "1.1.1";

                        break;
                    }
                }
                else
            if (direcao == 3) //baixo
                {
                    if (y > 2 && matriz[x, y - 1] == "0.0.0" && matriz[x, y - 2] == "0.0.0" && matriz[x, y - 3] == "0.0.0")
                    {
                        matriz[x, y] = "4.1.3";
                        matriz[x, y - 1] = "4.2.3";
                        matriz[x, y - 2] = "4.3.3";
                        matriz[x, y - 3] = "4.4.3";

                        if (x > 0)
                            matriz[x - 1, y] = "1.1.1";
                        if (y < 9)
                            matriz[x, y + 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y - 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y - 1] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y - 2] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y - 2] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y - 3] = "1.1.1";
                        if (y > 3)
                            matriz[x, y - 4] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y - 3] = "1.1.1";

                        break;
                    }
                }

                if (direcao < 3)
                    direcao++;
                else
                    direcao = 0;

                if (x < 9)
                    x++;
                else
                    x = 0;

                if (y < 9)
                    y++;
                else
                    y = 0;

            }

        }
        private void colocarDe5(String[,] matriz)
        {
            int direcao;
            int x;
            int y;

            direcao = RandomNumber(4);
            x = RandomNumber(10);
            y = RandomNumber(10);

            while (true)
            {
                while (matriz[x, y] != "0.0.0") //se for navio pega novas coordenadas
                {
                    x = RandomNumber(10);
                    y = RandomNumber(10);
                }

                if (direcao == 0) //esquerda
                {
                    if (x < 6 && matriz[x + 1, y] == "0.0.0" && matriz[x + 2, y] == "0.0.0" && matriz[x + 3, y] == "0.0.0" && matriz[x + 4, y] == "0.0.0")
                    {
                        matriz[x, y] = "5.1.0";
                        matriz[x + 1, y] = "5.2.0";
                        matriz[x + 2, y] = "5.3.0";
                        matriz[x + 3, y] = "5.4.0";
                        matriz[x + 4, y] = "5.5.0";

                        if (x > 0)
                            matriz[x - 1, y] = "1.1.1";
                        if (y > 0)
                            matriz[x, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x, y + 1] = "1.1.1";

                        if (y > 0)
                            matriz[x + 1, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x + 1, y + 1] = "1.1.1";

                        if (y > 0)
                            matriz[x + 2, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x + 2, y + 1] = "1.1.1";

                        if (y > 0)
                            matriz[x + 3, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x + 3, y + 1] = "1.1.1";

                        if (y > 0)
                            matriz[x + 4, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x + 4, y + 1] = "1.1.1";
                        if (x < 5)
                            matriz[x + 5, y] = "1.1.1";

                        break;
                    }
                }
                else
                if (direcao == 1) //cima
                {
                    if (y < 6 && matriz[x, y + 1] == "0.0.0" && matriz[x, y + 2] == "0.0.0" && matriz[x, y + 3] == "0.0.0" && matriz[x, y + 4] == "0.0.0")
                    {
                        matriz[x, y] = "5.1.1";
                        matriz[x, y + 1] = "5.2.1";
                        matriz[x, y + 2] = "5.3.1";
                        matriz[x, y + 3] = "5.4.1";
                        matriz[x, y + 4] = "5.5.1";

                        if (x > 0)
                            matriz[x - 1, y] = "1.1.1";
                        if (y > 0)
                            matriz[x, y - 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y + 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y + 1] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y + 2] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y + 2] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y + 3] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y + 3] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y + 4] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y + 4] = "1.1.1";
                        if (y < 5)
                            matriz[x, y + 5] = "1.1.1";

                        break;
                    }
                }
                else
                if (direcao == 2) //direita
                {
                    if (x > 3 && matriz[x - 1, y] == "0.0.0" && matriz[x - 2, y] == "0.0.0" && matriz[x - 3, y] == "0.0.0" && matriz[x - 4, y] == "0.0.0")
                    {
                        matriz[x, y] = "5.1.2";
                        matriz[x - 1, y] = "5.2.2";
                        matriz[x - 2, y] = "5.3.2";
                        matriz[x - 3, y] = "5.4.2";
                        matriz[x - 4, y] = "5.5.2";

                        if (y > 0)
                            matriz[x, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x, y + 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y] = "1.1.1";

                        if (y > 0)
                            matriz[x - 1, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x - 1, y + 1] = "1.1.1";

                        if (y > 0)
                            matriz[x - 2, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x - 2, y + 1] = "1.1.1";

                        if (y > 0)
                            matriz[x - 3, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x - 3, y + 1] = "1.1.1";

                        if (y > 0)
                            matriz[x - 4, y - 1] = "1.1.1";
                        if (y < 9)
                            matriz[x - 4, y + 1] = "1.1.1";
                        if (x > 4)
                            matriz[x - 5, y] = "1.1.1";

                        break;
                    }
                }
                else
                if (direcao == 3) //baixo
                {
                    if (y > 3 && matriz[x, y - 1] == "0.0.0" && matriz[x, y - 2] == "0.0.0" && matriz[x, y - 3] == "0.0.0" && matriz[x, y - 4] == "0.0.0")
                    {
                        matriz[x, y] = "5.1.3";
                        matriz[x, y - 1] = "5.2.3";
                        matriz[x, y - 2] = "5.3.3";
                        matriz[x, y - 3] = "5.4.3";
                        matriz[x, y - 4] = "5.5.3";

                        if (x > 0)
                            matriz[x - 1, y] = "1.1.1";
                        if (y < 9)
                            matriz[x, y + 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y - 1] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y - 1] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y - 2] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y - 2] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y - 3] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y - 3] = "1.1.1";

                        if (x > 0)
                            matriz[x - 1, y - 4] = "1.1.1";
                        if (y > 4)
                            matriz[x, y - 5] = "1.1.1";
                        if (x < 9)
                            matriz[x + 1, y - 4] = "1.1.1";

                        break;
                    }
                }

                direcao = RandomNumber(4);
                x = RandomNumber(10);
                y = RandomNumber(10);

            }
        }
        private void colocarAImagemDoNavioAfundado(DataGridView dtg, int tam, int direcao, int x, int y)
        {
            if (direcao == 2) //E
            {
                for (int i = 1; i <= tam; i++)
                {
                    Image img = Image.FromFile("../../../../imagens/navios/a/" + tam + "/" + tam + "-" + i + ".png");
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    dtg[x + 1, y].Value = img;
                    dtg.Refresh();
                    dtg[x + 1, y].Value = imlImagens.Images[15];

                    x--;
                }

            }
            else
            if (direcao == 3) //C
            {
                for (int i = 1; i <= tam; i++)
                {
                    Image img = Image.FromFile("../../../../imagens/navios/a/" + tam + "/" + tam + "-" + i + ".png");
                    img.RotateFlip(RotateFlipType.Rotate90FlipY);
                    dtg[x + 1, y].Value = img;
                    dtg.Refresh();
                    dtg[x + 1, y].Value = imlImagens.Images[15];
                    y--;
                }
            }
            else
            if (direcao == 0) //D
            {
                for (int i = 1; i <= tam; i++)
                {
                    Image img = Image.FromFile("../../../../imagens/navios/a/" + tam + "/" + tam + "-" + i + ".png");
                    dtg[x + 1, y].Value = img;
                    dtg.Refresh();
                    dtg[x + 1, y].Value = imlImagens.Images[15];
                    x++;
                }
            }
            else  //B
            {
                for (int i = 1; i <= tam; i++)
                {
                    Image img = Image.FromFile("../../../../imagens/navios/a/" + tam + "/" + tam + "-" + i + ".png");
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    dtg[x + 1, y].Value = img;
                    dtg.Refresh();
                    dtg[x + 1, y].Value = imlImagens.Images[15];
                    y++;

                }
            }
        }

        private bool derrubouNavio(String[,] mat, int x, int y)
        {
            if (x < 0 || x > 9 || y < 0 || y > 9)
                throw new Exception("Coordenadas inválidas !!");

            if (mat == null)
                throw new Exception("Matriz ausente !!");


            int direcao = Convert.ToInt16(mat[x, y].Substring(4, 1));

            int tam = Convert.ToInt16(mat[x, y].Substring(0, 1));

            int pos = Convert.ToInt16(mat[x, y].Substring(2, 1));

            if (tam > 5 || tam < 2)
                return false;

            String valor = mat[x, y];

            DataGridView dtgEmQuestao;

            if (mat.Equals(this.matriz))
                dtgEmQuestao = dtgCampo;
            else
                dtgEmQuestao = dtgAtaque;

            mat[x, y] = "6.6.6";

            if (direcao == 0) //esquerda
            {
                int inicio = x - pos + 1;
                for (int i = inicio; i < inicio + tam; i++)
                    if (Convert.ToInt16(mat[i, y].Substring(0, 1)) <= 5 && Convert.ToInt16(mat[i, y].Substring(0, 1)) >= 2)
                    {
                        mat[x, y] = valor;
                        return false;
                    }

                mat[x, y] = valor;
                if (!mat.Equals(this.matriz))
                    this.colocarAImagemDoNavioAfundado(dtgEmQuestao, tam, direcao, inicio, y);
                return true;
            }

            if (direcao == 1) //cima
            {
                int inicio = y - pos + 1;
                for (int i = inicio; i < inicio + tam; i++)
                    if (Convert.ToInt16(mat[x, i].Substring(0, 1)) <= 5 && Convert.ToInt16(mat[x, i].Substring(0, 1)) >= 2)
                    {
                        mat[x, y] = valor;
                        return false;
                    }

                mat[x, y] = valor;
                if (!mat.Equals(this.matriz))
                    this.colocarAImagemDoNavioAfundado(dtgEmQuestao, tam, direcao, x, inicio);
                return true;
            }

            if (direcao == 2) //direita
            {
                int inicio = x + pos - 1;
                for (int i = inicio; i > inicio - tam; i--)
                    if (Convert.ToInt16(mat[i, y].Substring(0, 1)) <= 5 && Convert.ToInt16(mat[i, y].Substring(0, 1)) >= 2)
                    {
                        mat[x, y] = valor;
                        return false;
                    }

                mat[x, y] = valor;
                if (!mat.Equals(this.matriz))
                    this.colocarAImagemDoNavioAfundado(dtgEmQuestao, tam, direcao, inicio, y);
                return true;
            }

            if (direcao == 3) //baixo
            {
                int inicio = y + pos - 1;
                for (int i = inicio; i > inicio - tam; i--)
                    if (Convert.ToInt16(mat[x, i].Substring(0, 1)) <= 5 && Convert.ToInt16(mat[x, i].Substring(0, 1)) >= 2)
                    {
                        mat[x, y] = valor;
                        return false;
                    }

                mat[x, y] = valor;
                if (!mat.Equals(this.matriz))
                    this.colocarAImagemDoNavioAfundado(dtgEmQuestao, tam, direcao, x, inicio);
                return true;
            }

            mat[x, y] = valor;
            return false;
        }

        public static int RandomNumber(int max)
        {
            lock (synclock)
            {
                return random.Next(max);
            }
        }
        // Matriz está completa de zeros.
        // Sobrescrever os zeros com numeros relativos ao tamanho dos navios.
        // E coloca uma virgula OU ponto para representar a direcao
        // Preencher apos completar com SUCESSSO o navio com o numero 1.
        // 010 2.1.1
        // 121
        // 121
        // 010

        private void PreencherDtg(DataGridView dtg, String pais)
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++) //Percorre todas as células
                {
                    if (dtg.RowCount < 10)
                        dtg.Rows.Add();
                    dtg.Rows[i].Cells[0].Value = "      " + Convert.ToChar(i + 65);
                }

            int num = 1;
            for (int j = 0; j < 10; j++)
                for (int i = 0; i < 10; i++)
                {
                    if (pais != "japao")
                    {
                        if (num < 10)
                            dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_00" + Convert.ToString(num) + ".jpg");
                        else
                                    if (num < 100)
                            dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_0" + Convert.ToString(num) + ".jpg");
                        else
                            dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_" + Convert.ToString(num) + ".jpg");
                    }
                    else
                    {
                        if (num < 10)
                            dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_00" + Convert.ToString(num) + ".png");
                        else
                                    if (num < 100)
                            dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_0" + Convert.ToString(num) + ".png");
                        else
                            dtg[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_" + Convert.ToString(num) + ".png");
                    }

                    num++;
                }

            dtg.RowHeadersDefaultCellStyle.Padding = new Padding(dtg.RowHeadersWidth);
        }
        private void frmSingle_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Height = 640;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.lblComp.BackColor = Color.FromArgb(120, Color.Red);
            this.lblJogador.BackColor = Color.FromArgb(120, Color.Red);

            this.PreencherDtg(dtgCampo, this.pais);
            this.PreencherDtg(dtgAtaque, this.paisAdv);

            this.paises[0] = "URSS";
            this.paises[1] = "ALEMANHA";
            this.paises[2] = "INGLATERRA";
            this.paises[3] = "JAPAO";
            this.paises[4] = "EUA";

            this.InicializaNavios(matriz);
            this.InicializaNavios(matrizAtaque);

            Application.DoEvents();
            this.Animacaozinha("Bem vindo marinheiro. As tropas inimigas estão chegando!");

            this.Animacaozinha("Boa sorte marinheiro, contamos com você!!");
        }
        private void dtgAtaque_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (sender.Equals(this.dtgCampo.Rows[0].Cells[0]))
                return;
            else
            {
            }
        }

        private void PrepararInsercao()
        {
            pbxDe2 = new PictureBox();
            pbxDe2.Image = Image.FromFile("../../../../imagens/navios/inteiros/2-1.png");
            pbxDe2.Location = new Point(dtgCampo.Location.X, dtgCampo.Location.Y + dtgCampo.Height + 5);
            pbxDe2.BackColor = Color.Transparent;
            pbxDe2.Width = 53;
            pbxDe2.Height = 27;
            pbxDe2.DoubleClick += new EventHandler(ClickPbxDe2);

            pbxDe3 = new PictureBox();
            pbxDe3.Image = Image.FromFile("../../../../imagens/navios/inteiros/3-1.png");
            pbxDe3.Location = new Point(dtgCampo.Location.X, pbxDe2.Location.Y + pbxDe2.Height + 2);
            pbxDe3.BackColor = Color.Transparent;
            pbxDe3.Width = 81;
            pbxDe3.Height = 27;
            pbxDe3.DoubleClick += new EventHandler(ClickPbxDe3);

            pbxDe4 = new PictureBox();
            pbxDe4.Image = Image.FromFile("../../../../imagens/navios/inteiros/4-1.png");
            pbxDe4.Location = new Point(dtgCampo.Location.X, pbxDe3.Location.Y + pbxDe3.Height + 2);
            pbxDe4.BackColor = Color.Transparent;
            pbxDe4.Width = 109;
            pbxDe4.Height = 27;
            pbxDe4.DoubleClick += new EventHandler(ClickPbxDe4);

            pbxDe5 = new PictureBox();
            pbxDe5.Image = Image.FromFile("../../../../imagens/navios/inteiros/5-1.png");
            pbxDe5.Location = new Point(dtgCampo.Location.X, pbxDe4.Location.Y + pbxDe4.Height + 2);
            pbxDe5.BackColor = Color.Transparent;
            pbxDe5.Width = 135;
            pbxDe5.Height = 27;
            pbxDe5.DoubleClick += new EventHandler(ClickPbxDe5);

            this.labelDe2 = new Label();
            this.labelDe2.Name = "labelDe2";
            this.labelDe2.Location = new Point(dtgCampo.Location.X + pbxDe2.Width + 80, dtgCampo.Location.Y + dtgCampo.Height + 5);
            this.labelDe2.BackColor = Color.FromArgb(0, Color.Red);
            this.labelDe2.ForeColor = Color.White;
            this.labelDe2.Text = "(4x)";
            this.labelDe2.Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            this.labelDe3 = new Label();
            this.labelDe3.Name = "labelDe3";
            this.labelDe3.Location = new Point(labelDe2.Location.X, labelDe2.Location.Y + labelDe2.Height + 5);
            this.labelDe3.BackColor = Color.FromArgb(0, Color.Red);
            this.labelDe3.ForeColor = Color.White;
            this.labelDe3.Text = "(3x)";
            this.labelDe3.Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            this.labelDe4 = new Label();
            this.labelDe4.Name = "labelDe4";
            this.labelDe4.Location = new Point(labelDe2.Location.X, labelDe3.Location.Y + labelDe3.Height + 5);
            this.labelDe4.BackColor = Color.FromArgb(0, Color.Red);
            this.labelDe4.Text = "(2x)";
            this.labelDe4.ForeColor = Color.White;
            this.labelDe4.Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            this.labelDe5 = new Label();
            this.labelDe5.Name = "labelDe5";
            this.labelDe5.Location = new Point(labelDe2.Location.X, labelDe4.Location.Y + labelDe4.Height + 5);
            this.labelDe5.BackColor = Color.FromArgb(0, Color.Red);
            this.labelDe5.Text = "(1x)";
            this.labelDe5.ForeColor = Color.White;
            this.labelDe5.Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            this.Controls.Add(pbxDe2);
            this.Controls.Add(pbxDe3);
            this.Controls.Add(pbxDe4);
            this.Controls.Add(pbxDe5);
            this.Controls.Add(this.labelDe2);
            this.Controls.Add(this.labelDe3);
            this.Controls.Add(this.labelDe4);
            this.Controls.Add(this.labelDe5);

            btnIniciar.Enabled = true;    
        }

        private void ClickPbxDe2(object sender, EventArgs e)
        {
            if (this.estaInserindoNoMomento == false && this.qtdNaviosDe2Inseridos < 4)
                this.ColocarONavioNaTela(2);
        }

        private void ClickPbxDe3(object sender, EventArgs e)
        {
            if (this.estaInserindoNoMomento == false && this.qtdNaviosDe3Inseridos < 3)
                this.ColocarONavioNaTela(3);
        }

        private void ClickPbxDe4(object sender, EventArgs e)
        {
            if (this.estaInserindoNoMomento == false && this.qtdNaviosDe4Inseridos < 2)
                this.ColocarONavioNaTela(4);
        }

        private void ClickPbxDe5(object sender, EventArgs e)
        {
            if (this.estaInserindoNoMomento == false && !this.inseriuODe5)
                this.ColocarONavioNaTela(5);
        }

        private void CompletarFrota()
        {
            if (estaInserindoNoMomento == false && estaInserindo)
            {
                while (this.qtdNaviosDe2Inseridos < 4)
                {
                    this.colocarDe2(this.matriz);
                    this.qtdNaviosDe2Inseridos++;
                }

                while (this.qtdNaviosDe3Inseridos < 3)
                {
                    this.colocarDe3(this.matriz);
                    this.qtdNaviosDe3Inseridos++;
                }

                while (this.qtdNaviosDe4Inseridos < 2)
                {
                    if (!this.temEspaco(this.matriz, 4))
                    {
                        this.AjustarLabel("Não há espaço para o Encouraçado!");
                        break;
                    }
                    else
                    {
                        this.qtdNaviosDe4Inseridos++;
                    }
                }

                if (!inseriuODe5)
                {
                    if (!this.temEspaco(matriz, 5))
                    {
                        this.AjustarLabel("Não há espaço para o Porta-Aviões!");
                    }
                    else
                    {
                        this.inseriuODe5 = true;
                    }
                }
                this.posicaoAtual = null;
            }
            else
            if (estaInserindo)
                this.AjustarLabel("Insira o navio atual primeiro !");

            if (this.inseriuTodos())
            {
                this.btnIniciar.Visible = true;
            }
            else
            {
                this.estaInserindo = true;
                this.btnIniciar.Visible = false;
            }

            this.refreshDtg();
            this.AtualizarLabels();
        }

        private void colocarOPrimeiroNavio()
        {
            this.estaInserindoNoMomento = true;
            this.matriz[4, 4] = "2.1.1";
            this.matriz[4, 5] = "2.2.1";
            this.refreshDtg();
            this.posicaoAtual = "44";
            this.tamanmhoUlt = 2;
            this.PreencherAoRedorNavio(this.matriz, 4, 4);
        }

        private String ZeroOuUm(int x, int y)
        {
            //aqui vou verificar se determinada posicao deve ser 0 ou 1.

            if (x > 0)
            {
                if (this.verPosicao(this.matriz, x - 1, y) == 1)
                    return "1.1.1";
            }

            if (x < 9)
            {
                if (this.verPosicao(this.matriz, x + 1, y) == 1)
                    return "1.1.1";
            }

            if (y > 0)
            {
                if (this.verPosicao(this.matriz, x, y - 1) == 1)
                    return "1.1.1";
            }

            if (y < 9)
            {
                if (this.verPosicao(this.matriz, x, y + 1) == 1)
                    return "1.1.1";
            }

            return "0.0.0";
        }

        private void GirarONavio(int x, int y)
        {
            {
                // Nessa funcao vou fazer o seguinte:
                //com o x e y tenho tam e a direcao. Tendo a direcao, vou girar p o proximo
                //se direcao==0, giro p cima. Se direcao==1, giro p direita
                //ou seja, vai ficar direcao+1.
                //se direcao==3, direcao sera 0

                //trocando o value da direcao n a matriz o DTG se vira e ja poe o navio certin
                //porém, tenho q verificar se n vai PASSAR DA BORDAS ao girar

                // Se for passar, tenho 2 options : stop or puxarONavioPTraz p ele n ficar p fora mais
                //Sinceramente, acho melhor a opcao 1.

                //Como eu teria as 2 pontas de opcao p ser o centro de rotacao, escolherei a ponta INICIO

                //ou seja, vou rodar a partir do 1.

                //Terei o cntrole de onde devem ir as novs posicoes a partir de Pos2, x ou y
                //Sendo Pos2 a posicao da parte do navio a ser trocada nqle instante

                //direcao 0 --> y+P2-1
                //direcao 1 --> x+1-P2
                //direcao 2 --> y+1-P2
                //direcao 3 --> x+P2-1

                //com um for, que deverá ser controlado pelo TAM, percorro as posicoes e as mudo, e ja tiro os 1's

                //Mas preciso iniciar do INICIO, entao, primeiro tenho que achá-lo

                //Nao esquecer de fazer as verificacoes

                //Na tese, a nova pos receberia a antiga, e eu setaria a nova direcao manualmente, ja que eh direcao+1
                //a posicao antiga deve ser zerada e preeenchida com 1.1.1, 0.0.0, depende, ai vai dar trabalho

                //Eu nao posso deixar girar se estiver em algum,as pontas. Pelo menos o inicio n pode ser o EIXO.
                //Quando ele estiver em pontas que nao deveria girar, vou fazer o ULTIMO ser o eixo de rotacao
            }

            int direcao = Convert.ToInt16(this.matriz[x, y].Substring(4, 1));

            int tam = Convert.ToInt16(this.matriz[x, y].Substring(0, 1));

            int pos = Convert.ToInt16(this.matriz[x, y].Substring(2, 1));

            if (tam > 5 || tam < 2)
                return;

            if (direcao == 0) //esquerda
            {
                int inicio = x - pos + 1;
                String valor = "";
                int Pos2 = Convert.ToInt16(this.matriz[inicio, y].Substring(2, 1)); // ACHO pos2

                if (y + tam - 1 > 9) //Ultimo deve ser o eixo de rotacao
                {
                    int ultimo = inicio + tam - 1;

                    if (y - tam >= 0 && this.ehNavio(this.matriz[ultimo, y - tam])) //checando se tem navios ao redor
                        return;

                    for (int i = y; i > y - tam + 1; i--)
                    {
                        if (i > 0 && (this.ehNavio(this.matriz[ultimo, i - 1])))
                            return;

                        if (ultimo > 0 && i > 0 && (this.ehNavio(this.matriz[ultimo - 1, i - 1])))
                            return;

                        if (ultimo < 9 && i > 0 && (this.ehNavio(this.matriz[ultimo + 1, i - 1])))
                            return;
                    }

                    Pos2 = Convert.ToInt16(this.matriz[ultimo, y].Substring(2, 1));
                    for (int i = ultimo; i > ultimo - tam; i--)
                    {
                        if (i != ultimo)
                            this.matriz[ultimo, y - tam + Pos2] = valor;
                        else
                            this.matriz[ultimo, y - tam + Pos2] = this.matriz[i, y].Substring(0, 4) + "1"; //coloco a pos na nova posicao

                        if (i > 0)
                        {
                            valor = this.matriz[i - 1, y];
                            if (valor.Substring(0, 1) != "1" && valor.Substring(0, 1) != "0")
                                valor = this.matriz[i - 1, y].Substring(0, 4) + "1"; //MUDO A DIRECAO so se n for agua

                            Pos2 = Convert.ToInt16(this.matriz[i - 1, y].Substring(2, 1));
                            this.matriz[i - 1, y] = this.ZeroOuUm(i - 1, y); //seto p zero ou um p sumir o navio dali

                            if (i != ultimo - tam + 1)
                            {
                                if (y > 0)
                                    this.matriz[i - 1, y - 1] = this.ZeroOuUm(i - 1, y - 1);

                                if (y < 9)
                                    this.matriz[i - 1, y + 1] = this.ZeroOuUm(i - 1, y + 1);
                            }
                        }

                    }
                    x = ultimo;
                    y = y - tam + 1;
                    this.posicaoAtual = "" + x + y;
                }
                else
                {
                    if (y + tam < 10 && this.ehNavio(this.matriz[inicio, y + tam])) //checando se tem navios ao redor
                        return;

                    for (int i = y; i < y + tam - 1; i++)
                    {
                        if (i > 0 && (this.ehNavio(this.matriz[inicio, i + 1])))
                            return;

                        if (inicio > 0 && i < 9 && (this.ehNavio(this.matriz[inicio - 1, i + 1])))
                            return;

                        if (inicio < 9 && i < 9 && (this.ehNavio(this.matriz[inicio + 1, i + 1])))
                            return;
                    }

                    for (int i = inicio; i < inicio + tam; i++)
                    {
                        if (i != inicio)
                            this.matriz[inicio, y + Pos2 - 1] = valor;
                        else
                            this.matriz[inicio, y + Pos2 - 1] = this.matriz[i, y].Substring(0, 4) + "1"; //coloco a pos na nova posicao

                        if (i < 9)
                        {
                            valor = this.matriz[i + 1, y];
                            if (valor.Substring(0, 1) != "1" && valor.Substring(0, 1) != "0")
                                valor = this.matriz[i + 1, y].Substring(0, 4) + "1";

                            Pos2 = Convert.ToInt16(this.matriz[i + 1, y].Substring(2, 1));
                            this.matriz[i + 1, y] = this.ZeroOuUm(i + 1, y); //seto p zero ou um p sumir o navio dali

                            if (i != inicio + tam - 1)
                            {
                                if (y > 0)
                                    this.matriz[i + 1, y - 1] = this.ZeroOuUm(i + 1, y - 1);

                                if (y < 9)
                                    this.matriz[i + 1, y + 1] = this.ZeroOuUm(i + 1, y + 1);
                            }
                        }
                    }
                }
            }

            if (direcao == 1) //cima
            {
                int inicio = y - pos + 1;
                String valor = "";
                int Pos2 = Convert.ToInt16(this.matriz[x, inicio].Substring(2, 1));

                if (x - tam + 1 < 0)
                {
                    int ultimo = inicio + tam - 1;

                    if (x + tam <= 9 && this.ehNavio(this.matriz[x + tam, ultimo])) //checando se tem navios ao redor
                        return;

                    for (int i = x; i < x + tam - 1; i++)
                    {
                        if (i > 0 && (this.ehNavio(this.matriz[i + 1, ultimo])))
                            return;

                        if (ultimo > 0 && i < 9 && (this.ehNavio(this.matriz[i + 1, ultimo - 1])))
                            return;

                        if (ultimo < 9 && i < 9 && (this.ehNavio(this.matriz[i + 1, ultimo + 1])))
                            return;
                    }

                    Pos2 = Convert.ToInt16(this.matriz[x, ultimo].Substring(2, 1));
                    for (int i = ultimo; i > ultimo - tam; i--)
                    {
                        if (i != ultimo)
                            this.matriz[x + tam - Pos2, ultimo] = valor;
                        else
                            this.matriz[x + tam - Pos2, ultimo] = this.matriz[x, i].Substring(0, 4) + "2"; //coloco a pos na nova posicao

                        if (i > 0)
                        {
                            valor = this.matriz[x, i - 1];
                            if (valor.Substring(0, 1) != "1" && valor.Substring(0, 1) != "0")
                                valor = this.matriz[x, i - 1].Substring(0, 4) + "2"; //MUDO A DIRECAO so se n for agua

                            Pos2 = Convert.ToInt16(this.matriz[x, i - 1].Substring(2, 1));
                            this.matriz[x, i - 1] = this.ZeroOuUm(x, i - 1); //seto p zero ou um p sumir o navio dali

                            if (i != ultimo - tam + 1)
                            {
                                if (x > 0)
                                    this.matriz[x - 1, i - 1] = this.ZeroOuUm(x - 1, i - 1);

                                if (x < 9)
                                    this.matriz[x + 1, i - 1] = this.ZeroOuUm(x + 1, i - 1);
                            }
                        }

                    }
                    y = ultimo;
                    x = x + tam - 1;
                    this.posicaoAtual = "" + x + y;

                }
                else
                {
                    if (x - tam >= 0 && this.ehNavio(this.matriz[x - tam, inicio])) //checando se tem navios ao redor
                        return;

                    for (int i = x; i > x - tam + 1; i--)
                    {
                        if (i > 0 && (this.ehNavio(this.matriz[i - 1, inicio])))
                            return;

                        if (inicio > 0 && i > 0 && (this.ehNavio(this.matriz[i - 1, inicio - 1])))
                            return;

                        if (inicio < 9 && i > 0 && (this.ehNavio(this.matriz[i - 1, inicio + 1])))
                            return;
                    }

                    for (int i = inicio; i < inicio + tam; i++)
                    {
                        if (i != inicio)
                            this.matriz[x + 1 - Pos2, inicio] = valor;
                        else
                            this.matriz[x + 1 - Pos2, inicio] = this.matriz[x, i].Substring(0, 4) + "2"; //coloco a pos na nova posicao

                        if (i < 9)
                        {
                            valor = this.matriz[x, i + 1];
                            if (valor.Substring(0, 1) != "1" && valor.Substring(0, 1) != "0")
                                valor = this.matriz[x, i + 1].Substring(0, 4) + "2";

                            Pos2 = Convert.ToInt16(this.matriz[x, i + 1].Substring(2, 1)); // Salvo Pos2 antes de mudar o valor na matriz
                            this.matriz[x, i + 1] = this.ZeroOuUm(x, i + 1); //seto p zero ou um p sumir o navio dali

                            if (i != inicio + tam - 1)
                            {
                                if (x > 0)
                                    this.matriz[x - 1, i + 1] = this.ZeroOuUm(x - 1, i + 1);

                                if (x < 9)
                                    this.matriz[x + 1, i + 1] = this.ZeroOuUm(x + 1, i + 1);
                            }
                        }
                    }
                }
            }

            if (direcao == 2) //direita
            {
                int inicio = x + pos - 1;
                String valor = "";
                int Pos2 = Convert.ToInt16(this.matriz[inicio, y].Substring(2, 1)); // ACHO pos2

                if (y - tam + 1 < 0)
                {
                    int ultimo = inicio - tam + 1;

                    if (y - tam >= 0 && this.ehNavio(this.matriz[inicio, y - tam])) //checando se tem navios ao redor
                        return;

                    for (int i = y; i < y + tam; i++)
                    {
                        if (i < 9 && (this.ehNavio(this.matriz[ultimo, i + 1])))
                            return;

                        if (ultimo > 0 && i < 9 && (this.ehNavio(this.matriz[ultimo - 1, i + 1])))
                            return;

                        if (ultimo < 9 && i < 9 && (this.ehNavio(this.matriz[ultimo + 1, i + 1])))
                            return;
                    }

                    Pos2 = Convert.ToInt16(this.matriz[ultimo, y].Substring(2, 1));
                    for (int i = ultimo; i < ultimo + tam; i++)
                    {
                        if (i != ultimo)
                            this.matriz[ultimo, y + tam - Pos2] = valor;
                        else
                            this.matriz[ultimo, y + tam - Pos2] = this.matriz[i, y].Substring(0, 4) + "3"; //coloco a pos na nova posicao

                        if (i < 9)
                        {
                            valor = this.matriz[i + 1, y];
                            if (valor.Substring(0, 1) != "1" && valor.Substring(0, 1) != "0")
                                valor = this.matriz[i + 1, y].Substring(0, 4) + "3"; //MUDO A DIRECAO so se n for agua

                            Pos2 = Convert.ToInt16(this.matriz[i + 1, y].Substring(2, 1));
                            this.matriz[i + 1, y] = this.ZeroOuUm(i + 1, y); //seto p zero ou um p sumir o navio dali

                            if (i != ultimo + tam - 1)
                            {
                                if (y > 0)
                                    this.matriz[i + 1, y - 1] = this.ZeroOuUm(i + 1, y - 1);

                                if (y < 9)
                                    this.matriz[i + 1, y + 1] = this.ZeroOuUm(i + 1, y + 1);
                            }
                        }

                    }
                    x = ultimo;
                    y = y + tam - 1;
                    this.posicaoAtual = "" + x + y;

                }
                else
                {
                    if (y - tam >= 0 && this.ehNavio(this.matriz[inicio, y - tam])) //checando se tem navios ao redor
                        return;

                    for (int i = y; i > y - tam + 1; i--)
                    {
                        if (i > 1 && (this.ehNavio(this.matriz[inicio, i - 1])))
                            return;

                        if (inicio > 0 && i > 0 && (this.ehNavio(this.matriz[inicio - 1, i - 1])))
                            return;

                        if (inicio < 9 && i > 0 && (this.ehNavio(this.matriz[inicio + 1, i - 1])))
                            return;
                    }

                    for (int i = inicio; i > inicio - tam; i--)
                    {
                        if (i != inicio)
                            this.matriz[inicio, y + 1 - Pos2] = valor;
                        else
                            this.matriz[inicio, y + 1 - Pos2] = this.matriz[i, y].Substring(0, 4) + "3"; //coloco a pos na nova posicao

                        if (i > 0)
                        {
                            valor = this.matriz[i - 1, y];
                            if (valor.Substring(0, 1) != "1" && valor.Substring(0, 1) != "0")
                                valor = this.matriz[i - 1, y].Substring(0, 4) + "3";

                            Pos2 = Convert.ToInt16(this.matriz[i - 1, y].Substring(2, 1)); // ACHO pos2
                            this.matriz[i - 1, y] = this.ZeroOuUm(i - 1, y); //seto p zero ou um p sumir o navio dali

                            if (i != inicio - tam + 1)
                            {
                                if (y > 0)
                                    this.matriz[i - 1, y - 1] = this.ZeroOuUm(i - 1, y - 1);

                                if (y < 9)
                                    this.matriz[i - 1, y + 1] = this.ZeroOuUm(i - 1, y + 1);
                            }
                        }
                    }
                }
            }

            if (direcao == 3) //baixo
            {
                int inicio = y + pos - 1;
                String valor = "";
                int Pos2 = Convert.ToInt16(this.matriz[x, inicio].Substring(2, 1)); // ACHO pos2

                if (x + tam - 1 > 9)
                {
                    int ultimo = inicio - tam + 1;

                    if (x - tam >= 0 && this.ehNavio(this.matriz[x - tam, ultimo])) //checando se tem navios ao redor
                        return;

                    for (int i = x; i > x - tam + 1; i--)
                    {
                        if (i > 0 && (this.ehNavio(this.matriz[i - 1, ultimo])))
                            return;

                        if (ultimo > 0 && i > 0 && (this.ehNavio(this.matriz[i - 1, ultimo - 1])))
                            return;

                        if (ultimo < 9 && i > 0 && (this.ehNavio(this.matriz[i - 1, ultimo + 1])))
                            return;
                    }

                    Pos2 = Convert.ToInt16(this.matriz[x, ultimo].Substring(2, 1));
                    for (int i = ultimo; i < ultimo + tam; i++)
                    {
                        if (i != ultimo)
                            this.matriz[x - tam + Pos2, ultimo] = valor;
                        else
                            this.matriz[x - tam + Pos2, ultimo] = this.matriz[x, i].Substring(0, 4) + "0"; //coloco a pos na nova posicao

                        if (i < 9)
                        {
                            valor = this.matriz[x, i + 1];
                            if (valor.Substring(0, 1) != "1" && valor.Substring(0, 1) != "0")
                                valor = this.matriz[x, i + 1].Substring(0, 4) + "0"; //MUDO A DIRECAO so se n for agua

                            Pos2 = Convert.ToInt16(this.matriz[x, i + 1].Substring(2, 1));
                            this.matriz[x, i + 1] = this.ZeroOuUm(x, i + 1); //seto p zero ou um p sumir o navio dali

                            if (i != ultimo + tam - 1)
                            {
                                if (x > 0)
                                    this.matriz[x - 1, i + 1] = this.ZeroOuUm(x - 1, i + 1);

                                if (x < 9)
                                    this.matriz[x + 1, i + 1] = this.ZeroOuUm(x + 1, i + 1);
                            }
                        }

                    }
                    y = ultimo;
                    x = x - tam + 1;
                    this.posicaoAtual = "" + x + y;

                }
                else
                {
                    if (x + tam <= 9 && this.ehNavio(this.matriz[x + tam, inicio])) //checando se tem navios ao redor
                        return;

                    for (int i = x; i < x + tam - 1; i++)
                    {
                        if (i > 1 && (this.ehNavio(this.matriz[i + 1, inicio])))
                            return;

                        if (inicio > 0 && i < 9 && (this.ehNavio(this.matriz[i + 1, inicio - 1])))
                            return;

                        if (inicio < 9 && i < 9 && (this.ehNavio(this.matriz[i + 1, inicio + 1])))
                            return;
                    }

                    for (int i = inicio; i > inicio - tam; i--)
                    {
                        if (i != inicio)
                            this.matriz[x + Pos2 - 1, inicio] = valor;
                        else
                            this.matriz[x + Pos2 - 1, inicio] = this.matriz[x, i].Substring(0, 4) + "0"; //coloco a pos na nova posicao

                        if (i > 0)
                        {
                            valor = this.matriz[x, i - 1];
                            if (valor.Substring(0, 1) != "1" && valor.Substring(0, 1) != "0")
                                valor = this.matriz[x, i - 1].Substring(0, 4) + "0";

                            Pos2 = Convert.ToInt16(this.matriz[x, i - 1].Substring(2, 1)); // ACHO pos2
                            this.matriz[x, i - 1] = this.ZeroOuUm(x, i - 1); //seto p zero ou um p sumir o navio dali

                            if (i != inicio - tam + 1)
                            {
                                if (x > 0)
                                    this.matriz[x - 1, i - 1] = this.ZeroOuUm(x - 1, i - 1);

                                if (x < 9)
                                    this.matriz[x + 1, i - 1] = this.ZeroOuUm(x + 1, i - 1);
                            }
                        }
                    }
                }
            }

            this.PreencherAoRedorNavio(this.matriz, x, y);
        }

        private bool ehNavio(String s)
        {
            if (s.Equals("0.0.0") || s.Equals("1.1.1") || s.Equals("7.7.7"))
                return false;

            return true;
        }

        private void PreencherAoRedorNavio(String[,] mat, int x, int y)
        {
            if (x < 0 || x > 9 || y > 9 || y < 0)
                return;

            int direcao = Convert.ToInt16(this.matriz[x, y].Substring(4, 1));

            int tam = Convert.ToInt16(this.matriz[x, y].Substring(0, 1));

            int pos = Convert.ToInt16(this.matriz[x, y].Substring(2, 1));

            if (tam > 5 || tam < 2)
                return;

            if (direcao == 0) //esquerda
            {
                int inicio = x - pos + 1;

                if (inicio > 0)
                    mat[inicio - 1, y] = "1.1.1";


                if (inicio + tam < 10)
                    mat[inicio + tam, y] = "1.1.1";


                for (int i = inicio; i < inicio + tam; i++)
                {
                    if (y > 0)
                        mat[i, y - 1] = "1.1.1";


                    if (y < 9)
                        mat[i, y + 1] = "1.1.1";

                }

            }

            if (direcao == 1) //cima
            {
                int inicio = y - pos + 1;

                if (inicio > 0)
                    mat[x, inicio - 1] = "1.1.1";


                if (inicio + tam < 10)
                    mat[x, inicio + tam] = "1.1.1";


                for (int i = inicio; i < inicio + tam; i++)
                {
                    if (x > 0)
                        mat[x - 1, i] = "1.1.1";


                    if (x < 9)
                        mat[x + 1, i] = "1.1.1";

                }
            }

            if (direcao == 2) //direita
            {
                int inicio = x + pos - 1;

                if (inicio < 9)
                    mat[inicio + 1, y] = "1.1.1";


                if (inicio - tam >= 0)
                    mat[inicio - tam, y] = "1.1.1";


                for (int i = inicio; i > inicio - tam; i--)
                {
                    if (y > 0)
                        mat[i, y - 1] = "1.1.1";


                    if (y < 9)
                        mat[i, y + 1] = "1.1.1";

                }
            }

            if (direcao == 3) //baixo
            {
                int inicio = y + pos - 1;

                if (inicio < 9)
                    mat[x, inicio + 1] = "1.1.1";


                if (inicio - tam >= 0)
                    mat[x, inicio - tam] = "1.1.1";


                for (int i = inicio; i > inicio - tam; i--)
                {
                    if (x > 0)
                        mat[x - 1, i] = "1.1.1";


                    if (x < 9)
                        mat[x + 1, i] = "1.1.1";

                }
            }
        }

        private void ConfirmarNavio()
        {
            {//Este metodo devera confirmar o navio na posicao desejada, guarda-lo num vetor dos naviosinseridos
             //e colocar o proximo na tela

                //Para isso, devera fazer diversas verificacoes, tais como se o navio sobrepõe algum outro ou se esta numa
                //posicao invalida(1.1.1)

                //nunca uma posicaoAtual eh igual à outra do vetor naviosInseridos, usar isso para as checagens

                //Checar se alguma posicaoAtual eh igual à outra no vetor. Se for, exibir mensagem de erro

                //FAZER FUNCAO ehAdjacente(String posicaoUm, posicaoDois) p saber se a pos1 eh adjacente à pos2

                //Se for adjacente tambem n pode inserir

                //Caso todas as verificacoes deixem, guardar no vetor a nova pos, alem dos pedacos do navio e
                //poe o prox navio no meio da tela

                //Usando as variaveis globais qtdNaviosDe2, qtdNaviosDe3 e qtdNaviosDe4, sei ql navio colocar no meio
                //Usando o tam, decremento essas variaveis neste metodo, e no Backspace, incremento-as

                //O problema mesmo sera ao verificar qdo colocar o navio2 do lado de outro navio ja inserido, pois
                //se eu setar as posicoes p 1 ou p o proprio navio, o navio antigo sera perdido

                //Fazer algo antes do RecarregarDtg sumir com o navio!!!

                //Solucoes:

                //Declarar nova matriz de string

                //Carrega a antiga na nova matriz

                //Qdo for inserir o segundo navio, comparar as 2 matrizes

                //if ((matriz1[x,y] != matriz2[x, y]) && this.verposicao(matriz1, x, y)==0)
                //  if (this.verPosicao(matriz2, x, y)==1)
                //      matriz1[x,y] = matriz2[x,y]
                //  else //Nao eh um navio na segunda
                //manter a primeira, n inclui a segunda!! //E SE FOR 1.1.1??? Eu deveria colocar....

                //esta solucao tem erros, mas eh por aqui. Ir aperfeicoando

                //Tambem tenho q impedir ele de mover o barco anterior

                //MEXER NA VARIAVEL tamanhoUlt TBM (ja mexo na funcao colocarNavio(int tam))

                //outra maneira eh deixar a posicao da matriz dupla : 2.1.13.1.1

                //mas antes de se sobescreverem ja apaga...

                //n vai ter jeito. Tenho q mexer no andarNavio

                //Talvez se no processCmdkey ehu n deixo recarregarDtg antes de checar as 2 matrizes.
                //Se for 1 eu n substituo
                //mas e as posicoes que ele seta p agua direto????? Seriam outro problema

                //aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
            }

            int x = Convert.ToInt16(this.posicaoAtual.Substring(0, 1));
            int y = Convert.ToInt16(this.posicaoAtual.Substring(1, 1));

            int tam = Convert.ToInt16(this.matriz[x, y].Substring(0, 1));

            this.estaInserindoNoMomento = false;

            if (tam == 2) //Estou inserindo um navio de 2
            {
                if (this.naviosInseridos == null)
                    this.naviosInseridos = new String[10];

                this.naviosInseridos[this.qtdNaviosDe2Inseridos] = this.posicaoAtual + this.formadorDePosicoes();
                this.qtdNaviosDe2Inseridos++;

                this.posicaoAtual = null;
                this.ultimoIndexInserido = this.qtdNaviosDe2Inseridos - 1;
                this.indicesInseridos = ultimoIndexInserido + this.indicesInseridos;
            }
            else
            if (tam == 3) //Estou inserindo um navio de 3
            {
                if (this.naviosInseridos == null)
                    this.naviosInseridos = new String[10];

                this.naviosInseridos[this.qtdNaviosDe3Inseridos + 4] = this.posicaoAtual + this.formadorDePosicoes();
                this.qtdNaviosDe3Inseridos++;

                this.posicaoAtual = null;
                this.ultimoIndexInserido = 4 + this.qtdNaviosDe3Inseridos - 1;
                this.indicesInseridos = ultimoIndexInserido + this.indicesInseridos;
            }
            else
            if (tam == 4) //Estou inserindo um navio de 4
            {
                if (this.naviosInseridos == null)
                    this.naviosInseridos = new String[10];

                this.naviosInseridos[this.qtdNaviosDe4Inseridos + 7] = this.posicaoAtual + this.formadorDePosicoes();
                this.qtdNaviosDe4Inseridos++;

                this.posicaoAtual = null;
                this.ultimoIndexInserido = 7 + this.qtdNaviosDe4Inseridos - 1;
                this.indicesInseridos = ultimoIndexInserido + this.indicesInseridos;
            }
            else
            if (tam == 5)//Estou inserindo um navio de 5
            {
                if (this.naviosInseridos == null)
                    this.naviosInseridos = new String[10];

                this.naviosInseridos[9] = this.posicaoAtual + this.formadorDePosicoes();
                this.ultimoIndexInserido = 9;
                this.posicaoAtual = null;
                this.inseriuODe5 = true;
                this.indicesInseridos = ultimoIndexInserido + this.indicesInseridos;
            }

            if (this.inseriuTodos())
            {
                this.btnIniciar.Visible = true;
                this.btnDeCima.Enabled = false;
            }
            else
            {
                this.estaInserindo = true;
                this.btnIniciar.Visible = false;
            }

            this.AtualizarLabels();
        } //DON

        private void ApagarNavio(int x, int y)
        {
            int direcao = Convert.ToInt16(this.matriz[x, y].Substring(4, 1));

            int tam = Convert.ToInt16(this.matriz[x, y].Substring(0, 1));

            int pos = Convert.ToInt16(this.matriz[x, y].Substring(2, 1));

            if (tam > 5 || tam < 2)
                return;

            if (direcao == 0) //esquerda
            {
                int inicio = x - pos + 1;

                if (inicio > 0)
                    if (inicio == 1)
                    {
                        if ((y > 0 && !ehNavio(matriz[inicio - 1, y - 1])) && (y < 9 && !ehNavio(this.matriz[inicio - 1, y + 1])))
                            matriz[inicio - 1, y] = "0.0.0";
                        else
                        if (y == 0)
                        {
                            if (!ehNavio(this.matriz[inicio - 1, y + 1]))
                                matriz[inicio - 1, y] = "0.0.0";
                        }
                        else
                        if (y == 9)
                        {
                            if (!ehNavio(matriz[inicio - 1, y - 1]))
                                matriz[inicio - 1, y] = "0.0.0";
                        }
                    }
                    else
                    {
                        if (!ehNavio(this.matriz[inicio - 2, y]) && (y > 0 && !ehNavio(this.matriz[inicio - 1, y - 1])) && (y < 9 && !ehNavio(this.matriz[inicio - 1, y + 1])))
                            matriz[inicio - 1, y] = "0.0.0";
                        else
                        if (y == 0)
                        {
                            if (!ehNavio(this.matriz[inicio - 1, y + 1]) && !ehNavio(this.matriz[inicio - 2, y]))
                                matriz[inicio - 1, y] = "0.0.0";
                        }
                        else
                        if (y == 9)
                        {
                            if (!ehNavio(this.matriz[inicio - 1, y - 1]) && !ehNavio(this.matriz[inicio - 2, y]))
                                matriz[inicio - 1, y] = "0.0.0";
                        }
                    }

                if (inicio + tam < 9)
                    if (inicio + tam == 8)
                    {
                        if ((y > 0 && !ehNavio(matriz[inicio + tam, y - 1])) && (y < 9 && !ehNavio(this.matriz[inicio + tam, y + 1])))
                            matriz[inicio + tam, y] = "0.0.0";
                        else
                        if (y == 0)
                        {
                            if (!ehNavio(this.matriz[inicio + tam, y + 1]))
                                matriz[inicio + tam, y] = "0.0.0";
                        }
                        else
                        if (y == 9)
                        {
                            if (!ehNavio(matriz[inicio + tam, y - 1]))
                                matriz[inicio + tam, y] = "0.0.0";
                        }
                    }
                    else
                    {
                        if (!ehNavio(this.matriz[inicio + tam + 1, y]) && (y > 0 && !ehNavio(this.matriz[inicio + tam, y - 1])) && (y < 9 && !ehNavio(this.matriz[inicio + tam, y + 1])))
                            matriz[inicio + tam, y] = "0.0.0";
                        else
                        if (y == 0)
                        {
                            if (!ehNavio(this.matriz[inicio + tam, y + 1]) && !ehNavio(this.matriz[inicio + tam + 1, y]))
                                matriz[inicio + tam, y] = "0.0.0";
                        }
                        else
                        if (y == 9)
                        {
                            if (!ehNavio(this.matriz[inicio + tam, y - 1]) && !ehNavio(this.matriz[inicio + tam + 1, y]))
                                matriz[inicio + tam, y] = "0.0.0";
                        }
                    }


                for (int i = inicio; i < inicio + tam; i++)
                {
                    if (y > 0)
                        if (y == 1)
                        {
                            if ((i > 0 && !ehNavio(matriz[i - 1, y - 1])) && (i < 9 && !ehNavio(matriz[i + 1, y - 1])))
                                matriz[i, y - 1] = "0.0.0";
                            else
                            if (i == 0)
                            {
                                if (!ehNavio(matriz[i + 1, y - 1]))
                                    matriz[i, y - 1] = "0.0.0";
                            }
                            else
                            if (i == 9)
                            {
                                if (!ehNavio(matriz[i - 1, y - 1]))
                                    matriz[i, y - 1] = "0.0.0";
                            }

                        }
                        else
                            if (!ehNavio(this.matriz[i, y - 2]) && (i > 0 && !ehNavio(matriz[i - 1, y - 1])) && (i < 9 && !ehNavio(matriz[i + 1, y - 1])))
                            matriz[i, y - 1] = "0.0.0";
                        else
                            if (i == 0)
                        {
                            if (!ehNavio(matriz[i + 1, y - 1]) && !ehNavio(this.matriz[i, y - 2]))
                                matriz[i, y - 1] = "0.0.0";
                        }
                        else
                            if (i == 9)
                        {
                            if (!ehNavio(matriz[i - 1, y - 1]) && !ehNavio(this.matriz[i, y - 2]))
                                matriz[i, y - 1] = "0.0.0";
                        }

                    matriz[i, y] = "0.0.0";

                    if (y < 9)
                        if (y == 8)
                        {
                            if ((i > 0 && !ehNavio(matriz[i - 1, y + 1])) && (i < 9 && !ehNavio(matriz[i + 1, y + 1])))
                                matriz[i, y + 1] = "0.0.0";
                            else
                            if (i == 0)
                            {
                                if (!ehNavio(matriz[i + 1, y + 1]))
                                    matriz[i, y + 1] = "0.0.0";
                            }
                            else
                            if (i == 9)
                            {
                                if (!ehNavio(matriz[i - 1, y + 1]))
                                    matriz[i, y + 1] = "0.0.0";
                            }
                        }
                        else
                        {
                            if (!ehNavio(this.matriz[i, y + 2]) && (i > 0 && !ehNavio(matriz[i - 1, y + 1])) && (i < 9 && !ehNavio(matriz[i + 1, y + 1])))
                                matriz[i, y + 1] = "0.0.0";
                            else
                            if (i == 0)
                            {
                                if (!ehNavio(matriz[i + 1, y + 1]) && !ehNavio(this.matriz[i, y + 1 + 1]))
                                    matriz[i, y + 1] = "0.0.0";
                            }
                            else
                            if (i == 9)
                            {
                                if (!ehNavio(matriz[i - 1, y + 1]) && !ehNavio(this.matriz[i, y + 1 + 1]))
                                    matriz[i, y + 1] = "0.0.0";
                            }
                        }

                }

            }

            if (direcao == 1) //cima
            {
                int inicio = y - pos + 1;

                if (inicio > 0)
                    if (inicio == 1)
                    {
                        if ((x > 0 && !ehNavio(matriz[x - 1, inicio - 1])) && (x < 9 && !ehNavio(matriz[x + 1, inicio - 1])))
                            matriz[x, inicio - 1] = "0.0.0";
                        else
                        if (x == 0)
                        {
                            if (!ehNavio(matriz[x + 1, inicio - 1]))
                                matriz[x, inicio - 1] = "0.0.0";
                        }
                        else
                        if (x == 9)
                        {
                            if (!ehNavio(matriz[x - 1, inicio - 1]))
                                matriz[x, inicio - 1] = "0.0.0";
                        }

                    }
                    else
                        if (!ehNavio(this.matriz[x, inicio - 2]) && (x > 0 && !ehNavio(matriz[x - 1, inicio - 1])) && (x < 9 && !ehNavio(matriz[x + 1, inicio - 1])))
                        matriz[x, inicio - 1] = "0.0.0";
                    else
                        if (x == 0)
                    {
                        if (!ehNavio(matriz[x + 1, inicio - 1]) && !ehNavio(this.matriz[x, inicio - 2]))
                            matriz[x, inicio - 1] = "0.0.0";
                    }
                    else
                        if (x == 9)
                    {
                        if (!ehNavio(matriz[x - 1, inicio - 1]) && !ehNavio(this.matriz[x, inicio - 2]))
                            matriz[x, inicio - 1] = "0.0.0";
                    }


                if (inicio + tam < 9)
                    if (inicio + tam == 8)
                    {
                        if ((x > 0 && !ehNavio(matriz[x - 1, inicio + tam])) && (x < 9 && !ehNavio(matriz[x + 1, inicio + tam])))
                            matriz[x, inicio + tam] = "0.0.0";
                        else
                        if (x == 0)
                        {
                            if (!ehNavio(matriz[x + 1, inicio + tam]))
                                matriz[x, inicio + tam] = "0.0.0";
                        }
                        else
                        if (x == 9)
                        {
                            if (!ehNavio(matriz[x - 1, inicio + tam]))
                                matriz[x, inicio + tam] = "0.0.0";
                        }
                    }
                    else
                    {
                        if (!ehNavio(this.matriz[x, inicio + tam + 1]) && (x > 0 && !ehNavio(matriz[x - 1, inicio + tam])) && (x < 9 && !ehNavio(matriz[x + 1, inicio + tam])))
                            matriz[x, inicio + tam] = "0.0.0";
                        else
                        if (x == 0)
                        {
                            if (!ehNavio(matriz[x + 1, inicio + tam]) && !ehNavio(this.matriz[x, inicio + tam + 1]))
                                matriz[x, inicio + tam] = "0.0.0";
                        }
                        else
                        if (x == 9)
                        {
                            if (!ehNavio(matriz[x - 1, inicio + tam]) && !ehNavio(this.matriz[x, inicio + tam + 1]))
                                matriz[x, inicio + tam] = "0.0.0";
                        }
                    }


                for (int i = inicio; i < inicio + tam; i++)
                {
                    if (x > 0)
                        if (x == 1)
                        {
                            if ((i > 0 && !ehNavio(matriz[x - 1, i - 1])) && (i < 9 && !ehNavio(this.matriz[x - 1, i + 1])))
                                matriz[x - 1, i] = "0.0.0";
                            else
                            if (i == 0)
                            {
                                if (!ehNavio(this.matriz[x - 1, i + 1]))
                                    matriz[x - 1, i] = "0.0.0";
                            }
                            else
                            if (i == 9)
                            {
                                if (!ehNavio(matriz[x - 1, i - 1]))
                                    matriz[x - 1, i] = "0.0.0";
                            }
                        }
                        else
                        {
                            if (!ehNavio(this.matriz[x - 2, i]) && (i > 0 && !ehNavio(this.matriz[x - 1, i - 1])) && (i < 9 && !ehNavio(this.matriz[x - 1, i + 1])))
                                matriz[x - 1, i] = "0.0.0";
                            else
                            if (i == 0)
                            {
                                if (!ehNavio(this.matriz[x - 1, i + 1]) && !ehNavio(this.matriz[x - 2, i]))
                                    matriz[x - 1, i] = "0.0.0";
                            }
                            else
                            if (i == 9)
                            {
                                if (!ehNavio(this.matriz[x - 1, i - 1]) && !ehNavio(this.matriz[x - 2, i]))
                                    matriz[x - 1, i] = "0.0.0";
                            }
                        }


                    matriz[x, i] = "0.0.0";

                    if (x < 9)
                        if (x == 8)
                        {
                            if ((i > 0 && !ehNavio(matriz[x + 1, i - 1])) && (i < 9 && !ehNavio(this.matriz[x + 1, i + 1])))
                                matriz[x + 1, i] = "0.0.0";
                            else
                            if (i == 0)
                            {
                                if (!ehNavio(this.matriz[x + 1, i + 1]))
                                    matriz[x + 1, i] = "0.0.0";
                            }
                            else
                            if (i == 9)
                            {
                                if (!ehNavio(matriz[x + 1, i - 1]))
                                    matriz[x + 1, i] = "0.0.0";
                            }
                        }
                        else
                        {
                            if (!ehNavio(this.matriz[x + 2, i]) && (i > 0 && !ehNavio(this.matriz[x + 1, i - 1])) && (i < 9 && !ehNavio(this.matriz[x + 1, i + 1])))
                                matriz[x + 1, i] = "0.0.0";
                            else
                            if (i == 0)
                            {
                                if (!ehNavio(this.matriz[x + 1, i + 1]) && !ehNavio(this.matriz[x + 2, i]))
                                    matriz[x + 1, i] = "0.0.0";
                            }
                            else
                            if (i == 9)
                            {
                                if (!ehNavio(this.matriz[x + 1, i - 1]) && !ehNavio(this.matriz[x + 2, i]))
                                    matriz[x + 1, i] = "0.0.0";
                            }
                        }

                }
            }

            if (direcao == 2) //direita
            {
                int inicio = x + pos - 1;

                if (inicio < 9)
                    if (inicio == 8)
                    {
                        if ((y > 0 && !ehNavio(matriz[inicio + 1, y - 1])) && (y < 9 && !ehNavio(this.matriz[inicio + 1, y + 1])))
                            matriz[inicio + 1, y] = "0.0.0";
                        else
                        if (y == 0)
                        {
                            if (!ehNavio(this.matriz[inicio + 1, y + 1]))
                                matriz[inicio + 1, y] = "0.0.0";
                        }
                        else
                        if (y == 9)
                        {
                            if (!ehNavio(matriz[inicio + 1, y - 1]))
                                matriz[inicio + 1, y] = "0.0.0";
                        }
                    }
                    else
                    {
                        if (!ehNavio(this.matriz[inicio + 2, y]) && (y > 0 && !ehNavio(this.matriz[inicio + 1, y - 1])) && (y < 9 && !ehNavio(this.matriz[inicio + 1, y + 1])))
                            matriz[inicio + 1, y] = "0.0.0";
                        else
                        if (y == 0)
                        {
                            if (!ehNavio(this.matriz[inicio + 1, y + 1]) && !ehNavio(this.matriz[inicio + 2, y]))
                                matriz[inicio + 1, y] = "0.0.0";
                        }
                        else
                        if (y == 9)
                        {
                            if (!ehNavio(this.matriz[inicio + 1, y - 1]) && !ehNavio(this.matriz[inicio + 2, y]))
                                matriz[inicio + 1, y] = "0.0.0";
                        }
                    }

                if (inicio - tam > 0)
                    if (inicio - tam == 1)
                    {
                        if ((y > 0 && !ehNavio(matriz[inicio - tam, y - 1])) && (y < 9 && !ehNavio(this.matriz[inicio - tam, y + 1])))
                            matriz[inicio - tam, y] = "0.0.0";
                        else
                        if (y == 0)
                        {
                            if (!ehNavio(this.matriz[inicio - tam, y + 1]))
                                matriz[inicio - tam, y] = "0.0.0";
                        }
                        else
                        if (y == 9)
                        {
                            if (!ehNavio(matriz[inicio - tam, y - 1]))
                                matriz[inicio - tam, y] = "0.0.0";
                        }
                    }
                    else
                    {
                        if (!ehNavio(this.matriz[inicio - tam - 1, y]) && (y > 0 && !ehNavio(this.matriz[inicio - tam, y - 1])) && (y < 9 && !ehNavio(this.matriz[inicio - tam, y + 1])))
                            matriz[inicio - tam, y] = "0.0.0";
                        else
                        if (y == 0)
                        {
                            if (!ehNavio(this.matriz[inicio - tam, y + 1]) && !ehNavio(this.matriz[inicio - tam - 1, y]))
                                matriz[inicio - tam, y] = "0.0.0";
                        }
                        else
                        if (y == 9)
                        {
                            if (!ehNavio(this.matriz[inicio - tam, y - 1]) && !ehNavio(this.matriz[inicio - tam - 1, y]))
                                matriz[inicio - tam, y] = "0.0.0";
                        }
                    }


                for (int i = inicio; i > inicio - tam; i--)
                {
                    if (y > 0)
                        if (y == 1)
                        {
                            if ((i > 0 && !ehNavio(matriz[i - 1, y - 1])) && (i < 9 && !ehNavio(matriz[i + 1, y - 1])))
                                matriz[i, y - 1] = "0.0.0";
                            else
                            if (i == 0)
                            {
                                if (!ehNavio(matriz[i + 1, y - 1]))
                                    matriz[i, y - 1] = "0.0.0";
                            }
                            else
                            if (i == 9)
                            {
                                if (!ehNavio(matriz[i - 1, y - 1]))
                                    matriz[i, y - 1] = "0.0.0";
                            }

                        }
                        else
                            if (!ehNavio(this.matriz[i, y - 2]) && (i > 0 && !ehNavio(matriz[i - 1, y - 1])) && (i < 9 && !ehNavio(matriz[i + 1, y - 1])))
                            matriz[i, y - 1] = "0.0.0";
                        else
                            if (i == 0)
                        {
                            if (!ehNavio(matriz[i + 1, y - 1]) && !ehNavio(this.matriz[i, y - 2]))
                                matriz[i, y - 1] = "0.0.0";
                        }
                        else
                            if (i == 9)
                        {
                            if (!ehNavio(matriz[i - 1, y - 1]) && !ehNavio(this.matriz[i, y - 2]))
                                matriz[i, y - 1] = "0.0.0";
                        }

                    matriz[i, y] = "0.0.0";

                    if (y < 9)
                        if (y == 8)
                        {
                            if ((i > 0 && !ehNavio(matriz[i - 1, y + 1])) && (i < 9 && !ehNavio(matriz[i + 1, y + 1])))
                                matriz[i, y + 1] = "0.0.0";
                            else
                            if (i == 0)
                            {
                                if (!ehNavio(matriz[i + 1, y + 1]))
                                    matriz[i, y + 1] = "0.0.0";
                            }
                            else
                            if (i == 9)
                            {
                                if (!ehNavio(matriz[i - 1, y + 1]))
                                    matriz[i, y + 1] = "0.0.0";
                            }
                        }
                        else
                        {
                            if (!ehNavio(this.matriz[i, y + 1 + 1]) && (i > 0 && !ehNavio(matriz[i - 1, y + 1])) && (i < 9 && !ehNavio(matriz[i + 1, y + 1])))
                                matriz[i, y + 1] = "0.0.0";
                            else
                            if (i == 0)
                            {
                                if (!ehNavio(matriz[i + 1, y + 1]) && !ehNavio(this.matriz[i, y + 1 + 1]))
                                    matriz[i, y + 1] = "0.0.0";
                            }
                            else
                            if (i == 9)
                            {
                                if (!ehNavio(matriz[i - 1, y + 1]) && !ehNavio(this.matriz[i, y + 1 + 1]))
                                    matriz[i, y + 1] = "0.0.0";
                            }
                        }

                }
            }

            if (direcao == 3) //baixo
            {
                int inicio = y + pos - 1;

                if (inicio < 9)
                    if (inicio == 8)
                    {
                        if ((x > 0 && !ehNavio(matriz[x - 1, inicio + 1])) && (x < 9 && !ehNavio(matriz[x + 1, inicio + 1])))
                            matriz[x, inicio + 1] = "0.0.0";
                        else
                        if (x == 0)
                        {
                            if (!ehNavio(matriz[x + 1, inicio + 1]))
                                matriz[x, inicio + 1] = "0.0.0";
                        }
                        else
                        if (x == 9)
                        {
                            if (!ehNavio(matriz[x - 1, inicio + 1]))
                                matriz[x, inicio + 1] = "0.0.0";
                        }

                    }
                    else
                        if (!ehNavio(this.matriz[x, inicio + 2]) && (x > 0 && !ehNavio(matriz[x - 1, inicio + 1])) && (x < 9 && !ehNavio(matriz[x + 1, inicio + 1])))
                        matriz[x, inicio + 1] = "0.0.0";
                    else
                    if (x == 0)
                    {
                        if (!ehNavio(matriz[x + 1, inicio + 1]) && !ehNavio(this.matriz[x, inicio + 2]))
                            matriz[x, inicio + 1] = "0.0.0";
                    }
                    else
                    if (x == 9)
                    {
                        if (!ehNavio(matriz[x - 1, inicio + 1]) && !ehNavio(this.matriz[x, inicio + 2]))
                            matriz[x, inicio + 1] = "0.0.0";
                    }


                if (inicio - tam > 0)
                    if (inicio - tam == 1)
                    {
                        if ((x > 0 && !ehNavio(matriz[x - 1, inicio - tam])) && (x < 9 && !ehNavio(matriz[x + 1, inicio - tam])))
                            matriz[x, inicio - tam] = "0.0.0";
                        else
                        if (x == 0)
                        {
                            if (!ehNavio(matriz[x + 1, inicio - tam]))
                                matriz[x, inicio - tam] = "0.0.0";
                        }
                        else
                        if (x == 9)
                        {
                            if (!ehNavio(matriz[x - 1, inicio - tam]))
                                matriz[x, inicio - tam] = "0.0.0";
                        }
                    }
                    else
                    {
                        if (!ehNavio(this.matriz[x, inicio - tam - 1]) && (x > 0 && !ehNavio(matriz[x - 1, inicio - tam])) && (x < 9 && !ehNavio(matriz[x + 1, inicio - tam])))
                            matriz[x, inicio - tam] = "0.0.0";
                        else
                        if (x == 0)
                        {
                            if (!ehNavio(matriz[x + 1, inicio - tam]) && !ehNavio(this.matriz[x, inicio - tam - 1]))
                                matriz[x, inicio - tam] = "0.0.0";
                        }
                        else
                        if (x == 9)
                        {
                            if (!ehNavio(matriz[x - 1, inicio - tam]) && !ehNavio(this.matriz[x, inicio - tam - 1]))
                                matriz[x, inicio - tam] = "0.0.0";
                        }
                    }


                for (int i = inicio; i > inicio - tam; i--)
                {
                    if (x > 0)
                        if (x == 1)
                        {
                            if ((i > 0 && !ehNavio(matriz[x - 1, i - 1])) && (i < 9 && !ehNavio(this.matriz[x - 1, i + 1])))
                                matriz[x - 1, i] = "0.0.0";
                            else
                            if (i == 0)
                            {
                                if (!ehNavio(this.matriz[x - 1, i + 1]))
                                    matriz[x - 1, i] = "0.0.0";
                            }
                            else
                            if (i == 9)
                            {
                                if (!ehNavio(matriz[x - 1, i - 1]))
                                    matriz[x - 1, i] = "0.0.0";
                            }
                        }
                        else
                        {
                            if (!ehNavio(this.matriz[x - 2, i]) && (i > 0 && !ehNavio(this.matriz[x - 1, i - 1])) && (i < 9 && !ehNavio(this.matriz[x - 1, i + 1])))
                                matriz[x - 1, i] = "0.0.0";
                            else
                            if (i == 0)
                            {
                                if (!ehNavio(this.matriz[x - 1, i + 1]) && !ehNavio(this.matriz[x - 2, i]))
                                    matriz[x - 1, i] = "0.0.0";
                            }
                            else
                            if (i == 9)
                            {
                                if (!ehNavio(this.matriz[x - 1, i - 1]) && !ehNavio(this.matriz[x - 2, i]))
                                    matriz[x - 1, i] = "0.0.0";
                            }
                        }


                    matriz[x, i] = "0.0.0";

                    if (x < 9)
                        if (x == 8)
                        {
                            if ((i > 0 && !ehNavio(matriz[x + 1, i - 1])) && (i < 9 && !ehNavio(this.matriz[x + 1, i + 1])))
                                matriz[x + 1, i] = "0.0.0";
                            else
                            if (i == 0)
                            {
                                if (!ehNavio(this.matriz[x + 1, i + 1]))
                                    matriz[x + 1, i] = "0.0.0";
                            }
                            else
                            if (i == 9)
                            {
                                if (!ehNavio(matriz[x + 1, i - 1]))
                                    matriz[x + 1, i] = "0.0.0";
                            }
                        }
                        else
                        {
                            if (!ehNavio(this.matriz[x + 2, i]) && (i > 0 && !ehNavio(this.matriz[x + 1, i - 1])) && (i < 9 && !ehNavio(this.matriz[x + 1, i + 1])))
                                matriz[x + 1, i] = "0.0.0";
                            else
                            if (i == 0)
                            {
                                if (!ehNavio(this.matriz[x + 1, i + 1]) && !ehNavio(this.matriz[x + 2, i]))
                                    matriz[x + 1, i] = "0.0.0";
                            }
                            else
                            if (i == 9)
                            {
                                if (!ehNavio(this.matriz[x + 1, i - 1]) && !ehNavio(this.matriz[x + 2, i]))
                                    matriz[x + 1, i] = "0.0.0";
                            }
                        }

                }
            }

        } //FINALLY DONE!!

        private void Backspace()
        {
            bool posAtualEraNull = false;
            if (posicaoAtual == null) //Navio Parado OU NAO TEM NENHUM NA TELA
            {
                if (naviosInseridos == null) //NAO TEM NENHUM NA TELA
                    return;

                if (naviosInseridos[ultimoIndexInserido] == null)
                    return;
                this.posicaoAtual = this.naviosInseridos[ultimoIndexInserido].Substring(0, 2);
                this.naviosInseridos[ultimoIndexInserido] = null;
                this.estaInserindoNoMomento = true;

                posAtualEraNull = true;

                if (ultimoIndexInserido < 4 && qtdNaviosDe2Inseridos > 0)
                {
                    qtdNaviosDe2Inseridos--;
                    this.tamanmhoUlt = 2;
                }
                else
                if (ultimoIndexInserido < 7 && qtdNaviosDe3Inseridos > 0)
                {
                    qtdNaviosDe3Inseridos--;
                    this.tamanmhoUlt = 3;
                }
                else
                if (ultimoIndexInserido < 9 && qtdNaviosDe4Inseridos > 0)
                {
                    qtdNaviosDe4Inseridos--;
                    this.tamanmhoUlt = 4;
                }
                else
                {
                    inseriuODe5 = false;
                    this.tamanmhoUlt = 5;
                }
            }
            else
            {
                if (this.tamanmhoUlt == 2)
                {
                    this.ApagarNavio(Convert.ToInt16(this.posicaoAtual.Substring(0, 1)), Convert.ToInt16(this.posicaoAtual.Substring(1, 1)));

                    posicaoAtual = null;


                    this.estaInserindoNoMomento = false;
                }
                else
                if (this.tamanmhoUlt == 3)
                {
                    this.ApagarNavio(Convert.ToInt16(this.posicaoAtual.Substring(0, 1)), Convert.ToInt16(this.posicaoAtual.Substring(1, 1)));

                    posicaoAtual = null;

                    this.estaInserindoNoMomento = false;
                }
                else
                if (this.tamanmhoUlt == 4)
                {
                    this.ApagarNavio(Convert.ToInt16(this.posicaoAtual.Substring(0, 1)), Convert.ToInt16(this.posicaoAtual.Substring(1, 1)));

                    posicaoAtual = null;

                    this.estaInserindoNoMomento = false;
                }
                else
                if (this.tamanmhoUlt == 5)
                {
                    this.ApagarNavio(Convert.ToInt16(this.posicaoAtual.Substring(0, 1)), Convert.ToInt16(this.posicaoAtual.Substring(1, 1)));

                    posicaoAtual = null;


                    this.estaInserindoNoMomento = false;

                }
            }

            this.AtualizarLabels();

            btnIniciar.Visible = false;
            btnDeCima.Enabled = true;

            if (indicesInseridos.Length > 0 && posAtualEraNull)
                this.indicesInseridos = indicesInseridos.Substring(1, indicesInseridos.Length - 1);

            if (posAtualEraNull && indicesInseridos.Length > 0) //ou seja, se o navio NAO estava inserido, ou seja, eu o apaguei mudo o indexUlt p o navio anterior
                this.ultimoIndexInserido = Convert.ToInt16(indicesInseridos.Substring(0, 1));

        } //FINALLY DONE!!

        private void refreshDtg()
        {
            int num = 1;
            for (int j = 0; j < 10; j++)
                for (int i = 0; i < 10; i++)
                {
                    if (pais != "japao")
                    {
                        if (num < 10)
                            dtgCampo[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_00" + Convert.ToString(num) + ".jpg");
                        else
                                    if (num < 100)
                            dtgCampo[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_0" + Convert.ToString(num) + ".jpg");
                        else
                            dtgCampo[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_" + Convert.ToString(num) + ".jpg");

                    }
                    else
                    {
                        if (num < 10)
                            dtgCampo[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_00" + Convert.ToString(num) + ".png");
                        else
                                    if (num < 100)
                            dtgCampo[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_0" + Convert.ToString(num) + ".png");
                        else
                            dtgCampo[i + 1, j].Value = Image.FromFile("../../../../imagens/" + pais + "/imgs/imgs/image_part_" + Convert.ToString(num) + ".png");

                    }
                    num++;
                }

            int direcao = -1;
            //dtgCampo.Refresh();

            for (int i = 0; i < 10; i++) //muda a coluna
                for (int j = 0; j < 10; j++) //linha a linha
                {
                    string valorImagem = this.matriz[i, j].ToString().Substring(0, 1);
                    string qualImagem = this.matriz[i, j].ToString().Substring(2, 1);

                    if (this.matriz[i, j].ToString().Equals("1.1.1") || this.matriz[i, j].ToString().Equals("0.0.0"))
                    {
                        continue;
                    }

                    if (!(this.matriz[i, j].Equals("0.0.0")))
                        direcao = Convert.ToInt16(this.matriz[i, j].ToString().Substring(4, 1));

                    Image img = imlImagens.Images[imlImagens.Images.Keys.IndexOf(this.matriz[i, j].ToString().Substring(0, 3) + ".png")];

                    if (direcao == 2) //direita
                        img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    else
                    if (direcao == 1) //cima
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    else //JA ESTAO POR PADRAO À ESQUERDA
                    if (direcao == 3) //baixo
                        img.RotateFlip(RotateFlipType.Rotate90FlipY);

                    dtgCampo[i + 1, j].Value = img;
                }
        }

        private void movimentarONavio(int direcao, String[] posicoes, int x, int y)
        {
            {
                //x reprresenta o VALOR FINAL DO NAVIO, ou seja, a posicao adjacente
                // à qual ele irá

                //y é a mesma coisa

                //POSICOES guarda os navios, asSTRINGS de cada pedaco do navio
                //a ideia eh a partir de posicoes, mudar os valores na matriz

                //DAR UM JEITO DE SABER SE TEM NAVIO SOBESCRITO, onde está esse navio

                // ESCREVER O 1.1.1 e TIRAR AS POSICOES DO NAVIO ANTIGO (INCLUSIVE OS 1's)
            }
            int tam = Convert.ToInt16(this.matriz[x, y].Substring(0, 1));

            int pos = Convert.ToInt16(this.matriz[x, y].Substring(2, 1));

            int dirDoNavio = Convert.ToInt16(this.matriz[x, y].Substring(4, 1)); //precisarei para saber onde TIRAR os 1.1.1

            if (direcao == 0 && dirDoNavio % 2 == 0) //esquerda
            {
                int inicio = 0;

                if (dirDoNavio == 0)
                    inicio = x - pos + 1;
                else
                    inicio = x + pos - 1;

                if (inicio == 0)
                    return;

                int indice = 0;

                if (dirDoNavio == direcao)
                {
                    if (inicio > 1 && this.ehNavio(this.matriz[inicio - 2, y]))
                        return;

                    if (y > 0 && this.ehNavio(this.matriz[inicio - 1, y - 1]))
                        return;

                    if (y < 9 && this.ehNavio(this.matriz[inicio - 1, y + 1]))
                        return;

                    for (int i = inicio; i < inicio + tam; i++)
                    {
                        this.matriz[i - 1, y] = posicoes[indice];
                        indice++;
                    }
                }
                else
                {
                    indice = tam;
                    int ultimo = inicio - tam + 1;
                    if (ultimo == 0)
                        return;

                    if (ultimo > 1 && this.ehNavio(this.matriz[ultimo - 2, y]))
                        return;

                    if (y > 0 && this.ehNavio(this.matriz[ultimo - 1, y - 1]))
                        return;

                    if (y < 9 && this.ehNavio(this.matriz[ultimo - 1, y + 1]))
                        return;

                    for (int i = ultimo; i < ultimo + tam; i++)
                    {
                        this.matriz[i - 1, y] = posicoes[indice - 1];
                        indice--;
                    }
                }

                if (dirDoNavio == direcao)
                {
                    int a = inicio + tam - 1;

                    if (a < 9)
                        this.matriz[a + 1, y] = this.ZeroOuUm(a + 1, y);

                    if (y > 0)
                        this.matriz[a, y - 1] = this.ZeroOuUm(a, y - 1);

                    if (y < 9)
                        this.matriz[a, y + 1] = this.ZeroOuUm(a, y + 1);
                }
                else
                {
                    if (inicio < 9)
                        this.matriz[inicio + 1, y] = this.ZeroOuUm(inicio + 1, y);

                    if (y > 0)
                        this.matriz[inicio, y - 1] = this.ZeroOuUm(inicio, y - 1);

                    if (y < 9)
                        this.matriz[inicio, y + 1] = this.ZeroOuUm(inicio, y + 1);
                }

                x--;
            }
            else
            if (direcao == 0 && dirDoNavio % 2 == 1)
            {
                if (x == 0)
                    return;

                int inicio = 0;

                if (dirDoNavio == 1)
                    inicio = y - pos + 1;
                else
                    inicio = y + pos - 1;

                if (dirDoNavio == 1)
                {
                    if (inicio > 0)
                        if (ehNavio(matriz[x - 1, inicio - 1]))
                            return;

                    if (inicio + tam <= 9)
                        if (ehNavio(matriz[x - 1, inicio + tam]))
                            return;
                }
                else
                {
                    if (inicio < 9)
                        if (ehNavio(matriz[x - 1, inicio + 1]))
                            return;

                    if (inicio - tam >= 0)
                        if (ehNavio(matriz[x - 1, inicio - tam]))
                            return;
                }

                if (x > 1)
                    if (dirDoNavio == 1)
                    {
                        for (int i = inicio; i < inicio + tam; i++)
                            if (ehNavio(matriz[x - 2, i]))
                                return;
                    }
                    else
                        for (int i = inicio; i > inicio - tam; i--)
                            if (ehNavio(matriz[x - 2, i]))
                                return;

                if (dirDoNavio == 1)
                    for (int i = inicio; i < inicio + tam; i++)
                    {
                        matriz[x, i] = "0.0.0"; //zero primeiro para o zeroOuUm dar certo
                    }
                else
                    for (int i = inicio; i > inicio - tam; i--)
                    {
                        matriz[x, i] = "0.0.0"; //zero primeiro para o zeroOuUm dar certo
                    }

                if (dirDoNavio == 1)
                {
                    if (inicio > 0)
                        matriz[x, inicio - 1] = this.ZeroOuUm(x, inicio - 1);

                    if (inicio + tam <= 9)
                        matriz[x, inicio + tam] = this.ZeroOuUm(x, inicio + tam);
                }
                else
                {
                    if (inicio < 9)
                        matriz[x, inicio + 1] = this.ZeroOuUm(x, inicio + 1);

                    if (inicio - tam >= 0)
                        matriz[x, inicio - tam] = this.ZeroOuUm(x, inicio - tam);
                }

                int index = 0;

                if (dirDoNavio == 1)
                    for (int i = inicio; i < inicio + tam; i++)
                    {
                        matriz[x - 1, i] = posicoes[i - inicio]; //zero primeiro para o zeroOuUm dar certo
                        if (x < 9)
                            matriz[x + 1, i] = this.ZeroOuUm(x + 1, i); //zero primeiro para o zeroOuUm dar certo
                    }
                else
                    for (int i = inicio; i > inicio - tam; i--)
                    {
                        matriz[x - 1, i] = posicoes[index]; //zero primeiro para o zeroOuUm dar certo
                        if (x < 9)
                            matriz[x + 1, i] = this.ZeroOuUm(x + 1, i); //zero primeiro para o zeroOuUm dar certo
                        index++;
                    }
                x--;
            }

            if (direcao == 1 && dirDoNavio % 2 == 1) //cima
            {
                int inicio = 0;

                if (dirDoNavio == 1)
                    inicio = y - pos + 1;
                else
                    inicio = y + pos - 1;

                if (inicio == 0)
                    return;

                int indice = 0;

                if (dirDoNavio == direcao)
                {
                    if (inicio > 1 && this.ehNavio(this.matriz[x, inicio - 2]))
                        return;

                    if (x > 0 && this.ehNavio(this.matriz[x - 1, inicio - 1]))
                        return;

                    if (x < 9 && this.ehNavio(this.matriz[x + 1, inicio - 1]))
                        return;

                    for (int i = inicio; i < inicio + tam; i++)
                    {
                        this.matriz[x, i - 1] = posicoes[indice];
                        indice++;
                    }
                }
                else
                {
                    indice = tam;
                    int ultimo = inicio - tam + 1;
                    if (ultimo == 0)
                        return;

                    if (ultimo > 1 && this.ehNavio(this.matriz[x, ultimo - 2]))
                        return;

                    if (x > 0 && this.ehNavio(this.matriz[x - 1, ultimo - 1]))
                        return;

                    if (x < 9 && this.ehNavio(this.matriz[x + 1, ultimo - 1]))
                        return;

                    for (int i = ultimo; i < ultimo + tam; i++)
                    {
                        this.matriz[x, i - 1] = posicoes[indice - 1];
                        indice--;
                    }
                }

                if (dirDoNavio == direcao)
                {
                    int a = inicio + tam - 1;

                    if (a < 9)
                        this.matriz[x, a + 1] = this.ZeroOuUm(x, a + 1);

                    if (x > 0)
                        this.matriz[x - 1, a] = this.ZeroOuUm(x - 1, a);

                    if (x < 9)
                        this.matriz[x + 1, a] = this.ZeroOuUm(x + 1, a);
                }
                else
                {
                    if (inicio < 9)
                        this.matriz[x, inicio + 1] = this.ZeroOuUm(x, inicio + 1);

                    if (x > 0)
                        this.matriz[x - 1, inicio] = this.ZeroOuUm(x - 1, inicio);

                    if (x < 9)
                        this.matriz[x + 1, inicio] = this.ZeroOuUm(x + 1, inicio);
                }

                y--;
            }
            else
            if (direcao == 1 && dirDoNavio % 2 == 0)
            {
                if (y == 0)
                    return;

                int inicio = 0;

                if (dirDoNavio == 0)
                    inicio = x - pos + 1;
                else
                    inicio = x + pos - 1;

                if (dirDoNavio == 0)
                {
                    if (inicio > 0)
                        if (ehNavio(matriz[inicio - 1, y - 1]))
                            return;

                    if (inicio + tam <= 9)
                        if (ehNavio(matriz[inicio + tam, y - 1]))
                            return;
                }
                else
                {
                    if (inicio < 9)
                        if (ehNavio(matriz[inicio + 1, y - 1]))
                            return;

                    if (inicio - tam >= 0)
                        if (ehNavio(matriz[inicio - tam, y - 1]))
                            return;
                }

                if (y > 1)
                    if (dirDoNavio == 0)
                    {
                        for (int i = inicio; i < inicio + tam; i++)
                            if (ehNavio(matriz[i, y - 2]))
                                return;
                    }
                    else
                        for (int i = inicio; i > inicio - tam; i--)
                            if (ehNavio(matriz[i, y - 2]))
                                return;

                if (dirDoNavio == 0)
                    for (int i = inicio; i < inicio + tam; i++)
                    {
                        matriz[i, y] = "0.0.0"; //zero primeiro para o zeroOuUm dar certo
                    }
                else
                    for (int i = inicio; i > inicio - tam; i--)
                    {
                        matriz[i, y] = "0.0.0"; //zero primeiro para o zeroOuUm dar certo
                    }

                if (dirDoNavio == 0)
                {
                    if (inicio > 0)
                        matriz[inicio - 1, y] = this.ZeroOuUm(inicio - 1, y);

                    if (inicio + tam <= 9)
                        matriz[inicio + tam, y] = this.ZeroOuUm(inicio + tam, y);
                }
                else
                {
                    if (inicio < 9)
                        matriz[inicio + 1, y] = this.ZeroOuUm(inicio + 1, y);

                    if (inicio - tam >= 0)
                        matriz[inicio - tam, y] = this.ZeroOuUm(inicio - tam, y);
                }

                int index = 0;

                if (dirDoNavio == 0)
                    for (int i = inicio; i < inicio + tam; i++)
                    {
                        matriz[i, y - 1] = posicoes[i - inicio]; //zero primeiro para o zeroOuUm dar certo
                        if (y < 9)
                            matriz[i, y + 1] = this.ZeroOuUm(i, y + 1); //zero primeiro para o zeroOuUm dar certo
                    }
                else
                    for (int i = inicio; i > inicio - tam; i--)
                    {
                        matriz[i, y - 1] = posicoes[index]; //zero primeiro para o zeroOuUm dar certo
                        if (y < 9)
                            matriz[i, y + 1] = this.ZeroOuUm(i, y + 1); //zero primeiro para o zeroOuUm dar certo
                        index++;
                    }
                y--;
            }

            if (direcao == 2 && dirDoNavio % 2 == 0) //direita
            {
                int inicio = 0;

                if (dirDoNavio == 0)
                    inicio = x - pos + 1;
                else
                    inicio = x + pos - 1;

                int indice = 0;

                if (inicio + 1 > 9)
                    return;

                if (direcao == dirDoNavio)
                {
                    if (inicio < 8 && this.ehNavio(this.matriz[inicio + 2, y]))
                        return;

                    if (y > 0 && this.ehNavio(this.matriz[inicio + 1, y - 1]))
                        return;

                    if (y < 9 && this.ehNavio(this.matriz[inicio + 1, y + 1]))
                        return;

                    for (int i = inicio; i > inicio - tam; i--)
                    {
                        this.matriz[i + 1, y] = posicoes[indice];
                        indice++;
                    }
                }
                else
                {
                    int ultimo = inicio + tam - 1;
                    int ultIndex = tam - 1;

                    if (ultimo == 9)
                        return;

                    if (ultimo < 8 && this.ehNavio(this.matriz[ultimo + 2, y]))
                        return;

                    if (y > 0 && this.ehNavio(this.matriz[ultimo + 1, y - 1]))
                        return;

                    if (y < 9 && this.ehNavio(this.matriz[ultimo + 1, y + 1]))
                        return;

                    for (int i = ultimo; i > ultimo - tam; i--)
                    {
                        this.matriz[i + 1, y] = posicoes[ultIndex];
                        indice++;
                        ultIndex--;
                    }
                }

                if (dirDoNavio == direcao)
                {
                    int a = inicio - tam + 1;

                    if (a > 0)
                        this.matriz[a - 1, y] = this.ZeroOuUm(a - 1, y);

                    if (y > 0)
                        this.matriz[a, y - 1] = this.ZeroOuUm(a, y - 1);

                    if (y < 9)
                        this.matriz[a, y + 1] = this.ZeroOuUm(a, y + 1);
                }
                else
                {
                    if (inicio > 0)
                        this.matriz[inicio - 1, y] = this.ZeroOuUm(inicio - 1, y);

                    if (y > 0)
                        this.matriz[inicio, y - 1] = this.ZeroOuUm(inicio, y - 1);

                    if (y < 9)
                        this.matriz[inicio, y + 1] = this.ZeroOuUm(inicio, y + 1);
                }

                x++;
            }
            else
            if (direcao == 2 && dirDoNavio % 2 == 1)
            {
                if (x == 9)
                    return;

                int inicio = 0;

                if (dirDoNavio == 1)
                    inicio = y - pos + 1;
                else
                    inicio = y + pos - 1;

                if (dirDoNavio == 1)
                {
                    if (inicio > 0)
                        if (ehNavio(matriz[x + 1, inicio - 1]))
                            return;

                    if (inicio + tam <= 9)
                        if (ehNavio(matriz[x + 1, inicio + tam]))
                            return;
                }
                else
                {
                    if (inicio < 9)
                        if (ehNavio(matriz[x + 1, inicio + 1]))
                            return;

                    if (inicio - tam >= 0)
                        if (ehNavio(matriz[x + 1, inicio - tam]))
                            return;
                }

                if (x < 8)
                    if (dirDoNavio == 1)
                    {
                        for (int i = inicio; i < inicio + tam; i++)
                            if (ehNavio(matriz[x + 2, i]))
                                return;
                    }
                    else
                        for (int i = inicio; i > inicio - tam; i--)
                            if (ehNavio(matriz[x + 2, i]))
                                return;

                if (dirDoNavio == 1)
                    for (int i = inicio; i < inicio + tam; i++)
                    {
                        matriz[x, i] = "0.0.0"; //zero primeiro para o zeroOuUm dar certo
                    }
                else
                    for (int i = inicio; i > inicio - tam; i--)
                    {
                        matriz[x, i] = "0.0.0"; //zero primeiro para o zeroOuUm dar certo
                    }

                if (dirDoNavio == 1)
                {
                    if (inicio > 0)
                        matriz[x, inicio - 1] = this.ZeroOuUm(x, inicio - 1);

                    if (inicio + tam <= 9)
                        matriz[x, inicio + tam] = this.ZeroOuUm(x, inicio + tam);
                }
                else
                {
                    if (inicio < 9)
                        matriz[x, inicio + 1] = this.ZeroOuUm(x, inicio + 1);

                    if (inicio - tam >= 0)
                        matriz[x, inicio - tam] = this.ZeroOuUm(x, inicio - tam);
                }

                int index = 0;

                if (dirDoNavio == 1)
                    for (int i = inicio; i < inicio + tam; i++)
                    {
                        matriz[x + 1, i] = posicoes[i - inicio]; //zero primeiro para o zeroOuUm dar certo
                        if (x > 0)
                            matriz[x - 1, i] = this.ZeroOuUm(x - 1, i); //zero primeiro para o zeroOuUm dar certo
                    }
                else
                    for (int i = inicio; i > inicio - tam; i--)
                    {
                        matriz[x + 1, i] = posicoes[index]; //zero primeiro para o zeroOuUm dar certo
                        if (x > 0)
                            matriz[x - 1, i] = this.ZeroOuUm(x - 1, i); //zero primeiro para o zeroOuUm dar certo
                        index++;
                    }
                x++;
            }

            if (direcao == 3 && dirDoNavio % 2 == 1) //baixo
            {
                int inicio = 0;

                if (dirDoNavio == 1)
                    inicio = y - pos + 1;
                else
                    inicio = y + pos - 1;

                if (inicio == 9)
                    return;

                int indice = 0;
                if (dirDoNavio == direcao)
                {
                    if (inicio < 8 && this.ehNavio(this.matriz[x, inicio + 2]))
                        return;

                    if (x > 0 && this.ehNavio(this.matriz[x - 1, inicio + 1]))
                        return;

                    if (x < 9 && this.ehNavio(this.matriz[x + 1, inicio + 1]))
                        return;

                    for (int i = inicio; i > inicio - tam; i--)
                    {
                        this.matriz[x, i + 1] = posicoes[indice];
                        indice++;
                    }
                }
                else
                {
                    int ultimo = inicio + tam - 1;
                    int ultIndex = tam - 1;

                    if (ultimo == 9)
                        return;

                    if (ultimo < 8 && this.ehNavio(this.matriz[x, ultimo + 2]))
                        return;

                    if (x > 0 && this.ehNavio(this.matriz[x - 1, ultimo + 1]))
                        return;

                    if (x < 9 && this.ehNavio(this.matriz[x + 1, ultimo + 1]))
                        return;

                    for (int i = ultimo; i > ultimo - tam; i--)
                    {
                        this.matriz[x, i + 1] = posicoes[ultIndex];
                        indice++;
                        ultIndex--;
                    }
                }

                if (dirDoNavio == direcao)
                {
                    int a = inicio - tam + 1;

                    if (a > 0)
                        this.matriz[x, a - 1] = this.ZeroOuUm(x, a - 1);

                    if (x > 0)
                        this.matriz[x - 1, a] = this.ZeroOuUm(x - 1, a);

                    if (x < 9)
                        this.matriz[x + 1, a] = this.ZeroOuUm(x + 1, a);
                }
                else
                {
                    if (inicio > 0)
                        this.matriz[x, inicio - 1] = this.ZeroOuUm(x, inicio - 1);

                    if (x > 0)
                        this.matriz[x - 1, inicio] = this.ZeroOuUm(x - 1, inicio);

                    if (x < 9)
                        this.matriz[x + 1, inicio] = this.ZeroOuUm(x + 1, inicio);
                }

                y++;
            }
            else
            if (direcao == 3 && dirDoNavio % 2 == 0)
            {
                if (y == 9)
                    return;

                int inicio = 0;

                if (dirDoNavio == 0)
                    inicio = x - pos + 1;
                else
                    inicio = x + pos - 1;

                if (dirDoNavio == 0)
                {
                    if (inicio > 0)
                        if (ehNavio(matriz[inicio - 1, y + 1]))
                            return;

                    if (inicio + tam <= 9)
                        if (ehNavio(matriz[inicio + tam, y + 1]))
                            return;
                }
                else
                {
                    if (inicio < 9)
                        if (ehNavio(matriz[inicio + 1, y + 1]))
                            return;

                    if (inicio - tam >= 0)
                        if (ehNavio(matriz[inicio - tam, y + 1]))
                            return;
                }

                if (y < 8)
                    if (dirDoNavio == 0)
                    {
                        for (int i = inicio; i < inicio + tam; i++)
                            if (ehNavio(matriz[i, y + 2]))
                                return;
                    }
                    else
                        for (int i = inicio; i > inicio - tam; i--)
                            if (ehNavio(matriz[i, y + 2]))
                                return;

                if (dirDoNavio == 0)
                    for (int i = inicio; i < inicio + tam; i++)
                    {
                        matriz[i, y] = "0.0.0"; //zero primeiro para o zeroOuUm dar certo
                    }
                else
                    for (int i = inicio; i > inicio - tam; i--)
                    {
                        matriz[i, y] = "0.0.0"; //zero primeiro para o zeroOuUm dar certo
                    }

                if (dirDoNavio == 0)
                {
                    if (inicio > 0)
                        matriz[inicio - 1, y] = this.ZeroOuUm(inicio - 1, y);

                    if (inicio + tam <= 9)
                        matriz[inicio + tam, y] = this.ZeroOuUm(inicio + tam, y);
                }
                else
                {
                    if (inicio < 9)
                        matriz[inicio + 1, y] = this.ZeroOuUm(inicio + 1, y);

                    if (inicio - tam >= 0)
                        matriz[inicio - tam, y] = this.ZeroOuUm(inicio - tam, y);
                }

                int index = 0;

                if (dirDoNavio == 0)
                    for (int i = inicio; i < inicio + tam; i++)
                    {
                        matriz[i, y + 1] = posicoes[i - inicio]; //zero primeiro para o zeroOuUm dar certo
                        if (y > 0)
                            matriz[i, y - 1] = this.ZeroOuUm(i, y - 1); //zero primeiro para o zeroOuUm dar certo
                    }
                else
                    for (int i = inicio; i > inicio - tam; i--)
                    {
                        matriz[i, y + 1] = posicoes[index]; //zero primeiro para o zeroOuUm dar certo
                        if (y > 0)
                            matriz[i, y - 1] = this.ZeroOuUm(i, y - 1); //zero primeiro para o zeroOuUm dar certo
                        index++;
                    }
                y++;
            }

            this.PreencherAoRedorNavio(this.matriz, x, y);
            this.posicaoAtual = "" + x + y;
        }

        private bool TemNavioAoredor(String[,] mat, int x, int y, int tam)
        {
            if (x < 0 || x > 9 || y > 9 || y < 0)
                return false;

            if (tam > 5 || tam < 2)
                return false;

            if (y > 0)
                if (ehNavio(mat[x, y - 1]))
                    return true;


            if (y + tam < 10)
                if (ehNavio(mat[x, y + tam]))
                    return true;

            for (int i = y; i < y + tam; i++)
            {
                if (x > 0)
                    if (ehNavio(mat[x - 1, i]))
                        return true;

                if (x < 9)
                    if (ehNavio(mat[x + 1, i]))
                        return true;
            }

            return false;

        }

        private bool TemNavioNoMeio(int tam)
        {
            if (tam == 2)
            {
                if (ehNavio(this.matriz[4, 4]) || ehNavio(this.matriz[4, 5]))
                    return true;
            }
            else
            if (tam == 3)
            {
                if (ehNavio(this.matriz[4, 4]) || ehNavio(this.matriz[4, 5]) || ehNavio(this.matriz[4, 6]))
                    return true;
            }
            else
            if (tam == 4)
            {
                if (ehNavio(this.matriz[4, 3]) || ehNavio(this.matriz[4, 4]) || ehNavio(this.matriz[4, 5]) || ehNavio(this.matriz[4, 6]))
                    return true;
            }
            else
            if (tam == 5)
            {
                if (ehNavio(this.matriz[4, 3]) || ehNavio(this.matriz[4, 4]) || ehNavio(this.matriz[4, 5]) || ehNavio(this.matriz[4, 6]) || ehNavio(this.matriz[4, 7]))
                    return true;
            }

            return false;
        }

        private void ColocarONavioNaTela(int tam)
        {
            if (tam == 2)
            {
                if (this.TemNavioAoredor(this.matriz, 4, 4, 2) || this.TemNavioNoMeio(2))
                {
                    this.AjustarLabel("Já há um navio no meio!! Por favor, o retire");
                    return;
                }

                this.estaInserindoNoMomento = true;
                this.matriz[4, 4] = "2.1.1";
                this.matriz[4, 5] = "2.2.1";
                this.refreshDtg();
                this.posicaoAtual = "44";
                this.tamanmhoUlt = 2;
                this.PreencherAoRedorNavio(this.matriz, 4, 4);
            }
            else
            if (tam == 3)
            {
                if (this.TemNavioAoredor(this.matriz, 4, 4, 3) || this.TemNavioNoMeio(3))
                {
                    this.AjustarLabel("Já há um navio no meio!! Por favor, o retire");
                    return;
                }

                this.estaInserindoNoMomento = true;
                this.matriz[4, 4] = "3.1.1";
                this.matriz[4, 5] = "3.2.1";
                this.matriz[4, 6] = "3.3.1";

                this.refreshDtg();
                this.posicaoAtual = "44";
                this.tamanmhoUlt = 3;
                this.PreencherAoRedorNavio(this.matriz, 4, 4);
            }
            else
            if (tam == 4)
            {
                if (this.TemNavioAoredor(this.matriz, 4, 3, 4) || this.TemNavioNoMeio(4))
                {
                    this.AjustarLabel("Já há um navio no meio!! Por favor, o retire");
                    return;
                }

                this.estaInserindoNoMomento = true;
                this.matriz[4, 3] = "4.1.1";
                this.matriz[4, 4] = "4.2.1";
                this.matriz[4, 5] = "4.3.1";
                this.matriz[4, 6] = "4.4.1";

                this.refreshDtg();
                this.posicaoAtual = "43";
                this.tamanmhoUlt = 4;
                this.PreencherAoRedorNavio(this.matriz, 4, 3);
            }
            else
            if (tam == 5)
            {
                if (this.TemNavioAoredor(this.matriz, 4, 3, 5) || this.TemNavioNoMeio(5))
                {
                    this.AjustarLabel("Já há um navio no meio!! Por favor, o retire");
                    return;
                }

                this.estaInserindoNoMomento = true;
                this.matriz[4, 3] = "5.1.1";
                this.matriz[4, 4] = "5.2.1";
                this.matriz[4, 5] = "5.3.1";
                this.matriz[4, 6] = "5.4.1";
                this.matriz[4, 7] = "5.5.1";

                this.refreshDtg();
                this.posicaoAtual = "43";
                this.tamanmhoUlt = 5;
                this.PreencherAoRedorNavio(this.matriz, 4, 3);
            }

        }

        private void VerBotoesDeInsersao(bool ver)
        {
            this.btnEnter.Visible = ver;
            this.btnUp.Visible = ver;
            this.btnLeft.Visible = ver;
            this.btnRight.Visible = ver;
            this.btnDown.Visible = ver;
            this.btnGirar.Visible = ver;
            this.btnDelete.Visible = ver;
        }

        private void ControleDosBotoesUpgrade(bool ver1, bool ver2, String nomePrim, String nomeSeg)
        {
            this.btnDeCima.Visible = ver1;
            this.btnDeBaixo.Visible = ver2;
            this.btnDeCima.Text = nomePrim;
            this.btnDeBaixo.Text = nomeSeg;
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            this.btnIniciar.Location = new Point(945, 580);
            this.btnIniciar.Visible = false;
            this.naviosInseridos = null;
            this.naviosInseridos = new String[10];
            this.indicesInseridos = "";
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Height = 715;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.RecarregarDtg(dtgCampo, false, this.pais);
            this.estaInserindo = true;
            this.Controls.Remove(btnInserir);
            this.Controls.Remove(btnAleatorio);
            this.PrepararInsercao();
            this.VerBotoesDeInsersao(true);
            this.btnIniciar.Visible = false;
            this.ControleDosBotoesUpgrade(true, true, "Completar Frota", "Inserir Aleatoriamente");
            this.colocarOPrimeiroNavio();
            
            this.Animacaozinha("Para inserir nossa frota, clique duas vezes nos desenhos de nossos navios");

        }

        private void btnAleatorio_Click(object sender, EventArgs e)
        {
            this.naviosInseridos = null;
            this.indicesInseridos = "";
            this.ultimoIndexInserido = -1;
            this.qtdNaviosDe2Inseridos = 0;
            this.qtdNaviosDe3Inseridos = 0;
            this.qtdNaviosDe4Inseridos = 0;
            this.inseriuODe5 = false;
            this.estaInserindo = false;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Height = 715;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.VerBotoesDeInsersao(false);
            this.RecarregarDtg(dtgCampo, false, this.pais);
            this.RecarregarDtg(dtgCampo, true, this.pais);
            this.ControleDosBotoesUpgrade(false, false, "Inserir Frota", "Inserir Aleatoriamente");
            this.btnIniciar.Location = new Point(this.Width / 2 - 75, btnDeBaixo.Location.Y);

            btnIniciar.Visible = true;
            if (this.pbxDe2 != null)
            {
                this.pbxDe2.Visible = false;
                this.pbxDe3.Visible = false;
                this.pbxDe4.Visible = false;
                this.pbxDe5.Visible = false;
                this.VerLabels(false);
                this.labelDe2.Visible = false;
                this.labelDe3.Visible = false;
                this.labelDe4.Visible = false;
                this.labelDe5.Visible = false;
            }
        }

        private bool inseriuTodos()
        {
            if (qtdNaviosDe2Inseridos < 4)
                return false;

            if (qtdNaviosDe3Inseridos < 3)
                return false;

            if (qtdNaviosDe4Inseridos < 2)
                return false;

            if (inseriuODe5 == false)
                return false;

            return true;
        }

        private void AtualizarLabels()
        {
            this.labelDe2.Text = "(" + Convert.ToString(4 - this.qtdNaviosDe2Inseridos) + "x)";
            this.labelDe3.Text = "(" + Convert.ToString(3 - this.qtdNaviosDe3Inseridos) + "x)";
            this.labelDe4.Text = "(" + Convert.ToString(2 - this.qtdNaviosDe4Inseridos) + "x)";

            if (this.inseriuODe5)
                this.labelDe5.Text = "(0x)";
            else
                this.labelDe5.Text = "(1x)";
        }

        private void jogadaDeBot()
        {
            if (!vezDoBot)
                return;
            Thread.Sleep(2000);
            int x;
            int y;

            while (true)
            {
                if (temPreferencia == -1)
                {
                    x = RandomNumber(10);
                    y = RandomNumber(10);
                }
                else
                {
                    int ult = Convert.ToInt16(this.ultimaJogada);
                    if (ult > 99)
                    {
                        if (Convert.ToInt16(this.ultimaJogada.Substring(1, 1)) == 0)
                        {
                            x = Convert.ToInt16(this.ultimaJogada.Substring(0, 2));
                            y = Convert.ToInt16(this.ultimaJogada.Substring(2, 1));
                        }
                        else
                        {
                            x = Convert.ToInt16(this.ultimaJogada.Substring(0, 1));
                            y = Convert.ToInt16(this.ultimaJogada.Substring(1, ultimaJogada.Length - 1));
                        }
                    }
                    else
                    {
                        x = Convert.ToInt16(this.ultimaJogada.Substring(0, 1));
                        y = Convert.ToInt16(this.ultimaJogada.Substring(1, ultimaJogada.Length - 1));
                    }



                    if (temPreferencia == 0) //esquerda
                        x--;
                    else
                    if (temPreferencia == 1) //cima
                        y--;
                    else
                    if (temPreferencia == 2) //direita
                        x++;
                    else
                    if (temPreferencia == 3) //baixo
                        y++;

                    int qlSeraAAdjacente = 0; //sera a posicao adjacente q vou jogar
                    if (temPreferencia == 4)
                    {
                        qlSeraAAdjacente = RandomNumber(4);
                        switch (qlSeraAAdjacente)
                        {
                            case 0: x++; break;
                            case 1: y++; break;
                            case 2: x--; break;
                            case 3: y--; break;
                        }
                    }
                    //se a preferencia for 4 tenho q jogar em algm lugar adjacente e ver noq dá
                    //se cair em agua, nao alteraria a ultimaJogada, p ele voltar da mesma pos
                    //se cair em navio, poe uma pref nova

                    //como fazer jogar em algm adjacente?? Qual adjacente??

                    //tenho que, primeiro deixar o BOT errar o tiro, para depois dizer onde jogar
                    if (x < 0 || x > 9 || y < 0 || y > 9)
                    {
                        if (temPreferencia == 0)
                            x += this.tamanmhoUlt;

                        if (temPreferencia == 1)
                            y += this.tamanmhoUlt;

                        if (temPreferencia == 2)
                            x -= this.tamanmhoUlt;

                        if (temPreferencia == 3)
                            y -= this.tamanmhoUlt;

                        if (temPreferencia == 4)
                        {
                            if (x < 0)
                                x = 0;

                            if (x > 9)
                                x = 9;

                            if (y < 0)
                                y = 0;

                            if (y < 9)
                                y = 9;
                        }

                    }
                }

                if (this.ultimaJogada == Convert.ToString(x) + Convert.ToString(y))
                    continue;

                int tem = this.verPosicao(matriz, x, y);

                if (tem == 1)
                {
                    if (this.matriz[x, y] != "6.6.6")
                    {
                        int tinha = 0;
                        //vou usar o tinha, pois soh se acertou 2x o navio ele deveria saber 100% onde esta o resto
                        //if(this.ultimaJogada!= null)
                        //tinha= this.verPosicao(matriz, Convert.ToInt16(this.ultimaJogada.Substring(0, 1)), Convert.ToInt16(this.ultimaJogada.Substring(1, 1)));

                        this.tamanmhoUlt = Convert.ToInt16(this.matriz[x, y].Substring(0, 1));

                        //if (tinha == 1)
                        //{
                        if (x > 0 && this.verPosicao(matriz, x - 1, y) == 1 && matriz[x - 1, y] != "6.6.6") //esquerda
                            temPreferencia = 0;
                        else
                        if (y > 0 && this.verPosicao(matriz, x, y - 1) == 1 && matriz[x, y - 1] != "6.6.6") //cima
                            temPreferencia = 1;
                        else
                        if (x < 9 && this.verPosicao(matriz, x + 1, y) == 1 && matriz[x + 1, y] != "6.6.6") //direita
                            temPreferencia = 2;
                        else
                        if (y < 9 && this.verPosicao(matriz, x, y + 1) == 1 && matriz[x, y + 1] != "6.6.6") //baixo
                            temPreferencia = 3;
                        //}
                        ///else //se acertou pela primeira vez, deveria tentar em TODAS as posicoes
                        //this.temPreferencia = 4;

                        if (derrubouNavio(matriz, x, y))
                        {
                            this.temPreferencia = -1;
                            this.Invoke((MethodInvoker)delegate
                            {
                                Label lbEmQuestao = new Label();
                               
                                int tam = Convert.ToInt16(matriz[x, y].Substring(0, 1));
                                if (tam == 2)
                                {
                                    this.lbQtdeSubarinos2.Text = Convert.ToString(Convert.ToInt16(lbQtdeSubarinos2.Text) - 1);
                                    lbEmQuestao = lbQtdeSubarinos2;
                                }
                                else
                                if (tam == 3)
                                {
                                    lbQtdCruzadores2.Text = Convert.ToString(Convert.ToInt16(lbQtdCruzadores2.Text) - 1);
                                    lbEmQuestao = lbQtdCruzadores2;
                                }
                                else
                                if (tam == 4)
                                {
                                    lbQtdEncouracados2.Text = Convert.ToString(Convert.ToInt16(lbQtdEncouracados2.Text) - 1);
                                    lbEmQuestao = lbQtdEncouracados2;
                                }
                                else
                                if (tam == 5)
                                {
                                    lbQtdPortaavioes2.Text = "0";
                                    lbEmQuestao = lbQtdPortaavioes2;
                                }                   
                            });  
                        }

                        this.qtdeNaviosMeus--;

                        dtgCampo[x + 1, y].Value = imlImagens.Images[15];

                        Thread s = new Thread(this.botAcertouNavioTocarSom);
                        s.Start();
                        this.matriz[x, y] = "6.6.6";
                        if (this.qtdeNaviosMeus == 0)
                        {
                            Thread.Sleep(2000);
                            this.Invoke((MethodInvoker)delegate
                            {
                                // close the form on the forms thread
                                fimDeJogo fim = new fimDeJogo(false, this.pais, this.nivel, soma, ehOCerto, this.nav, this.estaTocando);
                                fim.Show();
                                this.temp = false;
                                this.Close();
                            });
                            return;
                        }

                        break;
                    }
                    else //ja acertou esse navio
                    {
                        continue;
                    }

                }
                else
                {
                    if (this.matriz[x, y] != "7.7.7") //BOT ja clicou nessa posicao
                    {
                        Thread s = new Thread(this.botAcertouAguaTocarSom);
                        s.Start();
                        this.matriz[x, y] = "7.7.7"; // quer dizer que nao tem navio, mas ja clicou. Assim , o bot n clica 2x no mesmo lugar

                        dtgCampo[x + 1, y].Value = imlImagens.Images[16];

                        if (temPreferencia == 0)
                            x += this.tamanmhoUlt + 1;

                        if (temPreferencia == 1)
                            y += this.tamanmhoUlt + 1;

                        if (temPreferencia == 2)
                            x -= this.tamanmhoUlt - 1;

                        if (temPreferencia == 3)
                            y -= this.tamanmhoUlt - 1;
                        break;
                    }
                    else
                    {
                        int ult = Convert.ToInt16(this.ultimaJogada);
                        int xAnt = 0;
                        int yAnt = 0;

                        if (ult > 99)
                        {
                            if (Convert.ToInt16(this.ultimaJogada.Substring(1, 1)) == 0)
                            {
                                xAnt = Convert.ToInt16(this.ultimaJogada.Substring(0, 2));
                                yAnt = Convert.ToInt16(this.ultimaJogada.Substring(2, 1));
                            }
                            else
                            {
                                xAnt = Convert.ToInt16(this.ultimaJogada.Substring(0, 1));
                                yAnt = Convert.ToInt16(this.ultimaJogada.Substring(1, ultimaJogada.Length - 1));
                            }
                        }
                        else
                        {
                            xAnt = Convert.ToInt16(this.ultimaJogada.Substring(0, 1));
                            yAnt = Convert.ToInt16(this.ultimaJogada.Substring(1, ultimaJogada.Length - 1));
                        }

                        if (ehNavio(matriz[xAnt, yAnt]))
                        {

                            if (temPreferencia == 0)
                                x += this.tamanmhoUlt + 1;

                            if (temPreferencia == 1)
                                y += this.tamanmhoUlt + 1;

                            if (temPreferencia == 2)
                                x -= this.tamanmhoUlt - 1;

                            if (temPreferencia == 3)
                                y -= this.tamanmhoUlt - 1;

                            this.ultimaJogada = Convert.ToString(x) + Convert.ToString(y);
                        }
                        else
                            this.temPreferencia = -1;

                        continue;
                    }
                }
            }

            //ajeita a ultima jogada
            //nao vou alterar se for 4, pois se for eu vou tentar outra posicao adjacente à antiga
            if (this.temPreferencia != 4)
                this.ultimaJogada = Convert.ToString(x) + Convert.ToString(y);
            vezDoBot = false;

        }

        private void jogadaDeBotNivel2()
        {
            if (!vezDoBot)
                return;
            Thread.Sleep(2000);
            int x;
            int y;

            while (true)
            {
                if (temPreferencia == -1)
                {
                    x = RandomNumber(10);
                    y = RandomNumber(10);
                }
                else
                {
                    int ult = Convert.ToInt16(this.ultimaJogada);
                    if (ult > 99)
                    {
                        if (Convert.ToInt16(this.ultimaJogada.Substring(1, 1)) == 0)
                        {
                            x = Convert.ToInt16(this.ultimaJogada.Substring(0, 2));
                            y = Convert.ToInt16(this.ultimaJogada.Substring(2, 1));
                        }
                        else
                        {
                            x = Convert.ToInt16(this.ultimaJogada.Substring(0, 1));
                            y = Convert.ToInt16(this.ultimaJogada.Substring(1, ultimaJogada.Length - 1));
                        }
                    }
                    else
                    {
                        x = Convert.ToInt16(this.ultimaJogada.Substring(0, 1));
                        y = Convert.ToInt16(this.ultimaJogada.Substring(1, ultimaJogada.Length - 1));
                    }

                    if (temPreferencia == 0) //esquerda
                        x--;
                    else
                    if (temPreferencia == 1) //cima
                        y--;
                    else
                    if (temPreferencia == 2) //direita
                        x++;
                    else
                    if (temPreferencia == 3) //baixo
                        y++;

                    int qlSeraAAdjacente = 0; //sera a posicao adjacente q vou jogar
                    if (temPreferencia == 4)
                    {
                        qlSeraAAdjacente = RandomNumber(4);
                        switch (qlSeraAAdjacente)
                        {
                            case 0: x++; break;
                            case 1: y++; break;
                            case 2: x--; break;
                            case 3: y--; break;
                        }
                    }
                    //se a preferencia for 4 tenho q jogar em algm lugar adjacente e ver noq dá
                    //se cair em agua, nao alteraria a ultimaJogada, p ele voltar da mesma pos
                    //se cair em navio, poe uma pref nova

                    //como fazer jogar em algm adjacente?? Qual adjacente??

                    //tenho que, primeiro deixar o BOT errar o tiro, para depois dizer onde jogar
                    if (x < 0 || x > 9 || y < 0 || y > 9)
                    {
                        if (temPreferencia == 0)
                            x += this.tamanmhoUlt;

                        if (temPreferencia == 1)
                            y += this.tamanmhoUlt;

                        if (temPreferencia == 2)
                            x -= this.tamanmhoUlt;

                        if (temPreferencia == 3)
                            y -= this.tamanmhoUlt;

                        if (temPreferencia == 4)
                        {
                            if (x < 0)
                                x = 0;

                            if (x > 9)
                                x = 9;

                            if (y < 0)
                                y = 0;

                            if (y > 9)
                                y = 9;
                        }

                    }
                }

                if (this.ultimaJogada == Convert.ToString(x) + Convert.ToString(y))
                    continue;

                if (matriz[x, y] == "8.8.8")
                    continue;

                int tem = this.verPosicao(matriz, x, y);

                if (tem == 1)
                {
                    if (this.matriz[x, y] != "6.6.6")
                    {
                        int tinha = 0;
                        //vou usar o tinha, pois soh se acertou 2x o navio ele deveria saber 100% onde esta o resto
                        //if(this.ultimaJogada!= null)
                        //tinha= this.verPosicao(matriz, Convert.ToInt16(this.ultimaJogada.Substring(0, 1)), Convert.ToInt16(this.ultimaJogada.Substring(1, 1)));

                        this.tamanmhoUlt = Convert.ToInt16(this.matriz[x, y].Substring(0, 1));

                        //if (tinha == 1)
                        //{
                        if (x > 0 && this.verPosicao(matriz, x - 1, y) == 1 && matriz[x - 1, y] != "6.6.6") //esquerda
                            temPreferencia = 0;
                        else
                        if (y > 0 && this.verPosicao(matriz, x, y - 1) == 1 && matriz[x, y - 1] != "6.6.6") //cima
                            temPreferencia = 1;
                        else
                        if (x < 9 && this.verPosicao(matriz, x + 1, y) == 1 && matriz[x + 1, y] != "6.6.6") //direita
                            temPreferencia = 2;
                        else
                        if (y < 9 && this.verPosicao(matriz, x, y + 1) == 1 && matriz[x, y + 1] != "6.6.6") //baixo
                            temPreferencia = 3;
                        //}
                        ///else //se acertou pela primeira vez, deveria tentar em TODAS as posicoes
                        //this.temPreferencia = 4;


                        if (derrubouNavio(matriz, x, y))
                        {
                            this.temPreferencia = -1;
                            this.preencherAoRedorNavioProBotNivel2(matriz, x, y);
                        }
                        this.qtdeNaviosMeus--;

                        dtgCampo[x + 1, y].Value = imlImagens.Images[15];

                        Thread s = new Thread(this.botAcertouNavioTocarSom);
                        s.Start();
                        this.matriz[x, y] = "6.6.6";

                        if (this.qtdeNaviosMeus == 0)
                        {
                            Thread.Sleep(1000);
                            this.Invoke((MethodInvoker)delegate
                            {
                                // close the form on the forms thread
                                fimDeJogo fim = new fimDeJogo(false, this.pais, this.nivel, soma, ehOCerto, this.nav, this.estaTocando);
                                fim.Show();
                                this.Close();
                            });
                            return;
                        }

                        break;
                    }
                    else //ja acertou esse navio
                    {
                        continue;
                    }

                }
                else
                {
                    if (this.matriz[x, y] != "7.7.7") //BOT ja clicou nessa posicao
                    {
                        Thread s = new Thread(this.botAcertouAguaTocarSom);
                        s.Start();
                        this.matriz[x, y] = "7.7.7"; // quer dizer que nao tem navio, mas ja clicou. Assim , o bot n clica 2x no mesmo lugar

                        dtgCampo[x + 1, y].Value = imlImagens.Images[16];

                        if (temPreferencia == 0)
                            x += this.tamanmhoUlt + 1;

                        if (temPreferencia == 1)
                            y += this.tamanmhoUlt + 1;

                        if (temPreferencia == 2)
                            x -= this.tamanmhoUlt - 1;

                        if (temPreferencia == 3)
                            y -= this.tamanmhoUlt - 1;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            //ajeita a ultima jogada
            //nao vou alterar se for 4, pois se for eu vou tentar outra posicao adjacente à antiga
            if (this.temPreferencia != 4)
                this.ultimaJogada = Convert.ToString(x) + Convert.ToString(y);
            vezDoBot = false;

        }

        private void botAcertouNavioTocarSom()
        {
            if (estaTocando)
            {
                this.player1 = new SoundPlayer("../audios/explo.wav");
                player1.Play();
            }
        }

        private void botAcertouAguaTocarSom()
        {
            if (estaTocando)
            {
                this.player1 = new SoundPlayer("../audios/water.wav");
                player1.Play();
            }
        }

        private void jogadorAcertouNavioTocarSom()
        {
            if (estaTocando)
            {
                this.player2 = new SoundPlayer("../audios/explo.wav");
                player2.Play();
            }
        }

        private void jogadorAcertouAguaTocarSom()
        {
            if (estaTocando)
            {
                this.player2 = new SoundPlayer("../audios/water.wav");
                player2.Play();
            }
        }

        private void preencherAoRedorNavioProBotNivel2(String[,] mat, int x, int y)
        {
            if (x < 0 || x > 9 || y > 9 || y < 0)
                return;

            int direcao = Convert.ToInt16(this.matriz[x, y].Substring(4, 1));

            int tam = Convert.ToInt16(this.matriz[x, y].Substring(0, 1));

            int pos = Convert.ToInt16(this.matriz[x, y].Substring(2, 1));

            if (tam > 5 || tam < 2)
                return;

            if (direcao == 0) //esquerda
            {
                int inicio = x - pos + 1;

                if (inicio > 0)
                    mat[inicio - 1, y] = "8.8.8";


                if (inicio + tam < 10)
                    mat[inicio + tam, y] = "8.8.8";


                for (int i = inicio; i < inicio + tam; i++)
                {
                    if (y > 0)
                        mat[i, y - 1] = "8.8.8";


                    if (y < 9)
                        mat[i, y + 1] = "8.8.8";

                }

            }

            if (direcao == 1) //cima
            {
                int inicio = y - pos + 1;

                if (inicio > 0)
                    mat[x, inicio - 1] = "8.8.8";


                if (inicio + tam < 10)
                    mat[x, inicio + tam] = "8.8.8";


                for (int i = inicio; i < inicio + tam; i++)
                {
                    if (x > 0)
                        mat[x - 1, i] = "8.8.8";


                    if (x < 9)
                        mat[x + 1, i] = "8.8.8";

                }
            }

            if (direcao == 2) //direita
            {
                int inicio = x + pos - 1;

                if (inicio < 9)
                    mat[inicio + 1, y] = "8.8.8";


                if (inicio - tam >= 0)
                    mat[inicio - tam, y] = "8.8.8";


                for (int i = inicio; i > inicio - tam; i--)
                {
                    if (y > 0)
                        mat[i, y - 1] = "8.8.8";


                    if (y < 9)
                        mat[i, y + 1] = "8.8.8";

                }
            }

            if (direcao == 3) //baixo
            {
                int inicio = y + pos - 1;

                if (inicio < 9)
                    mat[x, inicio + 1] = "8.8.8";


                if (inicio - tam >= 0)
                    mat[x, inicio - tam] = "8.8.8";


                for (int i = inicio; i > inicio - tam; i--)
                {
                    if (x > 0)
                        mat[x - 1, i] = "8.8.8";


                    if (x < 9)
                        mat[x + 1, i] = "8.8.8";

                }
            }
        }

        private void preencherAoRedorDoNavio(DataGridView dtg, int x, int y, int tam, int direcao)
        {
            if (tam == 2)
                if (direcao == 0) //esquerda
                {
                    if (x > 0)
                        dtg[x - 1, y].Value = imlImagens.Images[16];
                    if (y > 0)
                        dtg[x, y - 1].Value = imlImagens.Images[16];
                    if (y < 9)
                        dtg[x, y + 1].Value = imlImagens.Images[16];

                    if (y > 0)
                        dtg[x + 1, y - 1].Value = imlImagens.Images[16];
                    if (y < 9)
                        dtg[x + 1, y + 1].Value = imlImagens.Images[16];
                    if (x < 8)
                        dtg[x + 2, y].Value = imlImagens.Images[16];
                }
                else
                if (direcao == 1) //cima
                {
                    if (x > 0)
                        dtg[x - 1, y].Value = imlImagens.Images[16];
                    if (y > 0)
                        dtg[x, y - 1].Value = imlImagens.Images[16];
                    if (x < 9)
                        dtg[x + 1, y].Value = imlImagens.Images[16];

                    if (x > 0)
                        dtg[x - 1, y + 1].Value = imlImagens.Images[16];
                    if (y < 8)
                        dtg[x, y + 2].Value = imlImagens.Images[16];
                    if (x < 9)
                        dtg[x + 1, y + 1].Value = imlImagens.Images[16];
                }
                else
                if (direcao == 2) //direita
                {
                    if (y > 0)
                        dtg[x, y - 1].Value = imlImagens.Images[16];
                    if (y < 9)
                        dtg[x, y + 1].Value = imlImagens.Images[16];
                    if (x < 9)
                        dtg[x + 1, y].Value = imlImagens.Images[16];

                    if (y > 0)
                        dtg[x - 1, y - 1].Value = imlImagens.Images[16];
                    if (y < 9)
                        dtg[x - 1, y + 1].Value = imlImagens.Images[16];
                    if (x > 1)
                        dtg[x - 2, y].Value = imlImagens.Images[16];

                }
                else
                if (direcao == 3)//baixo
                {
                    if (x > 0)
                        dtg[x - 1, y].Value = imlImagens.Images[16];
                    if (y < 9)
                        dtg[x, y + 1].Value = imlImagens.Images[16];
                    if (x < 9)
                        dtg[x + 1, y].Value = imlImagens.Images[16];

                    if (x > 0)
                        dtg[x - 1, y - 1].Value = imlImagens.Images[16];
                    if (y > 1)
                        dtg[x, y - 2].Value = imlImagens.Images[16];
                    if (x < 9)
                        dtg[x + 1, y - 1].Value = imlImagens.Images[16];
                }
                else
                if (tam == 3)
                    if (direcao == 0) //esquerda
                    {
                        if (x > 0)
                            dtg[x - 1, y].Value = imlImagens.Images[16];
                        if (y > 0)
                            dtg[x, y - 1].Value = imlImagens.Images[16];
                        if (y < 9)
                            dtg[x, y + 1].Value = imlImagens.Images[16];

                        if (y > 0)
                            dtg[x + 1, y - 1].Value = imlImagens.Images[16];
                        if (y < 9)
                            dtg[x + 1, y + 1].Value = imlImagens.Images[16];

                        if (y > 0)
                            dtg[x + 2, y - 1].Value = imlImagens.Images[16];
                        if (y < 9)
                            dtg[x + 2, y + 1].Value = imlImagens.Images[16];
                        if (x < 7)
                            dtg[x + 3, y].Value = imlImagens.Images[16];
                    }
                    else
                    if (direcao == 1) //cima
                    {
                        if (x > 0)
                            dtg[x - 1, y].Value = imlImagens.Images[16];
                        if (y > 0)
                            dtg[x, y - 1].Value = imlImagens.Images[16];
                        if (x < 9)
                            dtg[x + 1, y].Value = imlImagens.Images[16];

                        if (x > 0)
                            dtg[x - 1, y + 1].Value = imlImagens.Images[16];
                        if (x < 9)
                            dtg[x + 1, y + 1].Value = imlImagens.Images[16];

                        if (x > 0)
                            dtg[x - 1, y + 2].Value = imlImagens.Images[16];
                        if (y < 7)
                            dtg[x, y + 3].Value = imlImagens.Images[16];
                        if (x < 9)
                            dtg[x + 1, y + 2].Value = imlImagens.Images[16];
                    }
                    else
                    if (direcao == 2) //direita
                    {
                        if (y > 0)
                            dtg[x, y - 1].Value = imlImagens.Images[16];
                        if (y < 9)
                            dtg[x, y + 1].Value = imlImagens.Images[16];
                        if (x < 9)
                            dtg[x + 1, y].Value = imlImagens.Images[16];

                        if (y > 0)
                            dtg[x - 1, y - 1].Value = imlImagens.Images[16];
                        if (y < 9)
                            dtg[x - 1, y + 1].Value = imlImagens.Images[16];

                        if (y > 0)
                            dtg[x - 2, y - 1].Value = imlImagens.Images[16];
                        if (y < 9)
                            dtg[x - 2, y + 1].Value = imlImagens.Images[16];
                        if (x > 2)
                            dtg[x - 3, y].Value = imlImagens.Images[16];
                    }
                    else
                    if (direcao == 3) //baixo
                    {
                        if (x > 0)
                            dtg[x - 1, y].Value = imlImagens.Images[16];
                        if (y < 9)
                            dtg[x, y + 1].Value = imlImagens.Images[16];
                        if (x < 9)
                            dtg[x + 1, y].Value = imlImagens.Images[16];

                        if (x > 0)
                            dtg[x - 1, y - 1].Value = imlImagens.Images[16];
                        if (x < 9)
                            dtg[x + 1, y - 1].Value = imlImagens.Images[16];

                        if (x > 0)
                            dtg[x - 1, y - 2].Value = imlImagens.Images[16];
                        if (y > 2)
                            dtg[x, y - 3].Value = imlImagens.Images[16];
                        if (x < 9)
                            dtg[x + 1, y - 2].Value = imlImagens.Images[16];
                    }
                    else
                    if (tam == 4)
                        if (direcao == 0) //esquerda
                        {
                            if (x > 0)
                                dtg[x - 1, y].Value = imlImagens.Images[16];
                            if (y > 0)
                                dtg[x, y - 1].Value = imlImagens.Images[16];
                            if (y < 9)
                                dtg[x, y + 1].Value = imlImagens.Images[16];

                            if (y > 0)
                                dtg[x + 1, y - 1].Value = imlImagens.Images[16];
                            if (y < 9)
                                dtg[x + 1, y + 1].Value = imlImagens.Images[16];

                            if (y > 0)
                                dtg[x + 2, y - 1].Value = imlImagens.Images[16];
                            if (y < 9)
                                dtg[x + 2, y + 1].Value = imlImagens.Images[16];

                            if (y > 0)
                                dtg[x + 3, y - 1].Value = imlImagens.Images[16];
                            if (y > 0)
                                dtg[x + 3, y + 1].Value = imlImagens.Images[16];
                            if (x < 6)
                                dtg[x + 4, y].Value = imlImagens.Images[16];
                        }
                        else
                        if (direcao == 1) //cima
                        {
                            if (x > 0)
                                dtg[x - 1, y].Value = imlImagens.Images[16];
                            if (y > 0)
                                dtg[x, y - 1].Value = imlImagens.Images[16];
                            if (x < 9)
                                dtg[x + 1, y].Value = imlImagens.Images[16];

                            if (x > 0)
                                dtg[x - 1, y + 1].Value = imlImagens.Images[16];
                            if (x < 9)
                                dtg[x + 1, y + 1].Value = imlImagens.Images[16];

                            if (x > 1)
                                dtg[x - 1, y + 2].Value = imlImagens.Images[16];
                            if (x < 9)
                                dtg[x + 1, y + 2].Value = imlImagens.Images[16];

                            if (x > 0)
                                dtg[x - 1, y + 3].Value = imlImagens.Images[16];
                            if (y < 6)
                                dtg[x, y + 4].Value = imlImagens.Images[16];
                            if (x < 9)
                                dtg[x + 1, y + 3].Value = imlImagens.Images[16];
                        }
                        else
                        if (direcao == 2) //esquerda
                        {
                            if (y > 0)
                                dtg[x, y - 1].Value = imlImagens.Images[16];
                            if (y < 9)
                                dtg[x, y + 1].Value = imlImagens.Images[16];
                            if (x < 9)
                                dtg[x + 1, y].Value = imlImagens.Images[16];

                            if (y > 0)
                                dtg[x - 1, y - 1].Value = imlImagens.Images[16];
                            if (y < 9)
                                dtg[x - 1, y + 1].Value = imlImagens.Images[16];

                            if (y > 0)
                                dtg[x - 2, y - 1].Value = imlImagens.Images[16];
                            if (y < 9)
                                dtg[x - 2, y + 1].Value = imlImagens.Images[16];

                            if (y > 0)
                                dtg[x - 3, y - 1].Value = imlImagens.Images[16];
                            if (y < 9)
                                dtg[x - 3, y + 1].Value = imlImagens.Images[16];
                            if (x > 3)
                                dtg[x - 4, y].Value = imlImagens.Images[16];

                        }
                        else
                        if (direcao == 3) //baixo
                        {
                            if (x > 0)
                                dtg[x - 1, y].Value = imlImagens.Images[16];
                            if (y < 9)
                                dtg[x, y + 1].Value = imlImagens.Images[16];
                            if (x < 9)
                                dtg[x + 1, y].Value = imlImagens.Images[16];

                            if (x > 0)
                                dtg[x - 1, y - 1].Value = imlImagens.Images[16];
                            if (x < 9)
                                dtg[x + 1, y - 1].Value = imlImagens.Images[16];

                            if (x > 0)
                                dtg[x - 1, y - 2].Value = imlImagens.Images[16];
                            if (x < 9)
                                dtg[x + 1, y - 2].Value = imlImagens.Images[16];

                            if (x > 0)
                                dtg[x - 1, y - 3].Value = imlImagens.Images[16];
                            if (y > 3)
                                dtg[x, y - 4].Value = imlImagens.Images[16];
                            if (x < 9)
                                dtg[x + 1, y - 3].Value = imlImagens.Images[16];
                        }
                        else
                        if (tam == 5)
                            if (direcao == 0) //esquerda
                            {
                                if (x > 0)
                                    dtg[x - 1, y].Value = imlImagens.Images[16];
                                if (y > 0)
                                    dtg[x, y - 1].Value = imlImagens.Images[16];
                                if (y < 9)
                                    dtg[x, y + 1].Value = imlImagens.Images[16];

                                if (y > 0)
                                    dtg[x + 1, y - 1].Value = imlImagens.Images[16];
                                if (y < 9)
                                    dtg[x + 1, y + 1].Value = imlImagens.Images[16];

                                if (y > 0)
                                    dtg[x + 2, y - 1].Value = imlImagens.Images[16];
                                if (y < 9)
                                    dtg[x + 2, y + 1].Value = imlImagens.Images[16];

                                if (y > 0)
                                    dtg[x + 3, y - 1].Value = imlImagens.Images[16];
                                if (y < 9)
                                    dtg[x + 3, y + 1].Value = imlImagens.Images[16];

                                if (y > 0)
                                    dtg[x + 4, y - 1].Value = imlImagens.Images[16];
                                if (y < 9)
                                    dtg[x + 4, y + 1].Value = imlImagens.Images[16];
                                if (x < 5)
                                    dtg[x + 5, y].Value = imlImagens.Images[16];
                            }
                            else
                            if (direcao == 1) //cima
                            {
                                if (x > 0)
                                    dtg[x - 1, y].Value = imlImagens.Images[16];
                                if (y > 0)
                                    dtg[x, y - 1].Value = imlImagens.Images[16];
                                if (x < 9)
                                    dtg[x + 1, y].Value = imlImagens.Images[16];

                                if (x > 0)
                                    dtg[x - 1, y + 1].Value = imlImagens.Images[16];
                                if (x < 9)
                                    dtg[x + 1, y + 1].Value = imlImagens.Images[16];

                                if (x > 0)
                                    dtg[x - 1, y + 2].Value = imlImagens.Images[16];
                                if (x < 9)
                                    dtg[x + 1, y + 2].Value = imlImagens.Images[16];

                                if (x > 0)
                                    dtg[x - 1, y + 3].Value = imlImagens.Images[16];
                                if (x < 9)
                                    dtg[x + 1, y + 3].Value = imlImagens.Images[16];

                                if (x > 0)
                                    dtg[x - 1, y + 4].Value = imlImagens.Images[16];
                                if (x < 9)
                                    dtg[x + 1, y + 4].Value = imlImagens.Images[16];
                                if (y < 5)
                                    dtg[x, y + 5].Value = imlImagens.Images[16];
                            }
                            else
                            if (direcao == 2) //direita
                            {
                                if (y > 0)
                                    dtg[x, y - 1].Value = imlImagens.Images[16];
                                if (y < 9)
                                    dtg[x, y + 1].Value = imlImagens.Images[16];
                                if (x < 9)
                                    dtg[x + 1, y].Value = imlImagens.Images[16];

                                if (y > 0)
                                    dtg[x - 1, y - 1].Value = imlImagens.Images[16];
                                if (y < 9)
                                    dtg[x - 1, y + 1].Value = imlImagens.Images[16];

                                if (y > 0)
                                    dtg[x - 2, y - 1].Value = imlImagens.Images[16];
                                if (y < 9)
                                    dtg[x - 2, y + 1].Value = imlImagens.Images[16];

                                if (y > 0)
                                    dtg[x - 3, y - 1].Value = imlImagens.Images[16];
                                if (y < 9)
                                    dtg[x - 3, y + 1].Value = imlImagens.Images[16];

                                if (y > 0)
                                    dtg[x - 4, y - 1].Value = imlImagens.Images[16];
                                if (y < 9)
                                    dtg[x - 4, y + 1].Value = imlImagens.Images[16];
                                if (x > 4)
                                    dtg[x - 5, y].Value = imlImagens.Images[16];
                            }
                            else
                            if (direcao == 3) //baixo
                            {
                                if (x > 0)
                                    dtg[x - 1, y].Value = imlImagens.Images[16];
                                if (y < 9)
                                    dtg[x, y + 1].Value = imlImagens.Images[16];
                                if (x < 9)
                                    dtg[x + 1, y].Value = imlImagens.Images[16];

                                if (x > 0)
                                    dtg[x - 1, y - 1].Value = imlImagens.Images[16];
                                if (x < 9)
                                    dtg[x + 1, y - 1].Value = imlImagens.Images[16];

                                if (x > 0)
                                    dtg[x - 1, y - 2].Value = imlImagens.Images[16];
                                if (x < 9)
                                    dtg[x + 1, y - 2].Value = imlImagens.Images[16];

                                if (x > 0)
                                    dtg[x - 1, y - 3].Value = imlImagens.Images[16];
                                if (x < 9)
                                    dtg[x + 1, y - 3].Value = imlImagens.Images[16];

                                if (x > 0)
                                    dtg[x - 1, y - 4].Value = imlImagens.Images[16];
                                if (y > 4)
                                    dtg[x, y - 5].Value = imlImagens.Images[16];
                                if (x < 9)
                                    dtg[x + 1, y - 4].Value = imlImagens.Images[16];
                            }
        }

        private void PreencherAoRedor(DataGridView dtg, int x, int y)
        {
            if (x > 1 && matrizAtaque[x - 2, y] == "1.1.1")
                dtg[x - 1, y].Value = imlImagens.Images[16];

            if (y > 1 && matrizAtaque[x, y - 1] == "1.1.1")
                dtg[x, y - 1].Value = imlImagens.Images[16];

            if (x <= 9 && matrizAtaque[x, y] == "1.1.1")
                dtg[x + 1, y].Value = imlImagens.Images[16];

            if (y <= 9 && matrizAtaque[x, y + 1] == "1.1.1")
                dtg[x, y + 1].Value = imlImagens.Images[16];
        }

        private void alertaLabel(Label lbEmQuestao)
        {
            Random rand = new Random();
            for (int i = 0; i <= 100; i++)
            {
                int A = rand.Next(0, 255);
                int R = rand.Next(0, 255);
                int G = rand.Next(0, 255);
                int B = rand.Next(0, 255);
                lbEmQuestao.ForeColor = Color.FromArgb(A, R, G, B);
                lbEmQuestao.Refresh();
            }
        }

        private void dtgAtaque_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Thread t;
            if (vezDoBot)
            {
                return;
            }

            vezDoBot = true;

            if (e.ColumnIndex == 0 || e.ColumnIndex > 10)
                return;

            int x = e.ColumnIndex;
            int y = e.RowIndex;

            if (matrizAtaque[x - 1, y] == "7.7.7" || matrizAtaque[x - 1, y] == "6.6.6")
            {
                vezDoBot = false;
                return;
            }

            int tem = this.verPosicao(matrizAtaque, x - 1, y);

            if (tem == 1)
            {
                dtgAtaque[x, y].Value = imlImagens.Images[15];
                if (this.matrizAtaque[x - 1, y] != "6.6.6")
                {
                    Thread s = new Thread(this.jogadorAcertouNavioTocarSom);
                    s.Start();
                    if (derrubouNavio(matrizAtaque, x - 1, y))
                    {
                        int direcao = Convert.ToInt16(matrizAtaque[x - 1, y].Substring(4, 1));

                        int tam = Convert.ToInt16(matrizAtaque[x - 1, y].Substring(0, 1));

                        int pos = Convert.ToInt16(matrizAtaque[x - 1, y].Substring(2, 1));

                        int inicioX = x;
                        int inicioY = y;

                        if (direcao == 0)
                            inicioX = x - pos + 1;
                        else
                            if (direcao == 1)
                            inicioY = y - pos + 1;
                        else
                            if (direcao == 2)
                            inicioX = x + pos - 1;
                        else
                            if (direcao == 3)
                            inicioY = y + pos - 1;

                        //isso tem q vir dps de colocar 6.6.6  
                        Label lbEmQuestao = new Label();

                        if (tam == 2)
                        {
                            lbSub.Text = Convert.ToString(Convert.ToInt16(lbSub.Text) - 1);
                            lbEmQuestao = lbSub;
                        }
                        else
                        if (tam == 3)
                        {
                            lbCruz.Text = Convert.ToString(Convert.ToInt16(lbCruz.Text) - 1);
                            lbEmQuestao = lbCruz;
                        }
                        else
                        if (tam == 4)
                        {
                            lbEncou.Text = Convert.ToString(Convert.ToInt16(lbEncou.Text) - 1);
                            lbEmQuestao = lbEncou;
                        }
                        else
                        if (tam == 5)
                        {
                            lbPortaAvi.Text = "0";
                            lbEmQuestao = lbPortaAvi;
                        }
                        
                        
                        lbEmQuestao.ForeColor = Color.Red;
                        lbEmQuestao.Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
                        lbEmQuestao.Refresh();
                        Thread.Sleep(1000);
                        lbEmQuestao.ForeColor = Color.White;
                        lbEmQuestao.Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);
                        lbEmQuestao.Refresh();
                    }

                    this.matrizAtaque[x - 1, y] = "6.6.6";

                    this.qtdeNaviosOponente--;

                    if (qtdeNaviosOponente == 0)
                    {
                        dtgAtaque.Enabled = false;

                        fimDeJogo fim = new fimDeJogo(true, this.pais, this.nivel, soma, ehOCerto, this.nav, this.estaTocando);
                        fim.Show();
                        this.temp = false;
                        Close();
                        return;
                    }
                }
            }
            else
            {
                dtgAtaque[x, y].Value = imlImagens.Images[16];
                Thread s = new Thread(this.jogadorAcertouAguaTocarSom);
                s.Start();
                this.matrizAtaque[x - 1, y] = "7.7.7";
            }

            this.dtgAtaque.Refresh();
            if (this.nivel == 1)
                t = new Thread(this.jogadaDeBot);
            else
                t = new Thread(this.jogadaDeBotNivel2);
            t.Start();

            dtgAtaque.Enabled = true;
        }

        private int verPosicao(String[,] mat, int x, int y)
        {
            if (x < 0 || x > 9 || y < 0 || y > 9)
                throw new Exception("Valor inválido !!");

            if (mat[x, y] == "0.0.0" || mat[x, y] == "1.1.1" || mat[x, y] == "7.7.7")
                return 0;
            else
                return 1;
        }

        private void btnProx_Click(object sender, EventArgs e)
        {
            //   if (clicou)
            // {
            //      this.Controls.Remove(pbxInfo);
            ////      this.Controls.Remove(btnProx);
            //     this.Controls.Remove(btnVoltar);
            //  }
            // clicou = true;
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
                //pbxInfo.Image = global::pTesteBatalhaNaval.Properties.Resources.Screenshot_1;
                //clicou = false;
            }
        }

        private void frmSingle_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (pFundo != null)
            {
                pFundo.Dispose();
                pFundo.Stop();
            }

            if (this.temp)
                this.nav.Close();
        }

        private void dtgCampo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > 0 && e.RowIndex >= 0)
                MessageBox.Show(matriz[e.ColumnIndex - 1, e.RowIndex].ToString());
        }

        private void VerLabels(bool ver)
        {
            this.lbbarra1.Visible = ver;
            this.lbBarra2.Visible = ver;
            this.lbBarra3.Visible = ver;
            this.lbBarra4.Visible = ver;
            this.lbCruz.Visible = ver;
            this.lbCruzadores.Visible = ver;
            this.lbEncou.Visible = ver;
            this.lbencouracados.Visible = ver;
            this.lbPortaAvi.Visible = ver;
            this.lbportaAvioes.Visible = ver;
            this.lbSubmarinos.Visible = ver;
            this.lbSub.Visible = ver;

            this.pbxSubmarino.Visible = ver;
            this.pbxCruzador.Visible = ver;
            this.pbxEncouracado.Visible = ver;
            this.pbxPorta.Visible = ver;

            this.lbBarra5.Visible = ver;
            this.lbBarra6.Visible = ver;
            this.lbBarra7.Visible = ver;
            this.lbBarra8.Visible = ver;

            this.lbSubarinos2.Visible = ver;
            this.lbCruzadores2.Visible = ver;
            this.lbEncouracados2.Visible = ver;
            this.lbPortaAvioes2.Visible = ver;

            this.lbQtdeSubarinos2.Visible = ver;
            this.lbQtdCruzadores2.Visible = ver;
            this.lbQtdEncouracados2.Visible = ver;
            this.lbQtdPortaavioes2.Visible = ver;

            this.pbxSubmarinos2.Visible = ver;
            this.pbxCruzadores2.Visible = ver;
            this.pbxEncouracado2.Visible = ver;
            this.pbxPorta2.Visible = ver;

        }

        private void button1_Click(object sender, EventArgs e) //btnIniciar
        {
            this.estaInserindo = false;
            this.btnIniciar.Visible = false;
            this.ControleDosBotoesUpgrade(false, false, "Inserir Manualmente", "Inserir Aleatoriamente");
            this.VerBotoesDeInsersao(false);
            this.VerLabels(true);
            this.dtgAtaque.Enabled = true;
            this.RecarregarDtg(dtgAtaque, false, paisAdv);

            bool temTodos = false;
            while (!temTodos)
            {
                temTodos = true;
                for (int i = 0; i < 4; i++)
                    this.colocarDe2(matrizAtaque);

                for (int i = 0; i < 3; i++)
                    this.colocarDe3(matrizAtaque);

                if (!this.temEspaco(matrizAtaque, 4))
                {
                    this.InicializaNavios(matrizAtaque);
                    temTodos = false;
                    continue;
                }

                if (!this.temEspaco(matrizAtaque, 4))
                {
                    this.InicializaNavios(matrizAtaque);
                    temTodos = false;
                    continue;
                }

                if (!this.temEspaco(matrizAtaque, 5))
                {
                    this.InicializaNavios(matrizAtaque);
                    temTodos = false;
                }
            }


            this.qtdeNaviosMeus = 30;
            this.qtdeNaviosOponente = 30;

            this.Controls.Remove(pbxDe2);
            this.Controls.Remove(pbxDe3);
            this.Controls.Remove(pbxDe4);
            this.Controls.Remove(pbxDe5);
            this.Controls.Remove(labelDe2);
            this.Controls.Remove(labelDe3);
            this.Controls.Remove(labelDe4);


            btnInserir.Visible = false;
            btnAleatorio.Visible = false;

            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Height = 655;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.Animacaozinha("Você começa atacando contra " + paisAdv);
        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {
            fimDeJogo fim = new fimDeJogo(true, this.pais, this.nivel, soma, ehOCerto, this.nav, estaTocando);
            fim.Show();
            this.temp = false;
            Close();
            return;
        }

        private void lblJogador_Click(object sender, EventArgs e)
        {
            fimDeJogo fim = new fimDeJogo(false, this.pais, this.nivel, soma, ehOCerto, this.nav, estaTocando);
            fim.Show();
            this.temp = false;
            Close();
            return;
        }

        private String[] formadorDePosicoes()
        {
            int x = Convert.ToInt16(this.posicaoAtual.Substring(0, 1));
            int y = Convert.ToInt16(this.posicaoAtual.Substring(1, 1));

            int direcao = Convert.ToInt16(this.matriz[x, y].Substring(4, 1));

            int tam = Convert.ToInt16(this.matriz[x, y].Substring(0, 1));

            int pos = Convert.ToInt16(this.matriz[x, y].Substring(2, 1));

            String[] posicoes = new String[tam];

            if (tam > 5 || tam < 2)
                return null;

            if (direcao == 0) //esquerda
            {
                int inicio = x - pos + 1;
                for (int i = inicio; i < inicio + tam; i++)
                    posicoes[i - inicio] = this.matriz[i, y];
            }

            if (direcao == 1) //cima
            {
                int inicio = y - pos + 1;
                for (int i = inicio; i < inicio + tam; i++)
                    posicoes[i - inicio] = this.matriz[x, i];
            }

            if (direcao == 2) //direita
            {
                int inicio = x + pos - 1;
                int indice = 0;
                for (int i = inicio; i > inicio - tam; i--)
                {
                    posicoes[indice] = this.matriz[i, y];
                    indice++;
                }
            }

            if (direcao == 3) //baixo
            {
                int inicio = y + pos - 1;
                int indice = 0;
                for (int i = inicio; i > inicio - tam; i--)
                {
                    posicoes[indice] = this.matriz[x, i];
                    indice++;
                }
            }

            return posicoes;

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (estaInserindo)
            {
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (posicaoAtual == null)
                return;
            int x = Convert.ToInt16(this.posicaoAtual.Substring(0, 1));
            int y = Convert.ToInt16(this.posicaoAtual.Substring(1, 1));

            this.movimentarONavio(1, this.formadorDePosicoes(), x, y);
            this.refreshDtg();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (posicaoAtual == null)
                return;
            int x = Convert.ToInt16(this.posicaoAtual.Substring(0, 1));
            int y = Convert.ToInt16(this.posicaoAtual.Substring(1, 1));

            this.movimentarONavio(0, this.formadorDePosicoes(), x, y);
            this.refreshDtg();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (posicaoAtual == null)
                return;
            int x = Convert.ToInt16(this.posicaoAtual.Substring(0, 1));
            int y = Convert.ToInt16(this.posicaoAtual.Substring(1, 1));

            this.movimentarONavio(3, this.formadorDePosicoes(), x, y);
            this.refreshDtg();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (posicaoAtual == null)
                return;
            int x = Convert.ToInt16(this.posicaoAtual.Substring(0, 1));
            int y = Convert.ToInt16(this.posicaoAtual.Substring(1, 1));

            this.movimentarONavio(2, this.formadorDePosicoes(), x, y);
            this.refreshDtg();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (posicaoAtual == null)
                return;
            this.ConfirmarNavio();
            this.refreshDtg();
        }

        private void AnimarBotao(Button b)
        {
            for (int i = 222; i <= 255; i++)
            {
                b.BackColor = Color.FromArgb(i, i, i);
                b.Refresh();
                this.Refresh();
                Thread.Sleep(10);
            }
        }

        private void btnGirar_Click(object sender, EventArgs e)
        {
            if (posicaoAtual == null)
                return;
            int x = Convert.ToInt16(this.posicaoAtual.Substring(0, 1));
            int y = Convert.ToInt16(this.posicaoAtual.Substring(1, 1));

            this.GirarONavio(x, y);
            this.refreshDtg();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.Backspace();
            this.refreshDtg();
        }

        private void btnDeCima_Click(object sender, EventArgs e)
        {
            if (this.btnDeCima.Text == "Inserir Aleatoriamente")
                this.btnAleatorio.PerformClick();
            else
            if (this.btnDeCima.Text == "Inserir Frota")
                this.btnInserir.PerformClick();
            else
                this.CompletarFrota();
        }

        private void btnDeBaixo_Click(object sender, EventArgs e)
        {
            this.btnAleatorio.PerformClick();
            this.Controls.Add(btnAleatorio);
            btnAleatorio.Visible = true;
            this.Controls.Add(btnInserir);
            btnInserir.Visible = true;
        }

        private void pbxAudio_Click(object sender, EventArgs e)
        {
            if (estaTocando)
            {
                pbxAudio.BackgroundImage = Image.FromFile("../imagens/loudspeakerMute.png");
                pFundo.Stop();
                pFundo.Dispose();
                estaTocando = false;

            }
            else
            {
                pFundo = new SoundPlayer();
                pFundo.SoundLocation = "../audios/a juntar/Betrayal Desolation.wav";
                pFundo.PlayLooping();
                estaTocando = true;
                pbxAudio.BackgroundImage = Image.FromFile("../imagens/loudspeaker.png");
            }
        }

        private void Animacaozinha(String text)
        {
            lbAnimacazinha.Visible = true;

            lbAnimacazinha.Text = text;
            this.AnimarOLabel();
            Thread.Sleep(2000);

            lbAnimacazinha.Visible = false;
            lbAnimacazinha.Location = new Point(1178, lbAnimacazinha.Location.Y);

            this.Refresh();
        }

        private void AnimarOLabel()
        {
            this.lbAnimacazinha.Visible = true;

            while(lbAnimacazinha.Location.X+lbAnimacazinha.Width>dtgAtaque.Width+dtgAtaque.Location.X)
            {
                lbAnimacazinha.Location = new Point(lbAnimacazinha.Location.X - 2, lbAnimacazinha.Location.Y);
                lbAnimacazinha.Refresh();
                Thread.Sleep(10);
            }
        }

        private void AjustarLabel(String text)
        {
            this.lbAnimacazinha.Text = text;
            this.lbAnimacazinha.Visible = true;
            this.lbAnimacazinha.Location = new Point(dtgAtaque.Width + dtgAtaque.Location.X-lbAnimacazinha.Width, lbAnimacazinha.Location.Y);
            this.lbAnimacazinha.Refresh();
            this.lbAnimacazinha.Visible = false;
        }

        private void frmSingle_KeyUp(object sender, KeyEventArgs e)
        {
            if (estaInserindo)
            {
                if (e.KeyCode.Equals(Keys.Back))
                    this.Backspace();
                else
                if (posicaoAtual != null)
                {

                    int x = Convert.ToInt16(this.posicaoAtual.Substring(0, 1));
                    int y = Convert.ToInt16(this.posicaoAtual.Substring(1, 1));

                    if (e.KeyCode.Equals(Keys.Left))
                        this.movimentarONavio(0, this.formadorDePosicoes(), x, y);
                    else
                    if (e.KeyCode.Equals(Keys.Up))
                        this.movimentarONavio(1, this.formadorDePosicoes(), x, y);
                    else
                    if (e.KeyCode.Equals(Keys.Right))
                        this.movimentarONavio(2, this.formadorDePosicoes(), x, y);
                    else
                    if (e.KeyCode.Equals(Keys.Down))
                        this.movimentarONavio(3, this.formadorDePosicoes(), x, y);
                    else
                    if (e.KeyCode.Equals(Keys.Space))
                        this.GirarONavio(x, y);
                    else
                    if (e.KeyCode.Equals(Keys.Enter))
                        this.ConfirmarNavio();
                }
                this.refreshDtg();
            }
        }
    } 
}
