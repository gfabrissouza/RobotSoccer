using System.Drawing;

namespace AmbienteDeSimulacao.Ambiente.FisicaAmbiente.CentroDeMassa
{
    class CentroDeMassa : IFilhosFisica
    {
        private ModeloAmbiente modelAmbiente;


        #region Construtor

        public CentroDeMassa(ModeloAmbiente modelAmbiente)
        {
            this.modelAmbiente = modelAmbiente;
        }

        #endregion


        public void Atualiza()
        {
            if (modelAmbiente.cmAtualizado)
            {
                return;
            }

            CalculaCentroDeMassa();
            modelAmbiente.cmAtualizado = true;
        }


        #region Calcula o centro de massa dos corpos

        public void CalculaCentroDeMassa()
        {
            for (int i = 0; i < modelAmbiente.ListaCorpos.Count; i++)
            {
                if (i > 0)
                {
                    modelAmbiente.ListaCorpos[i].CentroDeMassa = new Point(modelAmbiente.ListaCorpos[i].Localizacao.X + 24,
                                                                           modelAmbiente.ListaCorpos[i].Localizacao.Y + 27);
                }
                else
                {
                    modelAmbiente.ListaCorpos[i].CentroDeMassa = new Point(modelAmbiente.ListaCorpos[i].Localizacao.X + 10,
                                                                           modelAmbiente.ListaCorpos[i].Localizacao.Y + 10);
                }
            }
        }

        #endregion
    }
}
