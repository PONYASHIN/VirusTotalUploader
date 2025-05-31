<div><p align="center"><img src="https://raw.githubusercontent.com/PONYASHIN/VirusTotalUploader/refs/heads/main/src/icon.ico" width="75" height="75" /></p><h3 align="center">VirusTotal Uploader</h3></div>
<p align="center">VirusTotal uploader</p>
Написана программа изначально для другого моего проекта - XPonTweak.

За костыли не бейте, ведь в том и был смысл - написать программу без единой зависимости от внешних модулей.

-------------

Загружать файлы можно либо через командную строку 

VTuploader.exe "путь к файлу"

Либо добавить пункт в контекстное меню exe файлов предварительно скопировав VTuploader.exe в папку windows

Windows Registry Editor Version 5.00

[HKEY_CLASSES_ROOT\exefile\shell\VTUpload]

@="Upload to VirusTotal"

"Icon"="\"C:\\Windows\\VTuploader.exe\""

[HKEY_CLASSES_ROOT\exefile\shell\VTUpload\command]

@="C:\\Windows\\VTuploader.exe \"%1\""
