using FluentValidation;
using InteractiveScheduleUad.Api.Models.Dtos;

namespace InteractiveScheduleUad.Api.Validators
{
    public class RoomForWriteDtoValidator : AbstractValidator<RoomForWriteDto>
    {
        public RoomForWriteDtoValidator()
        {
            RuleFor(room => room.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
