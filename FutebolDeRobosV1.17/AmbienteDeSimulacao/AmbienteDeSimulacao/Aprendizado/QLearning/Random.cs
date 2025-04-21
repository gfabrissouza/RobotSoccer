namespace AmbienteDeSimulacao.Aprendizado.QLearning
{
    class Random
    {
        private System.Random random = new System.Random();

        public int Generate(int de, int ate)
        {
            return random.Next(de, ate);
        }
    }
}
