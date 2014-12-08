using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
namespace VocabBooster
{
	public delegate void EventHandler();
	


	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		PopUpWin frm;
		
		public MainWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		/// This method works with an existance of a directory in Program Files
		/// that folder must be created before this method runs
		/// </summary>
		private void CopyExeFile(string copyPath)
		{
			string path = AppDomain.CurrentDomain.BaseDirectory;

			string completelyFullFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

			string fileName = Path.GetFileName(completelyFullFilePath);
			string copyFile = copyPath + "\\" + fileName;
			if (!System.IO.File.Exists(copyFile))
			{
				try
				{
					System.IO.File.Copy(completelyFullFilePath, copyFile);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		/// <summary>
		/// to creat a short cut on desktop with its icon
		/// Windows Script Host Object Model necessarily as mandatory for Interop.IWshRuntimeLibrary
		/// We can use WshShell, IWshShortcut
		/// </summary>
		private void CreateShortCut()
		{
			// The description of the shortcut
			string strDescription = "Vocabulary Booster is to help you that improving your vocabulary";
			// to get full path of desktop
			string strDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			string strShortCut = strDesktopPath + "\\" + Globals.strShortCutName + ".lnk";

			WshShell shell;
			try
			{
				shell = new WshShell();
				// if the shortcut is exist then pass to create it once.
				if (!System.IO.File.Exists(strShortCut)) 
				{
					IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(strDesktopPath + "\\" + Globals.strShortCutName);
					// The path of the file that will launch when the shortcut is run.
					shortcut.TargetPath = Globals.strTarget;
					shortcut.WindowStyle = 1;
					shortcut.IconLocation = Globals.strTarget + ", 0";
					shortcut.Description = strDescription;
					shortcut.Save();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		
		private void Window_Loaded_1(object sender, RoutedEventArgs e)
		{
			// Specifying a Folder in Program Files.
			string strProgFiles = Environment.GetEnvironmentVariable("PROGRAMFILES");
			// passing my project executable file name.
			string fullPath = strProgFiles + Globals.strExePath;

			// to get whole path of my project executable file.
			string completelyFullFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
			// then specify of the executable file name and new desired folder path in ProgramFiles
			string strMyProgramWithFullPath = fullPath + Path.GetFileName(completelyFullFilePath);
			// to reuse project executable file name.
			Globals.strShortCutName = Path.GetFileName(completelyFullFilePath);
			Globals.strTarget = completelyFullFilePath;
			if (!System.IO.Directory.Exists(fullPath))
			{
				try
				{
					System.IO.Directory.CreateDirectory(fullPath);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
				CopyExeFile(fullPath);
			}
			else 
			{
				if (!System.IO.File.Exists(strMyProgramWithFullPath)) 
				{
					try
					{
						System.IO.Directory.CreateDirectory(fullPath);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
				}
			}
			// to create a shortcut of the project on desktop.
			CreateShortCut();

			// Checking Database TXT file in Documents folder.
			if (!System.IO.File.Exists(Globals.strTxtFile))
			{
				FileStream FS;
				FS = new FileStream(Globals.strTxtFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				FS.Close();
			}
			// ItemsSource="{Binding}"  have to copy and paste this in DWGProgress' DataContex in XAML
			DGWProgress.ItemsSource = RetrieveDGWProgress().DefaultView;
			DGWLearned.ItemsSource = RetrieveDGWLearned().DefaultView;
			DGWListOfKnowing.ItemsSource = RetrieveDGWListOfKnowing().DefaultView;

		}

		/// <summary>
		/// Retrieve data from Text Database into DGWProgress
		/// </summary>
		private DataTable RetrieveDGWProgress()
		{
			string strARowFromTxtFile = string.Empty;
			int iDataRowCount = 0;
			string strLine = string.Empty;
			DataTable dt = new DataTable();
			dt.Columns.Add("Word", System.Type.GetType("System.String"));
			dt.Columns.Add("Meaning", System.Type.GetType("System.String"));
			dt.Columns.Add("A Sentence", System.Type.GetType("System.String"));
			StreamReader SR = new StreamReader(Globals.strTxtFile);
			// !SR.EndOfStream  (that is anothor method for while but it will skip last line in a TEXT file.) 
			while ((strLine = SR.ReadLine()) != null)			
			{
				if ((strLine.EndsWith("X") == true))
				{
					iDataRowCount += 1;
					strARowFromTxtFile = strLine;
					dt.Rows.Add(strARowFromTxtFile.Split('#').Take(3).ToArray());
				}
			}
			SR.Close();

			Globals.iDataRowCountSwap_DGV1 = iDataRowCount;
			if (iDataRowCount > 0)
			{
				lblAnnouncement.Content = "There are " + Globals.iDataRowCountSwap_DGV1.ToString() + " records in the Database.";
			}
			else
			{
				lblAnnouncement.Content = "There is not any record to count in the Database.";
			}
			if (iDataRowCount > 6)
			{
				Globals.checkCountOfData = true;
				btnTray.IsEnabled = true;
			}
			else
			{
				Globals.checkCountOfData = false;
				btnTray.IsEnabled = false;
			}
			return dt;
		}

		/// <summary>
		/// Retrieve data from Text Database into DGWLearned
		/// </summary>
		private DataTable RetrieveDGWLearned()
		{
			string strARowFromTxtFile = string.Empty;
			int iDataRowCount = 0;
			string strLine = string.Empty;
			DataTable dt = new DataTable();
			dt.Columns.Add("Word", System.Type.GetType("System.String"));
			dt.Columns.Add("Meaning", System.Type.GetType("System.String"));
			dt.Columns.Add("A Sentence", System.Type.GetType("System.String"));
			StreamReader SR = new StreamReader(Globals.strTxtFile);
			// !SR.EndOfStream  (that is anothor method for while but it will skip last line in a TEXT file.) 
			while ((strLine = SR.ReadLine()) != null)
			{
				if ((strLine.EndsWith("L") == true))
				{
					iDataRowCount += 1;
					strARowFromTxtFile = strLine;
					dt.Rows.Add(strARowFromTxtFile.Split('#').Take(3).ToArray());
				}
			}
			SR.Close();

			Globals.iDataRowCountSwap_DGV2 = iDataRowCount;
			if (iDataRowCount > 0)
			{
				lblAnnouncement.Content = "There are " + Globals.iDataRowCountSwap_DGV2.ToString() + " records in the Database.";
			}
			else
			{
				lblAnnouncement.Content = "There is not any record to count in the Database.";
			}
			if (iDataRowCount > 6)
			{
				Globals.checkCountOfData = true;
				btnTray.IsEnabled = true;
			}
			else
			{
				Globals.checkCountOfData = false;
				btnTray.IsEnabled = false;
			}
			return dt;
		}

		/// <summary>
		/// Retrieve data from Text Database into DGWListOfKnowing
		/// </summary>
		private DataTable RetrieveDGWListOfKnowing()
		{
			string strARowFromTxtFile = string.Empty;
			int iDataRowCount = 0;
			string strLine = string.Empty;
			DataTable dt = new DataTable();
			dt.Columns.Add("Word", System.Type.GetType("System.String"));
			dt.Columns.Add("Meaning", System.Type.GetType("System.String"));
			dt.Columns.Add("A Sentence", System.Type.GetType("System.String"));
			dt.Columns.Add("Counts", System.Type.GetType("System.String"));
			StreamReader SR = new StreamReader(Globals.strTxtFile);
			
			while ((strLine = SR.ReadLine()) != null)
			{
				// below two lines of code for checking zero numbers in line of text file
				string[] sFindNonZeroNumbersOutside = strLine.Split(new Char[] { '#' }).Take(4).ToArray();
				int iNonZeroNumbers = Convert.ToInt32(sFindNonZeroNumbersOutside[3]);

				if (iNonZeroNumbers != 0)
				{
					iDataRowCount += 1;
					strARowFromTxtFile = strLine;
					dt.Rows.Add(strARowFromTxtFile.Split('#').Take(4).ToArray());
					// sorting Counts column as default
					dt.DefaultView.Sort = "Counts DESC";
				}
			}
			SR.Close();
			Globals.iDataRowCountSwap_DGV3 = iDataRowCount;
			if (iDataRowCount > 0)
			{
				lblAnnouncement.Content = "There are " + Globals.iDataRowCountSwap_DGV3.ToString() + " records in the Database.";
			}
			else
			{
				lblAnnouncement.Content = "There is not any record to count in the Database.";
			}
			// I dont' want to see third column in DGWListOfKnowing
			dt.Columns.Remove("A Sentence");
			return dt;
		}

		/// <summary>
		/// This method will used in Time Ticks to provide a Quiz window
		/// </summary>
		private void buildingQuizWindow()
		{
			StreamReader sr = new StreamReader(Globals.strTxtFile);
			List<string> descLine		= new List<string>();
			List<string> wordLine		= new List<string>();
			List<string> specifyLength	= new List<string>();
			List<int> lengthString		= new List<int>();
			string line = sr.ReadLine();
			string[] strSplit = line.Split(new char[] {'#'}).Take(2).ToArray();
			wordLine.Add(strSplit[0]);
			descLine.Add(strSplit[1]);
			while (sr.Peek() > -1)
			{
				line = sr.ReadLine();
				strSplit = line.Split(new char[] {'#'}).Take(2).ToArray();
				wordLine.Add(strSplit[0]);
				descLine.Add(strSplit[1]);
			}
			sr.Close();
			sr.Dispose();
			List<int> secList = new List<int>();
			Random rnd = new Random();
			int i = 1;
			while (i <= 5) 
			{
				int j = Convert.ToInt32(rnd.Next(0, descLine.Count - 1));
				if (!secList.Contains(j)) 
				{
					secList.Add(j);
					i += 1;
				}
			}
			secList.Sort();
			for (int begin = 0; begin < secList.Count; begin++) 
			{
				// --below has not yet completed, still looking for a solution in WPF
				//((Label)mainGrid("Label" + (begin + 1))).Text = descLine[secList[begin]];
				specifyLength.Add(descLine[secList[begin]]);
				lengthString.Add(specifyLength[begin].Length);


				/////Above for loop does not work//////
				// because I could not find an equivalent of WPF yet
				//
				// below VB.Net code line and it must be converted to WPF
				// the exact line of code indicated with this -->
				//
				//For begin As Integer = 0 To seclist.Count - 1
				//--> CType(frm.Controls("Label" & begin + 1), Label).Text = DescLine(seclist(begin))
				//	specifyLength.Add(DescLine(seclist(begin)))
				//	lengthString.Add(specifyLength(begin).Length)
				//Next

			}
			lengthString.Sort();

			// generate a string count of lengthString.Item(4) to use in TextRenderer
			string mystr = string.Empty;
			
			for (int n = 0; n < Convert.ToInt32(lengthString.GetRange(4,1)); n++)
			{
				mystr += "a";
			}

			int pickANumber = rnd.Next(0, secList.Count);
			int pickedNumber = secList[pickANumber];
			frm.lblChoice.Content = wordLine[pickedNumber];
			frm.theWord = wordLine[pickedNumber];
			frm.theMeaning = descLine[pickedNumber];
		}

		public static event EventHandler _timerworker;
		DispatcherTimer dt = new DispatcherTimer();
		private bool MySubActiveTimer(bool bln)
		{
			dt = new System.Windows.Threading.DispatcherTimer();
			dt.Interval = new TimeSpan(0, 0, 0, 0, 1000);			// 500 Milliseconds x 2 = One second
			dt.Start();
			return bln;
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnView_Click(object sender, RoutedEventArgs e)
		{
			AddWindow addWPFWindow = new AddWindow();
			addWPFWindow.ShowDialog();
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnTray_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnSetting_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnInfo_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnExit_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
