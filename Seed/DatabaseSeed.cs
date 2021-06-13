using System.Linq;
using efCoreSeederSample.Models;
using efCoreSeederSample.Seeder.Attributes;
using Microsoft.EntityFrameworkCore;

namespace efCoreSeederSample.Seed
{
    public class DatabaseSeed
    {
        public SeederSampleDbContext DbContext { get; set; }

        public DatabaseSeed(SeederSampleDbContext dbContext)
        {
            DbContext = dbContext;
        }

        [Seeder(typeof(Category), 1)]
        public void CategorySeeder()
        {
            for (var i = 1; i <= 3; i++) {
                DbContext.Categories.Add(new Category {
                    Name = $"Category {i}"
                });
            }

            DbContext.SaveChanges();
        }

        [Seeder(typeof(Post), 2)]
        public void PostSeeder()
        {
            var categories =  DbContext.Categories.ToList();

            categories.ForEach(category =>
            {
                for (var i = 1; i <= 3; i++) {
                     DbContext.Posts.Add(new Post {
                        CategoryId = category.Id,
                        Title = $"Title for post {i} in category {category.Name}",
                        Description = $"Description for post {i} in category {category.Name}"
                    });
                }
            });

            DbContext.SaveChanges();
        }

        [Seeder(typeof(Comment), 3)]
        public void CommentSeeder()
        {
            var posts = DbContext.Posts
                .Include(x => x.Category)
                .ToList();

            posts.ForEach(post =>
            {
                for (var i = 1; i <= 3; i++) {
                    DbContext.Comments.Add(new Comment {
                        PostId = post.Id,
                        Content = $"Comment {i} for post {post.Id} in category {post.Category.Name}",
                    });
                }
            });

            DbContext.SaveChanges();
        }
    }
}