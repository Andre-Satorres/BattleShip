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
using System.Threading;

namespace pTesteBatalhaNaval
{
    public partial class frmCreditos : Form
    {
        SoundPlayer p = new SoundPlayer("../audios/hope.wav");
        public frmCreditos()
        {
            InitializeComponent();
          
            p.PlayLooping();            
        }

        public void Rodar()
        {
            lbEscrito.Location = new Point(lbEscrito.Location.X, this.Bottom);
            lbTitulo2.Location = new Point(lbTitulo2.Location.X, this.Bottom);
            lbCriadores.Location = new Point(lbCriadores.Location.X, this.Bottom);
            lbTitulo3.Location = new Point(lbTitulo3.Location.X, this.Bottom);
            lbTitulo4.Location = new Point(lbTitulo4.Location.X, this.Bottom);
            lbMaterias.Location = new Point(lbMaterias.Location.X, this.Bottom);
            lbAjudantes.Location = new Point(lbAjudantes.Location.X, this.Bottom);
            pbxCtc.Location = new Point(72, this.Bottom);
            pbxUnicamp.Location = new Point(372, this.Bottom);

            lbEscrito.Visible = true;
            Application.DoEvents();
            lbEscrito.Refresh();
            while (pbxCtc.Location.Y > -170)
            {
                if (lbEscrito.Visible == true)
                {
                    lbEscrito.Location = new Point(lbEscrito.Location.X, lbEscrito.Location.Y - 1);
                    Application.DoEvents();
                }

                if (lbEscrito.Location.Y == -67)
                    lbEscrito.Visible = false;

                if (lbEscrito.Location.Y <= 400)
                {
                    if (lbTitulo2.Visible == true)
                    {
                        lbTitulo2.Location = new Point(lbTitulo2.Location.X, lbTitulo2.Location.Y - 1);
                    }

                    if (lbEscrito.Location.Y == 400)
                        lbTitulo2.Visible = true;

                    if (lbTitulo2.Location.Y == 550)
                        lbCriadores.Visible = true;

                    if (lbTitulo2.Location.Y < -209)
                        lbCriadores.Hide();

                    if (lbTitulo2.Location.Y < 550)
                    {
                        lbCriadores.Location = new Point(lbCriadores.Location.X, lbCriadores.Location.Y - 1);
                    }

                    if (lbCriadores.Location.Y == 450)
                        lbTitulo3.Visible = true;

                    if (lbCriadores.Location.Y < 450)
                    {
                        lbTitulo3.Location = new Point(lbTitulo3.Location.X, lbTitulo3.Location.Y - 1);
                    }

                    if (lbTitulo3.Location.Y == 550)
                        lbAjudantes.Visible = true;

                    if (lbTitulo3.Location.Y < 550)
                    {
                        lbAjudantes.Location = new Point(lbAjudantes.Location.X, lbAjudantes.Location.Y - 1);
                        Thread.Sleep(10);
                    }

                    if (lbAjudantes.Location.Y == 100)
                        lbTitulo4.Visible = true;

                    if (lbAjudantes.Location.Y < 100)
                    {
                        lbTitulo4.Location = new Point(lbTitulo4.Location.X, lbTitulo4.Location.Y - 1);
                    }

                    if (lbTitulo4.Location.Y == 510)
                        lbMaterias.Visible = true;

                    if (lbTitulo4.Location.Y < 510)
                    {
                        lbMaterias.Location = new Point(lbMaterias.Location.X, lbMaterias.Location.Y - 1);
                        Thread.Sleep(10);
                    }

                    if (lbMaterias.Location.Y == 400)
                    {
                        pbxUnicamp.Visible = true;
                        pbxCtc.Visible = true;
                    }

                    if (lbMaterias.Location.Y < 400)
                    {
                        pbxCtc.Location = new Point(pbxCtc.Location.X, pbxCtc.Location.Y - 1);
                        pbxUnicamp.Location = new Point(pbxUnicamp.Location.X, pbxUnicamp.Location.Y - 1);
                    }

                    if (pbxUnicamp.Location.Y == -170)
                    {
                        pbxNavy.Visible = true;
                        
                    }
                }
                this.Refresh();
                Thread.Sleep(10);
            }
            Thread.Sleep(2000);
            p.SoundLocation = "../audios/main.wav";
            p.Play();
            Close();
        }

    }
}
