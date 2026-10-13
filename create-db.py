import sqlite3

# 1. Подключение к базе данных (файл создастся автоматически)
db_name = 'shop.db'
conn = sqlite3.connect(db_name)

try:
    # 2. Создание объекта курсора
    cursor = conn.cursor()

    # 3. Написание SQL-кода
    # Конструкция IF NOT EXISTS защищает от ошибки, если таблица уже существует
    sql_create_table = '''
    PRAGMA foreign_keys = ON;
CREATE TABLE Supplier (
SupplierID INTEGER PRIMARY KEY,
Name TEXT NOT NULL,
Email TEXT
);
CREATE TABLE Employee (
EmployeeID INTEGER PRIMARY KEY,
Surname TEXT NOT NULL,
Name TEXT NOT NULL,
Patronymic TEXT,
INN TEXT,
Phone TEXT,
Position TEXT NOT NULL,
HireDate TEXT NOT NULL,
DismissalDate TEXT
);
CREATE TABLE Product (
ProductID INTEGER PRIMARY KEY,
Name TEXT NOT NULL,
Category TEXT NOT NULL,
Price REAL NOT NULL CHECK (Price >= 0)
);
CREATE TABLE Supply (
SupplyID INTEGER PRIMARY KEY,
SupplierID INTEGER NOT NULL,
SupplyDate TEXT NOT NULL,
FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID)
ON UPDATE RESTRICT ON DELETE RESTRICT
);
CREATE TABLE SupplyProduct (
RecordID INTEGER PRIMARY KEY,
SupplyID INTEGER NOT NULL,
ProductID INTEGER NOT NULL,
Quantity REAL NOT NULL CHECK (Quantity > 0),
ExpirationDate TEXT,
SupplierPrice REAL NOT NULL CHECK (SupplierPrice >= 0),
FOREIGN KEY (SupplyID) REFERENCES Supply(SupplyID)
ON UPDATE RESTRICT ON DELETE RESTRICT,
FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
ON UPDATE RESTRICT ON DELETE RESTRICT
);
CREATE TABLE Sale (
SaleID INTEGER PRIMARY KEY,
EmployeeID INTEGER NOT NULL,
SaleDate TEXT NOT NULL,
PaymentMethod TEXT NOT NULL,
MaskedCard TEXT,
FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
ON UPDATE RESTRICT ON DELETE RESTRICT
);
CREATE TABLE SaleProduct (
RecordID INTEGER PRIMARY KEY,
SaleID INTEGER NOT NULL,
ProductID INTEGER NOT NULL,
Quantity REAL NOT NULL CHECK (Quantity > 0),
FOREIGN KEY (SaleID) REFERENCES Sale(SaleID)
ON UPDATE RESTRICT ON DELETE RESTRICT,
FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
ON UPDATE RESTRICT ON DELETE RESTRICT
);
CREATE TABLE "Return" (
ReturnID INTEGER PRIMARY KEY,
SaleID INTEGER NOT NULL,
EmployeeID INTEGER NOT NULL,
FOREIGN KEY (SaleID) REFERENCES "Sale"(SaleID)
ON UPDATE RESTRICT ON DELETE RESTRICT,
FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
ON UPDATE RESTRICT ON DELETE RESTRICT
);
CREATE TABLE ReturnProduct (
RecordID INTEGER PRIMARY KEY,
ReturnID INTEGER NOT NULL,
ProductID INTEGER NOT NULL,
Quantity REAL NOT NULL CHECK (Quantity > 0),
FOREIGN KEY (ReturnID) REFERENCES "Return"(ReturnID)
ON UPDATE RESTRICT ON DELETE RESTRICT,
FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
ON UPDATE RESTRICT ON DELETE RESTRICT
);
CREATE TABLE WriteOff (
WriteOffID INTEGER PRIMARY KEY,
EmployeeID INTEGER NOT NULL,
Reason TEXT NOT NULL,
WriteOffDate TEXT NOT NULL,
FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
ON UPDATE RESTRICT ON DELETE RESTRICT
);
CREATE TABLE WriteOffProduct (
RecordID INTEGER PRIMARY KEY,
WriteOffID INTEGER NOT NULL,
ProductID INTEGER NOT NULL,
Quantity REAL NOT NULL CHECK (Quantity > 0),
FOREIGN KEY (WriteOffID) REFERENCES WriteOff(WriteOffID)
ON UPDATE RESTRICT ON DELETE RESTRICT,
FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
ON UPDATE RESTRICT ON DELETE RESTRICT
);
    '''

    # 4. Выполнение SQL-запроса
    for sql_part in sql_create_table.split(";"):
        cursor.execute(sql_part)

    # 5. Сохранение (подтверждение) изменений
    conn.commit()
    print(f"База данных '{db_name}' успешно создана!")

except sqlite3.Error as e:
    print(f"Произошла ошибка при работе с SQLite: {e}")

finally:
    # 6. Закрытие соединения (выполнится в любом случае)
    if conn:
        conn.close()