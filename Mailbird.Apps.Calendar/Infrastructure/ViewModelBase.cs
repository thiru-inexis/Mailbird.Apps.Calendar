using System;
using System.Linq.Expressions;

namespace Mailbird.Apps.Calendar.Infrastructure
{
    public class ViewModelBase : NotificationObject
    {
        protected bool SetValue<T>(ref T targetPropertyValue, T sourceValue, Expression<Func<T>> propertyExpression)
        {
            if (Equals(targetPropertyValue, sourceValue)) return false;
            targetPropertyValue = sourceValue;
            RaisePropertyChanged(propertyExpression);
            return true;
        }

        protected bool SetValue<T>(T targetPropertyValue, T sourceValue, Action<T> valueSetter, Expression<Func<T>> propertyExpression)
        {
            if (Equals(targetPropertyValue, sourceValue)) return false;
            valueSetter(sourceValue);
            RaisePropertyChanged(propertyExpression);
            return true;
        }
    }
}
