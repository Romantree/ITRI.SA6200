   M S B A M L   `   `   `  ����K  GTS.FW.Wpf, Version=1.0.9053.17879, Culture=neutral, PublicKeyToken=nullJ3clr-namespace:TS.FW.Wpf.Controls;assembly=TS.FW.WpfTS.FW.Wpf.Controls  5   	   F1clr-namespace:TS.FW.Wpf.Helper;assembly=TS.FW.WpfTS.FW.Wpf.Helper  5   	   P LGIGA.ITRI.SA6200.UI, Version=24.12.2.1, Culture=neutral, PublicKeyToken=nullO,clr-namespace:GIGA.ITRI.SA6200.UI.ViewModelsGIGA.ITRI.SA6200.UI.ViewModels 5   	   E'clr-namespace:GIGA.ITRI.SA6200.UI.ViewsGIGA.ITRI.SA6200.UI.Views 5	   	   (   "GIGA.ITRI.SA6200.UI.Views.MainView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Bts3clr-namespace:TS.FW.Wpf.Controls;assembly=TS.FW.Wpf     5   	   ?h1clr-namespace:TS.FW.Wpf.Helper;assembly=TS.FW.Wpf     5   	   ;vm,clr-namespace:GIGA.ITRI.SA6200.UI.ViewModels     5   	   9local'clr-namespace:GIGA.ITRI.SA6200.UI.Views     5	   	      	xMainView#��[  5   	   $��12(�6,     � DataContext  5      2  ,GIGA.ITRI.SA6200.UI.ViewModels.MainViewModel  5   
   .��5      (�  (� Stretch$
 Fill=�6   .��5   
   � $
��1280��6   $
��1024��6    � RowDefinitions 5      �� 5      $	��100 �6    �� 5      �� 5      $	��100 �6    .+�5      !   TS.FW.Wpf.Controls.Border3D  $��0��6     	xBdrTitle#��[ 6'     
xTitleBack#��[ 6J   .��5      � $��5q�6    � ColumnDefinitions 5      �� 5      $	��200 �6+   �� 5      $	��550 �6+   �� 5      �� 5      $��90 �6+   �� 5      $��90 �6+   �� 5       $��90 �6+   �� 5!      $��50 �6+   �� 5"      $	��120 �6+   .+�5%        -   5        #��[ 5%   "     xLogBack#��[ 6E   .��5&      � .+�5'      �� $��0��6$   $L��Fpack://application:,,,/GIGA.ITRI.SA6200.UI;component/Assets/로고.png��64   $��Uniform=�6�   $��5q�6�   �� 5(       �� ContentStringFormat$ 	{0:f1} MB��6?   $��10(�6a   $
��Bold"�6o   $��Right=�6�   $	��Top=�6�   $
��10 2q�6�   
���������6$   �� *Memory+6�   5)      5*        5,      $��1��6"   #��[ 62   
���iii�   	IsPressed . .��5-      � $	��5 3q�6    � 	Resources 5.        xNetwork �� 
TargetType    TS.FW.Wpf.Controls.Label3D  	xLabState �� BasedOn	 �� Property
 �� Value �� RelativeSource �� Mode �� Path  xOnOffColor �� 	Converter �� ConverterParameter��%�   &	       7  7  7  �� 5/   "   "  69   8   .�50   &   �� 	 � ��
 6C   ��  �� $
 Self=�$ DataContext�8  $ NET_BACK���� 51   &   	 � ��
 6C   ��  �� $
 Self=�$ DataContext�8  $ NET_FONT��52   #   53       54      �� 55   "   �� 56   "    58      �� 59   "   $��90 �63   �� 5:   "   �� 5;   "   �� 5<   "   .+�5?        $��2��6)      CornerRadius$ 4��6:   $��Network
  Status��6K   
�������$
��Bold"�6�   $��17(�6�     5A      $��0��6)   $��1��66   #��[ 6F   $��Loadcell(Left)��6h     6�   �� *NetworkState.LeftLD+  5B      $��0��6)   $��2��66   #��[ 6F   $��Loadcell(Right)��6h     6�   �� *NetworkState.RightLD+  5C      $��0��6)   $��3��66   #��[ 6F   $��UV Lamp��6h     6z   �� *NetworkState.Uv+  5E      $��1��6)   $��1��66   #��[ 6F   $��Regulatror(Left)��6h     6�   �� *NetworkState.LeftRG+  5F      $��1��6)   $��2��66   #��[ 6F   $��Regulatror(Right)��6h     6�   �� *NetworkState.RightRG+  5G      $��1��6)   $��3��66   #��[ 6F   $�� ��6h   5H      5I      !   TS.FW.Wpf.Controls.Button3D  5K      $��3��6"     xBtnIcon#��[ 62      Icon$ Alarm=�6T   $��ALARM��6a   $��Alarm��6�   ��6q   �� *Alarm+# [ $ ALARM��{�6�   �� *
IsEnable+  5L      $��4��6"   #��[ 62   $ Buzzer=�6T   $��BUZZER��6b   $��Buzzer��6�   ��6s   �� *Buzzer+# [ $
 MENU��6�     5M      $��5��6"   #��[ 62   $ Custom=�6T     appbar_people_multiple   Visual# [ 6b   $��Login��6�   $��12(�6�   ��6�   �� *Login+6�   � 5O      $��6��6   $	��0 2q�6+     68   �� *	TowerLamp+ 5P      �� 5Q      �� 5R      �� 5S      .+�5V        $��0��6&   $��2q�63      CornerRadius$ 4��6>   ��6O   �� *Red+# [ $	 Red��  5W      $��1��6&   $��2q�63   $ 4��6>   ��6O   �� *Yellow+# [ $ Yellow��  5X      $��2��6&   $��2q�63   $ 4��6>   ��6O   �� *Green+# [ $ Green��5Y        5[      $��7��6"   #��[ 62   
���iii� . $	��2 4q�6{   .��5\      �� $��7��6%   $��Center=�65   .+�5]      �� $��0��6(   $��Center=�6�   $��Center=�6�   $
��Bold"�6�   
���������65   �� %   TS.FW.Wpf.Helper.DateTimeHelper
   Ins �� Source# Z $ NowDateString�6�   �� 5^      $��1��6(   $��Center=�6�   $��Center=�6�   $
��Bold"�6�   
���������65   �� # Z $ NowTimeString�6�   �� 5_      $��2��6(   $��Center=�6N   $��Center=�6k   $
��Bold"�6�   
�������$��0 10 0 0q�6�   ��65   �� *	Version+6�   5`      5a      5b      5c        5e      $��1��6   #��[ 6'    	 	xMainBack#��[	 6J   {�6r   �� *
IsEnable+.��5f      (� $
��1276��6   $	��822��6'   $
 Fill=�64   .��5g      !   TS.FW.Wpf.Controls.MenuView     ItemsSource 6"   �� *
MenuList+5h      5i        5k      $��2��6   #��[ 6'   #��[ 6J   .��5l      � $��5q�6    5m      �� 5n      �� 5o      $��90 �6+   .+�5r      !   TS.FW.Wpf.Controls.MenuList  $��0��6"      Orientation$ 
Horizontal=�62    
 xbtnMenu   	MenuStyle# [
 6�      ItemsSource 6K   �� *
MenuList+   SelectedMenu 6l   �� *SelectedMenu+{�6�   �� *
IsEnable+  5t      $��1��6"   #��[ 62   $ Custom=�6T     appbar_power# [ 6b   $��E X I T��6�   $
��EXIT��6�      DelayMsc$
 1000��6�   5u      5v      5w      5x      5y      