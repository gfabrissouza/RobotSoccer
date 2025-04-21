using System;

namespace AmbienteDeSimulacao.Ambiente.FisicaAmbiente.Movimento
{
    class ConsisteDeslocamento : IFilhosFisica
    {
        ModeloAmbiente modeloAmbiente;


        #region Construtor

        public ConsisteDeslocamento(ModeloAmbiente modeloAmbiente)
        {
            this.modeloAmbiente = modeloAmbiente;
        }

        #endregion


        public void Atualiza()
        {
            Consiste();
        }


        #region Consistir

        public void Consiste()
        {
            // S = S-acum
            for (int i = 0; i < modeloAmbiente.ListaCorpos.Count; i++)
            {
                // Consiste Movimento
                modeloAmbiente.ListaDeslocamentoAtual[i][0, 0] = modeloAmbiente.ListaAcumDeslocamento[i][0, 0];
                modeloAmbiente.ListaDeslocamentoAtual[i][0, 1] = modeloAmbiente.ListaAcumDeslocamento[i][0, 1];

                // Condição para zerar o deslocamento
                if (Math.Abs(modeloAmbiente.ListaDeslocamentoAtual[i][0, 0]) < 0.5) // 1
                {
                    modeloAmbiente.ListaDeslocamentoAtual[i][0, 0] = 0;
                }

                if (Math.Abs(modeloAmbiente.ListaDeslocamentoAtual[i][0, 1]) < 0.5) // 1
                {
                    modeloAmbiente.ListaDeslocamentoAtual[i][0, 1] = 0;
                }

                // Limpa o acumulador
                if (Math.Abs(modeloAmbiente.ListaAcumDeslocamento[i][0, 0]) >= 1 
                ||  Math.Abs(modeloAmbiente.ListaAcumDeslocamento[i][0, 0]) < 0.001)
                {
                    modeloAmbiente.ListaAcumDeslocamento[i][0, 0] = 0;
                }
                else
                {
                    modeloAmbiente.ListaAcumDeslocamento[i][0, 0] *= (float)eAmortecimento.amortecimento;
                }

                if (Math.Abs(modeloAmbiente.ListaAcumDeslocamento[i][0, 1]) >= 1 
                ||  Math.Abs(modeloAmbiente.ListaAcumDeslocamento[i][0, 1]) < 0.001)
                {
                    modeloAmbiente.ListaAcumDeslocamento[i][0, 1] = 0;
                }
                else
                {
                    modeloAmbiente.ListaAcumDeslocamento[i][0, 1] *= (float)eAmortecimento.amortecimento;
                }
            }
        }

        #endregion
    }
}
