   M S B A M L   `   `   `  ����K  GTS.FW.Wpf, Version=1.0.9053.17879, Culture=neutral, PublicKeyToken=nullJ3clr-namespace:TS.FW.Wpf.Controls;assembly=TS.FW.WpfTS.FW.Wpf.Controls  5   	   P LGIGA.ITRI.SA6200.UI, Version=24.12.2.1, Culture=neutral, PublicKeyToken=null[2clr-namespace:GIGA.ITRI.SA6200.UI.ViewModels.Popup$GIGA.ITRI.SA6200.UI.ViewModels.Popup 5   	   Q-clr-namespace:GIGA.ITRI.SA6200.UI.Views.PopupGIGA.ITRI.SA6200.UI.Views.Popup 5   	   3   -GIGA.ITRI.SA6200.UI.Views.Popup.RcpSeletcView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Bts3clr-namespace:TS.FW.Wpf.Controls;assembly=TS.FW.Wpf     5   	   Avm2clr-namespace:GIGA.ITRI.SA6200.UI.ViewModels.Popup     5   	   ?local-clr-namespace:GIGA.ITRI.SA6200.UI.Views.Popup     5   	      xPopupWindow#��[  5
   	   $	��650��6/   $	��500��6<   
�������$��15(�6Z   .��5      �   � RowDefinitions  5   
   �� 5      $��50 �6   �� 5      �� 5      $��50 �6   .+�5   
       TS.FW.Wpf.Controls.Label3D  -   5        $��0��5        xLabCaption#��[ 6"    �� ContentStringFormat$ {0} Recipe List��6a     
xTitleBack#��[ 6�   ��6G   �� *Title+6�   !   TS.FW.Wpf.Controls.Border3D  5   
   $��1��6   $��2q�6#      	IsPressed . #��[ 6?   .��5      �� $
��Auto=�6   $��2q�6>   .��5      �� ��6   �� *	RcpList+��5      �� 5      .R�5      	� $��70��6*    	� 	GroupName$ MOLD_RCP��6J   ��66   ��  d� 	IsChecked 6_   �� *	IsSelcted+��5   *   ��  �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType3  -GIGA.ITRI.SA6200.UI.Views.Popup.RcpSeletcView"   �� Path$" DataContext.OnRcpSelectedCmd���5   *   �� ��5   "   �� 5   &   	 �� 
TargetType"	 	�66   .R�5   *   �� #��[ 61   $��1q�6�   
 �� CornerRadius$
 5��6�   $��2q�6�   ��6Z   �� *	IsChecked+  xOnOffColor �� 	Converter# [  �� ConverterParameter$
 MENU��._�5   .   �   5    2   �� 5!   6   �� 5"   6    � ColumnDefinitions 5$   2   �� 5%   6   �� 5&   6   .+�5)   2   �� $��2��6\   $
��Left=�5*   C   $��Center=�5+   C   $��5 0 0 0q�5,   C   $��15(�5-   C   $
��Bold"�5/   C   ��5)   C   �� *Name+��5.   C   �� *	IsChecked+# [ $
 MENU��5/   C   �� 51   2   $��1��6d   $��0��6q    �� ContentStringFormat$ 
Create:{0}��6�   $
��Left=�52   C   $��Center=�53   C   $��5 0 0 0q�54   C   $��15(�55   C   $
��Bold"�57   C   ��51   C   �� *CreationTime+��56   C   �� *	IsChecked+# [ $
 MENU��57   C   �� 59   2   $��1��6e   $��1��6r   $ 
Modify:{0}��6�   $
��Left=�5:   C   $��Center=�5;   C   $��5 0 0 0q�5<   C   $��15(�5=   C   $
��Bold"�5?   C   ��59   C   �� *LastWriteTime+��5>   C   �� *	IsChecked+# [ $
 MENU��5?   C   5@   /   5A   +   5B   '   5D      5E      5G      5H      5I      �� 5K   
   $��2��6   $��Right=�6"    �� Orientation$ 
Horizontal=�6>   .+�5L      !   TS.FW.Wpf.Controls.Button3D    	xBtnEvent#��[ 6   $��OK��6=   $	��150��6J   
��� � �6V     5M      #��[ 6   $��CANCEL��6=   $	��150��6N   
���  ��6Z   5N      5O      5P      