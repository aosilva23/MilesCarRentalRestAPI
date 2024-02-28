using System;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Newtonsoft.Json;
using milescarrental.Application;
using milescarrental.Application.Configuration.Data;
using milescarrental.Application.Configuration.Processing;

namespace milescarrental.Infrastructure.Processing
{
    public class CommandsScheduler : ICommandsScheduler
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CommandsScheduler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task EnqueueAsync(IRequest command)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            const string sqlInsert = "INSERT INTO [app].[InternalCommands] ([Id], [EnqueueDate] , [Type], [Data]) VALUES " +
                                     "(@Id, @EnqueueDate, @Type, @Data)";

            await connection.ExecuteAsync(sqlInsert, new
            {
                Id = Guid.NewGuid(),
                EnqueueDate = DateTime.UtcNow,
                Type = command.GetType().FullName,
                Data = JsonConvert.SerializeObject(command)
            });
        }
    }
}