using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Kawa.Json;
using Microsoft.Xna.Framework.Graphics;

namespace Kafe
{
	public static class Mix
	{
		private class MixFileEntry
		{
			public string MixFile, Filename;
			public int Offset, Length;
			public bool IsCompressed;
		}

		private static Dictionary<string, MixFileEntry> fileList;

		private static Dictionary<string, object> cache;

		/// <summary>
		/// Populates the Mix database for usage
		/// </summary>
		public static void Initialize(string mainFile = "kafe")
		{
			Console.WriteLine("Initializing Mix system with main file \"{0}\"...", mainFile);
			fileList = new Dictionary<string, MixFileEntry>();
			cache = new Dictionary<string, object>();
			var mixfiles = new List<string>() { mainFile + ".zip" };
			foreach (var zipFile in Directory.EnumerateFiles(".", "*.zip"))
			{
				if (zipFile.Substring(2).Equals(mainFile + ".zip", StringComparison.OrdinalIgnoreCase))
					continue;
				Console.WriteLine(" * {0}", zipFile.Substring(2));
				mixfiles.Add(zipFile.Substring(2));
			}

			Console.WriteLine("Indexing contents...");
			foreach (var mixfile in mixfiles)
			{
				if (!File.Exists(mixfile))
				{
					Console.WriteLine("Mixfile \"{0}\" in list but nonexistant.", mixfile);
					continue;
				}
				using (var mStream = new BinaryReader(File.Open(mixfile, FileMode.Open)))
				{
					//This is not the "proper" way to do it. Fuck that.
					while (true)
					{
						var header = mStream.ReadBytes(4);
						if (header[0] != 'P' || header[1] != 'K' || header[2] != 3 || header[3] != 4)
						{
							if (header[2] == 1 && header[3] == 2) //reached the Central Directory
								break;
							throw new FileLoadException(string.Format("Mixfile \"{0}\" has an incorrect header.", mixfile));
						}
						mStream.BaseStream.Seek(4, SeekOrigin.Current);
						var method = mStream.ReadInt16();
						mStream.BaseStream.Seek(8, SeekOrigin.Current);
						var moto = mStream.ReadBytes(4);
						var compressedSize = (moto[3] << 24) | (moto[2] << 16) | (moto[1] << 8) | moto[0]; //0x000000F8
						moto = mStream.ReadBytes(4);
						var uncompressedSize = (moto[3] << 24) | (moto[2] << 16) | (moto[1] << 8) | moto[0]; //0x00000197
						moto = mStream.ReadBytes(2);
						var filenameLength = (moto[1] << 8) | moto[0];
						mStream.BaseStream.Seek(2, SeekOrigin.Current);
						var filename = new string(mStream.ReadChars(filenameLength)).Replace('/', '\\');
						var offset = (int)mStream.BaseStream.Position;
						mStream.BaseStream.Seek(compressedSize, SeekOrigin.Current);
						if (filename.EndsWith("\\"))
							continue;
						var entry = new MixFileEntry()
						{
							Offset = offset,
							Length = compressedSize,
							IsCompressed = method == 8,
							Filename = filename,
							MixFile = mixfile,
						};
						fileList[filename] = entry;
					}
				}
			}
		}

		public static bool FileExists(string fileName)
		{
			if (File.Exists(Path.Combine("data", fileName)))
				return true;
			return (fileList.ContainsKey(fileName));
		}

