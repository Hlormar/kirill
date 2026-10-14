using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;

namespace WindowsFormsApp1
{
    [SQLiteFunction(Name="lower",Arguments=1,FuncType=FunctionType.Scalar)]
    internal sealed class UnicodeLower : SQLiteFunction
    {
        public override object Invoke(object[] args)
        { return args[0]==DBNull.Value ? (object)DBNull.Value : Convert.ToString(args[0]).ToLowerInvariant(); }
    }
    internal static class Database
    {
        static Database() {SQLiteFunction.RegisterFunction(typeof(UnicodeLower));}
        public static string PathName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Data","store.sqlite");
        public static SQLiteConnection Open()
        {
            var c = new SQLiteConnection(new SQLiteConnectionStringBuilder { DataSource=PathName, ForeignKeys=true, DefaultTimeout=15 }.ToString());
            c.Open(); return c;
        }
        public static void Initialize()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(PathName));
            if (File.Exists(PathName)) return;
            // A failed initialization must not leave an apparently valid empty database.
            try {
                using (var c=Open()) using(var t=c.BeginTransaction()) {
                    Exec(c,t,File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Database","schema.sql")));
                    Exec(c,t,File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Database","seed.sql")));
                    t.Commit();
                }
            } catch { SQLiteConnection.ClearAllPools(); if(File.Exists(PathName)) File.Delete(PathName); throw; }
        }
        public static SQLiteCommand Command(SQLiteConnection c, SQLiteTransaction t, string sql, params object[] values)
        {
            var cmd=new SQLiteCommand(sql,c,t);
            for(int i=0;i<values.Length;i++) cmd.Parameters.AddWithValue("@p"+i,values[i]??DBNull.Value);
            return cmd;
        }
        public static void Exec(SQLiteConnection c,SQLiteTransaction t,string sql,params object[] values)
        { using(var cmd=Command(c,t,sql,values)) cmd.ExecuteNonQuery(); }
        public static DataTable Query(string sql,params object[] values)
        { using(var c=Open()) return Query(c,null,sql,values); }
        public static DataTable Query(SQLiteConnection c,SQLiteTransaction t,string sql,params object[] values)
        {
            using(var cmd=Command(c,t,sql,values)) using(var a=new SQLiteDataAdapter(cmd)) {
                var data=new DataTable(); a.Fill(data); return data;
            }
        }
        public static DataTable Lookup(string table)
        {
            var e=Catalog.Get(table);
            string label=table=="Employee" ? "Surname || ' ' || Name || ' ' || COALESCE(Patronymic,'')" :
                table=="Sale" ? "'Продажа №' || SaleID || ' от ' || SaleDate" :
                e.Detail!=null ? "'Документ №' || "+Catalog.Q(e.Key) : "Name";
            return Query("SELECT "+Catalog.Q(e.Key)+" AS Id,"+label+" AS Label FROM "+Catalog.Q(table)+" ORDER BY Label");
        }
        public static DataTable List(Entity e,string search)
        {
            // Search is literal: apostrophes and LIKE metacharacters cannot alter the query.
            return Query("SELECT * FROM "+Catalog.Q(e.Table)+" WHERE instr(lower(CAST("+Catalog.Q(e.Search)+" AS TEXT)),lower(@p0))>0 ORDER BY "+Catalog.Q(e.Key)+" DESC", search??"");
        }
        public static DataTable Lines(Entity e,long id)
        { return Query("SELECT * FROM "+Catalog.Q(e.Detail)+" WHERE "+Catalog.Q(e.Key)+"=@p0 ORDER BY RecordID",id); }
        public static long Save(Entity e,long? id,Dictionary<string,object> values,DataTable lines)
        {
            using(var c=Open()) using(var t=c.BeginTransaction()) {
                long result=SaveHeader(c,t,e,id,values);
                if(e.Detail!=null) {
                    Exec(c,t,"DELETE FROM "+Catalog.Q(e.Detail)+" WHERE "+Catalog.Q(e.Key)+"=@p0",result);
                    if(lines==null || lines.Rows.Count==0) throw new InvalidOperationException("Добавьте хотя бы одну позицию документа.");
                    var seen=new HashSet<long>();
                    foreach(DataRow row in lines.Rows) {
                        long product=Convert.ToInt64(row["ProductID"]);
                        if(!seen.Add(product)) throw new InvalidOperationException("Товар не должен повторяться в документе: измените количество одной позиции.");
                        decimal qty=Convert.ToDecimal(row["Quantity"]);
                        if(qty<=0) throw new InvalidOperationException("Количество должно быть больше нуля.");
                        var fields=new List<string>{e.Key,"ProductID","Quantity"};
                        var args=new List<object>{result,product,qty};
                        if(e.Table=="Supply") {
                            fields.AddRange(new[]{"ExpirationDate","SupplierPrice"});
                            args.Add(row["ExpirationDate"]); args.Add(row["SupplierPrice"]);
                        }
                        if(e.Table=="Sale" || e.Table=="Return") {
                            decimal unit;
                            if(e.Table=="Sale") unit=Convert.ToDecimal(row["UnitPrice"]);
                            else {
                                var sold=Query(c,t,"SELECT Quantity,Sum FROM SaleProduct WHERE SaleID=@p0 AND ProductID=@p1",values["SaleID"],product);
                                if(sold.Rows.Count==0) throw new InvalidOperationException("Возвращать можно только товары из исходной продажи.");
                                unit=Convert.ToDecimal(sold.Rows[0]["Sum"])/Convert.ToDecimal(sold.Rows[0]["Quantity"]);
                            }
                            if(unit<0) throw new InvalidOperationException("Цена не может быть отрицательной.");
                            fields.Add("Sum"); args.Add(Math.Round(unit*qty,2,MidpointRounding.AwayFromZero));
                        }
                        Exec(c,t,"INSERT INTO "+Catalog.Q(e.Detail)+" ("+string.Join(",",fields.Select(Catalog.Q))+") VALUES ("+string.Join(",",args.Select((v,i)=>"@p"+i))+")",args.ToArray());
                    }
                }
                Validate(c,t);
                t.Commit(); return result;
            }
        }
        private static long SaveHeader(SQLiteConnection c,SQLiteTransaction t,Entity e,long? id,Dictionary<string,object> values)
        {
            var names=e.Fields.Select(f=>f.Name).ToArray();
            var args=names.Select(n=>values[n]).ToList();
            if(id.HasValue) {
                args.Add(id.Value);
                Exec(c,t,"UPDATE "+Catalog.Q(e.Table)+" SET "+string.Join(",",names.Select((n,i)=>Catalog.Q(n)+"=@p"+i))+" WHERE "+Catalog.Q(e.Key)+"=@p"+names.Length,args.ToArray());
                return id.Value;
            }
            Exec(c,t,"INSERT INTO "+Catalog.Q(e.Table)+" ("+string.Join(",",names.Select(Catalog.Q))+") VALUES ("+string.Join(",",names.Select((n,i)=>"@p"+i))+")",args.ToArray());
            using(var cmd=Command(c,t,"SELECT last_insert_rowid()")) return Convert.ToInt64(cmd.ExecuteScalar());
        }
        public static void Delete(Entity e,long id)
        {
            using(var c=Open()) using(var t=c.BeginTransaction()) {
                if(e.Detail!=null) Exec(c,t,"DELETE FROM "+Catalog.Q(e.Detail)+" WHERE "+Catalog.Q(e.Key)+"=@p0",id);
                Exec(c,t,"DELETE FROM "+Catalog.Q(e.Table)+" WHERE "+Catalog.Q(e.Key)+"=@p0",id);
                Validate(c,t); t.Commit();
            }
        }
        public static void Validate(SQLiteConnection c,SQLiteTransaction t)
        {
            var stock=Query(c,t,"SELECT Name FROM Stock WHERE Quantity < -0.000001 LIMIT 1");
            if(stock.Rows.Count>0) throw new InvalidOperationException("Недостаточно товара на складе: "+stock.Rows[0][0]+". Операция отменена.");
            var returns=Query(c,t,@"SELECT r.SaleID,rp.ProductID,SUM(rp.Quantity) AS Returned,
                COALESCE((SELECT Quantity FROM SaleProduct WHERE SaleID=r.SaleID AND ProductID=rp.ProductID),0) AS Sold,
                SUM(rp.Sum) AS ReturnedSum, COALESCE((SELECT Sum FROM SaleProduct WHERE SaleID=r.SaleID AND ProductID=rp.ProductID),0) AS SoldSum
                FROM ReturnProduct rp JOIN ""Return"" r ON r.ReturnID=rp.ReturnID
                GROUP BY r.SaleID,rp.ProductID HAVING Returned>Sold+0.000001 OR ReturnedSum>SoldSum+0.01");
            if(returns.Rows.Count>0) throw new InvalidOperationException("Возвраты превышают количество или сумму исходной продажи. Операция отменена.");
            var pricing=Query(c,t,@"SELECT rp.RecordID FROM ReturnProduct rp JOIN ""Return"" r ON r.ReturnID=rp.ReturnID
                JOIN SaleProduct sp ON sp.SaleID=r.SaleID AND sp.ProductID=rp.ProductID
                WHERE ABS(rp.Sum-round(sp.Sum/sp.Quantity*rp.Quantity,2))>0.011 LIMIT 1");
            if(pricing.Rows.Count>0) throw new InvalidOperationException("У продажи уже есть возврат. Изменение цены нарушает сумму возврата.");
        }
        public static string Friendly(Exception ex)
        {
            if(ex is SQLiteException) {
                if(ex.Message.Contains("FOREIGN KEY")) return "Запись используется в связанных документах. Сначала удалите зависимые записи.";
                if(ex.Message.Contains("UNIQUE")) return "Такая запись уже существует. Проверьте название или повторяющиеся товары.";
                if(ex.Message.Contains("CHECK") || ex.Message.Contains("NOT NULL")) return "Проверьте обязательные поля, даты, положительное количество и неотрицательные цены.";
            }
            return ex.Message;
        }
    }
}
