using System;
using System.Collections.Generic;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using CarritoElectronicoApp.Models;
using Microsoft.Maui.Storage;
using System.Diagnostics;
using Colors = QuestPDF.Helpers.Colors;




namespace CarritoElectronicoApp.Services;

public class TicketService
{
    public string GenerarTicket(List<Producto> productos, decimal total, string metodoPago)
    {
        

        var numeroOrden = $"ORD-{DateTime.Now:yyyyMMddHHmmss}";
        var fecha = DateTime.Now;

        var fileName = $"Ticket_{numeroOrden}.pdf";
        var filePath = Path.Combine(FileSystem.Current.AppDataDirectory, fileName);

        decimal iva = total * 0.16m;
        decimal subtotal = total - iva;

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);

                page.Header().Column(col =>
                {
                    col.Item().Text("ELECTRO STORE")
                        .FontSize(24)
                        .Bold()
                        .FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);


                    col.Item().Text("Tecnología al mejor precio");
                    col.Item().Text("Av. Tecnología 123, Ciudad");
                    col.Item().Text("Tel: 555-123-4567");
                });

                page.Content().PaddingVertical(20).Column(col =>
                {
                    col.Item().Text($"Orden: {numeroOrden}").Bold();
                    col.Item().Text($"Fecha: {fecha:dd/MM/yyyy HH:mm}");
                    col.Item().Text($"Método de Pago: {metodoPago}");

                    col.Item().LineHorizontal(1);

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(4);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Producto").Bold();
                            header.Cell().AlignCenter().Text("Cant.").Bold();
                            header.Cell().AlignRight().Text("P. Unit").Bold();
                            header.Cell().AlignRight().Text("Subtotal").Bold();
                        });

                        foreach (var p in productos)
                        {
                            table.Cell().Text(p.Nombre);
                            table.Cell().AlignCenter().Text(p.Cantidad.ToString());
                            table.Cell().AlignRight().Text($"${p.Precio:F2}");
                            table.Cell().AlignRight().Text($"${p.Precio * p.Cantidad:F2}");
                        }
                    });

                    col.Item().LineHorizontal(1);

                    col.Item().AlignRight().Column(totalCol =>
                    {
                        totalCol.Item().Text($"Subtotal: ${subtotal:F2}");
                        totalCol.Item().Text($"IVA (16%): ${iva:F2}");
                        totalCol.Item().Text($"TOTAL: ${total:F2}")
                            .FontSize(16)
                            .Bold()
                            .FontColor(Colors.Green.Darken2);
                    });

                    col.Item().PaddingTop(20);
                    col.Item().AlignCenter().Text("¡Gracias por tu compra!")
                        .Italic()
                        .FontSize(14);
                });

                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Generado automáticamente por Electro Store ");
                        text.Span(DateTime.Now.Year.ToString());
                    });
            });
        })
        .GeneratePdf(filePath);

        return filePath;
    }

    public async Task AbrirTicketAsync(string ruta)
    {
        await Launcher.OpenAsync(new OpenFileRequest
        {
            File = new ReadOnlyFile(ruta)
        });
    }

}
