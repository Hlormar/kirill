using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly MenuStrip menu=new MenuStrip();
        private readonly ToolStripStatusLabel status=new ToolStripStatusLabel("Подготовка базы данных…");
        public Form1()
        {
            InitializeComponent();
            Ui.Form(this);
            Text="Строительный магазин — ПЗ 5–6";IsMdiContainer=true;
            Size=Ui.WindowSize(this,1180,760);MinimumSize=new Size(820,530);StartPosition=FormStartPosition.CenterScreen;
            menu.BackColor=Color.White;menu.Padding=new Padding(12,7,12,7);
            var refs=new ToolStripMenuItem("Справочники");var docs=new ToolStripMenuItem("Документы");
            foreach(var entity in Catalog.All) {
                var item=new ToolStripMenuItem(entity.Title);item.Click+=(s,e)=>OpenData(entity);
                (entity.Detail==null?refs:docs).DropDownItems.Add(item);
            }
            var reports=new ToolStripMenuItem("Отчёты");
            Add(reports,"Продажи — Word (.docx)",()=>OpenReport(ReportKind.Sales));
            Add(reports,"Поставки — Excel (.xlsx)",()=>OpenReport(ReportKind.Supplies));
            Add(reports,"Остатки и сроки годности — PDF",()=>OpenReport(ReportKind.Stock));
            Add(reports,"Доход за всё время",async()=>{
                try {
                    var data=await Task.Run(()=>Database.Query(@"SELECT
                        COALESCE((SELECT SUM(Sum) FROM SaleProduct),0) AS Sales,
                        COALESCE((SELECT SUM(Sum) FROM ReturnProduct),0) AS Returns,
                        COALESCE((SELECT SUM(Quantity*SupplierPrice) FROM SupplyProduct),0) AS Purchases"));
                    var row=data.Rows[0];decimal sales=Convert.ToDecimal(row["Sales"]),returns=Convert.ToDecimal(row["Returns"]),purchases=Convert.ToDecimal(row["Purchases"]);
                    MessageBox.Show(this,"Продажи: "+sales.ToString("N2")+" руб.\nВозвраты: "+returns.ToString("N2")+" руб.\nЗакупки: "+purchases.ToString("N2")+" руб.\n\nДоход = продажи − возвраты − закупки:\n"+(sales-returns-purchases).ToString("N2")+" руб.","Доход за всё время");
                } catch(Exception ex){MessageBox.Show(this,Database.Friendly(ex),"Ошибка расчёта");}
            });
            var windows=new ToolStripMenuItem("Окна");
            Add(windows,"Каскадом",()=>LayoutMdi(MdiLayout.Cascade));Add(windows,"Рядом",()=>LayoutMdi(MdiLayout.TileVertical));
            var help=new ToolStripMenuItem("Справка");
            Add(help,"О программе",()=>MessageBox.Show(this,"ПЗ 5–6. Строительный магазин\nШляхтин Даниил Константинович\nЕрмолаев Константин Дмитриевич\nГруппа 24.02\n\nДобавляйте справочники, затем поставки и продажи.\nСостав документа редактируется вместе с его реквизитами.\nВозврат привязан к исходной продаже.\n\nБаза данных: "+Database.PathName,"О программе"));
            menu.Items.AddRange(new ToolStripItem[]{refs,docs,reports,windows,help});menu.MdiWindowListItem=windows;
            menu.Enabled=false;MainMenuStrip=menu;Controls.Add(menu);
            var bar=new StatusStrip {BackColor=Color.White,Padding=new Padding(10,4,10,4)};bar.Items.Add(status);Controls.Add(bar);
            status.AutoSize=false;status.Height=24;status.TextAlign=ContentAlignment.MiddleLeft;
            bar.Resize+=(s,e)=>status.Width=Math.Max(100,bar.ClientSize.Width-40);
            status.Width=ClientSize.Width-40;status.AutoToolTip=true;
            foreach(var client in Controls.OfType<MdiClient>())client.BackColor=Ui.Background;
            Shown+=async(s,e)=>{
                try{await Task.Run(()=>Database.Initialize());menu.Enabled=true;status.Text="Готово. База: "+Database.PathName;OpenData(Catalog.Get("Product"));}
                catch(Exception ex){status.Text="Ошибка базы данных";MessageBox.Show(this,Database.Friendly(ex),"Не удалось запустить приложение",MessageBoxButtons.OK,MessageBoxIcon.Error);}
            };
        }
        private static void Add(ToolStripMenuItem root,string title,Action action)
        {var item=new ToolStripMenuItem(title);item.Click+=(s,e)=>action();root.DropDownItems.Add(item);}
        private void OpenData(Entity entity)
        {
            var old=MdiChildren.FirstOrDefault(f=>f.Tag as string==entity.Table);
            if(old!=null){old.Activate();return;}
            new DataForm(entity){MdiParent=this,Tag=entity.Table}.Show();
        }
        private void OpenReport(ReportKind kind) {new ReportForm(kind){MdiParent=this}.Show();}
    }
}