		/// <summary>
		/// Looks up the given file in the Mix database and returns a <see cref="MemoryStream"/> for it.
		/// </summary>
		/// <param name="fileName">The file to find.</param>
		/// <returns>Returns a <see cref="MemoryStream"/> if found, <see cref="null"/> otherwise.</returns>
		public static Stream GetStream(string fileName)
		{
			Console.WriteLine("Mix: get stream for \"{0}\"...", fileName);
			if (File.Exists(Path.Combine("data", fileName)))
				return new MemoryStream(File.ReadAllBytes(Path.Combine("data", fileName)));
			if (!fileList.ContainsKey(fileName))
				throw new FileNotFoundException("File " + fileName + " was not found in the MIX files.");
			//MemoryStream ret;
			var entry = fileList[fileName];
			using (var mStream = new BinaryReader(File.Open(entry.MixFile, FileMode.Open)))
			{
				mStream.BaseStream.Seek(entry.Offset, SeekOrigin.Begin);
				if (!entry.IsCompressed)
				{
					return new MemoryStream(mStream.ReadBytes(entry.Length));
				}
				else
				{
					var cStream = new MemoryStream(mStream.ReadBytes(entry.Length));
					var decompressor = new System.IO.Compression.DeflateStream(cStream, System.IO.Compression.CompressionMode.Decompress);
					var outStream = new MemoryStream();
					decompressor.CopyTo(outStream);
					//ret = new byte[outStream.Length];
					outStream.Seek(0, SeekOrigin.Begin);
					return outStream;
					//outStream.Read(ret, 0, ret.Length);
				}
			}
		}

		/// <summary>
		/// Looks up the given file in the Mix database and returns its contents as a <see cref="string"/>.
		/// </summary>
		/// <param name="fileName">The file to find.</param>
		/// <returns>Returns a <see cref="string"/> with the file's contents if found, <see cref="null"/> otherwise.</returns>
		public static string GetString(string fileName, bool useCache = true)
		{
			if (useCache && cache.ContainsKey(fileName))
				return (string)cache[fileName];
			Console.WriteLine("Mix: get string from \"{0}\"...", fileName);
			if (File.Exists(Path.Combine("data", fileName)))
				return File.ReadAllText(Path.Combine("data", fileName));
			if (!fileList.ContainsKey(fileName))
				throw new FileNotFoundException("File " + fileName + " was not found.");
			var bytes = GetBytes(fileName);
			var ret = Encoding.UTF8.GetString(bytes);
			if (useCache)
				cache[fileName] = ret;
			return ret;
		}

		/// <summary>
		/// Looks up the given file in the Mix database and returns its contents as a <see cref="Texture2D"/>.
		/// </summary>
		/// <param name="filename">The file to find.</param>
		/// <returns>Returns a <see cref="Texture2D"/> with the file's contents if found, <see cref="null"/> otherwise.</returns>
		public static Texture2D GetTexture(string fileName, bool useCache = true)
		{
			if (useCache && cache.ContainsKey(fileName))
				return (Texture2D)cache[fileName];
			Console.WriteLine("Mix: get texture from \"{0}\"...", fileName);
			if (!fileName.Contains("."))
				fileName += ".png";
			using (var str = GetStream(fileName))
			//Can't use a DeflateStream here.
			//using (var str = new MemoryStream(GetBytes(fileName)))
			{
				var ret = Texture2D.FromStream(Kafe.GfxDev, str);
				if (useCache)
					cache[fileName] = ret;
				return ret;
			}
		}

		/// <summary>
		/// Looks up the given file in the Mix database and returns its contents as a <see cref="byte[]"/>.
		/// </summary>
		/// <param name="fileName">The file to find.</param>
		/// <returns>Returns a <see cref="byte[]"/> with the file's contents if found, <see cref="null"/> otherwise.</returns>
		public static byte[] GetBytes(string fileName)
		{
			Console.WriteLine("Mix: get bytes from \"{0}\"...", fileName);
			var str = GetStream(fileName);
			//if (str is MemoryStream)
			//{
				var ret = new byte[str.Length];
				str.Read(ret, 0, ret.Length);
				return ret;
			//}
		}

		public static object GetJson(string fileName, bool useCache = true)
		{
			if (!fileName.Contains("."))
				fileName += ".json";
			if (useCache && cache.ContainsKey(fileName + "-asObj"))
				return cache[fileName + "-asObj"];
			Console.WriteLine("Mix: get JSON from \"{0}\"...", fileName);
			var ret = Json5.Parse(GetString(fileName));
			if (ret is Dictionary<string, object>)
			{
				foreach (var file in GetFilesWithPattern(fileName + ".patch"))
				{
					var patch = Json5.Parse(GetString(file));
					Kawa.Json.Patch.JsonPatch.Apply(ret, patch);
				}
			}
			cache[fileName + "-asObj"] = ret;
			return ret;
		}

