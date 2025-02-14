using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Beneficiario
    /// </summary>
    public class BeneficiarioModel
    {
        public long Id { get; set; }
        /// <summary>
        /// CPF
        /// </summary>
        [Required]
        public string CPF { get; set; }
        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }
        /// <summary>
        /// IdCliente
        /// </summary>
        public long IdCliente { get; set; }
    }
}