   M S B A M L   `   `   `  ����   AudioPlayer"    SimpleAudioPlayer.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Simple Audio Player��$	��400��$	��400�� � 
ResizeMode$ NoResize=�
���   �.��� $��0,0,0,0q� � ColumnDefinitions �� $
��124* ��� $
��102* ��� $	��71* ��� $
��103* � � RowDefinitions �� $
��125* ��� $	��91* ��� $
��151* ��� $	��17* �.+��� 
���   �$��	42,0,47,7q�.+��� -    ��Name$ 	soundLine��
��� � �$��5��	 �� X1$ 0��	 �� X2$ 0��	 �� Y1$ 0��	 �� Y2$ 0��r� -   	 r�Name$	 videoPlayer��
 r� Source$
 Videos/music-wave.mp4?� r� LoadedBehavior$ Manual=� r� Stretch$ UniformToFill=�$��4��$��4���� $��Audio Player��$
��Left=�$��31��$��	19,22,0,0q�$	��Top=�$	��114��$��18(����#15c6c6$��1��$��2���� -    ��Name$ btnPause��$��63,23,14,33q�
�������
������ $��1��.����  �� Orientation$ 
Horizontal=�.+��� $��Images/pause.png��$��27��$��23���� -   $ btnPlay��$��28,23,29,33q�
�������
������ $��1��$��1��.���� $ 
Horizontal=�.+��� $��Images/play.png��$��27��$��23���� -   $ btnStop��$��	8,23,8,33q�
�������
������ $��1��$��2��.���� $ 
Horizontal=�.+��� $��Images/stop.png��$��27��$��23���� -   $#��Добавить 
песни��$��	2,43,9,35q�
������ 
�������$��2��$��3���� -    ��Name$ sliderVolume�� � Minimum$ 0Q�$y�	Collapsed=� � Maximum$ 1Q� � Value$	 0.5Q�$��0,70,7,0q� J� RenderTransformOrigin$ 
0.521,1.03&�$��3��$��1�� � Triggers :�  :� RoutedEvent$ Slider.Loaded��.[��� .���� .��W�  �� 
TargetName$ sliderVolume�� �� TargetProperty$ Height� W� From$ 0X�	 W� To$ 50X� g� Duration$ 0:0:0.5D��� -   $ sliderPosition��$ 0Q�$	 100Q�$ 0Q�$��	65,70,6,1q�$��1��$��3��z�V� .~��� �� ��  �� Angle$ 359.902Q�T� �� -	    ��Name$ playlistView��$��72,7,4,3q����#231e28
�������$��2��$��3�� �� View �� .P��� $��
Песня�� �� Width$	 200�� �� CellTemplate �� .R��� ���� �� -
   $��3��$��Громкость��$
��Left=�$��28,9,0,0q�$��2��$	��Top=