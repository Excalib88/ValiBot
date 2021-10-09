﻿using Microsoft.EntityFrameworkCore;
using ValiBot.Entities;

namespace ValiBot
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
        public DbSet<AppUser> Users { get; set; }
    }
}