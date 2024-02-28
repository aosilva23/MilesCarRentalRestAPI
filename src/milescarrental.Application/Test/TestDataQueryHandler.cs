using Dapper;
using MediatR;
using milescarrental.Application.Configuration.Data;
using milescarrental.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace milescarrental.Application.Test
{
    internal class TestDataQueryHandler : IRequestHandler<TestDataQuery, List<TestGetdata>>
    {
        private readonly AppConfiguration _appConfig;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public TestDataQueryHandler(AppConfiguration appConfig, ISqlConnectionFactory sqlConnectionFactory)
        {
            this._appConfig = appConfig;
            this._sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<TestGetdata>> Handle(TestDataQuery request, CancellationToken cancellationToken)
        {
            List<TestGetdata> result = new List<TestGetdata>();

            var connection = this._sqlConnectionFactory.GetOpenConnection();
            
            var strQuery = @"SELECT NUMEROFILA_ AS NUMEROFILA, NOMBRETABLA, DESCRIPCION FROM CARLOG";

            var parameter = new
            {
            };

            var listSet = await connection.QueryAsync<TestGetdata>(strQuery, parameter);
            var qresult = listSet.AsList();

            connection.Dispose();

            return qresult;
        }
    }
}
