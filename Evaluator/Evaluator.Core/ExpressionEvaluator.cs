namespace Evaluator.Core;

public class ExpressionEvaluator
{
    // NO TOCAREMOS ESTA FUNCIÓN POR AHORA, SOLO CAMBIAREMOS LA LÓGICA INTERNA.
    public static double Evaluate(string infix)
    {
        // Primero, creamos una lista de "tokens" (números y operadores) a partir de la expresión.
        var tokens = Tokenize(infix);

        // A partir de los tokens, generamos la notación postfija.
        var postfixTokens = InfixToPostfix(tokens);

        // Y finalmente, calculamos el resultado usando la notación postfija.
        return Calulate(postfixTokens);
    }

    // ====================================================================
    // 💡 NUEVO MÉTODO: Tokenize 
    // Separa la expresión en números completos y operadores/paréntesis.
    // Esto resuelve el problema de los números de múltiples dígitos y decimales.
    // ====================================================================
    private static List<string> Tokenize(string infix)
    {
        var tokens = new List<string>();
        var currentToken = ""; // Almacenará los dígitos de un número antes de ser completado.

        foreach (char c in infix)
        {
            // Si el caracter es un dígito o un punto decimal, forma parte del número actual.
            if (char.IsDigit(c) || c == '.')
            {
                currentToken += c;
            }
            // Si el caracter es un operador o paréntesis
            else if (IsOperator(c))
            {
                // Si ya teníamos un número (currentToken) almacenado, lo guardamos primero como un token completo.
                if (!string.IsNullOrEmpty(currentToken))
                {
                    tokens.Add(currentToken);
                    currentToken = ""; // Reiniciamos para el siguiente número.
                }

                // Guardamos el operador/paréntesis como un token separado.
                tokens.Add(c.ToString());
            }
            // Manejar cualquier otro caracter no reconocido (opcional, pero buena práctica)
            else if (!char.IsWhiteSpace(c))
            {
                throw new Exception($"Carácter no válido encontrado: {c}");
            }
        }

        // Si después de terminar el bucle queda un número sin guardar, lo añadimos.
        if (!string.IsNullOrEmpty(currentToken))
        {
            tokens.Add(currentToken);
        }

        return tokens;
    }

    // ====================================================================
    // 🔄 MODIFICACIÓN: InfixToPostfix
    // Ahora recibe una LISTA de strings (tokens) en lugar de un string de caracteres.
    // ====================================================================
    private static List<string> InfixToPostfix(List<string> infixTokens)
    {
        // Usaremos una Pila (Stack) para manejar los operadores y paréntesis,
        // ya que la conversión a postfija tradicional se basa en el principio LIFO (Pila).
        var stack = new Stack<string>();

        // Usaremos una LISTA (que actúa como la 'cola' de salida) para almacenar la expresión postfija.
        // Aunque la nota pide usar 'colas' (Queue), la lógica postfija se construye en orden (FIFO de salida), 
        // así que una lista es más simple y cumple el espíritu de ir añadiendo la expresión en orden.
        var postfixTokens = new List<string>();

        foreach (string item in infixTokens)
        {
            // Verificamos si el token es un operador (o paréntesis) o un número.
            // Si no es un operador, asumimos que es un número (o decimal) y lo añadimos a la salida.
            if (!IsOperator(item[0])) // item[0] porque ahora item es un string
            {
                postfixTokens.Add(item);
            }
            else // Es un operador o paréntesis
            {
                char op = item[0];
                if (op == '(')
                {
                    stack.Push(item); // Paréntesis de apertura siempre va a la pila.
                }
                else if (op == ')')
                {
                    // Sacamos operadores de la pila hasta encontrar el '('
                    while (stack.Count > 0 && stack.Peek() != "(")
                    {
                        postfixTokens.Add(stack.Pop());
                    }
                    if (stack.Count > 0 && stack.Peek() == "(")
                    {
                        stack.Pop(); // Descartamos el '('
                    }
                    // NOTA: Deberíamos agregar manejo de error si stack.Count es 0 y nunca encontramos '('.
                }
                else // Es un operador (+, -, *, /, ^)
                {
                    // Mientras la pila no esté vacía y la prioridad del operador en la pila
                    // sea mayor o igual a la prioridad del operador actual,
                    // sacamos de la pila y lo agregamos a la salida.
                    while (stack.Count > 0 && PriorityStack(stack.Peek()[0]) >= PriorityInfix(op))
                    {
                        postfixTokens.Add(stack.Pop());
                    }
                    stack.Push(item); // Finalmente, ponemos el operador actual en la pila.
                }
            }
        }

        // Vaciamos los operadores restantes de la pila a la salida.
        while (stack.Count > 0)
        {
            postfixTokens.Add(stack.Pop());
        }

        // La expresión postfija ya no es un solo string, sino una lista de tokens.
        return postfixTokens;
    }

    // ====================================================================
    // 🧮 MODIFICACIÓN: Calulate
    // Ahora recibe una LISTA de strings (tokens) postfijos.
    // ====================================================================
    private static double Calulate(List<string> postfixTokens)
    {
        // Usamos una Pila (Stack) de doubles para la evaluación, ya que la notación postfija
        // requiere el principio LIFO (sacar los dos últimos operandos).
        var stack = new Stack<double>();

        foreach (string item in postfixTokens)
        {
            // Verificamos si el token es un operador
            if (IsOperator(item[0]))
            {
                // Es un operador: Sacamos los dos últimos números (op2 y op1)
                var op2 = stack.Pop();
                var op1 = stack.Pop();

                // Calculamos el resultado de la operación
                stack.Push(Calulate(op1, item[0], op2));
            }
            else
            {
                // Es un número: Lo convertimos de string a double y lo empujamos a la pila.
                stack.Push(Convert.ToDouble(item));
            }
        }
        // El resultado final es el único elemento que queda en la pila.
        return stack.Peek();
    }

    // ====================================================================
    // 🔍 Pequeñas MODIFICACIONES EN MÉTODOS AUXILIARES
    // Adaptamos para que solo reciban el char (que es lo que se evalúa).
    // ====================================================================

    // Función auxiliar para saber si un caracter es un operador.
    private static bool IsOperator(char item) => item is '^' or '/' or '*' or '%' or '+' or '-' or '(' or ')';

    // Se mantiene igual. Da la prioridad del operador cuando viene en la expresión infija.
    private static int PriorityInfix(char op) => op switch
    {
        '^' => 4,
        '*' or '/' or '%' => 2,
        '-' or '+' => 1,
        '(' => 5,
        _ => throw new Exception("Invalid expression."),
    };

    // Se mantiene igual. Da la prioridad del operador cuando ya está DENTRO de la pila.
    private static int PriorityStack(char op) => op switch
    {
        '^' => 3,
        '*' or '/' or '%' => 2,
        '-' or '+' => 1,
        '(' => 0,
        _ => throw new Exception("Invalid expression."),
    };

    // Se mantiene igual. Realiza la operación matemática.
    private static double Calulate(double op1, char item, double op2) => item switch
    {
        '*' => op1 * op2,
        '/' => op1 / op2,
        '^' => Math.Pow(op1, op2),
        '+' => op1 + op2,
        '-' => op1 - op2,
        _ => throw new Exception("Invalid expression."),
    };
}