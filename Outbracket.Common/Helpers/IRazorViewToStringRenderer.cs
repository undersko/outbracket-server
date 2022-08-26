using System.Threading.Tasks;

namespace Outbracket.Common.Helpers
{
    public interface IRazorViewToStringRenderer
    {
        Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
    }
}