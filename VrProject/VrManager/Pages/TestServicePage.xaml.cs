using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VrManager.Helpers;
using VrManager.Service;


namespace VrManager.View
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class TestServicePage : Page
    {
        private ClientService _service;
        public TestServicePage()
        {
            InitializeComponent();
            _service = new ClientService();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            //MonitorManager maneger = new MonitorManager();
            //OpenVrPleyerHelper player = new OpenVrPleyerHelper();
            //player.OpenVrPlayer();

            //Task.Factory.StartNew(() =>
            //{
            //    Stopwatch watch = new Stopwatch();
            //    watch.Start();
            //    while (watch.Elapsed < new TimeSpan(0, 0, 30))
            //    {
            //        if (maneger.IsWindowPlayerExist())
            //        {
            //            Task.Delay(2000).Wait();
            //            maneger.SetLoc765756ations(1);
            //            watch.Stop();
            //            return;
            //        }
            //        Task.Delay(500).Wait();
            //    }
            //});
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            _service.Play();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            _service.Pause();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            _service.Stop();
        }
    }
}
