using System.Security.AccessControl;

namespace RpnLogic
{

    class Token
    {

    }

    class Number : Token
    {
        public double Value { get; }
        public char ValueX { get; }

        public Number(double value)
        {
            Value = value;
        }

        public Number(char valueX)
        {
            ValueX = valueX;
        }

        public static bool CheckX(char valueX)
        {
            return valueX is 'x' or 'X';
        }
    }

    class Operation(char symbol) : Token
    {
        public char Symbol { get; } = symbol;
        public int Priority { get; } = GetPriority(symbol);

        private static int GetPriority(char symbol)
        {
            switch (symbol)
            {
                case '(': return 0;
                case ')': return 0;
                case '+': return 1;
                case '-': return 1;
                case '*': return 2;
                case '/': return 2;
                case '^': return 3;
                default: return 4;
            }
        }

        public static explicit operator char(Operation v)
        {
            throw new NotImplementedException();
        }
    }

    class Paranthesis(char symbol) : Token
    {
        public bool isClosing { get; set; } = symbol == ')';


    }

    public class RpnCalculator
    {
        public readonly double Result;
        public RpnCalculator(string expression)
        {
            List<Token> rpn = ToRpn(Tokenize(expression));
            Result = Calculate(rpn);
        }
        public RpnCalculator(string expression, int varX)
        {
            List<Token> rpn = ToRpn(Tokenize(expression));
            Result = CalculateWithX(rpn, varX);
        }
        private List<Token> Tokenize(string input)
        {
            List<Token> tokens = new List<Token>();
            string number = string.Empty;
            foreach (var c in input)
            {
                if (char.IsDigit(c))
                {
                    number += c;
                }
                else if (c == ',' || c == '.')
                {
                    number += ",";
                }
                else if (c == '+' || c == '-' || c == '*' || c == '/' || c == '^')
                {
                    if (number != string.Empty)
                    {
                        tokens.Add(new Number(double.Parse(number)));
                        number = string.Empty;
                    }
                    tokens.Add(new Operation(c));
                }
                else if (c == '(' || c == ')')
                {
                    if (number != string.Empty)
                    {
                        tokens.Add(new Number(double.Parse(number)));
                        number = string.Empty;
                    }
                    tokens.Add(new Paranthesis(c));
                }
                else if (Number.CheckX(c))
                {
                    tokens.Add(new Number(c));
                }
            }

            if (number != string.Empty)
            {
                tokens.Add(new Number(double.Parse(number)));
            }

            return tokens;
        }


        private static List<Token> ToRpn(List<Token> tokens)
        {
            List<Token> rpnOutput = new List<Token>();
            Stack<Token> operators = new Stack<Token>();
            string number = string.Empty;

            foreach (Token token in tokens)
            {
                if (operators.Count == 0 && !(token is Number))
                {
                    operators.Push(token);
                    continue;
                }

                if (token is Operation)
                {
                    if (operators.Peek() is Paranthesis)
                    {
                        operators.Push(token);
                        continue;
                    }

                    Operation first = (Operation)token;
                    Operation second = (Operation)operators.Peek();

                    if (first.Priority > second.Priority)
                    {
                        operators.Push(token);
                    }
                    else if (first.Priority <= second.Priority)
                    {
                        while (operators.Count > 0 && !(token is Paranthesis))
                        {
                            rpnOutput.Add(operators.Pop());
                        }
                        operators.Push(token);
                    }
                }
                else if (token is Paranthesis paranthesis)
                {
                    if (paranthesis.isClosing)
                    {
                        while (!(operators.Peek() is Paranthesis))
                        {
                            rpnOutput.Add(operators.Pop());
                        }

                        operators.Pop();
                    }
                    else
                    {
                        operators.Push(paranthesis);
                    }
                }
                else if (token is Number num)
                {
                    rpnOutput.Add(num);
                }
            }

            while (operators.Count > 0)
            {
                rpnOutput.Add(operators.Pop());
            }
            return rpnOutput;
        }
        private static double OperationExpress(double first, double second, char operation)
        {
            double result = 0;

            switch (operation)
            {
                case '+':
                    result = first + second;
                    break;
                case '-':
                    result = second - first;
                    break;
                case '*':
                    result = first * second;
                    break;
                case '/':
                    result = second / first;
                    break;
                case '^':
                    result = Math.Pow(second, first);
                    break;
            }

            return result;
        }

        private static double Calculate(List<Token> rpnTokens)
        {
            Stack<double> binCalculator = new Stack<double>();
            double result = 0;
            for (int i = 0; i < rpnTokens.Count; i++)
            {
                if (rpnTokens[i] is Number value)
                {
                    binCalculator.Push(value.Value);
                }
                else
                {
                    double firstNum = binCalculator.Pop();
                    double secondNum = binCalculator.Pop();

                    char oper = (char)(Operation)rpnTokens[i];
                    result = OperationExpress(firstNum, secondNum, oper);
                    binCalculator.Push(result);
                }
            }
            return binCalculator.Peek();
        }

        private static double CalculateWithX(List<Token> rpnCalc, int inputX)
        {
            Stack<double> tempCalc = new Stack<double>();

            foreach (Token token in rpnCalc)
            {
                if (token is Number num)
                {
                    if (Number.CheckX(num.ValueX))
                    {
                        tempCalc.Push(inputX);
                    }
                    else
                    {
                        tempCalc.Push(num.Value);
                    }
                }
                else if (token is Operation)
                {
                    double first = tempCalc.Pop();
                    double second = tempCalc.Pop();
                    var op = (Operation)token;
                    double result = OperationExpress(first, second, op.Symbol);
                    tempCalc.Push(result);
                }
            }

            return tempCalc.Peek();
        }

    }
}