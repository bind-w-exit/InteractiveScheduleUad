using FluentResults;
using InteractiveScheduleUad.Api.Errors;
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

    public async Task<Result<Subject>> CreateAsync(Subject subject)
    {
        await _subjectRepository.InsertAsync(subject);
        await _subjectRepository.SaveChangesAsync();

        return subject;
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var subject = await _subjectRepository.GetByIdAsync(id);

        if (subject is not null)
        {
            _subjectRepository.Delete(subject);
            await _subjectRepository.SaveChangesAsync();

            return Result.Ok();
        }
        else
            return new NotFoundError(nameof(Subject));
    }

    public async Task<Result<IEnumerable<Subject>>> GetAllAsync()
    {
        var subjects = await _subjectRepository.GetAllAsync();

        return Result.Ok(subjects);
    }

    public async Task<Result<Subject>> GetByIdAsync(int id)
    {
        var subject = await _subjectRepository.GetByIdAsync(id);

        if (subject is not null)
            return subject;
        else
            return new NotFoundError(nameof(Subject));
    }

    public async Task<Result> UpdateAsync(int id, string newName)
    {
        var subject = await _subjectRepository.GetByIdAsync(id);

        if (subject is not null)
        {
            subject.Name = newName;

            _subjectRepository.Update(subject);
            await _subjectRepository.SaveChangesAsync();

            return Result.Ok();
        }
        else
            return new NotFoundError(nameof(Subject));
    }
}