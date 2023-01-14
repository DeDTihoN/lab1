using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public class Expression
    {
        private string expr = "";
        private bool calculated;
        private bool is_correct;
        private double value;
        public static Dictionary<string, int> order = new Dictionary<string, int>()
        {
            {"+", 0},
            {"-", 1},
            {"*", 2},
            {"/", 3},
            {"^", 4},
            {"=",5},
            {"<",6},
            {">",7},
            {"not", 8},
            {"or",9},
            {"and",10 }
        };

        public string Expr { get => expr; set => expr = value; }
        public bool Calculated { get => calculated; set => calculated = value; }
        public bool Is_correct { get => is_correct; set => is_correct = value; }
        public double Value { get => value; set => this.value = value; }

        public Expression(string expr, bool calculated, bool is_correct, int value, bool is_logic)
        {
            this.Expr = expr;
            this.Calculated = calculated;
            this.Is_correct = is_correct;
            this.Value = value;
            if (expr == null) this.Expr = "0";
        }
        public Expression(string expr)
        {
            this.Expr = expr;
            this.Calculated = false;
            this.Is_correct = false;
            this.Value = 0;
            if (expr == null) this.Expr = "0";
        }
        public Expression()
        {
            this.Expr = "";
            this.Calculated = false;
            this.Is_correct = false;
            this.Value = 0;
        }
        public bool ValueIsLogic()
        {
            if (Value==0 || Value==1) return true;
            return false;
        }
        public void SetValidValue(double value)
        {
            Calculated= true;
            Is_correct = true;
            this.Value =value;
            return;
        }
        public void CopyResult (Expression ex)
        {
            Is_correct = ex.Is_correct;
            Value = ex.Value;
        }
    }
}
