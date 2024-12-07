using Microsoft.AspNetCore.Mvc;
using Quiz.Api.Services;

namespace Quiz.Api.Controllers
{
	[ApiController]
	public class QuizController : ControllerBase
	{
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
		{
            _quizService = quizService;
        }

        [HttpGet]
		[Route("getquestion")]
		public async Task<IActionResult> GetQuestion([FromQuery] int category)
		{
            var question = await _quizService.GetQuestionAsync(category);
            return question == null ? BadRequest("B³¹d podczas pobierania pytania") : Ok(question);
		}

        [HttpGet]
        [Route("checkanswer")]
        public async Task<IActionResult> CheckAnswer([FromQuery] Guid answerId, [FromQuery]int category)
        {
            var result = await _quizService.GetCheckAnswerAsync(answerId, category);
            return result == null ? BadRequest("B³¹d podczas sprawdzania odpowiedzi") : Ok(result);
        }
    }
}
