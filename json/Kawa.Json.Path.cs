﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace Kawa.Json
{
	public static partial class Json5
	{
		/// <summary>
		/// Returns an object of type T found in the specified path.
		/// </summary>
		/// <typeparam name="T">The type of object to return.</typeparam>
		/// <param name="obj">The JsonObj to look through.</param>
		/// <param name="path">The path to follow.</param>
		/// <returns>The object at the end of the path.</returns>
		/// <remarks>
		/// If obj is a Starbound Versioned JSON object, if the first key is not found,
		/// Path will automatically try skipping into the __content object.
		/// </remarks>
		public static T Path<T>(this JsonObj obj, string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				throw new ArgumentException("Path is empty.");
			if (!path.StartsWith("/"))
				throw new ArgumentException("Path does not start with root.");
			if (path.EndsWith("/"))
				throw new ArgumentException("Path does not end with a key or index.");
			var parts = path.Substring(1).Split('/');
			object root = obj;
			var here = root;
			var index = -1;
			foreach (var part in parts)
			{
				if (part == "-")
					throw new JsonException("Can't use - here; we're not patching anything.");
				bool isIndex = int.TryParse(part, out index);
				if (isIndex && here == root)
					throw new JsonException("Tried to start with an array index. That's extra special.");
				if (isIndex && here is object[])
				{
					var list = (object[])here;
					if (index < 0 || index >= list.Length)
						throw new IndexOutOfRangeException();
					here = list[index];
				}
				else if (isIndex && here is List<object>)
				{
					var list = (List<object>)here;
					if (index < 0 || index >= list.Count)
						throw new IndexOutOfRangeException();
					here = list[index];
				}
				else if (here is JsonObj)
				{
					var map = (JsonObj)here;
					if (here == root && map.ContainsKey("__content") && !map.ContainsKey(part))
					{
						//Sneakily stealthily skip into this.
						map = (JsonObj)map["__content"];
					}
					if (!map.ContainsKey(part))
					{
						throw new KeyNotFoundException();
					}
					here = map[part];
				}
				else
				{
					throw new JsonException("Current node is not an array or object, but path isn't done yet.");
				}
			}

			if (typeof(T).Name == "Int32" && here is double)
				here = (int)(double)here;
			else if (typeof(T).Name == "Int32[]" && here is List<object>)
				here = ((List<object>)here).Select(x => (int)(double)x).ToArray();
			else if (typeof(T).Name == "List`1")
			{
				var contained = typeof(T).GetGenericArguments()[0];
				var hereList = (List<object>)here;
				switch (contained.Name)
				{
					case "Int32":
						here = hereList.Select(x => (int)(double)x).ToList();
						break;
					case "Double":
						here = hereList.Select(x => (double)x).ToList();
						break;
					case "String":
						here = hereList.Select(x => (string)x).ToList();
						break;
					default:
						here = hereList.Select(x => (double)x).ToList();
						break;
				}
			}

			if (!(here is T))
				throw new JsonException(string.Format("Value at end of path is not of the requested type -- found {0} but expected {1}.", here.GetType(), typeof(T)));
			return (T)here;
		}

		/// <summary>
		/// Returns the JsonObj found in the specified path.
		/// </summary>
		/// <param name="obj">The JsonObj to look through.</param>
		/// <param name="path">The path to follow.</param>
		/// <returns>The JsonObj at the end of the path.</returns>
		/// <remarks>
		/// If obj is a Starbound Versioned JSON object, if the first key is not found,
		/// Path will automatically try skipping into the __content object.
		/// </remarks>
		public static JsonObj Path(this JsonObj obj, string path)
		{
			return Path<JsonObj>(obj, path);
		}
	}
}
