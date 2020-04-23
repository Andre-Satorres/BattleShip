using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Collections;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using System.IO;

namespace pTesteBatalhaNaval
{
    public partial class frmLista : Form
    {
        // Desclaracao das variaveis:
        // Socket para fazer a conexao entre o cliente e o servidor.
        private Socket skt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // Nome do jogador.
        private String nick;
        // Resultando do broadcasting.
        private IAsyncResult iar;
        // Formulario naval.
        private frmNaval nav;
        // Uma variavel que armazena uma lista de listas de strings (uma matriz de strings).
        private List<List<String>> ll;
        // Variavel para simbolizar o desejo do usuario, sair totalmente do programa ou voltar ao formulario anterior.
        private bool temp = true;

        public frmLista(String nomeJogador, frmNaval naval)
        {
            InitializeComponent();
            // Atribuindo o nome do jogador a variavel da classe.
            this.nick = nomeJogador;
            // Adicionando o form naval para esta classe para que se possa fechar o programa mais tarde.
            this.nav = naval;
        }

        // Metodo para mudar o valor de 'this.temp', obdecendo a vontade do usuario.
        public void setTemp (bool b)
        {
            this.temp = b;
        }

        private void btnHost_Click(object sender, EventArgs e)
        {
            // Atualizando controles.
            btnHost.Hide();
            btnLista.Hide();
            btnAtras.Hide();

            // Circular progress bar sendo exibida para carregar os servidores disponiveis.
            cpgCarregar.Show();

            // Devemos passar todos os nomes de sala que ja existem atualmente.
            this.ll = getTodosServersPorPorta(6969);

            // Adicionando a uma lista os nomes dos servidores.
            List<String> ls = new List<String>();
            for (int i = 0; i < this.ll.Count; i++)
                ls.Add(this.ll[i][0]);

            // Abrindo o form nomeSala para escolher o nome da sala.
            frmNomeSala frmNome = new frmNomeSala(this.nick, ls, GetIPs()[0],this.nav, this);
            frmNome.Show();
        }

        private String[] GetIPs()
        {
            // Achar o host pelo endereco IP.
            String strNomeServidor = Dns.GetHostName();
            // Pega o primeiro IP da Lista (que deve ser da placa principal).
            IPHostEntry ipHost = Dns.GetHostEntry(strNomeServidor);
            // Usando a classe lista ja que eh mais facil de se manusear.
            List<string> ips = new List<string>();

            // Percorrendo todos os IPV4s existentes na rede.
            foreach (IPAddress ipa in ipHost.AddressList)
                // Verificando se o IPAddres eh um IPV4.
                if (!(ipa.AddressFamily == AddressFamily.InterNetworkV6))
                    // Adicionando o IPV4 encontrado na lista.
                    ips.Add(ipa.ToString());

            // Retornando o vetor com os ips.
            // Convertendo a lista em um vetor.
            return ips.ToArray();
        }

