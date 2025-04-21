using System;
using System.Collections.Generic;
using System.Drawing;

namespace AmbienteDeSimulacao.Agente
{
    class Agente
    {
        // Força do chute
        private const int forcaMinChute = 10;
        private const int forcaMaxChute = 20;

        // Deslocamento do agente
        private int deslocAgente = 1;
        private const int deslocMinAgente = 1;
        private const int deslocMaxAgente = 3;        

        private double deltaX;
        private double deltaY;

        private int meuTime;
        private float[,] meuDeslocamento;
        private ModeloAgente modeloAgente;
        //private Rectangle limitesDoAmbiente;
        private Ambiente.FisicaAmbiente.Corpos.Corpo meuCorpo;

        private Point pOrientacaoDesvio = new Point();
        private Point objetivoTemporarioAux = new Point();

        Ambiente.ModeloAmbiente modeloAmbiente;


        #region Construtor

        public Agente(int id, float massaAgente, int[,] dimencoes, Point localizacao, Rectangle limitesDoAmbiente, bool meuControlador, int meuTime, Ambiente.ModeloAmbiente modeloAmbiente)
        {
            //meuCorpo = new Ambiente.FisicaAmbiente.Corpos.Corpo(id, "Agente", 0, massaAgente, dimencoes, localizacao);

            meuDeslocamento = new float[1, 2];

            // Informa o agente os limites de jogo
            //this.limitesDoAmbiente = limitesDoAmbiente;

            // Atribui o time do agente
            this.meuTime = meuTime;

            // Ajusta a direção inicial do agente condorme seu time
            if (meuTime == 1)
            {
                //MeuCorpo.Sprite = 0;
                meuCorpo = new Ambiente.FisicaAmbiente.Corpos.Corpo(id, "Agente", 0, massaAgente, dimencoes, localizacao);
            }
            else if (meuTime == 2)
            {
                //MeuCorpo.Sprite = 12;
                meuCorpo = new Ambiente.FisicaAmbiente.Corpos.Corpo(id, "Agente", 12, massaAgente, dimencoes, localizacao);
            }

            this.modeloAmbiente = modeloAmbiente;

            /*************** Instância Modelo *****************/
            modeloAgente = new ModeloAgente();
            /**************************************************/
        }

        #endregion


        #region Getters e Setters

        public ModeloAgente GetModeloAgente
        {
            get
            {
                return modeloAgente;
            }
        }

        public Ambiente.FisicaAmbiente.Corpos.Corpo MeuCorpo
        {
            get
            {
                return meuCorpo;
            }
            set
            {
                meuCorpo = value;
            }
        }

        public float[,] GetMeuDeslocamento
        {
            get
            {
                return meuDeslocamento;
            }
        }

        public int GetMeuTime
        {
            get { return meuTime; }
        }

        #endregion


        #region Agir

        /* Método que dispara o processo de "agir" do agente */
        public void Agir(Point objetivo, string acao)
        {
            modeloAgente.objetivoQLearning = objetivo;

            switch (acao)
            {
                // Ações do Goleiro
                case "Defender":
                    Defender();
                    break;

                case "Posicionar":
                    Posicionar();
                    break;

                case "Reposicao":
                    Reposicao();
                    break;

                case "Passe":
                    Passe();
                    break;

                // Ações do Atacante
                case "Chutar":
                    Chutar();
                    break;

                case "Conduzir":
                    ConduzirBola();
                    break;

                case "Driblar":
                    Driblar();
                    break;

                case "Desarmar":
                    Desarmar();
                    break;

                case "Dominar":
                    DominarBola();
                    break;

                case "Fintar":
                    Fintar();
                    break;
            }
        }

        #endregion


        #region Ir Para - Vai até um ponto objetivo

        // 24 Sprites defindo uma rotação de 360º (8 utilizados por falta de resolução)
        private void IrPara(Point objetivo)
        {
            // Delta objetivo / centro de massa
            deltaX = objetivo.X - MeuCorpo.CentroDeMassa.X;
            deltaY = objetivo.Y - MeuCorpo.CentroDeMassa.Y;

            modeloAgente.movendoPara = true;

            /*if (deltaX == 0)
            {
                theta = Math.PI / 2;
            }
            else
            {
                theta = Math.Atan(deltaY / deltaX);
            }*/

            if (Math.Abs(deltaX) > 10 || Math.Abs(deltaY) > 10) // 10
            {
                if (deltaX >= 0 && Math.Abs(deltaY) <= 10) // 20
                {
                    //meuCorpo.Sprite = 0;
                    RotacionarCorpo(meuCorpo.Sprite, 0);
                }
                else if (deltaX > 0 && deltaY > 0) // 0
                {
                    //meuCorpo.Sprite = 3;
                    RotacionarCorpo(meuCorpo.Sprite, 3);
                }
                else if (Math.Abs(deltaX) <= 15 && deltaY > 0) // 15
                {
                    //meuCorpo.Sprite = 6;
                    RotacionarCorpo(meuCorpo.Sprite, 6);
                }
                else if (deltaX < 0 && deltaY > 0)
                {
                    //meuCorpo.Sprite = 9;
                    RotacionarCorpo(meuCorpo.Sprite, 9);
                }
                else if (deltaX < 0 && Math.Abs(deltaY) <= 15 && deltaY > -5)
                {
                    //meuCorpo.Sprite = 12;
                    RotacionarCorpo(meuCorpo.Sprite, 12);
                }
                else if (Math.Abs(deltaX) <= 15 && deltaY < 0) // 15
                {
                    //meuCorpo.Sprite = 18;
                    RotacionarCorpo(meuCorpo.Sprite, 18);
                }
                else if (deltaX > 0 && deltaY < 0)
                {
                    //meuCorpo.Sprite = 21;
                    RotacionarCorpo(meuCorpo.Sprite, 21);
                }
                else if (deltaX < 0 && deltaY < 0)
                {
                    //meuCorpo.Sprite = 15;
                    RotacionarCorpo(meuCorpo.Sprite, 15);
                }

                Mover();
            }
            else
            {
                Parar();
                modeloAgente.movendoPara = false;
            }
        }

