using FluentValidation;

namespace FisherTournament.Application.Fishers.Commands.CreateFisher;

public class CreateFisherCommandValidator : AbstractValidator<CreateFisherCommand>
{
	public CreateFisherCommandValidator()
	{
		RuleFor(c => c.FirstName).MaximumLength(75).NotEmpty();
		RuleFor(c => c.LastName).MaximumLength(75).NotEmpty();
		RuleFor(c => c.DNI).MaximumLength(75).NotEmpty();
	}
}