using System;
using System.Text.Json;

namespace MemesFinderGateway.Extensions
{
	public static class ObjectExtensions
	{
		public static string ToJson(this object update) => update is null ? default : JsonSerializer.Serialize(update);
	}
}

