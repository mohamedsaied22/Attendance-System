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
    public partial class Search : Form
    {
        Database db = new Database();
        UserDatabase userDB = new UserDatabase();
        User currentUser;
        string fromDate, toDate;
        public Search(User currentUser)
        {
            InitializeComponent();

            this.currentUser = currentUser;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            operationLabel.Text = "Searching ...";
            operationLabel.Update();
            export.Enabled = false;
            fromDate = from.Value.ToString("yyyy-MM-dd") + " 00:00";
            toDate = to.Value.ToString("yyyy-MM-dd") + " 23:59";
            try
            {
                if (id.Text == "")
                {
                    searchResultGridView.DataSource = db.getAllRecordes(fromDate, toDate).Tables["employeeRecords"];
                    userDB.insertLog("The records were searched during the period from " + fromDate + " to " + toDate, currentUser.id);

                }

                else
                {
                    searchResultGridView.DataSource = db.getRecordesByID(id.Text, fromDate, toDate).Tables["employeeRecords"];
                    userDB.insertLog("The records of " + id.Text + " were searched during the period from " + fromDate + " to " + toDate, currentUser.id);
                }

                export.Visible = true;
                export.Enabled = true;
                operationLabel.Text = ""; 
                operationLabel.Update();
            }
            catch
            {
                MessageBox.Show("No Record Found");
            }

        }

        private void export_Click(object sender, EventArgs e)
        {
            operationLabel.Text = "Exporting ...";
            operationLabel.Update();
            //string filename = Export.toExcel(searchResultGridView, "Records");
            string filename = Export.toCSV(searchResultGridView, "Records");
            if (filename != "")
            {
                operationLabel.Text = "Records from " + fromDate + " to " + toDate + " has been exported to excel file";
                operationLabel.Update();
                userDB.insertLog("Records from " + fromDate + " to " + toDate + " has been exported to excel file", currentUser.id);
            }
            else
            {
                operationLabel.Text = "";
                operationLabel.Update();
            }
        }
    }
}
