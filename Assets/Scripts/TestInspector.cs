using System;
using System.Collections.Generic;
using System.Text;
using EasyToolkit.Core.Reflection;
using EasyToolkit.Inspector.Attributes;
using EasyToolkit.Logging;
using EasyToolkit.Logging.Configuration;
using EasyToolkit.Logging.Core;
using EasyToolkit.OdinSerializer;
using EasyToolkit.TileWorldPro;
using UnityEngine;

[Serializable]
[MetroBoxGroup("asssdnbbb")]
public class TestInner
{
    public int jjss;
    public float ddbba;
    public Vector2 assdaad;
}

public interface TestInterface
{
}

public class TestImplInterface : TestInterface
{
    public int Int1;
    public float Float2;
}

public class TestClass
{
    public TestInterface Interface;
}

[Serializable]
public class TestBase
{
    public int BaseInt;
}

[Serializable]
public class TestDerive1 : TestBase
{
    public float DeriveFloat1;
}

[Serializable]
public class TestDerive2 : TestBase
{
    public float DeriveFloat2;
}

[Serializable]
public class TestInner1
{
    public TestInner2 inner2;
    public TestInner2 inner21;
    public TestInner2 inner22;
    public TestInner2 inner23;
}

[Serializable]
public class TestInner2
{
    public TestInner3 inner3;
    public TestInner3 inner31;
    public TestInner3 inner32;
    public TestInner3 inner33;
}

[Serializable]
public class TestInner3
{
    public TestInner4 inner4;
    public TestInner4 inner41;
    public TestInner4 inner42;
    public TestInner4 inner43;
}

[Serializable]
public class TestInner4
{
    public int int3;
    public float float3;
    public Vector2 vector2;
    public string string1;
    public string string2;
    public string string3;
    public string string4;
}

[EasyInspector]
[ShowEasySerializeFieldsInInspector]
public class TestInspector : MonoBehaviour
{
    [TitleGroup("FG")]
    public int Int1;
    public int Int2;
    [TitleGroup("FG")]
    public int Int3;
    [EndGroup]

    [TitleGroup("LL")]
    public int Int4;
    public int Int5;
    [TitleGroup("FG")]
    public int Int6;
    public int Int7;

    [EndGroup]
    public List<int> List;

//     [OdinSerialize]
//     public TerrainDefinitionSet DefinitionSet;
//
//     public TestInner1 inner1;
//     [LabelText("地形定义表")]
// #if UNITY_EDITOR
//     [ValueDropdown(nameof(TerrainDefinitionItemDropdownList))]
// #endif
//     [MetroListDrawerSettings(ShowIndexLabel = false)]
//     public List<TerrainDefinitionNode> Nodes;


    // Start is called before the first frame update
    void Start()
    {
        Log.Logger = LoggerFactory.Configure()
            .WriteTo.UnityConsole()
            .CreateLogger();

        Log.Info("1234234", sender:this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Button]
    public void Test()
    {
        // for (int i = 0; i < 10; i++)
        // {
        //     var evaluator = ExpressionEvaluatorFactory.CreateEvaluator(
        //         "-t:StaticClassForExpressionEvaluator -p:GetStaticScore()", null);
        //
        //     // Act
        //     var result = evaluator.Evaluate<int>(null);
        // }

        var data = new TestClass();
        data.Interface = new TestImplInterface()
        {
            Int1 = 123,
            Float2 = 1.444f
        };
        var json = Encoding.UTF8.GetString(SerializationUtility.SerializeValue(data, DataFormat.JSON));
        Debug.Log(json);
    }

    private ValueDropdownList<TestBase> GetTestBaseDropdown()
    {
        var total = new ValueDropdownList<TestBase>();
        total.AddDelayed("Derive1", () => new TestDerive1());
        total.AddDelayed("Derive2", () => new TestDerive2());
        return total;
    }

#if UNITY_EDITOR
    private static readonly ValueDropdownList<TerrainDefinitionNode> TerrainDefinitionItemDropdownList = new()
    {
        new DelayedValueDropdownItem("分组", () => new TerrainDefinitionGroup()),
        new DelayedValueDropdownItem("地形", () => new TerrainDefinition()),
        new DelayedValueDropdownItem("复合地形", () => new TerrainDefinition(true)),
    };
#endif
}
