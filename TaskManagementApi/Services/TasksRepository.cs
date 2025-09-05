namespace TaskManagementApi.Services;

public class TasksRepository(
    AppDbContext context
    )
{
    public async System.Threading.Tasks.Task Create(Models.Task task)
    {
        context.Tasks.Add(task);
        await context.SaveChangesAsync();
    }

    public System.Threading.Tasks.Task Delete(Models.Task task)
    {
        throw new NotImplementedException();
    }

    public Task<Models.Task> Get(Guid Id, int pageNumber = 1, int pageSize = 10, int status = 0, int priority = 0)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Models.Task>> GetAll(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Models.Task>> GetAllByUser(AppUser user, int pageNumber = 1, int pageSize = 10, int status = 0, int priority = 0)
    {
        throw new NotImplementedException();
    }

    public System.Threading.Tasks.Task Update(Models.Task task)
    {
        throw new NotImplementedException();
    }
}
