using FluentValidation;

namespace FisherTournament.Application.Fishers.Commands.EditFisher;

public class EditFisherCommandValidator : AbstractValidator<EditFisherCommand>
{
	public EditFisherCommandValidator()
	{
		RuleFor(c => c.FisherId).NotEmpty();
		RuleFor(c => c.NewFirstName).MaximumLength(75).NotEmpty();
		RuleFor(c => c.NewLastName).MaximumLength(75).NotEmpty();
		RuleFor(c => c.NewDNI).MaximumLength(75).NotEmpty();
	}
}