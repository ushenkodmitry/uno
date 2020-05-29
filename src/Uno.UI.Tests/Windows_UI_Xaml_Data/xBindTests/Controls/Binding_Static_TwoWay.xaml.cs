using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Uno.UI.Tests.Windows_UI_Xaml_Data.xBindTests.Controls
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Binding_Static_TwoWay : Page
	{
		public Binding_Static_TwoWay()
		{
			this.InitializeComponent();
		}
	}

	public class Binding_Static_TwoWay_Data
	{
		public static Binding_Static_TwoWay_Data2 Instance { get; } = new Binding_Static_TwoWay_Data2();
	}

	public class Binding_Static_TwoWay_Data2 : INotifyPropertyChanged
	{
		private int _data;

		public Binding_Static_TwoWay_Data2()
		{
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public int Data
		{
			get => _data; set
			{
				_data = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Data)));
			}
		}
	}
}
