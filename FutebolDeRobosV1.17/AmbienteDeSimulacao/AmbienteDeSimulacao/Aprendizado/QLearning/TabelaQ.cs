using System.Collections.Generic;

namespace AmbienteDeSimulacao.Aprendizado.QLearning
{
    class TabelaQ
    {
        public List<Estado> estados { get; set; }

        public TabelaQ()
        {
            estados = new List<Estado>();
        }
    }
}
