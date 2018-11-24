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
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using ProcedureDB;

namespace ProcedureDB1
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand("Upd_DOCK", ProcedureDB.MainWindow.connection1);
            sqlCommand.CommandText = "select COUNT (*) " +
            "from Authoriz " +
            "where (User_Login = '" + User.Text + "' " +
            "and User_Pass = '" + Pass.Text + "')";
            ProcedureDB.MainWindow.connection1.Open();
            User.Text = sqlCommand.ExecuteScalar().ToString();
            if (User.Text == "1")
            {
                MainWindow mainWindow = new MainWindow();
                ProcedureDB.MainWindow.connection1.Close();
                mainWindow.Show();
                this.Hide();
       
            }
            else
            {
                MessageBox.Show("Неправильный пароль!");
                ProcedureDB.MainWindow.connection1.Close();
            }
            
            //if (User.Text == "1")
            

        }
    }
}
