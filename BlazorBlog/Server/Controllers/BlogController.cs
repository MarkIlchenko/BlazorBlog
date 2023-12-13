using BlazorBlog.Server.Data;
using BlazorBlog.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorBlog.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public BlogController(DataContext context)
        {
            _dataContext = context;
        }

        [HttpGet]
        public ActionResult<List<BlogPost>> GivemeAllTheBlogPosts()
        {
            return Ok(_dataContext.BlogPosts.OrderByDescending(p => p.DateCreated));
        }

        [HttpGet("{url}")]
        public ActionResult<BlogPost> GivemeThatBlogPost(string url) 
        {
            var post = _dataContext.BlogPosts.FirstOrDefault(p => p.Url.ToLower().ToLower().Equals(url.ToLower()));
            if(post == null)
            {
                return NotFound("This post does not exist.");
            }

            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult<BlogPost>> CreateNewBlogPost(BlogPost request)
        {
            _dataContext.Add(request);
            await _dataContext.SaveChangesAsync();

            return request;
        }
    }
}
