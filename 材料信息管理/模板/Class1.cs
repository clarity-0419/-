using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

public class WordDocumentGenerator
{
    public void CreateTrainingRecordDocument(string filePath, EmployeeInfo employee)
    {
        
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // 创建文档并添加内容
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(filePath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
        {
            // 添加主文档部分
            MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
            mainPart.Document = new Document(new Body());

            Body body = mainPart.Document.Body;

            // 设置页面大小为A3
            SetPageSize(body);

            // 添加员工信息
            AddEmployeeInfo(body, employee);

            // 添加培训记录表格
            //AddTrainingRecordsTable(body, employee.TrainingRecords);

            // 保存文档
            mainPart.Document.Save();
        }
    }

    private void SetPageSize(Body body)
    {
        // 设置页面大小为 A3
        SectionProperties sectionProps = new SectionProperties();
        PageSize pageSize = new PageSize() { Width = 16838, Height = 11906 }; // A3纸张大小，单位是1/1440英寸
        sectionProps.Append(pageSize);

        // 添加到文档中
        body.Append(sectionProps);
    }

    // 添加员工信息
    private void AddEmployeeInfo(Body body, EmployeeInfo employee)
    {
        // 创建并居中显示标题
        Paragraph titleParagraph = new Paragraph();
        ParagraphProperties titleParagraphProperties = new ParagraphProperties();
        titleParagraphProperties.Justification = new Justification { Val = JustificationValues.Center };
        titleParagraph.ParagraphProperties = titleParagraphProperties;
        titleParagraph.Append(new Run(new Text($"{employee.Wuliaoid}")));
        body.Append(titleParagraph);

        // 创建员工信息的段落
        Paragraph paragraph = new Paragraph();
        ParagraphProperties paragraphProperties = new ParagraphProperties();
        paragraphProperties.Justification = new Justification { Val = JustificationValues.Center }; // 居中对齐
        paragraph.ParagraphProperties = paragraphProperties;

        // 添加每个字段和对应的值，并插入 Tab 制表符实现间隔
        paragraph.Append(
            CreateTab(),
            CreateRun("凭证日期: " + employee.Pingzhengriqi), // 设置字体大小
            CreateTab(),
            CreateRun("项目定义描述: " + employee.Xiangmudingyi),
            CreateTab(),
            CreateRun("WBS: " + employee.WBS),
            CreateTab(),
            CreateRun("网络: " + employee.Wangluo),
            CreateTab(),
             CreateRun("物料编码: " + employee.Wuliaoid),
            CreateTab(),
             CreateRun("物料描述: " + employee.Wuliaomiaoshu),
            CreateTab(),
             CreateRun("计量单位: " + employee.Jiliangdanwei),
            CreateTab(),
             CreateRun("数量: " + employee.Shuliang),
            CreateTab(),
             CreateRun("移动平均价: " + employee.Yidongpingjunjia),
            CreateTab(),
             CreateRun("本位币金额: " + employee.Benweibijine),
            CreateTab(),
             CreateRun("移动类型: " + employee.Yidongleixing),
            CreateTab(),
             CreateRun("物料凭证: " + employee.Wuliaopingzheng),
            CreateTab(),
             CreateRun("经办人: " + employee.Jingbanren),
            CreateTab(),
             CreateRun("领料单位: " + employee.Lingliaodanwei),
            CreateTab(),
             CreateRun("采购订单/行项目: " + employee.Caigoudingdan),
            CreateTab(),
             CreateRun("供应商描述: " + employee.gongyishangmiaoshu)
            

        );

        // 将段落添加到文档主体
        body.Append(paragraph);
    }

    // 创建文本运行，支持修改字体大小
    private Run CreateRun(string text, int? fontSize = null)
    {
        Run run = new Run(new Text(text));

        // 如果提供了字体大小，则设置字体大小
        if (fontSize.HasValue)
        {
            RunProperties runProperties = new RunProperties();
            runProperties.FontSize = new FontSize() { Val = (fontSize.Value * 2).ToString() }; // 字号是磅大小 * 2
            run.PrependChild(runProperties);
        }

        return run;
    }

    // 创建 Tab 制表符
    private Run CreateTab()
    {
        return new Run(new TabChar());
    }

    //// 添加培训记录表格
    //private void AddTrainingRecordsTable(Body body, List<TrainingRecord> records)
    //{
    //    // 创建表格
    //    Table table = new Table();

    //    // 定义表格样式
    //    TableProperties tblProps = new TableProperties(
    //        new TableBorders(
    //            new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 },
    //            new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 },
    //            new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 },
    //            new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 },
    //            new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 },
    //            new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 }
    //        ),
    //        new TableJustification { Val = TableRowAlignmentValues.Center } // 表格居中
    //    );
    //    table.AppendChild(tblProps);

    //    // 添加表头
    //    TableRow headerRow = new TableRow();
    //    headerRow.Append(
    //        CreateTableCell("序号"),
    //        CreateTableCell("时间"),
    //        CreateTableCell("培训内容"),
    //        CreateTableCell("培训单位"),
    //        CreateTableCell("培训地点"),
    //        //CreateTableCell("成绩/考核"),
    //        CreateTableCell("费用"),
    //        CreateTableCell("备注")
    //    );
    //    table.Append(headerRow);

    //    // 添加数据行
    //    for (int i = 0; i < records.Count; i++)
    //    {
    //        TableRow dataRow = new TableRow();
    //        dataRow.Append(
    //            CreateTableCell((i + 1).ToString()),
    //            CreateTableCell(records[i].TrainingDate.ToString()),
    //            CreateTableCell(records[i].TrainingContent),
    //            CreateTableCell(records[i].TrainingUnit),
    //            CreateTableCell(records[i].TrainingLocation),
    //            //CreateTableCell(records[i].Assessment),
    //            CreateTableCell(records[i].Cost.ToString("C")), // 格式化为货币
    //            CreateTableCell(records[i].Remarks)
    //        );
    //        table.Append(dataRow);
    //    }

    //    // 将表格添加到文档主体
    //    body.Append(table);
    //}

    //// 创建表格单元格
    //private TableCell CreateTableCell(string text)
    //{
    //    TableCell cell = new TableCell();
    //    cell.Append(new Paragraph(new Run(new Text(text))));  // 在单元格中插入文本
    //    return cell;
    //}
}

// 使用示例
public class Program
{
    public async static void outDocx(string path, string Wuliaoid)
    {
        DatabaseManager dbManager = new DatabaseManager();
        List<Employee> employees = await dbManager.GetEmployees();
        //List<TrainingRecord> Records = await dbManager.GetTrainingRecordsByEmployeeId(employees[0].IDCardNumber);
        EmployeeInfo employee = new EmployeeInfo
        {
            Id= employees[0].Id,
            Pingzhengriqi = employees[0].Pingzhengriqi,
            Xiangmudingyi = employees[0].Xiangmudingyi,
            WBS = employees[0].WBS,
            Wangluo = employees[0].Wangluo,
            Wuliaoid = employees[0].Wuliaoid,
            Wuliaomiaoshu = employees[0].Wuliaomiaoshu,
            Jiliangdanwei = employees[0].Jiliangdanwei,
            Shuliang = employees[0].Shuliang,
            Yidongpingjunjia = employees[0].Yidongpingjunjia,
            Benweibijine = employees[0].Benweibijine,
            Yidongleixing = employees[0].Yidongleixing,
            Wuliaopingzheng = employees[0].Wuliaopingzheng,
            Jingbanren = employees[0].Jingbanren,
            Lingliaodanwei = employees[0].Lingliaodanwei,
            Caigoudingdan = employees[0].Caigoudingdan,
            gongyishangmiaoshu = employees[0].gongyishangmiaoshu,


            //TrainingRecords = Records
        };

        //TemplateBasedWordGenerator generator = new TemplateBasedWordGenerator();
        //generator.GenerateFromTemplate(@path + "/"  + employees[0].Name+ employees[0].IDCardNumber + "的培训记录.docx", employee);
    }

