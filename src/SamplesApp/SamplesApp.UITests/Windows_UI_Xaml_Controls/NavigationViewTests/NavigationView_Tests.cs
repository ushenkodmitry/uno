﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SamplesApp.UITests.TestFramework;
using Uno.UITest.Helpers;
using Uno.UITest.Helpers.Queries;

namespace SamplesApp.UITests.Windows_UI_Xaml_Controls.NavigationViewTests
{
	public class NavigationView_Tests : SampleControlUITestBase
	{
		[Test]
		[AutoRetry()]
		public void BasicNavigationView()
		{
			Run("SamplesApp.Samples.Windows_UI_Xaml_Controls.NavigationViewTests.NavigationView_Pane_Automated");

			var itemPlay = _app.Marked("Item Play");
			var itemSave = _app.Marked("Item Save");
			var selectedItemText = _app.Marked("selectedItemText");

			// Work around for Xamarin.Android not showing the icon (VS2017 only)
			var itemPlayRect = _app.Query(itemPlay).First().Rect;
			var itemSaveRect = _app.Query(itemSave).First().Rect;

			_app.TapCoordinates(itemPlayRect.X + 5, itemPlayRect.Y + 5);
			_app.WaitForDependencyPropertyValue(selectedItemText, "Text", "Play");

			_app.TapCoordinates(itemSaveRect.X + 5, itemSaveRect.Y + 5);
			_app.WaitForDependencyPropertyValue(selectedItemText, "Text", "Save");
		}
	}
}
