using System.Collections.Generic;
using System.Drawing;
using AmbienteDeSimulacao.Ambiente;

namespace AmbienteDeSimulacao
{
    class Fisica : IFilhosAmbiente
    {
        /*
            A classe física vai ser executada em toda iteração.            
            Calcula o centro de massa de todos os corpos (construtor)        
            Calcula os pontos delimitantes de todos os corpos (construtor)
            Recebe as posições de todos os corpos do ambiente (a cada iteração)
            Detecta colisão entre os corpos (a cada iteração)
            Calcula a componente x e y do modulo das forças q cada corpo carrega (a cada iteração)
            Calcula o deslocamento linear nos eixos x e y dos corpos (a cada iteração)
        */

        public ModeloAmbiente modeloAmbiente;
        public List<Ambiente.FisicaAmbiente.IFilhosFisica> listaFilhosFisica;

        private Ambiente.FisicaAmbiente.Movimento.MRU mru;
        private Ambiente.FisicaAmbiente.Colisoes.ColisaoElastica colisaoElastica;
        private Ambiente.FisicaAmbiente.CentroDeMassa.CentroDeMassa centroDeMassa;
        private Ambiente.FisicaAmbiente.Movimento.ConsisteDeslocamento consisteDeslocamento;
        private Ambiente.FisicaAmbiente.Utils.LocalizacaoDosCorpos localizacaCorpos;
        private Ambiente.FisicaAmbiente.Utils.SuperficieDosCorpos superficieDosCorpos;
        private Ambiente.FisicaAmbiente.ResistenciaDeslocamento.ResistenciaDeslocamento resistenciaDeslocamento;


        #region Construtor

        public Fisica(float tempo, Rectangle limitesDoAmbiente, ModeloAmbiente modeloAmbiente)
        {
            this.modeloAmbiente = modeloAmbiente;

            // Iniciliza classe responsável pelo cálculo do centro de massa
            centroDeMassa = new Ambiente.FisicaAmbiente.CentroDeMassa.CentroDeMassa(modeloAmbiente);
            
            // Inicializa a classe que calcula os pontos que compem a superfície dos corpos
            superficieDosCorpos = new Ambiente.FisicaAmbiente.Utils.SuperficieDosCorpos(limitesDoAmbiente, modeloAmbiente);

            // Inicializa a classe que calcula a localização dos corpos na tela
            localizacaCorpos = new Ambiente.FisicaAmbiente.Utils.LocalizacaoDosCorpos(modeloAmbiente);

            // Inicializa a classe que controla o movimento retilíneo uniforme dos corpos
            mru = new Ambiente.FisicaAmbiente.Movimento.MRU(tempo, modeloAmbiente);

            // Inicializa a classe de colisao elástica
            colisaoElastica = new Ambiente.FisicaAmbiente.Colisoes.ColisaoElastica(modeloAmbiente);

            // Inicializa a classe que consiste o movimento
            consisteDeslocamento = new Ambiente.FisicaAmbiente.Movimento.ConsisteDeslocamento(modeloAmbiente);

            // Inicializa a classe responsável por gerar resistância ao deslocamento
            resistenciaDeslocamento = new Ambiente.FisicaAmbiente.ResistenciaDeslocamento.ResistenciaDeslocamento(modeloAmbiente);

            /************************ Padrão Observadores ***********************/
            listaFilhosFisica = new List<Ambiente.FisicaAmbiente.IFilhosFisica>();
            listaFilhosFisica.Add(centroDeMassa);
            listaFilhosFisica.Add(superficieDosCorpos);
            listaFilhosFisica.Add(mru);
            listaFilhosFisica.Add(colisaoElastica);
            listaFilhosFisica.Add(consisteDeslocamento);
            listaFilhosFisica.Add(resistenciaDeslocamento);
            listaFilhosFisica.Add(localizacaCorpos);
            listaFilhosFisica.Add(centroDeMassa);
            listaFilhosFisica.Add(superficieDosCorpos);
            /********************************************************************/
        }

        #endregion


        #region Atualiza Modelo

        public void Atualiza()
        {
            // Força todos os filhos da física atualizar o modelo
            foreach (Ambiente.FisicaAmbiente.IFilhosFisica f in listaFilhosFisica)
            {
                f.Atualiza();
            }            
        }

        #endregion
    }
}
