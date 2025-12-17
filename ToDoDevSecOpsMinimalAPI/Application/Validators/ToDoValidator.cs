namespace ToDoDevSecOpsMinimalAPI.Application.Validators;

public class CreateToDoValidator : AbstractValidator<TodoCreateDTO>
{
    public CreateToDoValidator()
    {
        RuleFor(_ => _.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);
    }
}

public class UpdateToDoValidator : AbstractValidator<TodoUpdateDTO>
{
    public UpdateToDoValidator()
    {
        RuleFor(_ => _.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);
    }
}
