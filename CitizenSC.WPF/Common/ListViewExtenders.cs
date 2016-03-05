using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CitizenSC.WPF.Common
{
   /// <summary>
	/// This class contains a few useful extenders for the ListBox
	/// </summary>
	public class ListBoxExtenders : DependencyObject
   {
      public static readonly DependencyProperty AutoScrollToEndProperty
         = DependencyProperty.RegisterAttached("AutoScrollToEnd", typeof(bool), typeof(ListBoxExtenders),
         new UIPropertyMetadata(default(bool), OnAutoScrollToEndChanged));

      /// <summary>
      /// Returns the value of the AutoScrollToEndProperty
      /// </summary>
      /// <param name="obj">The dependency-object whichs value should be returned</param>
      /// <returns>The value of the given property</returns>
      public static bool GetAutoScrollToEnd(DependencyObject obj)
      {
         return (bool)obj.GetValue(AutoScrollToEndProperty);
      }

      /// <summary>
      /// Sets the value of the AutoScrollToEndProperty
      /// </summary>
      /// <param name="obj">The dependency-object whichs value should be set</param>
      /// <param name="value">The value which should be assigned to the AutoScrollToEndProperty</param>
      public static void SetAutoScrollToEnd(DependencyObject obj, bool value)
      {
         obj.SetValue(AutoScrollToEndProperty, value);
      }

      /// <summary>
      /// This method will be called when the AutoScrollToEnd
      /// property was changed
      /// </summary>
      /// <param name="s">The sender (the ListBox)</param>
      /// <param name="e">Some additional information</param>
      public static void OnAutoScrollToEndChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
      {
         var val = (bool)e.NewValue;
         var lb = s as ListBox;
         var ic = lb.Items;
         var data = ic.SourceCollection as INotifyCollectionChanged;

         var autoscroller = new System.Collections.Specialized.NotifyCollectionChangedEventHandler(
             (s1, e1) =>
             {
                object selectedItem = default(object);
                switch (e1.Action)
                {
                   case NotifyCollectionChangedAction.Add:
                   case NotifyCollectionChangedAction.Move:
                      selectedItem = e1.NewItems[e1.NewItems.Count - 1];
                      break;
                   case NotifyCollectionChangedAction.Remove:
                      if (ic.Count < e1.OldStartingIndex)
                         selectedItem = ic[e1.OldStartingIndex - 1];
                      else if (ic.Count > 0)
                         selectedItem = ic[0];
                      break;
                   case NotifyCollectionChangedAction.Reset:
                      if (ic.Count > 0)
                         selectedItem = ic[0];
                      break;
                }

                if (selectedItem != default(object))
                {
                   ic.MoveCurrentTo(selectedItem);
                   lb.ScrollIntoView(selectedItem);
                }
             });

         if (val)
            data.CollectionChanged += autoscroller;
         else
            data.CollectionChanged -= autoscroller;
      }
   }
}
