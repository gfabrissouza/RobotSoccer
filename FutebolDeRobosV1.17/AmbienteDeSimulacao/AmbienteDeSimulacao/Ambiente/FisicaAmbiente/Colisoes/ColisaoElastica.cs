using System;
using System.Collections.Generic;
using System.Drawing;

namespace AmbienteDeSimulacao.Ambiente.FisicaAmbiente.Colisoes
{
    class ColisaoElastica : IFilhosFisica
    {
        private ModeloAmbiente modeloAmbiente;
        private List<List<Point>> listaPontosSuperficies;
        private Utils.InterseccaoSegmentoReta interSegmentoReta;

        private float m1;
        private float m2;

        private double x1;
        private double y1;

        private double x2;
        private double y2;

        // Colisão ineslástica (perda de momentum)
        //private double R = 1;

        private double vx1;
        private double vx2;
        private double vy1;
        private double vy2;

        private double m21, dvx2, a, x21, y21, vx21, vy21, fy21, sign, vx_cm, vy_cm;


        #region Construtor

        public ColisaoElastica(ModeloAmbiente modeloAmbiente)
        {
            listaPontosSuperficies = new List<List<Point>>();
            interSegmentoReta = new Utils.InterseccaoSegmentoReta();

            /************** Padrão Observadores **************/
            this.modeloAmbiente = modeloAmbiente;
            /*************************************************/
        }

        #endregion


        public void Atualiza()
        {
            DeteccaoDeColisao();

            modeloAmbiente.cmAtualizado = false;
            modeloAmbiente.lcAtualizado = false;
            modeloAmbiente.scAtualizado = false;
        }


        #region Detecta Colisão

