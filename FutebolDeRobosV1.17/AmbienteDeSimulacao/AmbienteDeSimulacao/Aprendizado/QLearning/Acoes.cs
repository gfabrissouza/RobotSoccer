namespace AmbienteDeSimulacao.Aprendizado.QLearning
{
    class Acoes
    {
        public int acao { get; set; }
        public float recompensa { get; set; }

        #region Construtor

        public Acoes(int acao, float recompensa)
        {
            this.acao = acao;
            this.recompensa = recompensa;
        }

        #endregion
    }
}
