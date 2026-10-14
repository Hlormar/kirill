using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal sealed class DataForm : Form
    {
        private readonly Entity entity;
        private readonly DataGridView grid=MakeGrid(), detail=MakeGrid();
        private readonly ToolStrip tools=new ToolStrip();
        private readonly ToolStripTextBox search=new ToolStripTextBox {AutoSize=false,Width=250};
        private readonly ToolStripLabel status=new ToolStripLabel();
        private readonly Timer timer=new Timer { Interval=350 };
        private Dictionary<string,DataTable> lookups;
        private int revision, detailRevision;
        private bool binding;
        private bool sizedToContent;
        public DataForm(Entity e)
        {
            entity=e;Text=e.Title;Ui.Form(this);
            ClientSize=Ui.WindowSize(this,e.Table=="Employee"?1180:e.Detail!=null?1020:860,e.Detail!=null?570:410);
            MinimumSize=new Size(640,e.Detail!=null?450:290);
            tools.GripStyle=ToolStripGripStyle.Hidden;tools.BackColor=Color.White;
            tools.Padding=new Padding(8,6,8,6);tools.Stretch=true;
            tools.Items.Add(Button("Добавить",async()=>await EditAsync(null)));
            tools.Items.Add(Button("Редактировать",async()=>await EditSelected()));
            tools.Items.Add(Button("Удалить",async()=>await DeleteSelected()));
            tools.Items.Add(new ToolStripSeparator());
            var searchLabel=new ToolStripLabel("Поиск: "+Catalog.Caption(e.Search));
            tools.Items.Add(Button("Обновить",Reload));
            tools.Items.Add(Button("Закрыть",()=>{Close();return Task.CompletedTask;}));
            var filter=new ToolStrip {GripStyle=ToolStripGripStyle.Hidden,BackColor=Ui.Background,Padding=new Padding(10,5,10,5),Stretch=true,Dock=DockStyle.Fill};
            filter.Items.Add(searchLabel);filter.Items.Add(search);filter.Items.Add(status);
            status.Alignment=ToolStripItemAlignment.Right;status.Margin=new Padding(10,0,0,0);
            var layout=new TableLayoutPanel {Dock=DockStyle.Fill,ColumnCount=1,RowCount=4,Padding=new Padding(12),Margin=Padding.Empty};
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));layout.RowStyles.Add(new RowStyle(SizeType.Percent,100));
            layout.Controls.Add(Ui.Heading(e.Title,e.Detail==null?"Справочник • добавление и редактирование записей":"Документы • реквизиты и состав в одном окне"),0,0);
            tools.Dock=DockStyle.Fill;layout.Controls.Add(tools,0,1);layout.Controls.Add(filter,0,2);
            if(e.Detail!=null) {
                var split=new SplitContainer { Dock=DockStyle.Fill, Orientation=Orientation.Horizontal, Height=380, SplitterDistance=210,Panel1MinSize=90,Panel2MinSize=90,SplitterWidth=8,BackColor=Ui.Background };
                split.Resize+=(s,a)=>{int available=split.ClientSize.Height-split.SplitterWidth;if(available>=180)split.SplitterDistance=Math.Max(90,Math.Min(available-90,(int)(available*0.56)));};
                split.Panel1.Controls.Add(grid); split.Panel2.Controls.Add(detail);
                split.Panel2.Controls.Add(new Label { Text="Состав выбранного документа", Dock=DockStyle.Top, Height=32, Padding=new Padding(8,6,8,6),BackColor=Color.White,Font=new Font(Font,FontStyle.Bold) });
                layout.Controls.Add(split,0,3);
            } else layout.Controls.Add(grid,0,3);
            Controls.Add(layout);
            filter.SizeChanged+=(s,a)=>search.Width=Math.Max(120,Math.Min(350,filter.Width-searchLabel.Width-status.Width-75));
            search.TextChanged+=(s,a)=>{timer.Stop();timer.Start();};
            timer.Tick+=async(s,a)=>{timer.Stop();await Reload();};
            FormClosed+=(s,a)=>{timer.Dispose();revision++;detailRevision++;};
            Shown+=async(s,a)=>{if(!Verification.Active||Verification.AutomaticLoading)await Reload();};
            grid.SelectionChanged+=(s,a)=>{
                if(!binding&&IsHandleCreated&&!IsDisposed)BeginInvoke(new Action(async()=>await LoadDetails()));
            };
            grid.CellDoubleClick+=async(s,a)=>{if(a.RowIndex>=0) await EditSelected();};
        }
        private ToolStripButton Button(string text,Func<Task> action)
        {
            var b=new ToolStripButton(text);
            b.Click+=async(s,e)=>{try{await action();}catch(Exception ex){Error(ex);}}; return b;
        }
        internal static DataGridView MakeGrid()
        { var view=new DataGridView { Dock=DockStyle.Fill, ReadOnly=true, AllowUserToAddRows=false, AllowUserToDeleteRows=false, MultiSelect=false, SelectionMode=DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill, RowHeadersVisible=false };Ui.Grid(view);return view; }
        private long? Selected()
        {
            var row=grid.CurrentRow == null ? null : grid.CurrentRow.DataBoundItem as DataRowView;
            return row==null ? (long?)null : Convert.ToInt64(row[entity.Key]);
        }
        private async Task Reload()
        {
            int version=++revision; string text=search.Text;
            status.Text="Загрузка…";
            try {
                var loaded=await Task.Run(()=>new { Rows=Database.List(entity,text), Lookups=Catalog.All.ToDictionary(e=>e.Table,e=>Database.Lookup(e.Table)) });
                if(IsDisposed || version!=revision) return;
                lookups=loaded.Lookups;
                binding=true;
                try{Bind(grid,loaded.Rows,entity.Fields);}finally{binding=false;}
                status.Text="Записей: "+loaded.Rows.Rows.Count;
                await LoadDetails();
                if(!sizedToContent&&!IsDisposed){
                    sizedToContent=true;
                    int height=entity.Detail==null?ClientSize.Height-grid.Height+grid.ColumnHeadersHeight+Math.Max(2,Math.Min(12,loaded.Rows.Rows.Count))*32+SystemInformation.HorizontalScrollBarHeight+12:
                        280+Math.Max(3,Math.Min(8,loaded.Rows.Rows.Count))*32+Math.Max(3,Math.Min(6,detail.Rows.Count))*32;
                    ClientSize=Ui.WindowSize(this,ClientSize.Width,height);
                }
            }catch(Exception ex){if(!IsDisposed && version==revision){status.Text="Ошибка загрузки";Error(ex);}}
        }
        private void Bind(DataGridView target,DataTable data,Field[] fields)
        {
            if(target.InvokeRequired)throw new InvalidOperationException("UI binding was called outside the UI thread.");
            target.DataSource=null; target.Columns.Clear(); target.AutoGenerateColumns=false;
            foreach(DataColumn column in data.Columns) {
                var f=fields.FirstOrDefault(x=>x.Name==column.ColumnName);
                bool primary=target==grid&&column.ColumnName==entity.Key;
                string lookup=primary ? null : f==null ? (column.ColumnName=="ProductID" ? "Product" : null) : f.Lookup;
                DataGridViewColumn view;
                if(lookup!=null && lookups!=null) view=new DataGridViewComboBoxColumn { DataSource=lookups[lookup], ValueMember="Id", DisplayMember="Label", DisplayStyle=DataGridViewComboBoxDisplayStyle.Nothing };
                else view=new DataGridViewTextBoxColumn();
                view.Name=column.ColumnName; view.DataPropertyName=column.ColumnName;
                view.HeaderText=primary ? "№" : Catalog.Caption(column.ColumnName); view.MinimumWidth=70;
                if(column.ColumnName=="Sum" || column.ColumnName=="Price" || column.ColumnName=="SupplierPrice") view.DefaultCellStyle.Format="N2";
                target.Columns.Add(view);
            }
            target.DataSource=data;
            Ui.Columns(target);
        }
        private async Task LoadDetails()
        {
            if(entity.Detail==null || lookups==null) return;
            int version=++detailRevision; var id=Selected();
            if(!id.HasValue){detail.DataSource=null;return;}
            try {
                var rows=await Task.Run(()=>Database.Lines(entity,id.Value));
                if(IsDisposed || version!=detailRevision) return;
                Bind(detail,rows,new Field[0]);
                if(detail.Columns.Contains(entity.Key)) detail.Columns[entity.Key].Visible=false;
            }catch(Exception ex){if(!IsDisposed) Error(ex);}
        }
        private async Task EditSelected()
        { var id=Selected(); if(id.HasValue) await EditAsync(id); else MessageBox.Show(this,"Выберите запись.",Text); }
        private async Task EditAsync(long? id)
        {
            tools.Enabled=false;
            try {
                var loaded=await Task.Run(()=>new {
                    Row=id.HasValue ? Database.Query("SELECT * FROM "+Catalog.Q(entity.Table)+" WHERE "+Catalog.Q(entity.Key)+"=@p0",id.Value).Rows[0] : null,
                    Lines=entity.Detail!=null && id.HasValue ? Database.Lines(entity,id.Value) : null,
                    Lookups=Catalog.All.ToDictionary(e=>e.Table,e=>Database.Lookup(e.Table)),
                    Products=Database.Query("SELECT ProductID,Price FROM Product")
                });
                if(IsDisposed) return;
                using(var editor=new EditForm(entity,id,loaded.Row,loaded.Lines,loaded.Lookups,loaded.Products)) {
                    if(editor.ShowDialog(this)==DialogResult.OK) await Reload();
                }
            } finally {if(!IsDisposed) tools.Enabled=true;}
        }
        private async Task DeleteSelected()
        {
            var id=Selected(); if(!id.HasValue) return;
            if(MessageBox.Show(this,"Удалить запись №"+id+" и её состав?","Удаление",MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes) return;
            tools.Enabled=false;
            try{await Task.Run(()=>Database.Delete(entity,id.Value));await Reload();}
            finally{if(!IsDisposed) tools.Enabled=true;}
        }
        private void Error(Exception ex) {if(Verification.Active){Verification.LastError=ex.ToString();return;}MessageBox.Show(this,Database.Friendly(ex),"Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Warning);}
    }
}
