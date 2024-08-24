using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace IMCFinal
{
    public partial class Form1 : Form
    {
        private string connectionString;

        public Form1()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Teste"].ConnectionString;
        }

        private void btnCalcularIMC_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text;
            if (double.TryParse(txtPeso.Text, out double peso) && double.TryParse(txtAltura.Text, out double altura))
            {
                double imc = peso / (altura * altura);
                lblResultado.Text = $"{nome}, seu IMC é {imc:F2}";

                SaveRecord(nome, altura, peso, imc);
            }
            else
            {
                lblResultado.Text = "Por favor, insira valores válidos para peso e altura.";
            }
        }

        private void SaveRecord(string nome, double altura, double peso, double imc)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO IMC (Nome_User, Altura, Peso, IMC) VALUES (@Nome_User,@Altura, @Peso, @IMC)";

                using (var command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Nome_User", nome);
                    command.Parameters.AddWithValue("@Altura", altura);
                    command.Parameters.AddWithValue("@Peso", peso);
                    command.Parameters.AddWithValue("@IMC", imc);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
