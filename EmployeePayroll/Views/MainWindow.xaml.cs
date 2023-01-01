using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmployeePayroll
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public string profileLink = "";
        public bool isUpdate = false;
        public int EmpID;
        SqlConnection connection = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=EmployeePayroll;Integrated Security=True");
        public void Clear()
        {
            name_txt.Clear();
            note_txt.Clear();
            a_radio.IsChecked = false;
            b_radio.IsChecked = false;
            c_radio.IsChecked = false;
            d_radio.IsChecked =false;
            genderMenu.Text = "";
            hr_chk.IsChecked = false;
            sales_chk.IsChecked = false;    
            fin_chk.IsChecked = false;
            eng_chk.IsChecked = false;
            other_chk.IsChecked = false;
            slValue.Value = 0;
        }
        public bool isValid()
        {
            if(name_txt.Text == String.Empty)
            {
                MessageBox.Show("Name required.", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (a_radio.IsChecked == false && b_radio.IsChecked == false && c_radio.IsChecked == false && d_radio.IsChecked == false)
            {
                MessageBox.Show("Profile image required.", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (hr_chk.IsChecked == false && sales_chk.IsChecked == false && fin_chk.IsChecked == false & eng_chk.IsChecked == false && other_chk.IsChecked == false)
            {
                MessageBox.Show("Department required.", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (genderMenu.Text == String.Empty)
            {
                MessageBox.Show("Gender required.", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (slValue.Value == 0)
            {
                MessageBox.Show("Salary cannot be zero.", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private void Submit_Click(object sender, RoutedEventArgs e) 
        {
            if (!isUpdate)
            {
                if (isValid())
                {
                    string date = date_combo.Text + " " + month_combo.Text + " " + year_combo.Text;
                    var selectedItems = listOption.Items.Cast<CheckBox>().Where(x => x.IsChecked == true).Select(x => x.Content).ToList();
                    using (connection)
                        try
                        {
                            SqlCommand sqlCommand = new SqlCommand("dbo.InsertIntoEmployee", connection);

                            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                            connection.Open();

                            sqlCommand.Parameters.AddWithValue("@Name", name_txt.Text);
                            sqlCommand.Parameters.AddWithValue("@Profile", profileLink);
                            sqlCommand.Parameters.AddWithValue("@Gender", genderMenu.Text);
                            sqlCommand.Parameters.AddWithValue("@Dept", string.Join(",", selectedItems));
                            sqlCommand.Parameters.AddWithValue("@Salary", sliderValue.Text);
                            sqlCommand.Parameters.AddWithValue("@Date", date);
                            sqlCommand.Parameters.AddWithValue("@Note", note_txt.Text);

                            sqlCommand.ExecuteNonQuery();
                            connection.Close();
                            MessageBox.Show("Data added successfully", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                            Clear();
                            Dashboard dashboard = new Dashboard();
                            dashboard.LoadGrid();
                            dashboard.Show();
                            this.Close();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                }
            }
            else
            {
                if (isValid())
                {
                    string date = date_combo.Text + " " + month_combo.Text + " " + year_combo.Text;
                    var selectedItems = listOption.Items.Cast<CheckBox>().Where(x => x.IsChecked == true).Select(x => x.Content).ToList();
                    using (connection)
                        try
                        {
                            SqlCommand sqlCommand = new SqlCommand("dbo.UpdateEmployeeTable", connection);

                            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                            connection.Open();

                            sqlCommand.Parameters.AddWithValue("@EmpID", EmpID);
                            sqlCommand.Parameters.AddWithValue("@Name", name_txt.Text);
                            sqlCommand.Parameters.AddWithValue("@Profile", profileLink);
                            sqlCommand.Parameters.AddWithValue("@Gender", genderMenu.Text);
                            sqlCommand.Parameters.AddWithValue("@Dept", string.Join(",", selectedItems));
                            sqlCommand.Parameters.AddWithValue("@Salary", sliderValue.Text);
                            sqlCommand.Parameters.AddWithValue("@Date", date);
                            sqlCommand.Parameters.AddWithValue("@Note", note_txt.Text);

                            sqlCommand.ExecuteNonQuery();
                            connection.Close();
                            MessageBox.Show("Data updated successfully", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                            Clear();
                            Dashboard dashboard = new Dashboard();
                            dashboard.LoadGrid();
                            dashboard.Show();
                            this.Close();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                }
            }
        }
        private void Reset_Click(object sender, RoutedEventArgs e) 
        {
            Clear();
        }

        private void cancel_Click(object sender, RoutedEventArgs e) 
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            dashboard.LoadGrid();
            this.Close();
        }

        private void radio_Checked(object sender, RoutedEventArgs e)
        {
            profileLink = (string)(sender as RadioButton).Content;
        }

    }
}
