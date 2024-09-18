using ShopApp.Data.Abstract;
using ShopApp.Data.Concrete.EfCore;
using ShopApp.Data.Entity;

namespace ShopApp.Data.Concrete;

public class EfCommentRepository : ICommentRepository
{
    private readonly ShopDbContext _context;

    public EfCommentRepository(ShopDbContext context)
    {
        _context = context;
    }

    public IQueryable<Comment> Comments => _context.Comments;

    public void CreateComment(Comment comment)
    {
        _context.Comments.Add(comment);
        _context.SaveChanges();
    }
}