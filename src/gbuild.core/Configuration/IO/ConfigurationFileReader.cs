using System.IO;
using System.Text;
using GBuild.Core.Configuration.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GBuild.Core.Configuration.IO
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
				.WithNamingConvention(new HyphenatedNamingConvention())
				.Build();

			return deserializer.Deserialize<ConfigurationFile>(reader);
		}
	}
}