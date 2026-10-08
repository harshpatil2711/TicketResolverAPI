using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using TicketResolverAPI_DAL.ViewModels;

namespace TicketResolverAPI_DAL.DAL
{
    public class TicketDAL
    {
        private readonly Database db = DatabaseFactory.CreateDatabase();

        public DataSet Search(TicketSearchViewModel model)
        {
            using (DbCommand cmd = db.GetStoredProcCommand("TicketResolverTicketSearch"))
            {
                db.AddInParameter(cmd, "@SearchTerm", DbType.String, model.SearchTerm ?? (object)DBNull.Value);
                db.AddInParameter(cmd, "@CategoryId", DbType.Int32, model.CategoryId ?? (object)DBNull.Value);
                db.AddInParameter(cmd, "@PriorityId", DbType.Int32, model.PriorityId ?? (object)DBNull.Value);
                db.AddInParameter(cmd, "@StatusId", DbType.Int32, model.StatusId ?? (object)DBNull.Value);
                db.AddInParameter(cmd, "@AssignedTo", DbType.Int32, model.AssignedTo ?? (object)DBNull.Value);
                db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, model.CreatedBy ?? (object)DBNull.Value);
                db.AddInParameter(cmd, "@PageNumber", DbType.Int32, model.PageNumber);
                db.AddInParameter(cmd, "@PageSize", DbType.Int32, model.PageSize);
                db.AddInParameter(cmd, "@SortColumn", DbType.String, model.SortColumn);
                db.AddInParameter(cmd, "@SortDirection", DbType.String, model.SortDirection);
                db.AddInParameter(cmd, "@IsUnassigned", DbType.Boolean, model.IsUnassigned ?? (object)DBNull.Value);
                return db.ExecuteDataSet(cmd);
            }
        }
    }
}
