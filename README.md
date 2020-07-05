# NetworkMplsEmulator
Jest to emulator sieci posiadający 4 osobne komponenty takie jak Menadzer,Chmura kablowa, Host oraz Router.

Zadaniem menadzera jest ustawienie tablic kierowania pakietów w routerach po uruchomianiu emulatora.

![](images/mena.png)

Chmura kablowa odpowiada za wszystkie połączenia między routerami oraz między routerami a hostami.

![](images/Cloud.png)

Działanie routera polega na odebraniu pakietu i przekierowaniu go na odpowiedni port wyjściowy.

![](images/router.png)

Host ma za zadanie odebranie lub wysłanie wiadomości do innego hosta.

![](images/host.png)
