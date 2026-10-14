namespace TicketResolverAPI_DAL.Models
{
    public class TicketCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class TicketPriority
    {
        public int PriorityId { get; set; }
        public string PriorityName { get; set; }
        public int Sequence { get; set; }
    }

    public class TicketStatus
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public bool IsTerminalState { get; set; }
    }
}
