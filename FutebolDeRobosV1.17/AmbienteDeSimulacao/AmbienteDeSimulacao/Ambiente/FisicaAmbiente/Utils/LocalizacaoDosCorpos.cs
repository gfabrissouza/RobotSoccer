using System;
using System.Drawing;

namespace AmbienteDeSimulacao.Ambiente.FisicaAmbiente.Utils
{
    class LocalizacaoDosCorpos : IFilhosFisica
    {
        private ModeloAmbiente modelAmbiente;


        #region Construtor

        public LocalizacaoDosCorpos(ModeloAmbiente modelAmbiente)
        {
            this.modelAmbiente = modelAmbiente;
        }

        #endregion


        public void Atualiza()
        {
            if (modelAmbiente.lcAtualizado)
            {
                return;
            }

            CalculaNovaLocalizacaoCorpos();
            modelAmbiente.lcAtualizado = true;
        }


        #region Getters e Setters

       /* public List<float[,]> SetListaDeslocamentoCorpos
        {
            set
            {
                listaDeslocamentoCorpos = value;
            }
        }
        
        public List<Corpos.Corpo> GetListaCorpos
        {
            get
            {
                return listaCorpos;
            }
        }*/

        #endregion


        #region Localização dos Corpos

        public void CalculaNovaLocalizacaoCorpos()
        {
            // Calcula a nova localização da bola através do deslocamento e
            // da posição atual do bola
            modelAmbiente.ListaCorpos[0].Localizacao = new Point(Convert.ToInt32(modelAmbiente.ListaDeslocamentoAtual[0][0, 0]) + modelAmbiente.ListaCorpos[0].Localizacao.X, 
                                                                 Convert.ToInt32(modelAmbiente.ListaDeslocamentoAtual[0][0, 1]) + modelAmbiente.ListaCorpos[0].Localizacao.Y);

            for (int i = 1; i < modelAmbiente.ListaCorpos.Count; i++)
            {
                // Calcula a nova localização do agente através do deslocamento e
                // da posição atual do agente
                modelAmbiente.ListaCorpos[i].Localizacao = new Point(Convert.ToInt32(modelAmbiente.ListaDeslocamentoAtual[i][0, 0]) + modelAmbiente.ListaCorpos[i].Localizacao.X,
                                                                     Convert.ToInt32(modelAmbiente.ListaDeslocamentoAtual[i][0, 1]) + modelAmbiente.ListaCorpos[i].Localizacao.Y);
            }
        }

        #endregion
    }
}
