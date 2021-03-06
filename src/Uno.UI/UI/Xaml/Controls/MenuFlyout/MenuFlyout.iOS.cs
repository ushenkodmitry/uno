using System;
using System.Collections.Generic;
using System.Linq;
using Uno.Disposables;
using System.Text;
using System.Windows.Input;
using Foundation;
using Uno.Client;
using Uno.Extensions;
using Uno.UI.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UIKit;

namespace Windows.UI.Xaml.Controls
{
	public partial class MenuFlyout
	{

#pragma warning disable CS0618 // Type or member is obsolete
		private string LocalizedCancelString => NSBundle.FromIdentifier("com.apple.UIKit").LocalizedString("Cancel", null);
#pragma warning restore CS0618 // Type or member is obsolete

		internal protected override void Open()
		{
			if (UseNativePopup)
			{

				if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
				{
					ShowAlert(Target);
				}
				else if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0))
				{
					ShowActionSheet(Target);
				}
			}
			else
			{
				base.Open();
			}
		}

		internal protected override void Close()
		{
			if (UseNativePopup)
			{

				if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
				{
					HideAlert();
				}
				else if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0))
				{
					HideActionSheet();
				}
			}
			else
			{
				base.Close();
			}
		}
	}
}
