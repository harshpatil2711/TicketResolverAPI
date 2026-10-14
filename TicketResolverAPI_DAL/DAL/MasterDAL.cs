using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using TicketResolverAPI_DAL.Models;

namespace TicketResolverAPI_DAL.DAL
{
    public class MasterDAL
    {
        private readonly Database db = DatabaseFactory.CreateDatabase();

        public List<TicketCategory> GetCategories()
        {
            var list = new List<TicketCategory>();
            using (DataSet ds = db.ExecuteDataSet(db.GetStoredProcCommand("TicketResolverTicketCategoryGetAll")))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(new TicketCategory
                    {
                        CategoryId = Convert.ToInt32(row["CategoryId"]),
                        CategoryName = row["CategoryName"].ToString()
                    });
                }
            }
            return list;
        }

        public List<TicketPriority> GetPriorities()
        {
            var list = new List<TicketPriority>();
            using (DataSet ds = db.ExecuteDataSet(db.GetStoredProcCommand("TicketResolverTicketPriorityGetAll")))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(new TicketPriority
                    {
                        PriorityId = Convert.ToInt32(row["PriorityId"]),
                        PriorityName = row["PriorityName"].ToString(),
                        Sequence = Convert.ToInt32(row["Sequence"])
                    });
                }
            }
            return list;
        }

        public List<TicketStatus> GetStatuses()
        {
            var list = new List<TicketStatus>();
            using (DataSet ds = db.ExecuteDataSet(db.GetStoredProcCommand("TicketResolverTicketStatusGetAll")))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(new TicketStatus
                    {
                        StatusId = Convert.ToInt32(row["StatusId"]),
                        StatusName = row["StatusName"].ToString(),
                        IsTerminalState = Convert.ToBoolean(row["IsTerminalState"])
                    });
                }
            }
            return list;
        }
    }
}
