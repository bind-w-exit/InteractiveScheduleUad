using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InteractiveScheduleUad.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET: api/<AuthorController>
        /// <summary>
        /// Retrieves all authors
        /// </summary>
        /// <response code="200">Success - Returns an array of authors</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<AuthorForReadDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<AuthorForReadDto>>> Get()
        {
            IEnumerable<AuthorForReadDto> authors = await _authorService.GetAllAsync();

            return Ok(authors);
        }

        // GET api/<AuthorController>/5
        /// <summary>
        /// Retrieves a author by its ID
        /// </summary>
        /// <param name="id">Author ID</param>
        /// <response code="200">Success - Returns the author with the specified ID</response>
        /// <response code="404">NotFound - Author with the specified ID was not found</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Author), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Author>> Get(int id)
        {
            Author? author = await _authorService.GetByIdAsync(id);

            if (author is null)
                return NotFound("Author with the specified ID was not found");
            else
                return Ok(author);
        }

        // POST api/<AuthorController>
        /// <summary>
        /// Creates a new author
        /// </summary>
        /// <param name="author">The new author</param>
        /// <response code="201">Created - Returns the created author</response>
        /// <response code="400">BadRequest - One or more validation errors occurred</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Author), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Author>> Post(AuthorForWriteDto author)
        {
            Author authorFromDb = await _authorService.CreateAsync(author);

            return CreatedAtAction(nameof(Get), new { id = authorFromDb.Id }, authorFromDb);
        }

        // PUT api/<AuthorController>/5
        /// <summary>
        /// Updates an existing author
        /// </summary>
        /// <param name="id">The ID of the author to update</param>
        /// <param name="author">The updated author data</param>
        /// <response code="200">Success - Successfully updated</response>
        /// <response code="400">BadRequest - One or more validation errors occurred</response>
        /// <response code="404">NotFound - Author with the specified ID was not found</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Put(int id, [FromBody] AuthorForWriteDto author)
        {
            bool success = await _authorService.UpdateAsync(id, author);

            if (!success)
                return NotFound("Author with the specified ID was not found");
            else
                return Ok();
        }

        // DELETE api/<AuthorController>/5
        /// <summary>
        /// Deletes a author
        /// </summary>
        /// <param name="id">The ID of the author to delete</param>
        /// <response code="200">Success - Successfully deleted</response>
        /// <response code="404">NotFound - Author with the specified ID was not found</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            bool success = await _authorService.DeleteAsync(id);

            if (!success)
                return NotFound("Author with the specified ID was not found");
            else
                return Ok();
        }
    }
}