   M S B A M L   `   `   `  ����B  >Fitness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:FitnessFitness  5   	       Fitness.ChangeMailWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Fitness     5   	     � Title$  ChangeMailWindow��5   	   $��208.8��6"   $��311.6��61    � 
ResizeMode$ CanMinimize=�6?   ��5	      �� 5
   
    �� EndPoint$ 0.5,1&�6    �� 
StartPoint$ 0.5,0&�6/   .��5      �  � Color$ Black��6    � Offset$ 0Q�6)   � 5      $ 	#FFFF1010��6   $ 1Q�6-   5      .��5      � ���	#FF9C0000$��	0,0,-6,-6q�6"   .+�5   
   � -   5        $
��Code��5      ���	#FF9C0000$y�Visible=�62   .+�5      �� -   5        $��TextBox1��5      $
��Left=�6&   $��23��6A   $��	39,39,0,0q�6M    �� TextWrapping$
 Wrap=�6`   $�� ��6t   $	��Top=�6|   $	��229��6�   �� 5      -   5        $��Label1��5      $9��3Введите код для смены почты:��6"   $
��Left=�6I   $��	35,10,0,0q�6d   $	��Top=�6w   
�������6�   �� 5      -   5        $��Change��5   *   $y�Visible=�6   $��Проверить код��68   $
��Left=�6P   $��	39,78,0,0q�6k   $	��Top=�6~   $	��229��6�   6�   5      � 5   
   -   5        $��ChangeMailAndPassword��5      ���	#FF9C0000$y�Hidden=�6C   .+�5      �� -   5        $��TextBox2��5      $
��Left=�6&   $��23��6A   $��	39,39,0,0q�6M   $
 Wrap=�6`   $�� ��6t   $	��Top=�6|   $	��229��6�   �� 5      -   5        $��Label2��5      $+��%Введите новую почту:��6"   $
��Left=�6A   $��	35,10,0,0q�6\   $	��Top=�6o   
�������6�   �� 5      -   5        $��
ChangeMail��5   *   $y�Visible=�6   $!��Изменить почту��6<   $
��Left=�6U   $��
39,145,0,0q�6p   $	��Top=�6�   $	��229��6�   6�   �� 5      $=��7Введите пароль от новой почты:��6   $
��Left=�6=   $��	35,77,0,0q�6X   $	��Top=�6k   
�������6�   G� 5      -	   5        $��PasswordBox1��5      $
��Left=�6.   $��
39,103,0,0q�6I   $	��Top=�6]   $	��229��6u   $��23��6�   5      5      5      