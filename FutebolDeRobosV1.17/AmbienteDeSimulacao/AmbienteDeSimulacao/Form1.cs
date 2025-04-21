using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SpriteWorld;

namespace AmbienteDeSimulacao
{
    public partial class Form1 : Form, Ambiente.IFilhosAmbiente
    {
        // Controle da direção do sprite (Teclado)
        private bool paraCima = false;
        private bool paraBaixo = false;
        private bool paraEsquerda = false;
        private bool paraDireita = false;
        private int auxDelayDirecao = 0;

        // Contador auxiliar de timers tick
        private int contTimer = 0;

        // Movimentação do sprite
        private bool arrastarMouse = false;

        // Variáveis do ambiente
        private int quantidadeCorpos = 5; 
        private int[,] dimencoesBola;
        private int[,] dimencoesAgente;

        // Localização inicial dos corpos
        private Point localizacaoIniBola;
        private Point localizacaoIniJ1T1;
        private Point localizacaoIniJ2T1;
        private Point localizacaoIniJ1T2;
        private Point localizacaoIniJ2T2;
        private List<Point> listaposicaoInicialCorpos;

        private static Form1 instance;
        private Ambiente.Ambiente ambiente;
        private Ambiente.ModeloAmbiente modeloAmbiente;
        private Rectangle limitesDoAmbiente;

        // SPRITES
        // Declaração dos Canvas
        private SpriteCanvas.Canvas canvas1;
        private SpriteCanvas.Canvas canvas2;
        private SpriteCanvas.Canvas canvas3;
        private SpriteCanvas.Canvas canvas4;
        private SpriteCanvas.Canvas canvas5;

        // Cria "world" controlador dos sprites
        private World controlador = new World();
        private WorldView controladorView = new WorldView();

        private bool boolIteragir = false;
        private bool boolRenderiza = false;


        #region Construtor

        public Form1()
        {
            InitializeComponent();

            instance = this;

            modeloAmbiente = new Ambiente.ModeloAmbiente();

            // Tamanho do campo 
            limitesDoAmbiente = new Rectangle();
            limitesDoAmbiente.Width = pictureBox1.Size.Width;
            limitesDoAmbiente.Height = pictureBox1.Size.Height;

            // Localização inicial dos corpos
            localizacaoIniBola = new Point(292, 215);  
            localizacaoIniJ1T1 = new Point(40, 200); 
            localizacaoIniJ2T1 = new Point(180, 200); 
            localizacaoIniJ1T2 = new Point(520, 200); 
            localizacaoIniJ2T2 = new Point(305, 200); 

            // Define as dimenções dos corpos
            dimencoesBola = new int[1, 2];
            dimencoesAgente = new int[1, 2];

            dimencoesBola[0, 0] = 20;
            dimencoesBola[0, 1] = 20;
            dimencoesAgente[0, 0] = 30;
            dimencoesAgente[0, 1] = 30;

            // Carrega a lista com as localizações iniciais dos corpos
            listaposicaoInicialCorpos = new List<Point>();
            listaposicaoInicialCorpos.Add(localizacaoIniBola);
            listaposicaoInicialCorpos.Add(localizacaoIniJ1T1);
            listaposicaoInicialCorpos.Add(localizacaoIniJ2T1);
            listaposicaoInicialCorpos.Add(localizacaoIniJ1T2);
            listaposicaoInicialCorpos.Add(localizacaoIniJ2T2);

            /************************** SPRITES ***************************/
            // Cria a ViewPort
            controlador.CreateViewport(pictureBox1, campoBackground.Image);
            // Cria os sprites estáticos
            // Sprite da Bola
            controlador.AddSprite(canvas1, localizacaoIniBola);
            // Sprites dos Jogadores do Time 1
            controlador.AddSprite(canvas2, localizacaoIniJ1T1);
            controlador.AddSprite(canvas3, localizacaoIniJ2T1);
            // Sprites dos Jogadores do Time 2
            controlador.AddSprite(canvas4, localizacaoIniJ1T2);
            controlador.AddSprite(canvas5, localizacaoIniJ2T2);
            //controlador.ResizeSprite(1, 0.6);
            /**************************************************************/
        }

        #endregion


        #region Instâncias 

        public static Form1 Instance
        {
            get
            {
                return instance;
            }
        }

