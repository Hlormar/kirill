using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal static class Ui
    {
        public static readonly Color Background=Color.FromArgb(242,245,249);
        public static readonly Color Ink=Color.FromArgb(33,48,66);
        public static readonly Color Accent=Color.FromArgb(41,101,181);
        public static void Form(Form form)
        {
            form.Font=new Font("Segoe UI",9.5f);
            form.ForeColor=Ink;form.BackColor=Background;
            form.AutoScaleMode=AutoScaleMode.Font;
        }
        public static Control Heading(string title,string subtitle)
        {
            var panel=new TableLayoutPanel {Dock=DockStyle.Fill,AutoSize=true,ColumnCount=1,RowCount=2,Padding=new Padding(16,10,16,10),BackColor=Color.White};
            panel.Controls.Add(new Label {Text=title,AutoSize=true,Font=new Font("Segoe UI",15,FontStyle.Bold),ForeColor=Ink,Margin=Padding.Empty},0,0);
            panel.Controls.Add(new Label {Text=subtitle,AutoSize=true,ForeColor=Color.FromArgb(105,119,137),Margin=new Padding(0,3,0,0)},0,1);
            return panel;
        }
        public static void Button(Button button,bool primary=false)
        {
            button.AutoSize=true;button.AutoSizeMode=AutoSizeMode.GrowAndShrink;
            button.Padding=new Padding(14,5,14,5);button.MinimumSize=new Size(108,36);
            button.FlatStyle=FlatStyle.Flat;button.FlatAppearance.BorderSize=primary?0:1;
            button.FlatAppearance.BorderColor=Color.FromArgb(209,219,231);
            button.BackColor=primary?Accent:Color.White;button.ForeColor=primary?Color.White:Ink;
            button.Cursor=Cursors.Hand;button.Margin=new Padding(8,0,0,0);
        }
        public static void Grid(DataGridView grid)
        {
            grid.BorderStyle=BorderStyle.None;grid.BackgroundColor=Color.White;
            grid.EnableHeadersVisualStyles=false;grid.ColumnHeadersBorderStyle=DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle=new DataGridViewCellStyle {BackColor=Color.FromArgb(230,237,247),ForeColor=Ink,Font=new Font("Segoe UI",9.5f,FontStyle.Bold),Padding=new Padding(8,4,8,4),SelectionBackColor=Color.FromArgb(230,237,247),SelectionForeColor=Ink};
            grid.ColumnHeadersHeight=38;grid.ColumnHeadersHeightSizeMode=DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.DefaultCellStyle=new DataGridViewCellStyle {BackColor=Color.White,ForeColor=Ink,SelectionBackColor=Color.FromArgb(218,233,252),SelectionForeColor=Ink,Padding=new Padding(6,3,6,3)};
            grid.AlternatingRowsDefaultCellStyle.BackColor=Color.FromArgb(247,249,252);
            grid.CellBorderStyle=DataGridViewCellBorderStyle.SingleHorizontal;grid.GridColor=Color.FromArgb(231,236,243);
            grid.RowTemplate.Height=32;grid.RowHeadersWidth=30;
        }
        public static void Columns(DataGridView grid)
        {
            foreach(DataGridViewColumn column in grid.Columns) {
                string name=column.DataPropertyName;
                bool key=column.HeaderText=="№"||name=="RecordID";
                bool numeric=name=="Quantity"||name=="Sum"||name=="Price"||name=="SupplierPrice"||name=="Salary"||name=="UnitPrice";
                column.MinimumWidth=key?48:numeric?95:name.EndsWith("Date",StringComparison.Ordinal)?125:130;
                column.FillWeight=key?35:numeric?75:name=="Name"||name=="ProductID"||name=="Reason"?175:110;
                if(numeric)column.DefaultCellStyle.Alignment=DataGridViewContentAlignment.MiddleRight;
                if(name=="Price"||name=="Sum"||name=="SupplierPrice"||name=="Salary"||name=="UnitPrice")column.DefaultCellStyle.Format="N2";
            }
        }
        public static Size WindowSize(Form form,int width,int height)
        {
            var area=Screen.FromControl(form).WorkingArea;
            return new Size(Math.Min(width,Math.Max(560,area.Width-100)),Math.Min(height,Math.Max(340,area.Height-120)));
        }
    }
}
