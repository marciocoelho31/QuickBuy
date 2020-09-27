using Microsoft.AspNetCore.Mvc;
using QuickBuy.Dominio.Entidades;
using System;

namespace QuickBuy.Web.Controllers
{
    [Route("api/[Controller]")]
    public class UsuarioController : Controller
    {

        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        [HttpPost]
        public ActionResult Post()
        {
            try
            {
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        [HttpPost("VerificarUsuario")]
        public ActionResult Post([FromBody]Usuario usuario)
        {
            try
            {
                if (usuario.Email == "marcio@teste.com" && usuario.Senha == "xxx")
                    return Ok(usuario);
                
                return BadRequest("Usuário ou senha inválido");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }

    }
}
