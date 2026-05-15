using UnityEngine.SceneManagement;
using VirtualEngineer.Enums;
using VirtualEngineer.Services;

namespace VirtualEngineer.Validation
{
    public class ResponseValidator
    {
        public static bool CheckResponseSuccess<T>(ApiResponse<T> response)
        {
            if (!response.isSuccess)
            {
                AppDataService.SelectedSceneId = null;
                
                SceneManager.LoadScene(ConstCode.StartMenuScene);

                return false;
            }

            return true;
        }
    }
}