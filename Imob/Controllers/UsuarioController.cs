﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Imob.Models;

namespace Imob.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public UsuarioController(Context context,
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Usuario
        public async Task<IActionResult> ListaUsuarios()
        {
            return View(await _context.UsuarioLogado.ToListAsync());
        }
        // GET: Usuario/Create
        public IActionResult CadastrarUsuario()
        {
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CadastrarUsuario([Bind("Email,Senha,Id,CriadoEm,ConfirmacaoSenha")] UsuarioLogado usuarioLogado)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = new Usuario
                {
                    UserName = usuarioLogado.Email,
                    Email = usuarioLogado.Email
                };

                IdentityResult result = await _userManager.CreateAsync(usuario, usuarioLogado.Senha);
                var token = _userManager.GenerateEmailConfirmationTokenAsync(usuario);
                if (result.Succeeded)
                {
                    _context.Add(usuarioLogado);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ListaUsuarios));
                }
                AdicionarErros(result);
            }
            return View(usuarioLogado);
        }

        public void AdicionarErros(IdentityResult result)
        {
            foreach (IdentityError erro in result.Errors)
            {
                ModelState.AddModelError("", erro.Description);
            }
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email, Senha")] UsuarioLogado usuarioLogado)
        {
            var result = await _signInManager.PasswordSignInAsync(usuarioLogado.Email, usuarioLogado.Senha, false, false);

            var name = User.Identity.Name;
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Produto"); // CORRIGIR APONTAMENTO DE PÁGINA
            }
            ModelState.AddModelError("", "Login ou senha inválidos!");
            return View(usuarioLogado);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("InicialMenu", "Home");
        }
    }
}