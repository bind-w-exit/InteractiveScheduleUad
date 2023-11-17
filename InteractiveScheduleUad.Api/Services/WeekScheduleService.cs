using FluentResults;
using InteractiveScheduleUad.Api.Errors;
using InteractiveScheduleUad.Api.Mappers;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using InteractiveScheduleUad.Api.Services.Contracts;

namespace InteractiveScheduleUad.Api.Services;

public class WeekScheduleService : IWeekScheduleService
{
    private readonly IStudentsGroupRepository _studentsGroupRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly IRoomRepository _roomRepository;

    public WeekScheduleService(
        IStudentsGroupRepository studentsGroupRepository,
        ITeacherRepository teacherRepository,
        ISubjectRepository subjectRepository,
        IRoomRepository roomRepository)
    {
        _studentsGroupRepository = studentsGroupRepository;
        _teacherRepository = teacherRepository;
        _subjectRepository = subjectRepository;
        _roomRepository = roomRepository;
    }

    public async Task<Result<WeekScheduleForReadDto>> CreateAsync(int studentsGroupId, WeekScheduleForWriteDto weekScheduleForWriteDto, bool isSecondWeek)
    {
        var studentsGroup = await _studentsGroupRepository.GetByIdAsync(studentsGroupId);

        if (studentsGroup is null)
            return new NotFoundError(nameof(StudentsGroup));

        WeekSchedule weekSchedule = await WeekScheduleForWriteDtoToWeekSchedule(weekScheduleForWriteDto);

        if (isSecondWeek)
            studentsGroup.SecondWeekSchedule = weekSchedule;
        else
            studentsGroup.FirstWeekSchedule = weekSchedule;

        _studentsGroupRepository.Update(studentsGroup);
        await _studentsGroupRepository.SaveChangesAsync();

        var weekScheduleForRead = WeekScheduleMapper.WeekScheduleToWeekScheduleForReadDto(weekSchedule);

        return weekScheduleForRead;
    }

    public async Task<Result> DeleteAsync(int studentsGroupId, bool isSecondWeek)
    {
        var studentsGroup = await _studentsGroupRepository.GetByIdAsync(studentsGroupId);

        if (studentsGroup is null)
            return new NotFoundError(nameof(StudentsGroup));

        if (isSecondWeek)
            studentsGroup.SecondWeekSchedule = null;
        else
            studentsGroup.FirstWeekSchedule = null;

        await _studentsGroupRepository.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result<StudentsGroupForWriteDto>> GetByIdAsync(int studentsGroupId)
    {
        var studentsGroup = await _studentsGroupRepository.GetByIdAsync(studentsGroupId);

        if (studentsGroup is null)
            return new NotFoundError(nameof(StudentsGroup));

        var studentsGroupForWrite = StudentsGroupMapper.StudentsGroupToStudentsGroupForWriteDto(studentsGroup);

        return studentsGroupForWrite;
    }

    private async Task<WeekSchedule> WeekScheduleForWriteDtoToWeekSchedule(WeekScheduleForWriteDto weekScheduleForWriteDto)
    {
        var sundayTask = LessonsForWriteDtoToLessons(weekScheduleForWriteDto.Sunday);
        var mondayTask = LessonsForWriteDtoToLessons(weekScheduleForWriteDto.Monday);
        var tuesdayTask = LessonsForWriteDtoToLessons(weekScheduleForWriteDto.Tuesday);
        var wednesdayTask = LessonsForWriteDtoToLessons(weekScheduleForWriteDto.Wednesday);
        var thursdayTask = LessonsForWriteDtoToLessons(weekScheduleForWriteDto.Thursday);
        var fridayTask = LessonsForWriteDtoToLessons(weekScheduleForWriteDto.Friday);
        var saturdayTask = LessonsForWriteDtoToLessons(weekScheduleForWriteDto.Saturday);

        await Task.WhenAll(sundayTask, mondayTask, tuesdayTask, wednesdayTask, thursdayTask, fridayTask, saturdayTask);

        return new WeekSchedule()
        {
            Sunday = sundayTask.Result,
            Monday = mondayTask.Result,
            Tuesday = tuesdayTask.Result,
            Wednesday = wednesdayTask.Result,
            Thursday = thursdayTask.Result,
            Friday = fridayTask.Result,
            Saturday = saturdayTask.Result
        };
    }

    private async Task<IEnumerable<Lesson>?> LessonsForWriteDtoToLessons(IEnumerable<LessonForWriteDto>? lessonsForWriteDto)
    {
        if (lessonsForWriteDto is null)
            return null;

        var tasks = lessonsForWriteDto.Select(LessonForWriteDtoToLesson);
        var lessons = await Task.WhenAll(tasks);

        return lessons;
    }

    private async Task<Lesson> LessonForWriteDtoToLesson(LessonForWriteDto lessonForWriteDto)
    {
        var teacherTask = _teacherRepository.GetByIdAsync(lessonForWriteDto.TeacherId);
        var subjectTask = _subjectRepository.GetByIdAsync(lessonForWriteDto.SubjectId);
        var roomTask = _roomRepository.GetByIdAsync(lessonForWriteDto.RoomId);

        await Task.WhenAll(teacherTask, subjectTask, roomTask);

        return new Lesson()
        {
            Subject = subjectTask.Result,
            Teacher = teacherTask.Result,
            Room = roomTask.Result,
            ClassType = lessonForWriteDto.ClassType,
            Sequence = lessonForWriteDto.Sequence
        };
    }
}