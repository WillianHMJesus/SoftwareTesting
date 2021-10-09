using Features.Core;
using FluentValidation;
using System;

namespace Features.Customers
{
    public class Customer : Entity
    {
        protected Customer()
        { }

        public Customer(Guid id, string name, string surname, DateTime birthDate, string email, DateTime register, bool active)
        {
            Id = id;
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
            Email = email;
            Register = register;
            Active = active;
        }

        public string Name { get; private set; }
        public string Surname { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string Email { get; private set; }
        public DateTime Register { get; private set; }
        public bool Active { get; private set; }

        public string FullName()
        {
            return $"{Name} {Surname}";
        }

        public bool IsSpecial()
        {
            return Register < DateTime.Now.AddYears(-3) && Active;
        }

        public void Inactivate()
        {
            Active = false;
        }

        public override bool IsValid()
        {
            ValidationResult = new CustomerValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CustomerValidation : AbstractValidator<Customer>
    {
        public CustomerValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Por favor, certifique-se de ter inserido o nome")
                .Length(2, 150).WithMessage("O nome deve ter entre 2 e 150 caracteres");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Por favor, certifique-se de ter inserido o sobrenome")
                .Length(2, 150).WithMessage("O sobrenome deve ter entre 2 e 150 caracteres");

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .Must(HaveMinimumAge)
                .WithMessage("O cliente deve ter 18 anos ou mais");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty);
        }

        public static bool HaveMinimumAge(DateTime birthDate)
        {
            return birthDate <= DateTime.Now.AddYears(-18);
        }
    }
}
