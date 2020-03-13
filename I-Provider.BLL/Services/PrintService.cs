using I_Provider.BLL.Repositories;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System;

namespace I_Provider.BLL.Services
{
    public class PrintService
    {
        public void TariffPlansToPDF(TariffPlanRepos repository)
        {
            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(HttpContext.Current.Server.MapPath(@"\Content\TariffsPDF\Tariff plans.pdf"), FileMode.Create));
            doc.Open();
            BaseFont baseFont = BaseFont.CreateFont(HttpContext.Current.Server.MapPath(@"\Content\Fonts\arial.ttf"), BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
            PdfPTable table = new PdfPTable(4);
            PdfPCell cell = new PdfPCell(new Phrase($"Тарифные планы ({DateTime.Today.Day}.{DateTime.Today.Month}.{DateTime.Today.Year})", font));

            cell.Colspan = 4;
            cell.HorizontalAlignment = 1;
            cell.Border = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(new Phrase("Название", font)));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Phrase("Описание", font)));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Phrase("Услуги", font)));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Phrase("Цена (со скидкой)", font)));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);

            int i = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var item in repository.GetAll().Where(n => n.IsDeleted == false))
            {
                table.AddCell(new Phrase(item.ShortDesc, font));
                table.AddCell(new Phrase(item.Description, font));
                item.Types.Select(n => n.TypeName + " " + n.DiscountPrice.ToString() + " грн. ").ToList().ForEach(n => sb.Append(n));
                table.AddCell(new Phrase(sb.ToString(), font));
                table.AddCell(new Phrase(item.Price.ToString() + " грн", font));
                sb.Clear();
                i++;
            }
            
            doc.Add(table);
            doc.Close();
        }
    }
}
