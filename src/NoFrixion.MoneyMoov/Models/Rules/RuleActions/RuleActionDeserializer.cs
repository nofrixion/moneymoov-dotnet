// -----------------------------------------------------------------------------
//  Filename: RuleActionDeserializer.cs
// 
//  Description: Deserializes rule actions and their content:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  23 01 2023  Donal O'Connor   Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace NoFrixion.MoneyMoov.Models;

public interface IRuleActionDeserializer
{
    SweepAction? SweepAction { get; set; }
    SplitPercentageAction? SplitPercentageAction { get; set; }
    GreeterAction? GreeterAction { get; set; }
    void Deserialize(string ruleActionsJson);
}

public class RuleActionDeserializer : IRuleActionDeserializer
{
    private readonly ILogger<RuleActionDeserializer> _logger;

    public SweepAction? SweepAction { get; set; }

    public SplitPercentageAction? SplitPercentageAction { get; set; }

    public GreeterAction? GreeterAction { get; set; }

    public RuleActionDeserializer(ILogger<RuleActionDeserializer> logger)
    {
        _logger = logger;
    }

    public void Deserialize(string ruleActionsJson)
    {
        try
        {
            var ruleActions = JsonConvert.DeserializeObject<List<RuleAction>>(ruleActionsJson);

            if (ruleActions == null)
            {
                return;
            }

            foreach (var ruleAction in ruleActions)
            {
                switch (ruleAction.ActionType)
                {
                    case RuleActionsEnum.Sweep:
                        SweepAction = JsonConvert.DeserializeObject<SweepAction>(ruleAction.ActionContentJson);
                        break;

                    case RuleActionsEnum.Greeter:
                        GreeterAction = JsonConvert.DeserializeObject<GreeterAction>(ruleAction.ActionContentJson);
                        break;

                    case RuleActionsEnum.Split:
                        SplitPercentageAction = JsonConvert.DeserializeObject<SplitPercentageAction>(ruleAction.ActionContentJson);
                        break;

                    default:
                        _logger.LogWarning($"Unsupported rule action type {ruleAction.ActionType} in rules actions json: {ruleActionsJson}");
                        return;
                }
            }
        }
        catch (JsonException ex)
        {
            _logger.LogWarning($"Unable to deserialize the rules actions json: {ruleActionsJson}. {ex}");
        }
    }
}