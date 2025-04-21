using System.Collections.Generic;
using System.Drawing;

namespace AmbienteDeSimulacao.Ambiente.FisicaAmbiente.Corpos
{
    public class Corpo
    {
        private readonly int id;
        private readonly float massaCorpo;
        private readonly int[,] dimencoes;
        private readonly string identificacao;

        private int sprite;
        
        private Point localizacao;
        private Point centroDeMassa;

        private List<Point> listaPontosSuperficieCorpo;

        private List<double> listaIncrementoAngularAgente;


        #region Construtor

        public Corpo(int id, string identificacao, int sprite, float massaCorpo, int[,] dimencoes, Point localizacao)
        {
            this.id = id;
            this.identificacao = identificacao;
            this.sprite = sprite;
            this.massaCorpo = massaCorpo;
            this.dimencoes = dimencoes;
            this.localizacao = localizacao;
        }

        #endregion


        #region Getters e Setters

        public int GetId
        {
            get
            {
                return id;
            }
        }

        public string Identificacao
        {
            get
            {
                return identificacao;
            }
        }

        public float MassaCorpo
        {
            get
            {
                return massaCorpo;
            }
        }

        public int Sprite
        {
            get
            {
                return sprite;
            }
            set
            {
                sprite = value;
            }
        }

        public int[,] Dimencao
        {
            get
            {
                return dimencoes;
            }
        }

        public Point Localizacao
        {
            get
            {
                return localizacao;
            }
            set
            {
                localizacao = value;
            }
        }

        public Point CentroDeMassa
        {
            get
            {
                return centroDeMassa;
            }
            set
            {
                centroDeMassa = value;
            }
        }

        public List<Point> ListaPontosSuperficie
        {
            get
            {
                return listaPontosSuperficieCorpo;
            }
            set
            {
                listaPontosSuperficieCorpo = value;
            }
        }

        public List<double> ListaIncrementoAngularAgente
        {
            get
            {
                return listaIncrementoAngularAgente;
            }
            set
            {
                listaIncrementoAngularAgente = value;
            }
        }

        #endregion
    }
}
