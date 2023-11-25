﻿using InteractiveScheduleUad.Api.Mappers;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Net;

namespace InteractiveScheduleUad.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LessonController : ControllerBase
{
    private readonly InteractiveScheduleUadApiDbContext _context;

    public LessonController(InteractiveScheduleUadApiDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all lessons.
    /// </summary>
    /// <response code="200">Success - Returns all lessons</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LessonForReadDto[]), (int)HttpStatusCode.OK)]
    public ActionResult<IEnumerable<LessonForReadDto>> GetAll()
    {
        var lessons = _context.Lessons;
        var lessonsList = lessons.ToList();

        var lessonsForRead = lessonsList.Select(LessonMapper.LessonToLessonForReadDto);

        var result = new ActionResult<IEnumerable<LessonForReadDto>>(lessonsForRead);

        return result;
    }

    /// <summary>
    /// Retrieves a lesson by Id.
    /// </summary>
    /// <response code="200">Success - Returns a lesson</response>
    [HttpGet("{lessonId}")]
    [ProducesResponseType(typeof(LessonForReadDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public ActionResult<LessonForReadDto> Get(int lessonId)
    {
        var lesson = _context.Lessons
            .Where(l => l.Id == lessonId)
            .FirstOrDefault();

        if (lesson == null)
        {
            return NotFound();
        }

        var lessonForRead = LessonMapper.LessonToLessonForReadDto(lesson);

        var result = new ActionResult<LessonForReadDto>(lessonForRead);

        return result;
    }

    /// <summary>
    /// Creates a new lesson.
    /// </summary>
    /// <response code="200">Returns the newly created lesson</response>
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(LessonForReadDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<LessonForReadDto>> Post([FromBody] LessonForWriteDto lessonForWrite)
    {

        // create proxy for lazy loading to work right after creating the lesson
        var newLesson = _context.Lessons.CreateProxy((newLesson) =>
        {
            newLesson.ClassType = lessonForWrite.ClassType;
            newLesson.RoomId = lessonForWrite.RoomId;
            newLesson.SubjectId = lessonForWrite.SubjectId;
            newLesson.TeacherId = lessonForWrite.TeacherId;
        });

        _context.Lessons.Add(newLesson);
        await _context.SaveChangesAsync();

        LessonForReadDto lessonForRead = LessonMapper.LessonToLessonForReadDto(newLesson);

        return lessonForRead;
    }

    /// <summary>
    /// Updates a lesson.
    /// </summary>
    [HttpPut("{Id}")]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(LessonForReadDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<LessonForReadDto>> Update(int Id, [FromBody] LessonForWriteDto lessonForWrite)
    {
        // validate the id
        
        var lesson = _context.Lessons.AsNoTracking().FirstOrDefault(l => l.Id == Id);
        if (lesson == null)
        {
            return NotFound();
        }

        var newLesson = CreateLesson(lessonForWrite);
        newLesson.Id = Id;

        _context.Lessons.Update(newLesson);
        await _context.SaveChangesAsync();

        LessonForReadDto lessonForRead = LessonMapper.LessonToLessonForReadDto(newLesson);

        return lessonForRead;
    }

    private Lesson CreateLesson(LessonForWriteDto lessonForWrite)
    {

        // create proxy for lazy loading to work right after creating the lesson

        var newLesson = _context.Lessons.CreateProxy((newLesson) =>
        {
            newLesson.ClassType = lessonForWrite.ClassType;
            newLesson.RoomId = lessonForWrite.RoomId;
            newLesson.SubjectId = lessonForWrite.SubjectId;
            newLesson.TeacherId = lessonForWrite.TeacherId;
        });

        return newLesson;
    }

    /// <summary>
    /// Deletes a lesson.
    /// </summary>
    /// <response code="200"></response>
    [HttpDelete("{lessonId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Delete(int lessonId)
    {
        var lesson = _context.Lessons.Find(lessonId);

        if (lesson == null)
        {
            return NotFound();
        }

        _context.Lessons.Remove(lesson);
        await _context.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Deletes all lessons.
    /// </summary>
    /// <response code="200"></response>
    [HttpDelete()]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> DeleteAll()
    {
        await _context.Lessons.ExecuteDeleteAsync();
        await _context.SaveChangesAsync();

        return Ok();
    }
}