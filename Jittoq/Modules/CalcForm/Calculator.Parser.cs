using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Calculator
{
    internal partial class CalculatorParser
    {
        public CalculatorParser() : base(null) { }

        public bool Parse(string s)
        {
            byte[] inputBuffer = System.Text.Encoding.Default.GetBytes(s);
            MemoryStream stream = new MemoryStream(inputBuffer);
            this.Scanner = new CalculatorScanner(stream);
            return this.Parse();
        }
    }
    
    public class ExpressionException : Exception
    {
        public ExpressionException() : base() {}
        public ExpressionException( string s ) : base(s) {}
    }
}
