﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Vetheria.Vtedy.ApiService.Models;

namespace Vetheria.Vtedy.ApiService.DataAccess.Queries
{
    public interface IProjectDataProvider
    {
        Task<IEnumerable<Project>> GetByUserIdAsync(int userId);
        Task<Project> GetByProjectIdAsync(int userId, int projectId);
        Task<Project> Add(Project project);
        Task<Project> UpdateAsync(Project project);
    }
}