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
using System.Data.SqlClient;
using System.Data;


namespace Projektrichtig
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection DatenBankVerbindung = new SqlConnection(@"Data Source = (localdb)\MSSQLLocalDB; AttachDBFilename = C:\Users\danie\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\ProjektDatenBank.mdf; Integrated Security = True; Connect Timeout = 30");

        public MainWindow()
        {
            InitializeComponent();

        }

        private void Button_Click_KontoErstellen(object sender, RoutedEventArgs e)
        {
            this.Hide();

            RegistrierFenster registrierFenster = new RegistrierFenster();
            registrierFenster.Show();
        }

        private void Button_LoginClick(object sender, RoutedEventArgs e)
        {
            if (MainWindowTextboxPasswort.Text != "" || MainWindowTextboxName.Text != "")
            {
                DatenBankVerbindung.Open();
                SqlCommand sqlCommand = new SqlCommand("select * from [dbo].[Table] where Username=@Username and Passwort=@Passwort", DatenBankVerbindung);
                sqlCommand.Parameters.AddWithValue("@Username", MainWindowTextboxName.Text);
                sqlCommand.Parameters.AddWithValue("@Passwort", MainWindowTextboxPasswort.Text);

                // Führe die Abfrage aus
                var dr = sqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    this.Hide();
                    HauptMenueFenster hauptMenueFenster = new HauptMenueFenster();
                    hauptMenueFenster.Show();

                    DatenBankVerbindung.Close();

                }
                else
                {
                    dr.Close();
                    MessageBox.Show("Benutzer oder Passwort falsch");
                    DatenBankVerbindung.Close();
                }
            }
            else
            {
                MessageBox.Show("Bitte fülle alle Felder aus.");
            }

        }

        private void Button_AccountLöschen(object sender, RoutedEventArgs e)
        {
            if (MainWindowTextboxPasswort.Text != "" && MainWindowTextboxName.Text != "")
            {
                DatenBankVerbindung.Open();
                SqlCommand sqlCommand = new SqlCommand("DELETE FROM [dbo].[Table] WHERE Username=@Username AND Passwort=@Passwort", DatenBankVerbindung);
                sqlCommand.Parameters.AddWithValue("@Username", MainWindowTextboxName.Text);
                sqlCommand.Parameters.AddWithValue("@Passwort", MainWindowTextboxPasswort.Text);

                // Führe die Abfrage aus
                int affectedRows = sqlCommand.ExecuteNonQuery();
                DatenBankVerbindung.Close();

                if (affectedRows > 0)
                {
                    MessageBox.Show("Account erfolgreich gelöscht");
                    MainWindowTextboxName.Clear();
                    MainWindowTextboxPasswort.Clear();
                    // Führen Sie hier weitere Aktionen durch, die Sie nach dem Löschen des Kontos ausführen möchten
                }
                else
                {
                    MessageBox.Show("Benutzername oder Passwort falsch");
                }
            }
            else
            {
                MessageBox.Show("Bitte fülle alle Felder aus.");
            }
        }

    }
}
