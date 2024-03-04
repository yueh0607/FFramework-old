
using YooAsset.Editor;


/// <summary>
/// 收集全部Material
/// </summary>
[DisplayName("收集材质")]
public class CollectMaterials : IFilterRule
{
    bool IFilterRule.IsCollectAsset(FilterRuleData data)
    {
       
        if(data.AssetPath.EndsWith(".mat"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