        private void InstanciaAmbiente()
        {
            ambiente = new Ambiente.Ambiente(timerAmbiente.Interval, dimencoesBola, dimencoesAgente, limitesDoAmbiente, listaposicaoInicialCorpos, quantidadeCorpos);
            InicializaObservadores();
        }

        #endregion


        #region Observadores 

        public void Atualiza()
        {
            modeloAmbiente = ambiente.modeloAmbiente;
        }

        private void InicializaObservadores()
        {
            ambiente.InicializaObservadores();
        }

        private void IteracaoAmbiente()
        {
            ambiente.IteracaoAmbiente();
        }

        #endregion


        #region Desenha os Pontos de Orientação da Tela

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //this is used only when we have some area that needs to be redrawn
            //like when we start, or min. then restore app, or bring it to front from behind
            //some other app
            controlador.RePaint(sender, e.Graphics, e.ClipRectangle);

            if (checkBoxPontosOrientacao.Checked && ambiente != null)
            {
                // Mostra os pontos de delimitam as regiões do ambiente
                PontosAmbiente(e);
                // Mostra os pontos de delimitam as superfícies dos corpos
                PontosSuperficies(e);
                // Mostra os pontos de colisão
                PontosColisao(e);
            }
        }
        
        private void PontosAmbiente(PaintEventArgs e)
        {
            // Pontos Limites Ambiente

            // Ordem dos pontos - sentido horário
            // Pontos limites do campo esquerdo
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesMeioCampoEsq[0].X, ambiente.modeloAmbiente.ListaLimitesMeioCampoEsq[0].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesMeioCampoEsq[1].X, ambiente.modeloAmbiente.ListaLimitesMeioCampoEsq[1].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesMeioCampoEsq[2].X, ambiente.modeloAmbiente.ListaLimitesMeioCampoEsq[2].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesMeioCampoEsq[3].X, ambiente.modeloAmbiente.ListaLimitesMeioCampoEsq[3].Y, 1, 1);

