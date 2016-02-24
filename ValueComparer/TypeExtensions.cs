using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueComparison
{
    public static class TypeExtensions
    {
        public static bool IsEnumberable(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (type.IsArray)
            {
                return true;
            }

            Type[] interfaces = type.GetInterfaces();

            for (int i = 0; i < interfaces.Length; i++)
            {
                if (interfaces[i] == typeof(IEnumerable))
                {
                    return true;
                }
                else if (interfaces[i].IsGenericType && interfaces[i].GetGenericTypeDefinition() == typeof(IEnumerable))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
