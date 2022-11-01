using AutoMapper;
using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GoodAggregatorNews.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class SourceController : Controller
    {
        private readonly ISourceService _sourceService;
        private readonly IMapper _mapper;

        public SourceController(ISourceService sourceService,
            IMapper mapper)
        {
            _sourceService = sourceService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var sources = (await _sourceService.GetSourceAsync())
                                    .Select(dto => _mapper.Map<SourceModel>(dto))
                                    .ToList();

                return View(sources);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation is not successful");
                throw;
            }

        }

        [HttpGet]
        public async Task<IActionResult> AddSource()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSource(CreateSourceModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _mapper.Map<SourceDto>(model);
                    var source = await _sourceService.CreateSourceAsync(dto);

                    return RedirectToAction("Index", "Source");
                }


            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: AddSource is not successful");
                throw;
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> DeleteSource(Guid id)
        {
			try
			{
                if (id != Guid.Empty)
                {
                    var source = (await _sourceService.GetSourceByIdAsync(id));
                    if (source != null)
                    {
                        await _sourceService.DeleteSourceAsync(id);
                        return RedirectToAction("Index", "Source");
                    }
                }
                return BadRequest();

            }
            catch (Exception ex)
			{
                Log.Error(ex, "Operation is not successful");
                throw;
			}
        }

    }
}
