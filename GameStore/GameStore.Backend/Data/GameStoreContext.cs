using System;
using GameStore.Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Backend.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
    }
}
