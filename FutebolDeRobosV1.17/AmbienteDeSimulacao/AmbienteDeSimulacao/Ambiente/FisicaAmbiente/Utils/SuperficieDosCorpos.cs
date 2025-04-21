using System;
using System.Collections.Generic;
using System.Drawing;

namespace AmbienteDeSimulacao.Ambiente.FisicaAmbiente.Utils
{
    class SuperficieDosCorpos : IFilhosFisica
    {
        private ModeloAmbiente modeloAmbiente;

        #region Construtor

        public SuperficieDosCorpos(ModeloAmbiente modeloAmbiente)
        {
            // Instancia o modelo do ambiente
            this.modeloAmbiente = modeloAmbiente;
        }

        public SuperficieDosCorpos(Rectangle limitesDoAmbiente, ModeloAmbiente modeloAmbiente)
        {
            // Instancia o modelo do ambiente
            this.modeloAmbiente = modeloAmbiente;

            CalculaPontosSuperficieRetangular();
            CalculaPontosSuperficieCircular();
            LimitesAmbiente(limitesDoAmbiente);
        }

        #endregion


        public void Atualiza()
        {
            if (modeloAmbiente.scAtualizado)
            {
                return;
            }

            MontaListaPontosSuperficieCorpos();
            modeloAmbiente.scAtualizado = true;
        }


        #region Pontos Superfície Retangular

        public void CalculaPontosSuperficieRetangular()
        {
            List<Point> listaPontosSuperficieRetangular = new List<Point>();

            // Calcula o "raio" do agente (pega a largura do primeiro agente para calcular, pois são todos iguais)
            double raioAgente1 = modeloAmbiente.ListaCorpos[1].Dimencao[0, 0] / (2 * Math.Cos(Math.PI / 4)); // 21.21320344;
            double raioAgente2 = modeloAmbiente.ListaCorpos[1].Dimencao[0, 0] / 2;

            // Define a defasagem anbular de cada ponto que define a superfície do agente
            double defPonto1 = 0.5 * Math.PI;
            double defPonto2 = 2 * Math.PI;
            double defPonto3 = 1.5 * Math.PI;
            double defPonto4 = Math.PI;
            double defPonto5 = Math.PI / 4;

            // Valor inicial do acumulador para iniciar a matriz de pontos 
            // utilizado para deixar o primeiro conjunto de pontos da matriz correspondente a 
            // posição inicial que o agente vai iniciar em campo 
            double acumAux = -Math.PI/4;

            // Inicializa a matriz dos pontos correspontes para todo ângulo de rotação discretizado no uso de sprites
            // incremento de pi/12 desta forma o método que determina os pontos da superfície do agente deve apenas acessar
            // o respectivo conjunto de pontos que correspondente do sprite atual 
            for (int i = 0; i < 24; i++)
            {
                listaPontosSuperficieRetangular.Add(new Point(Convert.ToInt32(raioAgente1 * Math.Cos(defPonto1 + acumAux)),
                                                              Convert.ToInt32(raioAgente1 * Math.Sin(defPonto1 + acumAux))));

                listaPontosSuperficieRetangular.Add(new Point(Convert.ToInt32(raioAgente1 * Math.Cos(defPonto2 + acumAux)),
                                                              Convert.ToInt32(raioAgente1 * Math.Sin(defPonto2 + acumAux))));

                listaPontosSuperficieRetangular.Add(new Point(Convert.ToInt32(raioAgente1 * Math.Cos(defPonto3 + acumAux)),
                                                              Convert.ToInt32(raioAgente1 * Math.Sin(defPonto3 + acumAux))));

                listaPontosSuperficieRetangular.Add(new Point(Convert.ToInt32(raioAgente1 * Math.Cos(defPonto4 + acumAux)),
                                                              Convert.ToInt32(raioAgente1 * Math.Sin(defPonto4 + acumAux))));

                listaPontosSuperficieRetangular.Add(new Point(Convert.ToInt32(raioAgente2 * Math.Cos(defPonto5 + acumAux)),
                                                              Convert.ToInt32(raioAgente2 * Math.Sin(defPonto5 + acumAux))));

                acumAux += Math.PI/12;
            }

            // Carrega a lista no modelo do ambiente
            modeloAmbiente.ListaPontosSuperficieRetangular = listaPontosSuperficieRetangular;
        }

        #endregion


        #region Pontos Superfície Circular

