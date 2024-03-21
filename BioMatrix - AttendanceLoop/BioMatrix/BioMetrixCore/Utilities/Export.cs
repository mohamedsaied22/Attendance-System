using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Attendance.Utilities
{
    class Export
    {
        public static string toExcel(DataGridView dataGridView, string name)
        {
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();

            Workbook NewWorkbook = ExcelApp.Workbooks.Add(XlSheetType.xlWorksheet);

            Worksheet NewWorksheet = (Worksheet)ExcelApp.ActiveSheet;

            //ExcelApp.Visible = true;

            for (int i = 1; i < dataGridView.Columns.Count + 1; i++)

            {
                NewWorksheet.Cells[1, i] = dataGridView.Columns[i - 1].HeaderText;
            }

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView.Columns.Count; j++)
                {
                    NewWorksheet.Cells[i + 2, j + 1] = dataGridView.Rows[i].Cells[j].Value.ToString();
                }
            }

            NewWorksheet.Columns.AutoFit();
            //String filename = "C:\\Attendance\\" + name.Text + string.Format("{0:yyyy-MM-dd_HH-mm-ss-fff}", DateTime.Now) + ".xlsx";
            String filename = "C:\\Attendance\\" + name + ".xlsx";
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = "C:\\";
            saveFileDialog1.Title = "Save Excel Files";
            //saveFileDialog1.CheckFileExists = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "xlsx";
            saveFileDialog1.FileName = filename;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = saveFileDialog1.FileName;
                NewWorkbook.SaveAs(filename);
            }
            //String filename = "C:\\Attendance\\" + name.Text + string.Format("{0:yyyy-MM-dd_HH-mm-ss-fff}", DateTime.Now) + ".xlsx";
            //String filename = "C:\\Attendance\\" + name.Text  + ".xlsx";
            //NewWorkbook.SaveAs(filename, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, true, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
            //object misValue = System.Reflection.Missing.Value;
            //NewWorkbook.SaveAs(filename, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, misValue,
            //   misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);


            ExcelApp.Quit();
            return filename;
        }

        public static void toExcelWithoutDialog(List<MachineStatus> lstMachineStatus, string name)
        {
            var bindingList = new BindingList<MachineStatus>(lstMachineStatus);
            var source = new BindingSource(bindingList, null);
            DataGridView dataGridView = new DataGridView();
            dataGridView.DataSource = source;
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();

            Workbook NewWorkbook = ExcelApp.Workbooks.Add(XlSheetType.xlWorksheet);

            Worksheet NewWorksheet = (Worksheet)ExcelApp.ActiveSheet;

            //ExcelApp.Visible = true;

            for (int i = 1; i < dataGridView.Columns.Count + 1; i++)

            {
                NewWorksheet.Cells[1, i] = dataGridView.Columns[i - 1].HeaderText;
            }

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView.Columns.Count; j++)
                {
                    NewWorksheet.Cells[i + 2, j + 1] = dataGridView.Rows[i].Cells[j].Value.ToString();
                }
            }

            NewWorksheet.Columns.AutoFit();
            //String filename = "C:\\Attendance\\" + name.Text + string.Format("{0:yyyy-MM-dd_HH-mm-ss-fff}", DateTime.Now) + ".xlsx";
            /*String filename = "C:\\Attendance\\" + name + ".xlsx";
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = "C:\\";
            saveFileDialog1.Title = "Save Excel Files";
            //saveFileDialog1.CheckFileExists = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "xlsx";
            saveFileDialog1.FileName = filename;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = saveFileDialog1.FileName;
                NewWorkbook.SaveAs(filename);

            }*/
            String filename = "C:\\AttendanceLogs\\" + name +
                              string.Format("{0:yyyy-MM-dd_HH-mm-ss-fff}", DateTime.Now) + ".xlsx";
            //String filename = "C:\\Attendance\\" + name.Text  + ".xlsx";
            NewWorkbook.SaveAs(filename, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing,
                Type.Missing, true, false, XlSaveAsAccessMode.xlNoChange,
                XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
            object misValue = System.Reflection.Missing.Value;
            NewWorkbook.SaveAs(filename, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, misValue,
                misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue,
                misValue, misValue, misValue, misValue);


            ExcelApp.Quit();
            //return filename;
        }


        public static string toCSV(DataGridView dataGridView, string name)
        {
            if (dataGridView.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                //sfd.FileName = "Output.csv";
                sfd.FileName = "C:\\Attendance\\" + name + ".csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }

                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = dataGridView.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[dataGridView.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += dataGridView.Columns[i].HeaderText.ToString() + ",";
                            }

                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < dataGridView.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += dataGridView.Rows[i - 1].Cells[j].Value.ToString() + ",";
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                            MessageBox.Show("Data Exported Successfully !!!", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }

                    return sfd.FileName;
                }

                return "";
            }
            else
            {
                MessageBox.Show("No Record To Export !!!", "Info");
                return "";
            }
        }
        public static void SaveToCsv<T>(List<T> reportData, string path)
        {
            var lines = new List<string>();
            IEnumerable<PropertyDescriptor> props = TypeDescriptor.GetProperties(typeof(T)).OfType<PropertyDescriptor>();
            var header = string.Join(",", props.ToList().Select(x => x.Name));
            lines.Add(header);
            var valueLines = reportData.Select(row => string.Join(",", header.Split(',').Select(a => row.GetType().GetProperty(a).GetValue(row, null))));
            lines.AddRange(valueLines);
            File.WriteAllLines(path, lines.ToArray());
        }
        public static string toCSVWithoutDialog(List<MachineStatus> lstMachineStatus, string name)
        {
            /*var bindingList = new BindingList<MachineStatus>(lstMachineStatus);
            var source = new BindingSource(bindingList, null);*/
            MessageBox.Show(lstMachineStatus.Count.ToString());
            DataGridView dataGridView = new DataGridView();
            dataGridView.DataSource = lstMachineStatus;
            String filename = "C:\\AttendanceLogs\\" + name +
                              string.Format("{0:yyyy-MM-dd_HH-mm-ss-fff}", DateTime.Now) + ".csv";

            MessageBox.Show(dataGridView.RowCount.ToString());
            if (dataGridView.Rows.Count > 0)
            {
                try
                {
                    int columnCount = dataGridView.Columns.Count;
                    string columnNames = "";
                    string[] outputCsv = new string[dataGridView.Rows.Count + 1];
                    for (int i = 0; i < columnCount; i++)
                    {
                        columnNames += dataGridView.Columns[i].HeaderText.ToString() + ",";
                    }

                    outputCsv[0] += columnNames;

                    for (int i = 1; (i - 1) < dataGridView.Rows.Count; i++)
                    {
                        for (int j = 0; j < columnCount; j++)
                        {
                            outputCsv[i] += dataGridView.Rows[i - 1].Cells[j].Value.ToString() + ",";
                        }
                    }

                    File.WriteAllLines(filename, outputCsv, Encoding.UTF8);
                    //MessageBox.Show("Data Exported Successfully !!!", "Info");
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Error :" + ex.Message);
                    Console.WriteLine("Error :" + ex.Message);
                }
            }

            return filename;
        }
    }
}