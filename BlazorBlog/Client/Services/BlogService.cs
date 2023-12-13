using BlazorBlog.Shared;
using System.Net.Http.Json;

namespace BlazorBlog.Client.Services
{
    public class BlogService : IBlogService
    {
        private readonly HttpClient _httpClient;

        public BlogService(HttpClient http)
        {
            _httpClient = http;
        }

        public async Task<BlogPost> CreateNewBlogPost(BlogPost request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Blog", request);
            return await result.Content.ReadFromJsonAsync<BlogPost>();
        }

        public async Task<BlogPost> GetBlogPostByUrl(string url)
        {
            var result = await _httpClient.GetAsync($"api/Blog/{url}");
            if(result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var message = await result.Content.ReadAsStringAsync();
                Console.WriteLine(message);
                return new BlogPost { Title = message };
            }
            else
            {
                return await result.Content.ReadFromJsonAsync<BlogPost>();
            }
        }

        public async Task<List<BlogPost>> GetBlogPosts()
        {
            return await _httpClient.GetFromJsonAsync<List<BlogPost>>("api/Blog");
        }
    }
}
