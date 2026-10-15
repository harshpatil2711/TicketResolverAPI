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

        public TicketDetailViewModel GetById(int ticketId)
        {
            using (DbCommand cmd = db.GetStoredProcCommand("TicketResolverTicketGetById"))
            {
                db.AddInParameter(cmd, "@TicketId", DbType.Int32, ticketId);
                using (DataSet ds = db.ExecuteDataSet(cmd))
                {
                    if (ds.Tables[0].Rows.Count == 0)
                        return null;

                    DataRow row = ds.Tables[0].Rows[0];
                    return new TicketDetailViewModel
                    {
                        TicketId = Convert.ToInt32(row["TicketId"]),
                        TicketNumber = row["TicketNumber"].ToString(),
                        Subject = row["Subject"].ToString(),
                        Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString(),
                        CategoryId = Convert.ToInt32(row["CategoryId"]),
                        CategoryName = row["CategoryName"].ToString(),
                        PriorityId = Convert.ToInt32(row["PriorityId"]),
                        PriorityName = row["PriorityName"].ToString(),
                        StatusId = Convert.ToInt32(row["StatusId"]),
                        StatusName = row["StatusName"].ToString(),
                        IsTerminalState = Convert.ToBoolean(row["IsTerminalState"]),
                        CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                        CreatedByName = row["CreatedByName"].ToString(),
                        AssignedTo = row["AssignedTo"] == DBNull.Value ? null : (int?)Convert.ToInt32(row["AssignedTo"]),
                        AssignedToName = row["AssignedToName"] == DBNull.Value ? null : row["AssignedToName"].ToString(),
                        CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                        ResolvedDate = row["ResolvedDate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(row["ResolvedDate"]),
                        ClosedDate = row["ClosedDate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(row["ClosedDate"])
                    };
                }
            }
        }

        public int Update(TicketEditViewModel model)
        {
            using (DbCommand cmd = db.GetStoredProcCommand("TicketResolverTicketUpdate2"))
            {
                db.AddInParameter(cmd, "@TicketId", DbType.Int32, model.TicketId);
                db.AddInParameter(cmd, "@Subject", DbType.String, model.Subject);
                db.AddInParameter(cmd, "@Description", DbType.String, model.Description);
                db.AddInParameter(cmd, "@CategoryId", DbType.Int32, model.CategoryId);
                db.AddInParameter(cmd, "@PriorityId", DbType.Int32, model.PriorityId);
                db.AddInParameter(cmd, "@ModifiedBy", DbType.Int32, model.ModifiedBy ?? (object)DBNull.Value);
                db.AddOutParameter(cmd, "@AffectedRows", DbType.Int32, 4);
                db.ExecuteNonQuery(cmd);
                return Convert.ToInt32(cmd.Parameters["@AffectedRows"].Value);
            }
        }
    }
}
