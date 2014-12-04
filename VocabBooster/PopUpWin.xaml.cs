using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VocabBooster
{
	/// <summary>
	/// Interaction logic for PopUpWin.xaml
	/// </summary>
	public partial class PopUpWin : Window
	{
		// obtaining word's string from MainWindow
		internal string theWord = string.Empty;

		// // obtaining word's meaning string from MainWindow
		internal string theMeaning = string.Empty;

		MainWindow originalWin;
		public PopUpWin(MainWindow incomingWin)
		{
			originalWin = incomingWin;
			InitializeComponent();
		}
	}
}
