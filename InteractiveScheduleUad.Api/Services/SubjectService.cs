using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Repositories.Contracts;
using InteractiveScheduleUad.Api.Services.Contracts;

namespace InteractiveScheduleUad.Api.Services;

public class SubjectService : ISubjectService
{
    private readonly ISubjectRepository _subjectRepository;

    public SubjectService(ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public async Task<Subject> CreateAsync(string name)
    {
        Subject subject = new() { Name = name };

        await _subjectRepository.InsertAsync(subject);
        await _subjectRepository.SaveChangesAsync();

        return subject;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var subject = await _subjectRepository.GetByIdAsync(id);
        if (subject is not null)
        {
            _subjectRepository.Delete(subject);
            await _subjectRepository.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<Subject>> GetAllAsync()
    {
        return await _subjectRepository.GetAllAsync();
    }

    public async Task<Subject?> GetByIdAsync(int id)
    {
        return await _subjectRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(int id, string newName)
    {
        var subjectFromDb = await _subjectRepository.GetByIdAsync(id);
        if (subjectFromDb is not null)
        {
            subjectFromDb.Name = newName;

            _subjectRepository.Update(subjectFromDb);
            await _subjectRepository.SaveChangesAsync();
            return true;
        }
        return false;
    }
}