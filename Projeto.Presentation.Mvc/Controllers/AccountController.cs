using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Projeto.Infra.Data.Entities;
using Projeto.Infra.Data.Repositories;
using Projeto.Presentation.Mvc.Models;
using System;
using System.Security.Claims;

namespace Projeto.Presentation.Mvc.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AccountLoginModel model, [FromServices] UsuarioRepository usuarioRepository) 
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var login = usuarioRepository.GetByEmailAndSenha(model.Email, model.Senha);

                    if (login != null) 
                    {
                        var identity = new ClaimsIdentity(
                            new[] { new Claim(ClaimTypes.Name, login.Email) },
                            CookieAuthenticationDefaults.AuthenticationScheme);

                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(identity));

                        return RedirectToAction("Index", "Agenda");
                    }
                    else
                    {
                        throw new Exception("Email e/ou senha incorreto. Acesso negado. ");
                    }
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = ex.Message;
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult Register(AccountRegisterModel model, [FromServices] UsuarioRepository usuarioRepository)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (usuarioRepository.GetByEmail(model.Email) != null)
                    {
                        throw new Exception("Email encontra-se cadastrado. ");
                    }

                    var usuario = new Usuario();
                    usuario.Nome = model.Nome;
                    usuario.Email = model.Email;
                    usuario.Senha = model.Senha;
                    usuario.DataCriacao = DateTime.Now;

                    usuarioRepository.Create(usuario);

                    TempData["MensagemSucesso"] = $"Usuário(a) {usuario.Nome}, cadastrado(a) com sucesso.";
                    ModelState.Clear();
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = ex.Message;
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");            
        }
    }
}
