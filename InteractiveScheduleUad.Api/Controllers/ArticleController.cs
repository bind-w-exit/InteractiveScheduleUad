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
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        // GET: api/<ArticleController>
        /// <summary>
        /// Retrieves all articles
        /// </summary>
        /// <response code="200">Success - Returns an array of articles</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ArticleForReadDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ArticleForReadDto>>> Get()
        {
            IEnumerable<ArticleForReadDto> articles = await _articleService.GetAllAsync();

            return Ok(articles);
        }

        // GET api/<ArticleController>/5
        /// <summary>
        /// Retrieves a article by its ID
        /// </summary>
        /// <param name="id">Article ID</param>
        /// <response code="200">Success - Returns the article with the specified ID</response>
        /// <response code="404">NotFound - Article with the specified ID was not found</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Article), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Article>> Get(int id)
        {
            Article? article = await _articleService.GetByIdAsync(id);

            if (article is null)
                return NotFound("Article with the specified ID was not found");
            else
                return Ok(article);
        }

        // POST api/<ArticleController>
        /// <summary>
        /// Creates a new article
        /// </summary>
        /// <param name="article">The new article</param>
        /// <response code="201">Created - Returns the created article</response>
        /// <response code="400">BadRequest - One or more validation errors occurred</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Article), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Article>> Post(ArticleForWriteDto article)
        {
            Article articleFromDb = await _articleService.CreateAsync(article);

            return CreatedAtAction(nameof(Get), new { id = articleFromDb.Id }, articleFromDb);
        }

        // PUT api/<ArticleController>/5
        /// <summary>
        /// Updates an existing article
        /// </summary>
        /// <param name="id">The ID of the article to update</param>
        /// <param name="article">The updated article data</param>
        /// <response code="200">Success - Successfully updated</response>
        /// <response code="400">BadRequest - One or more validation errors occurred</response>
        /// <response code="404">NotFound - Article with the specified ID was not found</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Put(int id, [FromBody] ArticleForWriteDto article)
        {
            bool success = await _articleService.UpdateAsync(id, article);

            if (!success)
                return NotFound("Article with the specified ID was not found");
            else
                return Ok();
        }

        // DELETE api/<ArticleController>/5
        /// <summary>
        /// Deletes a article
        /// </summary>
        /// <param name="id">The ID of the article to delete</param>
        /// <response code="200">Success - Successfully deleted</response>
        /// <response code="404">NotFound - Article with the specified ID was not found</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            bool success = await _articleService.DeleteAsync(id);

            if (!success)
                return NotFound("Article with the specified ID was not found");
            else
                return Ok();
        }
    }
}