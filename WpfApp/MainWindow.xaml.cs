using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       ApplicationContext db = new ApplicationContext();
       SortedSet<Equipment> equipmentList = new SortedSet<Equipment>(new EquipmentComparer());   
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            LoadEquipmentsToBase();
            try
            {
                db.Equipments.Load();
            }
            catch { }
            DataContext = db.Equipments.Local.ToObservableCollection();
            equipmentsGrid.ItemsSource = db.Equipments.Local.ToBindingList();
            foreach (Equipment equipment in db.Equipments)
            equipmentList.Add(equipment);
        }
        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }
        private void exportButton_Click(object sender, RoutedEventArgs e)
        {
            ExportDataToExcel.export(ref equipmentList);
        }
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow(new Equipment());
            try
            {
                if (addWindow.ShowDialog() == true)
                {
                    Equipment equipment = addWindow.equipment;
                    db.Equipments.Add(equipment);
                    db.SaveChanges();
                    equipmentList.Add(equipment);
                }
            } catch { }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            if (equipmentsGrid.SelectedItems.Count > 0)
            {
                for (int i = equipmentsGrid.SelectedItems.Count - 1; i >= 0; i--)
                {
                    Equipment? equipment = equipmentsGrid.SelectedItems[i] as Equipment; 
                    if (equipment is null) return;

                    AddWindow addWindow = new AddWindow(new Equipment
                    {
                        Id = equipment.Id,
                        Name = equipment.Name,
                        EquipmentType = equipment.EquipmentType
                    });

                    try
                    {
                        if (addWindow.ShowDialog() == true)
                        {
                            equipmentList.Remove(equipment);
                            equipment = db.Equipments.Find(addWindow.equipment.Id);
                            if (equipment != null)
                            {
                                equipment.Name = addWindow.equipment.Name;
                                equipment.EquipmentType = addWindow.equipment.EquipmentType;
                                equipmentList.Add(equipment);
                                equipmentsGrid.Items.Refresh();
                            }
                        }
                    } catch { }
                }
                try
                {
                    db.SaveChanges();
                }
                catch { }
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (equipmentsGrid.SelectedItems.Count > 0)
            {
                for (int i = equipmentsGrid.SelectedItems.Count-1; i >=0; i--)
                {
                    Equipment? equipment = equipmentsGrid.SelectedItems[i] as Equipment;
                    if (equipment != null)
                    {
                        db.Equipments.Remove(equipment);
                        equipmentList.Remove(equipment);
                    }
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch { }
        }
        private void LoadEquipmentsToBase()
        {
            Equipment[] equipments = new Equipment[4];
            equipments[0] = new Equipment { Name = "Трактор", EquipmentType = "Карьерное оборудование" };
            equipments[1] = new Equipment { Name = "Молоток инженерный", EquipmentType = "Молотки" };
            equipments[2] = new Equipment { Name = "Лопата саперная", EquipmentType = "Лопаты" };
            equipments[3] = new Equipment { Name = "Рельс", EquipmentType = "Шахтное оборудование" };

            foreach (Equipment equipment in equipments)
            {
                db.Equipments.Add(equipment);
                equipmentList.Add(equipment);
            }
            try
            {
                db.SaveChanges();
            }
            catch { }
        }
    }
}