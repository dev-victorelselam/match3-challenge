using Controllers.Game;

namespace Tests.AssetLoader
{
    public class AssetLoaderTest : Match3AssertMonoBehaviour
    {
        public void Awake()
        {
            //prepare
            Setup("[Asset Loader Test");

            //execute
            var result1 = Context.AssetLoader.Load<GameController>($"Controllers/{nameof(GameController)}");
            //assert
            Assert(() => result1 != null).ShouldBe(true).Because("There's a GameController in this path").Run();
            Assert(() => FindObjectOfType<GameController>() != null)
                .ShouldBe(false).Because("We didn't instantiated it").Run();
            
            //execute
            var result2 = Context.AssetLoader.LoadAndInstantiate<GameController>($"Controllers/{nameof(GameController)}");
            //assert
            Assert(() => result2 != null).ShouldBe(true).Because("There's a GameController in this path").Run();
            Assert(() => FindObjectOfType<GameController>() != null)
                .ShouldBe(true).Because("We instantiated it").Run();

            Destroy(FindObjectOfType<GameController>().gameObject);
        }
    }
}
