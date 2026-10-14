using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    // Integration checks run on an isolated database; the user's database is untouched.
    internal static class Verification
    {
        public static bool Active;
        public static bool AutomaticLoading;
        public static string LastError;
        public static int Run(string output)
        {
            Active=true;
            Directory.CreateDirectory(output);
            var log=new List<string>();string original=Database.PathName;
            Database.PathName=Path.Combine(output,"test-"+Guid.NewGuid().ToString("N")+".sqlite");
            Action<bool,string> check=(ok,name)=>{if(!ok)throw new Exception("FAIL: "+name);log.Add("PASS: "+name);File.WriteAllLines(Path.Combine(output,"verification.txt"),log);};
            Action<Action,string> rejected=(action,name)=>{bool failed=false;try{action();}catch{failed=true;}check(failed,name);};
            try {
                Database.Initialize();
                check(Database.Query("PRAGMA integrity_check").Rows[0][0].ToString()=="ok","SQLite integrity");
                check(Database.Query("PRAGMA foreign_key_check").Rows.Count==0,"Foreign keys");
                check(Convert.ToDecimal(Database.Query("SELECT Quantity FROM Stock WHERE ProductID=1").Rows[0][0])==91,"Seed stock = supply + returns - sales - writeoffs");
                var category=Catalog.Get("ProductCategory");
                long id=Database.Save(category,null,new Dictionary<string,object>{{"Name","Тестовая категория"}},null);
                Database.Save(category,id,new Dictionary<string,object>{{"Name","Изменённая категория"}},null);
                check(Database.List(category,"Изменённая").Rows.Count==1,"Insert, update and Cyrillic search");
                check(Database.List(category,"ИЗМЕНЁННАЯ").Rows.Count==1,"Case-insensitive Cyrillic search");
                check(Database.List(category,"' OR 1=1 --").Rows.Count==0,"Parameterized search");
                Database.Delete(category,id);check(Database.List(category,"Изменённая").Rows.Count==0,"Delete dictionary record");
                rejected(()=>Database.Delete(Catalog.Get("Product"),1),"Referenced product deletion rejected");
                rejected(()=>Database.Delete(Catalog.Get("Supply"),1),"Stock-breaking supply deletion rejected");
                check(Database.Lines(Catalog.Get("Supply"),1).Rows.Count==2,"Failed document deletion rolls back its lines");
                var sale=Catalog.Get("Sale");var lines=EditForm.NewLines(sale);lines.Rows.Add(1L,1000m,650m,650000m);
                var values=new Dictionary<string,object>{{"EmployeeID",1L},{"SaleDate","2026-10-01"},{"PaymentMethod","Наличные"},{"MaskedCard",null}};
                rejected(()=>Database.Save(sale,null,values,lines),"Overselling rejected");
                check(Database.Query("SELECT * FROM Sale").Rows.Count==2,"Failed insert rolls back header");
                lines.Rows.Clear();lines.Rows.Add(1L,3m,650m,1950m);
                long saleId=Database.Save(sale,null,values,lines);
                check(Convert.ToDecimal(Database.Query("SELECT Quantity FROM Stock WHERE ProductID=1").Rows[0][0])==88,"Sale decreases stock");
                var ret=Catalog.Get("Return");var retLines=EditForm.NewLines(ret);retLines.Rows.Add(1L,4m,0m);
                var retValues=new Dictionary<string,object>{{"SaleID",saleId},{"EmployeeID",1L}};
                rejected(()=>Database.Save(ret,null,retValues,retLines),"Return above sold quantity rejected");
                retLines.Rows.Clear();retLines.Rows.Add(2L,1m,0m);
                rejected(()=>Database.Save(ret,null,retValues,retLines),"Return of unrelated product rejected");
                retLines.Rows.Clear();retLines.Rows.Add(1L,1m,0m);
                long retId=Database.Save(ret,null,retValues,retLines);
                check(Convert.ToDecimal(Database.Lines(ret,retId).Rows[0]["Sum"])==650,"Return uses receipt price");
                rejected(()=>Database.Delete(sale,saleId),"Sale with return cannot be deleted");
                Database.Delete(ret,retId);Database.Delete(sale,saleId);
                check(Convert.ToDecimal(Database.Query("SELECT Quantity FROM Stock WHERE ProductID=1").Rows[0][0])==91,"Document deletion restores stock");
                var totals=Database.Query("SELECT (SELECT SUM(Sum) FROM SaleProduct)-(SELECT SUM(Sum) FROM ReturnProduct)-(SELECT SUM(Quantity*SupplierPrice) FROM SupplyProduct)");
                check(Convert.ToDecimal(totals.Rows[0][0])==-93800,"Income = sales - returns - purchases");
                foreach(ReportKind kind in Enum.GetValues(typeof(ReportKind))) {
                    var data=Reports.Load(kind,new DateTime(2026,1,1),new DateTime(2026,12,31));
                    check(data.Rows.Count>0,kind+" multi-table query");
                    string ext=kind==ReportKind.Sales?"docx":kind==ReportKind.Supplies?"xlsx":"pdf";
                    Reports.Export(kind,data,Path.Combine(output,kind+"."+ext),"Проверочный отчёт");
                    check(new FileInfo(Path.Combine(output,kind+"."+ext)).Length>100,kind+" export");
                    var empty=data.Clone();Reports.Export(kind,empty,Path.Combine(output,kind+"-empty."+ext),"Нет данных");
                    check(new FileInfo(Path.Combine(output,kind+"-empty."+ext)).Length>100,kind+" empty export");
                }
                using(var c=Database.Open())Database.Validate(c,null);
                VerifyForms(output,check);
                log.Add("ALL CHECKS PASSED");File.WriteAllLines(Path.Combine(output,"verification.txt"),log);
                return 0;
            }catch(Exception ex){log.Add(ex.ToString());File.WriteAllLines(Path.Combine(output,"verification.txt"),log);return 1;}
            finally{Database.PathName=original;Active=false;}
        }
        private static void VerifyForms(string output,Action<bool,string> check)
        {
            Application.EnableVisualStyles();
            Exception failure=null;
            using(var uiHost=new Form {Opacity=0,ShowInTaskbar=false,StartPosition=FormStartPosition.Manual,Location=new Point(-20000,-20000)}) {
                uiHost.Shown+=async(s,a)=>{try{await VerifyFormsAsync(output,check);}catch(Exception ex){failure=ex;}finally{uiHost.Close();}};
                Application.Run(uiHost);
            }
            if(failure!=null)throw failure;
        }
        private static async Task VerifyFormsAsync(string output,Action<bool,string> check)
        {
            var lookups=Catalog.All.ToDictionary(e=>e.Table,e=>Database.Lookup(e.Table));
            var products=Database.Query("SELECT ProductID,Price FROM Product");
            foreach(var e in Catalog.All) {
                using(var list=new DataForm(e)) {
                    HiddenShow(list);
                    var task=(Task)typeof(DataForm).GetMethod("Reload",BindingFlags.NonPublic|BindingFlags.Instance).Invoke(list,null);
                    await task;
                    check(true,e.Table+" list loads on UI thread");
                    var grid=(DataGridView)typeof(DataForm).GetField("grid",BindingFlags.NonPublic|BindingFlags.Instance).GetValue(list);
                    check(grid.Columns.Count>0,e.Table+" data grid columns");
                    check(LastError==null,e.Table+" loads without UI errors: "+LastError);
                    VerifyResize(list,grid,check,e.Table+" list");
                }
                var row=Database.Query("SELECT * FROM "+Catalog.Q(e.Table)+" LIMIT 1").Rows[0];
                using(var editor=new EditForm(e,Convert.ToInt64(row[e.Key]),row,e.Detail==null?null:Database.Lines(e,Convert.ToInt64(row[e.Key])),lookups,products)) {
                    HiddenShow(editor);editor.PerformLayout();
                    using(var image=new Bitmap(editor.Width,editor.Height)){
                        editor.DrawToBitmap(image,new Rectangle(0,0,editor.Width,editor.Height));
                        image.Save(Path.Combine(output,e.Table+"-editor.png"));
                    }
                    check(editor.Controls.OfType<TableLayoutPanel>().Single().Controls.Count>=3,e.Table+" editor layout");
                    var lineGrid=Descendants(editor).OfType<DataGridView>().FirstOrDefault();
                    VerifyResize(editor,lineGrid,check,e.Table+" editor");
                    var inputs=(Dictionary<string,Control>)typeof(EditForm).GetField("inputs",BindingFlags.NonPublic|BindingFlags.Instance).GetValue(editor);
                    foreach(var field in e.Fields.Where(f=>f.Lookup!=null))check(Convert.ToInt64(((ComboBox)inputs[field.Name]).SelectedValue)==Convert.ToInt64(row[field.Name]),e.Table+" selected "+field.Name);
                    var saving=(Task)typeof(EditForm).GetMethod("SaveAsync",BindingFlags.NonPublic|BindingFlags.Instance).Invoke(editor,null);
                    await saving;
                    check(editor.DialogResult==DialogResult.OK,e.Table+" saves through editor: "+LastError);
                }
            }
            AutomaticLoading=true;
            try {
                using(var main=new Form1()) {
                    HiddenShow(main);
                    DateTime deadline=DateTime.UtcNow.AddSeconds(15);
                    DataGridView productGrid=null;
                    while(DateTime.UtcNow<deadline) {
                        var child=main.MdiChildren.OfType<DataForm>().FirstOrDefault();
                        if(child!=null)productGrid=(DataGridView)typeof(DataForm).GetField("grid",BindingFlags.NonPublic|BindingFlags.Instance).GetValue(child);
                        if(productGrid!=null&&productGrid.Rows.Count>0)break;
                        await Task.Delay(20);
                    }
                    check(main.IsMdiContainer&&main.MainMenuStrip.Items.Count>=5,"MDI main window and menus");
                    check(productGrid!=null&&productGrid.Rows.Count==3,"Real application startup loads products");
                    check(LastError==null,"Application startup without UI errors: "+LastError);
                    using(var image=new Bitmap(main.Width,main.Height)){main.DrawToBitmap(image,new Rectangle(0,0,main.Width,main.Height));image.Save(Path.Combine(output,"main-window.png"));}
                }
            }finally{AutomaticLoading=false;}
            foreach(ReportKind kind in Enum.GetValues(typeof(ReportKind)))using(var form=new ReportForm(kind)){
                HiddenShow(form);
                var view=Descendants(form).OfType<DataGridView>().Single();
                DateTime deadline=DateTime.UtcNow.AddSeconds(10);
                while(view.DataSource==null&&DateTime.UtcNow<deadline)await Task.Delay(20);
                check(view.DataSource!=null,kind+" report window loads");
                VerifyResize(form,view,check,kind+" report");
            }
        }
        private static System.Collections.Generic.IEnumerable<Control> Descendants(Control control)
        {foreach(Control child in control.Controls){yield return child;foreach(var nested in Descendants(child))yield return nested;}}
        private static void VerifyResize(Form form,DataGridView grid,Action<bool,string> check,string name)
        {
            Size original=form.Size;
            foreach(Size size in new[]{form.MinimumSize,new Size(original.Width+170,original.Height+90)}) {
                form.Size=size;form.PerformLayout();
                check(Descendants(form).OfType<Button>().All(button=>button.Width>=90&&button.Height>=30),name+" buttons remain usable after resize");
                if(grid!=null)check(grid.Width>200&&grid.Height>=60&&grid.Right<=grid.Parent.ClientSize.Width&&grid.Bottom<=grid.Parent.ClientSize.Height,name+" grid fits after resize ("+grid.Bounds+", parent "+grid.Parent.ClientSize+", window "+form.Size+")");
            }
            form.Size=original;form.PerformLayout();
        }
        private static void HiddenShow(Form form)
        { form.Opacity=0;form.ShowInTaskbar=false;form.StartPosition=FormStartPosition.Manual;form.Location=new Point(-20000,-20000);form.Show(); }
    }
}
