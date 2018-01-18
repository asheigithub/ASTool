﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LinkCodeGen
{
	abstract class NativeCodeCreatorBase
	{
		public static string GetAS3RuntimeTypeString(Type type)
		{
			if (type.Equals(typeof(void)))
			{
				return "RunTimeDataType.fun_void";
			}
			else if (
				type.Equals(typeof(double))
				||
				type.Equals(typeof(float))
				)
			{
				//ASBinCode.RunTimeDataType.rt_number
				return "RunTimeDataType.rt_number";
			}
			else if (
				type.Equals(typeof(UInt16))
				||
				type.Equals(typeof(UInt32))
				)
			{
				//ASBinCode.RunTimeDataType.rt_uint
				return "RunTimeDataType.rt_uint";
			}
			else if (
				type.Equals(typeof(Int16))
				||
				type.Equals(typeof(Int32))
				)
			{
				//ASBinCode.RunTimeDataType.rt_int;
				return "RunTimeDataType.rt_int";
			}
			else if (
				type.Equals(typeof(Boolean))
				)
			{
				//ASBinCode.RunTimeDataType.rt_boolean;
				return "RunTimeDataType.rt_boolean";
			}
			else if (
				type.Equals(typeof(string))
				)
			{
				return "RunTimeDataType.rt_string";
			}

			return "RunTimeDataType.rt_void";
		}


		public static ASBinCode.RunTimeDataType GetAS3Runtimetype(Type type)
		{
			if (type.Equals(typeof(void)))
			{
				return ASBinCode.RunTimeDataType.fun_void;
			}
			else if (
				type.Equals(typeof(double))
				||
				type.Equals(typeof(float))
				)
			{
				//ASBinCode.RunTimeDataType.rt_number
				return ASBinCode.RunTimeDataType.rt_number;
			}
			else if (
				type.Equals(typeof(UInt16))
				||
				type.Equals(typeof(UInt32))
				)
			{
				//ASBinCode.RunTimeDataType.rt_uint
				return ASBinCode.RunTimeDataType.rt_uint;
			}
			else if (
				type.Equals(typeof(Int16))
				||
				type.Equals(typeof(Int32))
				)
			{
				//ASBinCode.RunTimeDataType.rt_int;
				return ASBinCode.RunTimeDataType.rt_int;
			}
			else if (
				type.Equals(typeof(Boolean))
				)
			{
				//ASBinCode.RunTimeDataType.rt_boolean;
				return ASBinCode.RunTimeDataType.rt_boolean;
			}
			else if (
				type.Equals(typeof(string))
				)
			{
				return ASBinCode.RunTimeDataType.rt_string;
			}

			return ASBinCode.RunTimeDataType._OBJECT;
		}


		protected string GetLoadArgementString(Type parameterType,int position)
		{
			var rttype = GetAS3Runtimetype(parameterType);

			if (rttype > ASBinCode.RunTimeDataType.unknown)
			{
				if (parameterType.IsValueType && !parameterType.IsEnum)
				{
					string loadstructargement = Properties.Resources.LoadStructArgement;

					loadstructargement = loadstructargement.Replace("{argindex}", position.ToString());



					loadstructargement = loadstructargement.Replace("{argType}",GetTypeFullName( parameterType));

					return loadstructargement;
				}
				else
				{
					string loadargement = Properties.Resources.LoadArgement;

					loadargement = loadargement.Replace("{argindex}", position.ToString());
					loadargement = loadargement.Replace("{argtype}", GetTypeFullName( parameterType));
					return loadargement;
				}
			}
			else if (rttype == ASBinCode.RunTimeDataType.rt_int)
			{
				return string.Format("\t\t\t\t\tint arg{0} = TypeConverter.ConvertToInt(argements[{0}]);", position);
			}
			else if (rttype == ASBinCode.RunTimeDataType.rt_uint)
			{
				return string.Format("\t\t\t\t\tuint arg{0} = TypeConverter.ConvertToUInt(argements[{0}], stackframe, token);", position);
			}
			else if (rttype == ASBinCode.RunTimeDataType.rt_number)
			{
				return string.Format("\t\t\t\t\tdouble arg{0} = TypeConverter.ConvertToNumber(argements[{0}]);", position);
			}
			else if (rttype == ASBinCode.RunTimeDataType.rt_boolean)
			{
				return string.Format("\t\t\t\t\tbool arg{0} = TypeConverter.ConvertToBoolean(argements[{0}], stackframe, token).value;", position);
			}
			else if (rttype == ASBinCode.RunTimeDataType.rt_string)
			{
				return string.Format("\t\t\t\t\tstring arg{0} = TypeConverter.ConvertToString(argements[{0}], stackframe, token);", position);
			}

			return "代码生成错误，不能转换参数类型";
		}

		protected string GetLoadArgementString(ParameterInfo paramenter)
		{
			return GetLoadArgementString(paramenter.ParameterType, paramenter.Position);
		}



		public static string GetTypeFullName(Type type)
		{
			string pre = string.Empty;
			if (type.IsNested)
			{
				pre = GetTypeFullName(type.DeclaringType) + "."; 
			}

			if (type.IsGenericType)
			{
				var param = type.GetGenericArguments();


				string ns = string.Empty;

				if (string.IsNullOrEmpty(pre))
				{
					ns = type.Namespace;
					if (!string.IsNullOrEmpty(ns))
					{
						ns = ns + ".";
					}
				}
				int idx = type.Name.IndexOf("`");
				string part1 = ns+ type.Name.Substring(0, idx);

				part1 = part1 + "<";

				for (int i = 0; i < param.Length; i++)
				{
					part1 += GetTypeFullName(param[i]);

					if (i < param.Length - 1)
					{
						part1 += ",";
					}
				}

				part1 += ">";
				return pre + part1;
			}
			else
			{
				if (string.IsNullOrEmpty(pre))
				{
					return type.FullName;
				}
				else
				{
					return pre + type.Name;
				}
				
			}
		}


	}
}
