namespace Evaluator.Core;

public class ExpressionEvaluator
{
    public static double Evaluate(string infix)
    {
        var tokens = Tokenize(infix);

        var postfixTokens = InfixToPostfix(tokens);

        return Calulate(postfixTokens);
    }
    private static List<string> Tokenize(string infix)
    {
        var tokens = new List<string>();
        var currentToken = "";

        foreach (char c in infix)
        {
            if (char.IsDigit(c) || c == '.')
            {
                currentToken += c;
            }
            else if (IsOperator(c))
            {
                if (!string.IsNullOrEmpty(currentToken))
                {
                    tokens.Add(currentToken);
                    currentToken = "";
                }
                tokens.Add(c.ToString());
            }
            else if (!char.IsWhiteSpace(c))
            {
                throw new Exception($"Carácter no válido encontrado: {c}");
            }
        }
        if (!string.IsNullOrEmpty(currentToken))
        {
            tokens.Add(currentToken);
        }

        return tokens;
    }
    private static List<string> InfixToPostfix(List<string> infixTokens)
    {
        var stack = new Stack<string>();

        var postfixTokens = new Queue<string>();

        foreach (string item in infixTokens)
        {
            if (!IsOperator(item[0]))
            {
                postfixTokens.Enqueue(item);
            }
            else
            {
                char op = item[0];
                if (op == '(')
                {
                    stack.Push(item);
                }
                else if (op == ')')
                {
                    while (stack.Count > 0 && stack.Peek() != "(")
                    {
                        postfixTokens.Enqueue(stack.Pop());
                    }
                    if (stack.Count > 0 && stack.Peek() == "(")
                    {
                        stack.Pop();
                    }
                    else
                    {
                        throw new Exception("Error: Paréntesis mal balanceados.");
                    }
                }
                else
                {
                    while (stack.Count > 0 && PriorityStack(stack.Peek()[0]) >= PriorityInfix(op))
                    {
                        postfixTokens.Enqueue(stack.Pop());
                    }
                    stack.Push(item);
                }
            }
        }
        while (stack.Count > 0)
        {
            if (stack.Peek() == "(" || stack.Peek() == ")")
            {
                throw new Exception("Error: Paréntesis mal balanceados.");
            }
            postfixTokens.Enqueue(stack.Pop());
        }

        return postfixTokens.ToList();
    }
    private static double Calulate(List<string> postfixTokens)
    {
        var stack = new Stack<double>();

        foreach (string item in postfixTokens)
        {
            if (IsOperator(item[0]))
            {
                var op2 = stack.Pop();
                var op1 = stack.Pop();

                stack.Push(Calulate(op1, item[0], op2));
            }
            else
            {
                stack.Push(Convert.ToDouble(item));
            }
        }
        return stack.Peek();
    }
    private static bool IsOperator(char item) => item is '^' or '/' or '*' or '%' or '+' or '-' or '(' or ')';

    private static int PriorityInfix(char op) => op switch
    {
        '^' => 4,
        '*' or '/' or '%' => 2,
        '-' or '+' => 1,
        '(' => 5,
        _ => throw new Exception("Invalid expression."),
    };
    private static int PriorityStack(char op) => op switch
    {
        '^' => 3,
        '*' or '/' or '%' => 2,
        '-' or '+' => 1,
        '(' => 0,
        _ => throw new Exception("Invalid expression."),
    };
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