namespace BluePayLibrary.Interfaces
{
    public interface IBluePayResponseObjectConverter<T>
    {
        void SetValue(T o, string property, string value);

        T Create();
    }
}