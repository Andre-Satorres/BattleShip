using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Media;

namespace pTesteBatalhaNaval
{
    public partial class frmMulti : Form
    {
        // Nome nao pode aceitar ':'.
        // Espectar partidas.
        // Opcao revanche ou sair.
        private String[,] matriz = new String[10, 10];
        private String[,] matrizAtaque = new String[10, 10];
        private String[] posicoesQueAntesTinhaNavio = new String[100];
        private String[] naviosInseridos;
        private String posicaoAtual;
        private String indicesInseridos = "";
        private int ultimoIndexInserido = 0;
        private int qtdNaviosDe2Inseridos = 0;
        private int qtdNaviosDe3Inseridos = 0;
        private int qtdNaviosDe4Inseridos = 0;
        private int tamanhoUlt = 0;
        private int indicePosicoesQueAntesTinha = 0;
        private int qtdeNaviosMeus = 30;
        private int qtdeNaviosOponente = 30;

        private bool pronto = false;
        private bool vezDoOponente = false;
        private bool estaInserindo = false;
        private bool estaInserindoNoMomento = false;
        private bool inseriuODe5 = false;

        private static readonly Random random = new Random();
        private static object synclock = new object();

        private Image imagem = Image.FromFile("../../../../imagens/black.png");

        private Label labelDe2;
        private Label labelDe3;
        private Label labelDe4;
        private Label labelDe5;

        private PictureBox pbxDe2;
        private PictureBox pbxDe3;
        private PictureBox pbxDe4;
        private PictureBox pbxDe5;

        private SoundPlayer player1;
        private SoundPlayer player2;

        private Socket s = null;
        private frmNaval nav;
        private String nick;
        private String nickOponente="";
        private String nomeSala;
        private int tempo;
        private Socket[] maxSocket = new Socket[10000];
        private AsyncCallback pfnWorkerCallBack;
        private int qtdJogadores = 1;
        private int qtdCliente = 0;
        // Lista de imagens.
        private List<Image> li = new List<Image>();
        // Controle de imagens na tela.
        private int qualImagem = 1;
        private bool cliente=false;
        private int iRx = 1;
        private bool temp = true;

        public frmMulti()
        {
            InitializeComponent();
        }

        public frmMulti(Socket skt, String nickJogador, String nomeS, frmNaval naval, bool c)
        {
            InitializeComponent();
            this.s = skt;
            this.nick = nickJogador;
            this.lblNick.Text = this.nick;
            this.nomeSala = nomeS;
            this.nav = naval;
            this.cliente = c;

            if (c)
            {
                // Nao existe nome da sala, ja que quem abriu o formulario eh um usuario.
                this.nomeSala = "";
                // Recebendo o nick do oponente, que no caso, seria o nick de quem criou o servidor.
                this.nickOponente = nomeS;

                this.vezDoOponente = true;
            }
        }

