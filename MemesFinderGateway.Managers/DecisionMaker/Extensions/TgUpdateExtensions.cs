using System;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MemesFinderGateway.Managers.DecisionMaker.Extensions
{
	public static class TgUpdateExtensions
	{
		public static Chat? GetChat(this Update tgUpdate) => tgUpdate?.Type switch
		{
			UpdateType.EditedMessage => tgUpdate?.EditedMessage?.Chat,
			UpdateType.Message => tgUpdate?.Message?.Chat,
			_ => null
		};
	}
}

