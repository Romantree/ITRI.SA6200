   M S B A M L   `   `   `  ����K  GTS.FW.Wpf, Version=1.0.9053.17879, Culture=neutral, PublicKeyToken=nullJ3clr-namespace:TS.FW.Wpf.Controls;assembly=TS.FW.WpfTS.FW.Wpf.Controls  5      P LGIGA.ITRI.SA6200.UI, Version=24.12.2.1, Culture=neutral, PublicKeyToken=nullg8clr-namespace:GIGA.ITRI.SA6200.UI.ViewModels.Page.Utilty*GIGA.ITRI.SA6200.UI.ViewModels.Page.Utilty 5      ]3clr-namespace:GIGA.ITRI.SA6200.UI.Views.Page.Utilty%GIGA.ITRI.SA6200.UI.Views.Page.Utilty 5      9   3GIGA.ITRI.SA6200.UI.Views.Page.Utilty.UtLogFileView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Bts3clr-namespace:TS.FW.Wpf.Controls;assembly=TS.FW.Wpf     5      Gvm8clr-namespace:GIGA.ITRI.SA6200.UI.ViewModels.Page.Utilty     5      Elocal3clr-namespace:GIGA.ITRI.SA6200.UI.Views.Page.Utilty     5         xPageSub#��[  5
      .��5      �   � RowDefinitions  5   
   �� 5      $��40 �6   �� 5       � ColumnDefinitions 5   
   �� 5      $	��300 �6   �� 5      .+�5   
       TS.FW.Wpf.Controls.Label3D  $��0��6   $��0��6"     xLabCaption#��[ 62   $��LOG LIST��6W   � 5   
   $��1��6    5      �� 5      $	��250 �6#   �� 5      $��30 �6#   �� 5      $	��250 �6#   �� 5      $��50 �6#   �� 5      .+�5       �� $��1��6   $��~��6(   $��25(�61   $
��Bold"�6?   $��Center=�6Q   $��Center=�6n   %   TS.FW.Wpf.Controls.DateButton3D  5"      $��0��6   $��2q�6.      CornerRadius$ 5��69      IconSize$ 20Q�6J   $
��Bold"�6X      Content 6j   �� *	StartTime+  5#      $��2��6   $��2q�6.   $ 5��69   $ 20Q�6J   $
��Bold"�6X    6j   �� *	EndTime+!   TS.FW.Wpf.Controls.Button3D  5$      $��4��6     	xBtnEvent#��[ 6*   $��SEARCH��6M   $��SEARCH��6^   5%      !   TS.FW.Wpf.Controls.Border3D  5'   
   $��1��6   $��0��6#     
xTitleBack#��[ 63   $��2q�6\      	IsPressed . .��5(      �� $
��Auto=�6   $��2q�6>   .��5)      �� ��6   �� *	LogList+��5*      �� 5+      .R�5,      	� $��50��6*    	� 	GroupName$	 LOG��66   ��5-   *   ��  �� RelativeSource ��  �� Mode$ FindAncestor=�	 �� AncestorType9  3GIGA.ITRI.SA6200.UI.Views.Page.Utilty.UtLogFileView"	  
 �� Path$"
 DataContext.OnLogSelectedCmd���5.   *   �� ��5/   "   �� 50   &    �� 
TargetType" 	�66   .R�51   *   �� #��[ 61   
���iii�$��1q�6p    �� CornerRadius$ 5��6�   $��2q�6�   ._�52   .   � $��12(�63   $��2q�6K   $
��Bold"�54   3   ��53   3   �� *	IsChecked+  xOnOffColor �� 	Converter# [  �� ConverterParameter$
 MENU��  55   2   �� 56   6   �� 57   6   .+�5:   2   �� $��0��6<   ��6I   �� *Name+�� 5;   2   $��1��6<   ��6I   �� *Time+5<   /   5=   +   5>   '   5@      5A      5C      5D      5E        5G   
   $��1��6   $��1��6#   #��[ 63   $��2q�6\    . .��5H      �� 
������ 
�������$��0q�6B   $	��2 5q�6V    �� TextWrapping$
 Wrap=�6c    �� 
IsReadOnly . $
��Auto=�5I       � Cursor$	 Pen��6F   ��5H   �   �� *LogMsg+5I   F   5J      5K      5L      