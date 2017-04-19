using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrManager.Data.Abstract;

namespace VrManager.Data.Entity
{
    public class ModelUser : BaseEntity
    {
        public ModelUser()
        {
            Observers = new List<ModelObserve>();
        }

        public string Login { get; set; }
        public string Password { get; set; }

        public virtual ICollection<ModelObserve> Observers { get; set; }
    }
}
