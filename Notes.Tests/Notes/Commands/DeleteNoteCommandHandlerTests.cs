﻿using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteNote;
using Notes.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Notes.Commands
{
    public class DeleteNoteCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteNoteCommandHandler_Success()
        {
            // Arrange
            var handler = new DeleteNoteCommandHandler(Context);

            // Act 
            await handler.Handle(new DeleteNoteCommand
            {
                Id = NotesContextFactory.NoteIdForDelete,
                UserId = NotesContextFactory.UserAId
            },
            CancellationToken.None);

            // Assert
            Assert.Null(Context.Notes.SingleOrDefault(note =>
                note.Id == NotesContextFactory.NoteIdForDelete));
        }

        [Fact]
        public async Task DeleteNoteCommandHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new DeleteNoteCommandHandler(Context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle( new DeleteNoteCommand
                    {
                        Id = Guid.NewGuid(),
                        UserId = NotesContextFactory.UserAId
                    },
                    CancellationToken.None)
                );
        }

        [Fact]
        public async Task DeleteNoteCommandHandler_FailOnWrongUserId()
        {
            // Arrage
            var deleteHandler = new DeleteNoteCommandHandler(Context);
            var createHandler = new CreateNoteCommandHandler(Context);
            var noteId = await createHandler.Handle(
                new CreateNoteCommand
                {
                    Title = "NoteTitle",
                    Details = "NoteDetails",
                    UserId = NotesContextFactory.UserAId
                }, 
                CancellationToken.None);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await deleteHandler.Handle(
                    new DeleteNoteCommand
                    {
                        Id = noteId,
                        UserId = NotesContextFactory.UserBId
                    },
                    CancellationToken.None)
                );
        }
    }
}
