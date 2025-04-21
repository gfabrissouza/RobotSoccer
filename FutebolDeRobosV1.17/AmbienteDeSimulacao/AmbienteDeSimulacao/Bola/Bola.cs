using System.Drawing;

namespace AmbienteDeSimulacao
{
    class Bola
    {   
        private Ambiente.FisicaAmbiente.Corpos.Corpo meuCorpo;


        #region Construtor

        public Bola(int id, float massa, int[,] dimencao, Point localizacao)
        {
            meuCorpo = new Ambiente.FisicaAmbiente.Corpos.Corpo(id, "Bola", 0, massa, dimencao, localizacao);
        }

        #endregion


        #region Getters e Setters

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

        #endregion
    }
}
