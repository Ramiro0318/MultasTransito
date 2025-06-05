using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using MultasTransito2.Models;
using MultasTransito2.Models.DTOs;
using MultasTransito2.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using static System.Net.Mime.MediaTypeNames;
using Font = iTextSharp.text.Font;
using Image = iTextSharp.text.Image;
using Paragraph = iTextSharp.text.Paragraph;

namespace MultasTransito2.Services
{
    public class ReporteCiudadanos
    {
        public byte[] GetReporteSecciones(List<MultasPorCiudadanoDTO> ciudadano)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Archivos de Pdf|*.pdf";
            save.Title = "Guardar archivo";
            save.FileName = "ReporteCiudadanos.pdf";
            save.ShowDialog();
            var path = string.Empty;
            path = save.FileName;


            MemoryStream archivoMemoria = new MemoryStream();
            Document documentoPDF = new Document(PageSize.LETTER);
            //PdfWriter.GetInstance(documentoPDF, archivoMemoria);
            PdfWriter writer = PdfWriter.GetInstance(documentoPDF, new FileStream(path, FileMode.Create));

            documentoPDF.Open();

            Image logo = Image.GetInstance("C:\\Users\\ramir\\Tec\\programacion\\sexto\\Base de Datos\\MultasTransito2\\Resources\\TransitoLogo.png");
            logo.ScaleToFit(80f, 80f);

            logo.SetAbsolutePosition(12f, 691f);
            documentoPDF.Add(logo);


            PdfContentByte footer = writer.DirectContent;

            Font fuenteTitulo = FontFactory.GetFont("Arial", 20f, Font.BOLD);
            Font fuenteCelda = FontFactory.GetFont("Arial", 12f);
            Font fuenteCeldaBold = FontFactory.GetFont("Arial", 12f, Font.BOLD);

            documentoPDF.Add(new Paragraph("Reporte de multas de transito por usuario", fuenteTitulo)
            {
                Alignment = Element.ALIGN_CENTER
            });

            documentoPDF.Add(new Paragraph("\n\n"));

            documentoPDF.Add(new Paragraph((DateOnly.FromDateTime(DateTime.Now).ToString("dddd', 'MMMM' 'd'th 'yyyy")))
            {
                Alignment = Element.ALIGN_RIGHT
            });

            documentoPDF.Add(new Paragraph("\n"));


            ColumnText.ShowTextAligned(
                footer,
                Element.ALIGN_CENTER,
                new Phrase("Reporte de multas por ciudadano", fuenteCelda),
                (documentoPDF.PageSize.Left + documentoPDF.PageSize.Right) / 2,
                documentoPDF.BottomMargin / 2,
                0
            );



            foreach (var c in ciudadano)
            {

                PdfPTable? tabla = new PdfPTable(2);
                tabla.WidthPercentage = 100;
                tabla.SetWidths(new float[] { 7f, 3f });

                PdfPCell encabezadoCantidadEmpleados = new PdfPCell(new Phrase("Nombre completo", fuenteCelda));
                PdfPCell encabezadoMontoPagado = new PdfPCell(new Phrase("No. Licencia", fuenteCelda));

                encabezadoCantidadEmpleados.BackgroundColor = BaseColor.LIGHT_GRAY;
                encabezadoMontoPagado.BackgroundColor = BaseColor.LIGHT_GRAY;
                encabezadoMontoPagado.HorizontalAlignment = Element.ALIGN_CENTER;
                encabezadoCantidadEmpleados.HorizontalAlignment = Element.ALIGN_CENTER;

                encabezadoMontoPagado.VerticalAlignment = Element.ALIGN_CENTER;
                encabezadoCantidadEmpleados.VerticalAlignment = Element.ALIGN_CENTER;

                encabezadoCantidadEmpleados.PaddingTop = 5;
                encabezadoMontoPagado.PaddingTop = 5;

                encabezadoCantidadEmpleados.PaddingBottom = 7.5f;
                encabezadoMontoPagado.PaddingBottom = 7.5f;

                tabla.AddCell(encabezadoCantidadEmpleados);
                tabla.AddCell(encabezadoMontoPagado);

                tabla.AddCell(new PdfPCell(new Phrase(c.NombreCiudadano, fuenteCeldaBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,

                    Padding = 10
                });
                tabla.AddCell(new PdfPCell(new Phrase(c.NumeroLicenca))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    Padding = 10
                });


                var multasAgrupadas = ciudadano.GroupBy(m => m.NumeroLicenca); //


                if (c.Multas.Count() > 0) //Cantidad de multas
                {
                    documentoPDF.Add(tabla);

                    int numero = 1;
                    foreach (var m in c.Multas)
                    {
                        PdfPTable? tabla2 = new PdfPTable(3);
                        tabla2.WidthPercentage = 100;
                        tabla2.SetWidths(new float[] { 2f, 6f, 2f });



                        if (numero == 1)
                        {

                            PdfPCell encabezadoNumero = new PdfPCell(new Phrase("Fecha de multa", fuenteCelda));
                            PdfPCell encabezadoNombre = new PdfPCell(new Phrase("Motivo", fuenteCelda));
                            PdfPCell encabezadoSueldo = new PdfPCell(new Phrase("Sanción Pecuniaria", fuenteCelda));

                            encabezadoNumero.BackgroundColor = BaseColor.LIGHT_GRAY;
                            encabezadoNombre.BackgroundColor = BaseColor.LIGHT_GRAY;
                            encabezadoSueldo.BackgroundColor = BaseColor.LIGHT_GRAY;

                            encabezadoNumero.HorizontalAlignment = Element.ALIGN_CENTER;
                            encabezadoNombre.HorizontalAlignment = Element.ALIGN_CENTER;
                            encabezadoSueldo.HorizontalAlignment = Element.ALIGN_CENTER;

                            encabezadoNumero.VerticalAlignment = Element.ALIGN_CENTER;
                            encabezadoNombre.VerticalAlignment = Element.ALIGN_CENTER;
                            encabezadoSueldo.VerticalAlignment = Element.ALIGN_CENTER;

                            encabezadoNumero.PaddingTop = 5;
                            encabezadoNombre.PaddingTop = 5;
                            encabezadoSueldo.PaddingTop = 5;

                            encabezadoNumero.PaddingBottom = 7.5f;
                            encabezadoNombre.PaddingBottom = 7.5f;
                            encabezadoSueldo.PaddingBottom = 7.5f;

                            tabla2.AddCell(encabezadoNumero);
                            tabla2.AddCell(encabezadoNombre);
                            tabla2.AddCell(encabezadoSueldo);
                        }



                        tabla2.AddCell(new PdfPCell(new Phrase(m.Fecha.ToString("dd/MMM/yyyy"), fuenteCelda))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            Padding = 10
                        });
                        tabla2.AddCell(new PdfPCell(new Phrase(m.Motivo, fuenteCelda))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            Padding = 10
                        });
                        tabla2.AddCell(new PdfPCell(new Phrase(m.SancionPecuniaria.ToString("$0.00"), fuenteCelda))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            Padding = 10
                        });

                        numero++;
                        documentoPDF.Add(tabla2);
                        tabla2 = null;
                    }

                }

                documentoPDF.Add(new Paragraph("\n"));
                //documentoPDF.Add(tabla);


                tabla = null;

            }

            documentoPDF.Add(new Paragraph("\n"));

            documentoPDF.Close();
            return archivoMemoria.ToArray();
        }
    }

}
