using Application.Common.Models;
using Application.Feature.Commands.InsertProduct;
using Application.Feature.Commands.UpdateProduct;
using Application.Feature.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ProductController : Controller
	{
		private readonly IMediator mediator;

		public ProductController(IMediator mediator)
		{
			this.mediator = mediator;
		}

		[HttpPost]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InsertProductResponse))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
		public async Task<IActionResult> Post([FromBody] InsertProductRequest request)
		{
			return Ok(await this.mediator.Send(request));
		}

		[HttpPut("{id}")]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateProductResponse))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
		public async Task<IActionResult> Put(int id, [FromBody] UpdateProductRequest request)
		{
			return Ok(await this.mediator.Send(request.SetId(id)));
		}

		[HttpGet("{id}")]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK ,Type = typeof(GetProductByIdResponse))]
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
		public async Task<IActionResult> Get(int id)
		{
			return Ok(await this.mediator.Send(new GetProductByIdRequest() { ProductId = id }));
		}
	}
}
