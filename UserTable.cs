using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1
{
    public class UserTable
    {
        int n, m;
        DataTable Data = new DataTable();
        List<List<Expression>> list = new List<List<Expression>>();
        public List<List<Expression>> List { get => list;set=> list = value; }
        public int N { get => n; set => n = value; }
        public int M { get => m; set => m = value; }
        public DataTable Data1 { get => Data; set => Data = value; }

        public UserTable()
        {
            InitData(0, 0);
        }
        public UserTable(int _n, int _m)
        {
            InitData(_n, _m);
        }
        void clear()
        {
            N = 0;
            M = 0;
            list.Clear();
            Data1.Clear();
            Data1 = new DataTable();
            list = new List<List<Expression>>();
        }
        public void InitData(int _n,int _m)
        {
            clear();
            N = _n;
            M = _m;
            Data1.Columns.Add($" ", typeof(string));
            for (int i = 1; i <= M; ++i)
            {
                Data1.Columns.Add($"{i}", typeof(string));
            }
            for (int i = 0; i < N; ++i)
            {
                string[] ex = new string[M + 1];
                List<Expression> row = new List<Expression>();
                for (int j = 0; j <= M; ++j)
                {
                    ex[j] = "0";
                    row.Add(new Expression("0"));
                }
                Data1.Rows.Add(ex);
                Data1.Rows[i][0] = Convert.ToString(Convert.ToChar(Convert.ToInt64(Convert.ToInt32('A') + i)));
                list.Add(row);
            }

            Data1.Columns[0].ReadOnly = true;
        }
        private void update_expression(Expression ex,string s)
        {
            Expression new_ex = new Expression(s);
            calculate(new_ex);
            ex.CopyResult(new_ex);
            return;
        }

        public void RewriteTable()
        {
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    list[i][j] = new Expression(Data1.Rows[i][j+1] as string);
                }
            }
            for (int i = 0; i < N; ++i)
            {
                for (int j=0;j<M; ++j)
                {
                    calculate(list[i][j]);
                }
            }
        }

        public string WriteCell(int i,int j)
        {
            calculate(list[i][j]);
            if (list[i][j].Is_correct && list[i][j].ValueIsLogic())
            {
                return Convert.ToString(list[i][j].Value);
            }
            else
            {
                return "N/A";
            }
        }
        public Expression GetStringExpression(string s)
        {
            Expression StringExpr = new Expression(s);
            calculate(StringExpr);
            return StringExpr;
        }
        private bool If_Expression(Expression ex)
        {
            calculate(ex);
            if (ex.Is_correct) return true;
            else return false;
        }
        public Tuple<int,int> TryParseCell(string s)
        {
            int cnt = 0;
            for (int i = 0; i < s.Length; ++i)
            {
                if (s[i] != ' ' && s[i] != '\0') ++cnt;
            }
            if (cnt == 2)
            {
                for (int i = 0; i < s.Length - 1; ++i)
                {
                    if (s[i]>='A' && s[i]<='Z' && s[i+1]>='0' && s[i + 1] <= '9')
                    {
                        return Tuple.Create(Convert.ToInt32(s[i]-'A'), Convert.ToInt32(s[i+1]-'0')-1);
                    }
                }
                return Tuple.Create(-1,-1);
            }
            else if (cnt==3)
            {
                for (int i = 0; i < s.Length - 2; ++i)
                {
                    if (s[i] >= 'A' && s[i] <= 'Z' && s[i + 1] >= '0' && s[i + 1] <= '9' && s[i+2]>='0' && s[i+2]<='9')
                    {
                        return Tuple.Create(Convert.ToInt32(s[i] - 'A'), Convert.ToInt32(s[i + 1] - '0') * 10 + Convert.ToInt32(s[i+2]-'0')-1);
                    }
                }
                return Tuple.Create(-1, -1);
            }
            else
            {
                return Tuple.Create(-1,-1);
            }
        }
        public void calculate(Expression ex)
        {
            if (ex.Calculated)
            {
                return;
            }

            ex.Calculated = true;

            string CurrentExxpresion = ex.Expr;


            #region finding()ranges
            int LastPosMark = -1;
            for (int i = 0; i < CurrentExxpresion.Length; ++i)
            {
                if (CurrentExxpresion[i] == '(')
                {
                    LastPosMark = i;
                }
                else if (CurrentExxpresion[i] == ')')
                {
                    if (LastPosMark == -1)
                    {
                        ex.Is_correct = false;
                        return;
                    }
                    else
                    {
                        if (LastPosMark == i - 1)
                        {
                            ex.Is_correct = false;
                            return;
                        }
                        else
                        {
                            Expression InsideExpression = new Expression();
                            for (int j = LastPosMark + 1; j < i; ++j)
                            {
                                InsideExpression.Expr += CurrentExxpresion[j];
                            }
                            if (If_Expression(InsideExpression))
                            {
                                string NewExpression = "";
                                for (int j = 0; j < LastPosMark; ++j) NewExpression += CurrentExxpresion[j];
                                NewExpression += Convert.ToString(InsideExpression.Value);
                                for (int j= i + 1; j < CurrentExxpresion.Length; ++j)NewExpression+= CurrentExxpresion[j];
                                update_expression(ex, NewExpression);
                                return;
                            }
                            else
                            {
                                ex.Is_correct = false;
                                return;
                            }
                        }
                    }
                }
            }
            #endregion


            #region finding operations

            int ind = -1, len = -1, priority = -1;
            for (int i = CurrentExxpresion.Length-1; i>=0; --i)
            {
                if (Expression.order.ContainsKey(CurrentExxpresion.Substring(i, 1)))
                {
                    if (Expression.order[CurrentExxpresion.Substring(i, 1)] > priority)
                    {
                        ind = i;
                        len = 1;
                        priority = Expression.order[CurrentExxpresion.Substring(i, 1)];
                    }
                }
            }

            for (int i = CurrentExxpresion.Length-2; i>=0; --i)
            {
                if (Expression.order.ContainsKey(CurrentExxpresion.Substring(i, 2)))
                {
                    if (Expression.order[CurrentExxpresion.Substring(i, 2)] > priority)
                    {
                        ind = i;
                        len = 2;
                        priority = Expression.order[CurrentExxpresion.Substring(i, 2)];
                    }
                }
            }

            for (int i = CurrentExxpresion.Length-3; i>=0; --i)
            {
                if (Expression.order.ContainsKey(CurrentExxpresion.Substring(i, 3)))
                {
                    if (Expression.order[CurrentExxpresion.Substring(i, 3)] > priority)
                    {
                        ind = i;
                        len = 3;
                        priority = Expression.order[CurrentExxpresion.Substring(i, 3)];
                    }
                }
            }

            if (len != -1)
            {
                if (CurrentExxpresion.Substring(ind, len) == "not") {
                    Expression Right = new Expression();
                    for (int j = ind + len; j < CurrentExxpresion.Length; ++j) Right.Expr += CurrentExxpresion[j];
                    if (If_Expression(Right)){
                        if (Right.ValueIsLogic())
                        {
                            int NotVal;
                            if (Right.Value == 0)
                            {
                                NotVal = 1;
                            }
                            else
                            {
                                NotVal = 0;
                            }
                            string NewExpr = "";
                            for (int j=0;j<ind;++j)NewExpr+= CurrentExxpresion[j];
                            NewExpr += " ";
                            NewExpr += Convert.ToString(NotVal);
                            update_expression(ex, NewExpr);
                            return;
                        }
                        else
                        {
                            ex.Is_correct = false;
                            return;
                        }
                    }
                    else
                    {
                        ex.Is_correct = false;
                        return;
                    }
                    return;
                }
                else
                {
                    Expression Left = new Expression(), Right = new Expression();
                    for (int i = 0; i < ind; ++i) Left.Expr += CurrentExxpresion[i];
                    for (int i = ind + len; i < CurrentExxpresion.Length; ++i) Right.Expr += CurrentExxpresion[i];
                    calculate(Left);
                    calculate(Right);
                    if (Left.Is_correct && Right.Is_correct)
                    {
                        if (CurrentExxpresion.Substring(ind, len) == "+"){
                            ex.SetValidValue(Left.Value + Right.Value);
                            return;
                        }
                        else if (CurrentExxpresion.Substring(ind, len) == "-"){
                            ex.SetValidValue(Left.Value - Right.Value);
                            return;
                        }
                        else if (CurrentExxpresion.Substring(ind,len) == "*") {
                            ex.SetValidValue(Left.Value * Right.Value);
                            return;
                        }
                        else if (CurrentExxpresion.Substring(ind, len) == "/"){
                            if (Right.Value == 0)
                            {
                                ex.Is_correct = false;
                            }
                            else
                            {
                                ex.SetValidValue(Left.Value/ Right.Value);
                            }
                            return;
                        }
                        else if (CurrentExxpresion.Substring(ind, len) == "^"){
                            ex.SetValidValue(Math.Pow(Left.Value,Right.Value));
                            return;
                        }
                        else if (CurrentExxpresion.Substring(ind,len) == "=")
                        {
                            ex.SetValidValue(Convert.ToDouble(Left.Value==Right.Value));
                            return;
                        }
                        else if (CurrentExxpresion.Substring(ind,len) == "<")
                        {
                            ex.SetValidValue(Convert.ToDouble(Left.Value<Right.Value));
                            return;
                        }
                        else if (CurrentExxpresion.Substring(ind,len) == ">")
                        {
                            ex.SetValidValue(Convert.ToDouble(Left.Value>Right.Value));
                            return;
                        }
                        else if (Left.ValueIsLogic() && Right.ValueIsLogic()) {
                            if (CurrentExxpresion.Substring(ind, len) == "or")
                            {
                                ex.SetValidValue(Convert.ToDouble(Left.Value == 1 || Right.Value == 1));
                                return;
                            }
                            else if (CurrentExxpresion.Substring(ind, len) == "and")
                            {
                                ex.SetValidValue(Convert.ToDouble(Left.Value==1 && Right.Value == 1));
                                return;
                            }
                            throw new Exception("Не знайдено операцію");
                        }
                        else
                        {
                            ex.Is_correct=false;
                            return;
                        }
                    }
                    else
                    {
                        ex.Is_correct = false;
                        return;
                    }
                }
            }

            #endregion

            #region single value
            double DoubleRes;
            if (double.TryParse(CurrentExxpresion, out DoubleRes))
            {
                ex.SetValidValue(DoubleRes);
                return;
            }
            Tuple<int,int> cell = TryParseCell(CurrentExxpresion);
            if (cell.Item1 != -1)
            {
                if (cell.Item1 >= 0 && cell.Item1 < N && cell.Item2 >= 0 && cell.Item2 < M) {
                    calculate(list[cell.Item1][cell.Item2]);
                    if (list[cell.Item1][cell.Item2].ValueIsLogic())
                    {
                        ex.CopyResult(list[cell.Item1][cell.Item2]);
                    }
                    else
                    {
                        ex.Is_correct = false;
                    }
                    return;
                }
                else
                {
                    ex.Is_correct = false;
                    return;
                }
            }
            else
            {
                ex.Is_correct = false;
                return;
            }
            #endregion
        }
    }
}
