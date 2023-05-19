using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
 
using WriteDry.Models;
using WriteDry.Utils;

namespace WriteDry.PdfMarkup
{
    public static class CheckMarkup
    {
        public static string GenerateMarkup(Cart cart, float OrderAmmount, float DiscountAmmount, Point PickupPoint, int OrderCode, int OrderNumber)
        {
            var filename = $"Талон за {DateOnly.FromDateTime(DateTime.Now).ToString("d")}.pdf";
            PdfWriter writer = new(filename);
            PdfDocument pdf = new(writer);
            Document document = new(pdf);


            PdfFont corbel = PdfFontFactory.CreateFont(@"C:\Windows\Fonts\Corbel.ttf", PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_NOT_EMBEDDED);

            var content = new Paragraph("Благодарим за покупку")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(corbel)
                .SetFontSize(32);
            document.Add(content);

            content = new Paragraph(" ")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(corbel)
               .SetFontSize(16);
            document.Add(content);

            Table table = new(2, true);
            table.SetMarginBottom(10f);

            table.AddCell(new Paragraph("Дата заказа:")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(corbel)
                .SetFontSize(16));

            table.AddCell(new Paragraph(DateOnly.FromDateTime(DateTime.Now).ToString("d"))
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(corbel)
                .SetFontSize(16));

            table.AddCell(new Paragraph("Номер заказа:")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(corbel)
                .SetFontSize(16));

            table.AddCell(new Paragraph(string.Format("{0}", OrderNumber))
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(corbel)
                .SetFontSize(16));

            var tableOrder = new Table(6, false)
                .SetWidth(UnitValue.CreatePercentValue(100))
                .SetHeight(UnitValue.CreatePercentValue(100))
                .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

            tableOrder.AddCell(new Paragraph("Название")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(corbel)
               .SetFontSize(16));

            tableOrder.AddCell(new Paragraph("Описание")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(corbel)
               .SetFontSize(16));

            tableOrder.AddCell(new Paragraph("Цена")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(corbel)
               .SetFontSize(16));

            tableOrder.AddCell(new Paragraph("Скидка")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(corbel)
               .SetFontSize(16));

            tableOrder.AddCell(new Paragraph("Кол-во")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(corbel)
               .SetFontSize(16));

            tableOrder.AddCell(new Paragraph("Сумма")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(corbel)
               .SetFontSize(16));

            foreach (var item in cart.CartItems)
            {
                tableOrder.AddCell(new Paragraph(item.Product.ProductNameNavigation.ProductName)
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(corbel)
               .SetFontSize(11));

                tableOrder.AddCell(new Paragraph($"{item.Product.ProductDescription}\nПроизводитель:{item.Product.ProductManufacturerNavigation.ProductManufacturer}")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(corbel)
               .SetFontSize(11));

                tableOrder.AddCell(new Paragraph(item.Product.ProductCost.ToString("C"))
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(corbel)
               .SetFontSize(11));

                tableOrder.AddCell(new Paragraph(item.Product.ProductDiscountAmount.ToString() + "%")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(corbel)
               .SetFontSize(11));

                tableOrder.AddCell(new Paragraph(item.Count.ToString())
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(corbel)
               .SetFontSize(11));

                tableOrder.AddCell(new Paragraph(ProductCalculations.CalculateCostWithDiscount(item.Product.ProductCost * item.Count, (float)item.Product.ProductDiscountAmount).ToString("C"))
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(corbel)
               .SetFontSize(11));
            }

            table.AddCell(new Paragraph("Пункт выдачи:")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(corbel)
               .SetFontSize(16));

            table.AddCell(new Paragraph(string.Format("{0}, г. {1}, ул. {2}, д. {3}",
                PickupPoint.Index, PickupPoint.City, PickupPoint.Street, PickupPoint.House))
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(corbel)
               .SetFontSize(16));

            table.AddCell(new Paragraph("Код получения:")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(corbel)
               .SetFontSize(16));

            table.AddCell(new Paragraph(string.Format("{0}", OrderCode))
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(corbel)
               .SetFontSize(16));

            document.Add(table);
            table.Complete();
            document.Add(
                new Paragraph("Состав заказа:")
                .SetFont(corbel)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(22f));
            document.Add(tableOrder);

            var tableFinal = new Table(5, false)
                .SetWidth(UnitValue.CreatePercentValue(40))
                .SetHeight(UnitValue.CreatePercentValue(30))
                .SetMarginTop(40)
                .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT);

                tableFinal.AddCell(new Paragraph("Сумма заказа:")
                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                   .SetFont(corbel)
                   .SetFontSize(16));

                tableFinal.AddCell(new Paragraph(string.Format("{0:C2}", OrderAmmount))
                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                   .SetFont(corbel)
                   .SetFontSize(16));

                tableFinal.AddCell(new Paragraph("Сумма скидки:")
                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                   .SetFont(corbel)
                   .SetFontSize(16));

                tableFinal.AddCell(new Paragraph(string.Format("{0:C}", DiscountAmmount))
                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                   .SetFont(corbel)
                   .SetFontSize(16));

            document.Add(tableFinal);
            document.Close();

            return filename;
        }
    }
}