        public void CalculaPontosSuperficieCircular()
        {
            List<Point> listaPontosSuperficieCircular = new List<Point>();

            int raioBola = modeloAmbiente.ListaCorpos[0].Dimencao[0, 0] / 2;

            // Pontos que discretizam a superfície circular
            listaPontosSuperficieCircular.Add(new Point(0, raioBola));
            listaPontosSuperficieCircular.Add(new Point(Convert.ToInt32(raioBola * Math.Cos(0.7853)), Convert.ToInt32(raioBola * Math.Sin(0.7853))));
            listaPontosSuperficieCircular.Add(new Point(raioBola, 0));
            listaPontosSuperficieCircular.Add(new Point(Convert.ToInt32(raioBola * Math.Cos(0.7853)), -Convert.ToInt32(raioBola * Math.Sin(0.7853))));
            listaPontosSuperficieCircular.Add(new Point(0, -raioBola));
            listaPontosSuperficieCircular.Add(new Point(-Convert.ToInt32(raioBola * Math.Cos(0.7853)), -Convert.ToInt32(raioBola * Math.Sin(0.7853))));
            listaPontosSuperficieCircular.Add(new Point(-raioBola, 0));
            listaPontosSuperficieCircular.Add(new Point(-Convert.ToInt32(raioBola * Math.Cos(0.7853)), Convert.ToInt32(raioBola * Math.Sin(0.7853))));

            // Carrega a lista no modelo do ambiente
            modeloAmbiente.ListaPontosSuperficieCircular = listaPontosSuperficieCircular;
        }

        #endregion


        #region Pontos Superfície Ambiente

        public void LimitesAmbiente(Rectangle limitesDoAmbiente)
        {
            // Lateral Superior
            modeloAmbiente.ListaLimitesDoAmbiente.Add(new Point(42, 30));
            modeloAmbiente.ListaLimitesDoAmbiente.Add(new Point(limitesDoAmbiente.Width - 38, 30));
            // Trave 1 Time Vermelho
            modeloAmbiente.ListaLimitesDoAmbiente.Add(new Point(limitesDoAmbiente.Width - 38, 165));
            // Segmento do fundo do gol
            modeloAmbiente.ListaLimitesDoAmbiente.Add(new Point(limitesDoAmbiente.Width - 8, 165));
            modeloAmbiente.ListaLimitesDoAmbiente.Add(new Point(limitesDoAmbiente.Width - 8, 285));
            // Trave 2 Time Vermelho
            modeloAmbiente.ListaLimitesDoAmbiente.Add(new Point(limitesDoAmbiente.Width - 38, 285));
            // Lateral Inferior
            modeloAmbiente.ListaLimitesDoAmbiente.Add(new Point(limitesDoAmbiente.Width - 38, limitesDoAmbiente.Height - 30));
            modeloAmbiente.ListaLimitesDoAmbiente.Add(new Point(42, limitesDoAmbiente.Height - 30));
            // Trave 2 Time Azul
            modeloAmbiente.ListaLimitesDoAmbiente.Add(new Point(42, limitesDoAmbiente.Height - 165));
            // Segmento do fundo do gol
            modeloAmbiente.ListaLimitesDoAmbiente.Add(new Point(12, limitesDoAmbiente.Height - 165));
            modeloAmbiente.ListaLimitesDoAmbiente.Add(new Point(12, limitesDoAmbiente.Height - 288));
            // Trave 1 Time Azul
            modeloAmbiente.ListaLimitesDoAmbiente.Add(new Point(42, limitesDoAmbiente.Height - 288));


            // Limites goleira esquerda
            modeloAmbiente.ListaLimitesGoleiraEsq.Add(modeloAmbiente.ListaLimitesDoAmbiente[10]);
            modeloAmbiente.ListaLimitesGoleiraEsq.Add(modeloAmbiente.ListaLimitesDoAmbiente[11]);
            modeloAmbiente.ListaLimitesGoleiraEsq.Add(modeloAmbiente.ListaLimitesDoAmbiente[8]);
            modeloAmbiente.ListaLimitesGoleiraEsq.Add(modeloAmbiente.ListaLimitesDoAmbiente[9]);
            // Limites goleira direita
            modeloAmbiente.ListaLimitesGoleiraDir.Add(modeloAmbiente.ListaLimitesDoAmbiente[2]);
            modeloAmbiente.ListaLimitesGoleiraDir.Add(modeloAmbiente.ListaLimitesDoAmbiente[3]);
            modeloAmbiente.ListaLimitesGoleiraDir.Add(modeloAmbiente.ListaLimitesDoAmbiente[4]);
            modeloAmbiente.ListaLimitesGoleiraDir.Add(modeloAmbiente.ListaLimitesDoAmbiente[5]);
            // Limites área esquerda
            modeloAmbiente.ListaLimitesAreaEsq.Add(new Point(42,138));
            modeloAmbiente.ListaLimitesAreaEsq.Add(new Point(116, 138));
            modeloAmbiente.ListaLimitesAreaEsq.Add(new Point(116, 313));
            modeloAmbiente.ListaLimitesAreaEsq.Add(new Point(42, 313));
            // Limites área direita
            modeloAmbiente.ListaLimitesAreaDir.Add(new Point(490, 138));
            modeloAmbiente.ListaLimitesAreaDir.Add(new Point(562, 138));
            modeloAmbiente.ListaLimitesAreaDir.Add(new Point(562, 313));
            modeloAmbiente.ListaLimitesAreaDir.Add(new Point(490, 313));
            // Limites meio campo esquerdo
            modeloAmbiente.ListaLimitesMeioCampoEsq.Add(modeloAmbiente.ListaLimitesDoAmbiente[0]);
            modeloAmbiente.ListaLimitesMeioCampoEsq.Add(new Point(302, 30));
            modeloAmbiente.ListaLimitesMeioCampoEsq.Add(new Point(302, 419));
            modeloAmbiente.ListaLimitesMeioCampoEsq.Add(modeloAmbiente.ListaLimitesDoAmbiente[7]);
            // Limites meio campo direito
            modeloAmbiente.ListaLimitesMeioCampoDir.Add(new Point(302, 30));
            modeloAmbiente.ListaLimitesMeioCampoDir.Add(modeloAmbiente.ListaLimitesDoAmbiente[1]);
            modeloAmbiente.ListaLimitesMeioCampoDir.Add(modeloAmbiente.ListaLimitesDoAmbiente[6]);
            modeloAmbiente.ListaLimitesMeioCampoDir.Add(new Point(302, 419));
        }

