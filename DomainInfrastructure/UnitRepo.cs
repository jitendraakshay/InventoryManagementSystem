using Dapper;
using DomainEntities;
using DomainInterface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace DomainRepository
{
    public class UnitRepo : IUnitsRepo
    {
        IOptions<ReadConfig> connectionString;
        public UnitRepo(IOptions<ReadConfig> _connectionString)
        {
            connectionString = _connectionString;
        }
        public ReturnType deleteUnit(Units units)
        {
            throw new NotImplementedException();
        }

        public ReturnType editUnit(Units units)
        {
            throw new NotImplementedException();
        }

        public List<Units> getAllUnit()
        {
            try
            {
                
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    int operation = 2;
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@operation", operation);// 1 create, 2 read, 3 update, 4 delete                    
                    var returnType = SqlMapper.Query<Units>(
                                     connection, "[dbo].[usp_CrudUnit]", param, commandType: CommandType.StoredProcedure).ToList();
                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ReturnType saveUnit(Units units)
        {
           
                try
                {
                    int? operation = null;
                    using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                    {
                        if (units.UnitID == null)
                        {
                            operation = 1;
                        }
                        else
                        {
                            operation = 3;
                        }
                        DynamicParameters param = new DynamicParameters();

                        param.Add("@unitID", units.UnitID);
                        param.Add("@unitName", units.UnitName);
                        param.Add("@description", units.Description);
                        param.Add("@entryBy", units.EntryBy);
                        param.Add("@isActive", units.IsActive);
                        param.Add("@operation", operation);// 1 create, 2 read, 3 update, 4 delete                    
                        var returnType = SqlMapper.Query<ReturnType>(
                                         connection, "[dbo].[usp_CrudUnit]", param, commandType: CommandType.StoredProcedure).ToList()?.FirstOrDefault() ?? null;
                        return returnType;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
           
        }
    }
}
