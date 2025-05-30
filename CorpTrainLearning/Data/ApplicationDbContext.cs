using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CorpTrainLearning.Models;

namespace CorpTrainLearning.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

public DbSet<CorpTrainLearning.Models.User> User { get; set; } = default!;

public DbSet<CorpTrainLearning.Models.Role> Role { get; set; } = default!;

public DbSet<CorpTrainLearning.Models.Course> Course { get; set; } = default!;

public DbSet<CorpTrainLearning.Models.Admin> Admin { get; set; } = default!;

public DbSet<CorpTrainLearning.Models.Module> Module { get; set; } = default!;

public DbSet<CorpTrainLearning.Models.Quiz> Quiz { get; set; } = default!;

public DbSet<CorpTrainLearning.Models.Question> Question { get; set; } = default!;

public DbSet<CorpTrainLearning.Models.UserRole> UserRole { get; set; } = default!;

public DbSet<CorpTrainLearning.Models.UserCourse> UserCourse { get; set; } = default!;

public DbSet<CorpTrainLearning.Models.QuizAttempt> QuizAttempt { get; set; } = default!;
}