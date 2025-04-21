using System;

namespace AmbienteDeSimulacao.Ambiente.FisicaAmbiente.Movimento
{
    class MRU : IFilhosFisica
    {
        private ModeloAmbiente modeloAmbiente;

        private float tempo;
        private float tempoQuad;


        #region Construtor

        public MRU(float tempo, ModeloAmbiente modeloAmbiente)
        {
            this.tempo = tempo / 1000; // Conversao de milisegundos para segundos                     
  
            tempoQuad = this.tempo * this.tempo;

            /************** Padrão Observadores **************/
            this.modeloAmbiente = modeloAmbiente;
            /*************************************************/
        }

        #endregion


        public void Atualiza()
        {
            MovimentoRetilineoUniforme();

            modeloAmbiente.cmAtualizado = false;
            modeloAmbiente.lcAtualizado = false;
            modeloAmbiente.scAtualizado = false;
        }


        #region Movimento Retilíneo Uniforme

        public void MovimentoRetilineoUniforme(/*List<float[,]> forcaAceleracao*/)
        {
            // Calcula a força resultante entre a força de aceleração e desaceleração
            //ForcaResultante();

            // Calcula a aceleração resultante para as forças aplicadas nos corpos
            //Aceleracao();

            // Calcula o deslocamento resultante da aceleração aplicada nos corpos 
            // em determinado período de tempo
            Deslocamento();
            
            //modeloAmbiente.ListaForcaAnterior = modeloAmbiente.ListaForcaAtual;
            //modeloAmbiente.ListaAceleracaoAnterior = modeloAmbiente.ListaAceleracaoAtual;
            modeloAmbiente.ListaDeslocamentoAnterior = modeloAmbiente.ListaDeslocamentoAtual;

            /*for (int i = 0; i < modeloAmbiente.ListaCorpos.Count; i++)
            {
                modeloAmbiente.ListaForcaAtual[i] = new float[1, 2] { { 0 , 0 } };
                modeloAmbiente.ListaAceleracaoAtual[i] = new float[1, 2] { { 0, 0 } };
            }*/
        }

        #endregion


        #region Força

        // A lista forcaResultanteAtual armazena uma lista com a força resultante de todos 
        // os corpos na iteração atual
        // Força de desaceleração é sinal oposto a força de aceleração
        private void ForcaResultante( /*List<float[,]> forcaAceleracao*/ )
        {
            for (int i = 0; i < modeloAmbiente.ListaCorpos.Count; i++)
            {
                if (i > 0)
                {
                    // Calcula a força resultado dos jogadores 
                    // força aceleração - forca de atrito => (mi * força aceleração)
                    modeloAmbiente.ListaForcaAtual[i][0, 0] = Convert.ToSingle(1.0 * modeloAmbiente.ListaForcaAtual[i][0, 0]);
                    modeloAmbiente.ListaForcaAtual[i][0, 1] = Convert.ToSingle(1.0 * modeloAmbiente.ListaForcaAtual[i][0, 1]);
                }
                else
                {
                    // Calcula a força resultante da bola através da força que já existe movento o corpo
                    // da bola, uma vez que a bola é um objeto passivo, ou seja só possui força se um outro
                    // corpo tranferir a bola 
                    modeloAmbiente.ListaForcaAtual[i][0, 0] = Convert.ToSingle(1.0 * modeloAmbiente.ListaForcaAtual[i][0, 0]);
                    modeloAmbiente.ListaForcaAtual[i][0, 1] = Convert.ToSingle(1.0 * modeloAmbiente.ListaForcaAtual[i][0, 1]);
                }
            }
        }

        #endregion


        #region Aceleração

