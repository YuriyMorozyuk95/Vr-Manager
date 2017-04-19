using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VrManager.Data.Abstract
{
    [ServiceContract]
    public interface IRemoteVideoCommand
    {
        [OperationContract]
        void Pause();
        [OperationContract]
        void Play();
        [OperationContract]
        void Stop();
        [OperationContract]
        void ChangeSize();
        [OperationContract]
        void ChangeToampostMode(bool mode);
    }
}
