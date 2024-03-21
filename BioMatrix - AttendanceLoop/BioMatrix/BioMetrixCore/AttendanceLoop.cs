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
    public partial class AttendanceLoop : Form
    {
        Database db = new Database();
        DeviceManipulator manipulator = new DeviceManipulator();
        public ZkemClient objZkeeper;
        List<MachineStatus> lstMachineStatus = new List<MachineStatus>();

        private string location;
        //private bool isDeviceConnected = false;
        //string ipAddress = "10.3.7.22";

        private void RaiseDeviceEvent(object sender, string actionType)
        {
            switch (actionType)
            {
                case UniversalStatic.acx_Disconnect:
                {
                    break;
                }

                default:
                    break;
            }
        }

        private void saveAttLog(ICollection<MachineInfo> logList, string ipAddress)
        {
            //Utilities.Database db = new Database();


            foreach (var machineInfo in logList)
            {
                //Console.WriteLine(!db.isLogExist(machineInfo.IndRegID, machineInfo.DateTimeRecord, ipAddress));
                if (!db.isLogExist(machineInfo.IndRegID, machineInfo.DateTimeRecord, ipAddress))
                {
                    db.insertLog(machineInfo.IndRegID, machineInfo.DateTimeRecord, ipAddress);
                }
            }
        }

        public AttendanceLoop(string location)
        {
            this.location = location;
            InitializeComponent();
        }

        private void Form1_Shown(Object sender, EventArgs e)
        {
            //Database db = new Database();
            //MessageBox.Show(location);
            db.truncateTerminalFailure();
            start.Text = "Start : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            start.Update();
            foreach (Terminal terminal in db.getWorkingTerminalListOfLocation(location))
            {
                ip.Text = "IP : " + terminal.ip;
                ip.Update();
                name.Text = "Name : " + terminal.name;
                name.Update();
                getData(terminal);
            }

            String filepath = "C:\\AttendanceLogs\\" + location +
                              string.Format("_{0:dd_MM_yyyy_HH_mm_ss}", DateTime.Now) + ".csv";
            Export.SaveToCsv(lstMachineStatus, filepath);
            end.Text = "End : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            end.Update();
            Application.Exit();
            //getDevices();
            //getData();
        }

        public bool connectDevice(Terminal terminal)
        {
            MachineStatus machineStatus = new MachineStatus();
            machineStatus.terminalName = terminal.name;
            machineStatus.terminalIP = terminal.ip;
            machineStatus.date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            bool isDeviceConnected = false;
            try
            {
                bool isValidIpA = UniversalStatic.ValidateIP(terminal.ip);
                if (!isValidIpA)
                    throw new Exception("The Device IP is invalid !!");

                isValidIpA = UniversalStatic.PingTheDevice(terminal.ip);
                if (!isValidIpA)
                    throw new Exception("The device at " + terminal.ip + ":" + terminal.port + " did not respond!!");
                objZkeeper = new ZkemClient(RaiseDeviceEvent);
                //objZkeeper.SetCommPassword(12);
                isDeviceConnected = objZkeeper.Connect_Net(terminal.ip, terminal.port);
                status.Text = "Status : Connecting....";
                status.Update();
                if (isDeviceConnected)
                {
                    status.Text = "Status : Connected";
                    status.Update();
                    string deviceInfo = manipulator.FetchDeviceInfo(objZkeeper, 1);
                    info.Text = deviceInfo;
                    info.Update();
                }
            }
            catch (Exception ex)
            {
                machineStatus.operationStatus = OperationStatus.ConnectionFailed;
                db.insertTerminalFailure(terminal.id, ex.Message);
            }
            finally
            {
                lstMachineStatus.Add(machineStatus);
            }

            return isDeviceConnected;
        }


        public void getData(Terminal terminal)
        {
            /*objZkeeper = new ZkemClient(RaiseDeviceEvent);
            //objZkeeper.SetCommPassword(12);
            isDeviceConnected = objZkeeper.Connect_Net(ipAddress, portNo);
            status.Text = "Status : Connecting....";
            status.Update();*/

            bool isDeviceConnected = connectDevice(terminal);
            if (isDeviceConnected)
            {
                MachineStatus machineStatus = new MachineStatus();
                machineStatus.terminalName = terminal.name;
                machineStatus.terminalIP = terminal.ip;
                machineStatus.date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                try
                {
                    ICollection<MachineInfo> lstMachineInfo = manipulator.GetLogData(objZkeeper, 1);

                    if (lstMachineInfo != null && lstMachineInfo.Count > 0)
                    {
                        //BindToGridView(lstMachineInfo);
                        status.Text = "Status : Saving logs in database....";
                        status.Update();
                        saveAttLog(lstMachineInfo, terminal.ip);
                        //ShowStatusBar(lstMachineInfo.Count + " records found !!", true);
                        machineStatus.operationStatus = OperationStatus.Done;
                    }
                }
                catch (Exception ex)
                {
                    db.insertTerminalFailure(terminal.id, ex.Message);
                    machineStatus.operationStatus = OperationStatus.RetrievingFailed;
                }
                finally
                {
                    lstMachineStatus.Add(machineStatus);
                }
            }
        }
    }
}