        private void frmMulti_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Verificando se eh necessario chamar invoke.
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<object, FormClosedEventArgs>(frmMulti_FormClosed), new object[] { sender, e });
                return;
            }
            else
            {
                if (this.temp)
                {
                    // Verificando quem saiu da partida.
                    if (this.cliente)
                        // Enviando o simbolo '-' para que o servidor identifique que o oponente saiu e assim recomecar um jogo novo mais tarde.
                        this.enviar("-");
                    else
                        // Enviando o simbolo '-' para demonstrar que o servidor agora esta offline.
                        this.enviarDados("-");

                    // Fechar formulario frmNaval.
                    this.nav.Close();
                }
            }
        }

        private void frmMulti_Load(object sender, EventArgs e)
        {
            if (!this.cliente)
            {
                try
                {
                    // Adicionando todas as imagens em uma lista, para que nao seja necessario busca-las de novo.
                    for (int i = 1; i < 8; i++)
                        this.li.Add(Image.FromFile("../../../../imagens/selecao" + Convert.ToString(i) + ".jpg"));

                    // Comeco do jogo, ficar na selecao de imagens, para esperar um jogador.
                    this.tmrTempo.Interval = 5000;
                    this.tmrTempo.Enabled = true;

                    this.s.Listen(4);
                    this.s.BeginAccept(new AsyncCallback(acaoClienteConectado), null);
                }
                catch (SocketException)
                { }
            }
            else
            if (this.cliente)
            {
                // Preparar recebimento de dados.
                this.enviar("(");
                // Envio do nome ao servidor.
                this.enviar(this.nick + "+");
                // Voltar ao normal a recepcao.
                this.enviar(")");

                // Adicionando todas as imagens em uma lista, para que nao seja necessario busca-las de novo.
                for (int i = 1; i < 8; i++)
                    this.li.Add(Image.FromFile("../../../../imagens/selecao" + Convert.ToString(i) + ".jpg"));

                // Comeco do jogo, ficar na selecao de imagens, para esperar um jogador.
                this.tmrTempo.Interval = 5000;
                this.tmrTempo.Enabled = true;

                // Esperar por dados do servidor.
                this.esperarPorDados(this.s);

                //Colocando focus.
                this.txtMensagem.Focus();
            }
        }

        private void acaoClienteConectado(IAsyncResult iar)
        {
            try
            {
                // Aqui nos complementamos/finalizamos o BeginAcceps assyncronos
                // chamando EndAccept() = o qual retorna uma referencia
                // espera um novo socket.
                this.maxSocket[this.qtdCliente] = this.s.EndAccept(iar);
                // Agora deixaremos o workerSocket fazer o processamento futuro
                // do cliente recem conectado.
                esperarPorDados(this.maxSocket[this.qtdCliente]);

                // Incrementa o contador de clientes;
                ++this.qtdCliente;

                // Desde que o Socket principal esta livre para tratar novas conexões
                // vamos voltar a tentar tratar essas conexões.
                this.s.BeginAccept(new AsyncCallback(acaoClienteConectado), null);
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\n acaoClienteConectado: Socket foi fechada \n");
            }
            catch (SocketException)
            {}
        }

        private void esperarPorDados(Socket soc)
        {
            try
            {
                if (pfnWorkerCallBack == null)
                    // Especifica a callback a ser chamada quando qualquer
                    // atividade de escrita for detectada no socket cliente conectado.
                    pfnWorkerCallBack = new AsyncCallback(acaoDadosRecebidos);

                SocketPacket sktPct;
                
                if (this.iRx != 1)
                    sktPct = new SocketPacket(this.iRx);
                else
                    sktPct = new SocketPacket(1);

                sktPct.socketAtual = soc;

                soc.BeginReceive(sktPct.databuffer, 0, sktPct.databuffer.Length, SocketFlags.None, pfnWorkerCallBack, sktPct);
            }
            catch (Exception)
            {}
        }

        private void acaoDadosRecebidos(IAsyncResult asyn)
        {
            try
            {
                SocketPacket socketData = socketData = (SocketPacket)asyn.AsyncState;

                int k = 0;
                // A execução completa de BeginReceive() chamada assincronamente
                // por EndReceive() retorna o número de caraceteres escritos no
                // stream pelo cliente.
                k = socketData.socketAtual.EndReceive(asyn);
                char[] chars = new Char[k];
                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charlen = d.GetChars(socketData.databuffer, 0, k, chars, 0);
                
                String szData = new String(chars);

                // Verificacao dos ultimos caracteres da string para realizar comandos.
                // A leitura no caso quando szData eh diferente de null (szData.Length = 0).
                if (szData.Length >= 1)
                {
                    if (szData[szData.Length - 1] == '!')
                        // Pedido do nome da sala.
                        this.enviarDados(this.nomeSala);
                    else
                    if (szData[szData.Length - 1] == '@')
                        // Pedido da informacao do tempo de partida.
                        this.enviarDados(this.Converter(this.tempo));
                    else
                    if (szData[szData.Length - 1] == '#')
                        // Pedido de quantidade de jogadores.
                        this.enviarDados(Convert.ToString(this.qtdJogadores));
                    else
                    if (szData[szData.Length - 1] == '`')
                        // Pedido do nome deste usuario.
                        this.enviarDados(this.nick);
                    else
                    if (szData[szData.Length - 1] == '&')
                        // Pedido do nome do oponente deste usuario.
                        this.enviarDados(this.nickOponente);
                    else
                    if (szData[szData.Length - 1] == '$')
                        // Pedido de nome do oponente.
                        this.enviarDados(this.nick);
                    else
                    if (szData[szData.Length - 1] == '%')
                        // Metodo para colocar no text box.
                        this.appendChat(szData.Remove(szData.Length - 1, 1));
                    else
                    if (szData[szData.Length - 1] == '(')
                        this.iRx = 21;
                    else
                    if (szData[szData.Length - 1] == ')')
                        this.iRx = 1;
                    else
                    if (szData[szData.Length - 1] == '{')
                        this.iRx = 73;
                    else
                    if (szData[szData.Length - 1] == '}')
                        this.iRx = 1;
                    else
                    if (szData[szData.Length - 1] == ',')
                        enviarMatriz();
                    else
                    if (szData[szData.Length - 1] == '?')
                        mostrarPronto();
                    else
                    if (szData[szData.Length - 1] == '*')
                    {
                        // Servidor aceitou a partida.
                        contagem();
                        habilitarControles(true);
                    }
                    else
                    if (szData[szData.Length - 1] == '=')
                        this.fecharForm();
                    else
                    if (szData[szData.Length - 1] == '+')
                    {
                        // Adicionar ao nome dele o valor passado menos o ultimo caracter.
                        this.nickOponente = szData.Remove(szData.Length - 1, 1);

                        // Um jogador entrou.
                        this.qtdJogadores++;

                        // Usando as variaveis da classe para animar. Imagens do servidor e do cliente.
                        this.animar();
                    }
                    else
                    if (szData[szData.Length - 1] == '-')
                    {
                        if (!this.cliente)
                        {
                            // Um jogador saiu.
                            this.qtdJogadores--;
                            // Fazer animacao ganhar e esperar por um novo jogador.
                            animacaoGanhar();
                            habilitarControles(false);
                        }
                        else
                        {
                            // Informar que o servidor esta offline agora.
                            animacaoGanhar();
                            // O OK do form deve voltar para o form anterior.
                        }
                    }
                    else
                    if (szData[szData.Length - 1] == '^')
                    {
                        // Fazer os metodos para defender, checar posicoes próprias.
                    }
                }

                esperarPorDados(socketData.socketAtual);
            }
            catch (SocketException)
            {}
        }

        private void enviarMatriz()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(enviarMatriz));
                return;
            }
            else
            {
                btnPronto.Text = "Iniciar";
                btnPronto.Enabled = true;
                btnPronto.PerformClick();
            }
        }

        private void mostrarPronto()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(mostrarPronto));
                return;
            }
            else
            {
                if (this.pronto)
                {
                    this.lblPronto.Hide();
                    this.btnPronto.Text = "Iniciar";
                    this.btnPronto.Enabled = true;
                }
                else
                    this.lblPronto.Show();
            }
        }

        private void contagem()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(contagem));
                return;
            }
            else
            {
                this.tmrTempo.Enabled = false;

                btnRejeitar.Hide();
                btnAceitar.Hide();
                lblCliente.Hide();
                lblCliente.Text = "Iniciando...";
                lblCliente.Location = new Point(450, 580);
                lblCliente.Font = new Font("Comic Sans MS", 30);
                lblCliente.Show();
                cpgCarregar.Hide();
                lblContagem.Show();
                Thread.Sleep(1000);
                lblContagem.Text = "2";
                Application.DoEvents();
                Thread.Sleep(1000);
                lblContagem.Text = "1";
                Application.DoEvents();
                Thread.Sleep(1000);
                lblContagem.Hide();
                lblContagem.Location = new Point(lblContagem.Location.X-40, lblContagem.Location.Y);
                lblContagem.Text = "GO";
                lblContagem.Show();
                Application.DoEvents();
                Thread.Sleep(1000);
                lblContagem.Hide();

                this.tmrTempo.Enabled = true;
            }
        }

        private void fecharForm()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(fecharForm));
                return;
            }
            else
            {
                // Voltando ao form da lista, retornando os mesmo valores passados para a criacao deste form.
                frmLista l = new frmLista(this.nick, this.nav);
                l.Show();

                // Usuario nao deseja fechar o programa totalmente.
                this.temp = false;
                Close();
            }
        }

        private void animacaoGanhar()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(animacaoGanhar));
                return;
            }
            else
            {
                
            }
        }

        private void animacaoPerder()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(animacaoPerder));
                return;
            }
            else
            {

            }
        }

        private void animar()
        {
            // Verificar se temos que usar um invoke.
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(animar));
                return;
            }
            else
            {
                this.tmrTempo.Enabled = false;

                // Setando o label para o nick do oponente.
                this.lblOponente.Text = this.nickOponente;

                this.lblComeco.Hide();
                this.cpgCarregar.Hide();

                this.lblComeco.Location = new Point(410, 38);
                this.lblComeco.Text = "Jogador encontrado!";
                this.lblComeco.Show();

                // Redefinindo a posicao do picture box para reanimar caso necessario.
                this.pbxUsuario.Location = new Point(0, 127);
                this.pbxOponente.Location = new Point(784, 127);

                this.pbxUsuario.Show();
                this.pbxOponente.Show();

                for (int i = 1; i < 117; i++)
                {
                    this.pbxUsuario.Location = new Point(this.pbxUsuario.Location.X + 1, this.pbxUsuario.Location.Y);
                    this.pbxOponente.Location = new Point(this.pbxOponente.Location.X - 1, this.pbxOponente.Location.Y);
                    Application.DoEvents();
                }

                this.pbxVS.Show();
                this.pbxVS.Refresh();

                this.lblNick.Location = new Point(117, 532);
                this.lblNick.Show();
                this.lblOponente.Location = new Point(667, 532);
                this.lblOponente.Show();

                if (!this.cliente)
                {
                    this.btnAceitar.Show();
                    this.btnRejeitar.Show();
                }
                else
                {
                    this.lblCliente.Show();
                    this.cpgCarregar.Location = new Point(750, 605);
                    this.cpgCarregar.Show();
                }

                this.tmrTempo.Enabled = true;
            }
        }

        private void habilitarChat ()
        {
            lblInfo.Show();
            lblChat.Show();
            txtChat.Show();
            txtMensagem.Show();
            btnEnviar.Show();
            txtMensagem.Focus();
        }

        private void habilitarControles (bool b)
        {
            // Verificar se temos que usar um invoke.
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<bool>(habilitarControles), new object[] { b });
                return;
            }
            else
            {
                if (b)
                {
                    // Escondo os temporizadores, ja que um usuario ja entrou.
                    lblCliente.Hide();
                    lblComeco.Hide();
                    cpgCarregar.Hide();
                    pbxUsuario.Hide();
                    pbxOponente.Hide();
                    pbxVS.Hide();
                    lblNick.Hide();
                    lblOponente.Hide();
                    btnAceitar.Hide();
                    btnRejeitar.Hide();

                    // Arrumando a posicao dos labels dos nicks.
                    lblNick.Location = new Point(10, 54);
                    lblOponente.Location = new Point(615, 54);

                    tmrTempo.Enabled = false;
                    tmrTempo.Interval = 1000;
                    this.tempo = 0;
                    if (!this.cliente)
                        tmrTempo.Enabled = true;

                    // Mostrando os campos que o usuario pode ter acesso durante o jogo.
                    lblTitulo.Show();
                    lblNick.Show();
                    lblOponente.Show();
                    dtgCampo.Show();
                    dtgAtaque.Show();
                    btnInserir.Show();
                    btnAleatorio.Show();

                    PreencherDtg(dtgCampo);
                    PreencherDtg(dtgAtaque);
                }
                else
                {
                    // Escondo os campos ja que um usuario saiu.
                    lblComeco.Text = "Esperando por um jogador...";
                    lblComeco.Location = new Point(341, 38);
                    pbxUsuario.Hide();
                    pbxOponente.Hide();
                    pbxVS.Hide();
                    btnAceitar.Hide();
                    btnRejeitar.Hide();
                    lblTitulo.Hide();
                    lblNick.Hide();
                    lblOponente.Hide();
                    dtgCampo.Hide();
                    dtgAtaque.Hide();
                    lblChat.Hide();
                    txtChat.Hide();
                    txtMensagem.Hide();
                    btnEnviar.Hide();

                    // Mostro os temporizadores, ja que um usuario saiu.
                    lblComeco.Show();
                    cpgCarregar.Show();

                    tmrTempo.Enabled = false;
                    tmrTempo.Interval = 5000;
                    this.tempo = 0;
                    tmrTempo.Enabled = true;
                }
            }
        }

        private void appendChat (String str)
        {
            // Caso o invoke seja necessario.
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<String>(appendChat), new object[] { str });
                return;
            }
            else
            {
                // Formatando a string para o usuario nao ficar com espaco no nickname.
                String[] s = str.Split(':');
                // Retirando os espacos, caso existente do nickname do usuario.
                str = s[0].Trim() + ": ";
                // Formatando a string, para caso o usuario enviar ':';
                String formatado="";
                if (s.Length == 2)
                    formatado = s[1].Trim();
                // Verficando se o usuario enviou ':' na sua mensagem.
                else
                {
                    formatado = s[1];

                    for (int i = 1; i < s.Length - 2; i++)
                        // Adicionando o ':' a string.
                        formatado += ":" + s[i];

                    formatado += ":" + s[s.Length - 1].Trim();
                }

                // Dando valor a string que vai ao combo box.
                str += formatado+"\n";
                // Mostrar texto no text box, refere-se a mensagens entre os jogadores.
                this.txtChat.AppendText(str);
            }
        }

        private String Converter(int valor)
        {
            String ret="";

            if (valor < 60)
            {
                if (valor < 10)
                    ret = "00:0" + Convert.ToString(valor);
                else
                if (valor >= 10)
                    ret = "00:" + Convert.ToString(valor);
            }
            else
            {
                String m = "";
                String s = "";

                int aux = 0;
                while (valor >= 60)
                {
                    aux++;
                    valor -= 60;
                }

                if (aux < 10)
                    m = "0" + Convert.ToString(aux);
                else
                    m = Convert.ToString(aux);

                if (valor < 10)
                    s = "0" + Convert.ToString(valor);
                else
                if (valor >= 10)
                    s = Convert.ToString(valor);

                ret = m + ":" + s;
            }

            return ret;
        }

        public class SocketPacket
        {
            public System.Net.Sockets.Socket socketAtual;
            public byte[] databuffer;

            // Setando o tamanho da recepcao de bytes.
            public SocketPacket(int quantidade)
            {
                this.databuffer = new byte[quantidade];
            }
        }

        private void enviarDados(String str)
        {
            try
            {
                // Metodo para enviar ao clinte conectado.
                Object objData = str;

                byte[] byData = System.Text.Encoding.ASCII.GetBytes(objData.ToString());

                for (int i = 0; i < this.qtdCliente; i++)
                    if (this.maxSocket[i] != null)
                        if (this.maxSocket[i].Connected)
                            this.maxSocket[i].Send(byData);
            }
            catch (SocketException)
            {}
        }

        private void enviar (String str)
        {
            try
            {
                // Enviando para o servidor o parametro recebido.
                this.s.Send(System.Text.Encoding.ASCII.GetBytes(str));
            }
            catch
            {}
        }

        private void tmrTempo_Tick(object sender, EventArgs e)
        {
            // Verificar qual imagem deve ser colocada.
            if (tmrTempo.Interval == 5000)
            {
                // Alterando a imagem do fundo, antes do jogador entrar.
                this.BackgroundImage = this.li[qualImagem];

                if (this.qualImagem == 6)
                    this.qualImagem = 0;
                else
                    this.qualImagem++;
            }
            else
                // Tempo de jogo nao pode passar de 20 minutos.
                // Fazer metodo depois.
                this.tempo++;
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            // Texto nao pode ser nulo. Evitando o spam.
            if (txtMensagem.Text.Trim() != "")
            {
                // Formantando o texto digitado.
                while (txtMensagem.Text.Length != 50)
                    txtMensagem.Text += " ";

                // Mostrando ao usuario que ele enviou a mensagem.
                this.appendChat(this.nick + ": " + txtMensagem.Text);

                if (!this.cliente)
                {
                    // Enviando abertura para receber 73 caracteres.
                    this.enviarDados("{");
                    // Devemos enviar 20+2+50+1 caracteres -> 73 caracteres.
                    this.enviarDados(this.nick + ": " + txtMensagem.Text + "%");
                    // Fechando abertura.
                    this.enviarDados("}");
                }
                else
                {
                    // Enviando abertura para receber 73 caracteres.
                    this.enviar("{");
                    // Devemos enviar 20+2+50+1 caracteres -> 73 caracteres.
                    this.enviar(this.nick + ": " + txtMensagem.Text + "%");
                    // Fechando abertura.
                    this.enviar("}");
                }

                // Resetar text box e colocar foco.
                this.txtMensagem.Text = "";
                this.txtMensagem.Focus();
            }
            else
            {
                // Resetando o texto e colocando o leitor nele.
                txtMensagem.Text = "";
                txtMensagem.Focus();
            }
        }

        private void txtMensagem_KeyDown(object sender, KeyEventArgs e)
        {
            // Verificando se o usuario pressionou enter no campo de digitar mensagem ao parceiro.
            if (e.KeyCode == Keys.Enter)
                // Caso ele tenha pressionado enter, entao enviar a mensagem solicitada para o oponente.
                this.btnEnviar.PerformClick();
        }

        private void btnRejeitar_Click(object sender, EventArgs e)
        {
            this.enviarDados("=");
            this.qtdJogadores--;
            this.habilitarControles(false);
        }

        private void btnAceitar_Click(object sender, EventArgs e)
        {
            if (this.qtdJogadores == 2)
            {
                // Mostrar animacao.
                this.contagem();
                // Habilitando os controles e comecando o jogo.
                this.habilitarControles(true);
                // Enviando para a liberacao do jogo.
                this.enviarDados("*");
            }
            else
                // Atualizando controles, ja que o usuario saiu do jogo.
                this.habilitarControles(false);
        }

        private void frmMulti_Shown(object sender, EventArgs e)
        {
            if (this.cliente)
                // Animando a entrada.
                this.animar();
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
            this.verBotoesDeInsersao(false);
            this.RecarregarDtg(dtgCampo, false);
            this.RecarregarDtg(dtgCampo, true);
            this.controleDosBotoesUpgrade(false, false, "Inserir Frota", "Inserir Aleatoriamente");
            this.btnPronto.Show();

            if (this.pbxDe2 != null)
            {
                this.pbxDe2.Visible = false;
                this.pbxDe3.Visible = false;
                this.pbxDe4.Visible = false;
                this.pbxDe5.Visible = false;
                this.verLabels(false);
                this.labelDe2.Visible = false;
                this.labelDe3.Visible = false;
                this.labelDe4.Visible = false;
                this.labelDe5.Visible = false;
            }
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            this.btnPronto.Hide();
            this.naviosInseridos = null;
            this.naviosInseridos = new String[10];
            this.indicesInseridos = "";
            this.RecarregarDtg(dtgCampo, false);
            this.estaInserindo = true;
            this.Controls.Remove(btnInserir);
            this.Controls.Remove(btnAleatorio);
            this.PrepararInsercao();
            this.verBotoesDeInsersao(true);
            this.controleDosBotoesUpgrade(true, true, "Completar Frota", "Inserir Aleatoriamente");
            this.colocarOPrimeiroNavio();
        }

        private void InicializarNavios(String[,] mat)
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    mat[i, j] = "0.0.0";
        }

        private void PreencherDtg(DataGridView dtg)
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++) //Percorre todas as células
                {
                    if (dtg.RowCount < 10)
                        dtg.Rows.Add();
                    dtg.Rows[i].Cells[0].Value = "      " + Convert.ToChar(i + 65);
                }

            for (int j = 0; j < 10; j++)
                for (int i = 0; i < 10; i++)
                    dtg[i + 1, j].Value = imagem;

            dtg.RowHeadersDefaultCellStyle.Padding = new Padding(dtg.RowHeadersWidth);
        }

        private void colocarOPrimeiroNavio()
        {
            this.estaInserindoNoMomento = true;
            this.matriz[4, 4] = "2.1.1";
            this.matriz[4, 5] = "2.2.1";
            this.refreshDtg();
            this.posicaoAtual = "44";
            this.tamanhoUlt = 2;
            this.preencherAoRedorNavio(this.matriz, 4, 4);
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

            btnPronto.Enabled = true;
        }

        private void ClickPbxDe2(object sender, EventArgs e)
        {
            if (this.estaInserindoNoMomento == false && this.qtdNaviosDe2Inseridos < 4)
                this.colocarONavioNaTela(2);
        }

        private void ClickPbxDe3(object sender, EventArgs e)
        {
            if (this.estaInserindoNoMomento == false && this.qtdNaviosDe3Inseridos < 3)
                this.colocarONavioNaTela(3);
        }

        private void ClickPbxDe4(object sender, EventArgs e)
        {
            if (this.estaInserindoNoMomento == false && this.qtdNaviosDe4Inseridos < 2)
                this.colocarONavioNaTela(4);
        }

        private void ClickPbxDe5(object sender, EventArgs e)
        {
            if (this.estaInserindoNoMomento == false && !this.inseriuODe5)
                this.colocarONavioNaTela(5);
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

        private bool ehNavio(String s)
        {
            if (s.Equals("0.0.0") || s.Equals("1.1.1") || s.Equals("7.7.7"))
                return false;

            return true;
        }

        private bool temNavioNoMeio(int tam)
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

        private void colocarONavioNaTela(int tam)
        {
            if (tam == 2)
            {
                if (this.TemNavioAoredor(this.matriz, 4, 4, 2) || this.temNavioNoMeio(2))
                {
                    MessageBox.Show("Já há um navio no meio!! Por favor, o retire");
                    return;
                }

                this.estaInserindoNoMomento = true;
                this.matriz[4, 4] = "2.1.1";
                this.matriz[4, 5] = "2.2.1";
                this.refreshDtg();
                this.posicaoAtual = "44";
                this.tamanhoUlt = 2;
                this.preencherAoRedorNavio(this.matriz, 4, 4);
            }
            else
            if (tam == 3)
            {
                if (this.TemNavioAoredor(this.matriz, 4, 4, 3) || this.temNavioNoMeio(3))
                {
                    MessageBox.Show("Já há um navio no meio!! Por favor, o retire");
                    return;
                }

                this.estaInserindoNoMomento = true;
                this.matriz[4, 4] = "3.1.1";
                this.matriz[4, 5] = "3.2.1";
                this.matriz[4, 6] = "3.3.1";

                this.refreshDtg();
                this.posicaoAtual = "44";
                this.tamanhoUlt = 3;
                this.preencherAoRedorNavio(this.matriz, 4, 4);
            }
            else
            if (tam == 4)
            {
                if (this.TemNavioAoredor(this.matriz, 4, 3, 4) || this.temNavioNoMeio(4))
                {
                    MessageBox.Show("Já há um navio no meio!! Por favor, o retire");
                    return;
                }

                this.estaInserindoNoMomento = true;
                this.matriz[4, 3] = "4.1.1";
                this.matriz[4, 4] = "4.2.1";
                this.matriz[4, 5] = "4.3.1";
                this.matriz[4, 6] = "4.4.1";

                this.refreshDtg();
                this.posicaoAtual = "43";
                this.tamanhoUlt = 4;
                this.preencherAoRedorNavio(this.matriz, 4, 3);
            }
            else
            if (tam == 5)
            {
                if (this.TemNavioAoredor(this.matriz, 4, 3, 5) || this.temNavioNoMeio(5))
                {
                    MessageBox.Show("Já há um navio no meio!! Por favor, o retire");
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
                this.tamanhoUlt = 5;
                this.preencherAoRedorNavio(this.matriz, 4, 3);
            }
        }

        private void refreshDtg()
        {
            for (int j = 0; j < 10; j++)
                for (int i = 0; i < 10; i++)
                    dtgCampo[i + 1, j].Value = imagem;

            int direcao = -1;

            for (int i = 0; i < 10; i++) //muda a coluna
                for (int j = 0; j < 10; j++) //linha a linha
                {
                    string valorImagem = this.matriz[i, j].ToString().Substring(0, 1);
                    string qualImagem = this.matriz[i, j].ToString().Substring(2, 1);

                    if (this.matriz[i, j].ToString().Equals("1.1.1") || this.matriz[i, j].ToString().Equals("0.0.0"))
                        continue;

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

        private void controleDosBotoesUpgrade(bool ver1, bool ver2, String nomePrim, String nomeSeg)
        {
            this.btnDeCima.Visible = ver1;
            this.btnDeBaixo.Visible = ver2;
            this.btnDeCima.Text = nomePrim;
            this.btnDeBaixo.Text = nomeSeg;
        }

        private void verBotoesDeInsersao(bool ver)
        {
            this.btnEnter.Visible = ver;
            this.btnUp.Visible = ver;
            this.btnLeft.Visible = ver;
            this.btnRight.Visible = ver;
            this.btnDown.Visible = ver;
            this.btnGirar.Visible = ver;
            this.btnDelete.Visible = ver;
        }

        private void verLabels(bool ver)
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
        }

        private void ColocarONavioNaTela(int tam)
        {
            if (tam == 2)
            {
                if (this.TemNavioAoredor(this.matriz, 4, 4, 2) || this.temNavioNoMeio(2))
                {
                    MessageBox.Show("Já há um navio no meio!! Por favor, o retire");
                    return;
                }

                this.estaInserindoNoMomento = true;
                this.matriz[4, 4] = "2.1.1";
                this.matriz[4, 5] = "2.2.1";
                this.refreshDtg();
                this.posicaoAtual = "44";
                this.tamanhoUlt = 2;
                this.preencherAoRedorNavio(this.matriz, 4, 4);
            }
            else
            if (tam == 3)
            {
                if (this.TemNavioAoredor(this.matriz, 4, 4, 3) || this.temNavioNoMeio(3))
                {
                    MessageBox.Show("Já há um navio no meio!! Por favor, o retire");
                    return;
                }

                this.estaInserindoNoMomento = true;
                this.matriz[4, 4] = "3.1.1";
                this.matriz[4, 5] = "3.2.1";
                this.matriz[4, 6] = "3.3.1";

                this.refreshDtg();
                this.posicaoAtual = "44";
                this.tamanhoUlt = 3;
                this.preencherAoRedorNavio(this.matriz, 4, 4);
            }
            else
            if (tam == 4)
            {
                if (this.TemNavioAoredor(this.matriz, 4, 3, 4) || this.temNavioNoMeio(4))
                {
                    MessageBox.Show("Já há um navio no meio!! Por favor, o retire");
                    return;
                }

                this.estaInserindoNoMomento = true;
                this.matriz[4, 3] = "4.1.1";
                this.matriz[4, 4] = "4.2.1";
                this.matriz[4, 5] = "4.3.1";
                this.matriz[4, 6] = "4.4.1";

                this.refreshDtg();
                this.posicaoAtual = "43";
                this.tamanhoUlt = 4;
                this.preencherAoRedorNavio(this.matriz, 4, 3);
            }
            else
            if (tam == 5)
            {
                if (this.TemNavioAoredor(this.matriz, 4, 3, 5) || this.temNavioNoMeio(5))
                {
                    MessageBox.Show("Já há um navio no meio!! Por favor, o retire");
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
                this.tamanhoUlt = 5;
                this.preencherAoRedorNavio(this.matriz, 4, 3);
            }
        }

        private void RecarregarDtg(DataGridView dtg, bool preencher)
        {
            String[,] mat;
            if (dtg.Equals(dtgAtaque))
                mat = matrizAtaque;
            else
                mat = matriz;

            if (!preencher)
            {
                this.InicializarNavios(mat);
                for (int j = 0; j < 10; j++)
                    for (int i = 0; i < 10; i++)
                        dtg[i + 1, j].Value = imagem;
            }
            else
            {
                for (int j = 0; j < 10; j++)
                    for (int i = 0; i < 10; i++)
                        dtg[i + 1, j].Value = imagem;

                for (int i = 0; i < 4; i++)
                    this.colocarDe2(mat);

                for (int i = 0; i < 3; i++)
                    this.colocarDe3(mat);

                if (!this.temEspaco(mat, 4))
                {
                    this.InicializarNavios(mat);
                    this.RecarregarDtg(dtg, true);
                }

                if (!this.temEspaco(mat, 4))
                {
                    this.InicializarNavios(mat);
                    this.RecarregarDtg(dtg, true);
                }

                if (!this.temEspaco(mat, 5))
                {
                    this.InicializarNavios(mat);
                    this.RecarregarDtg(dtg, true);
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

        private void preencherAoRedorNavio(String[,] mat, int x, int y)
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

        public static int RandomNumber(int max)
        {
            lock (synclock)
            {
                return random.Next(max);
            }
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
                                this.colocar4NumLocalEspecifico(mat, i, j, direcao);

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
                                this.colocar4NumLocalEspecifico(mat, i, j - 3, direcao);

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
                                this.colocar4NumLocalEspecifico(mat, j, i, direcao);

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
                                this.colocar4NumLocalEspecifico(mat, j - 3, i, direcao);

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

        private void colocar4NumLocalEspecifico(String[,] matriz, int x, int y, int direcao)
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

        private void oponenteAcertouNavioTocarSom()
        {
            this.player1 = new SoundPlayer("../audios/explo.wav");
            player1.Play();
        }

        private void oponenteAcertouAguaTocarSom()
        {
            this.player1 = new SoundPlayer("../audios/water.wav");
            player1.Play();
        }

        private void jogadorAcertouNavioTocarSom()
        {
            this.player2 = new SoundPlayer("../audios/explo.wav");
            player2.Play();
        }

        private void jogadorAcertouAguaTocarSom()
        {
            this.player2 = new SoundPlayer("../audios/water.wav");
            player2.Play();
        }

        private void frmMulti_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.estaInserindo)
            {
                int x = Convert.ToInt16(this.posicaoAtual.Substring(0, 1));
                int y = Convert.ToInt16(this.posicaoAtual.Substring(1, 1));

                if (e.KeyChar.Equals(Keys.Left))
                    this.movimentarONavio(0, this.formadorDePosicoes(), x, y);
                else
                if (e.KeyChar.Equals(Keys.Up))
                    this.movimentarONavio(1, this.formadorDePosicoes(), x, y);
                else
                if (e.KeyChar.Equals(Keys.Right))
                    this.movimentarONavio(2, this.formadorDePosicoes(), x, y);
                else
                if (e.KeyChar.Equals(Keys.Down))
                    this.movimentarONavio(3, this.formadorDePosicoes(), x, y);

                this.refreshDtg();
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

            this.preencherAoRedorNavio(this.matriz, x, y);
            this.posicaoAtual = "" + x + y;
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (estaInserindo)
            {
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void checarNavios()
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j <= 9; j++)
                    for (int a = 0; a < indicePosicoesQueAntesTinha; a++)
                        if (this.matriz[i, j] == posicoesQueAntesTinhaNavio[indicePosicoesQueAntesTinha])
                            if (!this.ehNavio(this.matriz[i, j]))
                                this.matriz[i, j] = posicoesQueAntesTinhaNavio[indicePosicoesQueAntesTinha].Substring(2, 6);
        }

        private void confirmarNavio()
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
                this.btnPronto.Visible = true;
            }
            else
            {
                this.estaInserindo = true;
                this.btnPronto.Visible = false;
            }

            this.atualizarLabels();
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

        private void atualizarLabels()
        {
            this.labelDe2.Text = "(" + Convert.ToString(4 - this.qtdNaviosDe2Inseridos) + "x)";
            this.labelDe3.Text = "(" + Convert.ToString(3 - this.qtdNaviosDe3Inseridos) + "x)";
            this.labelDe4.Text = "(" + Convert.ToString(2 - this.qtdNaviosDe4Inseridos) + "x)";

            if (this.inseriuODe5)
                this.labelDe5.Text = "(0x)";
            else
                this.labelDe5.Text = "(1x)";
        }

        private void GirarONavio(int x, int y)
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

            this.preencherAoRedorNavio(this.matriz, x, y);
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
                    this.tamanhoUlt = 2;
                }
                else
                if (ultimoIndexInserido < 7 && qtdNaviosDe3Inseridos > 0)
                {
                    qtdNaviosDe3Inseridos--;
                    this.tamanhoUlt = 3;
                }
                else
                if (ultimoIndexInserido < 9 && qtdNaviosDe4Inseridos > 0)
                {
                    qtdNaviosDe4Inseridos--;
                    this.tamanhoUlt = 4;
                }
                else
                {
                    inseriuODe5 = false;
                    this.tamanhoUlt = 5;
                }
            }
            else
            {
                if (this.tamanhoUlt == 2)
                {
                    this.ApagarNavio(Convert.ToInt16(this.posicaoAtual.Substring(0, 1)), Convert.ToInt16(this.posicaoAtual.Substring(1, 1)));

                    posicaoAtual = null;

                    this.estaInserindoNoMomento = false;
                }
                else
                if (this.tamanhoUlt == 3)
                {
                    this.ApagarNavio(Convert.ToInt16(this.posicaoAtual.Substring(0, 1)), Convert.ToInt16(this.posicaoAtual.Substring(1, 1)));

                    posicaoAtual = null;

                    this.estaInserindoNoMomento = false;
                }
                else
                if (this.tamanhoUlt == 4)
                {
                    this.ApagarNavio(Convert.ToInt16(this.posicaoAtual.Substring(0, 1)), Convert.ToInt16(this.posicaoAtual.Substring(1, 1)));

                    posicaoAtual = null;

                    this.estaInserindoNoMomento = false;
                }
                else
                if (this.tamanhoUlt == 5)
                {
                    this.ApagarNavio(Convert.ToInt16(this.posicaoAtual.Substring(0, 1)), Convert.ToInt16(this.posicaoAtual.Substring(1, 1)));

                    posicaoAtual = null;

                    this.estaInserindoNoMomento = false;
                }
            }

            this.atualizarLabels();

            btnPronto.Visible = false;

            if (indicesInseridos.Length > 0 && posAtualEraNull)
                this.indicesInseridos = indicesInseridos.Substring(1, indicesInseridos.Length - 1);

            if (posAtualEraNull && indicesInseridos.Length > 0) //ou seja, se o navio NAO estava inserido, ou seja, eu o apaguei mudo o indexUlt p o navio anterior
                this.ultimoIndexInserido = Convert.ToInt16(indicesInseridos.Substring(0, 1));
        }

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
                        MessageBox.Show("Não há espaço para o Encouraçado!");
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
                        MessageBox.Show("Não há espaço para o Porta-Aviões!");
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
                MessageBox.Show("Insira o navio atual primeiro !");

            if (this.inseriuTodos())
            {
                this.btnPronto.Visible = true;
            }
            else
            {
                this.estaInserindo = true;
                this.btnPronto.Visible = false;
            }

            this.refreshDtg();
            this.atualizarLabels();
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

        private void btnPronto_Click(object sender, EventArgs e)
        {
            if (this.btnPronto.Text == "Pronto")
            {
                this.estaInserindo = false;
                this.btnPronto.Enabled = false;
                this.controleDosBotoesUpgrade(false, false, "Inserir Manualmente", "Inserir Aleatoriamente");
                this.verBotoesDeInsersao(false);
                this.dtgAtaque.Enabled = true;
                this.RecarregarDtg(dtgAtaque, false);

                this.qtdeNaviosMeus = 30;
                this.qtdeNaviosOponente = 30;

                this.Controls.Remove(pbxDe2);
                this.Controls.Remove(pbxDe3);
                this.Controls.Remove(pbxDe4);
                this.Controls.Remove(pbxDe5);
                this.Controls.Remove(labelDe2);
                this.Controls.Remove(labelDe3);
                this.Controls.Remove(labelDe4);
                this.Controls.Remove(labelDe5);

                btnInserir.Hide();
                btnAleatorio.Hide();

                // Enviando pronto ao oponente.
                if (this.cliente)
                    this.enviar("?");
                else
                {
                    if (lblPronto.Visible)
                    {
                        this.lblPronto.Hide();
                        this.btnPronto.Text = "Iniciar";
                        this.btnPronto.Enabled = true;
                    }
                    else
                    {
                        this.enviarDados("?");
                        this.pronto = true;
                    }
                }
            }
            else
            {
                this.verLabels(true);

                this.btnPronto.Hide();
                this.btnPronto.Text = "Pronto";
                this.habilitarChat();
            }
        }

        private void dtgAtaque_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show(matrizAtaque[e.ColumnIndex - 1, e.RowIndex]);

            if (vezDoOponente)
                return;
            
            vezDoOponente = true;
            
            if (e.ColumnIndex == 0 || e.ColumnIndex > 10)
                return;

            int x = e.ColumnIndex;
            int y = e.RowIndex;

            if (matrizAtaque[x-1, y] == "7.7.7" || matrizAtaque[x-1, y] == "6.6.6")
            {
                vezDoOponente = false;
                return;
            }

            int tem = this.verPosicao(matrizAtaque, x-1, y);

            if(tem == 1)
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
                        MessageBox.Show("AFUNDOU!!!");//TRATAR COLOCAR NAVIO AFUNDADO

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

                        //Task.Factory.StartNew(() => alertaLabel(lbEmQuestao)); 

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

                        MessageBox.Show("Jogou acabou!");
                        Close();
                        return;
                    }
                }               
            }               
            else
            {
                dtgAtaque[x,y].Value = imlImagens.Images[16];
                Thread s = new Thread(this.jogadorAcertouAguaTocarSom);
                s.Start();
                this.matrizAtaque[x - 1, y] = "7.7.7";
            }

            this.dtgAtaque.Refresh();

            dtgAtaque.Enabled = true;
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

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (posicaoAtual == null)
                return;
            int x = Convert.ToInt16(this.posicaoAtual.Substring(0, 1));
            int y = Convert.ToInt16(this.posicaoAtual.Substring(1, 1));

            this.movimentarONavio(1, this.formadorDePosicoes(), x, y);
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

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (posicaoAtual == null)
                return;
            this.confirmarNavio();
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

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (posicaoAtual == null)
                return;
            int x = Convert.ToInt16(this.posicaoAtual.Substring(0, 1));
            int y = Convert.ToInt16(this.posicaoAtual.Substring(1, 1));

            this.movimentarONavio(0, this.formadorDePosicoes(), x, y);
            this.refreshDtg();
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

        private void frmMulti_KeyUp(object sender, KeyEventArgs e)
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
                    this.confirmarNavio();
            }
            this.checarNavios();

            this.refreshDtg();
        
        }
    }
}