        private List<List<String>> getTodosServersPorPorta(int porta)
        {
            // Resetando os dados da lista de lista de string, a qual contem todas as informacoes sobre os servidores existentes na rede.
            this.ll = new List<List<String>>();
            
            // A variavel 'j' serve somente para regular o crescimento do value no circular progress bar.
            int j = 0;

            // Reinicializando o circular progress bar.
            this.cpgCarregar.Value = 0;

            // Percorrento por todos os IPV4s existentes.
            for (int qualIP = 0; qualIP < GetIPs().Length; qualIP++)
            {
                // Recebendo o primeiro endereco IP.
                // Dividindo o ip pelo '.' em subpartes, como 1.2.3.4 ficasse: posicao 0=1, posicao 1=2, posicao 2=3, posicao 3=4 em um vetor.
                String[] ip = GetIPs()[qualIP].Split('.');

                // Percorrendo ate a mascara 255.
                for (int i = 1; i < 255; i++)
                {
                    // Testando um ip candidato para checar se existe algum servidor naquela maquina.
                    String ipCandidato = ip[0] + "." + ip[1] + "." + ip[2] + "." + i;
                    // Resetando a lista para que esta contenha informacoes sobre o servidor, caso existir.
                    List<String> l = new List<String>();

                    // Definindo um tempo limite para tentar se conectar com o servidor que pode ou nao existir.
                    // Este metodo eh mais eficiente porque uma socket usando .Connect demoraria demais, o que 
                    // nao seria interessante para o usuario, o qual esperaria mais de 30 minutos para checar
                    // todos os ips naquela rede e definir se existe ou nao servidores ativos.
                    this.iar = this.skt.BeginConnect(ipCandidato, 6969, null, null);
                    bool b = this.iar.AsyncWaitHandle.WaitOne(10, true);

                    // Regulando 'j' para aumentar o progress bar.
                    j++;

                    if (j == 5)
                    {
                        // Para nao ter excessao, devemos verificar se o valor do progress bar esta em 100%.
                        if (this.cpgCarregar.Value != 100)
                        {
                            // Regulando o crescimento do progress bar.
                            if (GetIPs().Length == 2)
                                this.cpgCarregar.Value += 1;
                            else
                                this.cpgCarregar.Value += 2;

                            // Alterando o texto para informar o usuario do progresso atual.
                            this.cpgCarregar.Text = Convert.ToString(this.cpgCarregar.Value) + "%";

                            // Regulando o crescimento do progress bar.
                            j = 0;
                        }
                        else
                        {
                            // Alterando o texto para informar o usuario do progresso atual.
                            this.cpgCarregar.Text = Convert.ToString(this.cpgCarregar.Value) + "%";
                            // Processar as mensagens do Windows que estao na fila.
                            Application.DoEvents();
                        }
                    }
                    
                    // Checar se foi possivel se conectar com o servidor com o IP utilizado.
                    if (this.skt.Connected)
                    {
                        // Recebimento de dados.
                        byte[] recebidos = new byte[20];

                        // Envio do pedido de nome da partida.
                        this.skt.Send(System.Text.Encoding.ASCII.GetBytes("!"));

                        // Recebimento do nome da partida.
                        int it1 = this.skt.Receive(recebidos);

                        // Adicionando o nome da partida na lista.
                        l.Add(Encoding.ASCII.GetString(recebidos));

                        // Adicionando o ip testado para que o usuario possa entrar.
                        l.Add(ipCandidato);

                        // Alterando o tipo ja que iremos receber uma string de 5 caracteres.
                        recebidos = new byte[5];

                        // Envio do pedido do tempo da partida.
                        this.skt.Send(System.Text.Encoding.ASCII.GetBytes("@"));

                        // Recebimento do tempo da partida.
                        int it2 = this.skt.Receive(recebidos);

                        // Adicionando o tempo da partida na lista.
                        l.Add(Encoding.ASCII.GetString(recebidos));

                        // Alterando o tipo ja que iremos receber uma string de 1 caracterer.
                        recebidos = new byte[1];

                        // Envio do pedido da quantidade de jogadores.
                        this.skt.Send(System.Text.Encoding.ASCII.GetBytes("#"));

                        // Recebimento da quantidade de jogadores.
                        int it3 = this.skt.Receive(recebidos);

                        // Adicionando a quantidade de jogadores na partida na lista.
                        l.Add(Encoding.ASCII.GetString(recebidos));

                        // Alterando o tipo ja que iremos receber uma string de 20 caractereres, o nome do cliente do servidor.
                        recebidos = new byte[20];

                        // Envio do pedido do nome do cliente que hospeda o servidor.
                        this.skt.Send(System.Text.Encoding.ASCII.GetBytes("`"));

                        // Recebimento do nome do cliente que hospeda o servidor.
                        int it4 = this.skt.Receive(recebidos);

                        // Adicionando o nome do cliente que hospeda o servidor na lista.
                        l.Add(Encoding.ASCII.GetString(recebidos));

                        if (l[3] == "2")
                        {
                            // Alterando o tipo ja que iremos receber uma string de 20 caractereres, o nome do oponente do cliente do servidor.
                            recebidos = new byte[20];

                            // Envio do pedido do nome do oponente do cliente que hospeda o servidor.
                            this.skt.Send(System.Text.Encoding.ASCII.GetBytes("&"));

                            // Recebimento do nome do oponente do cliente que hospeda o servidor.
                            int it5 = this.skt.Receive(recebidos);

                            // Adicionando o nome do oponente do cliente que hospeda o servidor na lista.
                            l.Add(Encoding.ASCII.GetString(recebidos));
                        }

                        // Adicionando a lista de string na matriz de listas.
                        this.ll.Add(l);

                        // Encerrando a conexao.
                        this.skt.EndConnect(this.iar);

                        // Resetando o socket para testar com mais IPs e verificar se ainda existem mais servidores na rede.
                        this.skt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    }
                    else
                    {
                        // Resetando o socket para testar com mais IPs e verificar se ainda existem mais servidores na rede.
                        this.skt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    }
                }
            }

            // Setando o valor do Text e o valor par ao maximo.
            this.cpgCarregar.Text = "100%";
            this.cpgCarregar.Value = 100;

            // Retornando a matriz de dados de todos os servidores e suas respectivas informacoes.
            return ll;
        }
        
