using FisherTournament.Application.Common.Resources;
using FisherTournament.Application.Common.Validators;
using FluentValidation;

namespace FisherTournament.Application.Tournaments.Commands.AddCompetitions;

public class CompetitionCommandValidation : AbstractValidator<AddCompetitionCommand>
{
	public CompetitionCommandValidation(CompetitionLocationResourceValidation locationValidator)
	{
		RuleFor(c => c.StartDateTime)
			.IsUtcDateTime();

		RuleFor(c => c.Location)
			.SetValidator(locationValidator);
	}
}

public class AddCompetitionsCommandValidation : AbstractValidator<AddCompetitionsCommand>
{
	public AddCompetitionsCommandValidation(CompetitionCommandValidation competitionCommandValidation)
	{
		RuleFor(c => c.TournamentId).NotEmpty();
		RuleFor(c => c.Competitions).NotEmpty();
		RuleForEach(c => c.Competitions)
			.NotEmpty()
			.SetValidator(competitionCommandValidation);
	}
}