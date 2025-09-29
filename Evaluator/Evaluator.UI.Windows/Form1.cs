using Evaluator.Core;
using System.Windows.Forms; 

namespace Evaluator.UI.Windows
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void InputButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            txtDisplay.Text += button.Text;

            txtDisplay.Focus();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtDisplay.Text = "";
            txtDisplay.Focus();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtDisplay.Text.Length > 0)
            {
                txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);
            }
            txtDisplay.Focus();
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            try
            {
                string expression = txtDisplay.Text;

                double result = ExpressionEvaluator.Evaluate(expression);

                txtDisplay.Text = $"{expression}={result}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la expresión: {ex.Message}", "Error de Cálculo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDisplay.Text = "";
            }
            txtDisplay.Focus();
        }

        private void txtDisplay_TextChanged(object sender, EventArgs e)
        {

        }
    }
}