   M S B A M L   `   `   `  ����K  GTS.FW.Wpf, Version=1.0.9053.17879, Culture=neutral, PublicKeyToken=nullJ3clr-namespace:TS.FW.Wpf.Controls;assembly=TS.FW.WpfTS.FW.Wpf.Controls  5      P LGIGA.ITRI.SA6200.UI, Version=24.12.2.1, Culture=neutral, PublicKeyToken=nullK*clr-namespace:GIGA.ITRI.SA6200.UI.ControlsGIGA.ITRI.SA6200.UI.Controls 5      g8clr-namespace:GIGA.ITRI.SA6200.UI.ViewModels.Page.Recipe*GIGA.ITRI.SA6200.UI.ViewModels.Page.Recipe 5      ]3clr-namespace:GIGA.ITRI.SA6200.UI.Views.Page.Recipe%GIGA.ITRI.SA6200.UI.Views.Page.Recipe 5	      7   1GIGA.ITRI.SA6200.UI.Views.Page.Recipe.RcpMainView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Bts3clr-namespace:TS.FW.Wpf.Controls;assembly=TS.FW.Wpf     5      :cnt*clr-namespace:GIGA.ITRI.SA6200.UI.Controls     5      Gvm8clr-namespace:GIGA.ITRI.SA6200.UI.ViewModels.Page.Recipe     5      Elocal3clr-namespace:GIGA.ITRI.SA6200.UI.Views.Page.Recipe     5	         xPageSub#��[  5      .��5      �   � RowDefinitions  5   
   �� 5      $��40 �6   �� 5       � ColumnDefinitions 5   
   �� 5      $	��350 �6   �� 5      .+�5   
       TS.FW.Wpf.Controls.Label3D  $��0��6     xLabCaption#��[ 6%   $��Recipe List��6J   � 5   
   $��1��6   {�6   �� *UC.RcpSelected+  xIsNullValue �� 	Converter# [  5      �� 5      $	��150 �6#   �� 5      $��* �6#   .+�5        $��0��6   #��[ 6)   $
��Name��6N   !   TS.FW.Wpf.Controls.Button3D  5      $��1��6     	xBtnValue#��[ 6*   $
��NAME��6�   ��6M   �� *UC.RcpSelected.Name+��6u   �� *UC+6�   5       � 5"   
   $��2��6     5#      �� 5$      �� 5%      $	��180 �6    .+�5(      !   TS.FW.Wpf.Controls.Border3D  $��2q�6      	IsPressed .   
xTitleBack#��[ 66   .��5)      �� $
��Auto=�6   $��2q�6B   .��5*      �� ��6#   �� *
UC.RcpList+��5+      �� 5,      .R�5-   "   	� $��60��6.    	� 	GroupName$ MAIN_RCP��6N   ��6:   ��  d� 	IsChecked 6c   �� *	IsSelcted+��5.   .   ��  �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType7  1GIGA.ITRI.SA6200.UI.Views.Page.Recipe.RcpMainView"  	 �� Path$%	 DataContext.UC.OnRcpSelectedCmd���5/   .   �� ��50   &   �� 51   *   
 �� 
TargetType"
 	�6:   .R�52   .   �� #��[ 65   $��1q�6�    �� CornerRadius$ 5��6�   $��2q�6�   ��6^   �� *	IsChecked+  xOnOffColor �� 	Converter# [  �� ConverterParameter$
 MENU��._�53   2   �   54   6   �� 55   :   �� 56   :    58   6   �� 59   :   �� 5:   :   .+�5=   6   �� $��2��6`   $
��Left=�5>   G   $��Center=�5?   G   $��5 0 0 0q�5@   G   $��16(�5A   G   $
��Bold"�5C   G   ��5=   G   �� *Name+��5B   G   �� *	IsChecked+# [ $
 MENU��5C   G   �� 5E   6   $��1��6h   $��0��6u    �� ContentStringFormat$ Creation:{0}��6�   $
��Left=�5F   G   $��Center=�5G   G   $��5 0 0 0q�5H   G   $��12(�5I   G   $
��Bold"�5K   G   ��5E   G   �� *CreationTime+��5J   G   �� *	IsChecked+# [ $
 MENU��5K   G   �� 5M   6   $��1��6i   $��1��6v   $ Modification:{0}��6�   $
��Left=�5N   G   $��Center=�5O   G   $��5 0 0 0q�5P   G   $��12(�5Q   G   $
��Bold"�5S   G   ��5M   G   �� *LastWriteTime+��5R   G   �� *	IsChecked+# [ $
 MENU��5S   G   5T   3   5U   /   5V   +   5X   #   5Y      5[      5\      5]      � 5_      $��1��6    5`      �� 5a      �� 5b      �� 5c        5e      �� 5f      �� 5g      .+�5j        $��0��6   $��0��6+     xBtnIcon#��[ 6;      Icon$ Refresh=�6]   $��RELOAD��6l   $��RELOAD��6�   ��6}   �� *UC+6�     5k      $��0��6   $��1��6+   #��[ 6;   $	 New=�6]   $	��NEW��6h   $	��NEW��6�   ��6v   �� *UC+6�     5l      $��0��6   $��2��6+   #��[ 6;   $ Delete=�6]   $��DELETE��6k   $��DELTE��6�   ��6|   �� *UC+6�     5n      $��1��6   $��0��6+   #��[ 6;   $
 Save=�6]   $
��SAVE��6i   $
��SAVE��6�   ��6x   �� *UC+6�     5o      $��1��6   $��1��6+   #��[ 6;   $
 Copy=�6]   $��SAVE AS��6i   $��SAVE AS��6�   ��6{   �� *UC+6�   5p      5q        5s   
   $��2��6   $��1��6#    . #��[ 6D      CornerRadius$ 4��6m    � DataContext 6~   �� *UC.RcpSelected+.��5t      1  +GIGA.ITRI.SA6200.UI.Controls.RcpMainControl  5u      5v      5w      