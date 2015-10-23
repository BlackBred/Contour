namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ��������� ��������� �������� ��������������� � ������� ����.
    /// </summary>
    /// <typeparam name="TD">��� ��������� ����.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal interface ISagaContext<TD, TK>
    {
        /// <summary>
        /// ������������� ����.
        /// </summary>
        TK SagaId { get; }

        /// <summary>
        /// ���� <c>true</c> - ����� ���� ���������, ����� - <c>false</c>.
        /// </summary>
        bool Completed { get; }

        /// <summary>
        /// ���������������� ������ ����������� � ����.
        /// </summary>
        TD Data { get; }

        /// <summary>
        /// ��������� ���������������� ������ ����������� � ����.
        /// </summary>
        /// <param name="data">���������������� ������ ����������� � ����.</param>
        void UpdateData(TD data);

        /// <summary>
        /// �������� ���� ��� �����������.
        /// </summary>
        void Complete();
    }
}
