start ..\\Cloud\\Cloud\\bin\\Release\Tsst.exe "..\\Config\\CloudConfig.xml"
timeout 1
start ..\\Manager\\Manager\\bin\\Debug\\Manager.exe "..\\Config\\managerConfig.xml"
timeout 1
start ..\\Client\\Client\\bin\\Debug\WindowsFormsApp1.exe "..\\Config\\host1.xml"
timeout 1
start ..\\Client\\Client\\bin\\Debug\WindowsFormsApp1.exe "..\\Config\\host2.xml"
timeout 1
start ..\\Client\\Client\\bin\\Debug\WindowsFormsApp1.exe "..\\Config\\host3.xml"
timeout 1
start ..\\Client\\Client\\bin\\Debug\WindowsFormsApp1.exe "..\\Config\\host4.xml"
timeout 1
start ..\\NetworkNode\\NetworkNode\\bin\\Debug\WindowsFormsApp1.exe "..\\Config\\config1.xml"
timeout 1
start ..\\NetworkNode\\NetworkNode\\bin\\Debug\WindowsFormsApp1.exe "..\\Config\\config2.xml"
timeout 1
start ..\\NetworkNode\\NetworkNode\\bin\\Debug\WindowsFormsApp1.exe "..\\Config\\config3.xml"
timeout 1
start ..\\NetworkNode\\NetworkNode\\bin\\Debug\WindowsFormsApp1.exe "..\\Config\\config4.xml"
timeout 1
start ..\\NetworkNode\\NetworkNode\\bin\\Debug\WindowsFormsApp1.exe "..\\Config\\config5.xml"
start .\cmdow.exe R1 /mov 0 0
start .\cmdow.exe R2 /mov 0 351
start .\cmdow.exe R3 /mov 0 702
start .\cmdow.exe R4 /mov 501 0
start .\cmdow.exe R5 /mov 501 351
start .\cmdow.exe H1 /mov 1002 0
start .\cmdow.exe H2 /mov 1002 351
start .\cmdow.exe H3 /mov 1002 702
start .\cmdow.exe H4 /mov 1503 0
start .\cmdow.exe Cloud /mov 1503 702
start .\cmdow.exe Menager /mov 200 200 /act