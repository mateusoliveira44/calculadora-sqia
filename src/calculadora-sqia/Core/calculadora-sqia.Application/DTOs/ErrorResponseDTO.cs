using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace calculadora_sqia.Application.DTOs
{
    public class ErrorResponseDTO
    {
        public HttpStatusCode StatusCode { get; set; }
        public string MensagemErro { get; set; }
    }
}
