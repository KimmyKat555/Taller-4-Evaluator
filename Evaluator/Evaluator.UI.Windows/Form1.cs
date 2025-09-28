using Evaluator.Core;
using System.Windows.Forms; 

namespace Evaluator.UI.Windows
{
    public partial class Form1 : Form
    {
        // Constructor de la clase (se ejecuta al iniciar la aplicación)
        public Form1()
        {
            InitializeComponent();
        }

        // =================================================================
        // 1. MANEJADOR PARA NÚMEROS, OPERADORES Y PARÉNTESIS
        // Esta función se conectará a CADA UNO de los botones de números y operadores.
        // =================================================================
        private void InputButton_Click(object sender, EventArgs e)
        {
            // El 'sender' es el botón que se presionó.
            Button button = (Button)sender;

            // Agregamos el texto del botón (por ejemplo, '7', '+', '(') a la pantalla.
            txtDisplay.Text += button.Text;

            // Aseguramos que el foco esté en la pantalla para poder seguir escribiendo.
            txtDisplay.Focus();
        }

        // =================================================================
        // 2. MANEJADOR PARA EL BOTÓN CLEAR (C)
        // Borra toda la expresión.
        // =================================================================
        private void btnClear_Click(object sender, EventArgs e)
        {
            // Simplemente vaciamos el texto de la pantalla.
            txtDisplay.Text = "";
            txtDisplay.Focus();
        }

        // =================================================================
        // 3. MANEJADOR PARA EL BOTÓN DELETE (Borrar)
        // Borra el último carácter ingresado.
        // =================================================================
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Verificamos que haya algo que borrar.
            if (txtDisplay.Text.Length > 0)
            {
                // Tomamos todo el texto excepto el último carácter.
                txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);
            }
            txtDisplay.Focus();
        }

        // =================================================================
        // 4. MANEJADOR PARA EL BOTÓN IGUAL (=)
        // Este es el más importante: llama al Evaluador y muestra el resultado.
        // =================================================================
        private void btnEqual_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Tomamos la expresión que el usuario escribió en la pantalla.
                string expression = txtDisplay.Text;

                // 2. Llamamos a la función de cálculo del núcleo que modificamos.
                // Usamos la clase ExpressionEvaluator y su método Evaluate.
                double result = ExpressionEvaluator.Evaluate(expression);

                // 3. Mostramos la expresión completa junto con el resultado.
                // Por ejemplo: "4*5/2=10"
                txtDisplay.Text = $"{expression}={result}";
            }
            catch (Exception ex)
            {
                // Si ocurre algún error (ej. división por cero, expresión incompleta)
                // mostramos un mensaje de error simple.
                MessageBox.Show($"Error en la expresión: {ex.Message}", "Error de Cálculo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDisplay.Text = ""; // Limpiamos la pantalla después del error.
            }
            txtDisplay.Focus();
        }
    }
}