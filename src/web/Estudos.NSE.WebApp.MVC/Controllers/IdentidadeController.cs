﻿using System.Threading.Tasks;
using Estudos.NSE.WebApp.MVC.Extensions;
using Estudos.NSE.WebApp.MVC.Models;
using Estudos.NSE.WebApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.NSE.WebApp.MVC.Controllers
{
    public class IdentidadeController : MainController
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public IdentidadeController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet]
        [Route("nova-conta")]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return View(usuarioRegistro);

            var resposta = await _autenticacaoService.Registro(usuarioRegistro);
            if (ResponsePossuiErros(resposta.ResponseResult)) return View(usuarioRegistro);

            await _autenticacaoService.RealizarLogin(resposta);
            return RedirectToAction("Index", "Catalogo");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData.SetReturnUrl(returnUrl);
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UsuarioLogin usuarioLogin, string returnUrl = null)
        {
            ViewData.SetReturnUrl(returnUrl);
            if (!ModelState.IsValid) return View(usuarioLogin);

            var resposta = await _autenticacaoService.Login(usuarioLogin);
            if (ResponsePossuiErros(resposta.ResponseResult)) return View(usuarioLogin);

            await _autenticacaoService.RealizarLogin(resposta);

            if (!string.IsNullOrEmpty(returnUrl)) return LocalRedirect(returnUrl);

            return RedirectToAction("Index", "Catalogo");
        }

        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> Logout()
        {
            await _autenticacaoService.Logout();
            return RedirectToAction("Index", "Catalogo");
        }
    }
}