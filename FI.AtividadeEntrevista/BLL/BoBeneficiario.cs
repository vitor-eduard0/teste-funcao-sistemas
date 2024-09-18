using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.Beneficiario.DaoBeneficiario daoBenef = new DAL.Beneficiario.DaoBeneficiario();
            return daoBenef.Incluir(beneficiario);
        }

        /// <summary>
        /// Lista os beneficiários
        /// </summary>
        public List<DML.Beneficiario> Pesquisa(long idCliente)
        {
            DAL.Beneficiario.DaoBeneficiario daoBenef = new DAL.Beneficiario.DaoBeneficiario();
            return daoBenef.Pesquisa(idCliente);
        }
    }
}
