   M S B A M L   `   `   `  ���� E  AJKS_Report, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:JKS_Report.PagesJKS_Report.Pages      JKS_Report.Pages.Report   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:JKS_Report.Pages     b ^SharpVectors.Converters.Wpf, Version=1.7.7.0, Culture=neutral, PublicKeyToken=b532964b8548be771svgc&http://sharpvectors.codeplex.com/svgc/  -     N� Title$  Report�� � 	Resources    ShowMenu �� 
TargetName �� TargetProperty g� 	BeginTime t� From	 t� To  GeneralDuration g� Duration g� DecelerationRatio	 W� From	
 W� To��%�  &	        7  7  7  7  �� .��t� $ MenuContainer��$ (FrameworkElement.Margin)�$ 0:0:0X�$ 	-20,0,0,0X�$ 0,0,0,0X�8   $	 0.9Q�W� $ MenuContainer��$ (UIElement.Opacity)�$ 0:0:0X�$	 0X�$
 1X�8  $	 0.9Q�t� $ MMConfiguration��$ (FrameworkElement.Margin)�$ 0:0:0.1X�$ 	-20,0,0,0X�$ 0,0,0,0X�8  $	 0.9Q�W� $ MMConfiguration��$ (UIElement.Opacity)�$ 0:0:0.1X�$	 0X�$
 1X�8  $	 0.9Q�.��� $��
0,30,30,30q� � RowDefinitions �� $
��Auto ��� $
��Auto �.+�� $��0��.+��� -   $��TBMainTitle��$��0��$��Report Generation��$	��600��  H4#��[ $
��Left=�$��Stretch=��� $	��120��  PrimaryColor#��[ $	��Top=� J� Opacity$	 0.2Q���  K� ZIndex$	 152�� �� Padding$ 20q�$	��130��$	��130��$��Right=�$	��Top=�._�d� -    d�Name$ MainMenu��$��80��  
TBMainMenu#��[ .��]� -    ]�Name$ MenuLogo��$��70��$	��Top=�$
��Left=�.+�(  "SharpVectors.Converters.SvgViewbox     Source$% /Images/Logos/logo_CXSymbol.svg?�� $��1��$��
0,30,30,30q�.+�� $��0��$��	70,10,0,0q� �� $
��Auto ��� $
��Auto ��� $��* � � ColumnDefinitions �� $��2* ��� $��* �.+��� -   $��Label1��$��Date From :�� �� Left$ 10��
 �� Top$ 10��$��14(�$��Georgia)�$
��Bold"�$��0��$��0��$	��Top=�$
��Left=�$��0,14,0,0q��� -   $��Label2��$��	Date To :��$ 10��$ 10��$��14(�$��Georgia)�$
��Bold"�$��0��$	��Top=�$
��Left=�$��0,65,0,0q��� -   $��Label3��$��Loading Number :��$ 10��$ 10��$��14(�$��Georgia)�$
��Bold"�$��0��$	��Top=�$
��Left=�$��	0,110,0,0q�(  "System.Windows.Controls.DatePicker  -   $��DateFrom��$��30��$	��200��$
��Left=�$��0��$��150,10,0,105q�$	��Top=�  -	   $��DateTo��$��30��$	��200��$
��Left=�$��
150,65,0,0q�$	��Top=��� -
   $��LoadingNumber��$��30��$	��200��$
��Left=�$	��Top=�$��150,110,0,105q��� -   $��Stop Reporting Service��$ 10��$ 10��$��14(�$��Georgia)�$
��Bold"�$��0��$
��Left=�$��
10,180,0,0q�$	��Top=��� $��0��$��2��$��2�� �� Orientation$ 
Horizontal=�.+��� -   $��btnPDFGenerate��$	��PDF��  StandardButton#��[ {�. $��0��$��2��$	��Top=�$
��Left=�$��10q��� -   $��btnCSVGenerate��$	��CSV��#��[ {�. $��0��$��2��$	��Top=�$
��Left=�$��10q