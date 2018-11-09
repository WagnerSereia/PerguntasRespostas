using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PerguntasRespostas.Domain.Entities;
using PerguntasRespostas.Infra.Data.EntityConfig;
using PerguntasRespostas.Infra.Data.Extensions;
using System;
using System.IO;
using System.Linq;


namespace PerguntasRespostas.Infra.Data.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
            //this.Configuration.LazyLoadingEnabled = false;

            if (!Categoria.Any())
            {
                Categoria.Add(new Categoria("Banco de Dados", "Perguntas relacionados com banco de dados"));               
                Categoria.Add(new Categoria("Linguagem de programação", "Perguntas sobre as mais diversas linguagens de programação"));
                Categoria.Add(new Categoria("IoT", "Perguntas sobre sensores, automação e internet"));
                Categoria.Add(new Categoria("WEB", "Perguntas sobre desenvolvimento WEB"));
                
                SaveChanges();
            }
        }

        public DbSet<Pergunta> Pergunta { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Respostas> Respostas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddConfiguration(new PerguntasMap());
            modelBuilder.AddConfiguration(new CategoriasMap());
            modelBuilder.AddConfiguration(new RespostasMap());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }


        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }
            return base.SaveChanges();
        }

    }
}