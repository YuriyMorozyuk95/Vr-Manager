using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using VrManager.Data.Abstract;

namespace VrManager.Service
{
    class ClientService
    {
        private IRemoteVideoCommand channel;

        public ClientService()
        {
            Uri address = new Uri("http://localhost:4000/IManagerComand");  // ADDRESS.   (A)

            // Указание, как обмениваться сообщениями.
            BasicHttpBinding binding = new BasicHttpBinding();         // BINDING.   (B)

            // Создание Конечной Точки.
            EndpointAddress endpoint = new EndpointAddress(address);

            // Создание фабрики каналов.
            ChannelFactory<IRemoteVideoCommand> factory = new ChannelFactory<IRemoteVideoCommand>(binding, endpoint);  // CONTRACT.  (C) 

            // Использование factory для создания канала (прокси).
            channel = factory.CreateChannel();
        }

        public void Pause()
        {
            channel.Pause();
        }
        public void Play()
         {
            try
            {
                channel.Play();
            }
            catch(Exception e)
            {

            }
        }
        public void Stop()
        {
            try
            {
                channel.Stop();
            }
            catch
            {

            }
        }

        public void ChangeSize()
        {
            try
            {
                channel.ChangeSize();
            }
            catch
            {

            }
        }

        public void ChangeToampostMode(bool mode)
        {
            try
            {
                channel.ChangeToampostMode(mode);
            }
            catch
            {

            }
        }

    }
}
