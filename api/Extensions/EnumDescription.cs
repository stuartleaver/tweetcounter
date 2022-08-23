using System;
using System.ComponentModel;
using System.Linq;

namespace TweetCounter.Api.Extensions;

public static class EnumDescription
{
    public static string GetEnumDescription(this Enum value)
    {
        var fieldInformation = value.GetType().GetField(value.ToString());

        if (fieldInformation == null) return value.ToString();

        if (fieldInformation.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
        {
            return attributes.First().Description;
        }

        return value.ToString();
    }
}