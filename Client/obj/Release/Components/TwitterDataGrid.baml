   M S B A M L   `   `   `  ���� A  =Client, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null5clr-namespace:Client.ComponentsClient.Components  G(clr-namespace:Client.Components.CommandsClient.Components.Commands  '    !Client.Components.TwitterDataGrid   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     1localclr-namespace:Client.Components     6c(clr-namespace:Client.Components.Commands     |�.   � DataContext  ��  � 	Resources &   System.Windows.Controls.DataGrid �� 
TargetType �� Property �� Value �� Path �� Mode'   !Client.Components.TwitterDataGrid   StaticViewModel �� Source   
dgRowStyle)  #System.Windows.Controls.DataGridRow	 �� Triggers
 P� Property P� Value  dgCellStyle*  $System.Windows.Controls.DataGridCell �� BasedOn��%�  '        &	      &	 �    7 '�� :    �� "  .���  �@ AutoGenerateColumns .  ��  �@ 
IsReadOnly . �� " �@ EnableColumnVirtualization . ��  �@ EnableRowVirtualization . ��   �@ AlternatingRowBackground
 �������  � �� �� $ FontSize�$ OneWay=�# Z ��  �@ SelectionMode$ Single=��� % �@��IsSynchronizedWithCurrentItem$ FalseY��� % �@ HorizontalScrollBarVisibility$ Disabled=���  �@ GridLinesVisibility$
 None=���  �@ HeadersVisibility$	 Row=��� "  .���  � ��
 �������  �@J�SnapsToDevicePixels . 	 P� 
 �@ 
IsSelected . . ���  � ��
 ������� "  8   .���  �@J�SnapsToDevicePixels . 	 P� 
 �@ 
IsSelected . . ���  � ��
 ���� ��  � ��
 ���� ��  � ��
 �   ��� " ��.���  �@��TextDecorations$
 None{�.��  -     Name$ dataGrid��   RowStyle# [     	CellStyle# [ ���� $ OneWay=�   Columns 4  .System.Windows.Controls.DataGridTemplateColumn     CellTemplate �� .R��� ���� $  User.ProfileImageHyperlink�$ OneTime=�  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ *  �� .R��� ���� $ TextContainer�$ OneWay=