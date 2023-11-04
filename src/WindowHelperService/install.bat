sc create UMCHelperService binPath="%~dp0UMCHelperService.exe"
sc failure UMCHelperService actions=restart/60000/restart/60000/""/60000 reset= 86400
sc start UMCHelperService
sc config UMCHelperService start=delayed-auto
sc description UMCHelperService "Ung dung ho tro truy xuat thong tin client, in tu dong"
pause