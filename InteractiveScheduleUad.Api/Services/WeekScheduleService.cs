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

    public async Task<WeekScheduleForReadDto?> CreateAsync(int studentsGroupId, WeekScheduleForWriteDto weekScheduleForWriteDto, bool isSecondWeek)
    {
        var studentsGroup = await _studentsGroupRepository.GetByIdAsync(studentsGroupId);

        if (studentsGroup is null)
            return null;

        WeekSchedule weekSchedule = await WeekScheduleForReadDtoToWeekSchedule(weekScheduleForWriteDto);

        if (isSecondWeek)
            studentsGroup.SecondWeekSchedule = weekSchedule;
        else
            studentsGroup.FirstWeekSchedule = weekSchedule;

        await _studentsGroupRepository.SaveChangesAsync();

        return WeekScheduleMapper.WeekScheduleToWeekScheduleForReadDto(weekSchedule);
    }

    public async Task<bool> DeleteAsync(int studentsGroupId, bool isSecondWeek)
    {
        var studentsGroup = await _studentsGroupRepository.GetByIdAsync(studentsGroupId);

        if (studentsGroup is null)
            return false;

        if (isSecondWeek)
            studentsGroup.SecondWeekSchedule = null;
        else
            studentsGroup.FirstWeekSchedule = null;

        await _studentsGroupRepository.SaveChangesAsync();

        return true;
    }

    public async Task<StudentsGroupForWriteDto?> GetByIdAsync(int studentsGroupId)
    {
        var studentsGroup = await _studentsGroupRepository.GetByIdAsync(studentsGroupId);

        return StudentsGroupMapper.StudentsGroupToStudentsGroupForWriteDto(studentsGroup);
    }

    private async Task<WeekSchedule> WeekScheduleForReadDtoToWeekSchedule(WeekScheduleForWriteDto weekScheduleForWriteDto)
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
            Sunday = await sundayTask,
            Monday = await mondayTask,
            Tuesday = await tuesdayTask,
            Wednesday = await wednesdayTask,
            Thursday = await thursdayTask,
            Friday = await fridayTask,
            Saturday = await saturdayTask
        };
    }

    private async Task<Lesson> LessonForWriteDtoToLesson(LessonForWriteDto lessonForWriteDto)
    {
        var teacherTask = _teacherRepository.GetByIdAsync(lessonForWriteDto.TeacherId);
        var subjectTask = _subjectRepository.GetByIdAsync(lessonForWriteDto.SubjectId);
        var roomTask = _roomRepository.GetByIdAsync(lessonForWriteDto.RoomId);

        await Task.WhenAll(teacherTask, subjectTask, roomTask);

        var teacher = await teacherTask;
        var subject = await subjectTask;
        var room = await roomTask;

        return new Lesson()
        {
            Subject = subject,
            Teacher = teacher,
            Room = room,
            ClassType = lessonForWriteDto.ClassType,
            Sequence = lessonForWriteDto.Sequence
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
}