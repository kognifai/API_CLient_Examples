namespace GaloreAPIDemoV2.Models
{
    public partial class TimeseriesMetadata
    {
        /// <summary>
        /// The aggregate/resampling rate of the data in the dataset.
        /// </summary>
        /// <value>The aggregate/resampling rate of the data in the dataset.</value>
        public enum AggregateEnum
        {
            Raw = 0,
            Seconds1 = 1,
            Seconds5 = 2,
            Seconds10 = 3,
            Minutes1 = 4,
            Minutes5 = 5,
            Minutes10 = 6,
            Hours1 = 7,
            Hours6 = 8,
            Days1 = 9,
            Weeks1 = 10,
            Months1 = 11,
            Months3 = 12,
            Years1 = 13
        }

        /// <summary>
        /// The aggregate/resampling rate of the data in the dataset.
        /// </summary>
        /// <value>The aggregate/resampling rate of the data in the dataset.</value>
        public AggregateEnum? Aggregate { get; set; }

        /// <summary>
        /// The unit specifier for the data.
        /// </summary>
        /// <value>The unit specifier for the data.</value>
        public string? Unit { get; set; }
        public string? Dimension { get; set; }
        public string? QuantityClass { get; set; }

        /// <summary>
        /// Name of the element.
        /// </summary>
        /// <value>Name of the element.</value>
        public string? Name { get; set; }

        /// <summary>
        /// Description of the data set.
        /// </summary>
        /// <value>Description of the data set.</value>
        public string? Description { get; set; }
    }
}