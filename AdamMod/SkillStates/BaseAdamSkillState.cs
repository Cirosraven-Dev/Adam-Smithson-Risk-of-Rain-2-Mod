using EntityStates;
using AdamMod.Content;

namespace AdamMod.SkillStates
{
    public class BaseAdamSkillState : BaseSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            RefreshState();
        }

        public void RefreshState()
        {
        }
    }
}
