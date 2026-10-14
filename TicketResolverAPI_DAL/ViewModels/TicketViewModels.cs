using System;
using System.Collections.Generic;

namespace TicketResolverAPI_DAL.ViewModels
{
    public class TicketListRequestViewModel
    {
        public string SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public int? PriorityId { get; set; }
        public int? StatusId { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public bool? IsUnassigned { get; set; }
    }

    public class TicketSearchViewModel
    {
        public string SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public int? PriorityId { get; set; }
        public int? StatusId { get; set; }
        public int? AssignedTo { get; set; }
        public int? CreatedBy { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public string SortColumn { get; set; } = "Created";
        public string SortDirection { get; set; } = "DESC";
        public bool? IsUnassigned { get; set; }
        public List<TicketListItemViewModel> Results { get; set; }
        public List<TicketResolverAPI_DAL.Models.TicketCategory> Categories { get; set; }
        public List<TicketResolverAPI_DAL.Models.TicketPriority> Priorities { get; set; }
        public List<TicketResolverAPI_DAL.Models.TicketStatus> Statuses { get; set; }
    }

    public class TicketListItemViewModel
    {
        public int TicketId { get; set; }
        public string TicketNumber { get; set; }
        public string Subject { get; set; }
        public string CategoryName { get; set; }
        public string PriorityName { get; set; }
        public string StatusName { get; set; }
        public string CreatedByName { get; set; }
        public string AssignedToName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
