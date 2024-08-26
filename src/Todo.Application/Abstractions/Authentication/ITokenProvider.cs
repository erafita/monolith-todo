namespace Todo.Application.Abstractions.Authentication;

public interface ITokenProvider
{
    string Create(User user);
}
