using Asp.Versioning;
using Branef.Empresas.Domain.Models;
using Branef.Empresas.Domain.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Branef.Empresas.API.Controllers.v2
{
    [ApiVersion("2")]
    [ApiController]
    [Route("api/v{version:apiVersion}/companies")]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CompanyController> _logger;
        private readonly IValidator<InsertOrUpdateCompanyCommand> _companyCommandValidator;

        public CompanyController(
            IMediator mediator,
            ILogger<CompanyController> logger,
            IValidator<InsertOrUpdateCompanyCommand> companyModelValidator
        )
        {
            _logger = logger;
            _mediator = mediator;
            _companyCommandValidator = companyModelValidator;
        }

        /// <summary>
        /// Filter companies based on given query criterias.
        /// </summary>
        /// <param name="query"> The criterias used to filter companies (Optional). </param>
        /// <returns> A list of companies and their products. </returns>
        /// <response code="200"> Success - return a list of companies </response>
        /// <response code="500"> Error - return the error that occurs during the process. </response>
        [HttpGet()]
        public async Task<IActionResult> GetAsync([FromQuery] GetCompaniesQuery? query = null)
        {
            try
            {
                if (query is null)
                    query = new GetCompaniesQuery();

                var companies = await _mediator.Send(query);

                _logger.LogInformation("Companies got!");

                return Ok(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when trying to get companies");

                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex);
            }
        }

        /// <summary>
        /// Filter companies based on given query criterias and paginate those results.
        /// </summary>
        /// <param name="page"> The number of page. </param>
        /// <param name="pageSize"> The quantity of items per page </param>
        /// <returns> The total count of records, the page number and size, and the list of companies and their products. </returns>
        /// <response code="200"> Success - return a list of companies </response>
        /// <response code="500"> Error - return the error that occurs during the process. </response>
        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedAsync([FromQuery] GetPaginatedCompaniesQuery? query = null)
        {
            try
            {
                if (query is null)
                    query = new GetPaginatedCompaniesQuery();

                if (query.Page == 0 || query.PageSize == 0)
                    return BadRequest();

                var companies = await _mediator.Send(query);

                _logger.LogInformation("Paginated companies got!");

                return Ok(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when trying to get companies paginated");

                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex);
            }
        }

        /// <summary>
        /// Gets a company by the ID.
        /// </summary>
        /// <param name="id"> The ID of company </param>
        /// <returns> The company. </returns>
        /// <response code="200"> Success - return the company </response>
        /// <response code="400"> BadRequest - The ID is empty. </response>
        /// <response code="500"> Error - return the error that occurs during the process. </response>
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out Guid uuid))
                return BadRequest();

            try
            {
                var company = await _mediator.Send(new GetCompanyByIdQuery(uuid));

                return Ok(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when trying to get company ID {0}", id);

                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex);
            }
        }

        /// <summary>
        /// Inserts a new company.
        /// </summary>
        /// <param name="model"> The information about the company that will be inserted. </param>
        /// <returns> The inserted company. </returns>
        /// <response code="200"> Success - The company was inserted succeded, </response>
        /// <response code="400"> BadRequest - There's no information given or some validation error happened. </response>
        /// <response code="500"> Error - return the error that occurs during the process. </response>
        [HttpPost()]
        public async Task<IActionResult> PostAsync([FromBody] InsertOrUpdateCompanyCommand? model)
        {
            if (model is null)
                return BadRequest();

            var validationResult = _companyCommandValidator.Validate(model);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var company = await _mediator.Send(model);

                _logger.LogInformation("Company ID {0} inserted", company.Id);

                return Ok(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when trying to insert new company");

                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex);
            }
        }

        /// <summary>
        /// Updates a existing company.
        /// </summary>
        /// <param name="id"> The to be updated company ID. </param>
        /// <param name="model"> The information about the company that will be updated. </param>
        /// <returns> The updated company. </returns>
        /// <response code="200"> Success - The company was updated succeded. </response>
        /// <response code="400"> BadRequest - There's no information given or some validation error happened. </response>
        /// <response code="500"> Error - return the error that occurs during the process. </response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(string? id, [FromBody] InsertOrUpdateCompanyCommand? model)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out Guid uuid))
                return BadRequest();

            if (model is null)
                return BadRequest();

            var validationResult = _companyCommandValidator.Validate(model);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                model.Id = uuid;

                var company = _mediator.Send(model);

                _logger.LogInformation("Company ID {0} updated", company.Id);

                return Ok(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when trying to update company ID {0}", id);

                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex);
            }
        }

        /// <summary>
        /// Deletes a company.
        /// </summary>
        /// <param name="id"> The to be deleted company ID. </param>
        /// <returns> The delete company. </returns>
        /// <response code="200"> Success - The company was updated succeded. </response>
        /// <response code="400"> BadRequest - The ID is empty. </response>
        /// <response code="500"> Error - return the error that occurs during the process. </response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string? id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out Guid uuid))
                return BadRequest();

            try
            {
                var success = await _mediator.Send(new DeleteCompanyCommand(uuid));

                _logger.LogInformation("Company ID {0} deleted", id);

                return Ok(success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when trying to delete company ID {0}", id);

                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex);
            }
        }
    }
}
