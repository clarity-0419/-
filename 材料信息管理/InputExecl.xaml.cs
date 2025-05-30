﻿using MahApps.Metro.Controls;
using System.Windows;
using Microsoft.Win32;
using OfficeOpenXml;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data.SQLite;
using 培训记录管理软件;
using System.Collections.Generic;

namespace TrainingRecordManager
{
    public partial class InputExecl : MetroWindow
    {
        private readonly DatabaseManager _dbManager = new DatabaseManager();

        //private readonly string[] ExpectedHeaders =
        //{
        //    "姓名", "身份证", "培训时间", "培训地点", "培训单位", "培训内容", "费用", "备注"
        //};

        private readonly string[] EmployeesExpectedHeaders =
{
            "凭证日期", "项目定义描述", "WBS元素", "网络", "物料编码", "物料描述", "计量单位", "数量", "移动平均价", "本位币金额", "移动类型", "物料凭证", "经办人", "领料单位", "采购订单/行项目", "供应商描述"
        };

        public InputExecl()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 获取屏幕的工作区域
            var screen = SystemParameters.WorkArea;

            // 计算窗口的位置
            double left = (screen.Width - this.Width) / 2 + screen.Left;
            double top = (screen.Height - this.Height) / 2 + screen.Top;

            // 设置窗口的位置
            this.Left = left;
            this.Top = top;
        }

        public static string GenerateUniqueNumber()
        {
            // 使用当前时间（精确到毫秒）
            DateTime now = DateTime.Now;

            // 格式化为字符串：年(4位)月(2位)日(2位)小时(2位)分(2位)秒(2位)毫秒(3位)
            string uniqueNumber = now.ToString("yyyyMMddHHmmssfff");

            return uniqueNumber;
        }

