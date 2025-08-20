using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopAppApi.BusinessLogic.Lists;
using ShopAppApi.Controllers;
using ShopAppApi.Infrastructure.Models;

namespace ShopAppApi.Tests.Controllers
{
    internal class TestListsController
    {
        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ListsController( _mediatorMock.Object );
            _cts = new CancellationTokenSource();
        }

        #endregion

        #region Tests

        [Test]
        public async Task CreateList_ReturnsOkResult_WithListResponse()
        {
            #region Arrange

            ListCreationRequest request = new();
            ListResponse expectedResponse = new();
            _mediatorMock
                .Setup( m => m.Send( It.IsAny<CreateListCommand>(), It.IsAny<CancellationToken>() ) )
                .ReturnsAsync( expectedResponse );

            #endregion

            #region Act

            IActionResult result = await _controller.CreateList( request, _cts.Token );

            #endregion

            #region Assert

            result.Should().NotBeNull()
                  .And.BeOfType<OkObjectResult>()
                  .Which.Value.Should().Be( expectedResponse );

            #endregion
        }

        [Test]
        public async Task GetLists_ReturnsOkResult_WithListOfListResponses()
        {
            #region Arrange

            List<ListResponse> expectedList = new() { new() };
            _mediatorMock
                .Setup( m => m.Send( It.IsAny<GetListsQuery>(), It.IsAny<CancellationToken>() ) )
                .ReturnsAsync( expectedList );

            #endregion

            #region Act

            IActionResult result = await _controller.GetLists( _cts.Token );

            #endregion

            #region Assert

            result.Should().NotBeNull()
                  .And.BeOfType<OkObjectResult>()
                  .Which.Value.Should().Be( expectedList );

            #endregion
        }

        [Test]
        public async Task GetListDetails_ReturnsOkResult_WithItemResponses()
        {
            #region Arrange

            int listId = 1;
            ListDetailsResponse listDetails = new();
            _mediatorMock
                .Setup( m => m.Send( It.IsAny<GetListDetailsQuery>(), It.IsAny<CancellationToken>() ) )
                .ReturnsAsync( listDetails );

            #endregion

            #region Act

            IActionResult result = await _controller.GetListDetails( listId, _cts.Token );

            #endregion

            #region Assert

            result.Should().NotBeNull()
                  .And.BeOfType<OkObjectResult>()
                  .Which.Value.Should().Be( listDetails );

            #endregion
        }

        [Test]
        public async Task SetItemsStatuses_ReturnsOkResult()
        {
            #region Arrange

            Dictionary<int, bool> statuses = new() { { 1, true } };
            _mediatorMock
                .Setup( m => m.Send( It.IsAny<BatchMarkItemsCommand>(), It.IsAny<CancellationToken>() ) )
                .Returns( Task.CompletedTask );

            #endregion

            #region Act

            IActionResult result = await _controller.SetItemsStatuses( statuses, _cts.Token );

            #endregion

            #region Assert

            result.Should().BeOfType<OkResult>();

            #endregion
        }

        [Test]
        public async Task GetTemplateLists_ReturnsOkResult_WithListOfListResponses()
        {
            #region Arrange

            List<ListResponse> expectedTemplates = new() { new() };
            _mediatorMock
                .Setup( m => m.Send( It.IsAny<GetTemplateListsQuery>(), It.IsAny<CancellationToken>() ) )
                .ReturnsAsync( expectedTemplates );

            #endregion

            #region Act

            IActionResult result = await _controller.GetTemplateLists( _cts.Token );

            #endregion

            #region Assert

            result.Should().NotBeNull()
                  .And.BeOfType<OkObjectResult>()
                  .Which.Value.Should().Be( expectedTemplates );

            #endregion
        }

        [Test]
        public async Task UpdateList_ReturnsOkResult()
        {
            #region Arrange

            UpdateListRequest request = new();
            _mediatorMock
                .Setup( m => m.Send( It.IsAny<UpdateListCommand>(), It.IsAny<CancellationToken>() ) )
                .Returns( Task.CompletedTask );

            #endregion

            #region Act

            IActionResult result = await _controller.UpdateList( request, _cts.Token );

            #endregion

            #region Assert

            result.Should().BeOfType<OkResult>();

            #endregion
        }

        [Test]
        public async Task ArchiveList_ReturnsOkResult()
        {
            #region Arrange

            int listId = 123;
            _mediatorMock
                .Setup( m => m.Send( It.IsAny<ArchiveListCommand>(), It.IsAny<CancellationToken>() ) )
                .Returns( Task.CompletedTask );

            #endregion

            #region Act

            IActionResult result = await _controller.ArchiveList( listId, _cts.Token );

            #endregion

            #region Assert

            result.Should().BeOfType<OkResult>();

            #endregion
        }

        #endregion

        #region Private members

        private Mock<IMediator> _mediatorMock;
        private ListsController _controller;
        private CancellationTokenSource _cts;

        #endregion
    }
}