        // Metodo para ordenar a matriz de dados e assim printar no list box os nomes das partidas em ordem alfabetica.
        private List<List<String>> ordenar (List<List<String>> ll)
        {
            // Nova lista ordenada.
            List<List<String>> ret = new List<List<string>>();

            // Verificamos se existe uma lista para ser ordenada.
            if (ll == null)
                throw new Exception("Lista Ausente!");

            // Vericamos se existe algum elemento na lista.
            if (ll.Count == 0)
                throw new Exception("Lista Invalida!"); ;

            // Se somente existe 1 elemento na lista, devemos retorna-la, porque nao teriamos como ordenar.
            if (ll.Count == 1)
                return ll;
            
            // Comecando a ordenar a lista passada como parametro.
            // Setando o menor valor como o primeiro elemento, porem, ainda nao sabemos se este eh o menor.
            // Verificaremos o menor quando formos testa-lo.
            List<String> menor = ll[0];
            // Setando valores as variaveis que servirao de auxilio para ordenar a matriz de dados.
            int atual=1;
            int pos = 0;

            // Ordenando lista pelo comeco.
            for (int i = 0; i < ll.Count - 1; i++)
            {
                // Verificar se existe algum elemento.
                for (int j = atual; j < ll.Count; j++)
                {
                    // Comparando o menor valor encontrado com o proximo da lista da lista.
                    if (menor[0].CompareTo(ll[j][0]) == 1)
                    {
                        // Setando o menor como o proximo ja que o compareTo retornou 1, o que indica que o menor eh maior que o proximo.
                        menor = ll[j];
                        // Alterando o indice da posicao do menor.
                        pos = j;
                    }

                    // Caso chegarmos no ultimo da lista de lista, significa que podemos alterar o menor de lugar.
                    if (j == ll.Count - 1)
                    {
                        // Criando uma lista auxiliar para nao perder a lista que esta nao posicao que mudaremos.
                        List<String> aux = ll[i];
                        // Adicionando a matriz a lista encontrada.
                        ret.Add(menor);
                        // Colocando o menor no comeco da lista de lista caso for a primeira vez ou apos o segundo, terceiro, quarto ... menor da lista de lista.
                        ll[i] = menor;
                        ll[pos] = aux;

                        // Caso a lista tenha chegado ao fim, significa que ela ja esta ordenada e por isso devemos parar o metodo e nao cair neste if.
                        // Verificacao para saber se a lista ainda nao esta no fim.
                        if (i != ll.Count - 1)
                        {
                            // Setando o menor.
                            menor = ll[i + 1];
                            // Andando com posicao que ainda precisa ser ordenada.
                            pos = i + 1;
                            // Andando com o atual para testarmos os proximos valores.
                            atual++;
                        }
                    }
                }
            }
            // Adicionando o ultimo elemento da lista na matriz de dados ou lista de listas.
            // O lugar do ultimo ja esta certo, por que o metodo de cima empurra os que sao os ultimos no alfabeto para as ultimas posicoes.
            ret.Add(ll[ll.Count - 1]);

            // Retornando a matriz de dados ou lista de listas.
            return ret;
        }

