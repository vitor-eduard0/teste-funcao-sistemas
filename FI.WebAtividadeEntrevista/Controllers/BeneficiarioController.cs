using FI.AtividadeEntrevista.BLL;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Web.Mvc;
using WebAtividadeEntrevista.Models;
using FI.AtividadeEntrevista.DML;
using FI.WebAtividadeEntrevista.Utils;

namespace WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
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

        [HttpPost]
        public JsonResult Incluir(BeneficiarioModel model)
        {
            BoBeneficiario bo = new BoBeneficiario();

            if(!this.ModelState.IsValid)
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
                    var timestamp = DateTime.UtcNow.Ticks;
                    var guidPart = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
                    long idDinamico = timestamp ^ guidPart; // Combina o timestamp com uma parte do GUID

                    beneficiariosSession.Add(new Beneficiario()
                    {
                        Id = idDinamico * -1, //atribuo negativo para identificar o que ainda não está salvo na base de dados
                        CPF = model.CPF,
                        Nome = model.Nome,
                        IdCliente = model.IdCliente
                    });
                    return Json("Cadastro efetuado com sucesso");
                }
                else
                {
                    return Json("O CPF informado não é válido.");
                }
            }
        }

        [HttpPost]
        public JsonResult BeneficiarioList()
        {
            try
            {
                return Json(new { Result = "OK", Records = beneficiariosSession });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult Alterar(long id)
        {
            return Json(beneficiariosSession.Find(f => f.Id == id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Alterar(BeneficiarioModel model)
        {
            if(!this.ModelState.IsValid)
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
                    int index = beneficiariosSession.FindIndex(f => f.Id == model.Id);
                    long IdCliente = beneficiariosSession[index].IdCliente;
                    beneficiariosSession[index] = new Beneficiario()
                    {
                        Id = model.Id,
                        CPF = model.CPF,
                        Nome = model.Nome,
                        IdCliente = IdCliente
                    };
                    return Json("Cadastro atualizado com sucesso");
                }
                else
                {
                    return Json("O CPF informado não é válido.");
                }
            }
        }

        [HttpGet]
        public JsonResult Excluir(long id) 
        {
            beneficiariosExcluirSession.Add(beneficiariosSession.Find(f => f.Id == id));
            beneficiariosSession.RemoveAll(r => r.Id ==  id);
            return Json("O beneficiário foi removido.", JsonRequestBehavior.AllowGet);
        }
    }
}