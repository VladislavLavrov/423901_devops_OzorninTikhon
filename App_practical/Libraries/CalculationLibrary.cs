using App_practical.Models;

namespace App_practical.Libraries
{
    public class CalculationLibrary
    {
        public static double? Calculate(double value1, double value2, char operation)
        {
            switch (operation)
            {
                case '+':
                    return value1 + value2;
                case '-':
                    return value1 - value2;
                case '*':
                    return value1 * value2;
                case '/':
                    if (value2 == 0.0)
                    {
                        return null;
                    }
                    return value1 / value2;
                case '^':
                    return Math.Pow(value1, value2);
                default:
                    return null;
            }
        }
    }
}
