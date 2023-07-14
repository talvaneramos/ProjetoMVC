using Projeto.Infra.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Projeto.Presentation.Mvc.Models
{
    public class CompromissoCadastroModel
    {
        [Required(ErrorMessage = "Por favor, informe o título do compromisso")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "Por favor, informe a descrição do compromisso")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Por favor, informe a data início do compromisso")]
        public string DataInicio { get; set; }
        [Required(ErrorMessage = "Por favor, informe a data fim do compromisso")]
        public string DataFim { get; set; }
        [Required(ErrorMessage = "Por favor, informe a hora início do compromisso")]
        public string HoraInicio { get; set; }
        [Required(ErrorMessage = "Por favor, informe a hora fim do compromisso")]
        public string HoraFim { get; set; }
        [Required(ErrorMessage = "Por favor, selecione a categoria")]
        public Categoria Categoria { get; set; }
    }
}
