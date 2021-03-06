﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using System.Xml;
using System.Drawing;
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
		
		private void Window_Loaded_1(object sender, RoutedEventArgs e)
		{

		}

		private void buildingStone()
		{
			StreamReader sr = new StreamReader(Globals.strTxtFile);
			List<string> descLine = new List<string>();
			List<string> wordLine = new List<string>();
			List<string> specifyLength = new List<string>();
			List<int> lengthString = new List<int>();
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
			
			foreach(int begin in secList)
            {
            	// --below has not yet completed, still looking for a solution in WPF
            	(Label)(frm.Controls("Label" + (begin + 1).ToString)).Text = descLine[secList[begin]];
                specifyLength.Add(descLine[secList[begin]]);
				lengthString.Add(specifyLength[begin].Length);
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
			// below it nothing, don't consider that
			//
			//MessageBox.Show("Add Test");
			//XML();
			//DataSet ds = new DataSet();
			//ds.ReadXml(Globals.strXmlPath);
			//DGWProgress.ItemsSource = ds.Tables[0].DefaultView;
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