        private void RotacionarCorpo(int spriteAtual, int spriteDesejado)
        {
            if (spriteAtual == 0 && spriteDesejado > 12)
            {
                spriteAtual = 24;
            }
            else if (spriteAtual > 12 && spriteDesejado == 0)
            {
                spriteDesejado = 24;
            }

            int deltaSprite = spriteDesejado - spriteAtual;

            if (deltaSprite == 0)
            {
                deslocAgente = deslocMaxAgente;
                return;
            }else
            {
                deslocAgente = deslocMinAgente;
            }

            if (deltaSprite > 12 || deltaSprite < 0)  
            {
                meuCorpo.Sprite--;
            }
            else if (deltaSprite <= 12) 
            {
                meuCorpo.Sprite++;
            }

            // Ajuste primeiro e último sprite
            if (meuCorpo.Sprite == 24)
            {
                meuCorpo.Sprite = 0;
            }
            else if (meuCorpo.Sprite == -1)
            {
                meuCorpo.Sprite = 23;
            }
        }

        private void Mover()
        {
            switch (meuCorpo.Sprite)
            {
                case 0:

                    meuDeslocamento[0, 0] = deslocAgente;
                    meuDeslocamento[0, 1] = 0;
                    break;

                case 3:

                    meuDeslocamento[0, 0] = deslocAgente;
                    meuDeslocamento[0, 1] = deslocAgente;
                    break;

                case 6:
                    meuDeslocamento[0, 0] = 0;
                    meuDeslocamento[0, 1] = deslocAgente;
                    break;

                case 9:

                    meuDeslocamento[0, 0] = -deslocAgente;
                    meuDeslocamento[0, 1] = deslocAgente;
                    break;

                case 12:

                    meuDeslocamento[0, 0] = -deslocAgente;
                    meuDeslocamento[0, 1] = 0;
                    break;

                case 15:

                    meuDeslocamento[0, 0] = -deslocAgente;
                    meuDeslocamento[0, 1] = -deslocAgente;
                    break;

                case 18:

                    meuDeslocamento[0, 0] = 0;
                    meuDeslocamento[0, 1] = -deslocAgente;
                    break;

                case 21:

                    meuDeslocamento[0, 0] = deslocAgente;
                    meuDeslocamento[0, 1] = -deslocAgente;
                    break;
            }
        }

        private void Parar()
        {
            meuDeslocamento[0, 0] = 0;
            meuDeslocamento[0, 1] = 0;
        }

        #endregion


        #region Desviar