        //导入人员信息按钮
        private void ImportPersons_Click(object sender, RoutedEventArgs e)
        {
            string tempFilePath = "";
            try
            {
                var openFileDialog = new OpenFileDialog { Filter = "Excel Files|*.xlsx;*.xls" };
                if (openFileDialog.ShowDialog() == true)
                {
                    List<Employee> employeeList = new List<Employee>();
                    List<ImportHistory> importHistoryList = new List<ImportHistory>();
                    string filePath = openFileDialog.FileName;

                    if (!File.Exists(filePath))
                    {
                        MessageBox.Show("文件不存在！");
                        return;
                    }

                    // 生成临时文件路径
                     tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(filePath) + Guid.NewGuid().ToString());
                    File.Copy(filePath, tempFilePath);

                    // 调用方法读取 Excel 文件
                    employeeList = ReadEmployeesExcelFile(tempFilePath);
                    _dbManager.InsertOrUpdateEmployee(employeeList);
                    // 当前时间
                    string Date = GenerateUniqueNumber();
                    foreach (Employee employee in employeeList)
                    {
                        ImportHistory importHistory = new ImportHistory{

                            IDCardNumber=employee.Wuliaoid,
                            ImportCount = Date,
                            ImportTime = Date

                        };
                        importHistoryList.Add(importHistory);


                    }
                    _dbManager.InsertOrUpdateImportHistory(importHistoryList);
                    // 删除临时文件
                    File.Delete(tempFilePath);

                    // 显示导入结果（可选）
                    MessageBox.Show($"成功导入 {employeeList.Count} 条数据！");
                }
            }
            catch (Exception ex)
            {  // 删除临时文件
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
               
                MessageBox.Show("未知错误" + ex.Message);
            }
        }

        //private void ImportTrainingRecords_Click(object sender, RoutedEventArgs e)
        //{
        //    string tempFilePath = "";
        //    try
        //    {
        //        var openFileDialog = new OpenFileDialog { Filter = "Excel Files|*.xlsx;*.xls" };
        //        if (openFileDialog.ShowDialog() == true)
        //        {
        //            List<TrainingRecord> trainingRecords = new List<TrainingRecord>();
        //            List<Employee> employeeList = new List<Employee>();
        //            string filePath = openFileDialog.FileName;

        //            if (!File.Exists(filePath))
        //            {
        //                MessageBox.Show("文件不存在！");
        //                return;
        //            }
        //            string Date = GenerateUniqueNumber();
        //            // 生成临时文件路径
        //            tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(filePath) + Guid.NewGuid().ToString());
        //            File.Copy(filePath, tempFilePath);

        //            // 调用方法读取 Excel 文件
        //            (trainingRecords, employeeList) = ReadTrainingRecordExcelFile(tempFilePath);
        //            List <ImportHistory> importHistories = new List<ImportHistory>();
        //            // 开启事务，确保插入的原子性
               
                    
        //                    try
        //                    {
        //                        // 插入 TrainingRecord 数据
        //                        foreach (TrainingRecord trainingRecord in trainingRecords)
        //                        {
        //                            ImportHistory importHistory = new ImportHistory
        //                            {

        //                                IDCardNumber = trainingRecord.EmployeeId,
        //                                ImportCount = Date,
        //                                ImportTime = Date

        //                            };
                                  
        //                            importHistories.Add(importHistory);
                                    
        //                        }
        //                        _dbManager.InsertTrainingRecordOrUpdate(trainingRecords);
        //                        // 插入 Employee 数据

        //                        _dbManager.InsertOrUpdateEmployee(employeeList);

        //                        _dbManager.InsertOrUpdateImportHistory(importHistories);
                                
        //                        MessageBox.Show($"成功导入 {trainingRecords.Count} 条培训记录和 {employeeList
        //                 .Select(e => e.IDCardNumber) // 提取身份证号码
        //                 .Distinct() // 去重
        //                 .ToList().Count} 条员工数据！");
        //                    }
        //                    catch (Exception ex)
        //                    {
            
        //                        MessageBox.Show("导入失败，已回滚数据：" + ex.Message);
        //                    }
        //                }
                  

        //            // 删除临时文件
        //            File.Delete(tempFilePath);
               
        //    }
        //    catch (Exception ex)
        //    {
        //        // 删除临时文件
        //        if (File.Exists(tempFilePath))
        //        {
        //            File.Delete(tempFilePath);
        //        }
        //        MessageBox.Show("未知错误：" + ex.Message);
        //    }
        //}

        // 读取导入 Excel 文件
        private List<Employee> ReadEmployeesExcelFile(string filePath)
        {
            var employees = new List<Employee>();

            // 读取 Excel 文件
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var package = new ExcelPackage(fileStream))
                {
                    var worksheet = package.Workbook.Worksheets[1]; // 读取第一个工作表

                    // 检查第一行是否符合预期格式
                    for (int col = 1; col <= EmployeesExpectedHeaders.Length; col++)
                    {
                        var header = worksheet.Cells[1, col].Text.Trim();
                        if (header != EmployeesExpectedHeaders[col - 1])
                        {
                            MessageBox.Show($"表头格式错误: 第 {col} 列应为 '{EmployeesExpectedHeaders[col - 1]}', 实际为 '{header}'");
                            throw new Exception($"表头格式错误: 第 {col} 列应为 '{EmployeesExpectedHeaders[col - 1]}', 实际为 '{header}'");
                        }
                    }

                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // 从第2行开始读取（跳过表头）
                    {
                        if (worksheet.Cells[row, 3].Text == null || worksheet.Cells[row, 3].Text == "")
                        {
                            throw new FormatException($"请检查数据，{row}行的不能为空");
                        }

                        var employee = new Employee
                        {
                            Pingzhengriqi = worksheet.Cells[row, 1].Text?.Trim() ?? "",      
                            Xiangmudingyi = worksheet.Cells[row, 2].Text?.Trim() ?? "",      
                            WBS = worksheet.Cells[row, 3].Text?.Trim() ?? "",     
                            Wangluo = worksheet.Cells[row, 4].Text?.Trim() ?? "", 
                            Wuliaoid = worksheet.Cells[row, 5].Text?.Trim() ?? "",         
                            Wuliaomiaoshu = worksheet.Cells[row, 5].Text?.Trim() ?? "",      
                            Jiliangdanwei = worksheet.Cells[row, 6].Text?.Trim() ?? "",      
                            Shuliang = worksheet.Cells[row, 7].Text?.Trim() ?? "",           
                            Yidongpingjunjia = worksheet.Cells[row, 8].Text?.Trim() ?? "",   
                            Benweibijine = worksheet.Cells[row, 9].Text?.Trim() ?? "",      
                            Yidongleixing = worksheet.Cells[row, 9].Text?.Trim() ?? "",           
                            Wuliaopingzheng = worksheet.Cells[row, 9].Text?.Trim() ?? "",       
                            Jingbanren = worksheet.Cells[row, 9].Text?.Trim() ?? "",       
                            Lingliaodanwei = worksheet.Cells[row, 9].Text?.Trim() ?? "",
                            Caigoudingdan = worksheet.Cells[row, 9].Text?.Trim() ?? "",
                            gongyishangmiaoshu = worksheet.Cells[row, 9].Text?.Trim() ?? "",
                        };

                        employees.Add(employee);
                    }
                }
            }
            return employees;
        }

        //// 读取培训记录 Excel 文件
        //private (List<TrainingRecord>, List<Employee>) ReadTrainingRecordExcelFile(string filePath)
        //{
        //    var trainingRecords = new List<TrainingRecord>();
        //    var employees = new List<Employee>();

        //    // 读取 Excel 文件
        //    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        //    {
        //        using (var package = new ExcelPackage(fileStream))
        //        {
        //            var worksheet = package.Workbook.Worksheets[1]; // 读取第一个工作表

        //            // 检查第一行是否符合预期格式
        //            for (int col = 1; col <= ExpectedHeaders.Length; col++)
        //            {
        //                var header = worksheet.Cells[1, col].Text.Trim();
        //                if (header != ExpectedHeaders[col - 1])
        //                {
        //                    MessageBox.Show($"表头格式错误: 第 {col} 列应为 '{ExpectedHeaders[col - 1]}', 实际为 '{header}'");
        //                    throw new Exception($"表头格式错误: 第 {col} 列应为 '{ExpectedHeaders[col - 1]}', 实际为 '{header}'");
        //                }
        //            }

        //            for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // 从第2行开始读取（跳过表头）
        //            {
        //                if (worksheet.Cells[row, 3].Text == null || worksheet.Cells[row, 3].Text == "")
        //                {
        //                    throw new FormatException($"请检查数据，{row}行的身份证不能为空");
        //                }

        //                var trainingRecord = new TrainingRecord
        //                {
        //                    EmployeeId = worksheet.Cells[row, 2].Text?.Trim() ?? "",                     // B列
        //                    TrainingDate = ConvertToDateTime(worksheet.Cells[row, 3].Text?.Trim() ?? ""), // C列
        //                    TrainingLocation = worksheet.Cells[row, 4].Text?.Trim() ?? "",               // D列
        //                    TrainingUnit = worksheet.Cells[row, 5].Text?.Trim() ?? "",                   // E列
        //                    TrainingContent = worksheet.Cells[row, 6].Text?.Trim() ?? "",                // F列
        //                    Cost = ConvertToDecimal(worksheet.Cells[row, 7].Text?.Trim() ?? "0"),        // G列
        //                    Remarks = worksheet.Cells[row, 8].Text?.Trim() ?? ""                         // H列
        //                };

        //                var employee = new Employee
        //                {
        //                    Name = worksheet.Cells[row, 1].Text?.Trim() ?? "",              // A列
        //                    IDCardNumber = worksheet.Cells[row, 2].Text?.Trim() ?? "",      // B列   
        //                };

        //                trainingRecords.Add(trainingRecord);
        //                employees.Add(employee);
        //            }
        //        }
        //    }
        //    return (trainingRecords, employees);
        //}

        //public DateTime ConvertToDateTime(string dateString)
        //{
        //    if (DateTime.TryParse(dateString, out DateTime date))
        //    {
        //        return date;
        //    }
        //    else
        //    {
        //        MessageBox.Show($"无法将培训时间转换为日期格式: {dateString}");
        //        throw new FormatException($"无法将培训时间转换为日期格式: {dateString}");
        //    }
        //}

        //public decimal ConvertToDecimal(string costString)
        //{
        //    if (decimal.TryParse(costString, out decimal cost))
        //    {
        //        return cost;
        //    }
        //    else
        //    {
        //        MessageBox.Show($"无法将费用转换为数字格式: {costString}");
        //        throw new FormatException($"无法将费用转换为数字格式: {costString}");
        //    }
        //}
        // 导出人员信息模板
        private void ExportPersonTemplate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 创建并显示文件夹选择对话框
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    dialog.Description = "请选择导出文件的目标文件夹";
                    dialog.ShowNewFolderButton = true;

                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        // 获取用户选择的文件夹路径
                        string selectedPath = dialog.SelectedPath;
                        List<PersonnelInfo> PersonnelInfolist = new List<PersonnelInfo>();
                        // 导出人员信息
                        HomePage.ExportToExcel(selectedPath + "/出库单模板.xlsx", PersonnelInfolist, new List<string>
                            { "凭证日期", "项目定义描述", "WBS元素", "网络", "物料编码", "物料描述", "计量单位", "数量", "移动平均价", "本位币金额", "移动类型", "物料凭证", "经办人", "领料单位", "采购订单/行项目", "供应商描述" });
                    }
                    else
                    {
                        ShowPopu("操作已取消");
                    }
                }

            }
            catch (Exception ex)
            {
                ShowPopu("发生致命错误消息:" + ex.Message);
            }
        }

        // 导出培训记录模板
        //private void ExportTrainingTemplate_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        // 创建并显示文件夹选择对话框
        //        using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
        //        {
        //            dialog.Description = "请选择导出文件的目标文件夹";
        //            dialog.ShowNewFolderButton = true;

        //            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //            {
        //                // 获取用户选择的文件夹路径
        //                string selectedPath = dialog.SelectedPath;
        //                List<TrainingInfo> TrainingInfolist = new List<TrainingInfo>();
                       
        //                // 导出培训记录
        //                HomePage.ExportToExcel(selectedPath + "/培训档案模板.xlsx", TrainingInfolist, new List<string>
        //                    {"姓名", "身份证", "培训时间", "培训地点", "培训单位", "培训内容", "费用", "备注"});
        //            }
        //            else
        //            {
        //                ShowPopu("操作已取消");
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ShowPopu("发生致命错误消息:" + ex.Message);
        //    }

        //}

        private void GoToHomePage_Click(object sender, RoutedEventArgs e)
        {
            var homePage = new HomePage();
            homePage.Show();
            this.Close(); // 关闭当前窗口（可选）
        }

        private void ShowPopu(string errorMessage)
        {
            PopupWindow popup = new PopupWindow(errorMessage);
            popup.Owner = this; // 设置弹出框的拥有者为主窗口
            popup.ShowDialog(); // 显示模态对话框
        }
    }
}
