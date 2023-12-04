using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.E2ETests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveScheduleUad.E2ETests.UserActions;

// TODO: implement
public static class ScheduleActions
{
    // POSTS a complete lesson with related entities pre-created in advance
    //private LessonForReadDto CreateCompleteLesson(ScheduleClass rawScheduleClass)
    //{
    //    LessonForWriteDto requestBody;
    //    LessonForReadDto responseData;

    //    // pre-create necessary related entities (if they don't exist already)
    //    TeacherForWriteDto teacherForWrite = new() { LastName = rawScheduleClass.teacher, FirstName = "" };
    //    var teacher = client.EnsureExists<TeacherForReadDto, TeacherForWriteDto>(
    //        teachersEndpoint, null, teacherForWrite, (t) => t.LastName == rawScheduleClass.teacher);
    //    var teacherId = teacher.Id;

    //    var subject = client.EnsureExists<Subject, string>(
    //        subjectsEndpoint, null, Utls.EncaseInQuotes(rawScheduleClass.name), (s) => s.Name == rawScheduleClass.name);
    //    var subjectId = subject.Id;

    //    var roomForWrite = new RoomForWriteDto { Name = rawScheduleClass.room };
    //    var room = client.EnsureExists<Room, RoomForWriteDto>(
    //        roomsEndpoint, null, roomForWrite, (r) => r.Name == rawScheduleClass.room);
    //    var roomId = room.Id;

    //    // assemble final request
    //    requestBody = new()
    //    {
    //        RoomId = roomId,
    //        SubjectId = subjectId,
    //        TeacherId = teacherId
    //    };

    //    // POST the lesson with entities
    //    responseData = client.PostJson<LessonForWriteDto, LessonForReadDto>(lessonsEndpoint, requestBody);

    //    return responseData;
    //}
}