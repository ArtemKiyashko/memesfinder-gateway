using MemesFinderGateway.Interfaces.DecisionMaker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

namespace MemesFinderGateway.Managers.DecisionMaker
{
	public class RollDiceDecisionMaker : IDecisionMaker
	{
		private readonly Random _random;
        private readonly RollDiceDecisionMakerOptions _options;
        private readonly ILogger _logger;
        private const int DICE_SUCCESS_VALUE = 0;

        public RollDiceDecisionMaker(IOptions<RollDiceDecisionMakerOptions> options, ILogger logger)
		{
			_random = new Random();
			_options = options.Value;
            _logger = logger;
        }

		public ValueTask<Decision> GetDecisionAsync(Update tgUpdate)
		{
			var diceValue = _random.Next(0, _options.RangeTop);

			if (diceValue == DICE_SUCCESS_VALUE)
				return ValueTask.FromResult(new Decision(true));

			var decisionMessage = $"No luck this time. Expected value: {DICE_SUCCESS_VALUE}, but was: {diceValue}";
            _logger.LogInformation(decisionMessage);

            return ValueTask.FromResult(new Decision(
				false,
                decisionMessage));
		}
    }
}

