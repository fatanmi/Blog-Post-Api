using ServerLibrary.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Author { get; set; }
        public string Content { get; set; }
        public List<CreateCommentDTO> Comments { get; set; }
        //public List<Comment> Comments { get; set; }
    }
    public class UpdatePostDTO : CreatePostDTO
    {
        public int Id { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
    }

}

