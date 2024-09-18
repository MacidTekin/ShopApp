using ShopApp.Data.Entity;

namespace ShopApp.Data.Abstract;

public interface ICommentRepository 
{
    IQueryable<Comment> Comments { get; }  

    void CreateComment(Comment comment);
}