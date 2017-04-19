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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VrManager.Data.Concrete;
using VrManager.Data.Abstract;
using System.Collections.ObjectModel;
using VrManager.Data.Entity;
using VrManager.DialogWindow;

namespace VrManager.Pages
{
    public enum SelectedTab
    {
        Game,
        Video360,
        Video5D
    }
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class TablesPage : Page
    {
        public bool EddingMode {
            get
            {
                return _eddingMode;
            }
            set
            {
                _eddingMode = value;
                if(_eddingMode)
                {
                    ActionPanel.Visibility = Visibility.Collapsed;
                    ManageDbTabs.Visibility = Visibility.Collapsed;
                    CrudFrame.Visibility = Visibility.Visible;
                }
                else
                {
                    ActionPanel.Visibility = Visibility.Visible;
                    ManageDbTabs.Visibility = Visibility.Visible;
                    CrudFrame.Visibility = Visibility.Collapsed;
                }
            }
        }

        EntityRepository _rep;
        SelectedTab _selectedTab = SelectedTab.Video360;
        private static bool _eddingMode;
        private TypeItem _typeItem;

        public TablesPage()
        {
            InitializeComponent();
            _rep = App.Repository;
            _selectedTab = SelectedTab.Video360;
            _rep.RefreshTableVideo360 += _rep_RefreshTableVideo360;
            _rep.RefreshTableVideo5D += _rep_RefreshTableVideo5D;
            _rep.RefreshTableGames += _rep_RefreshTableTableGame;

            TableVideo360.EnableColumnVirtualization = true;
            TableVideo360.CanUserAddRows = false;
            TableVideo5D.EnableColumnVirtualization = true;
            TableVideo5D.CanUserAddRows = false;
            TableGame.EnableColumnVirtualization = true;
            TableGame.CanUserAddRows = false;

            _rep_RefreshTableVideo360(null,null);
            _rep_RefreshTableVideo5D(null,null);
            _rep_RefreshTableTableGame(null, null);

         
            IsItemSelected(TableVideo360, null); 
            IsItemSelected(TableVideo5D, null);
            IsItemSelected(TableGame, null);

           EddingMode = false;
        }

        

        public TablesPage(TypeItem typeItem) : this()
        {
            _typeItem = typeItem;
            ManageDbTabs.SelectedIndex = (int)typeItem;
        }

        private void _rep_RefreshTableVideo5D(object sender, EventArgs e)
        {     
            TableVideo5D.ItemsSource = _rep.Videos.Where(movi => movi.TypeItem == TypeItem.Video5D).ToList();
        }
        private void _rep_RefreshTableVideo360(object sender, EventArgs e)
        {
            TableVideo360.ItemsSource = _rep.Videos.Where(movi => movi.TypeItem == TypeItem.Video360).ToList();
        }
        private void _rep_RefreshTableTableGame(object sender, EventArgs e)
        {
            TableGame.ItemsSource = _rep.Games.ToList();
        }

        private void AddNewItem_Click(object sender, RoutedEventArgs e)
        {
            Page dialog = getAddDialogPage();
            CrudFrame.Navigate(dialog);
            EddingMode = true;
        }

        private void EditItem_Click(object sender, RoutedEventArgs e)
        {
            if(TableVideo360.SelectedItem == null && TableVideo5D.SelectedItem == null && TableGame.SelectedItem == null)
            {
                MessageBox.Show("Елемент не выбран");
                return;
            }

            DataGrid SourseTable = getCurrentTable();
            BaseContentEntity selectedItem = SourseTable.SelectedItem as BaseContentEntity;
            Page dialog = getEditialogPage(selectedItem);
            CrudFrame.Navigate(dialog);
            EddingMode = true;
        }
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataGrid SourseTable = getCurrentTable();
                BaseContentEntity selectedItem = SourseTable.SelectedItem as BaseContentEntity;
                if(_selectedTab == SelectedTab.Game)
                {
                    _rep.DeleteGame(selectedItem as ModelGame);
                }
                else
                {
                    _rep.DeleteVideo(selectedItem as ModelVideo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ManageDbTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if ((ManageDbTabs.SelectedItem as TabItem).Header.ToString() == "Video360")
            {               
                _selectedTab = SelectedTab.Video360;
                SelectionTabTitle.Text = "Видео 360";
            }
            else if ((ManageDbTabs.SelectedItem as TabItem).Header.ToString() == "Video5D")
            {
                _selectedTab = SelectedTab.Video5D;
                SelectionTabTitle.Text = "Видео 5D";
            }
            else if ((ManageDbTabs.SelectedItem as TabItem).Header.ToString() == "Game")
            {
                _selectedTab = SelectedTab.Game;
                SelectionTabTitle.Text = "Игры";
            }
            _typeItem = getTypeItem();
            EddingMode = false;
        }

        private TypeItem getTypeItem()
        {
            TypeItem? typeItem = null;
            switch (_selectedTab)
            {
                case SelectedTab.Video360:
                    typeItem = TypeItem.Video360;
                    break;
                case SelectedTab.Video5D:
                    typeItem = TypeItem.Video5D;
                    break;
                case SelectedTab.Game:
                    typeItem = TypeItem.Game;
                    break;
            }
            return (TypeItem)typeItem;
        }

        private DataGrid getCurrentTable()
        {
            if (_selectedTab == SelectedTab.Video360)
            {
                return TableVideo360;
            }
            else if (_selectedTab == SelectedTab.Video5D)
            {
                return TableVideo5D;
            }
            else if(_selectedTab == SelectedTab.Game)
            {
                return TableGame;
            }

            return null;

        }

        private Page getAddDialogPage()
        {
            if (_typeItem == TypeItem.Video360 || _typeItem == TypeItem.Video5D)
            {
                return new VideoAddOrEditDialog(getTypeItem());
            }
            else
            {
                return new GameAddOrEditDialog(getTypeItem());
            }
        }

        private Page getEditialogPage(BaseContentEntity enity)
        {
            if (_typeItem == TypeItem.Video360 || _typeItem == TypeItem.Video5D)
            {
                return new VideoAddOrEditDialog(getTypeItem(),enity as ModelVideo);
            }
            else
            {
                return new GameAddOrEditDialog(getTypeItem(),enity as ModelGame);
            }
        }

        private void OptionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EddingMode = false;
            ManageDbTabs.SelectedIndex = (sender as ListBox).SelectedIndex;
            TableVideo360.SelectedIndex = -1;
            TableVideo5D.SelectedIndex = -1;
            TableGame.SelectedIndex = -1;
            IsItemSelected(TableVideo360, null);
            IsItemSelected(TableVideo5D, null);
            IsItemSelected(TableGame, null);
        }

        private void IsItemSelected(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if(dataGrid.SelectedIndex == -1)
            {
                EditItem.IsEnabled = false;
                DeleteItem.IsEnabled = false;
            }
            else
            {
                EditItem.IsEnabled = true;
                DeleteItem.IsEnabled = true;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.MainWnd.ChangeTitle(Title);
        }
    }
}
