using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ValueComparison
{
    public sealed class PropertyComparisonResult : ObjectComparison
    {

        PropertyInfo lhsProp;
        PropertyInfo rhsProp;
        object lhsValue;
        object rhsValue;
        

        internal PropertyComparisonResult(object value1, PropertyInfo prop1, object value2, PropertyInfo prop2) : base()
        {
            this.lhsValue = value1;
            this.rhsValue = value2;
            this.lhsProp = prop1;
            this.rhsProp = prop2;
            this.Result = false;
        }


        public override bool AreSameType
        {
            get
            {
                return this.lhsProp.PropertyType == this.rhsProp.PropertyType;
            }
        }

        public PropertyInfo Property1
        {
            get
            {
                return this.lhsProp;
            }
        }

        public object Value1
        {
            get
            {
                return this.lhsValue;
            }
        }

        public PropertyInfo Property2
        {
            get
            {
                return this.rhsProp;
            }
        }

        public object Value2
        {
            get
            {
                return this.rhsValue;
            }
        }
    
        public bool Result { get; set; }
    }
}
