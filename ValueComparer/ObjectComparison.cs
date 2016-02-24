using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ValueComparison
{
    public class ObjectComparison
    {
        private const BindingFlags PropertyFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
        readonly object x;
        readonly object y;

        List<PropertyComparisonResult> _results = new List<PropertyComparisonResult>();
        
        internal ObjectComparison(object x, object y)
        {
            if (x == null)
                throw new ArgumentNullException("x");
            if (y == null)
                throw new ArgumentNullException("y");

            this.x = x;
            this.y = y;
        }

        internal ObjectComparison()
        {

        }

        internal bool CompareObjects()
        {
            bool result;

            if (Object.ReferenceEquals(this.x, this.y))
                result = true;
            else if (this.x == null && this.y != null || this.x != null && this.y == null)
                result = false;
            else
                result = CompareProperties();
            this.Result = result;
            return result;
        }

        private bool CompareProperties()
        {
            if (this.AreSameType)
            {
                Type type = this.x.GetType();
                PropertyInfo[] props = type.GetProperties(PropertyFlags);
                bool flag = true;

                foreach(var pi in props)
                {
                    Type propertyType = pi.PropertyType;
                    PropertyComparisonResult propCompareResult = null;
                    if (propertyType.IsValueType)
                    {
                        propCompareResult = ValueComparer.CompareProperty(pi, x, pi, y, true);
                    }
                    else if (propertyType == typeof(string))
                    {
                        propCompareResult = ValueComparer.CompareProperty(pi, x, pi, y, true);
                    }
                    else if (propertyType.IsGenericType)
                    {
                        //Ignored
                    }
                    else
                    {
                        propCompareResult = ValueComparer.CompareProperty(pi, x, pi, y, true);
                    }
                    if (propCompareResult != null)
                    {
                        this._results.Add(propCompareResult);

                        if (!propCompareResult.Result)
                            flag = false;

                    }
                }
                return flag;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public virtual bool AreSameType
        {
            get
            {
                return this.x.GetType() == this.y.GetType();
            }
        }


        public virtual bool AreEqual
        {
            get
            {
                return this._results.All(x => x.Result);
            }
        }

        public bool Result { get; internal set; }

        public ReadOnlyCollection<PropertyComparisonResult> Results
        {
            get
            {
                return this._results.AsReadOnly();
            }
        }
    }
}
