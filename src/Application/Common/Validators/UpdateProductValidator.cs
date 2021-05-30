using Application.Feature.Commands.UpdateProduct;
using FluentValidation;

namespace Application.Common.Validators
{
	public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
	{
		public UpdateProductValidator()
		{
			this.RuleFor(m => m.Description).NotEmpty();
			this.RuleFor(m => m.Stock).NotEmpty();
			this.RuleFor(m => m.Price).NotEmpty();
		}
	}
}
