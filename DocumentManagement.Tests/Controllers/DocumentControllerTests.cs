using DocumentManagement.Controllers;
using DocumentManagement.DAL.Services;
using DocumentManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Tests.Controllers
{
    [TestFixture]
    public class DocumentControllerTests
    {
        private DocumentsController _documentsController;
        private IDocumentService _documentService;

        [SetUp]
        public void SetUp()
        {
            var logger = Substitute.For<ILogger<DocumentsController>>();
            _documentService = Substitute.For<IDocumentService>();
            _documentsController = new DocumentsController(logger, _documentService);
        }

        [Test]
        public async Task Should_create_valid_document()
        {
            //Arrange
            var document = new DocumentDTO()
            {
                Id = Guid.NewGuid(),
                Name = "test.pdf",
                FileSize = 4999999,
                Location = ""
            };

            var formFile = Substitute.For<IFormFile>();
            formFile.FileName.Returns(document.Name);
            formFile.Length.Returns(document.FileSize);
            formFile.ContentType.Returns("application/pdf");
            _documentService.Create(Arg.Any<IFormFile>()).Returns(document);

            //Act
            var actionResult = await _documentsController.Create(formFile);

            //Assert
            Assert.AreEqual(actionResult.Value.Name, document.Name);
        }

        [Test]
        public async Task Should_failed_with_invalid_document_content_type()
        {
            //Arrange
            var formFile = Substitute.For<IFormFile>();
            formFile.Length.Returns(1000);
            formFile.ContentType.Returns("text/plain");

            //Act
            var actionResult = await _documentsController.Create(formFile);

            //Assert
            var badRequest = actionResult.Result as BadRequestObjectResult;
            Assert.IsTrue(actionResult.Result is BadRequestObjectResult);
            Assert.AreEqual(badRequest.Value, "Only pdf files are allowed.");
        }

        [Test]
        public async Task Should_failed_with_invalid_document_file_size()
        {
            //Arrange
            var formFile = Substitute.For<IFormFile>();
            formFile.Length.Returns(5000000);
            formFile.ContentType.Returns("application/pdf");

            //Act
            var actionResult = await _documentsController.Create(formFile);

            //Assert
            var badRequest = actionResult.Result as BadRequestObjectResult;
            Assert.IsTrue(actionResult.Result is BadRequestObjectResult);
            Assert.AreEqual(badRequest.Value, "File size succed max value 5MB.");
        }
    }
}
