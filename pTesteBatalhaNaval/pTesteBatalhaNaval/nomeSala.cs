using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pTesteBatalhaNaval
{
    public partial class frmNomeSala : Form
    {
        // Armazena o nome do usuario.
        private String nick;
        // Lista de nomes de servidores.
        private List<String> l;
        // Formulario naval, para fechar ou nao.
        private frmNaval nav;
        // Formulario lista, usado para mexer nas configuracoes das variaveis internas.
        private frmLista lista;
        // Usada para controlar se o usuario quer definitivamente sair do programa ou nao.
        private bool temp=true;

        public frmNomeSala(String nome, List<String> lis, String ip, frmNaval naval, frmLista lista)
        {
            InitializeComponent();
            this.nick = nome;
            this.l = lis;
            this.lblObs.Text += ip;
            this.nav = naval;
            this.lista = lista;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Formatadando o nome da sala.
            String nomeSala = txtNome.Text;
            while (nomeSala.Length != 20)
                nomeSala += " ";

            if (nomeSala.Trim() == "")
            {
                // Exibindo mensagem de erro.
                lblErro.Text = "Erro: Digite um nome de sala válido!";
                lblErro.Show();
                Application.DoEvents();
                Thread.Sleep(2000);
                lblErro.Hide();
                txtNome.Text = "";
                txtNome.Focus();
            }
            else
            if (this.l.Contains(nomeSala))
            {
                // Exibindo mensagem de erro.
                lblErro.Text = "Erro: Este nome de sala já esta sendo utilizado, escolha outro!";
                lblErro.Show();
                Application.DoEvents();
                Thread.Sleep(2000);
                lblErro.Hide();
                txtNome.Text = "";
                txtNome.Focus();
            }
            else
            {
                try
                {
                    // Criando um servidor para hospedar a partida.
                    Socket servidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, 6969);
                    servidor.Bind(ipLocal);
                    // Abrindo o form multiplayer para o hospedar.
                    frmMulti multi = new frmMulti(servidor, this.nick, nomeSala, this.nav, false);
                    multi.Show();
                    // Usuario nao deseja fechar o programa totalmente.
                    this.temp = false;
                    // Fechando o programa, 'frmNomeSala'.
                    Close();
                }
                catch (Exception)
                {
                    // Exibindo mensagem de erro.
                    lblErro.Text = "Erro: Um servidor já esta ativo nesta máquina!";
                    lblErro.Show();
                    Application.DoEvents();
                    Thread.Sleep(2000);
                    lblErro.Hide();
                    txtNome.Text = "";
                    txtNome.Focus();
                }
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            // Voltando ao form da lista, retornando os mesmo valores passados para a criacao deste form.
            frmLista l = new frmLista(this.nick, this.nav);
            l.Show();
            // Usuario nao deseja fechar o programa totalmente.
            this.temp = false;
            Close();
        }

        private void frmNomeSala_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Caso o usuario simplesmente fechar o programa e nao clicar nem em voltar e nem em ok, fechar tudo.
            if (this.temp)
                this.nav.Close();
        }

        private void frmNomeSala_Load(object sender, EventArgs e)
        {
            // Usuario nao deseja fechar o programa totalmente.
            this.lista.setTemp(false);
            this.lista.Close();
            // Setando o focus no texto box.
            txtNome.Focus();
        }

        private void txtNome_KeyDown(object sender, KeyEventArgs e)
        {
            // Caso o usuario pressionar 'enter', entao fazer a acao do botao ok.
            if (e.KeyCode == Keys.Enter)
                btnOK.PerformClick();
        }
    }
}