        private void btnLista_Click(object sender, EventArgs e)
        {
            // Atualizando controles.
            btnHost.Hide();
            btnLista.Hide();
            btnAtras.Hide();

            // Circular progress bar sendo exibida para carregar os servidores disponiveis.
            cpgCarregar.Show();

            // Atribuindo a uma lista de lista de string os possiveis servidores existentes na rede em uma porta.
            // Importante lembrar que o valor da progress bar muda no metodo getTodosServersPorPorta(int porta).
            this.ll = this.getTodosServersPorPorta(6969);

            // Escondendo o circular progress bar, ja que todos os servidores ja foram carregados.
            cpgCarregar.Hide();

            // Verificando se a lista de servidores nao eh nula.
            if (this.ll.Any())
            {
                // Ordenando a lista de servidores em relacao ao nome do servidor.
                List<List<String>> ordenado = this.ordenar(this.ll);
                // Adicionando os servidores no list box para o usuario selecionar depois.
                for (int i = 0; i < ordenado.Count; i++)
                {
                    // Verificando se existem dois jogadores jogando.
                    if (ordenado[i][3] == "2")
                        // Exibindo contra quem o servidor esta jogando.
                        lbxLista.Items.Add(ordenado[i][0] + "  -  " + ordenado[i][1] + "  -  " + ordenado[i][2] + "  -      " + ordenado[i][3] + "  -  "+ordenado[i][4].Trim()+" vs "+ordenado[i][5].Trim());
                    else
                        // Padronizando o tamanho entre cada string.
                        lbxLista.Items.Add(ordenado[i][0] + "  -  " + ordenado[i][1] + "  -  " + ordenado[i][2] + "  -      " + ordenado[i][3]);
                }
            }
            else
            {
                // Caso a lista for nula, informar que nenhum servidor foi encontrado.
                for (int i = 0; i < 5; i++)
                    lbxLista.Items.Add("");
                lbxLista.Items.Add("                                     Nenhum servidor encontrado!");
            }

            // Atualizando controles.
            lbLista.Show();
            lbObs.Show();
            lbxLista.Show();
            btnVoltar.Show();
            btnRecarregar.Show();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            // Atualizando controles.
            lbLista.Hide();
            lbObs.Hide();
            lbxLista.Hide();
            btnVoltar.Hide();
            btnRecarregar.Hide();

            // Limpando o list box e adicionando o cabecalho.
            lbxLista.Items.Clear();
            lbxLista.Items.Add("-> Nome Servidor                  IP            Tempo     Jogadores      Disputa");

            // Atualizando controles.
            btnHost.Show();
            btnLista.Show();
            btnAtras.Show();
        }

