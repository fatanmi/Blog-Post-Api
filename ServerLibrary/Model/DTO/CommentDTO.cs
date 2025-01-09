
namespace ServerLibrary.Model.DTO
{
    public class CommentDTO : CreateCommentDTO
    {
        public int Id { get; set; }

        public DateTime? UpdatedDate { get; set; }

    }

    public class CreateCommentDTO
    {
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreatedDate { get; } = DateTime.Now;
    }
    public class UpdateCommentDTO : CreateCommentDTO
    {
        public DateTime? UpdatedDate { get; } = DateTime.Now;
    }

}
