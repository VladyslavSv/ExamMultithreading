   M S B A M L   `   `   `  ����E  AWrongWords, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:WrongWords
WrongWords  5   	       WrongWords.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:WrongWords     5   	   -   5        $��mainWnd��5   	     � Title$%  Зпрещенные слова��5	   	   $��339.879��6"   $��364.177��63    � 	Resources 5
        WrongWords.MainViewModel   mainViewModel��%   &	          5   
   66   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��0.1* �6   �� 5      $
��0.1* �6   �� 5      $
��0.1* �6   �� 5      $
��0.6* �6   �� 5      $
��0.1* �6    � ColumnDefinitions 5   
   �� 5      $��0.02* �6   �� 5      $��0.96* �6   �� 5      $��0.02* �6   .+�5   
   � $��4��6   $��3��6    5      �� 5      �� 5      �� 5      .+�5       �� -   5         ��Name$ startButton��5    &   $��0��6   $��
Старт��6;   $��Center=�6K   $	��100��6f   ��5!      ��  �� Source# [   �� Path$ startCommand���6a   ��  �� ElementName$ mainWnd���� 5"      -   5        $ 
stopButton��5"   %   $��1��6   $��Стоп��69   $��Center=�6H   $	��100��6c   �� 5#      -   5        $ pauseButton��5#   %   $��2��6   $��
Пауза��6:   $��Center=�6J   $	��100��6e   5$      �� 5&   
   -   5         ��Name$ folderTextBox��5&   ;   $��1��6   $��1��6   $��25��6/   	 �� TextWrapping$
	 Wrap=�6R   ��6f   �� # [  $ DirectoryPath�
 �� Mode$
 TwoWay=�� 5(   
   -   5         �Name$ parent��5(   ,   $��3��6   $��1��6   .+�5)      �� -   5         ��Name$ 	myListBox��5+      ��5)      �� *Width+$ parent����5*      �� *ActualHeight+$ parent����5,      �� # [  $ Items�$
 TwoWay=�5-      5.      � 51   
   $��0��6   $��1��6    52      �� 53      �� 54      .+�56      �� $��0q�6b   $��1��6v   $#��Загрузить слова��6�   $��Right=�6�   $��Center=�6�   $��0,5,10,0q�6�   ���	#FF0C00FF��6   �� # [  $ labelCommand���57      �� 58       �� Opacity$ 1Q�6&    �� Color$ White��62   5:      �� 5;      $��0��6   $+��%Папка с результатом:��6$   $
��Left=�6C   $��Bottom=�6^   5<      � 5>   
   $��2��6   $��1��6    5?      �� 5@      �� 5A      .+�5C      �� -   5        $ browseButton��5C   %   $��0��6   $��Указать путь��6;   $
��Left=�6R   $	��Top=�6m   ��5D      �� # [  $ browseCommand���6b   �� $ mainWnd���� 5F      -	   5         ��Name$ repWords��5F   (   $��1��6   $
��Left=�6;   ��6W   �� # [  $ counterProp�$
 TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5G      5J      5K      