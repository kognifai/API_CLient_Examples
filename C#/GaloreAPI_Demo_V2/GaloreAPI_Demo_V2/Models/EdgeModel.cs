using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace GaloreAPIDemoV2.Models
{
    public class AssetNodeEdgeModel
    {
        /// <summary>
        /// Name of the edge. If this edgetype is "Link" then name can be different from the original asset node name. 
        /// </summary>
        [Required]
        public string Name { get; set; } = "";

        /// <summary>
        /// Display Name of the edge is specifically used to show in UI. 
        /// </summary>
        [Required]
        public string DisplayName { get; set; } = "";

        /// <summary>
        /// Node at which the current edge points
        /// </summary>
        [Required]
        public AssetNodeModel Node { get; set; } = null!;

        /// <summary>
        /// Edge Type. Eg. Child, Link
        /// </summary>
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EdgeType Type { get; set; }
    }

    public enum EdgeType
    {
        Parent,
        Child,
        SourceLink,
        TargetLink
    }
}
