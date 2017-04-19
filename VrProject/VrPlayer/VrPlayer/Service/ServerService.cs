using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using VrManager.Data.Abstract;

namespace VrPlayer.Service
{
    public class ServerService
    {
        private ServiceHost host;

        public ServerService()
        {
            Uri address = new Uri("http://localhost:4000/IManagerComand"); // ADDRESS.    (A)

            // Указание привязки, как обмениваться сообщениями.
            BasicHttpBinding binding = new BasicHttpBinding();        // BINDING.    (B)

            // Указание контракта.
            Type contract = typeof(IRemoteVideoCommand);                        // CONTRACT.   (C) 


            // Создание провайдера Хостинга с указанием Сервиса.
            host = new ServiceHost(typeof(Service));
            // Добавление "Конечной Точки".
            host.AddServiceEndpoint(contract, binding, address);

            // Начало ожидания прихода сообщений.
           
             host.Open();


            // Завершение ожидания прихода сообщений.

        }

        public void EndHosting()
        {
            host.Close();
        }
    }
}
