using FluentResults;
using InteractiveScheduleUad.Api.Errors;
using InteractiveScheduleUad.Api.Mappers;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using InteractiveScheduleUad.Api.Services.Contracts;

namespace InteractiveScheduleUad.Api.Services;

public class StudentsGroupService : IStudentsGroupService
{
    private readonly IStudentsGroupRepository _studentsGroupRepository;

    public StudentsGroupService(IStudentsGroupRepository studentsGroupRepository)
    {
        _studentsGroupRepository = studentsGroupRepository;
    }

    public async Task<Result<StudentsGroupForReadDto>> CreateAsync(string name)
    {
        StudentsGroup studentsGroup = new() { Name = name };

        await _studentsGroupRepository.InsertAsync(studentsGroup);
        await _studentsGroupRepository.SaveChangesAsync();

        var mappedStudentsGroup = StudentsGroupMapper.StudentsGroupToStudentsGroupForReadDto(studentsGroup);

        return mappedStudentsGroup;
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var studentsGroup = await _studentsGroupRepository.GetByIdAsync(id);

        if (studentsGroup is not null)
        {
            _studentsGroupRepository.Delete(studentsGroup);
            await _studentsGroupRepository.SaveChangesAsync();

            return Result.Ok();
        }
        else
            return new NotFoundError(nameof(StudentsGroup));
    }

    public async Task<Result<IEnumerable<StudentsGroupForReadDto>>> GetAllAsync()
    {
        var studentsGroups = await _studentsGroupRepository.GetAllAsync();
        var mappedStudentsGroups = studentsGroups.Select(StudentsGroupMapper.StudentsGroupToStudentsGroupForReadDto);

        return Result.Ok(mappedStudentsGroups);
    }

    public async Task<Result<StudentsGroupForReadDto>> GetByIdAsync(int id)
    {
        var studentsGroup = await _studentsGroupRepository.GetByIdAsync(id);

        if (studentsGroup is not null)
        {
            var mappedStudentsGroup = StudentsGroupMapper.StudentsGroupToStudentsGroupForReadDto(studentsGroup);

            return mappedStudentsGroup;
        }
        else
            return new NotFoundError(nameof(StudentsGroup));
    }

    public async Task<Result> UpdateAsync(int id, string newName)
    {
        var studentsGroup = await _studentsGroupRepository.GetByIdAsync(id);

        if (studentsGroup is not null)
        {
            studentsGroup.Name = newName;

            _studentsGroupRepository.Update(studentsGroup);
            await _studentsGroupRepository.SaveChangesAsync();

            return Result.Ok();
        }
        else
            return new NotFoundError(nameof(StudentsGroup));
    }
}