        public void Desviar()
        {
            /* Ponto dos limites da detecção de corpos */
            Point pv0 = new Point(meuCorpo.ListaPontosSuperficie[0].X, meuCorpo.ListaPontosSuperficie[0].Y);
            Point pv1 = new Point(meuCorpo.ListaPontosSuperficie[1].X, meuCorpo.ListaPontosSuperficie[1].Y);

            int distDeteccao = 25;
            int distDeteccaoCos45 = Convert.ToInt32(distDeteccao * Math.Cos(Math.PI/4)); //28;
            int teste = 70;

            string direcao = DirecaoAgente();

            switch (direcao)
            {
                case "S":
                    // Agente está direcionado para baixo (Sul)
                    pv0.Y += distDeteccao;
                    pv1.Y += distDeteccao;
                    break;

                case "N":
                    // Agente está direcionado para cima (Norte)
                    pv0.Y -= distDeteccao;
                    pv1.Y -= distDeteccao;
                    break;

                case "L":
                    // Agente está direcionado para direita (Leste)
                    pv0.X += distDeteccao;
                    pv1.X += distDeteccao;
                    break;

                case "O":
                    // Agente está direcionado para esquerda (Oeste) 
                    pv0.X -= distDeteccao;
                    pv1.X -= distDeteccao;
                    break;

                case "SE":
                    // Agente direcionado para o (SE)
                    pv0.X += distDeteccaoCos45;
                    pv0.Y += distDeteccaoCos45;
                    pv1.X += distDeteccaoCos45;
                    pv1.Y += distDeteccaoCos45;
                    break;

                case "SO":
                    // Agente direcionado para o (SO)
                    pv0.X -= distDeteccaoCos45;
                    pv0.Y += distDeteccaoCos45;
                    pv1.X -= distDeteccaoCos45;
                    pv1.Y += distDeteccaoCos45;
                    break;

                case "NE":
                    // Agente direcionado para o (NE)
                    pv0.X += distDeteccaoCos45;
                    pv0.Y -= distDeteccaoCos45;
                    pv1.X += distDeteccaoCos45;
                    pv1.Y -= distDeteccaoCos45;
                    break;

                case "NO":
                    // Agente direcionado para o (NO)
                    pv0.X -= distDeteccaoCos45;
                    pv0.Y -= distDeteccaoCos45;
                    pv1.X -= distDeteccaoCos45;
                    pv1.Y -= distDeteccaoCos45;
                    break;
            }


            Ambiente.FisicaAmbiente.Utils.InterseccaoSegmentoReta interSegmentoReta = new Ambiente.FisicaAmbiente.Utils.InterseccaoSegmentoReta();
            List<List<Point>> listaPontosSuperficies = new List<List<Point>>();

            List<Point> listaPontosDeteccao = new List<Point>();

            listaPontosDeteccao.Add(meuCorpo.ListaPontosSuperficie[0]);
            listaPontosDeteccao.Add(pv0);
            listaPontosDeteccao.Add(pv1);
            listaPontosDeteccao.Add(meuCorpo.ListaPontosSuperficie[1]);

            bool colisao = false;

            //int idx = 0;
            int pAux = 0;
            int kAux = 0;

            if (listaPontosSuperficies.Count != 0)
            {
                listaPontosSuperficies.Clear();
            }

            // Carrega lista de pontos que definem a superfícies
            foreach (Ambiente.FisicaAmbiente.Corpos.Corpo c in modeloAmbiente.ListaCorpos)
            {
                if (c.GetId != meuCorpo.GetId) // Não testa os pontos do corpo do agente
                {
                    listaPontosSuperficies.Add(c.ListaPontosSuperficie);
                }
                else
                {
                    listaPontosSuperficies.Add(listaPontosDeteccao);
                }
            }

            listaPontosSuperficies.Add(modeloAmbiente.ListaLimitesDoAmbiente);


            if (modeloAmbiente.ListaCorposColisao.Count != 0)
            {
                modeloAmbiente.ListaCorposColisao.Clear();
                modeloAmbiente.ListaPontosColisaoCorpos.Clear();
                modeloAmbiente.ListaPontosColisaoAmbiente.Clear();
            }

            if (modeloAgente.desviar == false)
            {
                pOrientacaoDesvio = meuCorpo.CentroDeMassa;
            }


            // Laço para acessar cada ponto da superfície que está sendo testada
            for (int p = 0; p <= 2 /*listaPontosSuperficies[meuCorpo.GetId].Count - 1*/ ; p += 2)
            {
                // Laço para acessar a outra superfície
                for (int j = 0; j < listaPontosSuperficies.Count; j++)
                {
                    if (j != meuCorpo.GetId && j != 0) // Não leva em consideração de desvio se for a bola e não testa colisão com o próprio corpo
                    {
                        // Laço para acessar cada ponto das outras supercícies
                        for (int k = 0; k < listaPontosSuperficies[j].Count; k++)
                        {
                            pAux = p + 1;

                            // Teste necessário para calcular a colisão com o último 
                            // segmento de linha (ultimo ponto com o primeiro ponto)
                            if (k == listaPontosSuperficies[j].Count - 1)
                            {
                                kAux = 0;
                            }
                            else
                            {
                                kAux = k + 1;
                            }

                            // Se os corpos testados se colidem em algum ponto, armazena o indice do corpo
                            colisao = interSegmentoReta.FasterLineSegmentIntersection(listaPontosSuperficies[meuCorpo.GetId][p],
                                                                                      listaPontosSuperficies[meuCorpo.GetId][pAux],
                                                                                      listaPontosSuperficies[j][k],
                                                                                      listaPontosSuperficies[j][kAux]);
                            if (colisao)
                            {
                                // DESVIAR 
                                modeloAgente.desviar = true;

                                if (listaPontosSuperficies[meuCorpo.GetId][p] == meuCorpo.ListaPontosSuperficie[0]
                                && listaPontosSuperficies[meuCorpo.GetId][pAux] == pv0)
                                {
                                    RotacionarCorpo(meuCorpo.Sprite, meuCorpo.Sprite - 6);

                                    direcao = DirecaoAgente();

                                    switch (direcao)
                                    {
                                        case "S":
                                            // Agente está direcionado para baixo (Sul)
                                            //meuCorpo.Sprite = 0;
                                            objetivoTemporarioAux.X = (pOrientacaoDesvio.X + teste);
                                            objetivoTemporarioAux.Y = (pOrientacaoDesvio.Y + teste);
                                            break;

                                        case "N":
                                            // Agente está direcionado para cima (Norte)
                                            //meuCorpo.Sprite = 12;
                                            objetivoTemporarioAux.X = (pOrientacaoDesvio.X - teste);
                                            objetivoTemporarioAux.Y = (pOrientacaoDesvio.Y - teste);
                                            break;

                                        case "L":
                                            // Agente está direcionado para direita (Leste)
                                            //meuCorpo.Sprite = 18;
                                            objetivoTemporarioAux.X = (pOrientacaoDesvio.X + teste);  //pOrientacaoDesvio.X;
                                            objetivoTemporarioAux.Y = (pOrientacaoDesvio.Y - teste);
                                            break;

                                        case "O":
                                            // Agente está direcionado para esquerda (Oeste) 
                                            //meuCorpo.Sprite = 6;
                                            objetivoTemporarioAux.X = (pOrientacaoDesvio.X - teste);
                                            objetivoTemporarioAux.Y = (pOrientacaoDesvio.Y + teste);
                                            break;

                                        case "SE":
                                            // Agente direcionado para o (SE)
                                            //meuCorpo.Sprite = 21;
                                            objetivoTemporarioAux.X = pOrientacaoDesvio.X + teste;
                                            objetivoTemporarioAux.Y = meuCorpo.CentroDeMassa.Y;
                                            //objeticoTemporarioAux.Y += distDeteccaoCos45;
                                            break;

                                        case "SO":
                                            // Agente direcionado para o (SO)
                                            //meuCorpo.Sprite = 3;
                                            //objeticoTemporarioAux.X -= distDeteccaoCos45;
                                            objetivoTemporarioAux.X = meuCorpo.CentroDeMassa.X;
                                            objetivoTemporarioAux.Y = pOrientacaoDesvio.Y + teste;
                                            break;

                                        case "NE":
                                            // Agente direcionado para o (NE)
                                            // meuCorpo.Sprite = 15;
                                            //objeticoTemporarioAux.X += distDeteccaoCos45;
                                            objetivoTemporarioAux.X = meuCorpo.CentroDeMassa.X;
                                            objetivoTemporarioAux.Y = pOrientacaoDesvio.Y - teste;
                                            break;

                                        case "NO":
                                            // Agente direcionado para o (NO)
                                            //meuCorpo.Sprite = 9;
                                            objetivoTemporarioAux.X = pOrientacaoDesvio.X - teste;
                                            objetivoTemporarioAux.Y = meuCorpo.CentroDeMassa.Y;
                                            //objeticoTemporarioAux.Y += distDeteccaoCos45;
                                            break;
                                    }
                                }
                                else if (listaPontosSuperficies[meuCorpo.GetId][p] == pv1
                                && listaPontosSuperficies[meuCorpo.GetId][pAux] == meuCorpo.ListaPontosSuperficie[1])
                                {
                                    RotacionarCorpo(meuCorpo.Sprite, meuCorpo.Sprite + 6);

                                    switch (direcao)
                                    {
                                        case "S":
                                            // Agente está direcionado para baixo (Sul)
                                            //meuCorpo.Sprite = 12;
                                            objetivoTemporarioAux.X = (pOrientacaoDesvio.X - teste);
                                            objetivoTemporarioAux.Y = (pOrientacaoDesvio.Y + teste);
                                            break;

                                        case "N":
                                            // Agente está direcionado para cima (Norte)
                                            //meuCorpo.Sprite = 0;
                                            objetivoTemporarioAux.X = (pOrientacaoDesvio.X + teste);
                                            objetivoTemporarioAux.Y = (pOrientacaoDesvio.Y - teste);
                                            break;

                                        case "L":
                                            // Agente está direcionado para direita (Leste)
                                            //meuCorpo.Sprite = 6;
                                            objetivoTemporarioAux.X = (pOrientacaoDesvio.X + teste);
                                            objetivoTemporarioAux.Y = (pOrientacaoDesvio.Y + teste);
                                            break;

                                        case "O":
                                            // meuCorpo.Sprite = 18;
                                            // Agente está direcionado para esquerda (Oeste) 
                                            objetivoTemporarioAux.X = (pOrientacaoDesvio.X - teste);
                                            objetivoTemporarioAux.Y = (pOrientacaoDesvio.Y - teste);
                                            break;

                                        case "SE":
                                            // Agente direcionado para o (SE)
                                            objetivoTemporarioAux.X = meuCorpo.CentroDeMassa.X;
                                            objetivoTemporarioAux.Y = pOrientacaoDesvio.Y + teste;
                                            break;

                                        case "SO":
                                            // Agente direcionado para o (SO)
                                            objetivoTemporarioAux.X = pOrientacaoDesvio.X - teste;
                                            objetivoTemporarioAux.Y = meuCorpo.CentroDeMassa.Y;
                                            break;

                                        case "NE":
                                            // Agente direcionado para o (NE)
                                            objetivoTemporarioAux.X = pOrientacaoDesvio.X + teste;
                                            objetivoTemporarioAux.Y = meuCorpo.CentroDeMassa.Y;
                                            break;

                                        case "NO":
                                            // Agente direcionado para o (NO)
                                            objetivoTemporarioAux.X = meuCorpo.CentroDeMassa.X;
                                            objetivoTemporarioAux.Y = pOrientacaoDesvio.Y - teste;
                                            break;
                                    }
                                }
                                objetivoTemporarioAux = LimitesAtacante(objetivoTemporarioAux);

                                break;
                            }
                        }
                    }
                    if (colisao)
                    {
                        break;
                    }
                }
                if (colisao)
                {
                    break;
                }
            }

            if (modeloAgente.desviar == false)
            {
                modeloAgente.objetivoTemporario = modeloAgente.objetivoQLearning;
            }
            else
            {
                if (modeloAgente.movendoPara)
                {
                    modeloAgente.objetivoTemporario = objetivoTemporarioAux;
                }
                else
                {
                    modeloAgente.desviar = false;
                    modeloAgente.objetivoTemporario = modeloAgente.objetivoQLearning;
                }
            }
        }

