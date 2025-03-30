using FluentValidation;

namespace Notes.Application.Notes.Commands.CreateNote
{
    class CreateNoteCommandValidation : AbstractValidator<CreateNoteCommand>
    {
        public CreateNoteCommandValidation() 
        {
            RuleFor(createNoteCommand => createNoteCommand.Title)
                .NotEmpty().MaximumLength(250);
            RuleFor(createNoteCommand => createNoteCommand.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
