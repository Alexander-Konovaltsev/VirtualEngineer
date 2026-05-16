namespace VirtualEngineer.Enums
{
    public static class Endpoint
    {
        public const string Roles = "/roles";
        public const string UserCreate = "/users/create";
        public const string UserAuthorization = "/users/authorization";
        public const string Scenes = "/scenes";
        public static string ModelsByScene(int sceneId)
        {
            return $"/scenes/{sceneId}/models";
        }
        public static string AllModelsByScene(int sceneId)
        {
            return $"/scenes/{sceneId}/models/all";
        }
        public static string ModelChildren(int modelId)
        {
            return $"/models/{modelId}/children";
        }
        public static string AllViewedModelsByScene(int sceneId)
        {
            return $"/models-views/{sceneId}";
        }
        public const string UserModelViewCreate = "/models-views/create";
        public static string QuizzesBySceneId(int sceneId)
        {
            return $"/quizzes/{sceneId}";
        }
        public const string ResultsByUser = "/results";
        public static string QuestionsByQuizId(int quizId)
        {
            return $"/questions/{quizId}";
        }
        public const string CreateResult = "/results/create";
    }
}