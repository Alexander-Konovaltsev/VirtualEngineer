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
        public static string ModelChildren(int modelId)
        {
            return $"/models/{modelId}/children";
        }
    }
}