using MemesFinderGateway.Interfaces.DecisionMaker;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace MemesFinderGateway.Managers.DecisionMaker
{
    public class DecisionMakerManager : IDecisionMakerManager
    {
        private readonly IEnumerable<IDecisionMaker> _decisionMakers;
        private readonly ILogger<DecisionMakerManager> _logger;

        public DecisionMakerManager(IEnumerable<IDecisionMaker> decisionMakers, ILogger<DecisionMakerManager> logger)
        {
            _decisionMakers = decisionMakers;
            _logger = logger;
        }

        public async ValueTask<DecisionManagerResult> GetFinalDecisionAsync(Update tgUpdate)
        {
            var decisions = new List<Decision>();
            foreach (var decisionMaker in _decisionMakers)
            {
                var decision = await decisionMaker.GetDecisionAsync(tgUpdate);
                decisions.Add(decision);
            }

            var finalDecisionResult = new DecisionManagerResult(
                !decisions.Any(decision => decision.DecisionResult == false),
                decisions.Where(decision => !string.IsNullOrEmpty(decision.Message)).Select(decision => decision.Message));

            return finalDecisionResult;
        }
    }
}

