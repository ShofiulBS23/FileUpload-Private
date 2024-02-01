﻿using FilePrivate.DbModels;
using Microsoft.EntityFrameworkCore;

namespace FilePrivate.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocGroupLU> DocGroups { get; set; }
    }
}
