using Quiz.Data;

namespace Quiz.Api.Services
{
    public interface IQuizService
    {
        Task<QuestionDto?> GetQuestionAsync(int category);
        Task<CheckAnswerDto?> GetCheckAnswerAsync(Guid answerId, int category);
    }
}
