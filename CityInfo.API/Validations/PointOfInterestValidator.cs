using CityInfo.API.Models;
using FluentValidation;

namespace CityInfo.API.Validations
{
    public class PointOfInterestValidator: AbstractValidator<PointOfInterestForCreationDto> {
        public PointOfInterestValidator() {
            RuleFor(x => x.Name).Length(0, 20).WithErrorCode("403");
            //Validation rule for the Name and Description properties
            RuleFor(x => new {x.Name, x.Description})
                .Must(x => NotSame(x.Name,x.Description))
                .WithMessage("The provided description should be different from the name.");
        }

        private bool NotSame(string name, string description)
        {
            return name != description;
        }
        
    }
}