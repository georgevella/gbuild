﻿using System.IO;
using System.Text;
using GBuild.Configuration.Entities;
using GBuild.Configuration.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GBuild.Configuration.IO
{
	public static class ConfigurationFileReader
	{
		public static ConfigurationFile Read(
			Stream stream
		)
		{
			using (var streamReader = new StreamReader(stream, Encoding.UTF8, false, 1024, true))
			{
				return Read(streamReader);
			}
		}

		public static ConfigurationFile Read(
			TextReader reader
		)
		{
			var deserializer = new DeserializerBuilder()
				.WithNamingConvention(new CamelCaseNamingConvention())
				.Build();

			return deserializer.Deserialize<ConfigurationFile>(reader);
		}
	}
}