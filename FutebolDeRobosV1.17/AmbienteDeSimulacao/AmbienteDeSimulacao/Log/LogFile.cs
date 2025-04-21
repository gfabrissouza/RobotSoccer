using System;
using System.IO;

namespace AmbienteDeSimulacao.Log
{
    class LogFile
    {
        StreamWriter sw;
        private string formatoLog;
        private string nomeLog;

        #region Construtor

        public LogFile(/*string diretorio*/)
        {
            formatoLog = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

            // Nome do arquivo de log
            nomeLog = "LogFutebolRobos-"
                    + DateTime.Now.Year.ToString() 
                    + DateTime.Now.Month.ToString()
                    + DateTime.Now.Day.ToString()
                    + ".txt";

            //sw = new StreamWriter(diretorio + nomeLog, true);
        }

        #endregion

        public void CriarArquivoLog(string diretorio)
        {
            if (!Directory.Exists(diretorio))
            {
                Console.Write("Diretório inexistente");
            }

            //sw = new StreamWriter(diretorio + nomeLog, true);
        }

        public void Log(string msgLog)
        {
            if (sw == null)
            {
                Console.Write("Arquivo de log ainda não existe!");
                return;
            }
            
            sw.WriteLine(msgLog);
            sw.Flush();
            sw.Close();
        }
    }
}
