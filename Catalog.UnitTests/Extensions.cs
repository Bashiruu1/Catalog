using Microsoft.AspNetCore.Mvc;

namespace Catalog.UnitTests
{
    public static class Extensions
    {
        public static T GetObjectResult<T>(this ActionResult<T> result)
        {
            if (result.Result != null)
                return (T)((ObjectResult)result.Result).Value;
            return result.Value;            
        }
    }
}