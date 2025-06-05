using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using MultasTransito2.Models;
using MultasTransito2.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using static System.Net.Mime.MediaTypeNames;
using Font = iTextSharp.text.Font;
using Image = iTextSharp.text.Image;
using Paragraph = iTextSharp.text.Paragraph;

namespace MultasTransito2.Services
{
    public class ReporteMultas
    {
        public byte[] GetReporteMultas(List<Multa> datosMultas)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Archivos de Pdf|*.pdf";
            save.Title = "Guardar archivo";
            save.FileName = "ReporteMultas.pdf";
            save.ShowDialog();
            var path = string.Empty;
            path = save.FileName;


            MemoryStream archivoMemoria = new MemoryStream();
            iTextSharp.text.Document documentoPDF = new iTextSharp.text.Document(PageSize.LETTER);
            PdfWriter.GetInstance(documentoPDF, new FileStream(path, FileMode.Create));

            documentoPDF.Open();

            Image logo = Image.GetInstance("C:\\Users\\ramir\\Tec\\programacion\\sexto\\Base de Datos\\MultasTransito2\\Resources\\TransitoLogo.png");
            logo.ScaleToFit(80f, 80f);
            logo.SetAbsolutePosition(35f, 640f);
            documentoPDF.Add(logo);


            Font fuenteTitulo = FontFactory.GetFont("Arial", 25, Font.BOLD);
            Font fuenteSubtitulo = FontFactory.GetFont("Arial", 20);
            Font fuenteTexto = FontFactory.GetFont("Arial", 14);
            Font fuenteEncabezadoTabla = FontFactory.GetFont("Arial", 14);
            Font fuenteCeldaTabla = FontFactory.GetFont("Arial", 12);


            documentoPDF.Add(new Paragraph("Reporte de multas de tránsito de Sabinas, Coahuila \n\n", fuenteTitulo)
            {
                Alignment = Element.ALIGN_CENTER,
            });

            documentoPDF.Add(new Paragraph(DateTime.Now.ToString("hh:mm tt  dd/MMM/yyyy"), fuenteCeldaTabla)
            {
                Alignment = Element.ALIGN_RIGHT
            });



            documentoPDF.Add(new Paragraph("\n"));

            documentoPDF.Add(new Paragraph("Datos generales: \n\n", fuenteSubtitulo)
            { Alignment = Element.ALIGN_LEFT, });


            PdfPTable tabla = new PdfPTable(4);
            tabla.WidthPercentage = 100;
            tabla.SetWidths(new float[] {1.5f, 1.5f, 5f, 2f});
            tabla.HorizontalAlignment = Element.ALIGN_CENTER;


            PdfPCell encabezadoNumero = new PdfPCell(new Phrase("No.", fuenteEncabezadoTabla));
            PdfPCell encabezadoFecha = new PdfPCell(new Phrase("Fecha", fuenteEncabezadoTabla));
            PdfPCell encabezadoMotivo = new PdfPCell(new Phrase("Motivo", fuenteEncabezadoTabla));
            PdfPCell encabezadoSancion = new PdfPCell(new Phrase("Sanción", fuenteEncabezadoTabla));


            encabezadoNumero.BackgroundColor = BaseColor.LIGHT_GRAY;
            encabezadoFecha.BackgroundColor = BaseColor.LIGHT_GRAY;
            encabezadoMotivo.BackgroundColor = BaseColor.LIGHT_GRAY;
            encabezadoSancion.BackgroundColor = BaseColor.LIGHT_GRAY;

            encabezadoNumero.HorizontalAlignment = Element.ALIGN_CENTER;
            encabezadoFecha.HorizontalAlignment = Element.ALIGN_CENTER;
            encabezadoMotivo.HorizontalAlignment = Element.ALIGN_CENTER;
            encabezadoSancion.HorizontalAlignment = Element.ALIGN_CENTER;

            tabla.AddCell(encabezadoNumero);
            tabla.AddCell(encabezadoFecha);
            tabla.AddCell(encabezadoMotivo);
            tabla.AddCell(encabezadoSancion);



            int totalrenglones = 1;

            foreach (var m in datosMultas)
            {
                tabla.AddCell(new PdfPCell(new Phrase(totalrenglones.ToString()))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });


                PdfPCell fecha = tabla.AddCell(new PdfPCell(new Phrase(m.Fecha.ToString())));
                PdfPCell motivo = tabla.AddCell(new PdfPCell(new Phrase(m.Motivo)));
                PdfPCell sansion = tabla.AddCell(new PdfPCell(new Phrase(m.SancionPecuniaria.ToString("0.00"))));


                fecha.HorizontalAlignment = Element.ALIGN_CENTER;
                motivo.HorizontalAlignment = Element.ALIGN_CENTER;
                sansion.HorizontalAlignment = Element.ALIGN_CENTER;

                totalrenglones++;

            }

            documentoPDF.Add(tabla);
            documentoPDF.Close();
            return archivoMemoria.ToArray();
        }

    }
}