        private Point LimitesAtacante(Point objetivo)
        {
            if (objetivo.Y < modeloAmbiente.ListaLimitesDoAmbiente[2].Y
            || objetivo.Y > modeloAmbiente.ListaLimitesDoAmbiente[5].Y)
            {
                // Limites do atacante 
                if (objetivo.X < modeloAmbiente.ListaLimitesDoAmbiente[0].X)
                {
                    objetivo.X = modeloAmbiente.ListaLimitesDoAmbiente[0].X;
                }
                else if (objetivo.X > modeloAmbiente.ListaLimitesDoAmbiente[1].X)
                {
                    objetivo.X = modeloAmbiente.ListaLimitesDoAmbiente[1].X;
                }
            }

            if (objetivo.Y > modeloAmbiente.ListaLimitesDoAmbiente[6].Y)
            {
                objetivo.Y = modeloAmbiente.ListaLimitesDoAmbiente[6].Y;
            }
            else if (objetivo.Y < modeloAmbiente.ListaLimitesDoAmbiente[0].Y)
            {
                objetivo.Y = modeloAmbiente.ListaLimitesDoAmbiente[0].Y;
            }

            return objetivo;
        }

        #endregion


        #region Direção do Agente

        public string DirecaoAgente()
        {
            float deltaX = meuCorpo.ListaPontosSuperficie[4].X - MeuCorpo.CentroDeMassa.X;
            float deltaY = meuCorpo.ListaPontosSuperficie[4].Y - MeuCorpo.CentroDeMassa.Y;

            // Agente movendo-se apenas no eixo Y
            if (deltaX == 0)
            {
                if (deltaY > 0)
                {
                    // Agente está direcionado para baixo (Sul)
                    return "S";
                }
                else if (deltaY < 0)
                {
                    // Agente está direcionado para cima (Norte)
                    return "N";
                }
            } // Agente movendo-se apenas no eixo X
            else if (deltaY == 0)
            {
                if (deltaX > 0)
                {
                    // Agente está direcionado para direita (Leste)
                    return "L";
                }
                else if (deltaX < 0)
                {
                    // Agente está direcionado para esquerda (Oeste) 
                    return "O";
                }
            }
            else
            {
                // deltaX != de zero e deltaY != de zero
                // Está se movendo nas diagonais
                if (deltaX > 0 && deltaY > 0)
                {
                    // Agente direcionado para o (SE)
                    return "SE";
                }
                else if (deltaX < 0 && deltaY > 0)
                {
                    // Agente direcionado para o (SO)
                    return "SO";
                }
                else if (deltaX > 0 && deltaY < 0)
                {
                    // Agente direcionado para o (NE)
                    return "NE";
                }
                else if (deltaX < 0 && deltaY < 0)
                {
                    // Agente direcionado para o (NO)
                    return "NO";
                }
            }

            return "erro";
        }

