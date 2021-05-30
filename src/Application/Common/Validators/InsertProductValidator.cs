using Application.Feature.Commands.InsertProduct;
using FluentValidation;

namespace Application.Common.Validators
{
	public class InsertProductValidator : AbstractValidator<InsertProductRequest>
	{
		public InsertProductValidator()
		{
			this.RuleFor(m => m.Description).NotEmpty();
			this.RuleFor(m => m.TypeId).NotEmpty();
			this.RuleFor(m => m.StateId).NotEmpty();
			this.RuleFor(m => m.Stock).NotEmpty();
			this.RuleFor(m => m.Price).NotEmpty();
		}
	}
}
