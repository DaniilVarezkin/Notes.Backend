using Notes.Tests.Common;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Application.Notes.Commands.CreateNote;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;

namespace Notes.Tests.Notes.Commands
{
    public class UpdateNoteCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateNoteCommandHandler_Success()
        {
            // Arrage
            var handler = new UpdateNoteCommandHandler(Context);
            var updatedTitle = "Updated Title";

            // Act
            await handler.Handle(new UpdateNoteCommand
            {
                Id = NotesContextFactory.NoteIdForUpdate,
                UserId = NotesContextFactory.UserBId,
                Title = updatedTitle,
            }, CancellationToken.None);

            // Assert
            Assert.NotNull(Context.Notes.SingleOrDefaultAsync(note => 
                note.Id == NotesContextFactory.NoteIdForUpdate &&
                note.Title == updatedTitle));
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_FailOnWrongId()
        {
            // Arrage
            var handler = new UpdateNoteCommandHandler(Context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new UpdateNoteCommand
                    {
                        Id = Guid.NewGuid(),
                        UserId = NotesContextFactory.UserBId,
                        Title = "New Title"
                    }, CancellationToken.None));
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_FailOnWrongUserId()
        {
            // Arrage
            var handler = new UpdateNoteCommandHandler(Context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateNoteCommand
                    {
                        Id = NotesContextFactory.NoteIdForUpdate,
                        UserId = NotesContextFactory.UserAId
                    }, CancellationToken.None);
            });

        }
    }
}
