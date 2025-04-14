using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace TrainingRecordManager
{
    public partial class QueryPage : MetroWindow
    {
        DatabaseManager databaseManager = new DatabaseManager();
        // 声明关闭事件
        public event EventHandler PageClosed;

        public QueryPage(string page)
        {
            InitializeComponent();
            if (page.Equals("SummaryPage") ){
                GoToHomePage.Visibility = Visibility.Collapsed;
            }
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
            LoadComboBoxData();
        }
      
        // 加载数据到下拉框
        private void LoadComboBoxData()
        {


            // 姓名下拉框
            databaseManager.LoadComboBoxItems("SELECT Pingzhengriqi FROM Employee", Search1);
            // 身份证号下拉框
            databaseManager.LoadComboBoxItems("SELECT Xiangmudingyi FROM Employee", Search2);
            // 职位下拉框
            databaseManager.LoadComboBoxItems("SELECT WBS FROM Employee", Search3);
            // 级别下拉框
            databaseManager.LoadComboBoxItems("SELECT Wangluo FROM Employee", Search4);
            // 单位名称下拉框
            databaseManager.LoadComboBoxItems("SELECT Wuliaoid FROM Employee", Search5);
            // 学校名称下拉框
            databaseManager.LoadComboBoxItems("SELECT Wuliaomiaoshu FROM Employee", Search6);
            // 专业下拉框
            databaseManager.LoadComboBoxItems("SELECT Jiliangdanwei FROM Employee", Search7);
            // 工种
            databaseManager.LoadComboBoxItems("SELECT Shuliang FROM Employee", Search8);
            // 工种
            databaseManager.LoadComboBoxItems("SELECT Yidongpingjunjia FROM Employee;", Search9);
            //
            databaseManager.LoadComboBoxItems("SELECT Benweibijine FROM Employee", Search10);
            //
            databaseManager.LoadComboBoxItems("SELECT Yidongleixing FROM Employee", Search11);
            //
            databaseManager.LoadComboBoxItems("SELECT Wuliaopingzheng FROM Employee", Search12);      
            
            databaseManager.LoadComboBoxItems("SELECT Jingbanren FROM Employee", Search13);     
            
            databaseManager.LoadComboBoxItems("SELECT Lingliaodanwei FROM Employee", Search14);     
            
            databaseManager.LoadComboBoxItems("SELECT Caigoudingdan FROM Employee", Search15);      
            
            databaseManager.LoadComboBoxItems("SELECT gongyishangmiaoshu FROM Employee", Search16);

            databaseManager.LoadComboBoxItems("SELECT DISTINCT ImportTime FROM ImportHistory;", ImportTime);
        }

        private void GoToHomePage_Click(object sender, RoutedEventArgs e)
        {
            var homePage = new HomePage();
            homePage.Show();
            this.Close(); // 关闭当前窗口（可选）
        }

        // 处理查询按钮点击事件
        private void QueryButton_Click(object sender, RoutedEventArgs e)
        {
            // 获取查询条件
            string Pingzhengriqi = Search1.Text;
            string Xiangmudingyi = Search2.Text;
            string WBS = Search3.Text;
            string Wangluo = Search4.Text;
            string Wuliaoid = Search5.Text;
            string Wuliaomiaoshu = Search6.Text;
            string Jiliangdanwei = Search7.Text;
            string Shuliang = Search8.Text;
            string Yidongpingjunjia = Search9.Text;
            string Benweibijine = Search10.Text;
            string Yidongleixing = Search11.Text;
            string Wuliaopingzheng = Search12.Text;
            string Jingbanren = Search13.Text;
            string Lingliaodanwei = Search14.Text;
            string Caigoudingdan = Search15.Text;
            string gongyishangmiaoshu = Search16.Text;


            // 创建 Employee 对象并赋值
            var employeeFilter = new Employee
            {
                Pingzhengriqi = string.IsNullOrWhiteSpace(Pingzhengriqi) ? null : Pingzhengriqi,
                Xiangmudingyi = string.IsNullOrWhiteSpace(Xiangmudingyi) ? null : Xiangmudingyi,
                WBS = string.IsNullOrWhiteSpace(WBS) ? null : WBS,
                Wangluo = string.IsNullOrWhiteSpace(Wangluo) ? null : Wangluo,
                Wuliaoid = string.IsNullOrWhiteSpace(Wuliaoid) ? null : Wuliaoid,
                Wuliaomiaoshu = string.IsNullOrWhiteSpace(Wuliaomiaoshu) ? null : Wuliaomiaoshu,
                Jiliangdanwei = string.IsNullOrWhiteSpace(Jiliangdanwei) ? null : Jiliangdanwei,
                Shuliang = string.IsNullOrWhiteSpace(Shuliang) ? null : Shuliang,
                Yidongpingjunjia = string.IsNullOrWhiteSpace(Yidongpingjunjia) ? null : Yidongpingjunjia,
                Benweibijine = string.IsNullOrWhiteSpace(Benweibijine) ? null : Benweibijine,
                Yidongleixing = string.IsNullOrWhiteSpace(Yidongleixing) ? null : Yidongleixing,
                Wuliaopingzheng = string.IsNullOrWhiteSpace(Wuliaopingzheng) ? null : Wuliaopingzheng,
                Jingbanren = string.IsNullOrWhiteSpace(Jingbanren) ? null : Jingbanren,
                Lingliaodanwei = string.IsNullOrWhiteSpace(Lingliaodanwei) ? null : Lingliaodanwei,
                Caigoudingdan = string.IsNullOrWhiteSpace(Caigoudingdan) ? null : Caigoudingdan,
                gongyishangmiaoshu = string.IsNullOrWhiteSpace(gongyishangmiaoshu) ? null : gongyishangmiaoshu,
            };
            // 跳转到查看一览页面
            var summaryPage = new SummaryPage(employeeFilter, string.IsNullOrWhiteSpace(ImportTime.Text) ? null : ImportTime.Text); // 确保您已创建该页面
            summaryPage.Show();
            this.Close(); // 关闭当前页面（可选）
            // 触发 PageClosed 事件
            PageClosed?.Invoke(this, EventArgs.Empty);

        }

    }
}
