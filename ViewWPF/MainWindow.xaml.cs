using System.Windows;
using Microsoft.Win32;

namespace ViewWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private BussinessLogic.Services.Import.ICarImportService? _importService;

    /// <summary>
    /// Инициализирует новый экземпляр MainWindow.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Устанавливает сервис импорта/экспорта автомобилей.
    /// </summary>
    /// <param name="importService">Сервис импорта/экспорта.</param>
    public void SetImportService(BussinessLogic.Services.Import.ICarImportService importService)
    {
        _importService = importService;
    }

    /// <summary>
    /// Обработчик нажатия кнопки экспорта автомобилей.
    /// </summary>
    private void ButtonExport_Click(object sender, RoutedEventArgs e)
    {
        var selectedItems = dataGridCars.SelectedItems;
        bool exportAll = false;

        if (selectedItems.Count > 0)
        {
            var result = MessageBox.Show(
                $"Экспортировать {selectedItems.Count} выбранных автомобилей?\n\n" +
                "Да - экспорт выбранных\n" +
                "Нет - экспорт всех автомобилей\n" +
                "Отмена - отменить экспорт",
                "Экспорт автомобилей",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Cancel)
                return;

            exportAll = (result == MessageBoxResult.No);
        }
        else
        {
            exportAll = true;
        }

        var dialog = new SaveFileDialog
        {
            Filter = "CSV файлы (*.csv)|*.csv|JSON файлы (*.json)|*.json",
            Title = "Экспорт автомобилей",
            FileName = $"cars_export_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}"
        };

        if (dialog.ShowDialog() == true)
        {
            if (_importService == null)
            {
                MessageBox.Show(
                    "Сервис экспорта не инициализирован.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            try
            {
                int exportedCount;
                bool isCsv = dialog.FilterIndex == 1;

                if (exportAll)
                {
                    if (isCsv)
                    {
                        exportedCount = _importService.ExportToCsv(dialog.FileName);
                    }
                    else
                    {
                        exportedCount = _importService.ExportToJson(dialog.FileName);
                    }
                }
                else
                {
                    var selectedIds = new List<int>();
                    foreach (var item in selectedItems)
                    {
                        if (item is Presenter.ObservableDTO.ObservableCarListItemDto car)
                        {
                            selectedIds.Add(car.Id);
                        }
                    }

                    if (isCsv)
                    {
                        exportedCount = _importService.ExportToCsv(selectedIds, dialog.FileName);
                    }
                    else
                    {
                        exportedCount = _importService.ExportToJson(selectedIds, dialog.FileName);
                    }
                }

                MessageBox.Show(
                    $"Экспорт успешно завершен!\n\n" +
                    $"Экспортировано автомобилей: {exportedCount}\n" +
                    $"Файл: {dialog.FileName}",
                    "Экспорт завершен",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при экспорте:\n\n{ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }

    /// <summary>
    /// Обработчик нажатия кнопки генерации QR-кода для выбранного автомобиля.
    /// </summary>
    private void ButtonQRCode_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is Presenter.ViewModels.MainViewModel viewModel)
        {
            var selectedCar = viewModel.SelectedCar;
            if (selectedCar == null)
            {
                MessageBox.Show(
                    "Пожалуйста, выберите автомобиль для генерации QR-кода.",
                    "Автомобиль не выбран",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            try
            {
                var car = viewModel.CarService.GetCar(selectedCar.Id);
                if (car == null)
                {
                    MessageBox.Show(
                        "Не удалось загрузить данные автомобиля.",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                var carQRDto = new BussinessLogic.Dto.CarQRDto
                {
                    Id = car.Id,
                    Brand = car.Brand,
                    Model = car.Model,
                    LicensePlate = car.LicensePlate,
                    Year = car.Year,
                    Mileage = car.Mileage,
                    RentalPricePerHour = car.RentalPricePerHour,
                    Status = car.Status.ToString()
                };

                var qrViewModel = viewModel.VMManager.CreateCarQRViewModel(carQRDto);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при создании QR-кода:\n\n{ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