    public static async void outAllDocx(string path, List<Employee> employees)
    {
        DatabaseManager dbManager = new DatabaseManager();
        foreach(Employee employee in employees)
        {
            //List<TrainingRecord> Records = await dbManager.GetTrainingRecordsByEmployeeId(employee.IDCardNumber);
            EmployeeInfo employeefo = new EmployeeInfo
            {
                //Name = employee.Name,
                //IDCardNumber = employee.IDCardNumber,
                //Education = employee.Education,
                //Title = employee.Title,
                //Position = employee.LevelJobType,
                Id = employee.Id,
                Pingzhengriqi = employee.Pingzhengriqi,
                Xiangmudingyi = employee.Xiangmudingyi,
                WBS = employee.WBS,
                Wangluo = employee.Wangluo,
                Wuliaoid = employee.Wuliaoid,
                Wuliaomiaoshu = employee.Wuliaomiaoshu,
                Jiliangdanwei = employee.Jiliangdanwei,
                Shuliang = employee.Shuliang,
                Yidongpingjunjia = employee.Yidongpingjunjia,
                Benweibijine = employee.Benweibijine,
                Yidongleixing = employee.Yidongleixing,
                Wuliaopingzheng = employee.Wuliaopingzheng,
                Jingbanren = employee.Jingbanren,
                Lingliaodanwei = employee.Lingliaodanwei,
                Caigoudingdan = employee.Caigoudingdan,
                gongyishangmiaoshu = employee.gongyishangmiaoshu,
                //TrainingRecords = Records
            };

            // 删除对 TemplateBasedWordGenerator 的引用
            //generator.GenerateFromTemplate(@path+"/" + employee.Name+ employee.IDCardNumber + "的培训记录.docx", employeefo);

        }
        
    }


    //    public static void outRecordsDocx(string path, List<EmployeeInfo> employeeInfos)
    //    {
    //        foreach (EmployeeInfo employeeInfo in employeeInfos)
    //        {
    //            WordDocumentGenerator generator = new WordDocumentGenerator();
    //            generator.CreateTrainingRecordDocument(@path + "/" + employeeInfo.Wuliaoid + "-"+employeeInfo.Pingzhengriqi + "的材料出库.docx", employeeInfo);

    //        }
    //    }
    //}





    //// 导出需要的员工信息类
    //public class EmployeeInfo
    //{
    //    public string Name { get; set; }
    //    public string IDCardNumber { get; set; }
    //    public string Education { get; set; }
    //    public string Title { get; set; }
    //    public string Position { get; set; }

    //}

    
}

// 导出需要的员工信息类
public class EmployeeInfo
{
    public string Id { get; set; }
    public String? Pingzhengriqi { get; set; }
    public string? Xiangmudingyi { get; set; }
    public string? WBS { get; set; }
    public string? Wangluo { get; set; }
    public string? Wuliaoid { get; set; }
    public string? Wuliaomiaoshu { get; set; }
    public string? Jiliangdanwei { get; set; }
    public string? Shuliang { get; set; }
    public string? Yidongpingjunjia { get; set; }
    public string? Benweibijine { get; set; }
    public string? Yidongleixing { get; set; }
    public string? Wuliaopingzheng { get; set; }
    public string? Jingbanren { get; set; }
    public string? Lingliaodanwei { get; set; }
    public string? Caigoudingdan { get; set; }
    public string? gongyishangmiaoshu { get; set; }
}