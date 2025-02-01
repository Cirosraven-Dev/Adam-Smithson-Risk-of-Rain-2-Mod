using EntityStates;

namespace AdamMod.SkillStates
{
    public class BaseAdamState : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            RefreshState();
        }

        public void RefreshState()
        {
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
