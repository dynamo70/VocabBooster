using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace VocabBooster
{
		public static class Globals
		{
			// This is for registry operations(options)
			const string c_strKeyName = "HKEY_CURRENT_USER\\Software\\VocabularyBooster\\Options\\";

			// Start up path in registry
			const string c_strRunWinStartUp = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\";

			public static string strExePath = "\\VocabBooster";
			public static string strTxtFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MyWordsToLearn.txt";
			public static string strRemove = string.Empty;

			// to get executable file name from MainWindow
			public static string strShortCutName = string.Empty;

			// to get my program fullpath with its name to run it by shortcut
			public static string strTarget = string.Empty;

			// counter of rows in database for swapping in DataGridView1
			public static int iDataRowCountSwap_DGV1 = 0;
			public static int iDataRowCountSwap_DGV2 = 0;
			public static int iDataRowCountSwap_DGV3 = 0;

			// To check count of data greater than 6 for random popup form
			public static bool checkCountOfData;             

			public static string strLearned = string.Empty;
		}

}
