using InteractiveScheduleUad.Api.Models;

namespace InteractiveScheduleUad.Api.Services;

public class CourseService : Contracts.ICourseService
{
    private readonly Repositories.Contracts.ICourseRepository _courseRepository;

    public CourseService(Repositories.Contracts.ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<Course> CreateAsync(string name)
    {
        Course course = new() { Name = name };

        await _courseRepository.InsertAsync(course);
        await _courseRepository.SaveChangesAsync();

        return course;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course is not null)
        {
            _courseRepository.Delete(course);
            await _courseRepository.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _courseRepository.GetAllAsync();
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await _courseRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(int id, string newName)
    {
        var courseFromDb = await _courseRepository.GetByIdAsync(id);
        if (courseFromDb is not null)
        {
            courseFromDb.Name = newName;

            _courseRepository.Update(courseFromDb);
            await _courseRepository.SaveChangesAsync();
            return true;
        }
        return false;
    }
}