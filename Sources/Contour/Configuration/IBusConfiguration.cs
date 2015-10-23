namespace Contour.Configuration
{
    using System.Collections.Generic;

    using Contour.Filters;
    using Contour.Serialization;

    /// <summary>
    ///   ������������ ������� ����.
    /// </summary>
    public interface IBusConfiguration
    {
        #region Public Properties

        /// <summary>
        ///   ������ ����������� � ���������� (�������).
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Gets the filters.
        /// </summary>
        IEnumerable<IMessageExchangeFilter> Filters { get; }

        /// <summary>
        ///   ���������� ����� ���������.
        /// </summary>
        IMessageLabelHandler MessageLabelHandler { get; }

        /// <summary>
        ///   ������������ ���������.
        /// </summary>
        IPayloadConverter Serializer { get; }

        #endregion
    }
}
