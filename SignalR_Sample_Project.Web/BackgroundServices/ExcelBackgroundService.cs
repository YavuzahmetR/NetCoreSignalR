using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.FileProviders;
using SignalR_Sample_Project.Web.Hubs;
using SignalR_Sample_Project.Web.Models;
using System.Data;
using System.Threading.Channels;

namespace SignalR_Sample_Project.Web.BackgroundServices
{
    public class ExcelBackgroundService(Channel<(string userId, List<Product> products)> channel,
        IFileProvider fileProvider,IServiceProvider serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (await channel.Reader.WaitToReadAsync(stoppingToken))
            {
                await Task.Delay(2000);

                var (userId,products) = await channel.Reader.ReadAsync(stoppingToken);

                var wwwroot = fileProvider.GetDirectoryContents("wwwroot");

                var filesFolder = wwwroot.Single(x => x.Name == "files");

                var newExcelFileName = $"productList-{Guid.NewGuid()}.xlsx";

                var newExcelFilePath = Path.Combine(filesFolder.PhysicalPath!, newExcelFileName);

                var workBook = new XLWorkbook();

                var dataSet = new DataSet();

                dataSet.Tables.Add(GetTable("ProductList", products));

                workBook.Worksheets.Add(dataSet);

                await using var excelFileStream = new FileStream(newExcelFilePath, FileMode.Create);

                workBook.SaveAs(excelFileStream);


                using(var scope = serviceProvider.CreateScope())
                {
                    var appHub = scope.ServiceProvider.GetRequiredService<IHubContext<AppHub>>();
                    await appHub.Clients.User(userId).SendAsync($"AlertCompleteDownloadFile", $"/files/{newExcelFileName}", stoppingToken);
                }
            }
        }

        private DataTable GetTable(string tableName, List<Product> products)
        {
            var table = new DataTable() { TableName = tableName};

            foreach(var item in typeof(Product).GetProperties())
            {
                table.Columns.Add(item.Name,item.PropertyType);
            }

            products.ForEach(x =>
            {
                table.Rows.Add(x.Id, x.Name, x.Description, x.Price, x.UserId);
            });

            return table;

        }
    }
}
