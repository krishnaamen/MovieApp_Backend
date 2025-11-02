using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAppPortfolio.DataServiceLayer;
using MovieAppPortfolio.DataServiceLayer.dtos;
using MovieAppPortfolio.WebServiceLayer.Models;
using System.Security.Claims;

namespace MovieAppPortfolio.WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotesController : BaseController
    {
        private new readonly IDataService _dataService;
        private readonly MyDbContext _context;

        public NotesController(
            IDataService dataService,
            LinkGenerator generator,
            MyDbContext context
        ) : base(dataService, generator)
        {
            _dataService = dataService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddNote([FromBody] NoteModel noteModel)
        {
            var userId = GetCurrentUserId();
            var userNote = await _dataService.AddUserNoteAsync(userId, noteModel.NoteText, noteModel.TConst, noteModel.NConst);

            if (userNote != null)
            {
                var response = new NoteResponseModel
                {
                    NoteId = userNote.NoteId,
                    TConst = userNote.tconst ?? "",
                    NConst = userNote.nconst,
                    NoteText = userNote.NoteText,
                    Title = userNote.TitleBasic?.primaryTitle,
                    Name = userNote.NameBasic?.primaryName,
                    CreatedAt = userNote.CreatedAt
                };
                return Ok(new { message = "Note added successfully", note = response });
            }
            else
            {
                return BadRequest(new { message = "Failed to add note" });
            }
        }


        [HttpGet("{noteId}")]
        public async Task<IActionResult> GetNoteById(int noteId)
        {
            var userId = GetCurrentUserId();
            var note = await _dataService.GetUserNoteByIdAsync(noteId, userId);

            if (note == null)
                return NotFound(new { message = "Note not found" });

            var response = new NoteResponseModel
            {
                NoteId = note.NoteId,
                TConst = note.tconst ?? "",
                NConst = note.nconst,
                NoteText = note.NoteText,
                Title = note.TitleBasic?.primaryTitle,
                Name = note.NameBasic?.primaryName,
                CreatedAt = note.CreatedAt
            };

            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetMyNotes()
        {
            var userId = GetCurrentUserId();
            var notes = await _dataService.GetUserNotesAsync(userId);

            var response = notes.Select(n => new NoteResponseModel
            {
                NoteId = n.noteId,
                TConst = n.tconst ?? "",
                NConst = n.nconst,
                NoteText = n.noteText,
                Title = n.movieTitle,
                Name = n.personName,
                CreatedAt = n.createdAt
            });

            return Ok(response);
        }


        [HttpPut("{noteId}")]
        public async Task<IActionResult> UpdateNote(int noteId, [FromBody] UpdateNoteModel updateNoteModel)
        {
            var userId = GetCurrentUserId();
            var success = await _dataService.UpdateUserNoteAsync(noteId, userId, updateNoteModel.NoteText);

            if (success)
                return Ok(new { message = "Note updated successfully" });
            else
                return BadRequest(new { message = "Failed to update note" });
        }



        [HttpDelete("{noteId}")]
        public async Task<IActionResult> DeleteNote(int noteId)
        {
            var userId = GetCurrentUserId();
            var success = await _dataService.DeleteUserNoteAsync(noteId, userId);

            if (success)
                return Ok(new { message = "Note deleted successfully" });
            else
                return BadRequest(new { message = "Failed to delete note" });
        }



        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }



    }
}
