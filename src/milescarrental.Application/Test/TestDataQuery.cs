using MediatR;
using milescarrental.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace milescarrental.Application.Test
{
    public class TestDataQuery : IRequest<List<TestGetdata>>
    {
        public TestDataQuery()
        {
        }
    }
}
