using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VrManager.Data.Concrete;
using VrManager.Data.Entity;
using VrManager.Helpers;

namespace VrManager.Pages
{
    /// <summary>
    /// Interaction logic for StatisticPage.xaml
    /// </summary>
    public partial class StatisticPage : Page
    {
        private OptionData obj;
        EntityRepository _rep;

        public StatisticPage()
        {
            InitializeComponent();
            _rep = App.Repository;
            try
            {
                obj = Serializer.Desirialize();
                TB_Email.Text = obj.Email;
            }
            catch
            {

            }     
 
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.MainWnd.ChangeTitle(Title);
            TableStatistic.ItemsSource = _rep.Observes.ToList();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            List<StatisticItem> StatisticItems = StisticFormatHelper
                                                .StatisticItemFactory(_rep.Observes);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = App.Setting.PathToFolderFiles + @"\Config";
            dialog.Filter = "Файл Excel|*.xlsx";
            dialog.Title = "Сохранить таблицу Excel";
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                CreateExcelFileHelper.CreateExcelDocument(StatisticItems, dialog.FileName);
            }
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            obj = new OptionData();
            obj.Email = TB_Email.Text;
            Serializer.Serilize(obj);
            SendEmailHalper.SendEmailTo(obj.Email);
        }
    }

    //public class IDataErrorInfo
    //{
    //    string Error { get; }
    //    string this[string columnName] { get; }
    //}
}
