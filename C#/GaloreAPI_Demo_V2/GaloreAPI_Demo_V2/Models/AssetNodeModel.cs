using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace GaloreAPIDemoV2.Models
{
    public class AssetNodeModel
    {
        /// <summary>
        /// Id is a globally unique Node id i.e. each node in a tree must have different Id
        /// </summary>
        [Required]
        public string Id { get; set; } = "";

        /// <summary>
        /// Name is a locally unique id i.e. each sibling node must have different Name
        /// </summary>
        [Required]
        public string Name { get; set; } = "";

        /// <summary>
        /// DisplayName is mainly used in ui. May be different from Name
        /// </summary>
        [Required]
        public string DisplayName { get; set; } = "";


        /// <summary>
        /// NodeType is mainly used to define the type of the node, eg. TimeSeries, SampleSet, Calculator
        /// </summary>
        [Required]
        public string NodeType { get; set; } = "";

        /// <summary>
        /// Path defines the direct position of the asset node based on the tree traversal path
        /// </summary>
        [Required]
        public string Path { get; set; } = "";

        /// <summary>
        /// If a node has edges or not. The complete list of edges is not always returned from service        
        /// </summary>
        [Required]
        public bool HasEdges { get; set; }

        /// <summary>
        /// If edges array is empty then either user has not requested for edges, or else asset node has no edges or children
        /// </summary>
        /// <value>If edges array is empty then either user has not requested for edges, or else asset node has no edges or children</value>
        public List<AssetNodeEdgeModel> Edges { get; set; } = new List<AssetNodeEdgeModel>();
        public string TimeseriesId { get; set; } = "";
    }
}
