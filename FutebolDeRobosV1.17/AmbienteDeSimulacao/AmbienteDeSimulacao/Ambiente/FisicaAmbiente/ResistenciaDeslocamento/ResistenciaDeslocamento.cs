namespace AmbienteDeSimulacao.Ambiente.FisicaAmbiente.ResistenciaDeslocamento
{
    class ResistenciaDeslocamento : IFilhosFisica
    {
        ModeloAmbiente modeloAmbiente;

        private const float coefResistBola = (float)0.0030;
        private const float coefResistAgente  = (float)0.040;

        private float deslocQuadX;
        private float deslocQuadY;
        private float resistX;
        private float resistY;


        #region Construtor

        public ResistenciaDeslocamento(ModeloAmbiente modeloAmbiente)
        {
            this.modeloAmbiente = modeloAmbiente;
        }

        #endregion


        public void Atualiza()
        {
            CalculaResistenciaDeslocamento();
        }

        #region Resistência ao Deslocamento

        private void CalculaResistenciaDeslocamento()
        {
            for (int i = 0; i < modeloAmbiente.ListaCorpos.Count; i++)
            {
                // Desconto quadrático de resistência ao movimento
                deslocQuadX = modeloAmbiente.ListaDeslocamentoAtual[i][0, 0] * modeloAmbiente.ListaDeslocamentoAtual[i][0, 0];
                deslocQuadY = modeloAmbiente.ListaDeslocamentoAtual[i][0, 1] * modeloAmbiente.ListaDeslocamentoAtual[i][0, 1];

                if (i > 0)
                {
                    resistX = coefResistAgente * deslocQuadX;
                    resistY = coefResistAgente * deslocQuadY;
                }
                else
                {
                    resistX = coefResistBola * deslocQuadX;
                    resistY = coefResistBola * deslocQuadY;
                }

                if (modeloAmbiente.ListaDeslocamentoAtual[i][0, 0] > 0)
                {
                    modeloAmbiente.ListaDeslocamentoAtual[i][0, 0] = modeloAmbiente.ListaDeslocamentoAtual[i][0, 0] - resistX;
                }
                else
                {
                    modeloAmbiente.ListaDeslocamentoAtual[i][0, 0] = modeloAmbiente.ListaDeslocamentoAtual[i][0, 0] + resistX;
                }

                if (modeloAmbiente.ListaDeslocamentoAtual[i][0, 1] > 0)
                {
                    modeloAmbiente.ListaDeslocamentoAtual[i][0, 1] = modeloAmbiente.ListaDeslocamentoAtual[i][0, 1] - resistY;
                }
                else
                {
                    modeloAmbiente.ListaDeslocamentoAtual[i][0, 1] = modeloAmbiente.ListaDeslocamentoAtual[i][0, 1] + resistY;
                }
            }
        }

        #endregion
    }
}
