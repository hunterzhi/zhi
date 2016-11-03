using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Zhi.Comm.Lib
{

    public static class EnumHelper
    {



        public static bool Contains(Type enumType, string value)
        {
            return Enum.GetNames(enumType).Contains(value, StringComparer.OrdinalIgnoreCase);
        }

        public static T GetEnumValue<T>(T defaultValue, string value)
        {
            T enumType = defaultValue;

            if ((!String.IsNullOrEmpty(value)) && (Contains(typeof(T), value)))
                enumType = (T)Enum.Parse(typeof(T), value, true);

            return enumType;
        }
        //public static T GetEnumValue<T>(string value)
        //{
        //    var enumType = typeof(T);

        //    if ((!String.IsNullOrEmpty(value)) && (Contains(typeof(T), value)))
        //        enumType = (T)Enum.Parse(typeof(T), value, true);

        //    return enumType;
        //}
        public static IList<EnumItem> GetItems(Type enumType)
        {
            IList<EnumItem> list = new List<EnumItem>();
            foreach (object value in Enum.GetValues(enumType))
            {
                string enumName = Enum.GetName(enumType, value);
                EnumItem item = new EnumItem(enumName, (int)value);
                list.Add(item);
            }
            return list;
        }
        public class EnumItem
        {
            private string _name;

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }
            private int _value;

            public int Value
            {
                get { return _value; }
                set { _value = value; }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="EnumItem"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="value">The value.</param>
            public EnumItem(string name, int value)
            {
                _name = name;
                _value = value;
            }

        }


        /// <summary>
        /// Retrieve the description on the enum, e.g.
        /// [Description("Bright Pink")]
        /// BrightPink = 2,
        /// Then when you pass in the enum, it will retrieve the description
        /// </summary>
        /// <param name="en">The Enumeration</param>
        /// <returns>A string representing the friendly name</returns>
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        ///<summary>
        ///</summary>
        ///<param name="enumeratedType"></param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        ///<exception cref="ArgumentException"></exception>
        public static string GetEnumDescription<T>(T enumeratedType)
        {
            var description = enumeratedType.ToString();

            var enumType = typeof(T);
            // Can't use type constraints on value types, so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            var fieldInfo = enumeratedType.GetType().GetField(enumeratedType.ToString());

            if (fieldInfo != null)
            {
                var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    description = ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            return description;
        }

        ///<summary>
        ///</summary>
        ///<param name="enums"></param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        ///<exception cref="ArgumentException"></exception>
        public static string GetEnumCollectionDescription<T>(Collection<T> enums)
        {
            var sb = new StringBuilder();

            var enumType = typeof(T);

            // Can't use type constraints on value types, so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            foreach (var enumeratedType in enums)
            {
                sb.AppendLine(GetEnumDescription(enumeratedType));
            }

            return sb.ToString();
        }

    }

}