		/// <summary>
		/// Returns a list of all the files in a given path.
		/// </summary>
		/// <param name="path">The path to look in.</param>
		/// <returns>A <see cref="string[]"/> with all the files found, which may be empty.</returns>
		public static string[] GetFilesInPath(string path)
		{
			var ret = new List<string>();
			foreach (var entry in fileList.Values)
			{
				if (entry.Filename.StartsWith(path))
					ret.Add(entry.Filename);
			}
			if (Directory.Exists(Path.Combine("data", path)))
			{
				var getFiles = Directory.GetFiles(Path.Combine("data", path), "*", SearchOption.AllDirectories);
				foreach (var x in getFiles)
				{
					if (!ret.Contains(x.Substring(5)))
						ret.Add(x.Substring(5));
				}
			}
			return ret.ToArray();
		}

		public static void GetFileRange(string fileName, out int offset, out int length, out string mixFile)
		{
			offset = -1;
			length = -1;
			mixFile = string.Empty;
			if (!fileList.ContainsKey(fileName))
				return;
			var entry = fileList[fileName];
			offset = entry.Offset;
			length = entry.Length;
			mixFile = entry.MixFile;
		}

		public static string[] GetFilesWithPattern(string pattern)
		{
			var ret = new List<string>();
			var regex = new System.Text.RegularExpressions.Regex(pattern.Replace("*", "(.*)").Replace("\\", "\\\\"));
			foreach (var entry in fileList.Values)
			{
				if (regex.IsMatch(entry.Filename))
					ret.Add(entry.Filename);
			}
			if (Directory.Exists("data"))
			{
				if (pattern.Contains("\\"))
				{
					if (!Directory.Exists(Path.Combine("data", Path.GetDirectoryName(pattern))))
						return ret.ToArray();
				}
				var getFiles = Directory.GetFiles("data", pattern, SearchOption.AllDirectories);
				foreach (var f in getFiles)
				{
					var f2 = f.Substring(5);
					if (!ret.Contains(f2))
						ret.Add(f2);
				}
			}
			return ret.ToArray();
		}

		public static void SpreadEm()
		{
			Console.WriteLine("Spreadin' em...");
			if (fileList == null || fileList.Count == 0)
				Initialize();
			foreach (var entry in fileList.Values)
			{
				if (entry.Length == 0)
				{
					Console.WriteLine("* Bogus entry with zero length: \"{0}\", offset {1}", entry.Filename, entry.Offset);
					continue;
				}
				var targetPath = Path.Combine("data", /* entry.MixFile.Remove(entry.MixFile.Length - 4),*/ entry.Filename);
				var targetDir = Path.GetDirectoryName(targetPath);
				if (!Directory.Exists(targetDir))
					Directory.CreateDirectory(targetDir);
				using (var mStream = new BinaryReader(File.Open(entry.MixFile, FileMode.Open)))
				{
					mStream.BaseStream.Seek(entry.Offset, SeekOrigin.Begin);
					if (!entry.IsCompressed)
					{
						File.WriteAllBytes(targetPath, mStream.ReadBytes(entry.Length));
					}
					else
					{
						var cStream = new MemoryStream(mStream.ReadBytes(entry.Length));
						var decompressor = new System.IO.Compression.GZipStream(cStream, System.IO.Compression.CompressionMode.Decompress);
						var outStream = new MemoryStream();
						var buffer = new byte[1024];
						var recieved = 1;
						while (recieved > 0)
						{
							recieved = decompressor.Read(buffer, 0, buffer.Length);
							outStream.Write(buffer, 0, recieved);
						}
						var unpacked = new byte[outStream.Length];
						outStream.Seek(0, SeekOrigin.Begin);
						outStream.Read(unpacked, 0, unpacked.Length);
						File.WriteAllBytes(targetPath, unpacked);
					}
				}
			}
			Console.WriteLine("All entries in mix files extracted. Happy Hacking.");
		}
	}
}