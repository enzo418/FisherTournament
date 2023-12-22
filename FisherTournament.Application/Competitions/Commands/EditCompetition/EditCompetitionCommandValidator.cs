using FisherTournament.Application.Common.Resources;
using FluentValidation;

namespace FisherTournament.Application.Competitions.Commands.EditCompetition
{
	public class EditCompetitionCommandValidator : AbstractValidator<EditCompetitionCommand>
	{
		public EditCompetitionCommandValidator(CompetitionLocationResourceValidation locationValidator)
		{
			RuleFor(c => c.Location!.Value)
				.SetValidator(locationValidator)
				.When(c => c.Location != null);
		}
	}
}
