using AdamMod.Modules;
using AdamMod.SkillStates;

namespace AdamMod.Content
{
    public static class States
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(MainState));
        }
    }
}
