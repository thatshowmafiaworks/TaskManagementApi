namespace TaskManagementApi.Services;

public class TasksRepository(
    AppDbContext context
    )
{
    public async Task<Models.Task> Get(Guid id)
    {
        var task = await context.Tasks.FindAsync();
        return task;
    }

    public async Task<IEnumerable<Models.Task>> GetAll(
        int pageNumber = 1,
        int pageSize = 10,
        Status? status = null,
        Priority? priority = null)
    {
        IQueryable<Models.Task> tasks = context.Tasks;
        if (status != null)
            tasks = tasks.Where(x => x.Status == status);
        if (priority != null)
            tasks = tasks.Where(x => x.Priority == priority);
        var result = await (tasks.Skip((pageNumber - 1) * pageSize).Take(pageSize)).ToListAsync();
        return result;
    }

    public async Task<IEnumerable<Models.Task>> GetAllByUser(
        Guid userId,
        int pageNumber = 1,
        int pageSize = 10,
        Status? status = null,
        Priority? priority = null)
    {
        IQueryable<Models.Task> tasks = context.Tasks.Where(x => x.UserId == userId);
        if (status != null)
            tasks = tasks.Where(x => x.Status == status);
        if (priority != null)
            tasks = tasks.Where(x => x.Priority == priority);
        var result = await (tasks.Skip((pageNumber - 1) * pageSize).Take(pageSize)).ToListAsync();
        return result;
    }

    public async System.Threading.Tasks.Task Create(Models.Task task)
    {
        context.Tasks.Add(task);
        await context.SaveChangesAsync();
    }

    public async System.Threading.Tasks.Task Update(Models.Task task)
    {
        context.Tasks.Update(task);
        await context.SaveChangesAsync();
    }

    public async System.Threading.Tasks.Task Delete(Models.Task task)
    {
        context.Tasks.Remove(task);
        await context.SaveChangesAsync();
    }
}
