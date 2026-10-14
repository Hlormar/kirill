using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace WindowsFormsApp1
{
    internal enum ReportKind { Sales, Supplies, Stock }
    internal sealed class ReportForm : Form
    {
        private readonly ReportKind kind;
        private readonly DateTimePicker from=new DateTimePicker {Format=DateTimePickerFormat.Short,Value=new DateTime(2026,1,1)};
        private readonly DateTimePicker to=new DateTimePicker {Format=DateTimePickerFormat.Short};
        private readonly DataGridView grid=DataForm.MakeGrid();
        private readonly Label status=new Label {Dock=DockStyle.Bottom,Height=32,Padding=new Padding(6)};
        private readonly FlowLayoutPanel toolbar=new FlowLayoutPanel {Dock=DockStyle.Top,AutoSize=true,AutoSizeMode=AutoSizeMode.GrowAndShrink,WrapContents=true,Padding=new Padding(10),BackColor=Color.White};
        private DataTable data;
        private string description;
        private bool busy;
        public ReportForm(ReportKind k)
        {
            kind=k;Text=Reports.Title(kind);Ui.Form(this);ClientSize=Ui.WindowSize(this,1080,470);MinimumSize=new Size(680,420);
            from.Width=135;to.Width=135;from.Margin=new Padding(8,4,12,4);to.Margin=new Padding(8,4,12,4);
            status.AutoSize=true;status.MaximumSize=new Size(0,0);status.MinimumSize=new Size(0,32);status.BackColor=Ui.Background;
            var preview=new Button {Text="Сформировать",AutoSize=true};var export=new Button {Text="Сохранить файл",AutoSize=true};
            Ui.Button(preview);Ui.Button(export,true);
            if(kind!=ReportKind.Stock) {
                toolbar.Controls.Add(new Label {Text="Период с",AutoSize=true,Padding=new Padding(0,6,0,0)});toolbar.Controls.Add(from);
                toolbar.Controls.Add(new Label {Text="по",AutoSize=true,Padding=new Padding(0,6,0,0)});toolbar.Controls.Add(to);
                from.ValueChanged+=(s,a)=>{data=null;status.Text="Период изменён. Сформируйте отчёт.";};
                to.ValueChanged+=(s,a)=>{data=null;status.Text="Период изменён. Сформируйте отчёт.";};
            }
            toolbar.Controls.Add(preview);toolbar.Controls.Add(export);
            var layout=new TableLayoutPanel {Dock=DockStyle.Fill,ColumnCount=1,RowCount=4,Padding=new Padding(12)};
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent,100));layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(Ui.Heading(Text,"Просмотр данных и сохранение файла"),0,0);
            toolbar.Dock=DockStyle.Fill;layout.Controls.Add(toolbar,0,1);layout.Controls.Add(grid,0,2);
            status.Dock=DockStyle.Fill;layout.Controls.Add(status,0,3);Controls.Add(layout);
            preview.Click+=async(s,a)=>await LoadAsync();
            export.Click+=async(s,a)=>await ExportAsync();
            Shown+=async(s,a)=>await LoadAsync();
            FormClosing+=(s,a)=>{if(busy)a.Cancel=true;};
        }
        private async Task LoadAsync()
        {
            if(busy)return;
            if(kind!=ReportKind.Stock&&from.Value.Date>to.Value.Date){MessageBox.Show(this,"Начало периода должно быть не позже конца.");return;}
            busy=true;toolbar.Enabled=false;status.Text="Запрос данных…";
            try {
                DateTime a=from.Value.Date,b=to.Value.Date;
                var result=await Task.Run(()=>Reports.Load(kind,a,b));
                data=result;
                description=kind==ReportKind.Stock ? "Текущие остатки. Срок годности — ближайшая дата из поставок (справочно)." : "Период: "+a.ToString("dd.MM.yyyy")+" — "+b.ToString("dd.MM.yyyy");
                grid.DataSource=data;status.Text="Строк: "+data.Rows.Count+". "+description;
                foreach(DataGridViewColumn column in grid.Columns){column.MinimumWidth=105;column.FillWeight=column.HeaderText=="Товар"||column.HeaderText=="Категория"||column.HeaderText=="Сотрудник"?165:95;}
            }catch(Exception ex){data=null;MessageBox.Show(this,Database.Friendly(ex),"Ошибка отчёта");}
            finally{busy=false;toolbar.Enabled=true;}
        }
        private async Task ExportAsync()
        {
            if(busy)return;
            if(data==null)await LoadAsync();
            if(data==null)return;
            string ext=kind==ReportKind.Sales?"docx":kind==ReportKind.Supplies?"xlsx":"pdf";
            using(var dialog=new SaveFileDialog {Filter=ext.ToUpperInvariant()+"|*."+ext,DefaultExt=ext,FileName=Reports.Title(kind)+"."+ext}) {
                if(dialog.ShowDialog(this)!=DialogResult.OK)return;
                var copy=data.Copy();string path=dialog.FileName,caption=description;
                busy=true;toolbar.Enabled=false;status.Text="Создание файла…";
                try{await Task.Run(()=>Reports.Export(kind,copy,path,caption));status.Text="Сохранено: "+path;MessageBox.Show(this,"Файл сохранён:\n"+path,"Экспорт завершён");}
                catch(Exception ex){MessageBox.Show(this,ex.Message,"Не удалось сохранить файл");}
                finally{busy=false;toolbar.Enabled=true;}
            }
        }
    }
    internal static class Reports
    {
        public static string Title(ReportKind k) {return k==ReportKind.Sales?"Продажи":k==ReportKind.Supplies?"Поставки":"Остатки и сроки годности";}
        public static DataTable Load(ReportKind k,DateTime from,DateTime to)
        {
            if(k==ReportKind.Sales)return Database.Query(@"SELECT s.SaleID AS [Чек],s.SaleDate AS [Дата],e.Surname||' '||e.Name AS [Сотрудник],
                p.Name AS [Товар],c.Name AS [Категория],sp.Quantity AS [Продано],sp.Sum AS [Выручка],
                COALESCE((SELECT SUM(rp.Sum) FROM ReturnProduct rp JOIN ""Return"" r ON r.ReturnID=rp.ReturnID WHERE r.SaleID=s.SaleID AND rp.ProductID=p.ProductID),0) AS [Возвраты],
                sp.Sum-COALESCE((SELECT SUM(rp.Sum) FROM ReturnProduct rp JOIN ""Return"" r ON r.ReturnID=rp.ReturnID WHERE r.SaleID=s.SaleID AND rp.ProductID=p.ProductID),0) AS [Нетто]
                FROM Sale s JOIN Employee e ON e.EmployeeID=s.EmployeeID JOIN SaleProduct sp ON sp.SaleID=s.SaleID
                JOIN Product p ON p.ProductID=sp.ProductID JOIN ProductCategory c ON c.CategoryID=p.CategoryID
                WHERE s.SaleDate BETWEEN @p0 AND @p1 ORDER BY s.SaleDate,s.SaleID,p.Name",from.ToString("yyyy-MM-dd"),to.ToString("yyyy-MM-dd"));
            if(k==ReportKind.Supplies)return Database.Query(@"SELECT s.SupplyID AS [Поставка],s.SupplyDate AS [Дата],v.Name AS [Поставщик],
                p.Name AS [Товар],c.Name AS [Категория],sp.Quantity AS [Количество],sp.SupplierPrice AS [Цена],
                round(sp.Quantity*sp.SupplierPrice,2) AS [Стоимость],sp.ExpirationDate AS [Срок годности]
                FROM Supply s JOIN Supplier v ON v.SupplierID=s.SupplierID JOIN SupplyProduct sp ON sp.SupplyID=s.SupplyID
                JOIN Product p ON p.ProductID=sp.ProductID JOIN ProductCategory c ON c.CategoryID=p.CategoryID
                WHERE s.SupplyDate BETWEEN @p0 AND @p1 ORDER BY s.SupplyDate,s.SupplyID,p.Name",from.ToString("yyyy-MM-dd"),to.ToString("yyyy-MM-dd"));
            return Database.Query(@"SELECT p.Name AS [Товар],c.Name AS [Категория],st.Quantity AS [Остаток],p.Price AS [Цена],
                round(st.Quantity*p.Price,2) AS [Стоимость],MIN(sp.ExpirationDate) AS [Срок годности]
                FROM Product p JOIN ProductCategory c ON c.CategoryID=p.CategoryID JOIN Stock st ON st.ProductID=p.ProductID
                LEFT JOIN SupplyProduct sp ON sp.ProductID=p.ProductID GROUP BY p.ProductID,p.Name,c.Name,st.Quantity,p.Price ORDER BY p.Name");
        }
        public static void Export(ReportKind kind,DataTable data,string path,string description)
        {
            // Build beside the destination and replace only after successful generation.
            string temporary=path+"."+Guid.NewGuid().ToString("N")+".tmp";
            try {
                if(kind==ReportKind.Sales)Word(data,temporary,Title(kind),description);
                else if(kind==ReportKind.Supplies)Excel(data,temporary);
                else Pdf(data,temporary,description);
                if(File.Exists(path))File.Replace(temporary,path,null);else File.Move(temporary,path);
            } finally {if(File.Exists(temporary))File.Delete(temporary);}
        }
        private static string Display(object value)
        {
            if(value==null||value==DBNull.Value)return "—";
            if(value is double||value is decimal||value is float)return Convert.ToDecimal(value).ToString("0.##",CultureInfo.GetCultureInfo("ru-RU"));
            return Convert.ToString(value,CultureInfo.GetCultureInfo("ru-RU"));
        }
        private static void Part(ZipArchive zip,string name,XDocument document)
        {using(var writer=new StreamWriter(zip.CreateEntry(name).Open(),new UTF8Encoding(false)))document.Save(writer);}
        private static XDocument Types(params string[] pairs)
        {
            XNamespace n="http://schemas.openxmlformats.org/package/2006/content-types";
            var root=new XElement(n+"Types",new XElement(n+"Default",new XAttribute("Extension","rels"),new XAttribute("ContentType","application/vnd.openxmlformats-package.relationships+xml")),new XElement(n+"Default",new XAttribute("Extension","xml"),new XAttribute("ContentType","application/xml")));
            for(int i=0;i<pairs.Length;i+=2)root.Add(new XElement(n+"Override",new XAttribute("PartName",pairs[i]),new XAttribute("ContentType",pairs[i+1])));
            return new XDocument(root);
        }
        private static XDocument Relationships(params string[] triples)
        {
            XNamespace n="http://schemas.openxmlformats.org/package/2006/relationships";var root=new XElement(n+"Relationships");
            for(int i=0;i<triples.Length;i+=3)root.Add(new XElement(n+"Relationship",new XAttribute("Id",triples[i]),new XAttribute("Type","http://schemas.openxmlformats.org/officeDocument/2006/relationships/"+triples[i+1]),new XAttribute("Target",triples[i+2])));
            return new XDocument(root);
        }
        internal static void Word(DataTable data,string path,string title,string description)
        {
            XNamespace w="http://schemas.openxmlformats.org/wordprocessingml/2006/main";
            Func<string,bool,XElement> para=(text,bold)=>new XElement(w+"p",new XElement(w+"r",new XElement(w+"rPr",bold?new XElement(w+"b"):null,new XElement(w+"rFonts",new XAttribute(w+"ascii","Arial"),new XAttribute(w+"hAnsi","Arial")),new XElement(w+"sz",new XAttribute(w+"val","18"))),new XElement(w+"t",new XAttribute(XNamespace.Xml+"space","preserve"),text)));
            var table=new XElement(w+"tbl",new XElement(w+"tblPr",new XElement(w+"tblW",new XAttribute(w+"w","0"),new XAttribute(w+"type","auto")),new XElement(w+"tblBorders",new[]{"top","left","bottom","right","insideH","insideV"}.Select(s=>new XElement(w+s,new XAttribute(w+"val","single"),new XAttribute(w+"sz","4"),new XAttribute(w+"color","CCCCCC"))))));
            table.Add(new XElement(w+"tblGrid",data.Columns.Cast<DataColumn>().Select(c=>new XElement(w+"gridCol",new XAttribute(w+"w",(14500/Math.Max(1,data.Columns.Count)).ToString())))));
            table.Add(new XElement(w+"tr",new XElement(w+"trPr",new XElement(w+"tblHeader")),data.Columns.Cast<DataColumn>().Select(c=>new XElement(w+"tc",para(c.ColumnName,true)))));
            foreach(DataRow row in data.Rows)table.Add(new XElement(w+"tr",row.ItemArray.Select(v=>new XElement(w+"tc",para(Display(v),false)))));
            var body=new XElement(w+"body",para(title,true),para(description,false),para("Сформировано: "+DateTime.Now.ToString("dd.MM.yyyy HH:mm"),false),table);
            if(data.Columns.Contains("Нетто")) {
                decimal net=data.Rows.Cast<DataRow>().Sum(r=>Convert.ToDecimal(r["Нетто"]));
                body.Add(para("Итого после возвратов: "+net.ToString("N2")+" руб.",true));
                body.Add(para("Возвраты учтены по выбранным продажам за всё время: физическая модель не содержит дату возврата.",false));
            }
            body.Add(new XElement(w+"sectPr",new XElement(w+"pgSz",new XAttribute(w+"w","16838"),new XAttribute(w+"h","11906"),new XAttribute(w+"orient","landscape")),new XElement(w+"pgMar",new XAttribute(w+"top","720"),new XAttribute(w+"right","720"),new XAttribute(w+"bottom","720"),new XAttribute(w+"left","720"))));
            using(var zip=ZipFile.Open(path,ZipArchiveMode.Create)) {
                Part(zip,"[Content_Types].xml",Types("/word/document.xml","application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml"));
                Part(zip,"_rels/.rels",Relationships("rId1","officeDocument","word/document.xml"));
                Part(zip,"word/document.xml",new XDocument(new XElement(w+"document",new XAttribute(XNamespace.Xmlns+"w",w),body)));
            }
        }
        private static string Column(int index)
        {string result="";for(int n=index+1;n>0;n=(n-1)/26)result=(char)('A'+(n-1)%26)+result;return result;}
        private static void Excel(DataTable data,string path)
        {
            XNamespace s="http://schemas.openxmlformats.org/spreadsheetml/2006/main",r="http://schemas.openxmlformats.org/officeDocument/2006/relationships";
            var rows=new XElement(s+"sheetData");
            Func<object,int,int,XElement> cell=(value,col,row)=>{
                bool numeric=value is double||value is decimal||value is float||value is long||value is int;
                return new XElement(s+"c",new XAttribute("r",Column(col)+row),new XAttribute("t",numeric?"n":"inlineStr"),numeric ? new XElement(s+"v",Convert.ToString(value,CultureInfo.InvariantCulture)) : new XElement(s+"is",new XElement(s+"t",Display(value))));
            };
            rows.Add(new XElement(s+"row",new XAttribute("r",1),data.Columns.Cast<DataColumn>().Select((c,i)=>cell(c.ColumnName,i,1))));
            for(int i=0;i<data.Rows.Count;i++)rows.Add(new XElement(s+"row",new XAttribute("r",i+2),data.Rows[i].ItemArray.Select((v,j)=>cell(v,j,i+2))));
            int last=data.Rows.Count+2;
            var total=new XElement(s+"row",new XAttribute("r",last),cell("Итого",0,last));
            int money=data.Columns.IndexOf("Стоимость");
            if(money>=0)total.Add(cell(data.Rows.Cast<DataRow>().Sum(row=>Convert.ToDecimal(row["Стоимость"])),money,last));
            rows.Add(total);
            var sheet=new XElement(s+"worksheet",new XElement(s+"sheetViews",new XElement(s+"sheetView",new XAttribute("workbookViewId","0"),new XElement(s+"pane",new XAttribute("ySplit","1"),new XAttribute("topLeftCell","A2"),new XAttribute("activePane","bottomLeft"),new XAttribute("state","frozen")))),new XElement(s+"cols",data.Columns.Cast<DataColumn>().Select((c,i)=>new XElement(s+"col",new XAttribute("min",i+1),new XAttribute("max",i+1),new XAttribute("width",c.ColumnName=="Товар"||c.ColumnName=="Поставщик"||c.ColumnName=="Категория"?28:18),new XAttribute("customWidth",1)))),rows,new XElement(s+"autoFilter",new XAttribute("ref","A1:"+Column(data.Columns.Count-1)+(data.Rows.Count+1))));
            using(var zip=ZipFile.Open(path,ZipArchiveMode.Create)) {
                Part(zip,"[Content_Types].xml",Types("/xl/workbook.xml","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml","/xl/worksheets/sheet1.xml","application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"));
                Part(zip,"_rels/.rels",Relationships("rId1","officeDocument","xl/workbook.xml"));
                Part(zip,"xl/_rels/workbook.xml.rels",Relationships("rId1","worksheet","worksheets/sheet1.xml"));
                Part(zip,"xl/workbook.xml",new XDocument(new XElement(s+"workbook",new XAttribute(XNamespace.Xmlns+"r",r),new XElement(s+"sheets",new XElement(s+"sheet",new XAttribute("name","Поставки"),new XAttribute("sheetId",1),new XAttribute(r+"id","rId1"))))));
                Part(zip,"xl/worksheets/sheet1.xml",new XDocument(sheet));
            }
        }
        private static void Pdf(DataTable data,string path,string description)
        {
            using(var document=new PdfDocument()) {
                document.Info.Title="Остатки строительного магазина";
                var font=new XFont("Arial",9,XFontStyle.Regular);var bold=new XFont("Arial",10,XFontStyle.Bold);
                var heading=new XFont("Arial",17,XFontStyle.Bold);
                double[] widths={205,175,75,80,105,115};
                PdfPage page=null;XGraphics graphics=null;double y=0;
                Action newPage=()=>{
                    if(graphics!=null)graphics.Dispose();page=document.AddPage();page.Size=PageSize.A4;page.Orientation=PageOrientation.Landscape;
                    graphics=XGraphics.FromPdfPage(page);y=32;
                    graphics.DrawString("Остатки и сроки годности",heading,XBrushes.DarkSlateBlue,new XRect(32,y,760,25),XStringFormats.TopLeft);y+=30;
                    graphics.DrawString("Сформировано: "+DateTime.Now.ToString("dd.MM.yyyy HH:mm"),font,XBrushes.Black,new XRect(32,y,760,16),XStringFormats.TopLeft);y+=22;
                    double x=32;for(int i=0;i<data.Columns.Count;i++){graphics.DrawRectangle(XBrushes.LightGray,x,y,widths[i],25);graphics.DrawString(data.Columns[i].ColumnName,bold,XBrushes.Black,new XRect(x+4,y+3,widths[i]-8,20),XStringFormats.TopLeft);x+=widths[i];}y+=25;
                };
                newPage();
                foreach(DataRow row in data.Rows) {
                    // Wrap variable-length names; never silently truncate product names.
                    var texts=row.ItemArray.Select(Display).ToArray();
                    var wrapped=texts.Select((text,i)=>Wrap(graphics,font,text,widths[i]-8)).ToArray();
                    double height=Math.Max(25,wrapped.Max(a=>a.Length)*13+8);
                    if(y+height>page.Height.Point-80)newPage();
                    double x=32;
                    for(int i=0;i<texts.Length;i++) {
                        graphics.DrawRectangle(XPens.LightGray,x,y,widths[i],height);
                        for(int j=0;j<wrapped[i].Length;j++)graphics.DrawString(wrapped[i][j],font,XBrushes.Black,new XRect(x+4,y+4+j*13,widths[i]-8,13),XStringFormats.TopLeft);
                        x+=widths[i];
                    }
                    y+=height;
                }
                if(y+60>page.Height.Point-32)newPage();
                decimal value=data.Rows.Cast<DataRow>().Sum(row=>Convert.ToDecimal(row["Стоимость"]));
                graphics.DrawString("Стоимость остатков по розничным ценам: "+value.ToString("N2")+" руб.",bold,XBrushes.Black,new XRect(32,y+12,760,20),XStringFormats.TopLeft);
                graphics.DrawString(description,font,XBrushes.Black,new XRect(32,y+35,760,20),XStringFormats.TopLeft);
                graphics.Dispose();document.Save(path);
            }
        }
        private static string[] Wrap(XGraphics g,XFont font,string text,double width)
        {
            var lines=new System.Collections.Generic.List<string>();string current="";
            foreach(char c in text) {
                if(current.Length>0&&g.MeasureString(current+c,font).Width>width){lines.Add(current);current="";}
                current+=c;
            }
            lines.Add(current);return lines.ToArray();
        }
    }
}
