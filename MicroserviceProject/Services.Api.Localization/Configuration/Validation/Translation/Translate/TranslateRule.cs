using FluentValidation;

using Services.Communication.Http.Broker.Localization.Models;

namespace Services.Api.Localization.Configuration.Validation.Translation.Translate
{
    public class TranslateRule : AbstractValidator<TranslationModel>
    {
        public TranslateRule()
        {
            RuleFor(x => x.Key).NotEmpty().WithMessage("Çevirilecek anahtar belirtilmemiş");
        }
    }
}
