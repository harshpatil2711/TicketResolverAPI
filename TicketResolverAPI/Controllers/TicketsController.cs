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
        private readonly MasterDAL masterDAL = new MasterDAL();

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

                model.Categories = masterDAL.GetCategories();
                model.Priorities = masterDAL.GetPriorities();
                model.Statuses = masterDAL.GetStatuses();

                return Ok(new
                {
                    items = model.Results,
                    totalCount = model.TotalCount,
                    page = model.PageNumber,
                    size = model.PageSize,
                    totalPages = model.PageSize <= 0 ? 0 : (int)Math.Ceiling(model.TotalCount / (double)model.PageSize),
                    categories = model.Categories,
                    priorities = model.Priorities,
                    statuses = model.Statuses
                });
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Failed to load tickets." });
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var detail = ticketDAL.GetById(id);
                if (detail == null)
                    return Content(HttpStatusCode.NotFound, new { message = "Ticket not found." });

                return Ok(new
                {
                    ticket = detail,
                    categories = masterDAL.GetCategories(),
                    priorities = masterDAL.GetPriorities()
                });
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Failed to load ticket." });
            }
        }

        [HttpPost]
        [Route("update")]
        public IHttpActionResult Update([FromBody] TicketEditViewModel request)
        {
            if (request == null || request.TicketId <= 0)
                return Content(HttpStatusCode.BadRequest, new { message = "Invalid request body." });

            if (string.IsNullOrWhiteSpace(request.Subject))
                return Content(HttpStatusCode.BadRequest, new { message = "Subject is required." });

            if (string.IsNullOrWhiteSpace(request.Description))
                return Content(HttpStatusCode.BadRequest, new { message = "Description is required." });

            if (request.CategoryId <= 0)
                return Content(HttpStatusCode.BadRequest, new { message = "Category is required." });

            if (request.PriorityId <= 0)
                return Content(HttpStatusCode.BadRequest, new { message = "Priority is required." });

            try
            {
                var affected = ticketDAL.Update(request);
                return Ok(new { affectedRows = affected, success = affected > 0 });
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Failed to update ticket." });
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
