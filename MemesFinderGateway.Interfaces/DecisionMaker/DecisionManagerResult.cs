using System;
namespace MemesFinderGateway.Interfaces.DecisionMaker
{
	public record DecisionManagerResult(bool Decision, IEnumerable<string>? Messages);
}

