namespace FFramework
{
    public interface IUpdate : ISendEvent<float>
    {
        void Update(float deltaTime);
    }


    public static partial class LifeCycleEx
    {
        public static void EnableUpdate(this IUpdate update)
        {
            FEvent.Instance.Operator<IUpdate>().Subscribe(update.Update);
        }

        public static void DisableUpdate(this IUpdate update)
        {
            FEvent.Instance.Operator<IUpdate>().UnSubscribe(update.Update);
        }

        public static void SendUpdate(float deltaTime)
        {
            FEvent.Instance.Operator<IUpdate>().Send(deltaTime);
        }
    }
}
