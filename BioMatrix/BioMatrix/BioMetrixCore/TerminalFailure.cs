using Attendance.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Attendance
{
    public partial class TerminalFailure : Form
    {
        public TerminalFailure()
        {
            InitializeComponent();
        }

        private void ClearGrid()
        {
            if (failureGridView.Controls.Count > 2)
            { failureGridView.Controls.RemoveAt(2); }


            failureGridView.DataSource = null;
            failureGridView.Controls.Clear();
            failureGridView.Rows.Clear();
            failureGridView.Columns.Clear();
        }
        private void BindToGridView(object list)
        {
            ClearGrid();
            failureGridView.DataSource = list;
            failureGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            UniversalStatic.ChangeGridProperties(failureGridView);
        }

        private void TerminalFailure_Load(object sender, EventArgs e)
        {
            try
            {
                Database db = new Database();
                failureGridView.DataSource = db.getTerminalFailureDataSet().Tables["terminal_failure"];
            }
            catch
            {
                MessageBox.Show("No Record Found");
            }
        }
    }
}
