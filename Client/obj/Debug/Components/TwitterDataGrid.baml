   M S B A M L   `   `   `  ����A  =Client, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null5clr-namespace:Client.ComponentsClient.Components  5      G(clr-namespace:Client.Components.CommandsClient.Components.Commands  5      '    !Client.Components.TwitterDataGrid   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      1localclr-namespace:Client.Components     5      6c(clr-namespace:Client.Components.Commands     5      |�.   � DataContext  5      ��  � 	Resources 5	      &   System.Windows.Controls.DataGrid �� 
TargetType �� Property �� Value �� Path �� Mode'   !Client.Components.TwitterDataGrid   StaticViewModel �� Source   
dgRowStyle)  #System.Windows.Controls.DataGridRow	 �� Triggers
 P� Property P� Value  dgCellStyle*  $System.Windows.Controls.DataGridCell �� BasedOn��%7  '        &	  �    &	 �    7 '�� �    �� 5
      "  6
   .�5      ��  �@ AutoGenerateColumns .  6+   �� 5       �@ 
IsReadOnly . 6"   �� 5      " �@ EnableColumnVirtualization . 62   �� 5       �@ EnableRowVirtualization . 6/   �� 5        �@ AlternatingRowBackground
 �����60   �� 5       � �� 6    �� $ FontSize�$ OneWay=�# Z �� 5       �@ SelectionMode$ Single=�6%   �� 5      % �@��IsSynchronizedWithCurrentItem$ FalseY�65   �� 5      % �@ HorizontalScrollBarVisibility$ Disabled=�65   �� 5       �@ GridLinesVisibility$
 None=�6+   �� 5       �@ HeadersVisibility$	 Row=�6)   5      �� 5      "  6   .�5      ��  � ��
 �����6#   �� 5       �@J�SnapsToDevicePixels . 6+   	 5      P� 5      
 �@ 
IsSelected . . �5      ��  � ��
 �����6$   5      5      �� 5       "  6   8   .�5!      ��  �@J�SnapsToDevicePixels . 6+   	 5"      P� 5#      
 �@ 
IsSelected . . �5$      ��  � ��
 ���� 6$   �� 5%       � ��
 ���� 6%   �� 5&       � ��
 �   �6$   5'      5)      �� 5*      " ��6
   .�5+      ��  �@��TextDecorations$
 None{�6'   5,      5-      .��5/        -   5          Name$ dataGrid��5/         RowStyle# [  51         	CellStyle# [ 52      ��50      �� $ OneWay=�   Columns 53      4  .System.Windows.Controls.DataGridTemplateColumn  55         CellTemplate 56      �� 57      .R�58      �� ��6   �� $  User.ProfileImageHyperlink�$ OneTime=�59      5;        5=      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6    5>      �� 5?      .R�5@      �� ��6   �� $ TextContainer�$ OneWay=�5A      5C      5E      5F      