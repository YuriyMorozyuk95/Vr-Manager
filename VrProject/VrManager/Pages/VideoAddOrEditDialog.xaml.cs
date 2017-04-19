using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Microsoft.Win32;
using VrManager.Data.Abstract;
using VrManager.Data.Concrete;
using VrManager.Data.Entity;
using VrManager.Pages;
using System.Text.RegularExpressions;
using MahApps.Metro.Controls;

namespace VrManager.DialogWindow
{
    public partial class VideoAddOrEditDialog : Page
    {
        private TypeItem _typeVideo;
        private EntityRepository _rep;
        private bool isAddOrEdit = false;
        private ModelVideo _oldVideo;
        private TimeSpan selectedTime;

        public VideoAddOrEditDialog(TypeItem videoType)
        {
            InitializeComponent();
            _typeVideo = videoType;
            _rep = App.Repository;
        }
        public VideoAddOrEditDialog(TypeItem videoType, ModelVideo editVideo)
        {
            InitializeComponent();
            _rep = App.Repository;
            _oldVideo = editVideo;
            _typeVideo = videoType;
            TBox_Name.Text = _oldVideo.Name;
            TB_OpenFileIcon.Text = _oldVideo.PathIcon;
            TB_OpenFileVideo.Text = _oldVideo.ItemPath;
            TB_OpenFileVideoBanner.Text = _oldVideo.PathToBannerVideo;

            if (_oldVideo.MonitorNumber == 1)
            {
                RBOneMonitor.IsChecked = true;
            }
            else if(_oldVideo.MonitorNumber == 2)
            {
                RBTwoMonitor.IsChecked = true;
            }

            TB_OpenFileSettings.Text = _oldVideo.VrSettingPath;
            TB_OpenFileMoution.Text = _oldVideo.FileMotion;

            if (_oldVideo.TimeOut != null)
            {
                selectedTime = _oldVideo.TimeOut.Value.TimeOfDay;
                TP_TimeOut.SelectedTime = selectedTime;
            }
            else
            {
                TP_TimeOut.SelectedTime = new TimeSpan(0, 0, 30);
            }

            if (_oldVideo.IconType == IconType.Image)
            {
                RBtn_Image.IsChecked = true;
            }
            else if(_oldVideo.IconType == IconType.Video)
            {
                RBtn_Video.IsChecked = true;
            }

            isAddOrEdit = true;
        }

        public ModelVideo Result { get; set; }

        private void Btn_OpenFileIcon_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            try
            {
                if (RBtn_Image.IsChecked == true)
                {
                    dialog.InitialDirectory = App.Setting.PathToFolderFiles + @"\Config\Icon\ImageIcon";
                }
                else if (RBtn_Video.IsChecked == true)
                {
                    dialog.InitialDirectory = App.Setting.PathToFolderFiles + @"\Config\Icon\VidoIcon";
                }
                else
                {
                    dialog.InitialDirectory = App.Setting.PathToFolderFiles + @"\Config\Icon";
                }
           

            if (dialog.ShowDialog() == true)
            {
                TB_OpenFileIcon.Text = dialog.FileName;          
            }
            }
            catch
            {
                ValidationMessage.Text = "Задайде в настройках путь к папке с файлмаи";
            }
        }
        private void Btn_OpenFileVideo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            try
            {
                if (_typeVideo == TypeItem.Video360)
                {
                    dialog.InitialDirectory = App.Setting.PathToFolderFiles + @"\Video\Video360";
                }
                else
                {
                    dialog.InitialDirectory = App.Setting.PathToFolderFiles + @"\Video\Video5D";
                }


                if (dialog.ShowDialog() == true)
                {
                    TB_OpenFileVideo.Text = dialog.FileName;
                }
            }
            catch
            {
                ValidationMessage.Text = "Задайде в настройках путь к папке с файлами";
            }
        }
        private void Btn_OpenFileSettings_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            try
            {
                dialog.InitialDirectory = App.Setting.PathToFolderFiles + @"\Config\SettingPlayer";

                if (dialog.ShowDialog() == true)
                {
                    TB_OpenFileSettings.Text = dialog.FileName;
                }
            }
            catch
            {
                ValidationMessage.Text = "Задайде в настройках путь к папке с файлами";
            }
        }
        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            App.Frame.Navigate( new TablesPage(_typeVideo));
        }
        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                if(TBox_Name.Text == string.Empty)
                {
                    throw new Exception();
                }
                ModelVideo newVideo = new ModelVideo()
                {
                    Name = TBox_Name.Text,
                    TypeItem = _typeVideo,
                    PathIcon = TB_OpenFileIcon.Text,
                    ItemPath = TB_OpenFileVideo.Text,
                    VrSettingPath = TB_OpenFileSettings.Text,
                    FileMotion = TB_OpenFileMoution.Text,
                    PathToBannerVideo = TB_OpenFileVideoBanner.Text
                };
                int? numMonitor = null ;
                if (RBOneMonitor.IsChecked == true)
                {
                    numMonitor = 1;
                }
                else if (RBTwoMonitor.IsChecked == true)
                {
                    numMonitor = 2;
                }
                newVideo.MonitorNumber = numMonitor;
                if (TP_TimeOut.SelectedTime != null)
                {
                    DateTime? nowDate = new DateTime(2000, 12, 12, 0, 0, 0);
                    nowDate += TP_TimeOut.SelectedTime;
                    newVideo.TimeOut = nowDate;
                }
                IconType? iconType = null;

                if (RBtn_Image.IsChecked == true)
                {
                    iconType = IconType.Image;
                }
                else if (RBtn_Video.IsChecked == true)
                {
                    iconType = IconType.Video;
                }

                newVideo.IconType = (IconType)iconType;

                if (isAddOrEdit)
                {
                    _rep.ChangeVideo(_oldVideo, newVideo);
                }
                else
                {
                    _rep.AddVideo(newVideo);
                }

                App.Frame.Navigate(new TablesPage(_typeVideo));
            //}
            //catch(Exception ex)
            //{
            //    ValidationMessage.Text = "Вы не заполнили всех полей!";
            //}
        }

       
        

     

        private void TB_MonitorNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Btn_OpenFileMoution_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            try
            {
                if (_typeVideo == TypeItem.Video360)
                {
                    dialog.InitialDirectory = App.Setting.PathToFolderFiles + @"\Moution\Video360";
                }
                else if (_typeVideo == TypeItem.Video5D)
                {
                    dialog.InitialDirectory = App.Setting.PathToFolderFiles + @"\Moution\Video5D";
                }

                if (dialog.ShowDialog() == true)
                {
                    TB_OpenFileMoution.Text = dialog.FileName;
                }
            }
            catch
            {
                ValidationMessage.Text = "Задайде в настройках путь к папке с файлами";
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(TP_TimeOut.SelectedTime == null)
            {
                TP_TimeOut.SelectedTime = new TimeSpan(0, 0, 30);
            }
            //else
            //{
            //    TP_TimeOut.SelectedTime = null;
            //    TP_TimeOut.SelectedTime = selectedTime;
            //}
        }

        private void Btn_OpenFileVideoBanner_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            try
            {
                dialog.InitialDirectory = App.Setting.PathToFolderFiles + @"\Video\BannerVideo";

                if (dialog.ShowDialog() == true)
                {
                    TB_OpenFileVideoBanner.Text = dialog.FileName;
                }
            }
            catch
            {
                ValidationMessage.Text = "Задайде в настройках путь к папке с файлами";
            }
        }
    }
}
