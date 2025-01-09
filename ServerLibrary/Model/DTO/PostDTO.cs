namespace ServerLibrary.Model.DTO
{
    public class PostDTO : CreatePostDTO
    {
        public int Id { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
    public class CreatePostDTO
    {
        public string Title { get; set; }
        public DateTime CreatedDate { get; } = DateTime.Now;
        public string Author { get; set; }
        public string Content { get; set; }
        public List<CreateCommentDTO> Comments { get; set; }
        //public List<Comment> Comments { get; set; }
    }
    public class UpdatePostDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? UpdatedDate { get; } = DateTime.Now;
    }

}

