using AutoMapper;
using Blog_Post_Api.Authentication;
using ServerLibrary.Model.DTO;
using ServerLibrary.Model.Entities;

namespace ServerLibrary.Data
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Post, PostDTO>().ReverseMap();
            CreateMap<Post, CreatePostDTO>().ReverseMap();
            CreateMap<Post, UpdatePostDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Comment, CreateCommentDTO>().ReverseMap();
            CreateMap<Comment, UpdateCommentDTO>().ReverseMap();
            CreateMap<ApiUser, UserDTO>().ReverseMap();
            CreateMap<ApiUser, LoginUserDTO>().ReverseMap();
            CreateMap<ApiUser, CreateUserDTO>().ReverseMap();

        }
    }
}
