using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrManager.Data.Entity;

namespace VrManager.Data.Abstract
{
    public interface IRepository
    {

        IEnumerable<ModelGame> Games { get; }

        IEnumerable<ModelVideo> Videos { get; }

        IEnumerable<ModelSetting> Setting { get; }

        IEnumerable<ModelUser> Users { get; }

       // IEnumerable<ModelStatistic> Statistics { get; }

        IEnumerable<ModelObserve> Observes { get; }
    }
}
