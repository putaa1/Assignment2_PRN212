using DataAccessLayer.DTO;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Interface;
using System.Windows;
using System.Windows.Controls;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for RoomManagement.xaml
    /// </summary>
    public partial class RoomManagement : UserControl
    {
        private readonly IRoomService _service;

        public RoomManagement()
        {
            InitializeComponent();
            _service = ((App)Application.Current).ServiceProvider.GetRequiredService<IRoomService>() ?? throw new ArgumentNullException(nameof(RoomService));
            LoadData();
        }

        private void LoadData()
        {
            dgRooms.ItemsSource = null;
            var rooms = _service.GetRooms(r => r.RoomNumber.Contains(txtSearch.Text));
            dgRooms.ItemsSource = rooms;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                try
                {
                    var rooms = _service.GetRooms(r => r.RoomNumber.Contains(txtSearch.Text));
                    // Ensure UI update happens on the main thread
                    Dispatcher.Invoke(() =>
                    {
                        dgRooms.ItemsSource = null;
                        dgRooms.ItemsSource = rooms;
                    });
                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void dgRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Implement selection changed logic if necessary
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addEditRoomDialog = new AddEditRoomDialog();
            if (addEditRoomDialog.ShowDialog() == true)
            {
                var newRoom = addEditRoomDialog.Room;
                _service.AddRoom(newRoom);
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgRooms.SelectedItem is RoomDTO selectedRoom)
            {
                var addEditRoomDialog = new AddEditRoomDialog(selectedRoom);
                if (addEditRoomDialog.ShowDialog() == true)
                {
                    var updatedRoom = addEditRoomDialog.Room;
                    _service.UpdateRoom(updatedRoom);
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Please select a room to edit.", "Edit Room", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgRooms.SelectedItem is RoomDTO selectedRoom)
            {
                if (MessageBox.Show($"Are you sure you want to delete Room {selectedRoom.RoomNumber}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    selectedRoom.RoomStatus = 0;
                    _service.UpdateRoom(selectedRoom);
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Please select a room to delete.", "Delete Room", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
