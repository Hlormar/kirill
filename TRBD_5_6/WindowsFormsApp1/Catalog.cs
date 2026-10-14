using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApp1
{
    // Metadata is shared by lists, editors and parameterized database commands.
    internal sealed class Field
    {
        public string Name, Title, Lookup;
        public bool Optional, Date, Number;
        public Field(string name, string title, string lookup = null, bool optional = false, bool date = false, bool number = false)
        { Name = name; Title = title; Lookup = lookup; Optional = optional; Date = date; Number = number; }
    }
    internal sealed class Entity
    {
        public string Table, Key, Title, Search, Detail;
        public Field[] Fields;
        public Entity(string table, string key, string title, string search, string detail, params Field[] fields)
        { Table = table; Key = key; Title = title; Search = search; Detail = detail; Fields = fields; }
    }
    internal static class Catalog
    {
        public static readonly Entity[] All = {
            new Entity("ProductCategory","CategoryID","Категории товаров","Name",null,new Field("Name","Название")),
            new Entity("Position","PositionID","Должности","Name",null,new Field("Name","Название"),new Field("Salary","Оклад",number:true)),
            new Entity("Supplier","SupplierID","Поставщики","Name",null,new Field("Name","Название"),new Field("Email","Электронная почта",optional:true)),
            new Entity("Product","ProductID","Товары","Name",null,new Field("CategoryID","Категория","ProductCategory"),new Field("Name","Название"),new Field("Price","Цена",number:true)),
            new Entity("Employee","EmployeeID","Сотрудники","Surname",null,new Field("PositionID","Должность","Position"),new Field("Surname","Фамилия"),new Field("Name","Имя"),new Field("Patronymic","Отчество",optional:true),new Field("INN","ИНН",optional:true),new Field("Phone","Телефон",optional:true),new Field("HireDate","Дата приёма",date:true),new Field("DismissalDate","Дата увольнения",optional:true,date:true)),
            new Entity("Supply","SupplyID","Поставки","SupplyDate","SupplyProduct",new Field("SupplierID","Поставщик","Supplier"),new Field("SupplyDate","Дата поставки",date:true)),
            new Entity("Sale","SaleID","Продажи","SaleDate","SaleProduct",new Field("EmployeeID","Сотрудник","Employee"),new Field("SaleDate","Дата продажи",date:true),new Field("PaymentMethod","Оплата"),new Field("MaskedCard","Маска карты",optional:true)),
            new Entity("Return","ReturnID","Возвраты","SaleID","ReturnProduct",new Field("SaleID","Исходная продажа","Sale"),new Field("EmployeeID","Сотрудник","Employee")),
            new Entity("WriteOff","WriteOffID","Списания","Reason","WriteOffProduct",new Field("EmployeeID","Сотрудник","Employee"),new Field("Reason","Причина"),new Field("WriteOffDate","Дата списания",date:true))
        };
        public static Entity Get(string table) { return All.Single(e => e.Table == table); }
        public static string Q(string name) { return "\"" + name.Replace("\"", "\"\"") + "\""; }
        public static string Caption(string name)
        {
            var f = All.SelectMany(e => e.Fields).FirstOrDefault(x => x.Name == name);
            if (f != null) return f.Title;
            switch (name) {
                case "ProductID": return "Товар"; case "Quantity": return "Количество";
                case "Sum": return "Сумма"; case "SupplierPrice": return "Цена закупки";
                case "ExpirationDate": return "Срок годности"; case "RecordID": return "№ позиции";
                case "UnitPrice": return "Цена продажи"; case "Category": return "Категория";
                default: return name.EndsWith("ID",StringComparison.Ordinal) ? "№" : name;
            }
        }
    }
}
