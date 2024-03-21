using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Utilities
{
    class FP
    {
        DeviceManipulator manipulator = new DeviceManipulator();
        public ZkemClient objZkeeper;
        //private bool isDeviceConnected = false;
        Database db = new Database();
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
                    Console.WriteLine("inserted");
                    db.insertLog(machineInfo.IndRegID, machineInfo.DateTimeRecord, ipAddress);

                }



            }
        }

        
        public bool connectDevice(Terminal terminal)
        {
            bool isDeviceConnected = false;

            bool isValidIpA = UniversalStatic.ValidateIP(terminal.ip);
            if (!isValidIpA)
                throw new Exception("The Device IP is invalid !!");

            isValidIpA = UniversalStatic.PingTheDevice(terminal.ip);
            Console.WriteLine(isValidIpA);
            if (!isValidIpA)
                throw new Exception("The device at " + terminal.ip + ":" + terminal.port + " did not respond!!");
            objZkeeper = new ZkemClient(RaiseDeviceEvent);
            objZkeeper.SetCommPassword(0);
            isDeviceConnected = objZkeeper.Connect_Net(terminal.ip, terminal.port);
            //status.Text = "Status : Connecting....";
            //status.Update();
            if (isDeviceConnected)
            {
                //status.Text = "Status : Connected";
                //status.Update();
                string deviceInfo = manipulator.FetchDeviceInfo(objZkeeper, 1);
                //info.Text = deviceInfo;
                //info.Update();
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

            //bool isDeviceConnected = connectDevice(terminal);
            //if (isDeviceConnected)
           // {
                /*status.Text = "Status : Connected";
                status.Update();
                string deviceInfo = manipulator.FetchDeviceInfo(objZkeeper, 1);
                info.Text = deviceInfo;
                info.Update();*/

                //lblDeviceInfo.Text = deviceInfo;


                ICollection<MachineInfo> lstMachineInfo = manipulator.GetLogData(objZkeeper, 1);

                if (lstMachineInfo != null && lstMachineInfo.Count > 0)
                {
                    //BindToGridView(lstMachineInfo);
                    //status.Text = "Status : Saving logs in database....";
                    //status.Update();
                    saveAttLog(lstMachineInfo, terminal.ip);
                    //ShowStatusBar(lstMachineInfo.Count + " records found !!", true);
                }


            //}
        }

        public void extractDataToExcel()
        {

        }

        public void deleteAttLog()
        {
            objZkeeper.ClearGLog(1);
        }

        public ICollection<MachineInfo> getLogs()
        {
            ICollection<MachineInfo> lstMachineInfo = manipulator.GetLogData(objZkeeper, 1);

            if (lstMachineInfo != null && lstMachineInfo.Count > 0)
            {
                return lstMachineInfo;

            }
            else
            {
                return null;
            }
        }
    }
}
