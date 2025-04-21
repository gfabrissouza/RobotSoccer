using System.Collections.Generic;

namespace AmbienteDeSimulacao.Aprendizado.QLearning
{
    class Estado 
    {
        public int idx { get; set; }
        public List<Acoes> acoes { get; set; }

        #region Construtor

        public Estado()
        {
        }

        public Estado(int idx, List<Acoes> acoes)
        {
            this.idx = idx;
            this.acoes = acoes;
        }

        #endregion
    }
}
