using Projeto.Infra.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Infra.Data.Entities
{
    public class Compromisso
    {
        public int IdCompromisso { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public Categoria Categoria { get; set; }

        public Compromisso()
        {
                
        }

        public Compromisso(int idCompromisso, string titulo, string descricao, DateTime dataInicio, DateTime dataFim, TimeSpan horaInicio, TimeSpan horaFim)
        {
            IdCompromisso = idCompromisso;
            Titulo = titulo;
            Descricao = descricao;
            DataInicio = dataInicio;
            DataFim = dataFim;
            HoraInicio = horaInicio;
            HoraFim = horaFim;
        }
    }
}
