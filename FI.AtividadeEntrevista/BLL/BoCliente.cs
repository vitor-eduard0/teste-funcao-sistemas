using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            using (var transacao = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                try
                {
                    DAL.DaoCliente cli = new DAL.DaoCliente();
                    DAL.Beneficiario.DaoBeneficiario ben = new DAL.Beneficiario.DaoBeneficiario();
                    cliente.Id = cli.Incluir(cliente);
                    foreach (Beneficiario beneficiario in cliente.Beneficiarios)
                    {
                        beneficiario.IdCliente = cliente.Id;
                        ben.Incluir(beneficiario);
                    }
                    transacao.Complete();
                    return cliente.Id;
                }
                catch(Exception ex)
                {
                    transacao.Dispose();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente, List<Beneficiario> beneficiariosRemover)
        {
            using (var transacao = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                try
                {
                    DAL.DaoCliente cli = new DAL.DaoCliente();
                    DAL.Beneficiario.DaoBeneficiario ben = new DAL.Beneficiario.DaoBeneficiario();
                    cli.Alterar(cliente);
                    foreach(Beneficiario beneficiario in cliente.Beneficiarios)
                    {
                        beneficiario.IdCliente = cliente.Id;

                        //Se o ID estiver negativo significa que é um novo registro e deve ser incluído, caso contrário, alterado
                        if (beneficiario.Id < 0)
                            ben.Incluir(beneficiario);
                        else
                            ben.Alterar(beneficiario);
                    }

                    foreach (Beneficiario beneficiario in beneficiariosRemover)
                        ben.Remover(beneficiario.Id);

                    transacao.Complete();
                }
                catch(Exception ex)
                {
                    transacao.Dispose();
                }
            }
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            DML.Cliente cliente = cli.Consultar(id);
            cliente.Beneficiarios = new BoBeneficiario().Pesquisa(cliente.Id);
            return cliente;
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm,  quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF, long? id = null)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.VerificarExistencia(CPF, id);
        }
    }
}
