using Context;
using Scripts;

namespace Tests
{
    public class Match3AssertMonoBehaviour : AssertMonoBehaviour
    {
        public IContext Context;
        protected override void Setup(string testName)
        {
            Context = new TestContext();
            base.Setup(testName);
        }
    }
}