using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ComPortPackages.Core;
using ComPortPackages.Core.Model;
using ComPortPackages.Core.RS232;
using ComPortPackages.Core.Serialization;
using ComPortPackages.Core.Serialization.Abstract;
using ComPortPackages.Core.Serialization.Impl;
using log4net;
using log4net.Config;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
//using Vr.License;

namespace ComPortPackages.Console
{
    public class ComPrortSender
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ComPrortSender));

        //public static void Send(TextBox terminal, Application current)
        //{
        //    current.Dispatcher
        //}

        private static readonly ISerializationService _serializationService = new CryptoSerializationService();
        private const string PACKAGE_FILE = @"C:\Users\Admin\Source\Workspaces\HelpsProjectFiles\VrManager\maximelle_repo-comportpackages-ce5e7746b1b3\22.cpgk";
        private static StartUpConfig _startUpConfig;

        public static Application App { get; private set; }
        public static Thread ThreadSender { get; set; }

        public static void Send(string pathToMotionFile,TextBox textBox_Terminal,Application app)
        {
            App = app;
            try
            {
                XmlConfigurator.Configure();

                _startUpConfig = StartUpConfig.CreateConfig(pathToMotionFile);
                var baudRate = ConstantRepositoriy.BAUNT_RATE;
                string comPort = ConstantRepositoriy.COM_PORT;

                Package package;
                ComPortPackagesService comPortPackagesService;

                //if (_startUpConfig.OnlyDone == false)
                //{
                    using (FileStream stream = new FileStream(_startUpConfig.File, FileMode.Open, FileAccess.Read))
                    {
                        stream.Position = 0;
                        package = _serializationService.Deserialize<Package>(stream, SerializationType.Binary);

                    }
                    comPortPackagesService = new ComPortPackagesService(
                  new Rs232Impl(package.ComPort, (BaudRates)baudRate, Parity.None, 8, StopBits.One)
                  , package);


                    comPortPackagesService.Completed += (s, e) =>
                    {
                        comPortPackagesService.StopSender();
                        comPortPackagesService.Close();
                        _log.Info("Отправка данных закончена");

                        if (string.IsNullOrWhiteSpace(package.ProccessName) == false)
                        {
                            _log.Info($"Остановка процесса {package.ProccessName} ...");
                            var process = Process.GetProcessesByName(package.ProccessName).FirstOrDefault();
                            if (process == null)
                            {
                                _log.Info($"Процесс {package.ProccessName} не найден в системе");
                            }
                            else
                            {
                                process.Kill();
                                _log.Info($"Процесс {package.ProccessName} успешно остановлен");
                            }
                        }

                        //System.Console.WriteLine("Нажмите ENTER для выхода");
                        //System.Console.ReadLine();

                    };
                    comPortPackagesService.Init(textBox_Terminal);
                    comPortPackagesService.StartSender();
                //}
                //else
                //{
                //     package = new Package();
                //    package.ComPort = comPort;
                //    comPortPackagesService = new ComPortPackagesService(new Rs232Impl(package.ComPort, (BaudRates)baudRate, Parity.None, 8, StopBits.One)
                //  , package);

                //    comPortPackagesService.SimpleSend(Consts.DoneBuffer);
                //    comPortPackagesService.Close();


                //} 
             }
            //catch (VrLicenseException l1)
            //{
            //    _log.Error(l1);
            //}
            catch (FileNotFoundException e0)
            {
                _log.Error(e0);
                //MessageBox.Show(e0.Message);
            }
            catch (Exception e)
            {
                _log.Error(e);
                //MessageBox.Show(e.Message);
            }

        }

        public static void StartMoutionPosition()
        {
            var baudRate = ConstantRepositoriy.BAUNT_RATE;
            string comPort = ConstantRepositoriy.COM_PORT;
            var package = new Package();
            package.ComPort = comPort;
           var  comPortPackagesService = new ComPortPackagesService(new Rs232Impl(package.ComPort, (BaudRates)baudRate, Parity.None, 8, StopBits.One)
          , package);

            comPortPackagesService.SimpleSend(Consts.DoneBuffer);
            comPortPackagesService.Close();
        }
        private static void ThreadHandler(object sender, ThreadExceptionEventArgs e)
        {
            Environment.Exit(1);
        }

    }
}
