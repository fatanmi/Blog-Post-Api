using ServerLibrary.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Model.DTO
{
    public class PostDTO : CreatePost
    {
        public int Id { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
    public class CreatePost
    {
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Author { get; set; }
        public string Content { get; set; }
        public List<Comment> Comments { get; set; }
        //public List<Comment> Comments { get; set; }
    }
    public class UpdatePost : CreatePost
    {
        public int Id { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
    }

}

