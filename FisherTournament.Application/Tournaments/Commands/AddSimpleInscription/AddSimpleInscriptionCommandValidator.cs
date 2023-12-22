using FluentValidation;

namespace FisherTournament.Application.Tournaments.Commands.AddInscription;

public class AddSimpleInscriptionCommandValidator : AbstractValidator<AddSimpleInscriptionCommand>
{
	public AddSimpleInscriptionCommandValidator()
	{
		RuleFor(c => c.TournamentId).NotEmpty();
		RuleFor(c => c.FisherFirstName).MaximumLength(75).NotEmpty();
		RuleFor(c => c.FisherLastName).MaximumLength(75).NotEmpty();
		RuleFor(c => c.CategoryId).NotEmpty();
	}
}