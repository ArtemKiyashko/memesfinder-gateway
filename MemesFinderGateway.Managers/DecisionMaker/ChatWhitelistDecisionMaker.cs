using System;
using MemesFinderGateway.Interfaces.DecisionMaker;
using MemesFinderGateway.Managers.DecisionMaker.Extensions;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

namespace MemesFinderGateway.Managers.DecisionMaker
{
    public class ChatWhitelistDecisionMaker : IDecisionMaker
    {
        private readonly ChatWhitelistDecisionMakerOptions _options;

        public ChatWhitelistDecisionMaker(IOptions<ChatWhitelistDecisionMakerOptions> options)
        {
            _options = options.Value;
        }

        public ValueTask<Decision> GetDecisionAsync(Update tgUpdate)
        {
            var chat = tgUpdate.GetChat();

            if (chat is null)
                return ValueTask.FromResult(new Decision(false, $"Chat not found"));

            if (string.IsNullOrEmpty(_options.AllowedChatIds))
                return ValueTask.FromResult(new Decision(true));

            var allowedChats = _options.AllowedChatIds.Split(';').ToHashSet<string>();

            if (allowedChats.Contains(chat.Id.ToString()))
                return ValueTask.FromResult(new Decision(true));

            return ValueTask.FromResult(new Decision(false, $"Chat {chat.Id} not allowed"));
        }
    }
}

