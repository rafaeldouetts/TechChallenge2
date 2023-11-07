using ElmahCore;
using EntityFrameworkCoreMock;
using FakeItEasy;
using FakeItEasy.Sdk;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NoticiasAPI.Service;
using TechChallenge.Identity.Controllers;
using TechChallenge.Identity.Models;
using TechChallenge.NoticiasAPI.Data;

namespace NoticiasTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }

		[Fact]
		public async void Post_addsItemToDbContext()
		{
			//arrange
			DbContextMock<IdentityContext> dbContextMock = getDbContext(getInitialDbEntities());
			//AuthenticateController todoController = AuthenticateControllerInit(dbContextMock);
			var userAdded = new CreateUserModel() { Email = "", Password = "", UserName = ""};
			var Mock = AuthenticateControllerInit(dbContextMock);

			//act

			var teste = await Mock.CreateUserAsync(userAdded);

			var dsds = teste.As<ResponseModel>;

			
			var dsdsdd = teste as ResponseModel;


			var result = await  Mock.CreateUserAsync(userAdded) as RetornoModel;

			teste.Should().Be("Microsoft.AspNetCore.Mvc.BadRequestObjectResult");

			//assert
			//Assert.True(result.StatusCode == 500);
		}

		private DbContextMock<IdentityContext> getDbContext(IdentityUser[] initialEntities)
		{
			DbContextMock<IdentityContext> dbContextMock = new DbContextMock<IdentityContext>(new DbContextOptionsBuilder<IdentityContext>().Options);
			dbContextMock.CreateDbSetMock(x => x.Users, initialEntities);
			return dbContextMock;
		}

		private IdentityUser[] getInitialDbEntities()
		{
			return new IdentityUser[]
			 {
				new IdentityUser {Id = "1", UserName="Test1"},
				new IdentityUser {Id = "2", UserName="Test2"},
				new IdentityUser {Id = "3", UserName="Test3"},
			};
		}

		private AuthenticateController AuthenticateControllerInit(DbContextMock<IdentityContext> dbContextMock)
		{
			//Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();

			//Mock<UserManager<IdentityUser>> userManagerMock = new Mock<UserManager<IdentityUser>>();
			//Mock<HttpClient> httpMock = new Mock<HttpClient>();
			//Mock<IEmailService> emailMock = new Mock<IEmailService>();

			var configurationMock = A.Fake<IConfiguration>();
			var userManagerMock = A.Fake<UserManager<IdentityUser>>();
			var httpMock = A.Fake<HttpClient>();
			var emailMock = A.Fake<IEmailService>();

			return new AuthenticateController(configurationMock, userManagerMock, httpMock, emailMock);
		}
	}
}