        #endregion


        #region Lista Pontos que Compoem a Superfície dos Corpos

        public void MontaListaPontosSuperficieCorpos()
        {
            // Recebe o índice inicial para acessar o conjunto de pontos 
            // correspondente que define a superfície do sprite atual
            int idxSpriteAux = 0;

            // Lista auxiliar para armazenar os pontos de cada corpo
            List<Point> listaPontosSuperficieCorpoAux;

            // Translada as coordenadas dos pontos que compoem a superfície da bola
            for (int i = 0; i < modeloAmbiente.ListaCorpos.Count; i++)
            {
                if (i > 0)
                {
                    idxSpriteAux = modeloAmbiente.ListaCorpos[i].Sprite * 5;

                    listaPontosSuperficieCorpoAux = new List<Point>();

                    // Translada as coordenadas dos pontos que compoem a superfície dos agentes.      
                    for (int k = 0; k < 5; k++)
                    {                       
                        listaPontosSuperficieCorpoAux.Add(new Point(modeloAmbiente.ListaPontosSuperficieRetangular[idxSpriteAux].X + modeloAmbiente.ListaCorpos[i].CentroDeMassa.X,
                                                                    modeloAmbiente.ListaPontosSuperficieRetangular[idxSpriteAux].Y + modeloAmbiente.ListaCorpos[i].CentroDeMassa.Y));
                        idxSpriteAux++;
                    }

                    modeloAmbiente.ListaCorpos[i].ListaPontosSuperficie = listaPontosSuperficieCorpoAux;
                }
                else
                {
                    listaPontosSuperficieCorpoAux = new List<Point>();

                    // Translada as coordenadas dos pontos que compoem a superfície da bola
                    foreach (Point p in modeloAmbiente.ListaPontosSuperficieCircular)
                    {
                        listaPontosSuperficieCorpoAux.Add(new Point(p.X + modeloAmbiente.ListaCorpos[i].CentroDeMassa.X, 
                                                                    p.Y + modeloAmbiente.ListaCorpos[i].CentroDeMassa.Y));
                    }

                    modeloAmbiente.ListaCorpos[i].ListaPontosSuperficie = listaPontosSuperficieCorpoAux;
                }
            }
        }

        #endregion
    }
}
