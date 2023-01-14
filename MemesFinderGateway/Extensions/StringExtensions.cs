using System;
using Telegram.Bot.Types;
using Newtonsoft.Json;

namespace MemesFinderGateway.Extensions
{
	public static class StringExtensions
	{
		public static Update ToTgUpdate(this string rawUpdate)
		{
			try
			{
				return JsonConvert.DeserializeObject<Update>(rawUpdate);
			}
			catch (Exception)
			{
				return default;
			}
		}
	}
}

