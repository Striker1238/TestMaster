using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestMaster.Models;

namespace TestMaster.Services
{
    class DatabaseConnectionService : DbContext
    {
        private readonly string connectionString = "Host=localhost;Port=5432;Database=TestMaster;Username=postgres;Password=postgres";
        public DbSet<Question> questions => Set<Question>();

        public DatabaseConnectionService(string ConnectionString = "")
        {
            if (!string.IsNullOrWhiteSpace(ConnectionString))
                this.connectionString = ConnectionString;

            if (Database.EnsureCreated())
            {
                // Если база данных была создана, добавляем начальные данные
                if (!questions.Any())
                {
                    questions.AddRange(new List<Question>
                    {
                        new Question
                        {
                            Text = "Кто определяет требования к составу комплекта и содержанию документов, обосновывающих обеспечение ядерной и радиационной безопасности лицензируемых видов деятельности и объектов использования атомной энергии в оборонных целях?",
                            Answers = new List<Answer>
                            {
                                new Answer { Text = "Госкорпорация «Росатом»." },
                                new Answer { Text = "Ростехнадзор." },
                                new Answer { Text = "Министерство энергетики" }
                            },
                            CorrectAnswerIndexes = new List<int> { 0 }
                        },
                        new Question
                        {
                            Text = "В течение какого срока должны храниться результаты индивидуального дозиметрического контроля?",
                            Answers = new List<Answer>
                            {
                                new Answer { Text = "Не менее 30 лет." },
                                new Answer { Text = "Не менее 20 лет." },
                                new Answer { Text = "Не менее 50 лет" }
                            },
                            CorrectAnswerIndexes = new List<int> { 2 }
                        },
                        new Question
                        {
                            Text = "Кто, согласно Федеральному закону «Об использовании атомной энергии», несет ответственность за убытки и вред, причиненные радиационным воздействием?",
                            Answers = new List<Answer>
                            {
                                new Answer { Text = "Правительство Российской Федерации." },
                                new Answer { Text = "Госкорпорация «Росатом»." },
                                new Answer { Text = "Эксплуатирующая организация." }
                            },
                            CorrectAnswerIndexes = new List<int> { 2 }
                        },
                        new Question
                        {
                            Text = "Какие значения основных пределов доз установлены для персонала (группа А)?",
                            Answers = new List<Answer>
                            {
                                new Answer { Text = "1 мЗв в год за любые последние 5 лет." },
                                new Answer { Text = "5 мЗв в год в среднем за любые 5 лет." },
                                new Answer { Text = "20 мЗв в год за любые последовательные 5 лет, но не более 50 мЗв в год" }
                            },
                            CorrectAnswerIndexes = new List<int> { 2 }
                        },
                        new Question
                        {
                            Text = "Какой документ необходим для обеспечения радиационной безопасности при транспортировании радиоактивных материалов?",
                            Answers = new List<Answer>
                            {
                                new Answer { Text = "Накладная." },
                                new Answer { Text = "Таможенная декларация." },
                                new Answer { Text = "Программа радиационной защиты" }
                            },
                            CorrectAnswerIndexes = new List<int> { 2 }
                        },
                        new Question
                        {
                            Text = "Какие радиационные объекты относятся к 1 категории по потенциальной радиационной опасности?",
                            Answers = new List<Answer>
                            {
                                new Answer { Text = "Объекты, где исключена возможность облучения лиц, не относящихся к персоналу." },
                                new Answer { Text = "Объекты, на которых радиационное воздействие при аварии ограничивается территорией объекта." },
                                new Answer { Text = "Объекты, при аварии на которых возможно их радиационное воздействие на население, и могут потребоваться меры по его защите" }
                            },
                            CorrectAnswerIndexes = new List<int> { 2 }
                        },
                        new Question
                        {
                            Text = "Какие основные принципы обеспечивают радиационную безопасность при нормальной эксплуатации источников излучения?",
                            Answers = new List<Answer>
                            {
                                new Answer { Text = "Защита здоровья людей." },
                                new Answer { Text = "Предотвращение аварий с радиационными последствиями." },
                                new Answer { Text = "Нормирование, обоснование, оптимизация" }
                            },
                            CorrectAnswerIndexes = new List<int> { 2 }
                        }
                    });
                    SaveChanges();
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
               .HasMany(q => q.Answers)
               .WithOne(a => a.Question)
               .HasForeignKey(a => a.QuestionId)
               .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Question>()
                .Property(q => q.CorrectAnswerIndexes)
                .HasColumnType("integer[]");

            

        }

    }
}
