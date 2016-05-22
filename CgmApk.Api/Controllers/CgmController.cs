using CgmApk.Api.Business;
using CgmApk.Api.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace CgmApk.Api.Controllers
{
    public class CgmController : ApiController
    {
        bool invalid = false;

        public RetornoPadrao UploadDenuncia()
        {
            try
            {
                var file = HttpContext.Current.Request.Files[0];

                var conteudo = HttpContext.Current.Request.Form["conteudo"];
                var email = HttpContext.Current.Request.Form["email"];
                var autor = HttpContext.Current.Request.Form["nome"];  

                if (!IsValidEmail(email))
                    return new Models.RetornoPadrao { CodigoRetorno = 0, DescricaoRetorno = "Email invalido" };

                string guidNome = string.Empty;
                if (file != null)
                {
                    guidNome = Guid.NewGuid().ToString() + file.FileName;

                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data"), guidNome);
                    file.SaveAs(fileSavePath);
                }

                DenunciaBusiness bu = new DenunciaBusiness();
                bu.SalvarUpload(email, autor, guidNome, conteudo);

                return new Models.RetornoPadrao { CodigoRetorno = 1, DescricaoRetorno = "Denuncia efetuada com sucesso" };

            }
            catch (Exception ex)
            {
                return new RetornoPadrao { CodigoRetorno = 0, DescricaoRetorno = ex.Message };
            }



        }

        [HttpGet]
        public List<DenunciaDto> ListarDenuncias()
        {
            DenunciaBusiness bu = new DenunciaBusiness();
            return bu.Listar();

        }


        [HttpGet]
        [Route("api/cgm/obterdenuncia/{id}")]
        public DenunciaDto ObterDenuncia(int id)
        {
            DenunciaBusiness bu = new DenunciaBusiness();
            return bu.Obter(id);
        }

        [HttpGet]
        [Route("api/cgm/salvarteste/")]
        public RetornoPadrao SalvarTeste()
        {
            string Conteudo = "Temer decide recriar Ministério da Cultura após críticas";
            string Email = "ca.laurin@gmail.com";
            string Autor = "Caio";
            string Caminho = "AppData/imagem.jpg";

            DenunciaBusiness bu = new DenunciaBusiness();
            bu.SalvarUpload(Email, Autor, Caminho, Conteudo);

            return new RetornoPadrao() { CodigoRetorno = 1, DescricaoRetorno = "Sucesso" };


        }

        //utils
        public bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
    }
}
