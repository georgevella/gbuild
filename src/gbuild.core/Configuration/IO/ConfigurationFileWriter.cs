using System.IO;
using System.Text;
using GBuild.Core.Configuration.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GBuild.Core.Configuration.IO
{
	public static class ConfigurationFileWriter
	{
		public static void Write(
			ConfigurationFile configurationFile,
			Stream stream,
			bool writeDefaults = false
		)
		{
			using (var streamWriter = new StreamWriter(stream, Encoding.UTF8, 1024, true))
			{
				Write(configurationFile, streamWriter, writeDefaults);
			}
		}

		public static void Write(
			ConfigurationFile configurationFile,
			TextWriter writer,
			bool writeDefaults = false
		)
		{
			var serializerBuilder = new SerializerBuilder()
				.WithNamingConvention(new HyphenatedNamingConvention());

			if (writeDefaults)
			{
				serializerBuilder.EmitDefaults();
			}

			var serializer = serializerBuilder.Build();

			serializer.Serialize(writer, configurationFile);
		}
	}
}