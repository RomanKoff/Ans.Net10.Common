using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ans.Net10.Common.Json
{

	public class BoolConverter
		: JsonConverter<bool>
	{

		/* functions */


		public override bool Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			return reader.TokenType switch
			{
				JsonTokenType.True => true,
				JsonTokenType.False => false,
				JsonTokenType.String => bool.TryParse(reader.GetString(), out var b1) && b1, //throw new JsonException(),
				JsonTokenType.Number => reader.TryGetInt64(out long l1)
					? Convert.ToBoolean(l1)
					: reader.TryGetDouble(out double d1) && Convert.ToBoolean(d1),
				_ => throw new JsonException(),
			};
		}


		/* methods */


		public override void Write(
			Utf8JsonWriter writer,
			bool value,
			JsonSerializerOptions options)
		{
			writer.WriteBooleanValue(value);
		}

	}

}
