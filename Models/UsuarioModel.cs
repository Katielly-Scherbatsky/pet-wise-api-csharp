﻿namespace Pet.Wise.Api.Models
{
    public class UsuarioModel : ModelBase
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
