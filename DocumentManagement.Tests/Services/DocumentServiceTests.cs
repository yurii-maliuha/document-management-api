using DocumentManagement.DAL.Entities;
using DocumentManagement.DAL.Helpers;
using DocumentManagement.DAL.Repositories;
using DocumentManagement.DAL.Services;
using DocumentManagement.Models.Exceptions;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.Tests.Services
{
    [TestFixture]
    public class DocumentServiceTests
    {
        private DocumentService _documentService;
        private IFileUploadHelper _fileHelper;
        private IDocumentRepository _documentRepository;

        [SetUp]
        public void SetUp()
        {
            _fileHelper = Substitute.For<IFileUploadHelper>();
            _documentRepository = Substitute.For<IDocumentRepository>();
            _documentService = new DocumentService(_fileHelper, _documentRepository);
        }

        [Test]
        public async Task Should_delete_the_existed_document_blob()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var blobName = "test.pdf";
            _fileHelper.ExistsAsync($"{id}_{blobName}").Returns(true);
            _documentRepository.ExistsAsync(blobName, id).Returns(true);

            //Act
            await _documentService.Delete(blobName, id);

            //Assert
            await _fileHelper.Received().DeleteAsync($"{id}_{blobName}");
            await _documentRepository.Received().DeleteAsync(blobName, id);
        }

        [Test]
        public void Should_throw_error_for_deletion_of_nonexisted_document_blob()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var blobName = "test.pdf";
            _fileHelper.ExistsAsync($"{id}_{blobName}").Returns(false);

            //Act
            //Assert
            Assert.ThrowsAsync<DocumentNotFoundException>(
                async () => await _documentService.Delete(blobName, id), 
                $"Can not delete non existed file {blobName}");
        }

        [Test]
        public void Should_throw_error_for_deletion_of_nonexisted_document_item()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var blobName = "test.pdf";
            _fileHelper.ExistsAsync($"{id}_{blobName}").Returns(true);
            _documentRepository.ExistsAsync(blobName, id).Returns(false);

            //Act
            //Assert
            Assert.ThrowsAsync<DocumentNotFoundException>(
                async () => await _documentService.Delete(blobName, id),
                $"Cannot delete the nonexistent file {blobName} : {id}");
        }

        [Test]
        public async Task Should_update_documents_order()
        {
            //Arrange
            _documentRepository.GetAll().Returns(DocumentServiceTestsData.UnsortedDocuments);
            DocumentEntity passedDocument = null;
            _documentRepository.UpdateAsync(Arg.Do<DocumentEntity>(d => passedDocument = d))
                .ReturnsForAnyArgs((document) =>
                    DocumentServiceTestsData.SortedDocuments.First(d => d.RowKey == passedDocument.RowKey)
                 );

            //Act
            var updatedDocuments = await _documentService.UpdateDocumentsOrder(DocumentServiceTestsData.DocumentPatchModels);

            //Assert
            var expectedResult = DocumentServiceTestsData.SortedDocuments.Select(document => document.RowKey);
            var result = updatedDocuments.Select(d => d.Id.ToString());
            Assert.AreEqual(result, expectedResult);
        }

    }
}
