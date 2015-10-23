using System;

namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ��������� ������������� ���� � ������� ���������� ������.
    /// ���������� ����� ��������� ������� � �������.
    /// </summary>
    /// <typeparam name="TM">��� ������������� ���������.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal class LambdaSagaSeparator<TM, TK> : ISagaIdSeparator<TM, TK>
        where TM : class
    {
        private readonly Func<Message<TM>, TK> separator;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="LambdaSagaSeparator{TM,TK}"/>. 
        /// </summary>
        /// <param name="separator">��������� ����� ��������� �������������� ���� �� ������ ��������� ���������.</param>
        public LambdaSagaSeparator(Func<Message<TM>, TK> separator)
        {
            this.separator = separator;
        }

        /// <summary>
        /// ��������� ������������� ���� �� ������ ��������� ���������.
        /// </summary>
        /// <param name="message">���������, � ������� ��������� ������������� ����.</param>
        /// <returns>������������� ����. </returns>
        public TK GetId(Message<TM> message)
        {
            return this.separator(message);
        }
    }
}
