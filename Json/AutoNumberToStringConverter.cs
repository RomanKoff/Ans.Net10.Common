using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ans.Net10.Common.Json
{

	public class AutoNumberToStringConverter
		: JsonConverter<object>
	{

		/* functions */


		public override bool CanConvert(
			Type typeToConvert)
		{
			return typeof(string) == typeToConvert;
		}


		public override object Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Number)
				return reader.TryGetInt64(out long l1)
					? l1.ToString()
					: reader.GetDouble().ToString();
			if (reader.TokenType == JsonTokenType.String)
				return reader.GetString();
			using var document1 = JsonDocument.ParseValue(ref reader);
			return document1.RootElement.Clone().ToString();
		}


		/* methods */


		public override void Write(
			Utf8JsonWriter writer,
			object value,
			JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString());
		}

	}

}
