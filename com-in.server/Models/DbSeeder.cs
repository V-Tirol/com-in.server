namespace com_in.server.Models
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ForumContext context)
        {

            if (context.Articles.Any() || context.Categories.Any() || context.Media.Any() || context.MediaType.Any())
            {
                return;
            }

            using var transaction = context.Database.BeginTransaction();
            try
            {
                var categories = new List<Category>
                {
                    new Category { CategoryName = "ORGANIZATION"},
                    new Category { CategoryName = "FACULTY"},
                    new Category { CategoryName = "BSIT"},
                    new Category { CategoryName = "BSED - ENGLISH"},
                    new Category { CategoryName = "BSED - MATH"},
                    new Category { CategoryName = "BSCRIM"},
                    new Category { CategoryName = "BSHM"}
                };
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();

                var types = new List<MediaType>
                {
                    new MediaType { Name = "ARTICLE"},
                    new MediaType { Name = "VIDEO"},
                    new MediaType { Name = "AUDIO"}
                };
                context.MediaType.AddRange(types);
                await context.SaveChangesAsync();

                var articles = new List<Article>
                {
                    new Article { Author = "Engr. John Cruz", Title = "DEVELOPING A WEB-BASED VOTING SYSTEM FOR BSIT STUDENTS", Date = new DateOnly(2024,3,12), CategoryId = categories.Single(c => c.CategoryName == "BSIT").Id },
                    new Article { Author = "Prof. Maria Dela Cruz", Title = "THE IMPACT OF SOCIAL MEDIA ON STUDENTS' WRITING SKILLS", Date = new DateOnly(2024,2,18), CategoryId = categories.Single(c => c.CategoryName == "BSED - ENGLISH").Id },
                    new Article { Author = "Dr. Jose Ramos", Title = "MATHEMATICAL MODELING IN REAL-LIFE SCENARIOS: A CASE STUDY", Date = new DateOnly(2023,11,5), CategoryId = categories.Single(c => c.CategoryName == "BSED - MATH").Id },
                    new Article { Author = "Col. Ana Santiago", Title = "A STUDY ON CRIME RATE REDUCTION STRATEGIES IN URBAN AREAS", Date = new DateOnly(2023,10,1), CategoryId = categories.Single(c => c.CategoryName == "BSCRIM").Id },
                    new Article { Author = "Chef Antonio Gomez", Title = "SUSTAINABLE PRACTICES IN MODERN HOSPITALITY BUSINESSES", Date = new DateOnly(2023,9,15), CategoryId = categories.Single(c => c.CategoryName == "BSHM").Id }
                };
                context.Articles.AddRange(articles);
                await context.SaveChangesAsync();

                var media = new List<Media>
                {
                    new Media { Title = "HOW TO BUILD A SIMPLE MOBILE APP (TUTORIAL)", TypeId = types.Single(c => c.Name == "VIDEO").Id, Duration = "55 min", Views = "34K", CategoryId = categories.Single(c => c.CategoryName == "BSIT").Id },
                    new Media { Title = "EFFECTIVE CLASSROOM ENGLISH STRATEGIES", TypeId = types.Single(c => c.Name == "AUDIO").Id, Duration = "25 min", Views = "18K", CategoryId = categories.Single(c => c.CategoryName == "BSED - ENGLISH").Id },
                    new Media { Title = "MATH PUZZLES AND LOGIC GAMES FOR LEARNING", TypeId = types.Single(c => c.Name == "VIDEO").Id, Duration = "40 min", Views = "29k", CategoryId = categories.Single(c => c.CategoryName == "BSED - MATH").Id },
                    new Media { Title = "THE ROLE OF FORENSICS IN CRIME INVESTIGATION", TypeId = types.Single(c => c.Name == "VIDEO").Id, Duration = "1 hr 10 min", Views = "42k", CategoryId = categories.Single(c => c.CategoryName == "BSCRIM").Id },
                    new Media { Title = "CUSTOMER SERVICE IN HOTEL MANAGEMENT", TypeId = types.Single(c => c.Name == "AUDIO").Id, Duration = "30 min", Views = "15k", CategoryId = categories.Single(c => c.CategoryName == "BSHM").Id },
                };
                context.Media.AddRange(media);
                await context.SaveChangesAsync();

                transaction.Commit();
            }
            catch (Exception ex) 
            { 
                transaction.Rollback();
            }
        }
    }
}
