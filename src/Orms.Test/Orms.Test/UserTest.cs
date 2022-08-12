using Microsoft.AspNetCore.Mvc;
using Moq;
using Orms.Domain.Entities;
using Orms.Domain.Entities.Pagination;
using Orms.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orms.Tests
{
    public class UserTest: TestBase
    {
        [Fact]
        public async Task Get_GetUser()
        {
            var idUser = 1;

            var (_, mock, _, userController, context) = GetHockRepository();

            mock.Setup(m => m.GetUserAsync(idUser, It.IsAny<CancellationToken>())).ReturnsAsync(() =>
                Helpers.GetUsers().First(p=> p.UserID ==idUser))
                .Verifiable();

            var result = await userController.Get(idUser) as ObjectResult;
            var value = result?.Value as User;

            Assert.IsType<OkObjectResult>(result);
            //Check name
            Assert.Equal("Edson 1", value?.Name);
        }
    }
}
