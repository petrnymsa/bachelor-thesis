using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BachelorThesis.Controls
{
    public class BindableToolbarItem : ToolbarItem
    {
        public static readonly BindableProperty IsVisibleProperty =
            BindableProperty.Create("BindableToolbarItem", typeof(bool), typeof(ToolbarItem),
                true, BindingMode.TwoWay, propertyChanged: OnIsVisibleChanged);

        public BindableToolbarItem()
        {
           // InitVisibility();
        }

        public bool IsVisible
        {
            get => (bool)GetValue(IsVisibleProperty);
            set => SetValue(IsVisibleProperty, value);
        }

        private void InitVisibility()
        {
            OnIsVisibleChanged(this, true, IsVisible);
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            InitVisibility();
        }

        private static void OnIsVisibleChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var item = bindable as BindableToolbarItem;

            if (item?.Parent == null)
                return;

         

            var items = ((ContentPage)item.Parent).ToolbarItems;


            if ((bool)newvalue && !items.Contains(item))
            {
                Device.BeginInvokeOnMainThread(() => { items.Add(item); });
            }
            else if (!(bool)newvalue && items.Contains(item))
            {
                Device.BeginInvokeOnMainThread(() => { items.Remove(item); });
            }
        }
    }
}
