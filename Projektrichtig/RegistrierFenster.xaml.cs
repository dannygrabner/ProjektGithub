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
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;



namespace Projektrichtig
{
    /// <summary>
    /// Interaktionslogik für RegistrierFenster.xaml
    /// </summary>
    public partial class RegistrierFenster : Window
    {
        private SqlConnection DatenBankVerbindung = new SqlConnection(@"Data Source = (localdb)\MSSQLLocalDB; AttachDBFilename = C:\Users\danie\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\ProjektDatenBank.mdf; Integrated Security = True; Connect Timeout = 30");
        public RegistrierFenster()
        {
            InitializeComponent();

        }

        private void Button_Click_Registrieren(object sender, RoutedEventArgs e)
        {
            if (TextboxName.Text == "" || TextboxPasswort.Text == "" || TextboxPasswortcheck.Text == "")
            {
                MessageBox.Show("Es müssen alle Felder ausgefüllt werden");
                return;
            }

            if (TextboxName != null && TextboxPasswort.Text == TextboxPasswortcheck.Text)
            {
                // Stelle sicher, dass die Verbindung geöffnet ist
                DatenBankVerbindung.Open();

                // Erstelle das SqlCommand-Objekt und setze die Verbindung
                SqlCommand sqlCommand = new SqlCommand("select * from [dbo].[Table] where Username=@Username", DatenBankVerbindung);
                sqlCommand.Parameters.AddWithValue("@Username", TextboxName.Text);

                // Führe die Abfrage aus
                var dr = sqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    MessageBox.Show("Username Existiert schon");
                }
                else
                {
                    dr.Close();
                    sqlCommand = new SqlCommand("insert into [dbo].[Table] values(@Username,@Password)", DatenBankVerbindung);
                    sqlCommand.Parameters.AddWithValue("@Username", TextboxName.Text);
                    sqlCommand.Parameters.AddWithValue("@Password", TextboxPasswort.Text);
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Account würde erstellt");
                    DatenBankVerbindung.Close();
                    TextboxName.Clear();
                    TextboxPasswort.Clear();
                }
            }
            else
            {
                MessageBox.Show("Passwörter stimmen nicht überein");
            }
        }

        private void Button_Click_zurückZumLogin(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }


}