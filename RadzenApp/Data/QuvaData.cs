using System.Data.Entity;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using RadzenApp.Connection.QUVA;
using RadzenApp.Entities.QUVA;

namespace RadzenApp.Data;

/// <summary>
/// Datenservice für QUVA
/// </summary>
public partial class QuvaData
{
    public QuvaContext Ctx { get; set; }
    private readonly NavigationManager navigationManager;

    public QuvaData(QuvaContext ctx, NavigationManager navigationManager)
    {
        this.Ctx = ctx;
        this.navigationManager = navigationManager;
    }

    // von CRMDemoBlazor:
    public IQueryable QueryableFromQuery(Query query, IQueryable items)
    {
        if (query != null)
        {
            if (!string.IsNullOrEmpty(query.Expand))
            {
                var propertiesToExpand = query.Expand.Split(',');
                foreach (var p in propertiesToExpand)
                {
                    items = items.Include(p);
                }
            }

            if (!string.IsNullOrEmpty(query.Filter))
            {
                if (query.FilterParameters != null)
                {
                    items = items.Where(query.Filter, query.FilterParameters);
                }
                else
                {
                    items = items.Where(query.Filter);
                }
            }

            if (!string.IsNullOrEmpty(query.OrderBy))
            {
                items = items.OrderBy(query.OrderBy);
            }

            if (query.Skip.HasValue)
            {
                items = items.Skip(query.Skip.Value);
            }

            if (query.Top.HasValue)
            {
                items = items.Take(query.Top.Value);
            }
        }
        return items;
    }

    public Query QueryFromLoadDataArgs(LoadDataArgs args)
    {
        //siehe d:\Blazor\Repos\radzenhq\radzen-blazor\Radzen.Blazor\Common.cs
        var query = new Query
        {
            Skip = args.Skip,
            Top = args.Top,
            Filter = args.Filter,
            OrderBy = args.OrderBy
        };
        return query;
    }

    public IQueryable<SPEDITIONEN> SpedQuery(Query query)
    {
        var items = Ctx.SPEDITIONEN_Tbl.AsQueryable();
        //items = items.Include(i => i.Contact);
        //items = items.Include(i => i.OpportunityStatus);
        //Test:
        items = (IQueryable<SPEDITIONEN>)QueryableFromQuery(query, items);

        return items;
    }

    public IQueryable<SPEDITIONEN> SpedQuery(LoadDataArgs args)
    {
        var query = QueryFromLoadDataArgs(args);
        return SpedQuery(query);
    }

    public int SpedQueryCount(Query query)
    {
        var items = Ctx.SPEDITIONEN_Tbl.AsQueryable();
        //items = items.Include(i => i.Contact);
        //items = items.Include(i => i.OpportunityStatus);
        if (query != null)
        {
            query.Skip = null;
            query.Top = null;
            items = (IQueryable<SPEDITIONEN>)QueryableFromQuery(query, items);
        }
        return items.Count();
    }

    public int SpedQueryCount(LoadDataArgs args)
    {
        var query = QueryFromLoadDataArgs(args);
        return SpedQueryCount(query);
    }
}
    