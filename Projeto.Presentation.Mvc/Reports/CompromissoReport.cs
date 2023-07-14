using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Projeto.Infra.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using iTextSharp.text.pdf.codec.wmf;

namespace Projeto.Presentation.Mvc.Reports
{
    public class CompromissoReport
    {
        public byte[] ObterRelatorioCompromissos(List<Compromisso> compromissos, Usuario usuario)
        {
            var html = new StringBuilder();

            #region Relatório em HTML

            html.Append("<html>");
            html.Append("<body>");

            html.Append("<h1 style='color: #4682B4'><strong> Relatório de Compromissos </ strong ></ h1 > ");
            html.Append($"<p style='font-size: 10pt;'>Documento gerado em: { DateTime.Now.ToString("dd/MM/yyyy HH:mm")}</ p > ");
            html.Append($"<p style='font-size: 10pt;'>Usuário da Agenda:<strong style='color: #4682B4' >{usuario.Nome}</ strong ></ p > ");
            html.Append($"<p style='font-size: 10pt;'>Email:<strong style='color: #4682B4' >{usuario.Email}</ strong ></ p > ");
            html.Append($"<p style='font-size: 10pt;'>Quantidade de compromissos:{compromissos.Count}</ p > ");
            html.Append("<br/>");

            html.Append("<table>");
            html.Append("<tr>");
            html.Append("<th bgcolor='#708090' style='font-size: 10pt; color: #FFF'><strong>Compromisso</strong></th>");
            html.Append("<th bgcolor='#708090' style='font-size: 10pt; color: #FFF'><strong>Data/Hora<br/>de Início</strong></th>");
            html.Append("<th bgcolor='#708090' style='font-size: 10pt; color: #FFF'><strong>Data/Hora<br/>de Término</strong></th>");
            html.Append("<th bgcolor='#708090' style='font-size: 10pt; color: #FFF'><strong>Categoria</strong></th>");
            html.Append("</tr>");

            var i = 1;

            foreach (var item in compromissos)
            {
                var rowColor = string.Empty;

                if (i % 2 == 0)
                {
                    rowColor = "#DCDCDC";
                }
                else
                {
                    rowColor = "#FAFAFA";
                }

                html.Append("<tr>");
                html.Append($"<td bgcolor={rowColor} style='font-size: 10pt; '>{item.Titulo}</td>");
                html.Append($"<td bgcolor={rowColor} style='font-size: 10pt; '>{item.DataInicio.ToString("dd/MM/yyyy")}<br/>às {item.HoraInicio.ToString(@"hh\:mm")}</td>");
                html.Append($"<td bgcolor={rowColor} style='font-size: 10pt; '>{item.DataFim.ToString("dd/MM/yyyy")}<br/>às {item.HoraFim.ToString(@"hh\:mm")}</td>");
                html.Append($"<td bgcolor={rowColor} style='font-size: 10pt; '>{item.Categoria.ToString()}</td>");
                html.Append("</tr>");
                i++;
            }

            html.Append("</table>");

            html.Append("</body>");
            html.Append("</html>");

            #region Gerar o documento PDF do relatório

            byte[] pdf = null;

            MemoryStream ms = new MemoryStream();
            TextReader reader = new StringReader(html.ToString());

            iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4, 40, 40, 40, 40);

            PdfWriter writer = PdfWriter.GetInstance(doc, ms);
            HTMLWorker htmlWorker = new HTMLWorker(doc);

            doc.Open();
            htmlWorker.StartDocument();
            htmlWorker.Parse(reader);

            htmlWorker.EndDocument();
            htmlWorker.Close();
            doc.Close();

            pdf = ms.ToArray();
            return pdf;

            #endregion           

            #endregion

        }
    }

}
