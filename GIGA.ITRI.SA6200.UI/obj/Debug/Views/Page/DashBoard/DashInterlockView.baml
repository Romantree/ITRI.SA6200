   M S B A M L   `   `   `  ����K  GTS.FW.Wpf, Version=1.0.9053.17879, Culture=neutral, PublicKeyToken=nullJ3clr-namespace:TS.FW.Wpf.Controls;assembly=TS.FW.WpfTS.FW.Wpf.Controls  5      P LGIGA.ITRI.SA6200.UI, Version=24.12.2.1, Culture=neutral, PublicKeyToken=nullm;clr-namespace:GIGA.ITRI.SA6200.UI.ViewModels.Page.DashBoard-GIGA.ITRI.SA6200.UI.ViewModels.Page.DashBoard 5      c6clr-namespace:GIGA.ITRI.SA6200.UI.Views.Page.DashBoard(GIGA.ITRI.SA6200.UI.Views.Page.DashBoard 5      @   :GIGA.ITRI.SA6200.UI.Views.Page.DashBoard.DashInterlockView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Bts3clr-namespace:TS.FW.Wpf.Controls;assembly=TS.FW.Wpf     5      Jvm;clr-namespace:GIGA.ITRI.SA6200.UI.ViewModels.Page.DashBoard     5      Hlocal6clr-namespace:GIGA.ITRI.SA6200.UI.Views.Page.DashBoard     5         xPageSub#��[  5
      .��5      �   � ColumnDefinitions  5   
   �� 5      �� 5      $	��450 �6   .+�5   
   �  � 	Resources 5       	 xItem    TS.FW.Wpf.Controls.Label3D  xLabCaption  	xLabState  xString �� 	Converter �� ConverterParameter��%  &	       7  7  7  �� 5      .R�5      � $��50��6     5      �� 5      $��3* �6/   �� 5      .+�5        $��0��6%   8��  ��6Z   �� *Name+  5      $��1��6%   8�� ��6X   �� *On+8  $ On/Off��5      5      5        5      �� 5       �� 5!       � RowDefinitions 5#      �� 5$      $��35 �6    �� 5%      .+�5(        $��2��6   #��[ 6-   $��Interlock State��6R   !   TS.FW.Wpf.Controls.Border3D  5*      $��1��6   $��2��6'     
xTitleBack#��[ 6;      CornerRadius$ 5��6d   �� 5,      $��1��6   $��2q�6&   .+�5-      �� #��[ 6!   ��6J   �� *!Interlock.X_MAIN_CDA_RESSURE_01+�� 5.      #��[ 6!   $��0 0 0 5q�6~   ��6J   �� *!Interlock.X_MAIN_CDA_RESSURE_02+6~   �� 50      #��[ 6!   ��6J   �� *Interlock.X_EMO_01+�� 51      #��[ 6!   ��6J   �� *Interlock.X_EMO_02+�� 52      #��[ 6!   ��6J   �� *Interlock.X_EMO_03+�� 53      #��[ 6!   $��0 0 0 5q�6q   ��6J   �� *Interlock.X_EMO_04+6q   �� 55      #��[ 6!   ��6J   �� *Interlock.X_ION_BLOWER_ALARM+�� 56      #��[ 6!   ��6J   �� *Interlock.X_UV_LAMP_ERROR+57      �� 59      $��1��6   $��1��6&   $��2q�66   .+�5:      �� #��[ 6!   ��6J   �� *Interlock.X_DOOR_LOCK_01+�� 5;      #��[ 6!   ��6J   �� *Interlock.X_DOOR_LOCK_02+�� 5<      #��[ 6!   ��6J   �� *Interlock.X_DOOR_LOCK_03+�� 5=      #��[ 6!   ��6J   �� *Interlock.X_DOOR_LOCK_04+�� 5>      #��[ 6!   ��6J   �� *Interlock.X_DOOR_LOCK_05+�� 5?      #��[ 6!   ��6J   �� *Interlock.X_DOOR_LOCK_06+�� 5@      #��[ 6!   ��6J   �� *Interlock.X_DOOR_LOCK_07+5A      5B      �� 5D   
   $��1��6    5E      !   TS.FW.Wpf.Controls.Button3D  	xBtnEvent��%�  &	       7  7  7  7  7  �� 5F      .R�5G      � $��50��6     5H      �� 5I      $��3* �6/   �� 5J      �� 5K      �� 5L      .+�5O        $��0��6%   8��  ��6Z   �� *Name+  5P      $��1��6%   8�� ��6X   �� *On+8  $ On/Off��  5R      $��2��6&   8�� $��On��6Y   ��6g   �� *OnSignalOnCmd+  5S      $��3��6&   8�� $	��Off��6Y   ��6h   �� *OnSignalOffCmd+5T      5U      5V      .+�5X      �� #��[ 6   $��0 0 0 5q�6j   ��6F   �� *Interlock.MC_ON+6j   �� 5Z      #��[ 6   ��6F   �� *Interlock.EQ_LAMP_ON+�� 5]      #��[ 6   ��6F   �� *Interlock.ION_BLOWER_RUN+�� 5^      #��[ 6   $��0 0 0 5q�6{   ��6F   �� *" Interlock.ION_BLOWER_TIP_CLENING+6{   �� 5`      #��[ 6   ��6F   �� *Interlock.LOADCELL_ZERO+� 5b      $��50��6     5c      �� 5d      �� 5e      .+�5h        $��0��6   #��[ 6.   $��ALL ON��6Q     5i      $��1��6   #��[ 6.   $��ALL OFF��6Q   5j      �� 5k      #��[ 6   ��6F   �� *Interlock.DOOR_LOCK_01+�� 5l      #��[ 6   ��6F   �� *Interlock.DOOR_LOCK_02+�� 5m      #��[ 6   ��6F   �� *Interlock.DOOR_LOCK_03+�� 5n      #��[ 6   ��6F   �� *Interlock.DOOR_LOCK_04+�� 5o      #��[ 6   ��6F   �� *Interlock.DOOR_LOCK_05+�� 5p      #��[ 6   ��6F   �� *Interlock.DOOR_LOCK_06+�� 5q      #��[ 6   ��6F   �� *Interlock.DOOR_LOCK_07+5r      5s      5t      