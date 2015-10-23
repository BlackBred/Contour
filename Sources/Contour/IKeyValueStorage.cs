namespace Contour
{
    /// <summary>
    /// ��������� ����-��������.
    /// </summary>
    /// <typeparam name="T">��� ������.</typeparam>
    public interface IKeyValueStorage<T> where T : class
    {
        /// <summary>
        /// ���������� �������� �� �����.
        /// </summary>
        /// <param name="key">����, �� �������� �������� ��������..</param>
        /// <returns>�������� ��� <c>null</c>, ���� �� ����� ������ �� �������.</returns>
        T Get(string key);

        /// <summary>
        /// ��������� �������� � ��������� ������.
        /// </summary>
        /// <param name="key">���� ��������.</param>
        /// <param name="value">����������� ��������.</param>
        void Set(string key, T value);
    }
}
