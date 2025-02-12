using Entidades.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validaciones.Validacion
{
    public class ValidadorUsuario : AbstractValidator<UsuarioDTO>
    {

        public ValidadorUsuario()
        {
            RuleFor(x => x.NombreApellido).NotEmpty()
                .WithMessage("NombreApellido es requerido");
            RuleFor(x => x.Email).EmailAddress()
                .WithMessage("Formato de email invalido.");


        }


    }
}
