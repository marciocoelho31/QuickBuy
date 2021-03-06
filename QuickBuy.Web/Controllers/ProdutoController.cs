﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickBuy.Dominio.Contratos;
using QuickBuy.Dominio.Entidades;
using System;
using System.IO;
using System.Linq;

namespace QuickBuy.Web.Controllers
{
    [Route("api/[controller]")]
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProdutoController(IProdutoRepositorio produtoRepositorio,
            IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _produtoRepositorio = produtoRepositorio;
            _httpContextAccessor = httpContextAccessor; // acesso ao contexto da requisição
            _hostingEnvironment = hostingEnvironment;   // acesso a raiz onde o site está sendo executado, para colocar o arquivo que será enviado via upload na tela de produto
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Json(_produtoRepositorio.ObterTodos());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Produto produto)
        {
            try
            {
                produto.Validate();
                if (!produto.EhValido)
                {
                    return BadRequest(produto.ObterMensagensDeValidacao());
                }

                if (produto.Id > 0)
                {
                    _produtoRepositorio.Atualizar(produto);
                }
                else
                {
                    _produtoRepositorio.Adicionar(produto);
                }

                return Created("api/produto", produto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost("Excluir")]
        public IActionResult Excluir([FromBody] Produto produto)
        {
            try
            {
                _produtoRepositorio.Remover(produto);
                return Json(_produtoRepositorio.ObterTodos());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost("EnviarArquivo")]
        public IActionResult EnviarArquivo()
        {
            try
            {
                var formFile = _httpContextAccessor.HttpContext.Request.Form.Files["arquivoEnviado"];
                var nomeArquivo = formFile.FileName;
                var extensao = nomeArquivo.Split(".").Last();

                string novoNomeArquivo = GerarNovoNomeArquivo(nomeArquivo, extensao);

                // referencia a subpasta em wwwroot
                var pastaArquivos = _hostingEnvironment.WebRootPath + "\\arquivos\\";
                var nomeCompleto = pastaArquivos + novoNomeArquivo;

                using (var streamArquivo = new FileStream(nomeCompleto, FileMode.Create))
                {
                    formFile.CopyTo(streamArquivo);
                }

                return Json(novoNomeArquivo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        private static string GerarNovoNomeArquivo(string nomeArquivo, string extensao)
        {
            var arrayNomeReduzido = Path.GetFileNameWithoutExtension(nomeArquivo)
                .Take(10).ToArray();
            var novoNomeArquivo = new string(arrayNomeReduzido).Replace(" ", "-");
            novoNomeArquivo = $"{novoNomeArquivo}_{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}.{extensao}";
            return novoNomeArquivo;
        }
    }
}
