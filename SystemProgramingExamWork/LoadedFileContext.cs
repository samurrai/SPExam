namespace SystemProgramingExamWork
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class LoadedFileContext : DbContext
    {
        public LoadedFileContext()
            : base("name=LoadedFileContext")
        {
        }
        public DbSet<File> Files { get; set; }
    }
}