        private void btnRecarregar_Click(object sender, EventArgs e)
        {
            // Atualizando controles.
            lbLista.Hide();
            lbObs.Hide();
            lbxLista.Hide();
            btnVoltar.Hide();
            btnRecarregar.Hide();

            // Limpando o list box e adicionando o cabecalho.
            lbxLista.Items.Clear();
            lbxLista.Items.Add("-> Nome Servidor                  IP            Tempo     Jogadores      Disputa");

            // Circular progress bar sendo exibida para carregar os servidores disponiveis.
            cpgCarregar.Show();

            // Atribuindo a uma lista de lista de string os possiveis servidores existentes na rede em uma porta.
            // Importante lembrar que o valor da progress bar muda no metodo getTodosServersPorPorta(int porta).
            this.ll = getTodosServersPorPorta(6969);

            // Escondendo o circular progress bar, ja que todos os servidores ja foram carregados.
            cpgCarregar.Hide();

            // Verificando se a lista de servidores nao eh nula.
            if (this.ll.Any())
            {
                // Ordenando a lista de servidores em relacao ao nome do servidor.
                List<List<String>> ordenado = this.ordenar(this.ll);
                // Adicionando os servidores no list box para o usuario selecionar depois.
                for (int i = 0; i < ordenado.Count; i++)
                {
                    // Verificando se existem dois jogadores jogando.
                    if (ordenado[i][3] == "2")
                        // Exibindo contra quem o servidor esta jogando.
                        lbxLista.Items.Add(ordenado[i][0] + "  -  " + ordenado[i][1] + "  -  " + ordenado[i][2] + "  -      " + ordenado[i][3] + "  -  " + ordenado[i][4].Trim() + " vs " + ordenado[i][5].Trim());
                    else
                        // Padronizando o tamanho entre cada string.
                        lbxLista.Items.Add(ordenado[i][0] + "  -  " + ordenado[i][1] + "  -  " + ordenado[i][2] + "  -      " + ordenado[i][3]);
                }
            }
            else
            {
                // Caso a lista for nula, informar que nenhum servidor foi encontrado.
                for (int i = 0; i < 5; i++)
                    lbxLista.Items.Add("");
                lbxLista.Items.Add("                                     Nenhum servidor encontrado!");
            }

            // Atualizando controles.
            lbLista.Show();
            lbObs.Show();
            lbxLista.Show();
            btnVoltar.Show();
            btnRecarregar.Show();
        }
        
        private void lbxLista_DoubleClick(object sender, EventArgs e)
        {
            if (!lbxLista.SelectedItem.Equals(lbxLista.Items[0]))
            {
                if (this.ll.Any())
                {
                    // Tentar estabelecer conexao com o servidor para verficar se aquele ip ainda esta ativo.
                    // Abrir o form multiplayer e passa o ip pegado no SelectedItem do listbox para conectar com o outro jogador.
                    int selecionado = lbxLista.SelectedIndex;

                    if (this.ll[selecionado - 1][3] == "2")
                    {
                        // Exibindo mensagem de erro.
                        lblErro.Text = "Erro: Este servidor esta cheio!";
                        lblErro.Show();
                        Application.DoEvents();
                        Thread.Sleep(2000);
                        lblErro.Hide();
                    }
                    else
                    {
                        try
                        {
                            this.skt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            // IP pegado do list box.
                            this.skt.Connect(this.ll[selecionado - 1][1], 6969);

                            // Verificando se eh possivel se conectar para estabelecer um jogo.
                            if (this.skt.Connected)
                            {
                                // Abrindo o form multiplayer para o cliente.
                                ///////
                                this.nick = "zuza                ";
                                ///////
                                frmMulti multi = new frmMulti(this.skt, this.nick, this.ll[selecionado - 1][4], this.nav, true);
                                multi.Show();
                                // Usuario nao deseja fechar o programa totalmente.
                                this.setTemp(false);
                                Close();
                            }
                        }
                        catch (Exception)
                        {
                            // Caso este IP ainda esteja inativo, entao exibir o label indicando que este servidor nao esta mais inativo e apaga-lo do list box.
                            lbxLista.Items.RemoveAt(selecionado);
                            // Removendo da lista de nomes, ips, tempo e qtdJogadores.
                            this.ll.RemoveAt(selecionado - 1);
                            // Exibindo mensagem de erro.
                            lblErro.Text = "Erro: Este servidor esta offline!";
                            lblErro.Show();
                            Application.DoEvents();
                            Thread.Sleep(2000);
                            lblErro.Hide();
                            // Resetando o socket.
                            this.skt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        }
                    }
                }
            }
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            // Voltando ao form anterior passando a instancia do form naval.
            frmJogo jogo = new frmJogo(nav, 1);
            jogo.Show();
            this.setTemp(false);
            Close();
        }

        private void frmLista_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Fechando o form naval caso o usuario clique diretamente no botao fechar e nao no botao voltar.
            if (this.temp)
                this.nav.Close();
        }
    }
}
