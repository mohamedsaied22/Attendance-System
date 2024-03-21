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
    public partial class Logs : Form
    {
        public Logs()
        {
            InitializeComponent();
        }
        private void ClearGrid()
        {
            if (logsGridView.Controls.Count > 2)
            { logsGridView.Controls.RemoveAt(2); }


            logsGridView.DataSource = null;
            logsGridView.Controls.Clear();
            logsGridView.Rows.Clear();
            logsGridView.Columns.Clear();
        }
        private void BindToGridView(object list)
        {
            ClearGrid();
            logsGridView.DataSource = list;
            logsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            UniversalStatic.ChangeGridProperties(logsGridView);
        }

        private void Logs_Load(object sender, EventArgs e)
        {
            try
            {
                UserDatabase userDB = new UserDatabase();
                logsGridView.DataSource = userDB.getLogsDataSetLast3Months().Tables["logs"];
            }
            catch
            {
                MessageBox.Show("No Record Found");
            }
        }
    }
}
