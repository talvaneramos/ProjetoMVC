using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projeto.Infra.Data.Entities;
using Projeto.Infra.Data.Repositories;
using Projeto.Presentation.Mvc.Models;
using Projeto.Presentation.Mvc.Reports;
using System;
using System.Collections.Generic;

namespace Projeto.Presentation.Mvc.Controllers
{
    [Authorize]
    public class AgendaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CadastroCompromisso()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastroCompromisso(CompromissoCadastroModel model,
            [FromServices] UsuarioRepository usuarioRepository,
            [FromServices] CompromissoRepository compromissoRepository)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usuario = usuarioRepository.GetByEmail(User.Identity.Name);

                    var compromisso = new Compromisso();
                    compromisso.Titulo = model.Titulo;
                    compromisso.Descricao = model.Descricao;
                    compromisso.DataInicio = DateTime.Parse(model.DataInicio);
                    compromisso.DataFim = DateTime.Parse(model.DataFim);
                    compromisso.HoraInicio = TimeSpan.Parse(model.HoraInicio);
                    compromisso.HoraFim = TimeSpan.Parse(model.HoraFim);   
                    compromisso.IdUsuario = usuario.IdUsuario;
                    compromisso.Categoria = model.Categoria;

                    compromissoRepository.Create(compromisso);

                    TempData["MensagemSucesso"] = $"Compromisso {compromisso.Titulo} agendado com sucessso";
                    ModelState.Clear();
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = ex.Message;
                }
            }
            return View();
        }
        public IActionResult ConsultaCompromisso()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ConsultaCompromisso(string dataMin, string dataMax,
            [FromServices] UsuarioRepository usuarioRepository,
            [FromServices] CompromissoRepository compromissoRepository)
        {
            var lista = new List<Compromisso>();

            try
            {

                if (!string.IsNullOrEmpty(dataMin) && !string.IsNullOrEmpty(dataMax))
                {

                    var usuario = usuarioRepository.GetByEmail(User.Identity.Name);

                    lista = compromissoRepository.GetByDatas(DateTime.Parse(dataMin), DateTime.Parse(dataMax), usuario.IdUsuario);

                    TempData["DataMin"] = dataMin;
                    TempData["DataMax"] = dataMax;

                }
                else
                {
                    TempData["MensagemErro"] = "Por favor, informe o período de datas para a consulta. ";
                }               
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro: " + ex.Message;
            }

            return View(lista);
        }

        public IActionResult EdicaoCompromisso(int id,
            [FromServices] UsuarioRepository usuarioRepository,
            [FromServices] CompromissoRepository compromissoRepository)
        {
            var model = new CompromissoEdicaoModel();

            try
            {
                var compromisso = compromissoRepository.GetById(id);
                var usuario = usuarioRepository.GetByEmail(User.Identity.Name);

                if (compromisso.IdUsuario == usuario.IdUsuario)
                {
                    model.IdCompromisso = compromisso.IdCompromisso;
                    model.Titulo = compromisso.Titulo;
                    model.Descricao = compromisso.Descricao;
                    model.DataInicio = compromisso.DataInicio.ToString("dd/MM/yyyy");
                    model.DataFim = compromisso.DataFim.ToString("dd/MM/yyyy");
                    model.HoraInicio = compromisso.HoraInicio.ToString(@"hh\:mm");
                    model.HoraFim = compromisso.HoraFim.ToString(@"hh\:mm");
                    model.Categoria = compromisso.Categoria;
                }
                else
                {
                    return RedirectToAction("ConsultaCompromisso");
                }

            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro: " + ex.Message;
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult EdicaoCompromisso(CompromissoEdicaoModel model,            
            [FromServices] CompromissoRepository compromissoRepository)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var compromisso = new Compromisso();
                    compromisso.IdCompromisso = model.IdCompromisso;
                    compromisso.Titulo = model.Titulo;
                    compromisso.Descricao = model.Descricao;
                    compromisso.DataInicio = DateTime.Parse(model.DataInicio);
                    compromisso.HoraInicio = TimeSpan.Parse(model.HoraInicio);
                    compromisso.DataFim = DateTime.Parse(model.DataFim);                    
                    compromisso.HoraFim = TimeSpan.Parse(model.HoraFim);
                    compromisso.Categoria = model.Categoria;
                    

                    compromissoRepository.Update(compromisso);

                    TempData["MensagemSucesso"] = $"Compromisso {compromisso.Titulo} atualizado com sucessso";
                    
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = ex.Message;
                }
            }
            return View();
        }


        public IActionResult ExclusaoCompromisso(int id,
            [FromServices] UsuarioRepository usuarioRepository,
            [FromServices] CompromissoRepository compromissoRepository)
        {
            try
            {
                var compromisso = compromissoRepository.GetById(id);

                var usuario = usuarioRepository.GetByEmail(User.Identity.Name);

                if (compromisso.IdUsuario == usuario.IdUsuario)
                {
                    compromissoRepository.Delete(compromisso);
                    TempData["MensagemSucesso"] = "Compromisso excluído com sucesso. ";
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
            }

            return RedirectToAction("ConsultaCompromisso");
        }

        public void GerarRelatorio([FromServices] CompromissoRepository compromissoRepository,
            [FromServices] UsuarioRepository usuarioRepository)
        {
            try
            {
                var compromissos = compromissoRepository.GetAll();

                var usuario = usuarioRepository.GetByEmail(User.Identity.Name);

                var compromissoReport = new CompromissoReport();
                var pdf = compromissoReport.ObterRelatorioCompromissos(compromissos, usuario);

                var nomeArquivo = $"relatorio_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.pdf";

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.Headers.Add("content-disposition", $"attachment; filename={nomeArquivo}");
                Response.Body.WriteAsync(pdf, 0, pdf.Length);
                Response.Body.Flush();
                Response.StatusCode = StatusCodes.Status200OK;

            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
            }
        }

        public JsonResult ObterDadosGrafico([FromServices] CompromissoRepository compromissoRepository,
            [FromServices] UsuarioRepository usuarioRepository)
        {
            try
            {
                var usuario = usuarioRepository.GetByEmail(User.Identity.Name);

                var compromissos = compromissoRepository.GetResumoCategoria(usuario.IdUsuario);

                var result = new List<HighChartsModel>();

                foreach (var item in compromissos)
                {
                    var model = new HighChartsModel();
                    model.name = item.Categoria.ToString();
                    model.data = new List<int>() { item.Quantidade };

                    result.Add(model);

                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
                
            }
        }
    }
}
