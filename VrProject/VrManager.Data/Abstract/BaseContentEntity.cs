using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrManager.Data.Abstract
{
    public class BaseContentEntity : BaseEntity
    {
        public string Name { get; set; }
        public string PathToBannerVideo { get; set; }
        public IconType IconType { get; set; }
        public string PathIcon { get; set; }
        public string ItemPath { get; set; }
        public DateTime? TimeOut { get; set; }
        public string FileMotion { get; set; }
    }
}
