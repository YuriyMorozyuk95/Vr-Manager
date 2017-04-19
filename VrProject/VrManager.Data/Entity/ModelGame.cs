using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrManager.Data.Abstract;
using System.Windows.Input;

namespace VrManager.Data.Entity
{
    public class ModelGame: BaseContentEntity
    {     
        /// <summary>
        /// •	Параметры для запуска игры 
        /// </summary>
        public string StartUpParams { get; set; }
        /// <summary>
        /// Кординати маусклила X
        /// </summary>
        public double? MouseClickCordX { get; set; }
        /// <summary>
        /// Кординати маусклила Y
        /// </summary>
        public double? MouseClickCordY { get; set; }
        /// <summary>
        /// •	Имя процесса
        /// </summary>
        public string NameProcess { get; set; }
      
        public int? StartTime { get; set; }
        /// <summary>
        /// •	Время в сек. нажатия клавиши Shift. Отсчитывается от момента старта движений, по умолчанию задать 3 сек.;
        /// </summary>
        public int? ShiftPressTime { get; set; }

        /// <summary>
        /// •	Кнопка, которую необходимо нажать, чтобы запустилась игра (Enter или Space);
        /// </summary>
        public Key? RunKey { get; set; }
        /// <summary>
        /// •	Нажатие допольнительных клавиш через секунду после старта игры (указание клавиши, в некоторых играх ими настраивается вид).
        /// </summary>
        public Key? AdditionalKey { get; set; }

        public TypeStartFocus TypeStartFocus { get; set; }




    }
}
