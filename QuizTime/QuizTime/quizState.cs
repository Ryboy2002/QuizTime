using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizTime
{
    class quizState
    {
        private GAMESTATE _gamestate;

        public enum GAMESTATE
        {
            GAME_START,
            SHOW_NEXT_QUESTION,
            SHOW_DIVIDER,
            CHECK_START,
            SHOW_CHECK_QUESTIONS,
            SHOW_CHECK_RIGHTANSWERS,
            QUIZ_END,
        }

        public GAMESTATE gamestate
        {
            get { return _gamestate; }
            set { _gamestate = value; }
        }
    }
}
