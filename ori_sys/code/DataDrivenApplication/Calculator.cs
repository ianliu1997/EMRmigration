using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace DataDrivenApplication
{

    public delegate double Calculate(double Num1,double Num2);

    public class Calculator
    {
        Calculate CalMethodPointer;
        private Operations _OperationType;

        public Operations OperationType
        {
            get
            {
                return _OperationType;
            }
            set
            {
                switch (value)
                {
                    case Operations.Add:
                        CalMethodPointer = Add;
                        break;
                    case Operations.Subtract:
                        CalMethodPointer = Subtract;
                        break;
                    case Operations.Multiply:
                        CalMethodPointer = Multiply;
                        break;
                    case Operations.Divide:
                        CalMethodPointer = Divide;
                        break;
                    default:
                        throw new ArgumentException();
                        break;
                }
                _OperationType = value;


            }
        }

        public double Calculate(Operations OperationType, double Num1, double Num2)
        {
            this.OperationType = OperationType;
            return CalMethodPointer(Num1, Num2);
        }


        private double Add(Double Num1, Double Num2)
        {
            return Num1 + Num2;
        }

        private double Subtract(Double Num1, Double Num2)
        {
            return Num1 - Num2;
        }

        private double Multiply(Double Num1, Double Num2)
        {
            return Num1 * Num2;
        }

        private double Divide(Double Num1, Double Num2)
        {
            return Num1 / Num2;
        }
    }

    public enum Operations
    {
        Add = 1,
        Subtract = 2,
        Multiply = 3,
        Divide = 4
    }


    public class MyClass
    {
        public static int HowManyHoursInTheFirstYear(IList<DateTime> samples, Func<DateTime, DateTime, bool> comparer)
        {
            DateTime firstDate = samples[0].Date;
            int count = 0;
            while (count < samples.Count && comparer(samples[count], firstDate))
            {
                count++;
            }
            return count;
        }
    }
}
