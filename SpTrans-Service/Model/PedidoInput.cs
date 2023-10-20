namespace SpTrans_Service.Model
{
    public class PedidoInput
    {
        public int codigoProduto { get; set; }

        public bool codigoProdutoEspecifico { get; set; }

        public int codigoTipoCredito { get; set; }

        public bool codigoTipoCreditoEspecifico { get; set; }

        public long numeroLogicoCartao { get; set; }

        public bool numeroLogicoCartaoEspecifico { get; set; }

        public int quantidade { get; set; }

        public bool quantidadeEspecifico { get; set; }

        public decimal valorTotal { get; set; }

        public bool valorTotalEspecifico { get; set; }

    }
}