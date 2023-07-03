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

    public async Task<StudentsGroup> CreateAsync(string name)
    {
        StudentsGroup studentsGroup = new() { GroupName = name };

        await _studentsGroupRepository.InsertAsync(studentsGroup);
        await _studentsGroupRepository.SaveChangesAsync();

        return studentsGroup;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var department = await _studentsGroupRepository.GetByIdAsync(id);
        if (department is not null)
        {
            _studentsGroupRepository.Delete(department);
            await _studentsGroupRepository.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<StudentsGroupForReadDto>> GetAllAsync()
    {
        var studentsGroups = await _studentsGroupRepository.GetAllAsync();

        List<StudentsGroupForReadDto> studentsGroupsForListDto = new();
        foreach (var studentsGroup in studentsGroups)
        {
            studentsGroupsForListDto.Add(StudentsGroupMapper.StudentsGroupToStudentsGroupForReadDto(studentsGroup));
        }

        return studentsGroupsForListDto;
    }

    public async Task<StudentsGroupForReadDto?> GetByIdAsync(int id)
    {
        var studentsGroup = await _studentsGroupRepository.GetByIdAsync(id);

        if (studentsGroup is not null)
            return StudentsGroupMapper.StudentsGroupToStudentsGroupForReadDto(studentsGroup);

        return null;
    }

    public async Task<bool> UpdateAsync(int id, string newName)
    {
        var studentsGroupFromDb = await _studentsGroupRepository.GetByIdAsync(id);
        if (studentsGroupFromDb is not null)
        {
            studentsGroupFromDb.GroupName = newName;

            _studentsGroupRepository.Update(studentsGroupFromDb);
            await _studentsGroupRepository.SaveChangesAsync();
            return true;
        }
        return false;
    }
}