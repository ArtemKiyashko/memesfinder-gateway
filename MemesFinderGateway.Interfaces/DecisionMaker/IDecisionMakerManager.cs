using System;
using Telegram.Bot.Types;

namespace MemesFinderGateway.Interfaces.DecisionMaker
{
	public interface IDecisionMakerManager
	{
		public ValueTask<DecisionManagerResult> GetFinalDecisionAsync(Update tgUpdate);
	}
}

