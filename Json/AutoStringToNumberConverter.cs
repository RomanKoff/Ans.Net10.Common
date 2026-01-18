using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ans.Net10.Common.Json
{

	public class AutoStringToNumberConverter
		: JsonConverter<object>
	{

		/* functions */


		public override bool CanConvert(
			Type typeToConvert)
		{
			/*
			 * see https://stackoverflow.com/questions/1749966/c-sharp-how-to-determine-whether-a-type-is-a-number
			 */
			return Type.GetTypeCode(typeToConvert) switch
			{
				TypeCode.Byte or
				TypeCode.SByte or
				TypeCode.UInt16 or
				TypeCode.UInt32 or
				TypeCode.UInt64 or
				TypeCode.Int16 or
				TypeCode.Int32 or
				TypeCode.Int64 or
				TypeCode.Decimal or
				TypeCode.Double or
				TypeCode.Single => true,
				_ => false,
			};
		}


		public override object Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.String)
			{
				var s1 = reader.GetString();
				return int.TryParse(s1, out var i1)
					? i1
					: (double.TryParse(s1, out var d1)
						? d1
						: throw new Exception($"unable to parse {s1} to number"));
			}
			if (reader.TokenType == JsonTokenType.Number)
				return reader.TryGetInt64(out long l1)
					? l1 : reader.GetDouble();
			using var document1 = JsonDocument.ParseValue(ref reader);
			throw new Exception($"unable to parse {document1.RootElement} to number");
		}


		/* methods */


		public override void Write(
			Utf8JsonWriter writer,
			object value,
			JsonSerializerOptions options)
		{
			var s1 = value.ToString();
			if (int.TryParse(s1, out var i1))
				writer.WriteNumberValue(i1);
			else if (double.TryParse(s1, out var d1))
				writer.WriteNumberValue(d1);
			else
				throw new Exception($"unable to parse {s1} to number");
		}

	}

}
