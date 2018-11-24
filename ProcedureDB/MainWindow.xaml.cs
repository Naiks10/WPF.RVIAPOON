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
using System.Data;
using System.Data.SqlClient;
using ProcedureDB1;

namespace ProcedureDB
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static SqlConnection connection1 = new SqlConnection("Data Source = DESKTOP-12BNNTQ\\RVO; " +
                        "Initial Catalog = ZAK_base1; " +
                        "Persist Security Info = True;" +
                        " User ID = sa; " +
                        "Password=\"123\"");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection1.Open();
                SqlCommand command1 = new SqlCommand("SELECT ID_DOCK, NAM_Dock, NOM, NOM1  FROM  DOCK", connection1);
                DataTable dataTable1 = new DataTable();
                dataTable1.Load(command1.ExecuteReader());
                dg1.ItemsSource = dataTable1.DefaultView;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection1.Close();
            }
        }

        private void dg1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dg1.SelectedItem == null) { Update.IsEnabled = false; Delete.IsEnabled = false; } else { Update.IsEnabled = true; Delete.IsEnabled = true; };
            try
            {
                DataRowView drv = (DataRowView)dg1.SelectedItem;
                tb1.Text = drv["НАЗВАНИЕ"].ToString();
                tb2.Text = drv["Номер"].ToString();
            }
            catch
            {
                tb1.Text = "";
                //tb2.Text = "";
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection1.Open();
                SqlCommand Add = new SqlCommand("Add_DOCK", connection1);
                Add.CommandType = CommandType.StoredProcedure;
                Add.Parameters.AddWithValue("@NAM_DOCK", tb1.Text);
                Add.Parameters.AddWithValue("@NOM", Convert.ToInt32(tb2.Text));
                Add.Parameters.AddWithValue("@NOM1", Convert.ToInt32(tb2.Text));
                Add.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection1.Close();
                Window_Loaded(sender, e);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            DataRowView id = (DataRowView)dg1.SelectedItem;
            try
            {
                connection1.Open();
                SqlCommand upd = new SqlCommand("Upd_DOCK", connection1);
                upd.CommandType = CommandType.StoredProcedure;
                upd.Parameters.AddWithValue("@ID_DOck", (int)id["КОД"]);
                upd.Parameters.AddWithValue("@Nam_Dock", tb1.Text);
                upd.Parameters.AddWithValue("@NOM", Convert.ToInt32(tb2.Text));
                upd.Parameters.AddWithValue("@NOM1", Convert.ToInt32(tb2.Text));
                DataRowView drv = (DataRowView)dg1.SelectedItem;
                upd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection1.Close();
                Window_Loaded(sender, e);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView id = (DataRowView)dg1.SelectedItem;
            try
            {
                connection1.Open();
                SqlCommand del = new SqlCommand("Del_DOCK", connection1);
                del.CommandType = CommandType.StoredProcedure;
                del.Parameters.AddWithValue("@ID_DOCK", (int)id["КОД"]);
                del.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection1.Close();
                Window_Loaded(sender, e);
            }
        }

        private void FindService(string Type, string Sign)
        {
            foreach (DataRowView Nazv in dg1.ItemsSource)
            {
                if (Nazv[Type].ToString() == Sign)
                {
                    dg1.SelectedIndex = dg1.Items.IndexOf(Nazv);
                    dg1.ScrollIntoView(dg1.SelectedItem);
                    DataGridRow row = (DataGridRow)dg1.ItemContainerGenerator.ContainerFromIndex(dg1.SelectedIndex);
                    row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    //Finder - textbox , collect combobox
                }
            }
        }

        private void ServiceFilter(string Sign)
        {
            try
            {
                connection1.Open();
                SqlCommand FILTR = new SqlCommand("FILTR_DOCK", connection1);
                FILTR.CommandType = CommandType.StoredProcedure;
                FILTR.Parameters.AddWithValue("@NAM_DOCK", Sign);
                FILTR.ExecuteNonQuery();
                DataTable dataTable1 = new DataTable();
                dataTable1.Load(FILTR.ExecuteReader());
                dg1.ItemsSource = dataTable1.DefaultView;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection1.Close();
                
            }
           
            /* if (FilterButton.Content.ToString() == "Фильтрация")
             {
                 try
                 {
                     DataTable dt = new DataTable();
                     dt = ((DataView)dg1.ItemsSource).ToTable();
                     dt.DefaultView.RowFilter = string.Format(Type + "= '{0}'", Sign);
                     dg1.ItemsSource = dt.DefaultView;
                     dg1.SelectedItem = null;
                     FilterButton.Content = "Сброс";
                 }
                 catch { };
             }
             else
             {
                 DataTable dt = new DataTable();
                 dt = ((DataView)dg1.ItemsSource).ToTable();
                 dt.DefaultView.RowFilter = null;
                 dg1.ItemsSource = dt.DefaultView;
                 dg1.SelectedItem = null;
                 FilterButton.Content = "Фильтрация";
             }*/
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            //FindService(collect.Text, Finder.Text);
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            ProcedureDB1.Window1 window1 = new Window1();
            window1.Show();
        }

        //    try
        //    {
        //        connection1.Open();
        //        SqlCommand FILTR = new SqlCommand("FILTR_DOCK", connection1);
        //        FILTR.CommandType = CommandType.StoredProcedure;
        //        FILTR.Parameters.AddWithValue("@NAM_DOCK", Finder.Text);
        //        FILTR.ExecuteNonQuery();
        //        DataTable dataTable1 = new DataTable();
        //        dataTable1.Load(FILTR.ExecuteReader());
        //        dg1.ItemsSource = dataTable1.DefaultView;
        //    }
        //    catch (SqlException ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        connection1.Close();

        //    }
        //   // Window_Loaded(sender, e);
        //}

        private void collect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Finder_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = "SELECT * FROM DOCK WHERE (NAM_DOCK = '" + Finder.Text + "' or NOM ='"+Finder.Text+"')" ;
            SqlCommand sqlCommand = new SqlCommand(query,connection1);
            connection1.Open();
            try
            {
                sqlCommand.ExecuteNonQuery();

                DataTable dataTable1 = new DataTable();
                dataTable1.Load(sqlCommand.ExecuteReader());
                dg1.ItemsSource = dataTable1.DefaultView;
            }
            catch
            {
                query = "SELECT * FROM DOCK WHERE NAM_DOCK = '" + Finder.Text + "'";
                sqlCommand = new SqlCommand(query, connection1);
                sqlCommand.ExecuteNonQuery();
                DataTable dataTable1 = new DataTable();
                dataTable1.Load(sqlCommand.ExecuteReader());
                dg1.ItemsSource = dataTable1.DefaultView;
            }
            if (Finder.Text == "" )
            {
                query = "SELECT * FROM DOCK";
                sqlCommand = new SqlCommand(query, connection1);
                sqlCommand.ExecuteNonQuery();
                DataTable dataTable1 = new DataTable();
                dataTable1.Load(sqlCommand.ExecuteReader());
                dg1.ItemsSource = dataTable1.DefaultView;
            }
            connection1.Close();
        }
    }
}
    

