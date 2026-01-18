using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ans.Net10.Common.Json
{

	public class IntToStringConverter
		: JsonConverter<int>
	{

		/* functions */


		public override bool CanConvert(
			Type typeToConvert)
		{
			return typeof(int) == typeToConvert;
		}


		public override int Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			return reader.GetString().ToInt(0);
		}


		/* methods */


		public override void Write(
			Utf8JsonWriter writer,
			int value,
			JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString());
		}

	}

}
