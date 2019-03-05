using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DomainEntities;
using DomainInterface;
using System.Globalization;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace DomainRepository
{
    public class DashboardRepo : IDashboardRepo
    {
        IOptions<ReadConfig> connectionString;
        public DashboardRepo(IOptions<ReadConfig> _connectionString)
        {
            connectionString = _connectionString;
        }

        
    }
}
