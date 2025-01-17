#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LBPUnion.ProjectLighthouse.Pages.Layouts;
using LBPUnion.ProjectLighthouse.Types;
using LBPUnion.ProjectLighthouse.Types.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBPUnion.ProjectLighthouse.Pages;

public class UsersPage : BaseLayout
{
    public int PageAmount;

    public int PageNumber;

    public int UserCount;

    public List<User> Users;

    public string SearchValue;

    public UsersPage([NotNull] Database database) : base(database)
    {}

    public async Task<IActionResult> OnGet([FromRoute] int pageNumber, [FromQuery] string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) name = "";

        this.UserCount = await this.Database.Users.CountAsync(u => !u.Banned && u.Username.Contains(name));

        this.SearchValue = name;
        this.PageNumber = pageNumber;
        this.PageAmount = Math.Max(1, (int)Math.Ceiling((double)this.UserCount / ServerStatics.PageSize));

        if (this.PageNumber < 0 || this.PageNumber >= this.PageAmount) return this.Redirect($"/users/{Math.Clamp(this.PageNumber, 0, this.PageAmount - 1)}");

        this.Users = await this.Database.Users.Where(u => !u.Banned && u.Username.Contains(name))
            .OrderByDescending(b => b.UserId)
            .Skip(pageNumber * ServerStatics.PageSize)
            .Take(ServerStatics.PageSize)
            .ToListAsync();

        return this.Page();
    }
}