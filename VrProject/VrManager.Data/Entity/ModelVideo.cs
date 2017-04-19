using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrManager.Data.Abstract;

namespace VrManager.Data.Entity
{
    public class ModelVideo: BaseContentEntity
    {
        public ModelVideo()
        {

        }
       /// <summary>
       /// тип видео (5д или 360)
       /// </summary>
        public TypeItem? TypeItem { get; set; } 
        /// <summary>
        /// •	Номер монитора; 
        /// </summary>
        public int? MonitorNumber { get; set; }
        /// <summary>
        /// •	Путь к файлу настроек ВР плеера;
        /// </summary>
        public string VrSettingPath { get; set; }

        //public TimeSpan EndOfVide
    }
}
