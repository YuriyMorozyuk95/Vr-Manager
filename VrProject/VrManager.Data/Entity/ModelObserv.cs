using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrManager.Data.Abstract;

namespace VrManager.Data.Entity
{
    public class ModelObserve
    {
        [Key]
        public int ObserveId { get; set; }
        public string Name { get; set; }
        public TypeItem? TypeItem { get; set; }
        public DateTime Duration { get; set; }
        public PC PC { get; set; } 
        /// <summary>
        /// First execute
        /// </summary>
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeStop { get; set; }
        /// <summary>
        /// Last Execute
        /// </summary>
        public DateTime? TimePause { get; set; }
        public int? PressStartCouner { get; set; }
        public int? PressPauseCouner { get; set; }
        public Halted? Halted { get; set; }

        public virtual ModelUser User { get; set; }
    }
}