        public void DeteccaoDeColisao()
        {
            bool colisao = false;

            int idx = 0;
            int pAux = 0;
            int kAux = 0;

            if (listaPontosSuperficies.Count != 0)
            {
                listaPontosSuperficies.Clear();
            }

            // Carrega lista de pontos que definem a superfícies
            foreach (Corpos.Corpo c in modeloAmbiente.ListaCorpos)
            {
                listaPontosSuperficies.Add(c.ListaPontosSuperficie);
            }

            listaPontosSuperficies.Add(modeloAmbiente.ListaLimitesDoAmbiente);


            if (modeloAmbiente.ListaCorposColisao.Count != 0)
            {
                modeloAmbiente.ListaCorposColisao.Clear();
                modeloAmbiente.ListaPontosColisaoCorpos.Clear();
                modeloAmbiente.ListaPontosColisaoAmbiente.Clear();
            }

            // Laço para acessar a lista de pontos que compoem a superfífcie de cada corpo
            // Teste dos corpos e dos limites do ambiente
            for (int i = 0; i < listaPontosSuperficies.Count; i++)
            {
                // Laço para acessar cada ponto da superfície que está sendo testada
                for (int p = 0; p < listaPontosSuperficies[i].Count; p++)
                {
                    // Laço para acessar a outra superfície
                    for (int j = i + 1; j < listaPontosSuperficies.Count; j++)
                    {
                        // Laço para acessar cada ponto das outras supercícies
                        for (int k = 0; k < listaPontosSuperficies[j].Count; k++)
                        {
                            // Teste necessário para calcular a colisão com o último 
                            // segmento de linha (ultimo ponto com o primeiro ponto)
                            if (p == listaPontosSuperficies[i].Count - 1)
                            {
                                pAux = 0;
                            }
                            else
                            {
                                pAux = p + 1;
                            }

                            if (k == listaPontosSuperficies[j].Count - 1)
                            {
                                kAux = 0;
                            }
                            else
                            {
                                kAux = k + 1;
                            }

                            // Se os corpos testados se colidem em algum ponto, armazena o indice do corpo
                            colisao = interSegmentoReta.FasterLineSegmentIntersection(listaPontosSuperficies[i][p],
                                                                                      listaPontosSuperficies[i][pAux],
                                                                                      listaPontosSuperficies[j][k],
                                                                                      listaPontosSuperficies[j][kAux]);
                            if (colisao)
                            {
                                if (modeloAmbiente.ListaCorposColisao.Count == 0
                                || (modeloAmbiente.ListaCorposColisao[idx - 1] != i && modeloAmbiente.ListaCorposColisao[idx] != j))
                                {
                                    // Lista dos corpos que se colidiram
                                    modeloAmbiente.ListaCorposColisao.Add(i);
                                    modeloAmbiente.ListaCorposColisao.Add(j);

                                    // Seta o id do corpo que colidiu com a bola para tratar
                                    // quando há domínio ou defesa da bola pelo agente
                                    if (modeloAmbiente.ListaCorposColisao[0] == 0)
                                    {
                                        modeloAmbiente.idColisaoBola = modeloAmbiente.ListaCorposColisao[1];
                                    }
                                    else
                                    {
                                        modeloAmbiente.idColisaoBola = -1;
                                    }

                                    idx = modeloAmbiente.ListaCorposColisao.Count - 1;

                                    if (i == modeloAmbiente.ListaCorpos.Count)
                                    {
                                        modeloAmbiente.ListaPontosColisaoAmbiente.Add(listaPontosSuperficies[i][p]);
                                        modeloAmbiente.ListaPontosColisaoAmbiente.Add(listaPontosSuperficies[i][pAux]);
                                    }
                                    else
                                    {
                                        modeloAmbiente.ListaPontosColisaoCorpos.Add(listaPontosSuperficies[i][p]);
                                        modeloAmbiente.ListaPontosColisaoCorpos.Add(listaPontosSuperficies[i][pAux]);
                                    }

                                    if (j == modeloAmbiente.ListaCorpos.Count)
                                    {
                                        modeloAmbiente.ListaPontosColisaoAmbiente.Add(listaPontosSuperficies[j][k]);
                                        modeloAmbiente.ListaPontosColisaoAmbiente.Add(listaPontosSuperficies[j][kAux]);
                                    }
                                    else
                                    {
                                        modeloAmbiente.ListaPontosColisaoCorpos.Add(listaPontosSuperficies[j][k]);
                                        modeloAmbiente.ListaPontosColisaoCorpos.Add(listaPontosSuperficies[j][kAux]);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            /*for (int i = 0; i < modeloAmbiente.ListaCorpos.Count; i++)
            {
                // Problema de tunelamento 
                //if (modeloAmbiente.ListaCorpos[0].CentroDeMassa.Y < modeloAmbiente.ListaLimitesDoAmbiente[2].Y - 20
                //|| modeloAmbiente.ListaCorpos[0].CentroDeMassa.Y > modeloAmbiente.ListaLimitesDoAmbiente[5].Y + 20)
                //{
                if (modeloAmbiente.ListaCorpos[i].CentroDeMassa.X < modeloAmbiente.ListaLimitesDoAmbiente[0].X - 10)
                {
                    teste(i);
                    //objetivo.X = modeloAmbiente.ListaLimitesDoAmbiente[0].X + 20;
                }
                else if (modeloAmbiente.ListaCorpos[i].CentroDeMassa.X > modeloAmbiente.ListaLimitesDoAmbiente[1].X + 10)
                {
                    teste(i);
                    //objetivo.X = modeloAmbiente.ListaLimitesDoAmbiente[1].X - 20;
                }
                //}

                if (modeloAmbiente.ListaCorpos[i].CentroDeMassa.Y > modeloAmbiente.ListaLimitesDoAmbiente[7].Y + 10)
                {
                    teste(i);
                    //objetivo.Y = modeloAmbiente.ListaLimitesDoAmbiente[6].Y - 20;
                }
                else if (modeloAmbiente.ListaCorpos[i].CentroDeMassa.Y < modeloAmbiente.ListaLimitesDoAmbiente[0].Y - 10)
                {
                    teste(i);
                    //objetivo.Y = modeloAmbiente.ListaLimitesDoAmbiente[0].Y + 20;
                }
                //}
            }*/

            if (modeloAmbiente.ListaCorposColisao.Count != 0)
            {
                ResolveColisao();
            }
        }

        #endregion


        #region Resolve colisões

        private void ResolveColisao()
        {
            ColisaoAmbiente();
            ColisaoBidimencional();
        }

        #endregion


        #region Colisão Bidimencional

        private void ColisaoBidimencional()
        {
            // Vetor para o cálculo do módulo do deslocamento do corpo
            Utils.Vetor2 v1 = new Utils.Vetor2();
            Utils.Vetor2 v2 = new Utils.Vetor2();

            //int colisoes = modeloAmbiente.ListaCorposColisao.Count;

            // Incremento de dois para tratar em duplas de colisão
            for (int i = 0; i < modeloAmbiente.ListaCorposColisao.Count; i += 2)
            {
                // Colisão entre os corpos
                m1 = modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].MassaCorpo;

                v1.X = modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i]][0, 0] / (float)0.015;
                v1.Y = modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i]][0, 1] / (float)0.015;

                //v2.X = modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i + 1]][0, 0] / (float)0.015;
                //v2.Y = modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i + 1]][0, 1] / (float)0.015;

                x1 = modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].CentroDeMassa.X;
                y1 = modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].CentroDeMassa.Y;

                //x2 = modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i + 1]].CentroDeMassa.X;
                //y2 = modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i + 1]].CentroDeMassa.Y;

                if (modeloAmbiente.ListaCorposColisao[i + 1] == 5)
                {
                    // Inicializa as variáveis caso a colisão for com o ambiente
                    m2 = 99999;
                    v2.X = 0;
                    v2.Y = 0;

                    // Centro de massa auxiliar para tratar colisão com o ambiente
                    if (modeloAmbiente.ListaPontosColisaoAmbiente.Count > 0
                    && modeloAmbiente.ListaPontosColisaoAmbiente[0].X == modeloAmbiente.ListaPontosColisaoAmbiente[1].X)
                    {
                        x2 = modeloAmbiente.ListaPontosColisaoAmbiente[0].X;
                        y2 = modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].CentroDeMassa.Y;
                    }
                    else if (modeloAmbiente.ListaPontosColisaoAmbiente.Count > 0)
                    {
                        x2 = modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].CentroDeMassa.X;
                        y2 = modeloAmbiente.ListaPontosColisaoAmbiente[0].Y;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    m2 = modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i + 1]].MassaCorpo;

