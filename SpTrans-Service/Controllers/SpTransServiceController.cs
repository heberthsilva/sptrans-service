using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceReference;
using SpTrans_Service.Model;

namespace SpTrans_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpTransServiceController : ControllerBase
    {
        private readonly ILogger<SpTransServiceController> _logger;
        private readonly IConfiguration _configuration;
        private readonly PedidoCreditoService _pedidoCreditoService;

        public SpTransServiceController(
            ILogger<SpTransServiceController> logger,
            IConfiguration configuration,            
            PedidoCreditoService pedidoCreditoService
            )
        {
            _logger = logger;
            _configuration = configuration;
            _pedidoCreditoService = pedidoCreditoService;
        }

        [HttpGet("ConsultarCartao/{numeroCartao}")]
        public consultarCartaoOut ConsultarCartao(long numeroCartao)
        {
            consultarCartaoOut consultaCartao = new consultarCartaoOut();
            try
            {
                consultaCartao.mensagem = "inicio Consulta Cartão";
                keyAutenticacaoIn login = new keyAutenticacaoIn();
                login.usuario = _configuration.GetValue<string>("SpTransLoggin:User");
                login.senha = _configuration.GetValue<string>("SpTransLoggin:Password");

                consultarCartao request = new consultarCartao();
                request.login = login;
                request.numeroCartao = numeroCartao;
                consultaCartao.mensagem = consultaCartao.mensagem + "| iniciando request";
                consultaCartao = _pedidoCreditoService.consultarCartaoAsync(request).Result.@return;
               

            }
            catch (Exception ex)
            {
                consultaCartao.mensagem = consultaCartao.mensagem + " Erro: " + ex.Message + " - " + ex.StackTrace;
            }
            return consultaCartao;
        }

        [HttpPost("GerarPedido")]
        public gerarPedidoOut GerarPedido(PedidoInput pedidoInput)
        {
            keyAutenticacaoIn login = new keyAutenticacaoIn();
            login.usuario = _configuration.GetValue<string>("SpTransLoggin:User");
            login.senha = _configuration.GetValue<string>("SpTransLoggin:Password");
            
            gerarPedidoIn gerarPedidoIn = new gerarPedidoIn();


            gerarPedidoIn.codigoProduto = pedidoInput.codigoProduto;
            gerarPedidoIn.codigoProdutoSpecified = pedidoInput.codigoProdutoEspecifico;
            gerarPedidoIn.codigoTipoCredito = pedidoInput.codigoTipoCredito;
            gerarPedidoIn.codigoTipoCreditoSpecified = pedidoInput.codigoTipoCreditoEspecifico;
            gerarPedidoIn.numeroLogicoCartao = pedidoInput.numeroLogicoCartao;
            gerarPedidoIn.numeroLogicoCartaoSpecified = pedidoInput.numeroLogicoCartaoEspecifico;
            gerarPedidoIn.quantidade = pedidoInput.quantidade;
            gerarPedidoIn.quantidadeSpecified = pedidoInput.quantidadeEspecifico;
            gerarPedidoIn.valorTotal = pedidoInput.valorTotal;
            gerarPedidoIn.valorTotalSpecified = pedidoInput.valorTotalEspecifico;

            if (pedidoInput.codigoProduto != null)
            { gerarPedidoIn.codigoProdutoSpecified = true; }

            if (pedidoInput.codigoTipoCredito != null)
            { gerarPedidoIn.codigoTipoCreditoSpecified = true; }

            if (pedidoInput.numeroLogicoCartao != null)
            { gerarPedidoIn.numeroLogicoCartaoSpecified = true; }

            if (pedidoInput.quantidade == 0)
            { gerarPedidoIn.quantidadeSpecified = false; }
            else
            {
             gerarPedidoIn.quantidadeSpecified = true;
            }
            if (pedidoInput.valorTotal != null)
            { gerarPedidoIn.valorTotalSpecified = true; }

            gerarPedido request = new gerarPedido();
            request.login = login;
            request.gerarPedidoIn = gerarPedidoIn;
            
            var gerarPedido = _pedidoCreditoService.gerarPedidoAsync(request).Result;
            var serialized = JsonConvert.SerializeObject(request.gerarPedidoIn);
            gerarPedido.@return.mensagem = gerarPedido.@return.mensagem + "request:" + serialized;

            return gerarPedido.@return;
        }

        [HttpGet("ConfirmarPedido/{numeroPedido}")]
        public mensagemOut ConfirmarPedido(long numeroPedido)
        {
            keyAutenticacaoIn login = new keyAutenticacaoIn();
            login.usuario = _configuration.GetValue<string>("SpTransLoggin:User");
            login.senha = _configuration.GetValue<string>("SpTransLoggin:Password");

            confirmarPedido request = new confirmarPedido();
            request.login = login;
            request.numeroPedido = numeroPedido;

            var confirmarPedido = _pedidoCreditoService.confirmarPedidoAsync(request).Result;

            return confirmarPedido.@return;
        }

        [HttpGet("CancelarPedido/{numeroPedido}")]
        public mensagemOut CancelarPedido(long numeroPedido)
        {
            keyAutenticacaoIn login = new keyAutenticacaoIn();
            login.usuario = _configuration.GetValue<string>("SpTransLoggin:User");
            login.senha = _configuration.GetValue<string>("SpTransLoggin:Password");

            cancelarPedido request = new cancelarPedido();
            request.login = login;
            request.numeroPedido = numeroPedido;

            var cancelarPedido = _pedidoCreditoService.cancelarPedidoAsync(request).Result;

            return cancelarPedido.@return;
        }

    }
}