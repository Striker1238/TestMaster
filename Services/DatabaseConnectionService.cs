﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestMaster.Models.DB;

namespace TestMaster.Services
{
    class DatabaseConnectionService : DbContext
    {
        private readonly string connectionString = "Host=localhost;Port=5432;Database=TestMaster;Username=postgres;Password=postgres";
        public DbSet<TestDB> tests => Set<TestDB>();
        public DbSet<QuestionDB> questions => Set<QuestionDB>();
        public DbSet<AnswerDB> answers => Set<AnswerDB>();
        public DbSet<IndividualTestsDB> individualtests => Set<IndividualTestsDB>();

        public DatabaseConnectionService()
        {
            connectionString = SettingsService.GetString("Database:ConnectionString");

            if (Database.EnsureCreated())
            {
                if (!tests.Any())
                {
                    SaveChanges();
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql(connectionString);
            optionsBuilder.UseSqlite(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestionDB>(entity =>
            {
                entity.ToTable("question");

                entity.HasKey(q => q.Id);

                entity.Property(q => q.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(q => q.TestId)
                    .IsRequired();

                entity.Property(q => q.Text)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(q => q.CorrectAnswerIndexes)
                    .HasColumnType("integer[]");

                entity.Property(q => q.Type)
                    .IsRequired();

                entity.HasMany(q => q.Answers)
                    .WithOne(a => a.Question)
                    .HasForeignKey(a => a.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(q => q.Test)
                    .WithMany(t => t.Questions)
                    .HasForeignKey(q => q.TestId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AnswerDB>(entity =>
            {
                entity.ToTable("answer");

                entity.HasKey(a => a.Id);

                entity.Property(a => a.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(a => a.QuestionId)
                    .IsRequired();

                entity.Property(a => a.Text)
                    .IsRequired()
                    .HasMaxLength(1000);
            });

            modelBuilder.Entity<TestDB>(entity =>
            {
                entity.ToTable("tests");

                entity.HasKey(t => t.Id);

                entity.Property(t => t.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(t => t.Description)
                    .HasMaxLength(1000);

                entity.Property(t => t.Category)
                    .HasMaxLength(200);

                entity.HasMany(t => t.Questions)
                    .WithOne(q => q.Test)
                    .HasForeignKey(q => q.TestId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(t => t.NumberQuestions)
                    .IsRequired();

                entity.Property(t => t.IsShuffleQuestions)
                    .IsRequired();

                entity.Property(t => t.IsShuffleAnswers)
                    .IsRequired();

                entity.Property(t => t.CorrectAnswersCount)
                    .IsRequired();
            });

            modelBuilder.Entity<IndividualTestsDB>(entity =>
            {
                entity.ToTable("individualtests");

                entity.HasKey(i => i.Id);

                entity.Property(i => i.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(i => i.UserName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(i => i.PersonnelNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(i => i.CountQuestions)
                    .IsRequired();

                entity.Property(i => i.Questions)
                    .IsRequired();

                entity.Property(i => i.TestId)
                    .HasColumnType("integer[]");

                entity.HasOne(i => i.Test)
                    .WithMany()
                    .HasForeignKey(i => i.TestId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}

/*
tests.Add(new TestDB
                    {
                        Title = "Тест по ЯРБ",
                        Description = "Тут у нас описание какое то",
                        Category = "ЯРБ",
                        Questions = new List<QuestionDB>
                        {
                            new QuestionDB
                            {
                                Text = "Кто определяет требования к составу комплекта и содержанию документов, обосновывающих обеспечение ядерной и радиационной безопасности лицензируемых видов деятельности и объектов использования атомной энергии в оборонных целях?",
                                Answers = new List<AnswerDB>
                                {
                                    new AnswerDB { Text = "Госкорпорация «Росатом»." },
                                    new AnswerDB { Text = "Ростехнадзор." },
                                    new AnswerDB { Text = "Министерство энергетики" }
                                },
                                CorrectAnswerIndexes = new List<int> { 0 }
                            },
                            new QuestionDB
                            {
                                Text = "В течение какого срока должны храниться результаты индивидуального дозиметрического контроля?",
                                Answers = new List<AnswerDB>
                                {
                                    new AnswerDB { Text = "Не менее 30 лет." },
                                    new AnswerDB { Text = "Не менее 20 лет." },
                                    new AnswerDB { Text = "Не менее 50 лет" }
                                },
                                CorrectAnswerIndexes = new List<int> { 2 }
                            },
                            new QuestionDB
                            {
                                Text = "Кто, согласно Федеральному закону «Об использовании атомной энергии», несет ответственность за убытки и вред, причиненные радиационным воздействием?",
                                Answers = new List<AnswerDB>
                                {
                                    new AnswerDB { Text = "Правительство Российской Федерации." },
                                    new AnswerDB { Text = "Госкорпорация «Росатом»." },
                                    new AnswerDB { Text = "Эксплуатирующая организация." }
                                },
                                CorrectAnswerIndexes = new List<int> { 2 }
                            },
                            new QuestionDB
                            {
                                Text = "Какие значения основных пределов доз установлены для персонала (группа А)?",
                                Answers = new List<AnswerDB>
                                {
                                    new AnswerDB { Text = "1 мЗв в год за любые последние 5 лет." },
                                    new AnswerDB { Text = "5 мЗв в год в среднем за любые 5 лет." },
                                    new AnswerDB { Text = "20 мЗв в год за любые последовательные 5 лет, но не более 50 мЗв в год" }
                                },
                                CorrectAnswerIndexes = new List<int> { 2 }
                            },
                            new QuestionDB
                            {
                                Text = "Какой документ необходим для обеспечения радиационной безопасности при транспортировании радиоактивных материалов?",
                                Answers = new List<AnswerDB>
                                {
                                    new AnswerDB { Text = "Накладная." },
                                    new AnswerDB { Text = "Таможенная декларация." },
                                    new AnswerDB { Text = "Программа радиационной защиты" }
                                },
                                CorrectAnswerIndexes = new List<int> { 2 }
                            },
                            new QuestionDB
                            {
                                Text = "Какие радиационные объекты относятся к 1 категории по потенциальной радиационной опасности?",
                                Answers = new List<AnswerDB>
                                {
                                    new AnswerDB { Text = "Объекты, где исключена возможность облучения лиц, не относящихся к персоналу." },
                                    new AnswerDB { Text = "Объекты, на которых радиационное воздействие при аварии ограничивается территорией объекта." },
                                    new AnswerDB { Text = "Объекты, при аварии на которых возможно их радиационное воздействие на население, и могут потребоваться меры по его защите" }
                                },
                                CorrectAnswerIndexes = new List<int> { 2 }
                            },
                            new QuestionDB
                            {
                                Text = "Какие основные принципы обеспечивают радиационную безопасность при нормальной эксплуатации источников излучения?",
                                Answers = new List<AnswerDB>
                                {
                                    new AnswerDB { Text = "Защита здоровья людей." },
                                    new AnswerDB { Text = "Предотвращение аварий с радиационными последствиями." },
                                    new AnswerDB { Text = "Нормирование, обоснование, оптимизация" }
                                },
                                CorrectAnswerIndexes = new List<int> { 2 }
                            }
                        }
                    });
 */
