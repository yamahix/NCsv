﻿using System.Globalization;

namespace NCsv.Converters
{
    /// <summary>
    /// <see cref="long"/>のコンバーターです。
    /// </summary>
    internal class LongConverter : CsvConverter
    {
        /// <inheritdoc/>
        public override string ConvertToCsvItem(ConvertToCsvItemContext context)
        {
            if (context.ObjectItem == null)
            {
                return string.Empty;
            }

            var objectItem = (long)context.ObjectItem;
            var format = context.Property.GetCustomAttribute<CsvFormatAttribute>();

            if (format != null)
            {
                return objectItem.ToString(format.Format, CultureInfo.InvariantCulture);
            }

            return objectItem.ToString(CultureInfo.InvariantCulture);
        }

        /// <inheritdoc/>
        public override bool TryConvertToObjectItem(ConvertToObjectItemContext context, out object result, out string errorMessage)
        {
            result = null;
            errorMessage = string.Empty;

            if (HasRequiredError(context.CsvItem))
            {
                errorMessage = CsvMessages.GetRequiredError(context);
                return false;
            }

            if (string.IsNullOrEmpty(context.CsvItem))
            {
                return true;
            }

            if (long.TryParse(context.CsvItem, NumberStyles.Number, CultureInfo.InvariantCulture, out long x))
            {
                result = x;
                return true;
            }

            errorMessage = CsvMessages.GetNumericConvertError(context);
            return false;
        }

        /// <summary>
        /// 必須エラーがあるかどうかを返します。
        /// </summary>
        /// <param name="value">値。</param>
        /// <returns>エラーがある場合にtrue。</returns>
        protected virtual bool HasRequiredError(string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
