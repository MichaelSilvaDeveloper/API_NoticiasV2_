using System;

namespace API_Noticias_V2_.Models
{
    public class MessageViewModel
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public bool Ativo { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }

        public string UserId { get; set; }
    }
}