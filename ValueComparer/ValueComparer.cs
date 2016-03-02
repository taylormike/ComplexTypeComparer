using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ValueComparison
{
    public class ValueComparer
    {

        private const BindingFlags PropertyFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        public static bool AreEqual<T>(T x, T y) where T : class
        {
            if (x == null && y != null || x != null && y == null)
                return false;

            ObjectComparison context = new ObjectComparison(x, y);

            bool equal = context.CompareObjects();
            return equal;
        }

        public static ObjectComparison Compare<T>(T x, T y) where T : class
        {
            if (x == null)
                throw new ArgumentNullException("x");
            if (y == null)
                throw new ArgumentNullException("y");

            var context = new ObjectComparison(x, y);

            context.CompareObjects();

            return context;
        }

        public static ObjectComparison Compare<T, U>(T x, T y)
            where T : class
            where U : class
        {
            if (x == null)
                throw new ArgumentNullException("x");
            if (y == null)
                throw new ArgumentNullException("y");

            var context = new ObjectComparison(x, y);

            context.CompareObjects();

            return context;
        }

        public static bool AreEqual(object x, object y, bool caseSensitive)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if ((x == null && y != null) || (x != null && y == null))
            {
                return false;
            }

            StringComparer comparer;
            StringComparison comparison;
            if (caseSensitive)
            {
                comparison = StringComparison.Ordinal;
                comparer = StringComparer.Ordinal;
            }
            else
            {
                comparison = StringComparison.OrdinalIgnoreCase;
                comparer = StringComparer.OrdinalIgnoreCase;
            }

            Type xType = x.GetType();
            Type yType = y.GetType();

            PropertyInfo[] xProps = xType.GetProperties(PropertyFlags);
            PropertyInfo[] yProps = yType.GetProperties(PropertyFlags);

            Dictionary<string, PropertyInfo> xPropMap = new Dictionary<string, PropertyInfo>(comparer);
            Dictionary<string, PropertyInfo> yPropMap = new Dictionary<string, PropertyInfo>(comparer);

            xProps.ForEach(pi => xPropMap.Add(pi.Name, pi));
            yProps.ForEach(pi => yPropMap.Add(pi.Name, pi));

            var comparableProperties = xProps.Select(p => p.Name).Intersect(yProps.Select(p => p.Name), comparer);

            List<Tuple<PropertyInfo, PropertyInfo>> propTuples = new List<Tuple<PropertyInfo, PropertyInfo>>();

            foreach (string propName in comparableProperties)
            {
                PropertyInfo xPropertyInfo = xProps.Single(p => String.Equals(p.Name, propName, comparison));
                PropertyInfo yPropertyInfo = yProps.Single(p => String.Equals(p.Name, propName, comparison));
                Tuple<PropertyInfo, PropertyInfo> tuple = new Tuple<PropertyInfo, PropertyInfo>(xPropertyInfo, yPropertyInfo);
                propTuples.Add(tuple);
            }

            foreach (Tuple<PropertyInfo, PropertyInfo> propTuple in propTuples)
            {
                PropertyComparisonResult compareResult = CompareProperty(propTuple.Item1, x, propTuple.Item2, y, caseSensitive);
                if (!compareResult.Result) return false;
            }

            return true;
        }

        internal static PropertyComparisonResult CompareProperty(PropertyInfo xProp, object x, PropertyInfo yProp, object y, bool caseSensitive)
        {
            object xValue = xProp.GetValue(x, null);
            object yValue = yProp.GetValue(y, null);

            PropertyComparisonResult result = new PropertyComparisonResult(xValue, xProp, yValue, yProp);

            if (xProp.PropertyType.IsValueType && yProp.PropertyType.IsValueType)
            {
                if (xProp.PropertyType == typeof(DateTime) && yProp.PropertyType == typeof(DateTime))
                {
                    DateTime dt1 = (DateTime)xValue;
                    DateTime dt2 = (DateTime)yValue;
                    TimeSpan diff = dt1 - dt2;
                    if (diff < TimeSpan.Zero)
                        diff = -diff;
                    if (diff.Ticks > TimeSpan.TicksPerSecond)
                        result.Result = false;
                    else
                        result.Result = true;
                }
                else
                    result.Result = ValueType.Equals(xValue, yValue);
            }
            else if (xProp.PropertyType == typeof(string) && yProp.PropertyType == typeof(string))
            {
                result.Result = String.Equals((string)xValue, (string)yValue);
            }
            else if (xProp.PropertyType.IsEnumberable() && yProp.PropertyType.IsEnumberable())
            {
                result.Result = ValueComparer.AreEnumerableEqual(xValue, yValue, caseSensitive, false);
            }
            else if (xProp.PropertyType.IsGenericType && yProp.PropertyType.IsGenericType)
            {
                // Non-enumerable generics ignored
                result.Result = true;
            }
            else
            {
                result.Result = ValueComparer.AreEqual(xValue, yValue, caseSensitive);
            }

            return result;

        }

        private static bool AreEnumerableEqual(object x, object y, bool caseSensitive, bool countOnlyComparison)
        {
            if (Object.ReferenceEquals(x, y))
                return true;
            else if (x == null && y != null || x != null && y == null)
                return false;

            if (!x.GetType().IsEnumberable())
                throw new ArgumentException("x");
            if (!y.GetType().IsEnumberable())
                throw new ArgumentException("y");


            var source1 = ((IEnumerable)x).Cast<object>().ToList();
            var source2 = ((IEnumerable)y).Cast<object>().ToList();

            if (source1.Count != source2.Count)
                return false;


            if (countOnlyComparison)
            {
                // Only compare based on number of items in sources
                return true;
            }

            // using KeyValuePair<,> approach
            foreach (var item in source1.Zip(source2))
            {
                if (!ValueComparer.AreEqual(item.Key, item.Value))
                    return false;
            }

            return true;
        }

    }
}


