   M S B A M L   `   `   `  ���� E  AJKS_Report, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:JKS_Report.PagesJKS_Report.Pages      JKS_Report.Pages.Home   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:JKS_Report.Pages     b ^SharpVectors.Converters.Wpf, Version=1.7.7.0, Culture=neutral, PublicKeyToken=b532964b8548be771svgc&http://sharpvectors.codeplex.com/svgc/  -     N� Title$
  Home�� � 	Resources    ShowMenu �� 
TargetName �� TargetProperty g� 	BeginTime t� From	 t� To  GeneralDuration g� Duration g� DecelerationRatio	 W� From	
 W� To��%�  &	        7  7  7  7  �� .��t� $ MenuContainer��$ (FrameworkElement.Margin)�$ 0:0:0X�$ 	-20,0,0,0X�$ 0,0,0,0X�8   $	 0.9Q�W� $ MenuContainer��$ (UIElement.Opacity)�$ 0:0:0X�$	 0X�$
 1X�8  $	 0.9Q�t� $ MMConfiguration��$ (FrameworkElement.Margin)�$ 0:0:0.1X�$ 	-20,0,0,0X�$ 0,0,0,0X�8  $	 0.9Q�W� $ MMConfiguration��$ (UIElement.Opacity)�$ 0:0:0.1X�$	 0X�$
 1X�8  $	 0.9Q�.��� $��
0,30,30,30q� � RowDefinitions �� $
��Auto ��� $
��Auto �.+�� $��0��.+��� -   $��TBMainTitle��$��0��$��JKS Report Tools��$	��600��  H4#��[ $
��Left=�$��Stretch=��� $	��120��  PrimaryColor#��[ $	��Top=� J� Opacity$	 0.2Q���  K� ZIndex$	 152�� �� Padding$ 20q�$	��130��$	��130��$��Right=�$	��Top=�._�d� -    d�Name$ MainMenu��$��80��  
TBMainMenu#��[ .��]� -    ]�Name$ MenuLogo��$��70��$	��Top=�$
��Left=�.+�(  "SharpVectors.Converters.SvgViewbox     Source$% /Images/Logos/logo_CXSymbol.svg?�� $��1��$��
0,30,30,30q� �� $
��Auto ��� $
��Auto � � ColumnDefinitions �� $
��Auto ��� .+�� $��0��$��0��$��	70,10,0,0q�.+��� -   $��TBReportTitle��$��0��$��0��$��Control��$	��300��  H1#��[ $
��Left=�� $��0��$��1��$��	70,10,0,0q�.+��� $��1��$��0��$��2�� �� Orientation$ 
Horizontal=�$
��Left=�.+��� -   $��btnStop��$
��Stop��  StandardButton#��[ $��0��$��0��$	��Top=�$
��Left=�$��10q