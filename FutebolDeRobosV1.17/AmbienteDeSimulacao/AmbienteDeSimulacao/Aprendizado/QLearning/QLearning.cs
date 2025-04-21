using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AmbienteDeSimulacao.Aprendizado.QLearning
{
    class QLearning
    {
        // Fator Gamma de desconto, determina a importância 
        // das recompensas futuras
        private const float GAMMA = 0.5f;
        private int EPSILON = 50; // 50

        // Nome do arquivo (salva tabela Q)
        private string nomeArquivo;

        // Ações
        private int acaoEscolhida;

        // Estados 
        private Estado estadoAnterior { get; set; }
        private Estado estadoAtual { get; set; }

        // Tabela Q
        private TabelaQ tabelaQ{ get; set; }
        private TabelaQ tabelaQGoleiro { get; set; }
        private TabelaQ tabelaQAtacante { get; set; }

        // Tabela de recompensas
        private List<float[]> tabelaRecompensa = new List<float[]>();
        private List<float[]> tabelaRecompensaGoleiro = new List<float[]>();
        private List<float[]> tabelaRecompensaAtacante = new List<float[]>();

        // Objeto random
        private Random random = new Random();

        // Log
        private int timeExec;
        private int timeToday = DateTime.Today.Millisecond;
        private string directory;
        private string filename;
        private string path;
        private StringBuilder log = new StringBuilder();

        // Sinalizadores auxiliares - Intensidade da emoção
        //public bool flagGol = false;
        //public bool flagDefesa = false;
        public bool flagGolTimeAzul = false;
        public bool flagGolTimeVermelho = false;
        public int idPosseBola = 0;
        public int idDefesa = 0;


        #region Construtor

        public QLearning(int idx)
        {
            estadoAtual = new Estado();
            estadoAnterior = new Estado();

            // Incializa a tabela de recompensas e a tabela Q
            Inicializa(idx);
            CarregarTabelaQ();

            log.AppendLine(string.Format("Tempo Exec.(Milis);Estado;Acao;Recompensa;ID Defesa;ID Posse Bola;Gol Azul; Gol Vermelho"));
        }

        #endregion


        #region 1 - Inicializa Tabela de Remcompensas e Tabela Q

        private void Inicializa(int idx)
        {
            if (idx == 1)
            {
                nomeArquivo = "goleiro";

                tabelaQGoleiro = new TabelaQ();

                MontaTabelaRemcompensasGoleiro();
                MontaTabelaQGoleiro();

                tabelaRecompensa = tabelaRecompensaGoleiro;
                tabelaQ = tabelaQGoleiro;

                estadoAtual.acoes = tabelaQ.estados[0].acoes;
                estadoAnterior.acoes = tabelaQ.estados[0].acoes;
            }
            else
            {
                nomeArquivo = "atacante";

                tabelaQAtacante = new TabelaQ();

                MontaTabelaRemcompensasAtacante();
                MontaTabelaQAtacante();

                tabelaRecompensa = tabelaRecompensaAtacante;
                tabelaQ = tabelaQAtacante;

                estadoAtual.acoes = tabelaQ.estados[0].acoes;
                estadoAnterior.acoes = tabelaQ.estados[0].acoes;
            }            
        }

        private List<Acoes> ListaAcoesGoleiro()
        {
            List<Acoes> l = new List<Acoes>();
            l.Add(new Acoes((int)eAcaoGoleiro.Defender, 0));
            l.Add(new Acoes((int)eAcaoGoleiro.Posicionar, 0));
            l.Add(new Acoes((int)eAcaoGoleiro.Reposicao, 0));

            return l;
        }

        private List<Acoes> ListaAcoesAtacante()
        {
            List<Acoes> l = new List<Acoes>();
            l.Add(new Acoes((int)eAcaoAtacante.Chutar, 0));
            l.Add(new Acoes((int)eAcaoAtacante.Driblar, 0));
            l.Add(new Acoes((int)eAcaoAtacante.Dominar, 0));
            l.Add(new Acoes((int)eAcaoAtacante.Desarmar, 0));
            l.Add(new Acoes((int)eAcaoAtacante.Fintar, 0));

            return l;
        }

        private void MontaTabelaRemcompensasGoleiro()
        {
            // (Estados) registro da lista, (Conteúdo) recompensa pelas ações
            tabelaRecompensaGoleiro.Add(new float[3] { -10, 20, -10 });
            tabelaRecompensaGoleiro.Add(new float[3] { 20, -10, 5 });
            tabelaRecompensaGoleiro.Add(new float[3] { 10, 0, 20 });

        }

        private void MontaTabelaRemcompensasAtacante()
        {
            // (Estados) registro da lista, (Conteúdo) recompensa pelas ações
            tabelaRecompensaAtacante.Add(new float[5] { -10, -10, -10, 20, 0 });
            tabelaRecompensaAtacante.Add(new float[5] { 20, 0, -10, -10, -10 });
            tabelaRecompensaAtacante.Add(new float[5] { 0, 20, 0, -10, 0 });
            tabelaRecompensaAtacante.Add(new float[5] { 5, 0, 20, -10, 0 });
            tabelaRecompensaAtacante.Add(new float[5] { -10, -10, -10, -10, 20 });
        }

        private void MontaTabelaQGoleiro(/*Problema problema*/)
        {
            // Popula tabela Q (Estado/Ação/Recompensa)

            // Tabela Q do goleiro
            tabelaQGoleiro.estados.Add(new Estado(0, ListaAcoesGoleiro()));
            tabelaQGoleiro.estados.Add(new Estado(1, ListaAcoesGoleiro()));
            tabelaQGoleiro.estados.Add(new Estado(2, ListaAcoesGoleiro()));
        }

        private void MontaTabelaQAtacante(/*Problema problema*/)
        {
            // Popula tabela Q (Estado/Ação/Recompensa)
        
            // Tabela Q do atacante
            tabelaQAtacante.estados.Add(new Estado(0, ListaAcoesAtacante()));
            tabelaQAtacante.estados.Add(new Estado(1, ListaAcoesAtacante()));
            tabelaQAtacante.estados.Add(new Estado(2, ListaAcoesAtacante()));
            tabelaQAtacante.estados.Add(new Estado(3, ListaAcoesAtacante()));
            tabelaQAtacante.estados.Add(new Estado(4, ListaAcoesAtacante()));
        }

        #endregion


        #region 2 - Seta estado atual

        public int SetEstadoAtual
        {
            set
            {
                estadoAtual = tabelaQ.estados[value];
            }
        }

        #endregion


        #region 3 - Escolher uma ação

        public int EscolheProximaAcao(Estado estado)
        {
            if ((random.Generate(0, 101) < EPSILON)
            || TodasAcoesMesmaRecompensa(estado.acoes))
            {
                acaoEscolhida = EscolheAcaoRandomica(estado);
                return acaoEscolhida;
            }
            else
            {
                acaoEscolhida = EscolheAcaoComMaiorRecompensa(estado).acao;
                return acaoEscolhida;
            }
        }

        private bool TodasAcoesMesmaRecompensa(List<Acoes> acoes)
        {
            if (acoes.Select(m => m.recompensa).Distinct().Count() == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Escolhe uma ação aleatória
        private int EscolheAcaoRandomica(Estado estado)
        {
            return random.Generate(0, estado.acoes.Count);
        }

        // Escolhe a ação com a maior recompensa
        private Acoes EscolheAcaoComMaiorRecompensa(Estado estado)
        {
            return estado.acoes.OrderBy(a => a.recompensa).Last();
        }

        #endregion


        #region 4 - Executa a ação escolhida

        /* retorna para o agente a ação escolhida e executa a mesma */
        public int GetAcaoEscolhida
        {
            get
            {
                return EscolheProximaAcao(estadoAtual);
            }
        }

        #endregion


        #region 5 - Observa os valores da recompensa e do estado anterior

        // recompensaAcaoExec => Recompensa pelo movimento executado
        // maiorRecompensaAcoesNovoEstado => Melhor recompensa para as ações do próximo estado
        private float CalculaRecompensa(int acaoEscolhida, float maiorRecompensaAcoesNovoEstado)
        {
            estadoAnterior = estadoAtual;

            return tabelaRecompensa[estadoAnterior.idx][acaoEscolhida] + (GAMMA * maiorRecompensaAcoesNovoEstado);
        }

        #endregion


        #region 6 - Cálculo do Q-Learning

        // Q(S,A) = recompensa + Gamma*max(Q(S+1,A))
        public void AtualizaTabelaQ(/*int acaoEscolhida*/)
        {
            // Busca as ações possíveis no novo estado
            List<Acoes> acoesPossiveis = tabelaQ.estados[estadoAtual.idx].acoes;

            // Busca a ação com a maior recompensa
            float acaoComMaiorRecompensa = EscolheAcaoComMaiorRecompensa(tabelaQ.estados[estadoAtual.idx]).recompensa;

            // Atualiza tabela Q
            var recompensa = CalculaRecompensa(acaoEscolhida, acaoComMaiorRecompensa);

            //if (tabelaQ.estados[estadoAnterior.idx].acoes[acaoEscolhida].recompensa < recompensa)
            //{
                tabelaQ.estados[estadoAnterior.idx].acoes[acaoEscolhida].recompensa = recompensa;
            //}
            //tabelaQ.estados[estadoAnterior.idx].acoes[acaoEscolhida].recompensa =
                //CalculaRecompensa(acaoEscolhida, acaoComMaiorRecompensa);

            /* Intensidade da emoção */
        }

        #endregion


        #region Ajuste Epsilon a cada gol

        public void AjustaEpsilon()
        {
            if (EPSILON > 1)
            {
                //EPSILON -= 1; 
            }
        }

        #endregion


        #region Imprime tabela Q no console

        public void ImprimeTabelaQ()
        {         
            Console.Write("TabelaQ\n");
            Console.WriteLine("Est. Ações");

            string acoes = "";

            foreach (Acoes a in tabelaQ.estados[0].acoes)
            {
                acoes += a.acao.ToString() + "   ";
            }

            Console.WriteLine("     {0}", acoes);

            for (int i = 0; i < tabelaQ.estados.Count; i++)
            {
                string recompensas = "";

                foreach (Acoes a in tabelaQ.estados[i].acoes)
                {
                    recompensas += a.recompensa.ToString() + " ";
                }
                Console.WriteLine(" {0}   {1}", tabelaQ.estados[i].idx.ToString(), recompensas);
            }                    
        }

        #endregion


        #region Log CSV

        public void Log()
        {
            var estado = estadoAtual.idx + 1;
            var acao = acaoEscolhida + 1;
            var recompensa = tabelaQ.estados[estadoAtual.idx].acoes[acaoEscolhida].recompensa;

            timeExec += (DateTime.Now.Millisecond - timeToday);

            var newLine = string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", timeExec, estado, acao, recompensa, idDefesa, idPosseBola, flagGolTimeAzul, flagGolTimeVermelho);
            log.AppendLine(newLine);
        }

        public void SalvaLog()
        {
            int count = 1;

            directory = Directory.GetCurrentDirectory();
            filename = string.Format("log_{0}", nomeArquivo);
            path = Path.Combine(directory, filename + ".csv");

            while (File.Exists(path))
            {
                string tempFileName = string.Format("{0}({1})", filename, count++);
                path = Path.Combine(directory, tempFileName + ".csv");
            }

            // Log da ultima iteração com o ambiente
            Log();

            // Escreve no arquivo
            File.WriteAllText(path, log.ToString());
        }

        #endregion

/*
        int count = 1;

        //string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
        string extension = Path.GetExtension(fullPath);
        string path = Path.GetDirectoryName(fullPath);
        string newFullPath = fullPath;

        while(File.Exists(newFullPath)) 
        {
            string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(path, tempFileName + extension);
        }

    */




        #region Salva tabela Q

        public void SalvarTabelaQ()
        {
            string directory = Directory.GetCurrentDirectory();
            string filename = string.Format("{0}.txt", nomeArquivo);
            string path = Path.Combine(directory, filename);

            if (!File.Exists(path))
            {
                using (StreamWriter str = File.CreateText(path))
                {
                    ConteudoArquivo(str);
                }
            }
            else if (File.Exists(path))
            {
                using (var str = new StreamWriter(path))
                {
                    ConteudoArquivo(str);
                }
            }

            path = Path.Combine(directory, "epsilon.txt");

            if (!File.Exists(path))
            {
                using (StreamWriter str = File.CreateText(path))
                {
                    str.WriteLine("{0}", EPSILON);
                    str.Flush();
                }
            }
            else if (File.Exists(path))
            {
                using (var str = new StreamWriter(path))
                {
                    str.WriteLine("{0}", EPSILON);
                    str.Flush();
                }
            }
        }

        private void ConteudoArquivo(StreamWriter str)
        {
            int estados = tabelaQ.estados.Count;

            for (int i = 0; i < estados; i++)
            {
                string recompensas = "";

                for (int j = 0; j < tabelaQ.estados[i].acoes.Count; j++)
                {
                    if (j < (tabelaQ.estados[i].acoes.Count - 1))
                    {
                        recompensas += tabelaQ.estados[i].acoes[j].recompensa.ToString() + ";";
                    }
                    else
                    {
                        recompensas += tabelaQ.estados[i].acoes[j].recompensa.ToString();
                    }
                }

                str.WriteLine("{0}", recompensas);
            }
            str.Flush();
        }

        #endregion


        #region Carrega tabela Q

        public void CarregarTabelaQ()
        {
            string directory = Directory.GetCurrentDirectory();
            string filename = string.Format("{0}.txt", nomeArquivo);
            string path = Path.Combine(directory, filename);

            if (!File.Exists(path))
            {
                return;
            }

            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                int e = 0;
                string line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    int a = 0;

                    foreach (string r in line.Split(';'))
                    {
                        tabelaQ.estados[e].acoes[a].recompensa = Convert.ToSingle(r);
                        a += 1;
                    }
                    e += 1;
                }
            }


            path = Path.Combine(directory, "epsilon.txt");

            if (!File.Exists(path))
            {
                return;
            }

            var fileStream2 = new FileStream(path, FileMode.Open, FileAccess.Read);

            using (var streamReader = new StreamReader(fileStream2, Encoding.UTF8))
            {
                EPSILON = Convert.ToInt32(streamReader.ReadLine());
            }
        }

        #endregion
    }
}
