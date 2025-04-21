using System.Drawing;

namespace AmbienteDeSimulacao.Ambiente.FisicaAmbiente.Utils
{
    class InterseccaoSegmentoReta
    {
        #region Análise de intersecção de segmentos de retas   

        // Teste de intersecção entre segmentos de retas, baseado em relaçoes vetoriais
        // da geometria analítica
        // http://www.geeksforgeeks.org/check-if-two-given-line-segments-intersect/ 
        // Faster Line Segment Intersection
        // Franklin Antonio, Graphics Gems III, 1992, pp. 199-202
        // ISBN: 0-12-409673-5
        public bool FasterLineSegmentIntersection(Point ip1, Point ip2, Point ip3, Point ip4)
        {
            Vetor2 p1 = new Vetor2(ip1.X, ip1.Y);
            Vetor2 p2 = new Vetor2(ip2.X, ip2.Y);
            Vetor2 p3 = new Vetor2(ip3.X, ip3.Y);
            Vetor2 p4 = new Vetor2(ip4.X, ip4.Y);

            Vetor2 a = p2 - p1;
            Vetor2 b = p3 - p4;
            Vetor2 c = p1 - p3;

            float alphaNumerator = b.Y * c.X - b.X * c.Y;
            float betaNumerator = a.X * c.Y - a.Y * c.X;
            float denominator = a.Y * b.X - a.X * b.Y;

            if (denominator == 0)
            {
                return false;
            }
            else
            {
                if (denominator > 0)
                {
                    if (alphaNumerator < 0 || alphaNumerator > denominator)
                    {
                        return false;
                    }
                }
                else if (alphaNumerator > 0 || alphaNumerator < denominator)
                {
                    return false;
                }

                if (denominator > 0)
                {
                    if (betaNumerator < 0 || betaNumerator > denominator)
                    {
                        return false;
                    }
                }
                else if (betaNumerator > 0 || betaNumerator < denominator)
                {
                    return false;
                }
                return true;
            }
        }

        #endregion
    }
}
