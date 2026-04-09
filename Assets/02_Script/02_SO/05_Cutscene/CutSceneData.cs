using UnityEngine;
using System.Collections.Generic;

namespace TheLastHeir.Runtime.Cutscene
{
    [CreateAssetMenu(fileName = "NewCutscene", menuName = "Cutscene/Cutscene Data")]
    public class CutsceneData : ScriptableObject
    {
        [Tooltip("해당 컷신이 끝난 후 원래대로 돌아올지 여부")]
        public bool returnCameraOnEnd = true;
        
        [Tooltip("컷신 종료 후 밝기 되돌리기")]
        public bool returnLightingOnEnd = true;
        
        public List<CutsceneStep> steps = new List<CutsceneStep>();
    }
}