        // Calcula a aceleração do corpo (diferença entre as forças / massa do corpo)  
        private void Aceleracao()
        {            
            for (int i = 0; i < modeloAmbiente.ListaCorpos.Count; i++)
            {
                // A primeira posição das listas devem ser reservadas para a bola
                if (i > 0)
                {
                    modeloAmbiente.ListaAceleracaoAtual[i][0, 0] = Convert.ToSingle(modeloAmbiente.ListaForcaAtual[i][0, 0] / modeloAmbiente.ListaCorpos[i].MassaCorpo);
                    modeloAmbiente.ListaAceleracaoAtual[i][0, 1] = Convert.ToSingle(modeloAmbiente.ListaForcaAtual[i][0, 1] / modeloAmbiente.ListaCorpos[i].MassaCorpo);

                    modeloAmbiente.ListaAceleracaoAnterior[i][0, 0] = Convert.ToSingle(modeloAmbiente.ListaForcaAnterior[i][0, 0] / modeloAmbiente.ListaCorpos[i].MassaCorpo);
                    modeloAmbiente.ListaAceleracaoAnterior[i][0, 1] = Convert.ToSingle(modeloAmbiente.ListaForcaAnterior[i][0, 1] / modeloAmbiente.ListaCorpos[i].MassaCorpo);
                }
                else
                {
                    modeloAmbiente.ListaAceleracaoAtual[i][0, 0] = Convert.ToSingle(modeloAmbiente.ListaForcaAtual[i][0, 0] / modeloAmbiente.ListaCorpos[i].MassaCorpo);
                    modeloAmbiente.ListaAceleracaoAtual[i][0, 1] = Convert.ToSingle(modeloAmbiente.ListaForcaAtual[i][0, 1] / modeloAmbiente.ListaCorpos[i].MassaCorpo);

                    modeloAmbiente.ListaAceleracaoAnterior[i][0, 0] = Convert.ToSingle(modeloAmbiente.ListaForcaAnterior[i][0, 0] / modeloAmbiente.ListaCorpos[i].MassaCorpo);
                    modeloAmbiente.ListaAceleracaoAnterior[i][0, 1] = Convert.ToSingle(modeloAmbiente.ListaForcaAnterior[i][0, 1] / modeloAmbiente.ListaCorpos[i].MassaCorpo);
                }
            }
        }

        #endregion


        #region Deslocamento

        // Método deslocamento retorna apenas o deslocamento calculado a partir das forças recebidas por parâmetro
        // a classe física não se interessa em onde o corpo está mas sim em aplicar as leis da física a ele
        // portanto a consequência disso é que o efeito das leis da física serão vistos na interação do corpo
        // com o ambiente
        public void Deslocamento()
        {
            //float[,] aux;
            //float[,] aux2;

            // Equação do deslocamento
            // S-acum = S-acum + S0 + (a0*t^2) + [(1/2)*a*t^2]
            // S-acum acumula o deslocamento caso o calculo resulta em menos de 1 pxl
            for (int i = 0; i < modeloAmbiente.ListaCorpos.Count; i++)
            {
                //aux = new float[1, 2];
                //aux2 = new float[1, 2];

                // a0*t^2
                // a0*t integrado -> v0
                //aux[0, 0] = Convert.ToSingle(0.5 * modeloAmbiente.ListaAceleracaoAnterior[i][0, 0] * tempoQuad);
                //aux[0, 1] = Convert.ToSingle(0.5 * modeloAmbiente.ListaAceleracaoAnterior[i][0, 1] * tempoQuad);

                // (1/2)*a*t^2
                //aux2[0, 0] = Convert.ToSingle(modeloAmbiente.ListaAceleracaoAtual[i][0, 0] * (tempoQuad / 2));
                //aux2[0, 1] = Convert.ToSingle(modeloAmbiente.ListaAceleracaoAtual[i][0, 1] * (tempoQuad / 2));


                modeloAmbiente.ListaAcumDeslocamento[i][0, 0] = modeloAmbiente.ListaAcumDeslocamento[i][0, 0] + modeloAmbiente.ListaDeslocamentoAnterior[i][0, 0]; //+ aux[0, 0] + aux2[0, 0];
                modeloAmbiente.ListaAcumDeslocamento[i][0, 1] = modeloAmbiente.ListaAcumDeslocamento[i][0, 1] + modeloAmbiente.ListaDeslocamentoAnterior[i][0, 1]; //+ aux[0, 1] + aux2[0, 1];               
            }
        }

        #endregion
    }
}
