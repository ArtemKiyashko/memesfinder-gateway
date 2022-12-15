namespace MemesFinderGateway.Interfaces.DecisionMaker
{
	public record Decision(
		bool DecisionResult,
		string Message = null);
}

