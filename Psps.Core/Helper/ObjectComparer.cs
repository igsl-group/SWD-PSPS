using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Psps.Core.Helper
{
    public class ObjectComparer
    {
        public static ComparisonResult Compare(object objectA, object objectB)
        {
            return Compare(objectA, objectB, string.Empty, new CompareSettings());
        }

        public static ComparisonResult Compare(object objectA, object objectB, CompareSettings settings)
        {
            return Compare(objectA, objectB, string.Empty, settings);
        }

        public static ComparisonResult Compare(object objectA, object objectB, string parentName)
        {
            return Compare(objectA, objectB, parentName, new CompareSettings());
        }

        public static ComparisonResult Compare(object objectA, object objectB, string parentName, CompareSettings settings)
        {
            var result = new ComparisonResult();

            Type objectType = null;

            if (objectA != null && objectB != null)
            {
                objectType = objectA.GetType();

                foreach (PropertyInfo propertyInfo in objectType.GetProperties(
                  BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead))
                {
                    string propertyName = !string.IsNullOrEmpty(parentName) ?
                       string.Format("{0}.{1}", parentName, propertyInfo.Name) : propertyInfo.Name;

                    if (settings.IgnoreProperties.Contains(propertyName) ||
                        settings.IgnoreProperties.Contains(propertyInfo.Name))
                        continue;

                    object valueA;
                    object valueB;

                    try
                    {
                        valueA = propertyInfo.GetValue(objectA, null);
                        valueB = propertyInfo.GetValue(objectB, null);
                    }
                    catch
                    {
                        continue; // oops... something went wrong, let's ignore this and continue looping
                    }

                    // if it is a primative type, value type or implements
                    // IComparable, just directly try and compare the value
                    if (CanDirectlyCompare(propertyInfo.PropertyType))
                    {
                        if (!AreValuesEqual(valueA, valueB))
                        {
                            result.Differences.Add(new Difference(propertyName, valueA, valueB, propertyInfo.PropertyType));
                            result.AreEqual = false;
                        }
                    }
                    // if it implements IEnumerable, then scan any items
                    else if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        IEnumerable<object> collectionItems1;
                        IEnumerable<object> collectionItems2;
                        int collectionItemsCount1;
                        int collectionItemsCount2;

                        // null check
                        if (valueA == null && valueB != null || valueA != null && valueB == null)
                        {
                            result.Differences.Add(new Difference(propertyName, valueA, valueB, propertyInfo.PropertyType));
                            result.AreEqual = false;
                        }
                        else if (valueA != null && valueB != null)
                        {
                            collectionItems1 = ((IEnumerable)valueA).Cast<object>();
                            collectionItems2 = ((IEnumerable)valueB).Cast<object>();
                            collectionItemsCount1 = collectionItems1.Count();
                            collectionItemsCount2 = collectionItems2.Count();

                            // check the counts to ensure they match
                            if (collectionItemsCount1 != collectionItemsCount2)
                            {
                                result.AreEqual = false;
                                result.Differences.Add(new Difference(propertyName + ".Count", collectionItemsCount1, collectionItemsCount2, propertyInfo.PropertyType));
                            }
                            // compare each item...

                            for (int i = 0; i < Math.Min(collectionItemsCount1, collectionItemsCount2); i++)
                            {
                                object collectionItem1;
                                object collectionItem2;
                                Type collectionItemType;

                                collectionItem1 = collectionItems1.ElementAt(i);
                                collectionItem2 = collectionItems2.ElementAt(i);
                                collectionItemType = collectionItem1.GetType();

                                string propertyItemName = string.Format("{0}[{1}]", propertyName, i);

                                result += Compare(collectionItem1, collectionItem2, propertyItemName, settings);
                            }
                        }
                    }
                    else if ((propertyInfo.PropertyType.IsClass || propertyInfo.PropertyType.IsValueType) &&
                             !ContainsType(propertyInfo.PropertyType, settings.IgnoreTypes))
                    {
                        result += Compare(propertyInfo.GetValue(objectA, null),
                                          propertyInfo.GetValue(objectB, null), propertyName, settings);
                    }
                    else
                    {
                        // Cannot compare these types, skip
                    }
                }
            }
            else
            {
                if (objectA == null && objectB == null)
                    result.AreEqual = true;
                else if (objectA != null)
                    objectType = objectA.GetType();
                else if (objectB != null)
                    objectType = objectB.GetType();

                if (!result.AreEqual)
                    result.Differences.Add(new Difference(parentName, objectA, objectB, objectType));
            }

            return result;
        }

        private static bool ContainsType(Type type, Type[] types)
        {
            if (types == null || types.Length == 0)
                return false;

            Type typeToCompare = type;

            if (type.IsGenericType)
            {
                if (!type.IsGenericTypeDefinition)
                    typeToCompare = type.GetGenericTypeDefinition();
            }

            return types.Contains(typeToCompare);
        }

        private static bool CanDirectlyCompare(Type type)
        {
            return typeof(IComparable).IsAssignableFrom(type) || type.IsPrimitive;
        }

        private static bool AreValuesEqual(object valueA, object valueB)
        {
            bool result;
            IComparable selfValueComparer;

            selfValueComparer = valueA as IComparable;

            if (valueA == null && valueB != null || valueA != null && valueB == null)
                result = false; // one of the values is null
            else if (selfValueComparer != null && selfValueComparer.CompareTo(valueB) != 0)
                result = false; // the comparison using IComparable failed
            else if (!object.Equals(valueA, valueB))
                result = false; // the comparison using Equals failed
            else
                result = true; // match

            return result;
        }
    }

    public class ComparisonResult
    {
        public ComparisonResult()
        {
            AreEqual = true;  // assume by default they are equal
            Differences = new List<Difference>();
        }

        public bool AreEqual { get; set; }

        public List<Difference> Differences { get; set; }

        public static ComparisonResult operator +(ComparisonResult result, ComparisonResult other)
        {
            if (!other.AreEqual) result.AreEqual = false;
            result.Differences.AddRange(other.Differences);
            return result;
        }
    }

    public class Difference
    {
        public Difference()
        {
        }

        public Difference(string propName, object value1, object value2, Type type)
        {
            PropertyName = propName; Value1 = value1; Value2 = value2; PropertyType = type;
        }

        public string PropertyName { get; set; }

        public Type PropertyType { get; set; }

        public object Value1 { get; set; }

        public object Value2 { get; set; }

        public override string ToString()
        {
            return string.Format("Property: {0} Value1: {1} Value2: {2} Type: {3}",
                PropertyName, Value1 ?? "null", Value2 ?? "null", PropertyType != null ? PropertyType.FullName : "null");
        }
    }

    public class CompareSettings
    {
        public CompareSettings()
        {
            IgnoreTypes = new Type[0];
            IgnoreProperties = new string[0];
        }

        public Type[] IgnoreTypes { get; set; }

        public string[] IgnoreProperties { get; set; }
    }
}