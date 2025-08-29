using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using teste.inbev.core.domain.Models;
using teste.inbev.core.domain.Entities;
using teste.inbev.core.domain.Interface.Application;

namespace teste.inbev.core.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoApplication produtoApplication;        
        private readonly ILogger<ProdutoController> logger;
        public ProdutoController(IProdutoApplication produtoApplication, ILogger<ProdutoController> logger)
        {
            this.produtoApplication = produtoApplication;            
            this.logger = logger;
        }

        [HttpGet]
        [Route("ObterPorId")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var produto = await produtoApplication.ObterPorId(id);                
                return Ok(produto);
            }
            catch (Exception e)
            {
                logger.LogError($"Erro: {e.Message}");
                return BadRequest($"Erro: {e.Message}");
            }
        }

        [HttpGet]
        [Route("ListarTodos")]
        public async Task<IActionResult> ListarTodos()
        {
            try
            {
                var listaProdutos = await produtoApplication.ListarTodosAsync();
                return Ok(listaProdutos);
            }
            catch (Exception e)
            {
                logger.LogError($"Erro: {e.Message}");
                return BadRequest($"Erro: {e.Message}");
            }
        }        

        [HttpPost]
        [Route("Inserir")]
        public async Task<IActionResult> Inserir([FromBody] ProdutoModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var produto = await produtoApplication.InserirAsync(model);
                    return CreatedAtAction(nameof(ObterPorId), new { id = produto.Id }, produto);
                }
                else
                {
                    logger.LogError($"Erro: Modelo informado inválido! {model}");
                    return BadRequest($"Modelo informado inválido! {model}");
                }
            }
            catch (Exception e)
            {
                logger.LogError($"Erro: {e.Message}");
                return BadRequest($"Erro: {e.Message}");
            }
        }

        [HttpPut]
        [Route("Atualizar")]
        public async Task<IActionResult> Atualizar([FromBody] ProdutoModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entidade = await produtoApplication.AtualizarAsync(model);

                    return NoContent(); 
                }
                else
                {
                    logger.LogError($"Modelo informado inválido! {model}");
                    return BadRequest($"Modelo informado inválido! {model}");
                }
            }
            catch (Exception e)
            {
                logger.LogError($"Erro: {e.Message}");
                return BadRequest($"Erro: {e.Message}");
            }
        }

        [HttpDelete]
        [Route("Deletar")]
        public async Task<IActionResult> Deletar([FromQuery] int id)
        {
            try
            {
                var retorno = await produtoApplication.DeleteAnsync(id);
                return NoContent(); 
                
            }
            catch (Exception e)
            {
                logger.LogError($"Erro: {e.Message}");
                return BadRequest($"Erro: {e.Message}");
            }
        }
    }
}
