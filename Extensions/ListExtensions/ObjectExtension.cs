using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ListExtensions
{
    public static class ObjectExtension
    {
        public static T ReduceCollectionProperties<T>(this T firstElement, T secondElement)
            where T: class
        {
            var arrayType = typeof(Array);
            var enumerableType = typeof(IEnumerable<>);

            typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.GetField | BindingFlags.Static)
                .ToList()
                .ForEach(propInfo => {
                    // if property is a List or a Dictionary
                    if (propInfo.PropertyType.IsGenericType &&

                        (propInfo.PropertyType).GenericTypeArguments != null && 
                        (propInfo.PropertyType).GenericTypeArguments.Length > 0)
                    {
                        var genericArguments = (propInfo.PropertyType).GenericTypeArguments;
                        var isListProperty = genericArguments.Count() == 1 && propInfo.PropertyType.IsEquivalentTo(typeof(List<>).MakeGenericType(genericArguments.First()));
                   
                        dynamic first = propInfo.GetValue(firstElement, null);
                        dynamic second = propInfo.GetValue(secondElement, null);

                        if (isListProperty)
                        {
                           first.AddRange(second);

                            return;
                        }

                        var isDictionaryProperty = genericArguments.Count() == 2 && propInfo.PropertyType.IsEquivalentTo(typeof(Dictionary<,>).MakeGenericType(genericArguments));

                        if (isDictionaryProperty)
                        {
                            foreach (var item in second)
                            {
                                // explicitly DON'T check to see if the key in the second dictionary is present or not in the first one
                                // if the key is present, the Add operation should fail naturally
                                first.Add(item.Key, item.Value);
                            }

                            return;
                        }
                    }

                    // if property is an Array
                    if (propInfo.PropertyType.IsArray)
                    {
                        dynamic firstArray = propInfo.GetValue(firstElement, null);
                        dynamic secondArray = propInfo.GetValue(secondElement, null);

                        var newArray = Array.CreateInstance(propInfo.PropertyType.GetElementType(), firstArray.Length + secondArray.Length);
                        for (var i = 0; i < firstArray.Length; i++)
                        {
                            newArray[i] = firstArray[i];
                        }
                        for (var i = 0; i < secondArray.Length; i++)
                        {
                            newArray[firstArray.Length + i] = secondArray[i];
                        }

                        propInfo.SetValue(firstElement, newArray);
                    }
                });

            return firstElement;
        }
    }
}