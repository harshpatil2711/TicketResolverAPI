using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Http;
using TicketResolverAPI_DAL.DAL;
using TicketResolverAPI_DAL.ViewModels;

namespace TicketResolverAPI.Controllers
{
    [RoutePrefix("api/tickets")]
    public class TicketsController : ApiController
    {
        private readonly TicketDAL ticketDAL = new TicketDAL();

        [HttpPost]
        [Route("list")]
        public IHttpActionResult Search([FromBody] TicketListRequestViewModel request)
        {
            if (request == null)
                return Content(HttpStatusCode.BadRequest, new { message = "Request body is empty." });

            try
            {
                var model = BuildSearchModel(request);
                var ds = ticketDAL.Search(model);

                model.TotalCount = ds.Tables[0].Rows.Count > 0
                    ? Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"])
                    : 0;

                model.Results = ds.Tables[0].Rows.Cast<DataRow>().Select(r => new TicketListItemViewModel
                {
                    TicketId = Convert.ToInt32(r["TicketId"]),
                    TicketNumber = r["TicketNumber"].ToString(),
                    Subject = r["Subject"].ToString(),
                    CategoryName = r["CategoryName"].ToString(),
                    PriorityName = r["PriorityName"].ToString(),
                    StatusName = r["StatusName"].ToString(),
                    CreatedByName = r["CreatedByName"].ToString(),
                    AssignedToName = r["AssignedToName"].ToString(),
                    CreatedDate = Convert.ToDateTime(r["CreatedDate"])
                }).ToList();

                return Ok(new
                {
                    items = model.Results,
                    totalCount = model.TotalCount,
                    page = model.PageNumber,
                    size = model.PageSize,
                    totalPages = model.PageSize <= 0 ? 0 : (int)Math.Ceiling(model.TotalCount / (double)model.PageSize)
                });
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Failed to load tickets." });
            }
        }

        private TicketSearchViewModel BuildSearchModel(TicketListRequestViewModel request)
        {
            return new TicketSearchViewModel
            {
                SearchTerm = request.SearchTerm,
                CategoryId = request.CategoryId,
                PriorityId = request.PriorityId,
                StatusId = request.StatusId,
                AssignedTo = null,
                CreatedBy = null,
                PageNumber = request.Page < 1 ? 1 : request.Page,
                PageSize = request.Size < 1 || request.Size > 100 ? 10 : request.Size,
                SortColumn = "Created",
                SortDirection = "DESC",
                IsUnassigned = request.IsUnassigned
            };
        }
    }
}
