using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CgmApk.Api.Models
{
    public class DenunciaDto
    {
        public int Id { get; set; }

        public string Conteudo { get; set; }

        public string Autor { get; set; }

        public string EmailAutor { get; set; }

        public string CaminhoAnexo { get; set; }

        public string Data { get; set; }
    }
}