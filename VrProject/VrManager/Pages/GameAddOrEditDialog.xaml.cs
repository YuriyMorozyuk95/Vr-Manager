using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Threading;
using VrManager.Data.Abstract;
using VrManager.Data.Concrete;
using VrManager.Data.Entity;
using VrManager.Helpers;

namespace VrManager.Pages
{
    /// <summary>
    /// Interaction logic for GameAddOrEditDialog.xaml
    /// </summary>
    public partial class GameAddOrEditDialog : Page
    {
        private TimeSpan? selectedTime;
        private EntityRepository _rep;
        private ModelGame _oldGame;
        private Key? _additionalKeyPressed;
        private TypeItem _typeItem;
        private bool isAddOrEdit = false;

        public GameAddOrEditDialog(TypeItem typeItem)
        {
            InitializeComponent();
            _typeItem = typeItem;
            _rep = App.Repository;
        }
        public GameAddOrEditDialog(TypeItem typeItem,ModelGame game) : this(typeItem)
        {
            _oldGame = game;
            isAddOrEdit = true;
            TBox_Name.Text  = _oldGame.Name;
            TB_OpenFileIcon.Text = _oldGame.PathIcon;
            TB_OpenFileGame.Text = _oldGame.ItemPath;
            TBox_Params.Text = _oldGame.StartUpParams;
            TBox_NameProcess.Text = _oldGame.NameProcess;
            TB_OpenFileMoution.Text = _oldGame.FileMotion;
            _additionalKeyPressed = _oldGame.AdditionalKey;
            TBox_AddingKey.Text =  _additionalKeyPressed.ToString();
            TBox_TimeShift.Text = _oldGame.StartTime.ToString();
            TB_xMousClick.Text = _oldGame.MouseClickCordX.ToString();
            TB_yMousClick.Text = _oldGame.MouseClickCordY.ToString();
            CB_TypeStart.SelectedIndex = (int)_oldGame.TypeStartFocus;
            TB_OpenFileVideoBanner.Text = _oldGame.PathToBannerVideo;

            if (_oldGame.ShiftPressTime == null)
            {
                TB_ShiftClick.Text = "";
                TS_ShiftClick.IsChecked = false;
            }
            else
            {
                TB_ShiftClick.Text = _oldGame.ShiftPressTime.Value.ToString();
                TS_ShiftClick.IsChecked = true;
            }

           IconType? iconType = _oldGame.IconType;

            if (iconType == IconType.Image)
            {
                RBtn_Image.IsChecked = true;
            }
            else if (iconType == IconType.Video)
            {
                RBtn_Video.IsChecked = true;              
            }

            Key? startUpButtpn = _oldGame.RunKey;

            if (startUpButtpn == Key.Enter)
            {
                RBEnter.IsChecked = true;
            }
            else if (startUpButtpn == Key.Space)
            {
                RBSpace.IsChecked = true;
            }

            if (_oldGame.TimeOut != null)
            {
                selectedTime = _oldGame.TimeOut.Value.TimeOfDay;
                TP_TimeOut.SelectedTime = selectedTime;
            }
            else
            {
                TP_TimeOut.SelectedTime = new TimeSpan(0, 0, 30);
            }
        }
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
        private void Btn_OpenFileGame_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            try
            {
                dialog.InitialDirectory = App.Setting.PathToFolderFiles + @"\Games";
                if (dialog.ShowDialog() == true)
                {
                    TB_OpenFileGame.Text = dialog.FileName;
                }
            }
            catch
            {
                ValidationMessage.Text = "Задайде в настройках путь к папке с файлами";
            }
        }
        private void Btn_OpenFileMoution_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            try
            {
                dialog.InitialDirectory = App.Setting.PathToFolderFiles + @"\Moution\Games";
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
            if (TP_TimeOut.SelectedTime == null)
            {
                TP_TimeOut.SelectedTime = new TimeSpan(0, 0, 30);
            }
            System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => { })).Wait();
        }
        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            App.Frame.Navigate(new TablesPage(Data.Abstract.TypeItem.Game));
        }
        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(TBox_Name.Text == string.Empty
                    || TB_OpenFileGame.Text ==string.Empty
                    || TBox_NameProcess.Text == string.Empty
                    || TB_OpenFileMoution.Text == string.Empty
                    )
                {
                    throw new Exception();
                }

                ModelGame newGame = new ModelGame
                {
                    Name = TBox_Name.Text,
                    PathIcon = TB_OpenFileIcon.Text,
                    ItemPath = TB_OpenFileGame.Text,
                    StartUpParams = TBox_Params.Text,
                    NameProcess = TBox_NameProcess.Text,
                    FileMotion = TB_OpenFileMoution.Text,
                    AdditionalKey = _additionalKeyPressed,
                    StartTime = Convert.ToInt32(TBox_TimeShift.Text),
                    MouseClickCordX = Convert.ToInt32(TB_xMousClick.Text),
                    MouseClickCordY = Convert.ToInt32(TB_yMousClick.Text),
                    TypeStartFocus = (TypeStartFocus)CB_TypeStart.SelectedIndex,
                    PathToBannerVideo = TB_OpenFileVideoBanner.Text

                };

                IconType? iconType = null;

                if (RBtn_Image.IsChecked == true) 
                {
                    iconType = IconType.Image;
                }
                else if (RBtn_Video.IsChecked == true)
                {
                    iconType = IconType.Video;
                }
                newGame.IconType = (IconType)iconType;

                Key? startUpButtpn = null;

                if (RBEnter.IsChecked == true)
                {
                    startUpButtpn = Key.Enter;
                }
                else if (RBSpace.IsChecked == true)
                {
                    startUpButtpn = Key.Space;
                }
                newGame.RunKey = (Key)startUpButtpn;

                if (TP_TimeOut.SelectedTime != null)
                {
                    DateTime? nowDate = new DateTime(2000, 12, 12, 0, 0, 0);
                    nowDate += TP_TimeOut.SelectedTime;
                    newGame.TimeOut = nowDate;
                }

                if(TS_ShiftClick.IsChecked.Value)
                {
                    newGame.ShiftPressTime = int.Parse(TB_ShiftClick.Text);
                }
                else
                {
                    newGame.ShiftPressTime = null;
                }


                if(isAddOrEdit)
                {
                    _rep.ChangeGame(_oldGame,newGame);
                }
                else
                {
                    _rep.AddGame(newGame);
                }

             

                App.Frame.Navigate(new TablesPage(_typeItem));
            }
            catch(FormatException)
            {
                ValidationMessage.Text = "Время в сек.нажатия клавиши Shift или Моусклика введенно некоректно";
            }
            catch(Exception ex)
            {
                ValidationMessage.Text = "Вы не заполнили всех полей!";

            }
        }
        private void TBox_AddingKey_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Back)
            {
                return;
            }
            TBox_AddingKey.Text = string.Empty;
            Key key = e.Key;
            TBox_AddingKey.Text = KeySimulationHelper.GetStringButton(key);
            _additionalKeyPressed = e.Key;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
