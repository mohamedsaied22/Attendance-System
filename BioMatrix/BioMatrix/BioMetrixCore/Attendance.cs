using Attendance.Utilities;
using Microsoft.Office.Interop.Excel;
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
    public partial class Attendance : Form
    {
        Panel container;
        Database db = new Database();
        UserDatabase userDB = new UserDatabase();
        User currentUser;
        FP fp = new FP();
        Terminal selectedItem = null;
        BindingSource bindingSource = new BindingSource();
        public Attendance(Panel container, User currentUser)
        {
            InitializeComponent();
            this.container = container;
            this.currentUser = currentUser;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            deviceListView.Items.Clear();
            List<Terminal> terminalList = db.getWorkingTerminalList();

            foreach (Terminal terminal in terminalList)
            {
                //var row = new string[] { terminal.name};
                var lvi = new ListViewItem(new string[] { terminal.name });
                lvi.Tag = terminal;
                deviceListView.Items.Add(lvi);

            }

        }

        private void deviceListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (deviceListView.SelectedItems.Count > 0)
            {
                selectedItem = (Terminal)deviceListView.SelectedItems[0].Tag;
                if (selectedItem != null)
                {
                    panel2.Visible = true;
                    //panel4.Enabled = true;
                    panel3.BackColor = System.Drawing.SystemColors.Window;
                    name.Text = selectedItem.name;
                    type.Text = selectedItem.type;
                    ip.Text = selectedItem.ip;

                    btnPingDevice_Click(null, null);
                    //status.Text = "Not Connected yet";
                }
            }

        }

        private void btnPingDevice_Click(object sender, EventArgs e)
        {
            string ipAddress = ip.Text.Trim();

            bool isValidIpA = UniversalStatic.ValidateIP(ipAddress);
            if (!isValidIpA)
                throw new Exception("The Device IP is invalid !!");

            isValidIpA = UniversalStatic.PingTheDevice(ipAddress);
            if (isValidIpA)
            {
                //panel3.BackColor = Color.GreenYellow;
                status.Text = "The device is active";
                //ShowStatusBar("The device is active", true);
            }

            else
            {
                //panel3.BackColor = Color.Red;
                status.Text = "Could not read any response";
                //ShowStatusBar("Could not read any response", false);
            }

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            operationStatus.Text = "";
            operationStatus.Update();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //ShowStatusBar(string.Empty, true);
                status.Text = "Try to connect.....";
                status.Update();
                bool isDeviceConnected = fp.connectDevice(selectedItem);
                if (isDeviceConnected)
                {
                    userDB.insertLog(selectedItem.name + " is connected", currentUser.id);
                    status.Text = "The device is connected";
                    status.Update();
                    panel3.BackColor = Color.GreenYellow;
                    panel4.Visible = true;
                    panel1.Enabled = false;
                    this.Cursor = Cursors.Default;
                    btnDisconnect.Enabled = true;
                    btnConnect.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                status.Text = ex.Message;
                status.Update();
                panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
                //ShowStatusBar(ex.Message, false);
            }
            this.Cursor = Cursors.Default;
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            userDB.insertLog(selectedItem.name + " is switched off", currentUser.id);
            operationStatus.Text = "";
            operationStatus.Update();
            //dgvRecords.Visible = false;
            //ClearGrid(dgvRecords);
            advancedDataGridView.Visible = true;
            ClearGrid(advancedDataGridView);
            status.Text = "The device is switched off";
            status.Update();
            panel3.BackColor = System.Drawing.SystemColors.Window;
            panel4.Visible = false;
            panel1.Enabled = true;
            this.Cursor = Cursors.Default;
            btnDisconnect.Enabled = false;
            btnConnect.Enabled = true;
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            operationStatus.Text = "";
            operationStatus.Update();
            ICollection<MachineInfo> logs = fp.getLogs();
            if (logs != null)
            {
                operationStatus.Text = "Getting log Data ....";
                operationStatus.Update();
                //dgvRecords.Visible = true;
                advancedDataGridView.Visible = true;
                BindToGridView(fp.getLogs());
                operationStatus.Text = logs.Count + " records found !!";
                userDB.insertLog(selectedItem.name + " get " + logs.Count + " records", currentUser.id);
                exportExcel.Enabled = true;
            }
            else
            {
                operationStatus.Text = "No records found";
            }


        }

        private void ClearGrid(DataGridView dgvRecords)
        {
            if (dgvRecords.Controls.Count > 2)
            { dgvRecords.Controls.RemoveAt(2); }


            dgvRecords.DataSource = null;
            dgvRecords.Controls.Clear();
            dgvRecords.Rows.Clear();
            dgvRecords.Columns.Clear();
        }
        private void BindToGridView(ICollection<MachineInfo> list)
        {

            //ClearGrid(dgvRecords);
            //dgvRecords.DataSource = list;
            //dgvRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //UniversalStatic.ChangeGridProperties(dgvRecords);
            ClearGrid(advancedDataGridView);
            bindingSource.DataSource = list.ToList();
            advancedDataGridView.DataSource = bindingSource;
            advancedDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            UniversalStatic.ChangeGridProperties(advancedDataGridView);
        }



        private void btnDetAll_Click(object sender, EventArgs e)
        {
            operationStatus.Text = "";
            operationStatus.Update();
            //ClearGrid(dgvRecords);
            ClearGrid(advancedDataGridView);
            // dgvRecords.Visible = false;
            advancedDataGridView.Visible = false;
            var resultDia = DialogResult.None;
            resultDia = MessageBox.Show("Do you wish to delete all logs from this Device ??", "Delete Logs", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultDia == DialogResult.Yes)
            {
                try
                {
                    fp.deleteAttLog();
                    operationStatus.Text = "Deleting Logs....";
                    operationStatus.Update();
                    this.Cursor = Cursors.WaitCursor;
                    panel5.Enabled = false;
                }
                catch (Exception ex)
                {
                    operationStatus.Text = ex.Message;
                    operationStatus.Update();
                    this.Cursor = Cursors.Default;
                    panel5.Enabled = true;
                }
                operationStatus.Text = "All logs deleted";
                this.Cursor = Cursors.Default;
                panel5.Enabled = true;
                userDB.insertLog(selectedItem.name + " all logs deleted", currentUser.id);
            }


        }

        private void saveLogs_Click(object sender, EventArgs e)
        {
            operationStatus.Text = "";
            operationStatus.Update();
            //ClearGrid(dgvRecords);
            ClearGrid(advancedDataGridView);
            //dgvRecords.Visible = false;
            advancedDataGridView.Visible = false;
            try
            {
                operationStatus.Text = "Saving logs in database....";
                operationStatus.Update();
                fp.getData(selectedItem);
                this.Cursor = Cursors.WaitCursor;
                panel5.Enabled = false;
            }
            catch (Exception ex)
            {
                operationStatus.Text = ex.Message;
                operationStatus.Update();
                this.Cursor = Cursors.Default;
                panel5.Enabled = true;
            }
            operationStatus.Text = "Logs saved to database";
            userDB.insertLog(selectedItem.name + " all logs saved to database", currentUser.id);
            this.Cursor = Cursors.Default;
            panel5.Enabled = true;
        }

        private void edit_Click(object sender, EventArgs e)
        {
            container.Controls.Clear();
            Console.WriteLine(selectedItem.name);
            Form frm = new AddDevice(selectedItem, "Edit Device", currentUser) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frm.FormBorderStyle = FormBorderStyle.None;
            container.Controls.Add(frm);
            frm.Show();
        }

        private void advancedDataGridView_SortStringChanged(object sender, EventArgs e)
        {
            Console.WriteLine("SortString : " + this.advancedDataGridView.SortString);
            this.bindingSource.Sort = this.advancedDataGridView.SortString;
            advancedDataGridView.DataSource = this.bindingSource;
        }

        private void advancedDataGridView_FilterStringChanged(object sender, EventArgs e)
        {
            this.bindingSource.Filter = this.advancedDataGridView.FilterString;
            advancedDataGridView.DataSource = this.bindingSource;
        }



        private void exportExcel_Click(object sender, EventArgs e)
        {
            //exportGridViewToExcel();
            export();
        }

        private void export()
        {
            operationStatus.Text = "Exporting Data ....";
            operationStatus.Update();
            //string filename = Export.toExcel(advancedDataGridView,name.Text);
            string filename = Export.toCSV(advancedDataGridView, name.Text);
            if (filename != "")
            {
                operationStatus.Text = "Data Exported to " + filename;
                operationStatus.Update();
                userDB.insertLog(name.Text + " has been exported", currentUser.id);
            }
            else
            {
                operationStatus.Text = "";
                operationStatus.Update();
            }

        }
    }
}