                    v2.X = modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i + 1]][0, 0] / (float)0.015;
                    v2.Y = modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i + 1]][0, 1] / (float)0.015;

                    x2 = modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i + 1]].CentroDeMassa.X;
                    y2 = modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i + 1]].CentroDeMassa.Y;
                }

                vx1 = v1.X;
                vx2 = v2.X;
                vy1 = v1.Y;
                vy2 = v2.Y;

                m21 = m2 / m1;
                x21 = x2 - x1;
                y21 = y2 - y1;
                vx21 = vx2 - vx1;
                vy21 = vy2 - vy1;

                //vx_cm = (m1 * vx1 + m2 * vx2) / (m1 + m2);
                //vy_cm = (m1 * vy1 + m2 * vy2) / (m1 + m2);

                //     *** return old velocities if balls are not approaching ***
                if ((vx21 * x21 + vy21 * y21) >= 0) return;


                //     *** I have inserted the following statements to avoid a zero divide; 
                //         (for single precision calculations, 
                //          1.0E-12 should be replaced by a larger value). **************  

                fy21 = 1.0E-12 * Math.Abs(y21);
                if (Math.Abs(x21) < fy21)
                {
                    if (x21 < 0) { sign = -1; } else { sign = 1; }
                    x21 = fy21 * sign;
                }

                //     ***  update velocities ***
                a = y21 / x21;
                dvx2 = -2 * (vx21 + a * vy21) / ((1 + a * a) * (1 + m21));
                vx2 = vx2 + dvx2;
                vy2 = vy2 + a * dvx2;
                vx1 = vx1 - m21 * dvx2;
                vy1 = vy1 - a * m21 * dvx2;

                //     ***  velocity correction for inelastic collisions ***
                //vx1 = (vx1 - vx_cm) * R + vx_cm;
                //vy1 = (vy1 - vy_cm) * R + vy_cm;
                //vx2 = (vx2 - vx_cm) * R + vx_cm;
                //vy2 = (vy2 - vy_cm) * R + vy_cm;

                // Deslocamento resultante após calculo de colisão bidimencional
                modeloAmbiente.ListaAcumDeslocamento[modeloAmbiente.ListaCorposColisao[i]][0, 0] = Convert.ToSingle(vx1) * (float)0.015;

                modeloAmbiente.ListaAcumDeslocamento[modeloAmbiente.ListaCorposColisao[i]][0, 1] = Convert.ToSingle(vy1) * (float)0.015;

                if (modeloAmbiente.ListaCorposColisao[i + 1] < 5)
                {
                    modeloAmbiente.ListaAcumDeslocamento[modeloAmbiente.ListaCorposColisao[i + 1]][0, 0] = Convert.ToSingle(vx2) * (float)0.015;

                    modeloAmbiente.ListaAcumDeslocamento[modeloAmbiente.ListaCorposColisao[i + 1]][0, 1] = Convert.ToSingle(vy2) * (float)0.015;
                }
            }
        }

        #endregion


        #region Colisão com o Ambiente

        private void ColisaoAmbiente()
        {
            // Colisão com os limites do ambiente
            // Força contrátria mais bloqueio de deslocamento além dos limites do ambiente
            for (int i = 0; i < modeloAmbiente.ListaPontosColisaoAmbiente.Count - 1; i += 2)
            {
                // Testa se é um dos limites horizontais
                if (modeloAmbiente.ListaPontosColisaoAmbiente[i].Y == modeloAmbiente.ListaPontosColisaoAmbiente[i + 1].Y)
                {
                    // Se for sabe-se que deve fazer força contrária para Y+ ou Y-
                    if ((modeloAmbiente.ListaLimitesDoAmbiente[0] == modeloAmbiente.ListaPontosColisaoAmbiente[i]
                    && modeloAmbiente.ListaLimitesDoAmbiente[1] == modeloAmbiente.ListaPontosColisaoAmbiente[i + 1])
                    || (modeloAmbiente.ListaLimitesDoAmbiente[2] == modeloAmbiente.ListaPontosColisaoAmbiente[i]
                    && modeloAmbiente.ListaLimitesDoAmbiente[3] == modeloAmbiente.ListaPontosColisaoAmbiente[i + 1])
                    || (modeloAmbiente.ListaLimitesDoAmbiente[10] == modeloAmbiente.ListaPontosColisaoAmbiente[i]
                    && modeloAmbiente.ListaLimitesDoAmbiente[11] == modeloAmbiente.ListaPontosColisaoAmbiente[i + 1]))
                    {
                        // Força para Y+
                        if (modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i]][0, 1] <= 0)
                        {
                            // Colisão na parte superior ou inferior do ambiente
                            modeloAmbiente.ListaAcumDeslocamento[modeloAmbiente.ListaCorposColisao[0]][0, 1] *= -1;
                            modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].CentroDeMassa = new Point(modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].Localizacao.X,
                                                                                                                       modeloAmbiente.ListaPontosColisaoAmbiente[i].Y);
                        }

                        if (modeloAmbiente.ListaLimitesDoAmbiente[2] == modeloAmbiente.ListaPontosColisaoAmbiente[i])
                        {
                            // Força para X+
                            if (modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i]][0, 0] >= 0
                            && modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i]][0, 1] == 0)
                            {
                                // Colisão na parte superior ou inferior do ambiente
                                modeloAmbiente.ListaAcumDeslocamento[modeloAmbiente.ListaCorposColisao[0]][0, 0] *= -1;
                                modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].CentroDeMassa = new Point(modeloAmbiente.ListaPontosColisaoAmbiente[i].X,
                                                                                                                           modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].Localizacao.Y);
                            }
                        }
                    }
                    else if ((modeloAmbiente.ListaLimitesDoAmbiente[4] == modeloAmbiente.ListaPontosColisaoAmbiente[i]
                         && modeloAmbiente.ListaLimitesDoAmbiente[5] == modeloAmbiente.ListaPontosColisaoAmbiente[i + 1])
                         || (modeloAmbiente.ListaLimitesDoAmbiente[6] == modeloAmbiente.ListaPontosColisaoAmbiente[i]
                         && modeloAmbiente.ListaLimitesDoAmbiente[7] == modeloAmbiente.ListaPontosColisaoAmbiente[i + 1])
                         || (modeloAmbiente.ListaLimitesDoAmbiente[8] == modeloAmbiente.ListaPontosColisaoAmbiente[i]
                         && modeloAmbiente.ListaLimitesDoAmbiente[9] == modeloAmbiente.ListaPontosColisaoAmbiente[i + 1]))
                    {
                        // Força para Y-
                        if (modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i]][0, 1] >= 0)
                        {
                            // Colisão na parte superior ou inferior do ambiente
                            modeloAmbiente.ListaAcumDeslocamento[modeloAmbiente.ListaCorposColisao[0]][0, 1] *= -1;
                            modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].CentroDeMassa = new Point(modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].Localizacao.X,
                                                                                                                       modeloAmbiente.ListaPontosColisaoAmbiente[i].Y);
                        }

                        if (modeloAmbiente.ListaLimitesDoAmbiente[8] == modeloAmbiente.ListaPontosColisaoAmbiente[i]
                         || modeloAmbiente.ListaLimitesDoAmbiente[5] == modeloAmbiente.ListaPontosColisaoAmbiente[i + 1])
                        {
                            // Força para X+
                            if (modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i]][0, 0] <= 0
                            && modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i]][0, 1] == 0)
                            {
                                // Colisão na parte superior ou inferior do ambiente
                                modeloAmbiente.ListaAcumDeslocamento[modeloAmbiente.ListaCorposColisao[0]][0, 0] *= -1;
                                modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].CentroDeMassa = new Point(modeloAmbiente.ListaPontosColisaoAmbiente[i].X,
                                                                                                                         modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].Localizacao.Y);
                            }
                        }
                    }
                } // Testa se é um dos limites verticais
                else if (modeloAmbiente.ListaPontosColisaoAmbiente[i].X == modeloAmbiente.ListaPontosColisaoAmbiente[i + 1].X)
                {
                    // Se for sabe-se que deve fazer força contrária para X+ ou X-
                    if ((modeloAmbiente.ListaLimitesDoAmbiente[1] == modeloAmbiente.ListaPontosColisaoAmbiente[i]
                    && modeloAmbiente.ListaLimitesDoAmbiente[2] == modeloAmbiente.ListaPontosColisaoAmbiente[i + 1])
                    || (modeloAmbiente.ListaLimitesDoAmbiente[3] == modeloAmbiente.ListaPontosColisaoAmbiente[i]
                    && modeloAmbiente.ListaLimitesDoAmbiente[4] == modeloAmbiente.ListaPontosColisaoAmbiente[i + 1])
                    || (modeloAmbiente.ListaLimitesDoAmbiente[5] == modeloAmbiente.ListaPontosColisaoAmbiente[i]
                    && modeloAmbiente.ListaLimitesDoAmbiente[6] == modeloAmbiente.ListaPontosColisaoAmbiente[i + 1]))
                    {
                        // Força para X-
                        if (modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i]][0, 0] >= 0)
                        {
                            // Colisão na parte superior ou inferior do ambiente
                            modeloAmbiente.ListaAcumDeslocamento[modeloAmbiente.ListaCorposColisao[0]][0, 0] *= -1;
                            modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].CentroDeMassa = new Point(modeloAmbiente.ListaPontosColisaoAmbiente[i].X,
                                                                                                                     modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].Localizacao.Y);
                        }

                        if (modeloAmbiente.ListaLimitesDoAmbiente[5] == modeloAmbiente.ListaPontosColisaoAmbiente[i])
                        {
                            // Força para Y-
                            if (modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i]][0, 1] >= 0
                            && modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i]][0, 0] == 0)
                            {
                                // Colisão na parte superior ou inferior do ambiente
                                modeloAmbiente.ListaAcumDeslocamento[modeloAmbiente.ListaCorposColisao[0]][0, 1] *= -1;
                                modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].CentroDeMassa = new Point(modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].Localizacao.X,
                                                                                                                         modeloAmbiente.ListaPontosColisaoAmbiente[i].Y);
                            }
                        }
                    }
                    else if ((modeloAmbiente.ListaLimitesDoAmbiente[7] == modeloAmbiente.ListaPontosColisaoAmbiente[i]
                         && modeloAmbiente.ListaLimitesDoAmbiente[8] == modeloAmbiente.ListaPontosColisaoAmbiente[i + 1])
                         || (modeloAmbiente.ListaLimitesDoAmbiente[9] == modeloAmbiente.ListaPontosColisaoAmbiente[i]
                         && modeloAmbiente.ListaLimitesDoAmbiente[10] == modeloAmbiente.ListaPontosColisaoAmbiente[i + 1])
                         || (modeloAmbiente.ListaLimitesDoAmbiente[11] == modeloAmbiente.ListaPontosColisaoAmbiente[i]
                         && modeloAmbiente.ListaLimitesDoAmbiente[0] == modeloAmbiente.ListaPontosColisaoAmbiente[i + 1]))
                    {
                        // Força para X+
                        if (modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i]][0, 0] <= 0)
                        {
                            // Colisão na parte superior ou inferior do ambiente
                            modeloAmbiente.ListaAcumDeslocamento[modeloAmbiente.ListaCorposColisao[0]][0, 0] *= -1;
                            modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].CentroDeMassa = new Point(modeloAmbiente.ListaPontosColisaoAmbiente[i].X,
                                                                                                                     modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].Localizacao.Y);
                        }
                        if (modeloAmbiente.ListaLimitesDoAmbiente[11] == modeloAmbiente.ListaPontosColisaoAmbiente[i])
                        {
                            // Força para Y+
                            if (modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i]][0, 1] <= 0
                            && modeloAmbiente.ListaDeslocamentoAtual[modeloAmbiente.ListaCorposColisao[i]][0, 0] == 0)
                            {
                                // Colisão na parte superior ou inferior do ambiente
                                modeloAmbiente.ListaAcumDeslocamento[modeloAmbiente.ListaCorposColisao[0]][0, 1] *= -1;
                                modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].CentroDeMassa = new Point(modeloAmbiente.ListaCorpos[modeloAmbiente.ListaCorposColisao[i]].Localizacao.X,
                                                                                                                         modeloAmbiente.ListaPontosColisaoAmbiente[i].Y);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
