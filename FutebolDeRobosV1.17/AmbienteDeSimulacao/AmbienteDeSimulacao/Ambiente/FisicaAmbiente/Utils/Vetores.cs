using System;

namespace AmbienteDeSimulacao.Ambiente.FisicaAmbiente.Utils
{
    #region Estrutura para cálculos básicos com vetores

    // Estrutura responsável por facilitar a manipulação de vetores no plano bidimencional
    public struct Vetor2
    {
        public float X;
        public float Y;

        public Vetor2(float x, float y)
        {
            X = x;
            Y = y;
        }

        // Construtor com conversão de um array float para a estrutura vector2
        public Vetor2(float[,] d)
        {
            X = Convert.ToSingle(d[0, 0]);
            Y = Convert.ToSingle(d[0, 1]);
        }

        public static Vetor2 operator +(Vetor2 v1, Vetor2 v2)
        {
            return new Vetor2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vetor2 operator -(Vetor2 v1, Vetor2 v2)
        {
            return new Vetor2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vetor2 operator *(Vetor2 v1, float m)
        {
            return new Vetor2(v1.X * m, v1.Y * m);
        }

        public static float operator *(Vetor2 v1, Vetor2 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public static Vetor2 operator /(Vetor2 v1, float m)
        {
            return new Vetor2(v1.X / m, v1.Y / m);
        }

        public static float Distance(Vetor2 v1, Vetor2 v2)
        {
            return (float)Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + Math.Pow(v1.Y - v2.Y, 2));
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }
    }

    #endregion
}