        #endregion


        #region Conduzir a bola 

        /*
            Manipulação direta da propriedade dentro de massa da bola
            e do deslocamento da bola, para que a mesma acompanhe o agente.            
        */
        public void ConduzirBola()
        {
            float deltaX = modeloAmbiente.ListaCorpos[0].CentroDeMassa.X - meuCorpo.ListaPontosSuperficie[4].X;
            float deltaY = modeloAmbiente.ListaCorpos[0].CentroDeMassa.Y - meuCorpo.ListaPontosSuperficie[4].Y;

            Ambiente.FisicaAmbiente.Utils.Vetor2 vDistancia = new Ambiente.FisicaAmbiente.Utils.Vetor2(deltaX, deltaY);

            float distancia = vDistancia.Length();

            // Verifica se a distância está respeitando o limiar para conduzir a bola
            if (distancia > 25)
            {
                return;
            }

            // Tratativa para que quando há colisão entre a bola e 
            // outro jogador o atacante perca o domínio da mesma
            if (modeloAmbiente.idPosseBola == meuCorpo.GetId 
            && modeloAmbiente.idColisaoBola != meuCorpo.GetId
            && modeloAmbiente.idColisaoBola > 0
            && modeloAmbiente.idColisaoBola < modeloAmbiente.ListaCorpos.Count)
            {
                return;
            }

            // Ajusta as propriedades físicas da bola
            DominioBola();

            string direcao = DirecaoAgente();

            switch (direcao)
            {
                case "S":
                    // Agente está direcionado para baixo (Sul)
                    modeloAmbiente.ListaCorpos[0].Localizacao = new Point(meuCorpo.ListaPontosSuperficie[4].X - 10, meuCorpo.ListaPontosSuperficie[4].Y + 4);
                    break;

                case "N":
                    // Agente está direcionado para cima (Norte)
                    modeloAmbiente.ListaCorpos[0].Localizacao = new Point(meuCorpo.ListaPontosSuperficie[4].X - 10, meuCorpo.ListaPontosSuperficie[4].Y - 24);
                    break;

                case "L":
                    // Agente está direcionado para direita (Leste)
                    modeloAmbiente.ListaCorpos[0].Localizacao = new Point(meuCorpo.ListaPontosSuperficie[4].X + 4, meuCorpo.ListaPontosSuperficie[4].Y - 10);
                    break;

                case "O":
                    // Agente está direcionado para esquerda (Oeste) 
                    modeloAmbiente.ListaCorpos[0].Localizacao = new Point(meuCorpo.ListaPontosSuperficie[4].X - 24, meuCorpo.ListaPontosSuperficie[4].Y - 10);
                    break;

                case "SE":
                    // Agente direcionado para o (SE)
                    modeloAmbiente.ListaCorpos[0].Localizacao = new Point(meuCorpo.ListaPontosSuperficie[4].X - 2, meuCorpo.ListaPontosSuperficie[4].Y + 4);
                    break;

                case "SO":
                    // Agente direcionado para o (SO)
                    modeloAmbiente.ListaCorpos[0].Localizacao = new Point(meuCorpo.ListaPontosSuperficie[4].X - 22, meuCorpo.ListaPontosSuperficie[4].Y);
                    break;

                case "NE":
                    // Agente direcionado para o (NE)
                    modeloAmbiente.ListaCorpos[0].Localizacao = new Point(meuCorpo.ListaPontosSuperficie[4].X + 3, meuCorpo.ListaPontosSuperficie[4].Y - 19);
                    break;

                case "NO":
                    // Agente direcionado para o (NO)
                    modeloAmbiente.ListaCorpos[0].Localizacao = new Point(meuCorpo.ListaPontosSuperficie[4].X - 18, meuCorpo.ListaPontosSuperficie[4].Y - 22);
                    break;
            }

            // Indicador de condução
            modeloAgente.conduzir = true;

            // Seta o id do jogador com o dominio da bola
            modeloAmbiente.idPosseBola = meuCorpo.GetId;
        }

