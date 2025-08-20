using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopAppApi.BusinessLogic.Auth;
using ShopAppApi.Controllers;
using ShopAppApi.Infrastructure.Models;

namespace ShopAppApi.Tests.Controllers;

internal class TestAuthController
{
    #region Setup

    [OneTimeSetUp]
    public void Setup()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AuthController( _mediatorMock.Object );
        _cts = new CancellationTokenSource();
    }

    #endregion

    #region Tests

    [Test]
    public async Task GetUserAuthenticated_Password_ReturnsOk_WithTokenResponse()
    {
        #region Assert

        TokenResponse tokenResponse = new() { Token = "test_token" };
        _mediatorMock
            .Setup( m => m.Send( It.IsAny<GetUserAuthenticatedQuery>(), It.IsAny<CancellationToken>() ) )
            .ReturnsAsync( tokenResponse );
        LoginRequest loginRequest = new() { Email = "test", Password = "1234", AuthType = "Password" };

        #endregion

        #region Act

        IActionResult actual = await _controller.GetUserAuthenticated( loginRequest, _cts.Token );

        #endregion

        #region Assert

        actual.Should().NotBeNull().And.BeOfType<OkObjectResult>().Which.Value.Should().Be( tokenResponse );

        #endregion
    }

    [Test]
    public async Task GetUserAuthenticated_RefreshToken_ReturnsOk_WithTokenResponse()
    {
        #region Assert

        TokenResponse tokenResponse = new() { Token = "test_token" };
        _mediatorMock
            .Setup( m => m.Send( It.IsAny<GetUserAuthenticatedQuery>(), It.IsAny<CancellationToken>() ) )
            .ReturnsAsync( tokenResponse );
        LoginRequest loginRequest = new() { RefreshToken = "token", AuthType = "RefreshToken" };

        #endregion

        #region Act

        IActionResult actual = await _controller.GetUserAuthenticated( loginRequest, _cts.Token );

        #endregion

        #region Assert

        actual.Should().NotBeNull().And.BeOfType<OkObjectResult>().Which.Value.Should().Be( tokenResponse );

        #endregion
    }

    [Test]
    public async Task CreateUser_ReturnsOk_WithTokenResponse()
    {
        #region Arrange

        TokenResponse tokenResponse = new() { Token = "created_token" };
        _mediatorMock
            .Setup( m => m.Send( It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>() ) )
            .ReturnsAsync( tokenResponse );

        RegistrationRequest registrationRequest = new() { Email = "newuser", Password = "password" };

        #endregion

        #region Act

        IActionResult actual = await _controller.CreateUser( registrationRequest, _cts.Token );

        #endregion

        #region Assert

        actual.Should().NotBeNull()
              .And.BeOfType<OkObjectResult>()
              .Which.Value.Should().Be( tokenResponse );

        #endregion
    }

    #endregion

    #region Private members

    private Mock<IMediator> _mediatorMock;
    private AuthController _controller;
    private CancellationTokenSource _cts;

    #endregion
}