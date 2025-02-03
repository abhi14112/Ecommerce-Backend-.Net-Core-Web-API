using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Repository.Interface
{
    public interface IProfileRepository
    {
        public IActionResult GetCurrentUser();
    }
}
