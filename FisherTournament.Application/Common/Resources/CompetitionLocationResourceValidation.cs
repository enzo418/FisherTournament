using FluentValidation;

namespace FisherTournament.Application.Common.Resources
{
	public class CompetitionLocationResourceValidation : AbstractValidator<CompetitionLocationResource>
	{
		public CompetitionLocationResourceValidation()
		{
			RuleFor(c => c.Place).MaximumLength(75);
			RuleFor(c => c.State).MaximumLength(75);
			RuleFor(c => c.City).MaximumLength(75);
			RuleFor(c => c.Country).MaximumLength(75);
		}
	}
}
