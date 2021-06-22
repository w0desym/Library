using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Library.Controls.StateContainer
{
    [ContentProperty("Conditions")]
    public class StateContainer : ContentView
    {
        #region -- Public properties --

        public List<StateCondition> Conditions { get; set; } = new List<StateCondition>();

        public static readonly BindableProperty StateProperty = BindableProperty.Create(
           propertyName: nameof(State),
           returnType: typeof(object),
           declaringType: typeof(StateContainer),
           propertyChanged: OnStatePropertyChanged);

        public object State
        {
            get => GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        #endregion

        private static async void OnStatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is StateContainer parent)
            {
                await parent.ChooseStateAsync(newValue);
            }
        }

        private Task ChooseStateAsync(object newValue)
        {
            try
            {
                var currentCondition = Conditions?.FirstOrDefault(condition => condition?.State?.ToString() == newValue?.ToString());

                var view = currentCondition?.Content;

                Content = view;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return Task.CompletedTask;
        }
    }
}
