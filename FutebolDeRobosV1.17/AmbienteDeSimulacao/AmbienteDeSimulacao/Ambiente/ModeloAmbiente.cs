using System.Drawing;
using System.Collections.Generic;

namespace AmbienteDeSimulacao.Ambiente
{
    public class ModeloAmbiente
    {
        /************************** Flags *****************************/
        public bool cmAtualizado;
        public bool lcAtualizado;
        public bool scAtualizado;
        public int idColisaoBola;
        public int idPosseBola;
        public int idDefesa;
        /**************************************************************/

        /********************* Lista de Corpos ************************/
        protected List<FisicaAmbiente.Corpos.Corpo> listaCorpos;
        /**************************************************************/

        #region Getters e Setters

        public List<FisicaAmbiente.Corpos.Corpo> ListaCorpos
        {
            get
            {
                return listaCorpos;
            }
            set
            {
                listaCorpos = value;
            }
        }

        #endregion

        /***************** Superfície dos Corpos **********************/
        protected List<Point> listaLimitesDoAmbiente;
        protected List<Point> listaLimitesGoleiraEsq;
        protected List<Point> listaLimitesGoleiraDir;
        protected List<Point> listaLimitesAreaEsq;
        protected List<Point> listaLimitesAreaDir;
        protected List<Point> listaLimitesMeioCampoEsq;
        protected List<Point> listaLimitesMeioCampoDir;
        protected List<Point> listaPontosSuperficieCircular;
        protected List<Point> listaPontosSuperficieRetangular;
        /**************************************************************/

        #region Getters e Setters

        public List<Point> ListaLimitesDoAmbiente
        {
            get
            {
                return listaLimitesDoAmbiente;
            }
            set
            {
                listaLimitesDoAmbiente = value;
            }
        }

        public List<Point> ListaLimitesGoleiraEsq
        {
            get
            {
                return listaLimitesGoleiraEsq;
            }
            set
            {
                listaLimitesGoleiraEsq = value;
            }
        }

        public List<Point> ListaLimitesGoleiraDir
        {
            get
            {
                return listaLimitesGoleiraDir;
            }
            set
            {
                listaLimitesGoleiraDir = value;
            }
        }

        public List<Point> ListaLimitesAreaEsq
        {
            get
            {
                return listaLimitesAreaEsq;
            }
            set
            {
                listaLimitesAreaEsq = value;
            }
        }

        public List<Point> ListaLimitesAreaDir
        {
            get
            {
                return listaLimitesAreaDir;
            }
            set
            {
                listaLimitesAreaDir = value;
            }
        }

        public List<Point> ListaLimitesMeioCampoEsq
        {
            get
            {
                return listaLimitesMeioCampoEsq;
            }
            set
            {
                listaLimitesMeioCampoEsq = value;
            }
        }

        public List<Point> ListaLimitesMeioCampoDir
        {
            get
            {
                return listaLimitesMeioCampoDir;
            }
            set
            {
                listaLimitesMeioCampoDir = value;
            }
        }

        public List<Point> ListaPontosSuperficieCircular
        {
            get
            {
                return listaPontosSuperficieCircular;
            }
            set
            {
                listaPontosSuperficieCircular = value;
            }
        }

        public List<Point> ListaPontosSuperficieRetangular
        {
            get
            {
                return listaPontosSuperficieRetangular;
            }
            set
            {
                listaPontosSuperficieRetangular = value;
            }
        }

        #endregion

        /************************** MRU *******************************/
        protected List<int> listaAmortecimento;
        protected List<float[,]> listaAceleracaoAtual;
        protected List<float[,]> listaAceleracaoAnterior;
        protected List<float[,]> listaAcumDeslocamento;
        protected List<float[,]> listaDeslocamentoAtual;
        protected List<float[,]> listaDeslocamentoAnterior;
        protected List<float[,]> listaForcaAtual;
        protected List<float[,]> listaForcaAnterior;
        /**************************************************************/

        #region Getters e Setters

        public List<int> ListaAmortecimento
        {
            get
            {
                return listaAmortecimento;
            }
            set
            {
                listaAmortecimento = value;
            }
        }

        public List<float[,]> ListaAceleracaoAtual
        {
            get
            {
                return listaAceleracaoAtual;
            }
            set
            {
                listaAceleracaoAtual = value;
            }
        }

        public List<float[,]> ListaAceleracaoAnterior
        {
            get
            {
                return listaAceleracaoAnterior;
            }
            set
            {
                listaAceleracaoAnterior = value;
            }
        }

        public List<float[,]> ListaAcumDeslocamento
        {
            get
            {
                return listaAcumDeslocamento;
            }
            set
            {
                listaAcumDeslocamento = value;
            }
        }

        public List<float[,]> ListaDeslocamentoAtual
        {
            get
            {
                return listaDeslocamentoAtual;
            }
            set
            {
                listaDeslocamentoAtual = value;
            }
        }

        public List<float[,]> ListaDeslocamentoAnterior
        {
            get
            {
                return listaDeslocamentoAnterior;
            }
            set
            {
                listaDeslocamentoAnterior = value;
            }
        }

        public List<float[,]> ListaForcaAtual
        {
            get
            {
                return listaForcaAtual;
            }
            set
            {
                listaForcaAtual = value;
            }
        }

        public List<float[,]> ListaForcaAnterior
        {
            get
            {
                return listaForcaAnterior;
            }
            set
            {
                listaForcaAnterior = value;
            }
        }

        #endregion

        /************************ Colisão *****************************/
        protected List<int> listaCorposColisao;
        protected List<Point> listaPontosColisaoAmbiente;
        protected List<Point> listaPontosColisaoCorpos;
        /**************************************************************/

        #region Getters e Setters

        public List<int> ListaCorposColisao
        {
            get
            {
                return listaCorposColisao;

            }
            set
            {
                listaCorposColisao = value;
            }
        }

        public List<Point> ListaPontosColisaoAmbiente
        {
            get
            {
                return listaPontosColisaoAmbiente;

            }
            set
            {
                listaPontosColisaoAmbiente = value;
            }
        }

        public List<Point> ListaPontosColisaoCorpos
        {
            get
            {
                return listaPontosColisaoCorpos;
            }
            set
            {
                listaPontosColisaoCorpos = value;
            }
        }

        #endregion
    }
}
