using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Kawa.Json
{
	public enum SchemaErrors
	{
		None,
		MissingRequirement,
		SchemaMismatch,
		TypeMismatch,
		PatternMismatch,
		NumberMismatch,
	}

	public static partial class Json5
	{
		public static SchemaErrors SchemaError { get; private set; }

		private static bool typesMatch(string type, object item)
		{
			return ((type == "object" && item is JsonObj) ||
				(type == "string" && item is string) ||
				(type == "array" && (item is List<object> || item is object[])) ||
				(type == "number" && (item is int || item is double || item is float)) ||
				(type == "integer" &&
					(item is int) ||
					(
						(item is double || item is float) &&
						((int)(Convert.ToDouble(item)) == Convert.ToInt64(item))
					)
				)
			);
		}

		public static bool IsValid(JsonObj data, JsonObj schema, JsonObj node = null)
		{
			if (node == null)
			{
				SchemaError = SchemaErrors.None;
				if (!schema["type"].Equals("object"))
					throw new JsonException("Can only validate objects.");
				return IsValid(data, schema, schema);
			}
			var properties = node["properties"] as JsonObj;
			if (node.ContainsKey("required"))
			{
				foreach (var requirement in (node["required"] as List<object>).Select(i => i.ToString()))
				{
					if (!data.ContainsKey(requirement))
					{
						SchemaError = SchemaErrors.MissingRequirement;
						return false;
					}
				}
			}
			foreach (var property in properties)
			{
				var key = property.Key;
				var value = property.Value as JsonObj;
				var type = value["type"] as string;
				if (data.ContainsKey(key))
				{
					var item = data[key];
					if (!typesMatch(type, item))
					{
						SchemaError = SchemaErrors.TypeMismatch;
						return false;
					}
					if (type == "object")
					{
						if (!IsValid(item as JsonObj, schema, value))
						{
							return false;
						}
					}
					if (type == "array")
					{
						var array = (IEnumerable<object>)item;
						if (value.ContainsKey("items"))
						{
							var items = value["items"] as JsonObj;
							var itemType = items["type"] as string;
							foreach (var thing in array)
							{
								if (!typesMatch(itemType, thing))
								{
									SchemaError = SchemaErrors.TypeMismatch;
									return false;
								}
							}
						}
						if (value.ContainsKey("maxItems") && array.Count() >= Convert.ToInt64(value["maxItems"]))
							return false;
						if (value.ContainsKey("minItems") && array.Count() < Convert.ToInt64(value["minItems"]))
							return false;
						if (value.ContainsKey("uniqueItems") && Convert.ToBoolean(value["uniqueItems"]))
						{
							var distinct = array.Distinct();
							if (array.Count() != distinct.Count())
								return false;
						}
					}
					if (type == "string")
					{
						if (value.ContainsKey("pattern") && !Regex.IsMatch(item as string, value["pattern"] as string, RegexOptions.IgnoreCase))
						{
							SchemaError = SchemaErrors.PatternMismatch;
							return false;
						}
						if (value.ContainsKey("maxLength") && (item as string).Length > Convert.ToInt64(value["maxLength"]))
							return false;
						if (value.ContainsKey("minLength") && (item as string).Length < Convert.ToInt64(value["minLength"]))
							return false;
					}
					if (type == "integer" || type == "number")
					{
						var lastError = SchemaError;
						SchemaError = SchemaErrors.NumberMismatch;
						var val = Convert.ToDouble(item);
						if (value.ContainsKey("multipleOf") && val % Convert.ToDouble(value["multipleOf"]) != 0)
							return false;
						if (value.ContainsKey("maximum"))
						{
							var max = Convert.ToDouble(value["maximum"]);
							if (value.ContainsKey("exclusiveMaximum") && Convert.ToBoolean(value["exclusiveMaximum"]) && val >= max)
								return false;
							else if (val > max)
								return false;
						}
						if (value.ContainsKey("minimum"))
						{
							var min = Convert.ToDouble(value["minimum"]);
							if (value.ContainsKey("exclusiveMinimum") && Convert.ToBoolean(value["exclusiveMinimum"]) && val <= min)
								return false;
							else if (val < min)
								return false;
						} 
						SchemaError = lastError;
					}
				}
			}
			return true;
		}

		public static bool IsValid(object data, object schema)
		{
			if (!(data is JsonObj && schema is JsonObj))
				throw new ArgumentException("Can only validate JsonObj.");
			return IsValid((JsonObj)data, (JsonObj)schema);
		}
	}
}
