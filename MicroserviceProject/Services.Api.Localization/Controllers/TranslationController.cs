using AutoMapper;

using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Localization.Services;
using Services.Api.Localization.Util.Validation.Translation.Translate;
using Services.Communication.Http.Broker.Localization.Models;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Localization.Controllers
{
    [Authorize]
    [Route("Translation")]
    public class TranslationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly TranslationService _translationService;

        public TranslationController(
            IMapper mapper,
            TranslationService translationService)
        {
            _translationService = translationService;
            _mapper = mapper;
        }

        [Route(nameof(GetTranslations))]
        public async Task<IActionResult> GetTranslations(CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<List<TranslationModel>>(async () =>
            {
                List<Infrastructure.Localization.Translation.Models.TranslationModel> translations =
                await _translationService.GetTranslationsAsync(cancellationTokenSource);

                return _mapper.Map<List<Infrastructure.Localization.Translation.Models.TranslationModel>, List<TranslationModel>>(translations);
            },
            services: _translationService);
        }

        [Route(nameof(Translate))]
        [HttpPost]
        public async Task<IActionResult> Translate([FromBody] TranslationModel translation, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<TranslationModel>(async () =>
            {
                await TranslateValidator.ValidateAsync(translation, cancellationTokenSource);

                Infrastructure.Localization.Translation.Models.TranslationModel translationResult =
                await _translationService.TranslateAsync(translation.Key, translation.Region, translation.Parameters, cancellationTokenSource);

                return _mapper.Map<Infrastructure.Localization.Translation.Models.TranslationModel,TranslationModel>(translationResult);
            }, services: _translationService);
        }
    }
}
