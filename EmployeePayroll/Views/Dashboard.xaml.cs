using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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

namespace EmployeePayroll
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
            LoadGrid();
        }
        SqlConnection connection = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=EmployeePayroll;Integrated Security=True");
        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("select EmpID, Name,Profile, Gender, Dept, Salary, Date, Note from Employee", connection);
            DataTable dataTable = new DataTable();
            connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dataTable.Load(sdr);
            connection.Close();
            datagrid.ItemsSource = dataTable.DefaultView;
        }

        private void AddUser(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void DeleteEvent(object sender, RoutedEventArgs e)
        {
            //SqlCommand sqlCommand = new SqlCommand("dbo.InsertIntoEmployee", connection);
            //sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //connection.Open();
            //sqlCommand.Parameters.AddWithValue("@EmpID", id.GetValue);
            DataRowView row = (DataRowView)datagrid.SelectedItem;
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SqlCommand sqlCommand = new SqlCommand("dbo.DeleteFromEmployeeTable", connection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                sqlCommand.Parameters.AddWithValue("@EmpID", row["EmpID"]);
                sqlCommand.ExecuteNonQuery();
                connection.Close();
                LoadGrid();
            }
        }
        private void EditEvent(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)datagrid.SelectedItem;
            MainWindow mainWindow = new MainWindow();
            mainWindow.name_txt.Text = row["Name"].ToString();
            mainWindow.genderMenu.Text = row["Gender"].ToString();
            mainWindow.sliderValue.Text = row["Salary"].ToString();
            string fullDate = row["Date"].ToString();
            string[] date = fullDate.Split(' ');
            mainWindow.date_combo.Text = date[0];
            mainWindow.month_combo.Text = date[1];
            mainWindow.year_combo.Text = date[2];
            mainWindow.note_txt.Text = row["Note"].ToString();
            mainWindow.isUpdate = true;
            mainWindow.EmpID = Convert.ToInt32(row["EmpID"]);
            mainWindow.Show();
            this.Close();
        }
    }
}
