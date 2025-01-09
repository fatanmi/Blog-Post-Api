
namespace ServerLibrary.Model.DTO
{
    public class CommentDTO : CreateComment
    {
        public int Id { get; set; }

        public DateTime? UpdatedDate { get; set; }

    }

    public class CreateComment
    {
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        //public List<Comment> Comments { get; set; }
    }
    public class UpdateComment : CreateComment
    {
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
    }

}
