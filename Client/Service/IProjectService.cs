using Core;
using Microsoft.AspNetCore.Components.Forms;

namespace Client.Service;

public interface IProjectService
{
    Task<bool> CreateProject(Project project, IBrowserFile timeFile, IBrowserFile materialFile);
}