using DocumentManagement.DAL.Azure;
using DocumentManagement.DAL.Helpers;
using DocumentManagement.DAL.Repositories;
using DocumentManagement.DAL.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentManagement.DAL.Extensions
{
    public static class DocumentManagementServiceCollection
    {
        public static IServiceCollection AddDocumentManagementServices(this IServiceCollection services, 
            string storageConnectionString)
        {
            services.AddScoped<AzureUtils>(provider =>
            {
                return new AzureUtils(storageConnectionString);
            });

            services.AddScoped<IFileUploadHelper, FileUploadHelper>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IDocumentService, DocumentService>();

            return services;
        }
    }
}
