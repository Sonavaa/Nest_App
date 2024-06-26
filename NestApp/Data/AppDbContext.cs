﻿using Microsoft.EntityFrameworkCore;
using NestApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
    }
}
