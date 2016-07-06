using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Project_Space___New_Live.modules.Dispatchers
{
    /// <summary>
    /// Модуль сохранения статистики
    /// </summary>
    class DataSaver
    {
        /// <summary>
        /// Поток вывода в файл
        /// </summary>
        private StreamWriter writer;

        /// <summary>
        /// Текущее общее количество побед
        /// </summary>
        private int currentWinCount = 0;

        /// <summary>
        /// Текущее общее количество "смертей" объекта
        /// </summary>
        private int currentDeathCount = 0;

        /// <summary>
        /// Конструктор модуля сохранения статистики
        /// </summary>
        /// <param name="filename"></param>
        public DataSaver(String filename)
        {
            this.writer = new StreamWriter(filename);
            writer.WriteLine("WINS | DEATHS | Middle count taken decitions to death");
        }

        /// <summary>
        /// Запись данных в файл
        /// </summary>
        /// <param name="winCount">Общее количество побед</param>
        /// <param name="deathCount">Общее количество смертей</param>
        /// <param name="decisionCount">Количество принятых решений</param>
        public void WriteData(int winCount, int deathCount, int decisionCount)
        {
            writer.WriteLine((winCount - this.currentWinCount).ToString() + "|" + (deathCount - this.currentDeathCount).ToString() + "|" + (decisionCount / (1 + deathCount - this.currentDeathCount)));
            writer.Flush();//принудительная запись в поток
            this.currentWinCount = winCount;
            this.currentDeathCount = deathCount;
        }

    }
}
