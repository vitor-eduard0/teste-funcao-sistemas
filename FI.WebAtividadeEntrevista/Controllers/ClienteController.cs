using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using FI.WebAtividadeEntrevista.Utils;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public List<Beneficiario> beneficiariosSession
        {
            get
            {
                if (Session["beneficiarioSession"] == null)
                    Session["beneficiarioSession"] = new List<Beneficiario>();
                return (List<Beneficiario>)Session["beneficiarioSession"];
            }
            set { Session["beneficiarioSession"] = value; }
        }

        public List<Beneficiario> beneficiariosExcluirSession
        {
            get
            {
                if (Session["beneficiariosExcluirSession"] == null)
                    Session["beneficiariosExcluirSession"] = new List<Beneficiario>();
                return (List<Beneficiario>)Session["beneficiariosExcluirSession"];
            }
            set { Session["beneficiariosExcluirSession"] = value; }
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            beneficiariosSession.Clear();
            beneficiariosExcluirSession.Clear();
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                if (Utils.ValidarCPF(model.CPF))
                {
                    if (!bo.VerificarExistencia(model.CPF))
                    {
                        model.Id = bo.Incluir(new Cliente()
                        {
                            CEP = model.CEP,
                            Cidade = model.Cidade,
                            CPF = model.CPF,
                            Email = model.Email,
                            Estado = model.Estado,
                            Logradouro = model.Logradouro,
                            Nacionalidade = model.Nacionalidade,
                            Nome = model.Nome,
                            Sobrenome = model.Sobrenome,
                            Telefone = model.Telefone,
                            Beneficiarios = beneficiariosSession
                        });
                        return Json("Cadastro efetuado com sucesso");
                    }
                    else
                    {
                        return Json($"Já existe um registro com o CPF {model.CPF}.");
                    }
                }
                else
                {
                    return Json("O CPF informado não é válido.");
                }
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
       
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                if (Utils.ValidarCPF(model.CPF))
                {
                    if (!bo.VerificarExistencia(model.CPF, model.Id))
                    {
                        bo.Alterar(new Cliente()
                        {
                            Id = model.Id,
                            CEP = model.CEP,
                            Cidade = model.Cidade,
                            CPF = model.CPF,
                            Email = model.Email,
                            Estado = model.Estado,
                            Logradouro = model.Logradouro,
                            Nacionalidade = model.Nacionalidade,
                            Nome = model.Nome,
                            Sobrenome = model.Sobrenome,
                            Telefone = model.Telefone,
                            Beneficiarios = beneficiariosSession
                        }, beneficiariosExcluirSession);


                        return Json("Cadastro alterado com sucesso");
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        return Json($"Já existe um registro com o CPF {model.CPF}.");
                    }
                }
                else
                {
                    Response.StatusCode = 400;
                    return Json("O CPF informado não é válido.");
                }
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    CPF = cliente.CPF,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    Beneficiarios = cliente.Beneficiarios.Select(s => new BeneficiarioModel()
                    {
                        Id = s.Id,
                        CPF = s.CPF,
                        Nome = s.Nome,
                        IdCliente = s.IdCliente
                    }).ToList()
                };

                beneficiariosSession = cliente.Beneficiarios;
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        
    }
}