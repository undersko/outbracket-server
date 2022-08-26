using System;
using System.Collections.Generic;
using Amazon;

namespace Outbracket.Common.Constants
{
    public static class AWSConstants
    {
        public static readonly Dictionary<string, RegionEndpoint> RegionsMap = new()
        {
            {
                "af-south-1", RegionEndpoint.AFSouth1 // "Africa (Cape Town)"
            },
            {
                "ap-east-1", RegionEndpoint.APEast1 // "Asia Pacific (Hong Kong)")
            },
            {
                "ap-northeast-1", RegionEndpoint.APNortheast1 // "Asia Pacific (Tokyo)")
            },
            {
                "ap-northeast-2", RegionEndpoint.APNortheast2 // "Asia Pacific (Seoul)")
            },
            {
                "ap-northeast-3", RegionEndpoint.APNortheast3 // "Asia Pacific (Osaka)")
            },
            {
                "ap-south-1", RegionEndpoint.APSouth1 // "Asia Pacific (Mumbai)")
            },
            {
                "ap-southeast-1", RegionEndpoint.APSoutheast1 // "Asia Pacific (Singapore)")
            },

            {
                "ap-southeast-2", RegionEndpoint.APSoutheast2 // "Asia Pacific (Sydney)")
            },
            {
                "ap-southeast-3", RegionEndpoint.APSoutheast3 // "Asia Pacific (Jakarta)")
            },
            {
                "ca-central-1", RegionEndpoint.CACentral1 // "Canada (Central)")
            },
            {
                "eu-central-1", RegionEndpoint.EUCentral1 // "Europe (Frankfurt)")
            },
            {
                "eu-north-1", RegionEndpoint.EUNorth1 // "Europe (Stockholm)")
            },
            {
                "eu-south-1", RegionEndpoint.EUSouth1 // "Europe (Milan)")
            },
            {
                "eu-west-1", RegionEndpoint.EUWest1 // "Europe (Ireland)")
            },
            {
                "eu-west-2", RegionEndpoint.EUWest2 // "Europe (London)")
            },
            {
                "eu-west-3", RegionEndpoint.EUWest3 // "Europe (Paris)")
            },
            {
                "me-south-1", RegionEndpoint.MESouth1 // "Middle East (Bahrain)")
            },
            {
                "sa-east-1", RegionEndpoint.SAEast1 // "South America (Sao Paulo)")
            },
            {
                "us-east-1", RegionEndpoint.USEast1 // "US East (N. Virginia)")
            },
            {
                "us-east-2", RegionEndpoint.USEast2 // "US East (Ohio)")
            },
            {
                "us-west-1", RegionEndpoint.USWest1 // "US West (N. California)")
            },
            {
                "us-west-2", RegionEndpoint.USWest2 // "US West (Oregon)")
            },
            {
                "cn-north-1", RegionEndpoint.CNNorth1 // "China (Beijing)")
            },
            {
                "cn-northwest-1", RegionEndpoint.CNNorthWest1 // "China (Ningxia)")
            },
            {
                "us-gov-east-1", RegionEndpoint.USGovCloudEast1 // "AWS GovCloud (US-East)")
            },
            {
                "us-gov-west-1", RegionEndpoint.USGovCloudWest1 // "AWS GovCloud (US-West)")
            },
            {
                "us-iso-east-1", RegionEndpoint.USIsoEast1 // "US ISO East")
            },
            {
                "us-iso-west-1", RegionEndpoint.USIsoWest1 // "US ISO WEST")
            },
            {
                "us-isob-east-1", RegionEndpoint.USIsobEast1 // "US ISOB East (Ohio)")
            }
        };
    }
}