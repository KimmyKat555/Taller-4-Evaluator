using Evaluator.Core;
using System.Windows.Forms; 

namespace Evaluator.UI.Windows
{
    public partial class Form1 : Form
    {
        // Constructor de la clase (se ejecuta al iniciar la aplicaci�n)
        public Form1()
        {
            InitializeComponent();
        }

        // =================================================================
        // 1. MANEJADOR PARA N�MEROS, OPERADORES Y PAR�NTESIS
        // Esta funci�n se conectar� a CADA UNO de los botones de n�meros y operadores.
        // =================================================================
        private void InputButton_Click(object sender, EventArgs e)
        {
            // El 'sender' es el bot�n que se presion�.
            Button button = (Button)sender;

            // Agregamos el texto del bot�n (por ejemplo, '7', '+', '(') a la pantalla.
            txtDisplay.Text += button.Text;

            // Aseguramos que el foco est� en la pantalla para poder seguir escribiendo.
            txtDisplay.Focus();
        }

        // =================================================================
        // 2. MANEJADOR PARA EL BOT�N CLEAR (C)
        // Borra toda la expresi�n.
        // =================================================================
        private void btnClear_Click(object sender, EventArgs e)
        {
            // Simplemente vaciamos el texto de la pantalla.
            txtDisplay.Text = "";
            txtDisplay.Focus();
        }

        // =================================================================
        // 3. MANEJADOR PARA EL BOT�N DELETE (Borrar)
        // Borra el �ltimo car�cter ingresado.
        // =================================================================
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Verificamos que haya algo que borrar.
            if (txtDisplay.Text.Length > 0)
            {
                // Tomamos todo el texto excepto el �ltimo car�cter.
                txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);
            }
            txtDisplay.Focus();
        }

        // =================================================================
        // 4. MANEJADOR PARA EL BOT�N IGUAL (=)
        // Este es el m�s importante: llama al Evaluador y muestra el resultado.
        // =================================================================
        private void btnEqual_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Tomamos la expresi�n que el usuario escribi� en la pantalla.
                string expression = txtDisplay.Text;

                // 2. Llamamos a la funci�n de c�lculo del n�cleo que modificamos.
                // Usamos la clase ExpressionEvaluator y su m�todo Evaluate.
                double result = ExpressionEvaluator.Evaluate(expression);

                // 3. Mostramos la expresi�n completa junto con el resultado.
                // Por ejemplo: "4*5/2=10"
                txtDisplay.Text = $"{expression}={result}";
            }
            catch (Exception ex)
            {
                // Si ocurre alg�n error (ej. divisi�n por cero, expresi�n incompleta)
                // mostramos un mensaje de error simple.
                MessageBox.Show($"Error en la expresi�n: {ex.Message}", "Error de C�lculo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDisplay.Text = ""; // Limpiamos la pantalla despu�s del error.
            }
            txtDisplay.Focus();
        }
    }
}