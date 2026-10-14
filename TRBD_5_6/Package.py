"""Package the built application and clean project sources for practical work 5-6."""
from pathlib import Path
import shutil
import sqlite3
from zipfile import ZipFile, ZIP_DEFLATED

root = Path(__file__).resolve().parent
source = root / 'WindowsFormsApp1'
release = source / 'bin' / 'Release'
ready = root / 'Готовая_программа'
archive = root / 'Шляхтин_ДК_Ермолаев_КД_ПЗ_5_6.zip'

assert (release / 'WindowsFormsApp1.exe').is_file(), 'Build Release first'
ready.mkdir(exist_ok=True)
for file in release.rglob('*'):
    if file.is_file() and file.suffix.lower() != '.pdb' and 'Data' not in file.relative_to(release).parts:
        target = ready / file.relative_to(release)
        target.parent.mkdir(parents=True, exist_ok=True)
        shutil.copy2(file, target)

# Preserve any data if this packaging script is run again after the user works.
database = ready / 'Data' / 'store.sqlite'
database.parent.mkdir(exist_ok=True)
if not database.exists():
    with sqlite3.connect(database) as connection:
        connection.executescript((source / 'Database' / 'schema.sql').read_text(encoding='utf-8'))
        connection.executescript((source / 'Database' / 'seed.sql').read_text(encoding='utf-8'))
        assert connection.execute('PRAGMA integrity_check').fetchone()[0] == 'ok'
        assert not connection.execute('PRAGMA foreign_key_check').fetchall()
        assert connection.execute("SELECT count(*) FROM sqlite_master WHERE type='table'").fetchone()[0] == 13

shutil.copy2(root / 'README.md', ready / 'Инструкция.md')
with ZipFile(archive, 'w', ZIP_DEFLATED) as zip_file:
    for file in ready.rglob('*'):
        if file.is_file():
            zip_file.write(file, Path('Программа') / file.relative_to(ready))
    for file in source.rglob('*'):
        if file.is_file() and not any(part in {'bin', 'obj', 'zadanie'} for part in file.relative_to(source).parts):
            zip_file.write(file, Path('Исходники/WindowsFormsApp1') / file.relative_to(source))
    for name in ['WindowsFormsApp1.sln', 'WindowsFormsApp1.slnx', 'README.md', 'Verify.ps1', 'Package.py']:
        zip_file.write(root / name, Path('Исходники') / name)
    zip_file.write(root / 'verification' / 'verification.txt', 'Проверка/verification.txt')
    zip_file.write(root / 'README.md', 'README.md')
with ZipFile(archive) as zip_file:
    assert zip_file.testzip() is None
    required = ['Программа/WindowsFormsApp1.exe', 'Программа/System.Data.SQLite.dll',
                'Программа/x64/SQLite.Interop.dll', 'Программа/x86/SQLite.Interop.dll',
                'Программа/PdfSharp.dll', 'Программа/Data/store.sqlite',
                'Исходники/WindowsFormsApp1/WindowsFormsApp1.csproj']
    assert all(name in zip_file.namelist() for name in required)
    print(f'Archive verified: {archive.name}, {archive.stat().st_size:,} bytes, {len(zip_file.namelist())} files')
print(f'Ready executable: {ready / "WindowsFormsApp1.exe"}')