        private void DominioBola()
        {
            modeloAmbiente.ListaAcumDeslocamento[0][0, 0] = meuDeslocamento[0, 0];
            modeloAmbiente.ListaAcumDeslocamento[0][0, 1] = meuDeslocamento[0, 1];
            modeloAmbiente.ListaDeslocamentoAnterior[0] = new float[1, 2] { { 0, 0 } };
        }

        #endregion


        #region Ajusta velocidade do atacante

        /*private void AjustaVelocAtacante()
        {
            if (meuCorpo.GetId == modeloAmbiente.idPosseBola)
            {
                deslocAgente = deslocMinAtacante;
            }
            else
            {
                deslocAgente = deslocMaxAtacante;
            }
        }*/

        #endregion


        #region Defender - Goleiro Ação 1 

        /*
            Verifica se a bola está a uma distância que uma defesa deve ser preparada
            se sim prepara para se defender em um ponto estimado pela trajetória da bola 
        */
        public void Defender()
        {
            Point pDefesa;

            int deltaCmX = Math.Abs(meuCorpo.CentroDeMassa.X - modeloAmbiente.ListaCorpos[0].CentroDeMassa.X);

            if (meuTime == 1) // Time Azul 
            {
                if (modeloAmbiente.ListaDeslocamentoAtual[0][0, 0] < 0
                && deltaCmX < 150)
                {
                    pDefesa = new Point(70, modeloAmbiente.ListaCorpos[0].CentroDeMassa.Y);
                 }
                 else if (modeloAmbiente.ListaDeslocamentoAtual[0][0, 0] < 0
                      && deltaCmX >= 150)
                {
                    pDefesa = new Point(modeloAmbiente.ListaLimitesAreaEsq[1].X, modeloAmbiente.ListaCorpos[0].CentroDeMassa.Y);
                }
                else
                {
                    pDefesa = new Point(60, 220);
                }

                // Limita área de atuação do goleiro
                pDefesa = AreaAtuacaoGoleiroTimeAzul(pDefesa);
            }
            else // Time Vermelho
            {
                if (modeloAmbiente.ListaDeslocamentoAtual[0][0, 0] > 0
                && deltaCmX < 150)
                {
                    pDefesa = new Point(550, modeloAmbiente.ListaCorpos[0].CentroDeMassa.Y);
                }
                else if (modeloAmbiente.ListaDeslocamentoAtual[0][0, 0] > 0
                     && deltaCmX >= 150)
                {
                    pDefesa = new Point(modeloAmbiente.ListaLimitesAreaDir[0].X, modeloAmbiente.ListaCorpos[0].CentroDeMassa.Y);
                }
                else
                {
                    pDefesa = new Point(540, 220);
                }

                // Limita área de atuação do goleiro
                pDefesa = AreaAtuacaoGoleiroTimeVermelho(pDefesa);
            }        

            // Movimento o goleiro até o ponto de defesa
            IrPara(pDefesa);

            if (!modeloAgente.movendoPara)
            {
                DirecionarCorpo();
            }

            SinalizaDefesa();
        }

        // Método necessário para a introdução da emoção no agente
        // quando uma defesa é sinalizada podemos saber se posteriormento
        // houve um gol ou não, decidindo se a ação do goleiro foi efetiva.
        private void SinalizaDefesa()
        {
            // Se a bola bater no goleiro ele está com a posse de bola
            if (modeloAmbiente.idColisaoBola == meuCorpo.GetId)
            {
                modeloAmbiente.idDefesa = meuCorpo.GetId;
            }
        }

        private Point AreaAtuacaoGoleiroTimeAzul(Point objetivo)
        {
            // Limites dos pontos de defesa (limites da área do gol)
            if (objetivo.X < modeloAmbiente.ListaLimitesAreaEsq[0].X)
            {
                objetivo.X = modeloAmbiente.ListaLimitesAreaEsq[0].X + 25;
            }
            else if (objetivo.X > modeloAmbiente.ListaLimitesAreaEsq[1].X)
            {
                objetivo.X = modeloAmbiente.ListaLimitesAreaEsq[1].X;                
            }
            if (objetivo.Y > modeloAmbiente.ListaLimitesAreaEsq[2].Y)
            {
                objetivo.Y = modeloAmbiente.ListaLimitesAreaEsq[2].Y;
            }
            else if (objetivo.Y < modeloAmbiente.ListaLimitesAreaEsq[1].Y)
            {
                objetivo.Y = modeloAmbiente.ListaLimitesAreaEsq[1].Y;
            }

            return objetivo;
        }

