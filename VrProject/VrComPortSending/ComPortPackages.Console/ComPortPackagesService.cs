using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using ComPortPackages.Core.Extensions;
using ComPortPackages.Core.Model;
using ComPortPackages.Core.RS232;
using log4net;
using System.Windows.Controls;
using System.Windows;
using System.Threading.Tasks;

namespace ComPortPackages.Console
{
    public class ComPortPackagesService : IDisposable
    {
        private static readonly object _syncThreadObject = new object();
        private readonly ILog _log = LogManager.GetLogger(typeof(ComPortPackagesService));
        private readonly Queue<EffectStreamBytesSample> _messages = new Queue<EffectStreamBytesSample>();
        private readonly Package _package;
        private static Rs232Impl _rs232;
        private readonly ManualResetEvent _threadEvent = new ManualResetEvent(false);
        private SendThreadState _sendThreadState = SendThreadState.StandBy;
        private static Thread _thread;

        private int T0 = ConstantRepositoriy.T0;
        private int T1 = ConstantRepositoriy.T1;
        private int T2 = ConstantRepositoriy.T2;
        private int T3 = ConstantRepositoriy.T3;

        public TextBox TB_showProcess { get; private set; }

        public ComPortPackagesService(Rs232Impl rs232, Package package)
        {
            _rs232 = rs232;
            _package = package;
        }

        public event EventHandler Completed;

        private void OnCompleted()
        {
            if (Completed != null)
            {
                Completed(this, EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            Close();
        }

        public void Init(TextBox TB_showProcess)
        {
            this.TB_showProcess = TB_showProcess;
            foreach (var eStream in _package.Effect.BytesSamples)
            {
                _messages.Enqueue(eStream);
            }
            _rs232.Open();
            _thread = new Thread(SendFunc);
            _thread.IsBackground = false;
            _thread.Start();
        }

        public void StartSender()
        {
            lock (_syncThreadObject)
            {
                _sendThreadState = SendThreadState.Send;
            }
        }

        public void StopSender()
        {
            lock (_syncThreadObject)
            {
                _sendThreadState = SendThreadState.StandBy;
            }
        }

        public void SimpleSend(byte[] buffer)
        {
            try
            {
                _rs232.Open();
                _rs232.Send(buffer);
                _log.InfoFormat("Данные {0} отправлены", string.Concat(buffer.Select(b => $" 0x{b.ToString("X2")}")));
            }
            catch (Exception e)
            {
                _log.Error(e);
                
            }
        }


        private void SendFunc()
        {
            bool firstPack = true;
            int counter = 0;
            while (_sendThreadState != SendThreadState.Terminated)
            {
                if (_sendThreadState == SendThreadState.Send)
                {
                    if (_messages.Count > 0)
                    {
                        var message = _messages.Dequeue();
                        if (message != null)
                        {
                            try
                            {
                                _rs232.Send(message.Data.StringToByteArray());

                                _log.InfoFormat("Данные {0} отправлены", message.SampleTime.ToString("HH:mm:ss"));

                                Action a = () =>
                                {
                                    TB_showProcess.Text += $"Итерацыя №{counter++} Данные {message.SampleTime.ToString("HH:mm:ss")} отправлены" + Environment.NewLine;
                                };
                                ComPrortSender.App.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, a);

                            }
                            catch (Exception e)
                            {
                                _log.Error(e);
                            }
                        }
                    }
                    else
                    {
                        _threadEvent.Set();
                        OnCompleted();
                    }
                    //Thread.Sleep(1000);
                }


                if (firstPack == false)
                {
                    if (_messages.Count > 1)
                    {
                        Thread.Sleep(T1);
                    }

                    if (_messages.Count == 1)
                    {
                        Thread.Sleep(T2);
                    }

                    if (_messages.Count == 0)
                    {
                        Thread.Sleep(T3);
                    }
                }
                else
                {
                    //после пакета инициализации
                    Thread.Sleep(T0);
                }

                firstPack = false;
            }

            _threadEvent.Set();
        }

        public void Close()
        {
            _rs232.Close();
            TerminateThread();
        }

        private void TerminateThread()
        {
            lock (_syncThreadObject)
            {
                if (_thread != null && _thread.IsAlive)
                {
                    if (_sendThreadState != SendThreadState.Terminated)
                    {
                        _sendThreadState = SendThreadState.Terminated;
                        _threadEvent.WaitOne();
                    }
                }
            }
        }

        public static bool Pause()
        {
            if(_thread == null)
            {
                //MessageBox.Show("Поток отправки файлу движений не запущин");
                return true;
            }

            if (_thread.ThreadState == ThreadState.WaitSleepJoin)
            {
                _thread.Suspend();
                return true;
            }
            else
            {
                //MessageBox.Show(_thread.ThreadState.ToString());
                return true;
            }
        }

        public static bool Resume()
        {
            if (_thread == null)
            {
                //MessageBox.Show("Поток отправки файлу движений не запущин");
                return true; 
            }

            if (_thread.ThreadState == ThreadState.Suspended)
            {
                _thread.Resume();
                return true;
            }
            else
            {
                //MessageBox.Show(_thread.ThreadState.ToString());
                return true;
            }
        }

        public static void Stop()
        {
            if (_thread == null)
            {
                return;
            }

            if(_thread.ThreadState == ThreadState.Suspended)
            {
                _thread.Resume();
            }
         
                _thread.Abort();
                _rs232.Close();
                _thread.DisableComObjectEagerCleanup();

        }
    }
}