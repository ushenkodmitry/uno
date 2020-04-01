﻿using CoreGraphics;
using Uno.Extensions;
using Uno.UI.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Windows.Foundation;
using Uno.Logging;
using Uno.UI;
using AppKit;

namespace Windows.UI.Xaml
{
	public partial class FrameworkElement
	{
		private CGSize? _lastAvailableSize;
		private CGSize _lastMeasure;

		partial void Initialize();

		internal bool RequiresArrange { get; private set; }

		internal bool RequiresMeasure { get; private set; }

		public override bool NeedsLayout
		{
			set
			{
				RequiresMeasure = true;
				RequiresArrange = true;

				if (ShouldInterceptInvalidate)
				{
					return;
				}

				SetSuperviewNeedsLayout();
			}
		}


		protected internal override void OnInvalidateMeasure()
		{
			base.OnInvalidateMeasure();

			RequiresMeasure = true;
			RequiresArrange = true;
		}

		/// <summary>
		/// When set, measure and invalidate requests will not be propagated further up the visual tree, ie they won't trigger a relayout.
		/// Used where repeated unnecessary measure/arrange passes would be unacceptable for performance (eg scrolling in a list).
		/// </summary>
		internal bool ShouldInterceptInvalidate { get; set; }

		public FrameworkElement()
		{
			Initialize();
		}

		public override void Layout()
		{
			try
			{
				try
				{
					// _inLayoutSubviews = true;
					var originalRequiresArrange = RequiresArrange;
					var originalRequiresMeasure = RequiresMeasure;

					if (RequiresMeasure)
					{
						XamlMeasure(Bounds.Size);
					}

					if (RequiresArrange)
					{
						OnBeforeArrange();

						var size = SizeFromUISize(Bounds.Size);
						_layouter.Arrange(new Rect(0, 0, size.Width, size.Height));

						OnAfterArrange();
					}
					else
					{
						RequiresArrange = false;
					}
				}
				finally
				{
					//_inLayoutSubviews = false;
					RequiresArrange = false;
				}
			}
			catch (Exception e)
			{
				this.Log().Error($"Layout failed in {GetType()}", e);
			}
		}
		/// <summary>
		/// Called before Arrange is called, this method will be deprecated
		/// once OnMeasure/OnArrange will be implemented completely
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected virtual void OnBeforeArrange()
		{

		}

		/// <summary>
		/// Called after Arrange is called, this method will be deprecated
		/// once OnMeasure/OnArrange will be implemented completely
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected virtual void OnAfterArrange()
		{

		}

		internal CGSize? XamlMeasure(CGSize availableSize)
		{
			// If set layout has not been called, we can 
			// return a previously cached result for the same available size.
			if (
				!RequiresMeasure
				&& _lastAvailableSize.HasValue
				&& availableSize == _lastAvailableSize
			)
			{
				return _lastMeasure;
			}

			_lastAvailableSize = availableSize;
			RequiresMeasure = false;

			var result = _layouter.Measure(SizeFromUISize(availableSize));

			return result.LogicalToPhysicalPixels();
		}

		public CGSize SizeThatFits(CGSize size)
		{
			try
			{
				//_inLayoutSubviews = true;

				var xamlMeasure = XamlMeasure(size);

				if (xamlMeasure != null)
				{
					return _lastMeasure = xamlMeasure.Value;
				}
				else
				{
					return _lastMeasure = CGSize.Empty;
				}
			}
			finally
			{
				//_inLayoutSubviews = false;
			}
		}

		protected Size SizeFromUISize(CGSize size)
		{
			var width = nfloat.IsNaN(size.Width) ? float.PositiveInfinity : size.Width;
			var height = nfloat.IsNaN(size.Height) ? float.PositiveInfinity : size.Height;

			return new Size(width, height).PhysicalToLogicalPixels();
		}

		private bool IsTopLevelXamlView()
		{
			NSView parent = this;
			while (parent != null)
			{
				parent = parent.Superview;
				if (parent is IFrameworkElement)
				{
					return false;
				}
			}
			return true;
		}
	}
}