        private Point AreaAtuacaoGoleiroTimeVermelho(Point objetivo)
        {
            // Limites dos pontos de defesa (limites da área do gol)
            if (objetivo.X < modeloAmbiente.ListaLimitesAreaDir[1].X)
            {
                objetivo.X = modeloAmbiente.ListaLimitesAreaDir[1].X - 25;
            }
            else if (objetivo.X > modeloAmbiente.ListaLimitesAreaEsq[0].X)
            {
                objetivo.X = modeloAmbiente.ListaLimitesAreaDir[0].X;
            }
            if (objetivo.Y > modeloAmbiente.ListaLimitesAreaDir[3].Y)
            {
                objetivo.Y = modeloAmbiente.ListaLimitesAreaDir[3].Y;
            }
            else if (objetivo.Y < modeloAmbiente.ListaLimitesAreaDir[0].Y)
            {
                objetivo.Y = modeloAmbiente.ListaLimitesAreaDir[0].Y;
            }

            return objetivo;
        }

        #endregion


        #region Posicionar - Goleiro Ação 2

        /* 
            Posicionar: 
            A ação posicionar faz com que o goleiro ajuste a sua posição, para o ponto definido inicialmente
            para a sua função.
            Para realizar esta ação o goleiro vai utilizar o método DirecionarCorpos da classe Agente para 
            deslocar-se até as coordenadas do ponto, e ficar de costas para o gol
        */
        public void Posicionar()
        {
            //deslocAgente = deslocMinAtacante;
            // Move o goleiro para o centro do gol
            IrPara(modeloAgente.objetivoQLearning);

            if (!modeloAgente.movendoPara)
            {
				// Direciona o corpo do goleiro
                DirecionarCorpo();
            }
        }

        private void DirecionarCorpo()
        {
            /* 
                Testa se o agente está na direção correta,
                ajusta sua direção de acordo com o time que ele joga             
            */
            if (meuTime == 1)
            {
                meuCorpo.Sprite = 0;
            }
            else if (meuTime == 2)
            {
                meuCorpo.Sprite = 12;
            }
        }

        #endregion


        #region Reposicao - Goleiro Ação 3

        /*
            Composto basicamento de uma ação defesa
            mais um chute para repor rapidamente a bola em jogo 
        */
        public void Reposicao()
        {
            Defender();
            Chutar();
        }

        #endregion


        #region Passe - Goleiro Ação 4

        public void Passe()
        {
            Defender();
            DominarBola();

            if (meuTime == 1)
            {
                IrPara(modeloAmbiente.ListaCorpos[2].CentroDeMassa);
            }
            else
            {
                IrPara(modeloAmbiente.ListaCorpos[4].CentroDeMassa);
            }

            if (!modeloAgente.movendoPara)
            {
                Chutar();
            }

            /*
            ConduzirBola();

            if (modeloAgente.conduzir)
            {
                if (meuTime == 1)
                {
                    modeloAgente.conduzir = false;

                    int deltaY = meuCorpo.CentroDeMassa.Y - modeloAmbiente.ListaCorpos[2].CentroDeMassa.Y;

                    if (deltaY >= 0)
                    {
                        if (deltaY > 60)
                        {
                            RotacionarCorpo(meuCorpo.Sprite, 21);

                            if (meuCorpo.Sprite == 21)
                            {
                                Chutar();
                            }
                        }
                        else
                        {
                            RotacionarCorpo(meuCorpo.Sprite, 0);

                            if (meuCorpo.Sprite == 0)
                            {
                                Chutar();
                            }
                        }
                    }
                    else if (deltaY < 0)
                    {
                        if (deltaY < 60)
                        {
                            RotacionarCorpo(meuCorpo.Sprite, 3);

                            if (meuCorpo.Sprite == 3)
                            {
                                Chutar();
                            }
                        }
                        else
                        {
                            RotacionarCorpo(meuCorpo.Sprite, 0);

                            if (meuCorpo.Sprite == 0)
                            {
                                Chutar();
                            }
                        }
                    }
                }
                else
                {
                    int deltaY = meuCorpo.CentroDeMassa.Y - modeloAmbiente.ListaCorpos[4].CentroDeMassa.Y;

                    if (deltaY >= 0)
                    {
                        if (deltaY > 60)
                        {
                            RotacionarCorpo(meuCorpo.Sprite, 15);

                            if (meuCorpo.Sprite == 15)
                            {
                                Chutar();
                            }
                        }
                        else
                        {
                            RotacionarCorpo(meuCorpo.Sprite, 12);

                            if (meuCorpo.Sprite == 12)
                            {
                                Chutar();
                            }
                        }
                    }
                    else if (deltaY < 0)
                    {
                        if (deltaY < 60)
                        {
                            RotacionarCorpo(meuCorpo.Sprite, 9);

                            if (meuCorpo.Sprite == 9)
                            {
                                Chutar();
                            }
                        }
                        else
                        {
                            RotacionarCorpo(meuCorpo.Sprite, 12);

                            if (meuCorpo.Sprite == 12)
                            {
                                Chutar();
                            }
                        }
                    }
                }
            }
            else
            {
                Defender();
            }
            */
        }

