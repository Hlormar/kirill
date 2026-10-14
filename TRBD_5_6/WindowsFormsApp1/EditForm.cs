using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal sealed class EditForm : Form
    {
        private readonly Entity entity;
        private readonly long? id;
        private readonly Dictionary<string,Control> inputs=new Dictionary<string,Control>();
        private readonly DataTable lines;
        private readonly DataGridView grid;
        private readonly Button save=new Button {Text="Сохранить",AutoSize=true};
        private bool saving;
        public EditForm(Entity e,long? key,DataRow row,DataTable source,Dictionary<string,DataTable> lookups,DataTable products)
        {
            entity=e; id=key; Text=(key.HasValue?"Редактирование — ":"Добавление — ")+e.Title;
            Ui.Form(this);StartPosition=FormStartPosition.CenterParent;MinimizeBox=false;ShowInTaskbar=false;
            ClientSize=Ui.WindowSize(this,e.Detail==null?580:920,300);
            var layout=new TableLayoutPanel {Dock=DockStyle.Fill,ColumnCount=1,RowCount=e.Detail==null?3:4,Padding=new Padding(12)};
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(e.Detail==null?SizeType.Percent:SizeType.AutoSize,e.Detail==null?100:0));
            if(e.Detail!=null)layout.RowStyles.Add(new RowStyle(SizeType.Percent,100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(Ui.Heading(key.HasValue?"Редактирование записи":"Новая запись",e.Title+" • обязательные поля отмечены звёздочкой"),0,0);
            var fields=new TableLayoutPanel {Dock=DockStyle.Top,AutoSize=true,AutoSizeMode=AutoSizeMode.GrowAndShrink,ColumnCount=2,Padding=new Padding(12,8,12,8),BackColor=Color.White};
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,165)); fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));
            foreach(var f in e.Fields) {
                Control input;
                object value=row==null ? DBNull.Value : row[f.Name];
                if(f.Lookup!=null) {
                    var combo=new ComboBox {BindingContext=new BindingContext(),DropDownStyle=ComboBoxStyle.DropDownList,ValueMember="Id",DisplayMember="Label",DataSource=lookups[f.Lookup].Copy()};
                    if(value!=DBNull.Value) combo.SelectedValue=value;
                    input=combo;
                } else if(f.Date) {
                    var date=new DateTimePicker {Format=DateTimePickerFormat.Custom,CustomFormat="yyyy-MM-dd",ShowCheckBox=f.Optional,Checked=!f.Optional};
                    if(value!=DBNull.Value){date.Value=DateTime.Parse(Convert.ToString(value),CultureInfo.InvariantCulture);date.Checked=true;}
                    input=date;
                } else if(f.Number) {
                    input=new NumericUpDown {DecimalPlaces=2,Maximum=1000000000,Value=value==DBNull.Value?0:Convert.ToDecimal(value)};
                } else if(f.Name=="PaymentMethod") {
                    var combo=new ComboBox {DropDownStyle=ComboBoxStyle.DropDownList}; combo.Items.AddRange(new object[]{"Наличные","Карта","Перевод"});
                    combo.SelectedItem=value==DBNull.Value?"Наличные":value;
                    if(combo.SelectedIndex<0) {combo.Items.Add(value);combo.SelectedItem=value;}
                    input=combo;
                } else input=new TextBox {Text=value==DBNull.Value?"":Convert.ToString(value),MaxLength=250};
                input.Anchor=AnchorStyles.Left|AnchorStyles.Right;input.Margin=new Padding(4,7,4,7);
                if(input is TextBox)((TextBox)input).BorderStyle=BorderStyle.FixedSingle;
                inputs[f.Name]=input;
                int index=fields.RowCount++;
                fields.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                fields.Controls.Add(new Label {Text=f.Title+(f.Optional?"":" *"),AutoSize=true,Anchor=AnchorStyles.Left,Margin=new Padding(4,8,4,8)},0,index);
                fields.Controls.Add(input,1,index);
            }
            var footer=new FlowLayoutPanel {Dock=DockStyle.Fill,AutoSize=true,AutoSizeMode=AutoSizeMode.GrowAndShrink,FlowDirection=FlowDirection.RightToLeft,WrapContents=false,Padding=new Padding(0,12,0,0)};
            var cancel=new Button {Text="Отмена",DialogResult=DialogResult.Cancel,AutoSize=true};
            Ui.Button(cancel);Ui.Button(save,true);
            footer.Controls.Add(cancel);footer.Controls.Add(save);CancelButton=cancel;AcceptButton=save;
            var fieldsHost=new Panel {Dock=DockStyle.Fill,AutoScroll=e.Detail==null,BackColor=Color.White,Margin=new Padding(0,8,0,0)};
            fieldsHost.Controls.Add(fields);layout.Controls.Add(fieldsHost,0,1);
            if(e.Detail!=null) {
                lines=NewLines(e);
                if(source!=null) foreach(DataRow r in source.Rows) {
                    var n=lines.NewRow(); foreach(DataColumn c in lines.Columns) if(source.Columns.Contains(c.ColumnName)) n[c.ColumnName]=r[c.ColumnName];
                    if(e.Table=="Sale") n["UnitPrice"]=Convert.ToDecimal(r["Sum"])/Convert.ToDecimal(r["Quantity"]);
                    lines.Rows.Add(n);
                }
                grid=new DataGridView {Dock=DockStyle.Fill,AutoGenerateColumns=false,AllowUserToAddRows=true,AllowUserToDeleteRows=true,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill,BackgroundColor=Color.White};
                Ui.Grid(grid);
                grid.Columns.Add(new DataGridViewComboBoxColumn {Name="ProductID",DataPropertyName="ProductID",HeaderText="Товар",DataSource=lookups["Product"].Copy(),ValueMember="Id",DisplayMember="Label",FillWeight=180});
                foreach(DataColumn c in lines.Columns) if(c.ColumnName!="ProductID") grid.Columns.Add(new DataGridViewTextBoxColumn {Name=c.ColumnName,DataPropertyName=c.ColumnName,HeaderText=Catalog.Caption(c.ColumnName)});
                if(e.Table=="Sale") grid.Columns["Sum"].ReadOnly=true;
                if(e.Table=="Return") grid.Columns["Sum"].Visible=false;
                grid.DataSource=lines;
                Ui.Columns(grid);
                grid.DefaultValuesNeeded+=(s,a)=>{a.Row.Cells["Quantity"].Value=1m;if(e.Table=="Supply")a.Row.Cells["SupplierPrice"].Value=0m;if(e.Table=="Sale"){a.Row.Cells["UnitPrice"].Value=0m;a.Row.Cells["Sum"].Value=0m;}};
                grid.CurrentCellDirtyStateChanged+=(s,a)=>{if(grid.IsCurrentCellDirty)grid.CommitEdit(DataGridViewDataErrorContexts.Commit);};
                grid.CellValueChanged+=(s,a)=>{
                    if(a.RowIndex<0 || a.ColumnIndex<0 || e.Table!="Sale") return;
                    var r=grid.Rows[a.RowIndex]; var name=grid.Columns[a.ColumnIndex].Name;
                    if(name=="ProductID" && r.Cells["ProductID"].Value!=DBNull.Value && r.Cells["ProductID"].Value!=null) {
                        var p=products.Select("ProductID="+Convert.ToInt64(r.Cells["ProductID"].Value));
                        if(p.Length>0)r.Cells["UnitPrice"].Value=Convert.ToDecimal(p[0]["Price"]);
                    }
                    if(name=="ProductID"||name=="Quantity"||name=="UnitPrice") {
                        decimal q,u;
                        if(decimal.TryParse(Convert.ToString(r.Cells["Quantity"].Value),out q)&&decimal.TryParse(Convert.ToString(r.Cells["UnitPrice"].Value),out u))r.Cells["Sum"].Value=Math.Round(q*u,2,MidpointRounding.AwayFromZero);
                    }
                };
                grid.DataError+=(s,a)=>{a.ThrowException=false;a.Cancel=true;if(Verification.Active){Verification.LastError=a.Exception.ToString();return;}MessageBox.Show(this,"Введите число или выберите товар из списка.","Проверка данных");};
                var content=new TableLayoutPanel {Dock=DockStyle.Fill,ColumnCount=1,RowCount=2,Margin=new Padding(0,10,0,0)};
                content.RowStyles.Add(new RowStyle(SizeType.AutoSize));content.RowStyles.Add(new RowStyle(SizeType.Percent,100));
                content.Controls.Add(new Label {Dock=DockStyle.Fill,AutoSize=true,Text="Состав документа\nДобавьте товар в последней строке. Delete удаляет выбранную позицию.",Padding=new Padding(0,0,0,8),ForeColor=Ui.Ink},0,0);
                content.Controls.Add(grid,0,1);layout.Controls.Add(content,0,2);
            }
            layout.Controls.Add(footer,0,e.Detail==null?2:3);Controls.Add(layout);
            int fieldsHeight=fields.GetPreferredSize(new Size(ClientSize.Width-24,0)).Height;
            if(e.Detail!=null){layout.RowStyles[1]=new RowStyle(SizeType.Absolute,fieldsHeight+8);fieldsHost.AutoScroll=true;}
            int lineHeight=e.Detail==null?0:100+Math.Max(3,Math.Min(7,(source==null?0:source.Rows.Count)+1))*32;
            ClientSize=Ui.WindowSize(this,e.Detail==null?580:920,fieldsHeight+142+lineHeight);
            MinimumSize=e.Detail==null?new Size(480,240):new Size(740,fieldsHeight+310);
            Load+=(s,a)=>{
                int actualFields=fields.GetPreferredSize(new Size(fieldsHost.ClientSize.Width,0)).Height;
                int top=layout.GetControlFromPosition(0,0).Height;
                int bottom=footer.GetPreferredSize(new Size(layout.ClientSize.Width,0)).Height;
                int chrome=top+bottom+layout.Padding.Vertical+28;
                if(e.Detail!=null) {
                    layout.RowStyles[1]=new RowStyle(SizeType.Absolute,actualFields+fieldsHost.Margin.Vertical);
                    MinimumSize=new Size(740,actualFields+chrome+200+Height-ClientSize.Height);
                }
                ClientSize=Ui.WindowSize(this,ClientSize.Width,actualFields+chrome+lineHeight);
                layout.PerformLayout();
            };
            save.Click+=async(s,a)=>await SaveAsync();
            FormClosing+=(s,a)=>{if(saving)a.Cancel=true;};
        }
        internal static DataTable NewLines(Entity e)
        {
            var data=new DataTable();data.Columns.Add("ProductID",typeof(long));data.Columns.Add("Quantity",typeof(decimal));
            if(e.Table=="Supply"){data.Columns.Add("ExpirationDate",typeof(string));data.Columns.Add("SupplierPrice",typeof(decimal));}
            if(e.Table=="Sale"){data.Columns.Add("UnitPrice",typeof(decimal));data.Columns.Add("Sum",typeof(decimal));}
            if(e.Table=="Return") data.Columns.Add("Sum",typeof(decimal));
            return data;
        }
        private async Task SaveAsync()
        {
            try {
                if(grid!=null){if(!grid.EndEdit())return;((CurrencyManager)BindingContext[lines]).EndCurrentEdit();}
                var values=new Dictionary<string,object>();
                foreach(var f in entity.Fields) {
                    var input=inputs[f.Name];object value;
                    if(f.Lookup!=null) value=((ComboBox)input).SelectedValue;
                    else if(f.Date){var d=(DateTimePicker)input;value=f.Optional&&!d.Checked?null:d.Value.ToString("yyyy-MM-dd");}
                    else if(f.Number)value=((NumericUpDown)input).Value;
                    else value=input.Text.Trim();
                    if(value is string && string.IsNullOrWhiteSpace((string)value))value=null;
                    if(value==null&&!f.Optional)throw new InvalidOperationException("Заполните поле «"+f.Title+"».");
                    values[f.Name]=value;
                }
                if(entity.Table=="Employee") {
                    string inn=Convert.ToString(values["INN"]);
                    if(inn.Length>0 && (inn.Length!=12||!inn.All(char.IsDigit)))throw new InvalidOperationException("ИНН сотрудника должен содержать 12 цифр.");
                }
                if(entity.Table=="Sale") {
                    string card=Convert.ToString(values["MaskedCard"]);
                    if(card.Length>0 && !System.Text.RegularExpressions.Regex.IsMatch(card,@"^\*{4} ?\d{4}$")) throw new InvalidOperationException("Маска карты: **** 1234. Полный номер карты не сохраняется.");
                }
                DataTable snapshot=null;
                if(lines!=null) {
                    snapshot=lines.Clone();
                    foreach(DataRow row in lines.Rows) if(row.RowState!=DataRowState.Deleted) {
                        if(row.IsNull("ProductID")||row.IsNull("Quantity"))throw new InvalidOperationException("Выберите товар и заполните количество во всех строках.");
                        if(entity.Table=="Supply") {
                            if(row.IsNull("SupplierPrice")||Convert.ToDecimal(row["SupplierPrice"])<0)throw new InvalidOperationException("Цена закупки должна быть неотрицательной.");
                            string expiry=Convert.ToString(row["ExpirationDate"]).Trim(); DateTime parsed;
                            if(expiry.Length>0&&!DateTime.TryParseExact(expiry,"yyyy-MM-dd",CultureInfo.InvariantCulture,DateTimeStyles.None,out parsed))throw new InvalidOperationException("Срок годности укажите в формате yyyy-MM-dd или оставьте пустым.");
                            row["ExpirationDate"]=expiry.Length==0?(object)DBNull.Value:expiry;
                        }
                        if(entity.Table=="Sale"&&row.IsNull("UnitPrice"))throw new InvalidOperationException("Заполните цену продажи.");
                        snapshot.ImportRow(row);
                    }
                }
                saving=true;save.Enabled=false;
                foreach(var control in inputs.Values)control.Enabled=false;
                if(grid!=null)grid.Enabled=false;
                await Task.Run(()=>Database.Save(entity,id,values,snapshot));
                saving=false;DialogResult=DialogResult.OK;Close();
            }catch(Exception ex){if(Verification.Active)Verification.LastError=ex.ToString();else MessageBox.Show(this,Database.Friendly(ex),"Не удалось сохранить",MessageBoxButtons.OK,MessageBoxIcon.Warning);}
            finally{saving=false;if(!IsDisposed){save.Enabled=true;foreach(var control in inputs.Values)control.Enabled=true;if(grid!=null)grid.Enabled=true;}}
        }
    }
}
