using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Knx
{
    [DataContract(IsReference = true)]
    public abstract class BindableObject : ExtendedObject, INotifyPropertyChanged
    {
        #region private fields

        private static readonly Dictionary<string, PropertyChangedEventArgs> ChangedEventArgCache;

        private static readonly object SyncLock = new object();

        #endregion private fields

        #region construction

        static BindableObject()
        {
            ChangedEventArgCache = new Dictionary<string, PropertyChangedEventArgs>();
        }

        protected BindableObject()
        {
        }

        #endregion construction

        protected override bool Set<T>(string name, T value)
        {
            var result = base.StorePropertyValue(name, value);
            if (result)
                RaisePropertyChanged(name);

            return result;
        }

        #region Public Members

        /// <summary>
        /// Raised when a public property of this object is set.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("propertyName");

            PropertyChangedEventArgs args;

            lock (SyncLock)
            {
                if (!ChangedEventArgCache.TryGetValue(propertyName, out args))
                    ChangedEventArgCache.Add(propertyName, args = new PropertyChangedEventArgs(propertyName));
            }

            return args;
        }

        #endregion Public Members

        /// <summary>
        /// Attempts to raise the PropertyChanged event, and
        /// invokes the virtual AfterPropertyChanged method,
        /// regardless of whether the event was raised or not.
        /// </summary>
        /// <param name="propertyName">
        /// The property which was changed.
        /// </param>
        protected void RaisePropertyChanged(string propertyName)
        {
            try
            {
                var handler = PropertyChanged;
                if (handler != null)
                    handler(this, GetPropertyChangedEventArgs(propertyName));
            }
            finally
            {
                AfterPropertyChanged(propertyName);
            }
        }

        protected virtual void AfterPropertyChanged(string propertyName)
        {
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            RaisePropertyChanged(ObjectHelper.GetPropertyName(propertyExpression));
        }
    }
}