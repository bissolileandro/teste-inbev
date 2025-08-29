using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using teste.inbev.core.application.Applications;
using teste.inbev.core.domain.Interface.Application;
using teste.inbev.core.domain.Models;

namespace teste.inbev.core.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrcamentoController : ControllerBase
    {
        private readonly IOrcamentoApplication orcamentoApplication;
        private readonly ILogger<OrcamentoController> logger;
        public OrcamentoController(IOrcamentoApplication orcamentoApplication, ILogger<OrcamentoController> logger)
        {
            this.orcamentoApplication = orcamentoApplication;
            this.logger = logger;
        }

        [HttpGet]
        [Route("ObterPorId")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var produto = await orcamentoApplication.ObterPorId(id);
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
                var listOrcamentos = await orcamentoApplication.ListarTodosAsync();
                return Ok(listOrcamentos);
            }
            catch (Exception e)
            {
                logger.LogError($"Erro: {e.Message}");
                return BadRequest($"Erro: {e.Message}");
            }
        }

        [HttpPost]
        [Route("Inserir")]
        public async Task<IActionResult> Inserir([FromBody] OrcamentoRequestModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var orcamento = await orcamentoApplication.InserirAsync(model);
                    return CreatedAtAction(nameof(ObterPorId), new { id = orcamento.Id }, orcamento);
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
        public async Task<IActionResult> Atualizar([FromBody] OrcamentoRequestModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entidade = await orcamentoApplication.AtualizarAsync(model);

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
                var retorno = await orcamentoApplication.DeleteAnsync(id);
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
