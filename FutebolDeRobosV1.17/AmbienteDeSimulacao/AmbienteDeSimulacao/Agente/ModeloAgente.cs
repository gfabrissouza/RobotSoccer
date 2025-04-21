using System.Drawing;

namespace AmbienteDeSimulacao.Agente
{
    public class ModeloAgente
    {
        /**************************************************************************/
        public bool conduzir;
        public bool desviar;
        public bool movendoPara;

        public Point objetivoTemporario; // objetivo ao realizar um ação temporária
        public Point objetivoQLearning;  // objetivo definido pelo QLearning
        /**************************************************************************/
    }
}