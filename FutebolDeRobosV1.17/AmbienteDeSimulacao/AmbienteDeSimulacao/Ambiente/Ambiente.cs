using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace AmbienteDeSimulacao.Ambiente
{
    class Ambiente
    {
        // Corpos
        public Bola bola;
        public List<Agente.Agente> listaAgentes;

        // Modelo do ambiente
        public ModeloAmbiente modeloAmbiente;

        // Q-Learning
        public Aprendizado.QLearning.QLearning qLearningGoleiro;
        public Aprendizado.QLearning.QLearning qLearningAtacante;

        // Sinalizadores de gol
        public bool flagGol = false;
        public bool flagGolTimeAzul = false;
        public bool flagGolTimeVermelho = false;

        // Contadores de gols
        public int golsTimeAzul = 0;
        public int golsTimeVermelho = 0;

        // Física
        private Fisica fisica;
        private List<IFilhosAmbiente> listaFilhosAmbiente;

        // Ação escolhida (Q-learning)
        private int acaoEscolhidaGoleiro = -1;
        private int acaoEscolhidaAtacante = -1;

        // Calculo do objetivo do goleiro 
        private float deltaX;
        private float deltaY;
        private float aux;

        // Objetivos
        private Point objetivoGoleiro;
        private Point objetivoAtacante;

        // Log
        public bool log;


        #region Construtor

        public Ambiente(int tempo, int[,] dimencoesBola, int[,] dimencoesAgente, Rectangle limitesDoAmbiente, List<Point> listaLocalizacaoCorpos, int quantidadeCorpos)
        {
            modeloAmbiente = new ModeloAmbiente();

            /********************************************************************* Cria os agentes e a bola ***************************************************************************************/
            bola = new Bola(0, (float)FisicaAmbiente.Corpos.Massa.massaBola, dimencoesBola, listaLocalizacaoCorpos.ElementAt(0));

            listaAgentes = new List<Agente.Agente>();

            for (int i = 1; i < quantidadeCorpos; i++)
            {
                if (i < 3)
                {
                    /* Agentes do Time 1 */
                    listaAgentes.Add(new Agente.Agente(i, (float)FisicaAmbiente.Corpos.Massa.massaAgente, dimencoesAgente, listaLocalizacaoCorpos[i], limitesDoAmbiente, true, 1, modeloAmbiente));
                }
                else
                {
                    /* Agentes do Time 2 */
                    listaAgentes.Add(new Agente.Agente(i, (float)FisicaAmbiente.Corpos.Massa.massaAgente, dimencoesAgente, listaLocalizacaoCorpos[i], limitesDoAmbiente, true, 2, modeloAmbiente));
                }
            }
            /**************************************************************************************************************************************************************************************/

            /************** Modelo do Ambiente **************************/
            InicilizaModelo(quantidadeCorpos);

            modeloAmbiente.ListaCorpos.Add(bola.MeuCorpo);
            foreach (Agente.Agente a in listaAgentes)
            {
                modeloAmbiente.ListaCorpos.Add(a.MeuCorpo);
            }
            /************************************************************/

            /************************ Fisica ****************************/
            fisica = new Fisica(tempo, limitesDoAmbiente, modeloAmbiente);
            /************************************************************/

            /************** Padrão Observadores *************************/
            listaFilhosAmbiente = new List<IFilhosAmbiente>();
            listaFilhosAmbiente.Add(fisica);
            listaFilhosAmbiente.Add(Form1.Instance);
            /************************************************************/

            /*********************** Q-Learning *************************/
            qLearningGoleiro = new Aprendizado.QLearning.QLearning(1);
            qLearningAtacante = new Aprendizado.QLearning.QLearning(2);
            /************************************************************/
        }

        #endregion


        #region Inicializa observadores

        public void InicializaObservadores()
        {
            foreach (IFilhosAmbiente f in listaFilhosAmbiente)
            {
                f.Atualiza();
            }
        }

        #endregion


        #region Inicializa as Listas do Modelo

        private void InicilizaModelo(int quantidadeCorpos)
        {
            modeloAmbiente.idPosseBola = 0;
            modeloAmbiente.idDefesa = 0;

            modeloAmbiente.cmAtualizado = false;
            modeloAmbiente.lcAtualizado = false;
            modeloAmbiente.scAtualizado = false;

            modeloAmbiente.ListaAmortecimento = new List<int>();
            modeloAmbiente.ListaAcumDeslocamento = new List<float[,]>();
            modeloAmbiente.ListaAceleracaoAtual = new List<float[,]>();
            modeloAmbiente.ListaAceleracaoAnterior = new List<float[,]>();
            modeloAmbiente.ListaCorpos = new List<FisicaAmbiente.Corpos.Corpo>();
            modeloAmbiente.ListaCorposColisao = new List<int>();
            modeloAmbiente.ListaDeslocamentoAtual = new List<float[,]>();
            modeloAmbiente.ListaDeslocamentoAnterior = new List<float[,]>();
            modeloAmbiente.ListaForcaAnterior = new List<float[,]>();
            modeloAmbiente.ListaForcaAtual = new List<float[,]>();
            modeloAmbiente.ListaLimitesDoAmbiente = new List<Point>();
            modeloAmbiente.ListaLimitesMeioCampoEsq = new List<Point>();
            modeloAmbiente.ListaLimitesMeioCampoDir = new List<Point>();
            modeloAmbiente.ListaLimitesAreaEsq = new List<Point>();
            modeloAmbiente.ListaLimitesAreaDir = new List<Point>();
            modeloAmbiente.ListaLimitesGoleiraEsq = new List<Point>();
            modeloAmbiente.ListaLimitesGoleiraDir = new List<Point>();
            modeloAmbiente.ListaPontosColisaoCorpos = new List<Point>();
            modeloAmbiente.ListaPontosColisaoAmbiente = new List<Point>();
            modeloAmbiente.ListaPontosSuperficieCircular = new List<Point>();
            modeloAmbiente.ListaPontosSuperficieRetangular = new List<Point>();

            // Inicializa as listas
            for (int i = 0; i < quantidadeCorpos; i++)
            {
                modeloAmbiente.ListaAmortecimento.Add(0);
                modeloAmbiente.ListaAcumDeslocamento.Add(new float[1, 2] { { 0, 0 } });
                modeloAmbiente.ListaAceleracaoAtual.Add(new float[1, 2] { { 0, 0 } });
                modeloAmbiente.ListaAceleracaoAnterior.Add(new float[1, 2] { { 0, 0 } });
                modeloAmbiente.ListaDeslocamentoAtual.Add(new float[1, 2] { { 0, 0 } });
                modeloAmbiente.ListaDeslocamentoAnterior.Add(new float[1, 2] { { 0, 0 } });
                modeloAmbiente.ListaForcaAnterior.Add(new float[1, 2] { { 0, 0 } });
                modeloAmbiente.ListaForcaAtual.Add(new float[1, 2] { { 0, 0 } });
            }
        }

        #endregion


        #region Verifica a posse da bola

        private void VerificaPosseBola()
        {
            // Verifica se o agente que está indicado como tendo controle da bola
            // realmente está a uma distância que o deixa em controle da bola

            if (modeloAmbiente.idPosseBola == 0)
            {
                return;
            }

            // Atualiza o id de quem está com a posse de bola apenas quando 
            // a posse da bola mudar de atacante. 
            qLearningGoleiro.idPosseBola = modeloAmbiente.idPosseBola;
            qLearningAtacante.idPosseBola = modeloAmbiente.idPosseBola;

            // Atualiza o id de quem defendeu bola apenas quando 
            qLearningGoleiro.idDefesa = modeloAmbiente.idDefesa;
            qLearningAtacante.idDefesa = modeloAmbiente.idDefesa;

            FisicaAmbiente.Utils.Vetor2 vDistancia = new FisicaAmbiente.Utils.Vetor2(listaAgentes[modeloAmbiente.idPosseBola - 1].MeuCorpo.ListaPontosSuperficie[4].X - bola.MeuCorpo.CentroDeMassa.X,
                                                                                     listaAgentes[modeloAmbiente.idPosseBola - 1].MeuCorpo.ListaPontosSuperficie[4].Y - bola.MeuCorpo.CentroDeMassa.Y);

            float distancia = vDistancia.Length();

            if (distancia > 30)
            {
                modeloAmbiente.idPosseBola = 0;
            }
        }

        #endregion


        #region Verifica defesa

        private void VerificaDefesa()
        {
            if (modeloAmbiente.idDefesa == 0)
            {
                return;
            }

            // Atualiza o id de quem defendeu bola apenas quando 
            qLearningGoleiro.idDefesa = modeloAmbiente.idDefesa;
            qLearningAtacante.idDefesa = modeloAmbiente.idDefesa;

            FisicaAmbiente.Utils.Vetor2 vDistancia = new FisicaAmbiente.Utils.Vetor2(listaAgentes[modeloAmbiente.idDefesa - 1].MeuCorpo.CentroDeMassa.X - bola.MeuCorpo.CentroDeMassa.X,
                                                                                     listaAgentes[modeloAmbiente.idDefesa - 1].MeuCorpo.CentroDeMassa.Y - bola.MeuCorpo.CentroDeMassa.Y);

            float distancia = vDistancia.Length();

            if (distancia > 35)
            {
                modeloAmbiente.idDefesa = 0;
            }
        }

        #endregion


        #region Verifica se houve gol

        private void VerificarGol()
        {
            if (!flagGol)
            {
                if (bola.MeuCorpo.ListaPontosSuperficie[2].X < modeloAmbiente.ListaLimitesGoleiraEsq[1].X
                && bola.MeuCorpo.CentroDeMassa.Y >= modeloAmbiente.ListaLimitesGoleiraDir[1].Y
                && bola.MeuCorpo.CentroDeMassa.Y <= modeloAmbiente.ListaLimitesGoleiraDir[2].Y)
                {
                    // Gol na goleira esquerda
                    flagGol = true;
                    flagGolTimeVermelho = true;

                    // Incrementa placar
                    golsTimeVermelho++;

                    qLearningAtacante.AjustaEpsilon();
                    //qLearningAtacante.flagGol = true;
                    qLearningAtacante.flagGolTimeVermelho = true;

                    qLearningGoleiro.AjustaEpsilon();
                    //qLearningGoleiro.flagGol = true;
                    qLearningGoleiro.flagGolTimeVermelho = true;
                }
                else if (bola.MeuCorpo.ListaPontosSuperficie[6].X > modeloAmbiente.ListaLimitesGoleiraDir[0].X
                     && bola.MeuCorpo.CentroDeMassa.Y >= modeloAmbiente.ListaLimitesGoleiraDir[0].Y
                     && bola.MeuCorpo.CentroDeMassa.Y <= modeloAmbiente.ListaLimitesGoleiraDir[3].Y)
                {
                    // Gol na goleira direita
                    flagGol = true;
                    flagGolTimeAzul = true;

                    // Incrementa placar
                    golsTimeAzul++;

                    qLearningAtacante.AjustaEpsilon();
                    //qLearningAtacante.flagGol = true;
                    qLearningAtacante.flagGolTimeAzul = true;

                    qLearningGoleiro.AjustaEpsilon();
                    //qLearningGoleiro.flagGol = true;
                    qLearningGoleiro.flagGolTimeAzul = true;
                }
            }
        }

        #endregion


        #region Verifica se a bola não saiu de campo (Problema de tunelamento)

        /*
            Ajusta a localização da bola para a mesma nao sair de campo
            A origem das coordenadas de orientação do canvas encontra-se
            localizado no canto superior esquerdo:
            x__
            |__|
        */
        private Point LimitesAmbiente(int idx, Point localizacao)
        {
            if (localizacao.Y < modeloAmbiente.ListaLimitesDoAmbiente[2].Y
            || localizacao.Y > modeloAmbiente.ListaLimitesDoAmbiente[5].Y)
            {
                // Localização menor ou igual que o fundo de campo esquerdo
                if (localizacao.X <= modeloAmbiente.ListaLimitesDoAmbiente[0].X)
                {
                    localizacao.X = modeloAmbiente.ListaLimitesDoAmbiente[0].X;
                }
                // Localização + dimensão em X maior ou igual que o fundo de campo direito
                else if ((localizacao.X + modeloAmbiente.ListaCorpos[idx].Dimencao[0, 0]) >= modeloAmbiente.ListaLimitesDoAmbiente[1].X)
                {
                    localizacao.X = modeloAmbiente.ListaLimitesDoAmbiente[1].X - modeloAmbiente.ListaCorpos[idx].Dimencao[0, 0];
                }
            }
            // Localização + dimensão em Y maior que a lateral inferior
            if ((localizacao.Y + modeloAmbiente.ListaCorpos[idx].Dimencao[0, 1]) >= modeloAmbiente.ListaLimitesDoAmbiente[6].Y)
            {
                localizacao.Y = modeloAmbiente.ListaLimitesDoAmbiente[6].Y - modeloAmbiente.ListaCorpos[idx].Dimencao[0, 1];
            }
            else if (localizacao.Y <= modeloAmbiente.ListaLimitesDoAmbiente[0].Y)
            {
                localizacao.Y = modeloAmbiente.ListaLimitesDoAmbiente[0].Y;
            }

            return localizacao;
        }

        #endregion


        #region Sobreposição entre corpos

        /*
            Ajusta a localização dos corpos em colisão para os mesmos não se sobreporem
            baseado no corpo com a maior velocidade é feito um calculo da distância a ser respeitada
            entre os centros de massa e a distância atual entre os centros de massa.
            Baseado neste resultado é realizado um ajuste da localização do agente, através
            da proporção entre o quando diferença entre centros de massa
            devem ser corrigida em cada componente X e Y,
            valor percentual da componente em relação ao deslocamento do agente (módulo do vetor) 
            multiplicado pela diferença entre os centros de massa.
        */
        private void SobreposicaoCorpos(int idx1, int idx2)
        {
            int deltaX = Math.Abs(modeloAmbiente.ListaCorpos[idx1].CentroDeMassa.X - modeloAmbiente.ListaCorpos[idx2].CentroDeMassa.X);
            int deltaY = Math.Abs(modeloAmbiente.ListaCorpos[idx1].CentroDeMassa.Y - modeloAmbiente.ListaCorpos[idx2].CentroDeMassa.Y);

            FisicaAmbiente.Utils.Vetor2 v = new FisicaAmbiente.Utils.Vetor2(deltaX, deltaY);

            float d = v.Length();

            // Simplificação aplicada fazendo tratativa como se os corpos fossem círculos 
            // modeloAmbiente.ListaCorpos[idx1].Dimencao[0, 0] + modeloAmbiente.ListaCorpos[idx2].Dimencao[0, 0] / 2
            int distCorpos = Convert.ToInt32(modeloAmbiente.ListaCorpos[idx1].Dimencao[0, 0] + modeloAmbiente.ListaCorpos[idx2].Dimencao[0, 0]) / 2;
            if (d < distCorpos)
            {
                int X = modeloAmbiente.ListaCorpos[idx1].Localizacao.X;
                int Y = modeloAmbiente.ListaCorpos[idx1].Localizacao.Y;
                float dif = distCorpos - d;
                float dif1 = dif / 2;
                float def2 = dif1;

                FisicaAmbiente.Utils.Vetor2 vDesloc1 = new FisicaAmbiente.Utils.Vetor2(modeloAmbiente.ListaDeslocamentoAtual[idx1][0, 0], modeloAmbiente.ListaDeslocamentoAtual[idx1][0, 1]);
                FisicaAmbiente.Utils.Vetor2 vDesloc2 = new FisicaAmbiente.Utils.Vetor2(modeloAmbiente.ListaDeslocamentoAtual[idx2][0, 0], modeloAmbiente.ListaDeslocamentoAtual[idx2][0, 1]);

                float vd1 = vDesloc1.Length();
                float vd2 = vDesloc2.Length();

                if (vd1 == 0)
                {
                    vDesloc1.X = 1;
                    vDesloc1.Y = 1;
                    vd1 = 1;
                }

                if (vd2 == 0)
                {
                    vDesloc2.X = 1;
                    vDesloc2.Y = 1;
                    vd2 = 1;
                }

                if (vd1 > vd2)
                {
                    // Trata corpo 1
                    if (modeloAmbiente.ListaDeslocamentoAtual[idx1][0, 0] > 0)
                    {
                        X = modeloAmbiente.ListaCorpos[idx1].Localizacao.X - Convert.ToInt32(vDesloc1.X * dif / vd1);
                    }
                    else if (modeloAmbiente.ListaDeslocamentoAtual[idx1][0, 0] < 0)
                    {
                        X = modeloAmbiente.ListaCorpos[idx1].Localizacao.X + Convert.ToInt32(vDesloc1.X * dif / vd1);
                    }

                    if (modeloAmbiente.ListaDeslocamentoAtual[idx1][0, 1] > 0)
                    {
                        Y = modeloAmbiente.ListaCorpos[idx1].Localizacao.Y - Convert.ToInt32(vDesloc1.Y * dif / vd1);
                    }
                    else if (modeloAmbiente.ListaDeslocamentoAtual[idx1][0, 1] < 0)
                    {
                        Y = modeloAmbiente.ListaCorpos[idx1].Localizacao.Y + Convert.ToInt32(vDesloc1.Y * dif / vd1);
                    }

                    modeloAmbiente.ListaCorpos[idx1].Localizacao = new Point(X, Y);
                }
                else
                {
                    // Trata corpo 2
                    if (modeloAmbiente.ListaDeslocamentoAtual[idx2][0, 0] > 0)
                    {
                        X = modeloAmbiente.ListaCorpos[idx2].Localizacao.X - Convert.ToInt32(vDesloc2.X * dif / vd2);
                    }
                    else if (modeloAmbiente.ListaDeslocamentoAtual[idx2][0, 0] < 0)
                    {
                        X = modeloAmbiente.ListaCorpos[idx2].Localizacao.X + Convert.ToInt32(vDesloc2.X * dif / vd2);
                    }

                    if (modeloAmbiente.ListaDeslocamentoAtual[idx2][0, 1] > 0)
                    {
                        Y = modeloAmbiente.ListaCorpos[idx2].Localizacao.Y - Convert.ToInt32(vDesloc2.Y * dif / vd2);
                    }
                    else if (modeloAmbiente.ListaDeslocamentoAtual[idx2][0, 1] < 0)
                    {
                        Y = modeloAmbiente.ListaCorpos[idx2].Localizacao.Y + Convert.ToInt32(vDesloc2.Y * dif / vd2);
                    }

                    modeloAmbiente.ListaCorpos[idx2].Localizacao = new Point(X, Y);
                }
            }
        }

        #endregion


        #region Iteração

        public void IteracaoAmbiente()
        {
            // Método de iteração com os agentes
            IteracaoAgentes();

            // Verifica se houve gol
            VerificarGol();

            // Verifica a posse da bola
            VerificaPosseBola();

            // Verifica defesa (intesidade da emoção goleiro)
            VerificaDefesa();

            /*for (int i = 0; i < modeloAmbiente.ListaCorpos.Count; i++)
            {
                for (int j = i + 1; j < modeloAmbiente.ListaCorpos.Count; j++)
                {
                    SobreposicaoCorpos(i, j);
                }
            }*/

            foreach (FisicaAmbiente.Corpos.Corpo c in modeloAmbiente.ListaCorpos)
            {
                modeloAmbiente.ListaCorpos[c.GetId].Localizacao = LimitesAmbiente(c.GetId, c.Localizacao);
            }

            // Atualiza a física
            foreach (IFilhosAmbiente f in listaFilhosAmbiente)
            {
                f.Atualiza();
            }
        }

        public void IteracaoAgentes()
        {
            foreach (Agente.Agente a in listaAgentes)
            {
                if (a.GetMeuTime == 1)
                {
                    if (a.MeuCorpo.GetId == 1)
                    {
                        // Atualiza informações do agente
                        a.MeuCorpo = modeloAmbiente.ListaCorpos[a.MeuCorpo.GetId];

                        // Verifica estado
                        EstadosGoleiroTimeAzul(a);

                        // Escolhe uma ação
                        acaoEscolhidaGoleiro = qLearningGoleiro.GetAcaoEscolhida;

                        /*********************************************** Determina objetivo ************************************************************/
                        deltaX = modeloAmbiente.ListaDeslocamentoAtual[0][0, 0];
                        deltaY = modeloAmbiente.ListaDeslocamentoAtual[0][0, 1];

                        // Verifica se a bola esta parada fora da área ou está indo em direção ao outro gol
                        if ((deltaX == 0 
                        && deltaY == 0 
                        && bola.MeuCorpo.CentroDeMassa.X > modeloAmbiente.ListaLimitesAreaEsq[1].X 
                        && (bola.MeuCorpo.CentroDeMassa.Y < modeloAmbiente.ListaLimitesAreaEsq[1].Y 
                        || bola.MeuCorpo.CentroDeMassa.Y > modeloAmbiente.ListaLimitesAreaEsq[2].Y))
                        || deltaX > 0)
                        {
                            objetivoGoleiro = new Point(60, 220);
                        }
                        /*else if (deltaX < 0 && deltaY == 0
                             || deltaX == 0 && deltaY != 0 && bola.MeuCorpo.CentroDeMassa.X < modeloAmbiente.ListaLimitesAreaEsq[1].X)
                        {
                            objetivoGoleiro = bola.MeuCorpo.CentroDeMassa;
                        }*/
                        else
                        {
                            aux = bola.MeuCorpo.CentroDeMassa.Y
                                + (Math.Abs(a.MeuCorpo.CentroDeMassa.X - bola.MeuCorpo.CentroDeMassa.X) / modeloAmbiente.ListaDeslocamentoAtual[0][0, 0]);

                            // Verifica se a bola está na trajetória do gol
                            if (aux < modeloAmbiente.ListaLimitesAreaEsq[0].Y
                            && aux > modeloAmbiente.ListaLimitesDoAmbiente[3].Y)
                            {
                                objetivoGoleiro = bola.MeuCorpo.CentroDeMassa;
                            }
                            else
                            {
                                objetivoGoleiro = new Point(60, 220);
                            }
                        }
                        /*******************************************************************************************************************************/

                        // Executa a ação escolhida 
                        switch (acaoEscolhidaGoleiro)
                        {
                            case 0:
                                a.Agir(objetivoGoleiro, "Defender");
                                break;

                            case 1:
                                a.Agir(objetivoGoleiro, "Posicionar");
                                break;

                            case 2:
                                a.Agir(objetivoGoleiro, "Reposicao");
                                break;
                        }

                        // Retorno do deslocamento do agente
                        modeloAmbiente.ListaDeslocamentoAtual[a.MeuCorpo.GetId] = a.GetMeuDeslocamento;

                        // Atualiza tabela Q
                        //qLearningGoleiro.AtualizaTabelaQ(/*acaoEscolhidaGoleiro*/);

                        if (log)
                        {
                            qLearningGoleiro.Log();
                        }

                        if (qLearningGoleiro.flagGolTimeAzul || qLearningGoleiro.flagGolTimeVermelho)
                        {
                            qLearningGoleiro.flagGolTimeAzul = false;
                            qLearningGoleiro.flagGolTimeVermelho = false;
                        }
                    }
                    else if (a.MeuCorpo.GetId == 2)
                    {
                        // Atualiza informações do agente
                        a.MeuCorpo = modeloAmbiente.ListaCorpos[a.MeuCorpo.GetId];

                        // Verifica estado
                        EstadosAtacanteTimeAzul(a);

                        // Escolhe uma ação
                        acaoEscolhidaAtacante = qLearningAtacante.GetAcaoEscolhida;

                        /****************** Determina objetivo ****************/
                        if (a.MeuCorpo.GetId == modeloAmbiente.idPosseBola)
                        {
                            objetivoAtacante = new Point(530, 220);
                        }
                        else
                        {
                            objetivoAtacante = bola.MeuCorpo.CentroDeMassa;
                        }
                        /******************************************************/

                        // Executa a ação escolhida 
                        switch (acaoEscolhidaAtacante)
                        {
                            case 0:
                                a.Agir(objetivoAtacante, "Chutar");
                                break;

                            case 1:
                                a.Agir(objetivoAtacante, "Driblar");
                                break;

                            case 2:
                                a.Agir(objetivoAtacante, "Dominar");
                                break;

                            case 3:
                                a.Agir(objetivoAtacante, "Desarmar");
                                break;

                            case 4:
                                a.Agir(objetivoAtacante, "Fintar");
                                break;
                        }

                        // Retorno do deslocamento do agente
                        modeloAmbiente.ListaDeslocamentoAtual[a.MeuCorpo.GetId] = a.GetMeuDeslocamento;

                        // Atualiza tabela Q
                        //qLearningAtacante.AtualizaTabelaQ(/*acaoEscolhidaAtacante*/);

                        if (log)
                        {
                            qLearningAtacante.Log();
                        }

                        if (qLearningAtacante.flagGolTimeAzul || qLearningAtacante.flagGolTimeVermelho)
                        {
                            qLearningAtacante.flagGolTimeAzul = false;
                            qLearningAtacante.flagGolTimeVermelho = false;
                        }
                    }
                }
                else if (a.GetMeuTime == 2)
                {
                    if (a.MeuCorpo.GetId == 3)
                    {
                        // Atualiza informações do agente
                        a.MeuCorpo = modeloAmbiente.ListaCorpos[a.MeuCorpo.GetId];

                        // Verifica estado e executa ação 
                        EstadosGoleiroTimeVermelho(a);

                        // Retorno do deslocamento do agente
                        modeloAmbiente.ListaDeslocamentoAtual[a.MeuCorpo.GetId] = a.GetMeuDeslocamento;
                    }
                    else if (a.MeuCorpo.GetId == 4)
                    {
                        // Atualiza informações do agente
                        a.MeuCorpo = modeloAmbiente.ListaCorpos[a.MeuCorpo.GetId];

                        // Verifica estado e executa ação
                        EstadosAtacanteTimeVermelho(a);

                        // Retorno do deslocamento do agente
                        modeloAmbiente.ListaDeslocamentoAtual[a.MeuCorpo.GetId] = a.GetMeuDeslocamento;
                    }
                }

                // Atualiza os parâmetros da bola a cada iteração do foreach                
                bola.MeuCorpo = modeloAmbiente.ListaCorpos[bola.MeuCorpo.GetId];
            }
        }

        #endregion


        #region Estados

        private void EstadosGoleiroTimeAzul(Agente.Agente a)
        {
            if (modeloAmbiente.ListaCorpos[3].CentroDeMassa.X > modeloAmbiente.ListaLimitesMeioCampoEsq[1].X
            && modeloAmbiente.ListaDeslocamentoAtual[0][0, 0] >= 0)
            {
                // Estado 1
                qLearningGoleiro.SetEstadoAtual = 0;
            }
            else
            {
                // Bola dentro da área
                FisicaAmbiente.Utils.Vetor2 vDistanciaAdv = new FisicaAmbiente.Utils.Vetor2(modeloAmbiente.ListaCorpos[4].CentroDeMassa.X - modeloAmbiente.ListaCorpos[1].CentroDeMassa.X,
                                                                                            modeloAmbiente.ListaCorpos[4].CentroDeMassa.Y - modeloAmbiente.ListaCorpos[1].CentroDeMassa.Y);

                float distancia = vDistanciaAdv.Length();

                // Testa se o adversário está próximo
                if (distancia > 225)
                {
                    // Estado 3
                    qLearningGoleiro.SetEstadoAtual = 2;
                }
                else
                {
                    // Estado 2
                    qLearningGoleiro.SetEstadoAtual = 1;
                }
            }
        }


        private void EstadosAtacanteTimeAzul(Agente.Agente a)
        {
            // Se nimguém estiver com a posse de bola, ir até a bola
            // para dominá-la
            if (modeloAmbiente.idPosseBola == 0)
            {
                // Estado 5
                qLearningAtacante.SetEstadoAtual = 4;

                return;
            }

            // Se a posse de bola está com o atacante em questão
            if (modeloAmbiente.idPosseBola == listaAgentes[1].MeuCorpo.GetId)
            {
                // Caso o atacante esteja além do limite do chutar, retorna para uma posição mais apropriada
                if (listaAgentes[1].MeuCorpo.CentroDeMassa.X > modeloAmbiente.ListaLimitesAreaDir[0].X)
                {
                    // Estado 5
                    qLearningAtacante.SetEstadoAtual = 4;
                    return;
                }
                else if (listaAgentes[1].MeuCorpo.CentroDeMassa.X < modeloAmbiente.ListaLimitesAreaEsq[1].X)
                {
                    // Estado 5
                    qLearningAtacante.SetEstadoAtual = 4;
                    return;
                }

                // Calcula a direção do agente
                float deltaX = a.MeuCorpo.ListaPontosSuperficie[4].X - a.MeuCorpo.CentroDeMassa.X;
                float deltaY = a.MeuCorpo.ListaPontosSuperficie[4].Y - a.MeuCorpo.CentroDeMassa.Y;
                // Calcula a direção do objetivo (gol)
                float deltaYObjetivo = 250 - a.MeuCorpo.ListaPontosSuperficie[4].Y;
                // Calcula a direção da bola (se está na mesma direção que o agente)
                float deltaYBola = a.MeuCorpo.ListaPontosSuperficie[4].Y - bola.MeuCorpo.CentroDeMassa.Y;

                // Se o adversário não estiver na frente do agente
                if (listaAgentes[1].MeuCorpo.CentroDeMassa.X > listaAgentes[3].MeuCorpo.CentroDeMassa.X
                || Math.Abs(listaAgentes[1].MeuCorpo.CentroDeMassa.Y - listaAgentes[3].MeuCorpo.CentroDeMassa.Y) > 30) //distancia < 200)
                {
                    // Testa se o agente está na direção correta para realizar um chute
                    if (deltaY >= 0 && deltaYObjetivo >= 0
                    || deltaY <= 0 && deltaYObjetivo <= 0)
                    {
                        if (deltaX > 0 && Math.Abs(deltaYBola) < 20)
                        {
                            // Levando em consideração que o atacante faz gol para o lado direito
                            // Se o adversário está atrás ou se o adversário está fora da trajetória do atacante

                            // Estado 2
                            qLearningAtacante.SetEstadoAtual = 1;

                            return;
                        }
                    }
                }

                float deltaXAdv = listaAgentes[3].MeuCorpo.CentroDeMassa.X - listaAgentes[1].MeuCorpo.CentroDeMassa.X;
                float deltaYAdv = listaAgentes[3].MeuCorpo.CentroDeMassa.Y - listaAgentes[1].MeuCorpo.CentroDeMassa.Y;

                if (deltaXAdv > 0 && deltaXAdv < 80 && Math.Abs(deltaYAdv) < 35) // maior que zero e menor que dois robos
                {
                    // Estado 3
                    qLearningAtacante.SetEstadoAtual = 2;
                }
                else if ((deltaYAdv > 80 && Math.Abs(deltaXAdv) < 35 && deltaY < 0)
                     || (deltaYAdv > 0 && deltaYAdv < 80 && Math.Abs(deltaXAdv) < 35 && deltaY > 0))
                { 
                        // Estado 3
                        qLearningAtacante.SetEstadoAtual = 2;
                }
                else
                {
                    // Estado 4
                    qLearningAtacante.SetEstadoAtual = 3;
                }
            }
            else if (modeloAmbiente.idPosseBola == listaAgentes[3].MeuCorpo.GetId)
            {
                // Se a posse de bola estiver com o adversário
                // vai até a bola para tentar tomar a posse de bola

                // Estado 1
                qLearningAtacante.SetEstadoAtual = 0;
            }
        }


        private void EstadosGoleiroTimeVermelho(Agente.Agente a)
        {
            if (modeloAmbiente.ListaCorpos[1].CentroDeMassa.X < modeloAmbiente.ListaLimitesMeioCampoDir[0].X
            && modeloAmbiente.ListaDeslocamentoAtual[0][0, 0] <= 0)
            {
                // Se o valor da coordenada X da bola for maior que o valor de X do(s) ponto(s) 
                // com o(s) maior(es) valores em X (ponto 1 e 2) então a bola está fora da área 
                // não necessitando testar os limites de Y

                // Estado 1
                a.Agir(new Point(540, 225), "Posicionar");
            }
            else
            {
                // Bola dentro da área
                FisicaAmbiente.Utils.Vetor2 vDistanciaAdv = new FisicaAmbiente.Utils.Vetor2(modeloAmbiente.ListaCorpos[2].CentroDeMassa.X - modeloAmbiente.ListaCorpos[3].CentroDeMassa.X,
                                                                                            modeloAmbiente.ListaCorpos[2].CentroDeMassa.Y - modeloAmbiente.ListaCorpos[3].CentroDeMassa.Y);

                float distancia = vDistanciaAdv.Length();

                // Testa se o adversário está próximo
                if (distancia > 225)
                {
                    // Estado 3
                    a.Agir(bola.MeuCorpo.CentroDeMassa, "Reposicao");
                }
                else
                {
                    // Estado 2
                    a.Agir(bola.MeuCorpo.CentroDeMassa, "Defender");
                }
            }
        }


        private void EstadosAtacanteTimeVermelho(Agente.Agente a)
        {
            // Se nimguém estiver com a posse de bola, ir até a bola
            // para dominá-la
            if (modeloAmbiente.idPosseBola == 0)
            {
                // Estado 5
                a.Agir(bola.MeuCorpo.CentroDeMassa, "Fintar");

                return;
            }

            // Se a posse de bola está com o atacante em questão
            if (modeloAmbiente.idPosseBola == listaAgentes[3].MeuCorpo.GetId)
            {

                if (listaAgentes[3].MeuCorpo.CentroDeMassa.X < modeloAmbiente.ListaLimitesAreaEsq[1].X)
                {
                    a.Agir(new Point(modeloAmbiente.ListaLimitesMeioCampoEsq[1].X, listaAgentes[3].MeuCorpo.CentroDeMassa.Y), "Fintar");
                    return;
                }
                else if (listaAgentes[3].MeuCorpo.CentroDeMassa.X > modeloAmbiente.ListaLimitesAreaDir[0].X)
                {
                    a.Agir(new Point(modeloAmbiente.ListaLimitesMeioCampoDir[0].X, listaAgentes[3].MeuCorpo.CentroDeMassa.Y), "Fintar");
                    return;
                }

                // Calcula a direção do agente
                float deltaX = a.MeuCorpo.ListaPontosSuperficie[4].X - a.MeuCorpo.CentroDeMassa.X;
                float deltaY = a.MeuCorpo.ListaPontosSuperficie[4].Y - a.MeuCorpo.CentroDeMassa.Y;
                // Calcula a direção do objetivo (gol)
                float deltaYObjetivo = 250 - a.MeuCorpo.ListaPontosSuperficie[4].Y;
                // Calcula a direção da bola (se está na mesma direção que o agente)
                float deltaYBola = a.MeuCorpo.ListaPontosSuperficie[4].Y - bola.MeuCorpo.CentroDeMassa.Y;

                // Se o adversário não estiver na frente do agente
                if (listaAgentes[3].MeuCorpo.CentroDeMassa.X < listaAgentes[1].MeuCorpo.CentroDeMassa.X
                || Math.Abs(listaAgentes[3].MeuCorpo.CentroDeMassa.Y - listaAgentes[1].MeuCorpo.CentroDeMassa.Y) > 30) //distancia < 200)
                {
                    // Testa se o agente está na direção correta para realizar um chute
                    if (deltaY >= 0 && deltaYObjetivo >= 0
                    || deltaY <= 0 && deltaYObjetivo <= 0)
                    {
                        if (deltaX < 0 && Math.Abs(deltaYBola) < 20)
                        {
                            // Levando em consideração que o atacante faz gol para o lado direito
                            // Se o adversário está atrás ou se o adversário está fora da trajetória do atacante

                            // Estado 2
                            a.Agir(new Point(40, 250), "Chutar");
                            return;
                        }
                    }
                }

                float deltaXAdv = listaAgentes[3].MeuCorpo.CentroDeMassa.X - listaAgentes[1].MeuCorpo.CentroDeMassa.X;
                float deltaYAdv = listaAgentes[3].MeuCorpo.CentroDeMassa.Y - listaAgentes[1].MeuCorpo.CentroDeMassa.Y;

                if (deltaXAdv > 0 && deltaXAdv < 80 && Math.Abs(deltaYAdv) < 35) // maior que zero e menor que dois robos
                {
                    // Estado 3
                    a.Agir(new Point(40, 250), "Driblar");
                }
                else if ((deltaYAdv > 80 && Math.Abs(deltaXAdv) < 35 && deltaY < 0)
                     || (deltaYAdv > 0 && deltaYAdv < 80 && Math.Abs(deltaXAdv) < 35 && deltaY > 0))
                {
                    // Estado 3
                    a.Agir(new Point(40, 250), "Driblar");
                }
                else
                {
                    // Estado 4
                    a.Agir(new Point(40, 250), "Dominar");
                }
            }
            else if (modeloAmbiente.idPosseBola == listaAgentes[1].MeuCorpo.GetId)
            {
                // Se a posse de bola estiver com o adversário
                // vai até a bola para tentar tomar a posse de bola

                // Estado 1
                a.Agir(bola.MeuCorpo.CentroDeMassa, "Desarmar");
            }
        }

        #endregion


        #region Salva os da Tabela Q

        public void SalvarDadosQLearning(bool goleiro, bool atacante)
        {
            if (goleiro)
            {
                qLearningGoleiro.SalvarTabelaQ();
            }
            if (atacante)
            {
                qLearningAtacante.SalvarTabelaQ();
            }
        }

        #endregion


        #region Log

        public void Log()
        {
            qLearningAtacante.SalvaLog();
            qLearningGoleiro.SalvaLog();
        }

        #endregion 
    }
}