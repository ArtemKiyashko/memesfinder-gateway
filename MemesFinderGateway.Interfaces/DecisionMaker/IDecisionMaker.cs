using Telegram.Bot.Types;

namespace MemesFinderGateway.Interfaces.DecisionMaker
{
	public interface IDecisionMaker
	{
		public ValueTask<Decision> GetDecisionAsync(Update tgUpdate);
	}
}

