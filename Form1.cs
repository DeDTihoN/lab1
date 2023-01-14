using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1
{
    public partial class Form1 : Form
    {
        public static Form1 selfref { get; set; }
        public Form1()
        {
            selfref = this;
            InitializeComponent();
        }

        int n, m;
        int SelI = -1, SelJ = -1;
        UserTable table = new UserTable();
        UserTable ActualTable = new UserTable();
        private void button1_Click(object sender, EventArgs e)
        {
            Form ifrm = new Form3();
            ifrm.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form ifrm = new Form2();
            ifrm.Show();
        }

        public void clearTable()
        {
            while (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows.Remove(dataGridView1.Rows[dataGridView1.Rows.Count - 1]);
            }
            while(dataGridView1.Columns.Count != 0)
            {
                dataGridView1.Columns.Remove(dataGridView1.Columns[dataGridView1.Columns.Count - 1]);
            }
            table = new UserTable();
            ActualTable = new UserTable();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex; int j = e.ColumnIndex;
            ActualTable.Data1.Rows[i][j] = table.Data1.Rows[i][j];
            ActualTable.RewriteTable();
            for (int i1 = 0; i1 < n; ++i1)
            {
                for (int j1 = 0; j1 < m; ++j1)
                {
                    if (i == i1 && j == j1+1) continue;
                    table.Data1.Rows[i1][j1+1] = ActualTable.WriteCell(i1, j1);
                }
            }
            return;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex, j = e.ColumnIndex;
            if (i < 0) return;
            if (SelI >= 0 && SelJ > 0 && !(i == SelI && j == SelJ))
            {
                table.Data1.Rows[SelI][SelJ] = ActualTable.WriteCell(SelI, SelJ - 1);
            }
            else if (i == SelI && j == SelJ) return;
            SelI = i;
            SelJ = j;
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
            dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[j];
            if (i < 0 || j <= 0) return;
            table.Data1.Rows[i][j] = ActualTable.Data1.Rows[i][j];
            return;
        }

        public void BuildData(int NumberOfRows, int NumberOfColumns)
        {
            if (NumberOfRows<0 || NumberOfColumns < 0)
            {
                MessageBox.Show("Введіть коректні розміри таблиці");
                return;
            }
            n = NumberOfRows;
            m = NumberOfColumns;

            table.InitData(n, m);
            ActualTable.InitData(n, m);

            dataGridView1.DataSource = table.Data1;
            dataGridView1.AllowUserToAddRows = false;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode=DataGridViewColumnSortMode.NotSortable;
            }
        }

    }
}