        #endregion


        #region Chutar - Atacante Ação 1

        private int RandomForcaChute()
        {
            Random random = new Random();

            return random.Next(forcaMinChute, forcaMaxChute);
        }

        public void Chutar()
        {          
            float deltaX = modeloAmbiente.ListaCorpos[0].CentroDeMassa.X - meuCorpo.ListaPontosSuperficie[4].X;
            float deltaY = modeloAmbiente.ListaCorpos[0].CentroDeMassa.Y - meuCorpo.ListaPontosSuperficie[4].Y;

            Ambiente.FisicaAmbiente.Utils.Vetor2 vDistancia = new Ambiente.FisicaAmbiente.Utils.Vetor2(deltaX, deltaY);

            if (vDistancia.Length() > 20)
            {
                return;
            }

            string direcao = DirecaoAgente();

            int forcaChute = RandomForcaChute();

            switch (direcao)
            {
                case "S":
                    // Agente está direcionado para baixo (Sul)
                    // Altera a o deslocamento da bola
                    modeloAmbiente.ListaAcumDeslocamento[0][0, 0] = 0;
                    modeloAmbiente.ListaAcumDeslocamento[0][0, 1] = forcaChute;
                    break;

                case "N":
                    // Agente está direcionado para cima (Norte)
                    // Altera a o deslocamento da bola
                    modeloAmbiente.ListaAcumDeslocamento[0][0, 0] = 0;
                    modeloAmbiente.ListaAcumDeslocamento[0][0, 1] = -forcaChute;
                    break;

                case "L":
                    // Agente está direcionado para direita (Leste)
                    // Altera a o deslocamento da bola
                    modeloAmbiente.ListaAcumDeslocamento[0][0, 0] = forcaChute;
                    modeloAmbiente.ListaAcumDeslocamento[0][0, 1] = 0;
                    break;

                case "O":
                    // Agente está direcionado para esquerda (Oeste) 
                    // Altera a o deslocamento da bola
                    modeloAmbiente.ListaAcumDeslocamento[0][0, 0] = -forcaChute;
                    modeloAmbiente.ListaAcumDeslocamento[0][0, 1] = 0;
                    break;

                case "SE":
                    // Agente direcionado para o (SE)
                    // Altera a o deslocamento da bola
                    modeloAmbiente.ListaAcumDeslocamento[0][0, 0] = forcaChute;
                    modeloAmbiente.ListaAcumDeslocamento[0][0, 1] = forcaChute;
                    break;

                case "SO":
                    // Agente direcionado para o (SO)
                    // Altera a o deslocamento da bola
                    modeloAmbiente.ListaAcumDeslocamento[0][0, 0] = -forcaChute;
                    modeloAmbiente.ListaAcumDeslocamento[0][0, 1] = forcaChute;
                    break;

                case "NE":
                    // Agente direcionado para o (NE)
                    // Altera a o deslocamento da bola
                    modeloAmbiente.ListaAcumDeslocamento[0][0, 0] = forcaChute;
                    modeloAmbiente.ListaAcumDeslocamento[0][0, 1] = -forcaChute;
                    break;

                case "NO":
                    // Agente direcionado para o (NO)
                    // Altera a o deslocamento da bola
                    modeloAmbiente.ListaAcumDeslocamento[0][0, 0] = -forcaChute;
                    modeloAmbiente.ListaAcumDeslocamento[0][0, 1] = -forcaChute;
                    break;
            }
        }

        #endregion


        #region Drible - Atacante Ação 3

        /*
            A ação drible vai necessitar do método desviar
            o qual utiliza a matriz de campo de visão para identificar os
            corpos do ambiente e limites do campo, e desviar dos mesmos
        */
        public void Driblar()
        {
            //AjustaVelocAtacante();
            Desviar();            
            IrPara(modeloAgente.objetivoTemporario);
            ConduzirBola();
        }

        #endregion


        #region Finta - Atacante Ação 2

        /*
            A ação drible vai necessitar do método desviar
            o qual utiliza a matriz de campo de visão para identificar os
            corpos do ambiente e limites do campo, e desviar dos mesmos
        */
        public void Fintar()
        {
            //AjustaVelocAtacante();
            Desviar();
            IrPara(modeloAgente.objetivoTemporario);
            ConduzirBola();
        }

        #endregion


        #region Desarme - Atacante Ação 4

        /*
            Desarme é composto basicamento de um deslocamento 
            até a posição da bola.
            Para melhorar o desarme o método de desviar pode ser adicionado
            para que o mesmo quando o adversário esta de costas o desarme
            possa ocorrer, desviando de uma colisão direta com o adversário através
            do contorno do mesmo e movendo-se até a face frontal onde há o domínio da bola
        */

        public void Desarmar()
        {
            //AjustaVelocAtacante();
            IrPara(modeloAmbiente.ListaCorpos[0].CentroDeMassa);
        }

        #endregion


        #region Dominar a bola - Atacante Ação 5 

        /*
            Se nimguém estiver com a posse de bola 
            vai até a bola para dominá-la 
        */
        public void DominarBola()
        {
            //AjustaVelocAtacante();
            //Desviar();
            IrPara(modeloAgente.objetivoQLearning);
            ConduzirBola();
        }

        #endregion
    }
}