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

namespace AnvarovAvtosevice
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {

        private Service _currentService = new Service();

        public bool check = false;

        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();

            if (SelectedService != null)
            {
                check = true;
                _currentService = SelectedService;
            }

            DataContext = _currentService;
        }

        

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentService.Title))
                errors.AppendLine("Укажите название услуги");

            if (_currentService.Cost <= 0)
                errors.AppendLine("Укажите стоимость улсуги");

            if (_currentService.Discount < 0 || _currentService.Discount > 100)
                errors.AppendLine("Не правильная скидка");

            if (_currentService.Duration == 0 || string.IsNullOrWhiteSpace(_currentService.Duration.ToString()))
                errors.AppendLine("Укажите длительность услуги");
            else
            {
                if (_currentService.Duration > 240 || _currentService.Duration < 1)
                    errors.AppendLine("Длительность не может быть больше 240 минут и меньше 1");
            }
            

            if (string.IsNullOrWhiteSpace(_currentService.Discount.ToString()))
            {
                _currentService.Discount = 0;
            }

           

            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var allServices = RasstegaevServiceEntities.GetContext().Service.ToList();
            allServices = allServices.Where(p => p.Title == _currentService.Title).ToList();

            if (allServices.Count == 0 || check == true)
            {
                if (_currentService.ID == 0)
                    RasstegaevServiceEntities.GetContext().Service.Add(_currentService);

                try
                {
                    RasstegaevServiceEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
                MessageBox.Show("Уже существует такая услуга");
                
            }
        }
    }

