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
    public partial class frmNaval : Form
    {
        SoundPlayer player;
        private Boolean estaTocando = true;
        public frmNaval()
        {
            InitializeComponent();
            player = new SoundPlayer("../audios/main.wav");
            player.PlayLooping();
        }
        public void clickNovo()
        {         
            this.btnNovo.PerformClick();
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
        private void btnNovo_Click(object sender, EventArgs e)
        {
            if (pbxCanhao.Top == 136)
            {
                int soma = 1;
                frmJogo jogo = new frmJogo(this, player, estaTocando, soma);
                jogo.Show();
                Hide();
            }
            else
                btnSair.Focus();
        }
        private void btnSair_Click(object sender, EventArgs e)
        {
            if (pbxCanhao.Top == 224)
                Close();
            else
                btnNovo.Focus();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
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

        private void frmNaval_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (pbxCanhao.Top == 136)
                {
                    pbxCanhao.Top = 224;

                    btnSair.Focus();
                }
                else
                if (pbxCanhao.Top == 224)
                {
                    pbxCanhao.Top = 136;

                    btnNovo.Focus();
                }
            }
            else
            if (e.KeyCode == Keys.Down)
            {
                if (pbxCanhao.Top == 136)
                {
                    pbxCanhao.Top = 224;

                    btnSair.Focus();
                }
                else
                if (pbxCanhao.Top == 224)
                {
                    pbxCanhao.Top = 136;

                    btnNovo.Focus();
                }
            }

            if (e.KeyCode == Keys.Enter)
            {
                if (btnSair.Focused)
                    btnSair.PerformClick();
                else
                    if (btnNovo.Focused)
                    btnNovo.PerformClick();

            }
            pbxCanhao.Refresh();
            this.Refresh();
        }
    }
}