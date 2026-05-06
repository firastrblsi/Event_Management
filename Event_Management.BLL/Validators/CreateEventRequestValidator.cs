using Event_Management.BLL.DTOs.Event;
using FluentValidation;

namespace Event_Management.BLL.Validators
{
    public class CreateEventRequestValidator : AbstractValidator<CreateEventRequest>
    {
        public CreateEventRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Event title is required")
                .Length(3, 200).WithMessage("Title must be between 3 and 200 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .Length(10, 2000).WithMessage("Description must be between 10 and 2000 characters");

            RuleFor(x => x.EventDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Event date must be in the future");

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Valid category is required");

            RuleFor(x => x.LocationId)
                .GreaterThan(0).WithMessage("Valid location is required");
        }
    }
}
