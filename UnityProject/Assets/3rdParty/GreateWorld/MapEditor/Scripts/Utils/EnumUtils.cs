#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace GEngine
{
	public static class EnumUtils
	{
		public static string GetDescription(this Enum value)
		{
			DescriptionAttribute[] da =
				(DescriptionAttribute[]) (value.GetType().GetField(value.ToString())).GetCustomAttributes(
					typeof(DescriptionAttribute), false);
			return da.Length > 0 ? da[0].Description : value.ToString();
		}

		public static IDictionary<T, string> GetEnumValuesWithDescription<T>(this Type type)
			where T : struct, IConvertible
		{
			if (!type.IsEnum)
			{
				throw new ArgumentException("T must be an enumerated type");
			}

			return type.GetEnumValues()
				.OfType<T>()
				.ToDictionary(
					key => key,
					val => (val as Enum).GetDescription()
				);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// usage
		/// var d = typeof(xxx).GetDescriptions<xxx>();
		public static List<string> GetDescriptions<T>(this Type type) where T : struct, IConvertible
		{
			if (!type.IsEnum)
			{
				throw new ArgumentException("T must be an enumerated type");
			}

			return type.GetEnumValues()
				.OfType<T>()
				.Where(o=> !o.ToString().ToLower().Equals("count"))
				.Select(o => (o as Enum).GetDescription())
				.ToList();
		}
	}
}
#endif