            // Pontos limites do campo direito
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesMeioCampoDir[0].X, ambiente.modeloAmbiente.ListaLimitesMeioCampoDir[0].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesMeioCampoDir[1].X, ambiente.modeloAmbiente.ListaLimitesMeioCampoDir[1].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesMeioCampoDir[2].X, ambiente.modeloAmbiente.ListaLimitesMeioCampoDir[2].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesMeioCampoDir[3].X, ambiente.modeloAmbiente.ListaLimitesMeioCampoDir[3].Y, 1, 1);

            // Pontos goleira esquerda
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesGoleiraEsq[0].X, ambiente.modeloAmbiente.ListaLimitesGoleiraEsq[0].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesGoleiraEsq[1].X, ambiente.modeloAmbiente.ListaLimitesGoleiraEsq[1].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesGoleiraEsq[2].X, ambiente.modeloAmbiente.ListaLimitesGoleiraEsq[2].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesGoleiraEsq[3].X, ambiente.modeloAmbiente.ListaLimitesGoleiraEsq[3].Y, 1, 1);

            // Pontos goleira direita
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesGoleiraDir[0].X, ambiente.modeloAmbiente.ListaLimitesGoleiraDir[0].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesGoleiraDir[1].X, ambiente.modeloAmbiente.ListaLimitesGoleiraDir[1].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesGoleiraDir[2].X, ambiente.modeloAmbiente.ListaLimitesGoleiraDir[2].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesGoleiraDir[3].X, ambiente.modeloAmbiente.ListaLimitesGoleiraDir[3].Y, 1, 1);

            // Area esquerda 
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesAreaEsq[0].X, ambiente.modeloAmbiente.ListaLimitesAreaEsq[0].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesAreaEsq[1].X, ambiente.modeloAmbiente.ListaLimitesAreaEsq[1].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesAreaEsq[2].X, ambiente.modeloAmbiente.ListaLimitesAreaEsq[2].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesAreaEsq[3].X, ambiente.modeloAmbiente.ListaLimitesAreaEsq[3].Y, 1, 1);

            // Area direita
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesAreaDir[0].X, ambiente.modeloAmbiente.ListaLimitesAreaDir[0].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesAreaDir[1].X, ambiente.modeloAmbiente.ListaLimitesAreaDir[1].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesAreaDir[2].X, ambiente.modeloAmbiente.ListaLimitesAreaDir[2].Y, 1, 1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 2f), ambiente.modeloAmbiente.ListaLimitesAreaDir[3].X, ambiente.modeloAmbiente.ListaLimitesAreaDir[3].Y, 1, 1);
        }

        private void PontosSuperficies(PaintEventArgs e)
        {
            // Pontos Superfície Corpos
            for (int i = 0; i < modeloAmbiente.ListaCorpos.Count; i++)
            {
                e.Graphics.DrawRectangle(new Pen(Color.Red, 2f), modeloAmbiente.ListaCorpos[i].CentroDeMassa.X, modeloAmbiente.ListaCorpos[i].CentroDeMassa.Y, 1, 1);

                if (modeloAmbiente.ListaCorpos[i].ListaPontosSuperficie.Count != 0)
                {
                    for (int j = 0; j < modeloAmbiente.ListaCorpos[i].ListaPontosSuperficie.Count; j++)
                    {
                        if (i < modeloAmbiente.ListaCorpos.Count)
                        {
                            if (j < 2 && i > 0)
                                e.Graphics.DrawRectangle(new Pen(Color.Red, 2f), modeloAmbiente.ListaCorpos[i].ListaPontosSuperficie[j].X, modeloAmbiente.ListaCorpos[i].ListaPontosSuperficie[j].Y, 1, 1);
                            else
                                e.Graphics.DrawRectangle(new Pen(Color.Orange, 2f), modeloAmbiente.ListaCorpos[i].ListaPontosSuperficie[j].X, modeloAmbiente.ListaCorpos[i].ListaPontosSuperficie[j].Y, 1, 1);
                        }
                    }
                }
            }
        }

        private void PontosColisao(PaintEventArgs e)
        {
            // Pontos Colisão Corpos 
            if (modeloAmbiente.ListaPontosColisaoCorpos.Count != 0)
            {
                for (int k = 0; k < modeloAmbiente.ListaPontosColisaoCorpos.Count; k++)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Yellow, 2f), modeloAmbiente.ListaPontosColisaoCorpos[k].X, modeloAmbiente.ListaPontosColisaoCorpos[k].Y, 1, 1);
                }
            }

            // Pontos Colisão Ambiente
            if (modeloAmbiente.ListaPontosColisaoAmbiente.Count != 0)
            {
                for (int k = 0; k < modeloAmbiente.ListaPontosColisaoAmbiente.Count; k++)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Yellow, 2f), modeloAmbiente.ListaPontosColisaoAmbiente[k].X, modeloAmbiente.ListaPontosColisaoAmbiente[k].Y, 1, 1);
                }
            }
        }

        #endregion


        #region Iteração

        private void Iteracao()
        {

            //Stopwatch sw = Stopwatch.StartNew();
                
            IteracaoAmbiente();

            for (int i = 0; i < modeloAmbiente.ListaCorpos.Count; i++)
            {
                controlador.Library.Item(i).oPosition = modeloAmbiente.ListaCorpos[i].Localizacao;
                controlador.Library.Item(i).oFrame = modeloAmbiente.ListaCorpos[i].Sprite;

                controlador.RequestRendering(i);
            }

            // Movimentar o jogador utilizando o teclado
            if (checkBoxMovTeclado.Checked)
            {
                AnimacaoMovimentoTeclado();
            }


            if (ambiente.flagGol)
            {
                lbPlacarTimeAzul.Text = ambiente.golsTimeAzul.ToString();
                lbPlacarTimeVermelho.Text = ambiente.golsTimeVermelho.ToString();

                if (numericUpDownEpisodios.Value == 1)
                {
                    numericUpDownEpisodios.Value = 0;

                    boolIteragir = false;

                    btnPosicMeioCampo.Enabled = true;
                    btnIniciaPartida.Enabled = false;
                    btnPausarPartida.Enabled = false;
                    btnPosicMeioCampo.Enabled = false;

                    ambiente.SalvarDadosQLearning(checkBoxTabelaQGoleiro.Checked, checkBoxTabelaQAtacante.Checked);

                    if (checkBoxLog.Checked)
                    {
                        ambiente.Log();
                    }
                }
                else
                {
                    Reposicionar();

                    ambiente.flagGol = false;
                    //ambiente.LimpaFlagGol();
                    numericUpDownEpisodios.Value -= 1;
                }
            }

            Refresh();

            //Console.WriteLine(sw.ElapsedMilliseconds);
            

            //sw.Stop();

        }

    #endregion


        #region Render

    private void Renderizar()
        {
            if (boolRenderiza)
            {
                controlador.RenderingLoop();

                boolRenderiza = false;
            }
            else
            {
                boolRenderiza = true;
            }
        }

        #endregion


        #region Timer

        private void TimerAmbiente_Tick(object sender, EventArgs e)
        {
            if (boolIteragir)
            {
                //contTimer++;
                //if (contTimer == 1)
                //{
                    Iteracao();
                    //contTimer = 0;
                //}
            }

            Renderizar();
        }

        #endregion


        #region Controles da tela

        // Inicia a simulação
        private void btnIniciaSimulacao_Click(object sender, EventArgs e)
        {
            // Reinicia ambiente
            InstanciaAmbiente();

            checkBoxPontosOrientacao.Enabled = true;

            btnIniciaSimulacao.Enabled = false;
            btnFinalizaSimulacao.Enabled = true;

            btnIniciaPartida.Enabled = true;
            btnPosicMeioCampo.Enabled = true;

            checkBoxTabelaQAtacante.Enabled = true;
            checkBoxTabelaQGoleiro.Enabled = true;
            checkBoxLog.Enabled = true;

            numericUpDownEpisodios.Enabled = true;
            numericUpDownEpisodios.Value = 1;

            Reposicionar();
        }

        // Finaliza simulação
        private void btnFinalizaSimulacao_Click(object sender, EventArgs e)
        {
            /*if (checkBoxLog.Checked)
            {
                ambiente.Log();
                ambiente.log = false;
            }*/

            ambiente.golsTimeAzul = 0;
            ambiente.golsTimeVermelho = 0;

            //ambiente.SalvarDadosQLearning(checkBoxTabelaQGoleiro.Checked, checkBoxTabelaQAtacante.Checked);

            ambiente = null;

            boolIteragir = false;

            checkBoxPontosOrientacao.Enabled = false;

            // Intervalor de atualização fora de jogo
            timerAmbiente.Interval = 100;

            btnIniciaSimulacao.Enabled = true;
            btnFinalizaSimulacao.Enabled = false;

            btnIniciaPartida.Text = "Iniciar Partida";
            btnIniciaPartida.Enabled = false;
            btnPosicMeioCampo.Enabled = false;
            btnPausarPartida.Enabled = false;
            btnReposicBola.Enabled = false;

            checkBoxTabelaQAtacante.Enabled = false;
            checkBoxTabelaQGoleiro.Enabled = false;
            checkBoxLog.Enabled = false;
            checkBoxLog.Checked = false;

            numericUpDownEpisodios.Enabled = false;

            lbPlacarTimeAzul.Text = "0";
            lbPlacarTimeVermelho.Text = "0";
        }

        private void btPosicMeioCampo_Click(object sender, EventArgs e)
        {
            Reposicionar();
        }

        private void Reposicionar()
        {
            ambiente.modeloAmbiente.ListaCorpos[0].Localizacao = new Point(292, 215);
            ambiente.modeloAmbiente.ListaCorpos[1].Localizacao = new Point(40, 200);
            ambiente.modeloAmbiente.ListaCorpos[3].Localizacao = new Point(520, 200);

            if (ambiente.flagGolTimeVermelho)
            {
                ambiente.modeloAmbiente.ListaCorpos[2].Localizacao = new Point(250, 200);
                ambiente.modeloAmbiente.ListaCorpos[4].Localizacao = new Point(370, 200);
            }
            else if (ambiente.flagGolTimeAzul)
            {
                ambiente.modeloAmbiente.ListaCorpos[2].Localizacao = new Point(180, 200);
                ambiente.modeloAmbiente.ListaCorpos[4].Localizacao = new Point(310, 200);
            }

            ambiente.modeloAmbiente.ListaCorpos[0].Sprite = 0;
            ambiente.modeloAmbiente.ListaCorpos[1].Sprite = 0;
            ambiente.modeloAmbiente.ListaCorpos[2].Sprite = 0;
            ambiente.modeloAmbiente.ListaCorpos[3].Sprite = 12;
            ambiente.modeloAmbiente.ListaCorpos[4].Sprite = 12;

            // Zera deslocamento bola
            ambiente.modeloAmbiente.ListaDeslocamentoAtual[0] = new float[,] { { 0, 0 } };
            ambiente.modeloAmbiente.ListaDeslocamentoAtual[1] = new float[,] { { 0, 0 } };
            ambiente.modeloAmbiente.ListaDeslocamentoAtual[2] = new float[,] { { 0, 0 } };
            ambiente.modeloAmbiente.ListaDeslocamentoAtual[3] = new float[,] { { 0, 0 } };
            ambiente.modeloAmbiente.ListaDeslocamentoAtual[4] = new float[,] { { 0, 0 } };

            InicializaObservadores();

            for (int i = 0; i < modeloAmbiente.ListaCorpos.Count; i++)
            {
                controlador.Library.Item(i).oPosition = ambiente.modeloAmbiente.ListaCorpos[i].Localizacao;
                controlador.Library.Item(i).oFrame = ambiente.modeloAmbiente.ListaCorpos[i].Sprite;

                controlador.RequestRendering(i);
            }

            Refresh();

            btnIniciaPartida.Enabled = true;
        }

        private void btIniciaPartida_Click(object sender, EventArgs e)
        {
            btnIniciaPartida.Text = "Iniciar Partida";

            // Intervalo de atualização em jogo
            timerAmbiente.Interval = 10; // (jogo em 10ms)

            if (checkBoxLog.Checked)
            {
                ambiente.log = true;
            }

            boolIteragir = true;

            ambiente.flagGol = false;
            ambiente.flagGolTimeAzul = false;
            ambiente.flagGolTimeVermelho = false;

            btnPosicMeioCampo.Enabled = false;
            btnIniciaPartida.Enabled = false;
            btnReposicBola.Enabled = false;
            btnPausarPartida.Enabled = true;

            checkBoxLog.Enabled = false;
            checkBoxPontosOrientacao.Enabled = false;
            checkBoxTabelaQAtacante.Enabled = false;
            checkBoxTabelaQGoleiro.Enabled = false;

            numericUpDownEpisodios.Enabled = false;   
        }

        private void btnPausarPartida_Click(object sender, EventArgs e)
        {
            btnIniciaPartida.Text = "Continuar Partida";
                
            boolIteragir = false;

            btnIniciaPartida.Enabled = true;
            btnReposicBola.Enabled = true;
            btnPausarPartida.Enabled = false;
        }

        private void btnReposicBola_Click(object sender, EventArgs e)
        {
            ambiente.modeloAmbiente.ListaCorpos[0].Localizacao = new Point(292, 215);
            ambiente.modeloAmbiente.ListaDeslocamentoAtual[0] = new float[,] { { 0, 0 } };

            InicializaObservadores();

            controlador.Library.Item(0).oPosition = ambiente.modeloAmbiente.ListaCorpos[0].Localizacao;
            controlador.Library.Item(0).oFrame = ambiente.modeloAmbiente.ListaCorpos[0].Sprite;

            controlador.RequestRendering(0);

            Refresh();

            btnReposicBola.Enabled = false;
        }

        private void checkBoxPontosOrientacao_CheckedChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        #endregion


        #region Movimentar sprite com o mouse

        // Evento para realocar os sprites clicando para selecionar o sprite
        // e clicando novamente para realoca-lo   
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {                 
            if (arrastarMouse)
            {
                arrastarMouse = false;

                for (int i = 0; i < quantidadeCorpos; i++)
                {
                    if (ambiente == null)
                    {
                        listaposicaoInicialCorpos[i] = controlador.Library.Item(i).oPosition;
                    }
                    else
                    {
                        ambiente.modeloAmbiente.ListaCorpos[i].Localizacao = controlador.Library.Item(i).oPosition;
                    }
                }

                if (ambiente != null)
                {
                    InicializaObservadores();
                }

                Refresh();
                return;
            }
            controlador.StartMouseDrag(e.X, e.Y);
            arrastarMouse = true;
        }

        // Evento para mover o sprite selecionado para uma nova localização
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (arrastarMouse)
            {
                controlador.MoveSelected(e.X, e.Y);
            }
        }

        #endregion


        #region Movimentar Jogador 1 Utilizando Teclado (DEBUG MODE)

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (checkBoxMovTeclado.Checked == false)
            {
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.W:

                    modeloAmbiente.ListaCorpos[1].Sprite = 18;
                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 0] = 0;
                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 1] = -3;

                    paraCima = true;

                    break;

                case Keys.S:

                    modeloAmbiente.ListaCorpos[1].Sprite = 6;
                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 0] = 0;
                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 1] = 3;

                    paraBaixo = true;

                    break;

                case Keys.A:

                    modeloAmbiente.ListaCorpos[1].Sprite = 12;
                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 0] = -3;
                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 1] = 0;

                    paraEsquerda = true;

                    break;

                case Keys.D:

                    modeloAmbiente.ListaCorpos[1].Sprite = 0;
                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 0] = 3;
                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 1] = 0;

                    paraDireita = true;

                    break;

                case Keys.Q:

                    modeloAmbiente.ListaCorpos[1].Sprite = 15;
                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 0] = -3;
                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 1] = -3;

                    paraDireita = true;

                    break;

                case Keys.E:

                    modeloAmbiente.ListaCorpos[1].Sprite = 21;
                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 0] = 3;
                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 1] = -3;

                    paraDireita = true;

                    break;

                case Keys.Z:

                    modeloAmbiente.ListaCorpos[1].Sprite = 9;
                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 0] = -3;
                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 1] = 3;

                    paraDireita = true;

                    break;

                case Keys.X:

                    modeloAmbiente.ListaCorpos[1].Sprite = 3;
                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 0] = 3;
                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 1] = 3;

                    paraDireita = true;

                    break;

                case Keys.F:

                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 0] = 0;
                    modeloAmbiente.ListaDeslocamentoAtual[1][0, 1] = 0;

                    paraDireita = true;

                    break;

                case Keys.F1:
                    /* Ação 1 - Goleiro */
                    ambiente.listaAgentes[0].Posicionar();

                    break;

                case Keys.F2:
                    /* Ação 2 - Goleiro */
                    ambiente.listaAgentes[0].Defender();

                    break;

                case Keys.F3:
                    /* Ação 1 - Atacante */
                    ambiente.listaAgentes[0].Chutar();

                    break;

                case Keys.F4:
                    /* Ação 2 - Atacante */
                    ambiente.listaAgentes[0].ConduzirBola();

                    break;

                case Keys.F5:
                    /* Ação 4 - Atacante */
                    ambiente.listaAgentes[0].Desarmar();

                    break;

                case Keys.F6:
                    /* Ação 4 - Atacante */
                    ambiente.listaAgentes[0].Desviar();

                    break;                    
            }
        }

        private void Cima()
        {
            if (controlador.Library.Item(1).oFrame < 18 && controlador.Library.Item(1).oFrame > 6)
            {
                controlador.Library.Item(1).oFrame++;
            }
            else if (controlador.Library.Item(1).oFrame <= 6)
            {
                controlador.Library.Item(1).oFrame--;
            }
            else if (controlador.Library.Item(1).oFrame > 18)
            {
                controlador.Library.Item(1).oFrame--;
            }
            else
            {
                paraCima = false;
            }
        }

        private void Baixo()
        {
            if (controlador.Library.Item(1).oFrame < 18 && controlador.Library.Item(1).oFrame > 6)
            {
                controlador.Library.Item(1).oFrame--;
            }
            else if (controlador.Library.Item(1).oFrame < 6)
            {
                controlador.Library.Item(1).oFrame++;
            }
            else if (controlador.Library.Item(1).oFrame >= 18)
            {
                controlador.Library.Item(1).oFrame++;
            }
            else
            {
                paraBaixo = false;
            }
        }

        private void Esquerda()
        {
            if (controlador.Library.Item(1).oFrame < 23 && controlador.Library.Item(1).oFrame > 12)
            {
                controlador.Library.Item(1).oFrame--;
            }
            else if (controlador.Library.Item(1).oFrame < 12)
            {
                controlador.Library.Item(1).oFrame++;
            }
            else
            {
                paraEsquerda = false;
            }
        }

        private void Direita()
        {
            if (controlador.Library.Item(1).oFrame > 0 && controlador.Library.Item(1).oFrame < 12)
            {
                controlador.Library.Item(1).oFrame--;
            }
            else if (controlador.Library.Item(1).oFrame >= 12)
            {
                controlador.Library.Item(1).oFrame++;
            }
            else
            {
                paraDireita = false;
            }
        }

        private void AnimacaoMovimentoTeclado()
        {
            if (auxDelayDirecao == 0)
            {
                auxDelayDirecao = 0;

                if (paraCima)
                {
                    Cima();
                }
                else if (paraBaixo)
                {
                    Baixo();
                }
                else if (paraEsquerda)
                {
                    Esquerda();
                }
                else if (paraDireita)
                {
                    Direita();
                }
            }
            else auxDelayDirecao++;
        }

        #endregion
    }
}
