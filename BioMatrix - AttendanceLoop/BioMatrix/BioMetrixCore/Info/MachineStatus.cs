using System;

namespace Attendance
{
    public class MachineStatus
    {
        public string terminalName { get; set; }
        public string terminalIP { get; set; }

        public string date { get; set; }

        public OperationStatus operationStatus { get; set; }
    }
}