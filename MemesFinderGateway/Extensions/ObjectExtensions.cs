using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MemesFinderGateway.Extensions
{
	public static class ObjectExtensions
	{
		private static readonly JsonSerializerOptions TelegramJsonOptions = new()
		{
			PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
		};

		public static string ToJson(this object update) => update is null ? default : JsonSerializer.Serialize(update, TelegramJsonOptions);
	}
}

